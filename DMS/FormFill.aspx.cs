using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using System.Net;
using System.IO;
using DMS.BAL;
using QuickPDFDLL0813;
using System.Configuration;
using System.Collections;
using DMS.Actions;

using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;

namespace DMS
{
    public partial class FormFill : System.Web.UI.Page
    {
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");
        #region
        /// Define reference of the Webservices
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;
        private RepositoryService repoServiceA;

        public RepositoryService RepoService
        {
            set { repoService = value; }
        }
        public RepositoryService RepoServiceA
        {
            set { repoServiceA = value; }
        }
        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Set the session variables blank which are used to set the previous selected path start
                    Session["SelectedCabUUID"] = "";
                    Session["SelectedDrwUUID"] = "";
                    Session["SelectedFldUUID"] = "";
                    Session["SelectedDocID"] = "";
                    // Set the session variables blank which are used to set the previous selected path end
                    Page.Header.DataBind();
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = true;
                        }
                        
                        // Get the repository service

                        WebServiceFactory wsF = new WebServiceFactory();
                        wsF.UserName = Session["UserID"].ToString();
                        wsF.Ticket = Session["Ticket"].ToString();
                        this.repoService = wsF.getRepositoryService();

                        // Admin Credentials start
                        WebServiceFactory wsFA = new WebServiceFactory();
                        wsFA.UserName = Session["AdmUserID"].ToString();
                        wsFA.Ticket = Session["AdmTicket"].ToString();
                        this.repoServiceA = wsFA.getRepositoryService();
                        // Admin Credentials end

                        // Initialise the reference to the spaces store
                        this.spacesStore = new Alfresco.RepositoryWebService.Store();
                        this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                        this.spacesStore.address = "SpacesStore";
                        /// Authentication required for Alfresco Connectivity End
                        
                        if (Session["hfPageControl"].ToString() == "F") //For Fresh WF
                        {
                            hfPageControl.Value = "F";
                            divRunningWF.Visible = false;
                            divFreshWF.Visible = true;

                            Session["OpenDocName"] = Request.QueryString["docname"].ToString();
                            hfDocument.Value = Request.QueryString["docname"].ToString();
                            Session["SelDocUUID"] = Request.QueryString["DocUUID"].ToString();
                            PopDocProp(Session["SelDocUUID"].ToString());
                            // Fetch Signature & Date Fields Start
                            DataSet ds = FetchSignFlds(Session["SelDocUUID"].ToString());
                            Session["dsSignFlds"] = ds;
                        }
                        else if (Session["hfPageControl"].ToString() == "R") //For Running WF
                        {
                            hfPageControl.Value = "R";
                            divRunningWF.Visible = true;
                            divFreshWF.Visible = false;
                            Session["OpenDocName"] = Request.QueryString["docname"].ToString();                            
                            hfDocument.Value = Request.QueryString["docname"].ToString();
                            Session["SelDocUUID"] = Request.QueryString["DocUUID"].ToString();
                            Session["AttachedDocUUID"] = Request.QueryString["AttachedDocUUID"].ToString();
                            PopForTaskUpdate();
                            divLocal.Visible = false;
                            DisplayApprovals();
                            lblDocNameCheckIn.Text = lblAttachedDocName.Text;
                            DataSet ds = FetchSignFlds(Session["AttachedDocUUID"].ToString());
                            Session["dsSignFlds"] = ds;
                            Session["StageNo"] = lblStage.Text;
                        }
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdCheckInWFL_Click(object sender, EventArgs e)
        {
            try
            {
                if (hfSelCheckIn.Value == "OptExist")
                {
                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    DataSet ds002 = new DataSet();
                    con.Open();
                    cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "'", con);
                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                    adapter002.Fill(ds002);
                    cmd = new SqlCommand("update doc_mast set doc_stat='Check In' where uuid='" + ds002.Tables[0].Rows[0][2].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Utility.CloseConnection(con);
                    throw new Exception("This document has been Checked In");
                }
                else if (hfSelCheckIn.Value == "OptNew")
                {
                    //Response.Redirect("doc_mast.aspx?CIDocUUID=" + lblDocUUID.Text, true);
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void cmdCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet ds001 = new DataSet();
                DataSet ds002 = new DataSet();
                cmd = new SqlCommand("select * from doc_mast where uuid='" + Session["SelDocUUID"].ToString() + "'", con);
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                con.Close();
                if (ds001.Tables[0].Rows[0][18].ToString() == "Check Out")
                {
                    throw new Exception("This document is already Checked Out");
                }
                else
                {
                    con.Open();
                    cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "'", con);
                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                    adapter002.Fill(ds002);
                    cmd = new SqlCommand("update doc_mast set doc_stat='Check Out',CheckedOutBy='" + Session["UserID"].ToString() + "' where uuid='" + ds002.Tables[0].Rows[0][2].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    throw new Exception("This document has been Checked Out");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void DisplayApprovals()
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();

                Int64 WFID = 0;
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataSet ds03 = new DataSet();

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Stage No", typeof(string));
                dt1.Columns.Add("User Name", typeof(string));
                dt1.Columns.Add("Date", typeof(DateTime));
                dt1.Columns.Add("Comment", typeof(string));
                dt1.Columns.Add("Action", typeof(string));

                cmd = new SqlCommand("select a.wf_id,b.doc_name from wf_log_mast a, doc_mast b where a.doc_id=b.doc_id and a.wf_log_id='" + lblWFID.Text + "'", con);
                ds01 = new DataSet();
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);

                if (ds01.Tables[0].Rows.Count > 0)
                {
                    DataSet ds0001 = new DataSet();
                    cmd = new SqlCommand("select doc_name from doc_mast where uuid in(select ActualDocUUID from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "')", con);
                    SqlDataAdapter adapter0001 = new SqlDataAdapter(cmd);
                    adapter0001.Fill(ds0001);
                    lblAttachedDocName.Text = ds0001.Tables[0].Rows[0][0].ToString();
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        WFID = Convert.ToInt64(ds01.Tables[0].Rows[i][0].ToString());
                        cmd = new SqlCommand("select a.StepNo,b.task_name,a.TaskDoneDate,a.Comments,a.UserID from WFLog a,task_mast b where a.TaskID=b.task_id and a.TaskID not like('PRE%') and a.TaskID not like('POST%') and (a.TaskDoneDate!='Not Required' and a.TaskDoneDate!='Waiting') and a.WFLogID='" + lblWFID.Text + "'", con);
                        ds02 = new DataSet();
                        SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                        adapter02.Fill(ds02);

                        if (ds02.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds02.Tables[0].Rows.Count; j++)
                            {
                                DataRow r = dt1.NewRow();
                                r["Stage No"] = ds02.Tables[0].Rows[j][0].ToString();
                                r["Date"] = ds02.Tables[0].Rows[j][2].ToString();
                                r["Comment"] = ds02.Tables[0].Rows[j][3].ToString();
                                
                                DataSet ds0002 = new DataSet();
                                cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo='" + ds02.Tables[0].Rows[j][0].ToString() + "'", con);
                                SqlDataAdapter adapter0002 = new SqlDataAdapter(cmd);
                                adapter0002.Fill(ds0002);
                                if (ds0002.Tables[0].Rows[0][4].ToString() == "Yes")
                                {
                                    r["Action"] = ds02.Tables[0].Rows[j][1].ToString() + " & Uploaded a New Document";
                                }
                                else
                                {
                                    r["Action"] = ds02.Tables[0].Rows[j][1].ToString();
                                }
                                cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id='" + ds02.Tables[0].Rows[j][4].ToString() + "'", con);
                                ds03 = new DataSet();
                                SqlDataAdapter adapter03 = new SqlDataAdapter(cmd);
                                adapter03.Fill(ds03);

                                if (ds03.Tables[0].Rows.Count > 0)
                                {
                                    r["User Name"] = ds03.Tables[0].Rows[0][0].ToString();
                                }
                                dt1.Rows.Add(r);
                            }
                        }
                    }
                }
                Utility.CloseConnection(con);

                gvComment.DataSource = dt1;
                gvComment.DataBind();
                dt1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        private DataSet FetchSignFlds(string DocUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds001 = new DataSet();
            DataSet ds002 = new DataSet();
            cmd = new SqlCommand("select doc_type_id from doc_mast where uuid='" + DocUUID + "'", con);
            SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
            adapter001.Fill(ds001);
            if (ds001.Tables[0].Rows.Count > 0)
            {
                cmd = new SqlCommand("select SignFieldNo1,SignDateFieldNo1,SignFieldNo2,SignDateFieldNo2,SignFieldNo3,SignDateFieldNo3 from doc_type_mast where doc_type_id='" + ds001.Tables[0].Rows[0][0].ToString() + "'", con);
                SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                adapter002.Fill(ds002);
            }
            Utility.CloseConnection(con);
            return ds002;
        }

        protected void cmdProceed_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("doc_mast.aspx?NewFile=" + hfDocument.Value + "&TemplateUUID=" + Session["SelDocUUID"].ToString(), false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        
        private void Redirect2Dashboard(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"userhome.aspx\"</script>";

            Page.Controls.Add(lbl);
        }

        protected void cmdStart_Click(object sender, EventArgs e)
        {
            try
            {
                doc_mast_bal Obj_DocMast = new doc_mast_bal();
                UserRights RightsObj = new UserRights();
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Redirected' where wf_log_id='" + lblWFID.Text + "'", con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("select dept_id,doc_type_id from wf_mast where wf_id='" + ddWFName.SelectedValue + "'", con);
                DataSet ds0001 = new DataSet();
                SqlDataAdapter adapter0001 = new SqlDataAdapter(cmd);
                adapter0001.Fill(ds0001);
                if (ds0001.Tables[0].Rows.Count > 0)
                {
                    Obj_DocMast.DocTypeCode = ds0001.Tables[0].Rows[0][1].ToString();
                    Obj_DocMast.DeptCode = ds0001.Tables[0].Rows[0][0].ToString();
                    string result = Obj_DocMast.DefinedWF();
                    if (Convert.ToInt32(result) == -111) /// If the Workflow is not defined for the Dept & Doc Type Combination; default workflow will be started
                    {
                        
                    }
                    else /// If the Workflow is defined for the Dept & Doc Type Combination
                    {
                        DataSet ds0002 = new DataSet();
                        ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                        ds0002.Reset();
                        ds0002 = ObjClassStoreProc.DocDetails(Request.QueryString["AttachedDocUUID"].ToString());

                        if (ds0002.Tables[0].Rows.Count > 0)
                        {
                            Session["DocId"] = ds0002.Tables[0].Rows[0][0].ToString();
                        }

                        /// Store the defined Wokflow ID into <Session["WFId"]>
                        Session["WFId"] = result;
                        Obj_DocMast.DocID = Convert.ToInt64(Session["DocId"]);
                        Obj_DocMast.WFID = Convert.ToInt64(ddWFName.SelectedValue);
                        Obj_DocMast.Start_Dt = DateTime.Now;
                        Obj_DocMast.UserID = Session["UserID"].ToString();

                        result = Obj_DocMast.StartDefaultWF();
                        if (result != "") /// Start the defined workflow                    
                        {
                            /// Insert the Workflow based roles and due time into the database table name:<wf_log_dtl>
                            /// Store the <wf_log_id> into <Session["WFLogId"]>
                            /// Select the stage wise roles defined in the workflow
                            Session["WFLogId"] = result;
                            Obj_DocMast.WFID = Convert.ToInt64(Session["WFId"]);
                            DataSet ds1 = new DataSet();
                            ds1 = Obj_DocMast.SelectWFDtls();
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                                {
                                    Obj_DocMast.WFLogID = Session["WFLogId"].ToString();
                                    Obj_DocMast.StepNo = Convert.ToInt32(ds1.Tables[0].Rows[i]["step_no"]);
                                    Obj_DocMast.Start_Dt = DateTime.Now;
                                    Obj_DocMast.Duration = ds1.Tables[0].Rows[i]["duration"].ToString();
                                    result = Obj_DocMast.StartDefaultWFLogDtl();
                                    // Insert the Log for versioning of the file Start
                                    if (Request.QueryString["NewFile"] != null)
                                    {
                                        cmd = new SqlCommand("insert into WFDocVersion(WFLogID,StepNo,ActualDocUUID,NewDocUUID,CompCode) values('" + Obj_DocMast.WFLogID + "','" + Obj_DocMast.StepNo + "','" + Session["TemplateUUID"].ToString() + "','','" + Session["CompCode"].ToString() + "')", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Session["WFDocUUId"].ToString() + "' where WFLogID='" + Obj_DocMast.WFLogID + "' and StepNo='1' and ActualDocUUID='" + Session["TemplateUUID"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Invisible the Older versions
                                        DataSet dsV01 = new DataSet();
                                        cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo<'" + lblStage.Text + "' order by StepNo", con);
                                        SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                        adapterV01.Fill(dsV01);
                                        if (dsV01.Tables[0].Rows.Count > 0)
                                        {
                                            for (int kk = 0; kk < dsV01.Tables[0].Rows.Count; kk++)
                                            {
                                                RightsObj.UpdatePermissions4Doc(dsV01.Tables[0].Rows[kk][3].ToString(), "Document", "", "X");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("insert into WFDocVersion(WFLogID,StepNo,ActualDocUUID,NewDocUUID,CompCode) values('" + Obj_DocMast.WFLogID + "','" + Obj_DocMast.StepNo + "','" + Request.QueryString["AttachedDocUUID"].ToString() + "','','" + Session["CompCode"].ToString() + "')", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Request.QueryString["AttachedDocUUID"].ToString() + "' where WFLogID='" + Obj_DocMast.WFLogID + "' and StepNo='1' and ActualDocUUID='" + Request.QueryString["AttachedDocUUID"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Invisible the Older versions
                                        DataSet dsV01 = new DataSet();
                                        cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo<'" + lblStage.Text + "' order by StepNo", con);
                                        SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                        adapterV01.Fill(dsV01);
                                        if (dsV01.Tables[0].Rows.Count > 0)
                                        {
                                            for (int kk = 0; kk < dsV01.Tables[0].Rows.Count; kk++)
                                            {
                                                RightsObj.UpdatePermissions4Doc(dsV01.Tables[0].Rows[kk][3].ToString(), "Document", "", "X");
                                            }
                                        }
                                    }
                                    // Insert the Log for versioning of the file End
                                }
                            }

                            /// Insert the Workflow based roles and tasks into the database table name:<wf_log_task>
                            /// Select the stage wise tasks defined in the workflow
                            Obj_DocMast.WFID = Convert.ToInt64(Session["WFId"]);
                            DataSet ds2 = new DataSet();
                            ds2 = Obj_DocMast.SelectWFTasks();
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                {
                                    Obj_DocMast.WFLogID = Session["WFLogId"].ToString();
                                    Obj_DocMast.StepNo = Convert.ToInt32(ds2.Tables[0].Rows[i]["step_no"]);
                                    Obj_DocMast.TaskID = ds2.Tables[0].Rows[i]["task_id"].ToString();
                                    Obj_DocMast.AmbleMails = ds2.Tables[0].Rows[i]["amble_mails"].ToString();
                                    Obj_DocMast.AmbleMsg = ds2.Tables[0].Rows[i]["amble_msg"].ToString();
                                    Obj_DocMast.AmbleAttach = ds2.Tables[0].Rows[i]["amble_attach"].ToString();
                                    Obj_DocMast.AppendDoc = ds2.Tables[0].Rows[i]["AppendDocUUID"].ToString();
                                    Obj_DocMast.AmbleURL = ds2.Tables[0].Rows[i]["amble_url"].ToString();
                                    Obj_DocMast.AmbleSub = ds2.Tables[0].Rows[i]["AmbleSub"].ToString();
                                    result = Obj_DocMast.StartDefaultWFLogTask();
                                }
                                // Update the Initiator Start
                                DataSet dsI01 = new DataSet();
                                cmd = new SqlCommand("select email from user_mast where user_id='" + Session["UserID"].ToString() + "'", con);
                                SqlDataAdapter adapterI01 = new SqlDataAdapter(cmd);
                                adapterI01.Fill(dsI01);
                                if (dsI01.Tables[0].Rows.Count > 0)
                                {
                                    if (Session["AccessControl"].ToString() == "Outside")
                                    {
                                        cmd = new SqlCommand("update wf_log_task set amble_mails=REPLACE(amble_mails,'guest@guest.com','" + Session["InitiatorEmailID"].ToString() + "') where wf_log_id='" + Session["WFLogId"].ToString() + "'", con);
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("update wf_log_task set amble_mails=REPLACE(amble_mails,'init@init.com','" + dsI01.Tables[0].Rows[0][0].ToString() + "') where wf_log_id='" + Session["WFLogId"].ToString() + "'", con);
                                    }
                                    cmd.ExecuteNonQuery();
                                }
                                // Update the Initiator End
                                CheckAction(Obj_DocMast.WFLogID, 1, DateTime.Now, "", "");
                                if (Session["NewFile"] == null)
                                {

                                }
                                else
                                {
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                }
                                Session["WFTask"] = null;
                                // Update the Doc's Stat in TempDocSaving table
                                if (Session["hfPageControl"] != null)
                                {
                                    if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                    {
                                        cmd = new SqlCommand("update TempDocSaving set TempDocStat='Uploaded',CreationDate='" + DateTime.Now + "' where TempDocName='" + Session["OpenDocName"].ToString() + "' and UserID='" + Session["UserID"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                Session["hfPageControl"] = null;
                                Session["AccessControl"] = null;
                                Session["NewFile"] = null;
                                Session["TemplateUUID"] = null;
                                Redirect2Dashboard("Workflow with tasks has been re-assigned!", "userhome.aspx");
                            }
                            else
                            {
                                // For uploading a doc in a pre selected folder
                                if (Request.QueryString["UF"] != null)
                                {
                                    if (Request.QueryString["UF"].ToString() != "")
                                    {

                                    }
                                }
                                else
                                {
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                }
                                // Update the Doc's Stat in TempDocSaving table
                                if (Session["hfPageControl"] != null)
                                {
                                    if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                    {
                                        cmd = new SqlCommand("update TempDocSaving set TempDocStat='Uploaded',CreationDate='" + DateTime.Now + "' where TempDocName='" + Session["OpenDocName"].ToString() + "' and UserID='" + Session["UserID"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                Redirect2Dashboard("Workflow with tasks has been re-assigned!", "userhome.aspx");
                            }
                        }
                        Redirect2Dashboard("Workflow with tasks has been re-assigned!", "userhome.aspx");
                    }
                    Utility.CloseConnection(con);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void UploadRevisedVersion()
        {
            try
            {
                string CIDocName = "";
                string FolderUUID = "";
                string DocDesc = "";
                string DocName = "";
                DataSet ds0002 = new DataSet();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                ds0002.Reset();
                ds0002 = ObjClassStoreProc.DocDetails(Session["SelDocUUID"].ToString());

                if (ds0002.Tables[0].Rows.Count > 0)
                {
                    CIDocName = ds0002.Tables[0].Rows[0][1].ToString();
                    Session["CIDocTypeID"] = ds0002.Tables[0].Rows[0][2].ToString();
                    Session["CIDeptID"] = ds0002.Tables[0].Rows[0][3].ToString();
                    FolderUUID = ds0002.Tables[0].Rows[0][5].ToString();
                    DocDesc = ds0002.Tables[0].Rows[0][21].ToString();
                    DocName = ObjFetchOnlyNameORExtension.FetchOnlyDocName(CIDocName) + "." + ObjFetchOnlyNameORExtension.FetchOnlyDocExt(CIDocName);
                    Session["OldDocName"] = ObjFetchOnlyNameORExtension.FetchOnlyDocName(CIDocName) + " ARCHIVE " + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "." + ObjFetchOnlyNameORExtension.FetchOnlyDocExt(CIDocName);
                }

                string UDocType = Session["CIDocTypeID"].ToString();
                string UDept = Session["CIDeptID"].ToString();

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();

                // Initialise the reference to the spaces store
                this.spacesStore = new Alfresco.RepositoryWebService.Store();
                this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                this.spacesStore.address = "SpacesStore";
                //create a predicate with the first CMLCreate result
                Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                referenceForNode.store = this.spacesStore;
                referenceForNode.uuid = Session["SelDocUUID"].ToString(); // Selected Doc's UUID

                Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                sourcePredicate.Items = obj_new;

                // Create the properties list
                NamedValue[] properties1 = new NamedValue[2];

                NamedValue nameProperty1 = new NamedValue();
                nameProperty1.name = Constants.PROP_NAME;
                nameProperty1.value = Session["OldDocName"].ToString();
                nameProperty1.isMultiValue = false;
                properties1[0] = nameProperty1;

                nameProperty1 = new NamedValue();
                nameProperty1.name = Constants.PROP_TITLE;
                nameProperty1.value = Session["OldDocName"].ToString();
                nameProperty1.isMultiValue = false;
                properties1[1] = nameProperty1;

                // cml update
                CMLUpdate update = new CMLUpdate();
                update.property = properties1;
                update.where = sourcePredicate;

                CML cmlUpdate = new CML();
                cmlUpdate.update = new CMLUpdate[] { update };

                //perform a CML update to update the node

                WebServiceFactory wsF1 = new WebServiceFactory();
                wsF1.UserName = Session["AdmUserID"].ToString();
                wsF1.Ticket = Session["AdmTicket"].ToString();
                wsF1.getRepositoryService().update(cmlUpdate);
                // Alfresco Part End

                // For Checked In
                cmd = new SqlCommand("update doc_mast set doc_name='" + Session["OldDocName"].ToString() + "' where uuid='" + Session["SelDocUUID"].ToString() + "'", con);
                cmd.ExecuteNonQuery();
                // Need to set "No Permission" for all the users
                UserRights RightsObj = new UserRights();
                cmd = new SqlCommand("update UserRights set Permission='X' where NodeUUID='" + Session["SelDocUUID"].ToString() + "' and UserID!='admin'", con);
                cmd.ExecuteNonQuery();


                string fileName = "";
                /// Checking the Original File Start
                String file = btnBrowseNewVer.FileName;

                if (file == null || file.Equals(""))
                {
                    return;
                }
                int start1 = file.LastIndexOf(".") + 1;
                if (start1 == 0)
                {
                    throw new Exception("Unrecognized document");
                }
                // Checking the Original File End
                //Checking the Alias File Start
                fileName = DocName;
                if (fileName == null || fileName.Equals(""))
                {
                    return;
                }
                int start = fileName.LastIndexOf(".") + 1;
                if (start == 0)
                {
                    throw new Exception("Enter the correct extension of the file!!");
                }
                else
                {

                }
                Session["FileNameExt"] = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(fileName);
                /// Checking the Alias File End
                string FileSize = "";
                double fileSize = Math.Round(Convert.ToDouble(btnBrowseNewVer.PostedFile.ContentLength) / 1024, 2);
                if (fileSize >= 1024)
                {
                    fileSize = Math.Round(fileSize / 1024, 2);
                    FileSize = fileSize.ToString() + " MB";
                }
                else
                {
                    FileSize = fileSize.ToString() + " KB";
                }
                doc_mast_bal Obj_DocMast = new doc_mast_bal();
                Obj_DocMast.DocName = fileName;
                Obj_DocMast.DocDesc = DocDesc;
                Obj_DocMast.DocTypeCode = UDocType;
                Obj_DocMast.DeptCode = UDept;
                Obj_DocMast.FolderCode = FolderUUID;
                Obj_DocMast.Upld_By = Session["UserID"].ToString();
                Obj_DocMast.Upld_Dt = DateTime.Now;
                Obj_DocMast.Tag1 = ds0002.Tables[0].Rows[0][8].ToString();
                Obj_DocMast.Tag2 = ds0002.Tables[0].Rows[0][9].ToString();
                Obj_DocMast.Tag3 = ds0002.Tables[0].Rows[0][10].ToString();
                Obj_DocMast.Tag4 = ds0002.Tables[0].Rows[0][11].ToString();
                Obj_DocMast.Tag5 = ds0002.Tables[0].Rows[0][12].ToString();
                Obj_DocMast.Tag6 = ds0002.Tables[0].Rows[0][13].ToString();
                Obj_DocMast.Tag7 = ds0002.Tables[0].Rows[0][14].ToString();
                Obj_DocMast.Tag8 = ds0002.Tables[0].Rows[0][15].ToString();
                Obj_DocMast.Tag9 = ds0002.Tables[0].Rows[0][16].ToString();
                Obj_DocMast.Tag10 = ds0002.Tables[0].Rows[0][17].ToString();                
                
                string result = Obj_DocMast.ExistDoc();

                if (Convert.ToInt32(result) == -111)
                {
                    throw new Exception("Document already exists in this folder!");
                }
                else
                {
                    DocUpload(FolderUUID, fileName);
                    result = ObjClassStoreProc.InsertDocMast(fileName, DocDesc, FolderUUID, UDocType, UDept, Session["UserID"].ToString(), DateTime.Now, ds0002.Tables[0].Rows[0][8].ToString(), ds0002.Tables[0].Rows[0][9].ToString(), ds0002.Tables[0].Rows[0][10].ToString(), ds0002.Tables[0].Rows[0][11].ToString(), ds0002.Tables[0].Rows[0][12].ToString(), ds0002.Tables[0].Rows[0][13].ToString(), ds0002.Tables[0].Rows[0][14].ToString(), ds0002.Tables[0].Rows[0][15].ToString(), ds0002.Tables[0].Rows[0][16].ToString(), ds0002.Tables[0].Rows[0][17].ToString(), "", Session["NewVUUID"].ToString(), Session["NewVDocPath"].ToString(),"",Session["CompCode"].ToString(),fileSize);

                    DataSet dsPerm = new DataSet();
                    dsPerm.Reset();
                    dsPerm = RightsObj.FetchPermission(FolderUUID, Session["CompCode"].ToString());
                    if (dsPerm.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(Obj_DocMast.UUID, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                        }
                    }

                    InsertMetaTags(Obj_DocMast._UUID, fileName);
                    cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Obj_DocMast._UUID + "',NewUpload='Yes' where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + Obj_DocMast._UUID + "' where WFLogID='" + lblWFID.Text + "' and StepNo > '" + lblStage.Text + "'", con);
                    cmd.ExecuteNonQuery();
                    // Invisible the Older versions start
                    DataSet dsV01 = new DataSet();
                    cmd = new SqlCommand("select * from WFDoc where WFLogID='" + lblWFID.Text + "' and DocUUID='" + Obj_DocMast._UUID + "'", con);
                    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                    adapterV01.Fill(dsV01);
                    if (dsV01.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + lblWFID.Text + "','" + Obj_DocMast._UUID + "','" + Session["CompCode"].ToString() + "')", con);
                        cmd.ExecuteNonQuery();
                        //Invisible the Older versions
                        DataSet dsV001 = new DataSet();
                        cmd = new SqlCommand("select * from WFDoc where WFLogID='" + lblWFID.Text + "' and DocUUID!='" + Obj_DocMast._UUID + "' order by AutoID desc", con);
                        SqlDataAdapter adapterV001 = new SqlDataAdapter(cmd);
                        adapterV001.Fill(dsV001);
                        if (dsV001.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < dsV001.Tables[0].Rows.Count; k++)
                            {
                                RightsObj.UpdatePermissions4Doc(dsV001.Tables[0].Rows[k][2].ToString(), "Document", "", "X");
                            }
                        }
                    }
                    // Invisible the Older versions end
                    DataSet dsDoc01 = new DataSet();
                    cmd = new SqlCommand("select doc_id from doc_mast where uuid='" + Obj_DocMast._UUID + "'", con);
                    SqlDataAdapter adapterDoc01 = new SqlDataAdapter(cmd);
                    adapterDoc01.Fill(dsDoc01);
                    cmd = new SqlCommand("update wf_log_mast set doc_id='" + dsDoc01.Tables[0].Rows[0][0].ToString() + "' where wf_log_id='" + lblWFID.Text + "'", con);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void DocUpload(string FolderUUID, string fileName)
        {
            // Admin Credentials start
            WebServiceFactory wsFA = new WebServiceFactory();
            wsFA.UserName = Session["AdmUserID"].ToString();
            wsFA.Ticket = Session["AdmTicket"].ToString();
            this.repoServiceA = wsFA.getRepositoryService();
            // Admin Credentials end

            doc_mast_bal Obj_DocMast = new doc_mast_bal();
            // Initialise the reference to the spaces store
            Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
            spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
            spacesStore.address = "SpacesStore";

            // Create the parent reference, the company home folder
            Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
            parentReference.store = spacesStore;
            parentReference.uuid = FolderUUID; // Folder's uuid

            parentReference.associationType = Constants.ASSOC_CONTAINS;
            parentReference.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, fileName);

            // Create the properties list
            NamedValue nameProperty = new NamedValue();
            nameProperty.name = Constants.PROP_NAME;
            nameProperty.value = fileName;
            nameProperty.isMultiValue = false;

            NamedValue[] properties = new NamedValue[2];
            properties[0] = nameProperty;
            nameProperty = new NamedValue();
            nameProperty.name = Constants.PROP_TITLE;
            nameProperty.value = fileName;
            nameProperty.isMultiValue = false;
            properties[1] = nameProperty;

            // Create the CML create object
            CMLCreate create = new CMLCreate();
            create.parent = parentReference;
            create.id = "1";
            create.type = Constants.TYPE_CONTENT;
            create.property = properties;

            // Create and execute the cml statement
            CML cml = new CML();
            cml.create = new CMLCreate[] { create };
            UpdateResult[] updateResult = repoServiceA.update(cml);

            // work around to cast Alfresco.RepositoryWebService.Reference to
            // Alfresco.ContentWebService.Reference 
            Alfresco.RepositoryWebService.Reference rwsRef = updateResult[0].destination;
            Alfresco.ContentWebService.Reference newContentNode = new Alfresco.ContentWebService.Reference();
            newContentNode.path = rwsRef.path;
            newContentNode.uuid = rwsRef.uuid;
            Alfresco.ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
            cwsStore.address = "SpacesStore";
            spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
            newContentNode.store = cwsStore;

            // Open the file and convert to byte array 
            Stream inputStream;
            byte[] bytes;
            Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();

            inputStream = btnBrowseNewVer.PostedFile.InputStream;
            int bufferSize = (int)inputStream.Length;
            bytes = new byte[bufferSize];
            inputStream.Read(bytes, 0, bufferSize);
            inputStream.Close();
            FileType ObjFileType = new FileType();
            contentFormat.mimetype = ObjFileType.GetFileType(Session["FileNameExt"].ToString());

            WebServiceFactory wsF = new WebServiceFactory();
            wsF.UserName = Session["AdmUserID"].ToString();
            wsF.Ticket = Session["AdmTicket"].ToString();
            wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);
            /// Alfresco Part End

            Session["NewVUUID"] = newContentNode.uuid;
            Session["NewVDocPath"] = newContentNode.uuid + "/" + fileName.Replace(" ", "%20");
        }

        protected string GenNewDocName(string DocName)
        {
            try
            {
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                string WithoutVerDocName = "";
                string NewDocName = "";
                string DocVersion = "";
                string NewDocVersion = "";
                string OnlyName =ObjFetchOnlyNameORExtension.FetchOnlyDocName(DocName);
                string OnlyExt =ObjFetchOnlyNameORExtension.FetchOnlyDocExt(DocName);
                if (OnlyName.LastIndexOf("_V0") != -1)
                {
                    WithoutVerDocName = OnlyName.Substring(0, OnlyName.Length - 4);
                    DocVersion = OnlyName.Substring(WithoutVerDocName.Length + 2, 2);
                    NewDocVersion = (Convert.ToInt32(DocVersion) + 1).ToString().PadLeft(2, '0');
                    NewDocName = WithoutVerDocName + "_V" + NewDocVersion + "." + OnlyExt;
                }
                else
                {
                    NewDocName = OnlyName + "_V01." + OnlyExt;
                }
                return NewDocName;
            }
            catch (Exception ex)
            {
                return "Error V";
            }
        }

        /// <summary>
        /// Update will update the assigned task along with the document with a new version
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                string TravellingDocName = "";
                byte[] bytes;
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                string UpldFolderUUID = "";
                string DocID = "";
                con.Open();
                DataSet ds001 = new DataSet();
                DataSet ds002 = new DataSet();
                DataSet ds003 = new DataSet();

                cmd = new SqlCommand("select fld_uuid from wf_mast where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + lblWFID.Text + "')", con);
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                UpldFolderUUID = ds001.Tables[0].Rows[0][0].ToString();

                DataSet ds0001 = new DataSet();
                cmd = new SqlCommand("select * from doc_mast where uuid in(select ActualDocUUID from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "')", con);
                SqlDataAdapter adapter0001 = new SqlDataAdapter(cmd);
                adapter0001.Fill(ds0001);
                TravellingDocName = ds0001.Tables[0].Rows[0][1].ToString();
                if (ds0001.Tables[0].Rows[0][18].ToString() == "Check Out")
                {
                    if (btnBrowseNewVer.FileName == null || btnBrowseNewVer.FileName.Equals(""))
                    {
                        throw new Exception("Please Check In with a Document and then Proceed");
                    }
                }

                // If Reject
                if (ddTask.SelectedValue == "REJECT")
                {

                }
                else
                {
                    //New Version Uploading Part Start
                    if (hfSelCheckIn.Value == "OptNew")
                    {
                        UploadRevisedVersion();
                    }
                    //New Version Uploading Part End

                    cmd = new SqlCommand("select wf_id,doc_id from wf_log_mast where wf_log_id='" + lblWFID.Text + "'", con);
                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                    adapter002.Fill(ds002);
                    DocID = ds002.Tables[0].Rows[0][1].ToString();

                    if (hfSelCheckIn.Value != "OptNew")
                    {
                        string fileName = GenNewDocName(TravellingDocName);
                        SqlDataAdapter adapterDocExist;
                        DataSet dsDocExist = new DataSet();
                        cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                        adapterDocExist = new SqlDataAdapter(cmd);
                        dsDocExist.Reset();
                        adapterDocExist.Fill(dsDocExist);
                        if (dsDocExist.Tables[0].Rows.Count > 0)
                        {
                            fileName = GenNewDocName(fileName);
                        }
                        cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                        adapterDocExist = new SqlDataAdapter(cmd);
                        dsDocExist.Reset();
                        adapterDocExist.Fill(dsDocExist);
                        if (dsDocExist.Tables[0].Rows.Count > 0)
                        {
                            fileName = GenNewDocName(fileName);
                        }
                        cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                        adapterDocExist = new SqlDataAdapter(cmd);
                        dsDocExist.Reset();
                        adapterDocExist.Fill(dsDocExist);
                        if (dsDocExist.Tables[0].Rows.Count > 0)
                        {
                            fileName = GenNewDocName(fileName);
                        }
                        cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                        adapterDocExist = new SqlDataAdapter(cmd);
                        dsDocExist.Reset();
                        adapterDocExist.Fill(dsDocExist);
                        if (dsDocExist.Tables[0].Rows.Count > 0)
                        {
                            fileName = GenNewDocName(fileName);
                        }
                        cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                        adapterDocExist = new SqlDataAdapter(cmd);
                        dsDocExist.Reset();
                        adapterDocExist.Fill(dsDocExist);
                        if (dsDocExist.Tables[0].Rows.Count > 0)
                        {
                            fileName = GenNewDocName(fileName);
                        }

                        if (File.Exists(Server.MapPath("TempDownload") + "\\" + fileName))
                        {
                            File.Delete(Server.MapPath("TempDownload") + "\\" + fileName);
                        }

                        File.Copy(Server.MapPath("TempDownload") + "\\" + hfDocument.Value, Server.MapPath("TempDownload") + "\\" + fileName);
                        Session["RWFDocName"] = fileName;
                        //Initialise the reference to the spaces store
                        Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                        spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                        spacesStore.address = "SpacesStore";

                        // Create the parent reference, the company home folder
                        Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
                        parentReference.store = spacesStore;
                        parentReference.uuid = UpldFolderUUID; // Folder's uuid

                        parentReference.associationType = Constants.ASSOC_CONTAINS;
                        parentReference.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, fileName);

                        // Create the properties list
                        NamedValue nameProperty = new NamedValue();
                        nameProperty.name = Constants.PROP_NAME;
                        nameProperty.value = fileName;
                        nameProperty.isMultiValue = false;

                        NamedValue[] properties = new NamedValue[2];
                        properties[0] = nameProperty;
                        nameProperty = new NamedValue();
                        nameProperty.name = Constants.PROP_TITLE;
                        nameProperty.value = fileName;
                        nameProperty.isMultiValue = false;
                        properties[1] = nameProperty;

                        // Create the CML create object
                        CMLCreate create = new CMLCreate();
                        create.parent = parentReference;
                        create.id = "1";
                        create.type = Constants.TYPE_CONTENT;
                        create.property = properties;

                        // Create and execute the cml statement
                        CML cml = new CML();
                        cml.create = new CMLCreate[] { create };
                        // Get the repository service
                        // Admin Credentials start
                        WebServiceFactory wsFA = new WebServiceFactory();
                        wsFA.UserName = Session["AdmUserID"].ToString();
                        wsFA.Ticket = Session["AdmTicket"].ToString();
                        this.repoServiceA = wsFA.getRepositoryService();
                        // Admin Credentials end
                        UpdateResult[] updateResult = repoServiceA.update(cml);

                        // work around to cast Alfresco.RepositoryWebService.Reference to
                        // Alfresco.ContentWebService.Reference 
                        Alfresco.RepositoryWebService.Reference rwsRef = updateResult[0].destination;
                        Alfresco.ContentWebService.Reference newContentNode = new Alfresco.ContentWebService.Reference();
                        newContentNode.path = rwsRef.path;
                        newContentNode.uuid = rwsRef.uuid;
                        Alfresco.ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
                        cwsStore.address = "SpacesStore";
                        spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                        newContentNode.store = cwsStore;

                        bytes = StreamFile(Server.MapPath("TempDownload") + "\\" + Session["RWFDocName"].ToString());
                        Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
                        FileType ObjFileType = new FileType();
                        contentFormat.mimetype = ObjFileType.GetFileType(ObjFetchOnlyNameORExtension.FetchOnlyDocExt(hfDocument.Value));

                        WebServiceFactory wsF = new WebServiceFactory();
                        wsF.UserName = Session["AdmUserID"].ToString();
                        wsF.Ticket = Session["AdmTicket"].ToString();
                        wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);

                        //Upload the document into the DB
                        cmd = new SqlCommand("select * from doc_mast where doc_id='" + DocID + "'", con);
                        SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                        adapter003.Fill(ds003);

                        DataSet ds01 = new DataSet();
                        doc_mast_bal Obj_DocMast = new doc_mast_bal();
                        /// Insert into the database using Store Procedure <DocMast_Insert> start

                        Obj_DocMast.DocName = Session["RWFDocName"].ToString();
                        Obj_DocMast.DocTypeCode = ds003.Tables[0].Rows[0][2].ToString();
                        Obj_DocMast.DeptCode = ds003.Tables[0].Rows[0][3].ToString();
                        Obj_DocMast._UUID = newContentNode.uuid;
                        Obj_DocMast.FolderCode = UpldFolderUUID;
                        Obj_DocMast.Upld_By = Session["UserID"].ToString();
                        Obj_DocMast.Upld_Dt = DateTime.Now;
                        Obj_DocMast.Tag1 = ds003.Tables[0].Rows[0][8].ToString();
                        Obj_DocMast.Tag2 = ds003.Tables[0].Rows[0][9].ToString();
                        Obj_DocMast.Tag3 = ds003.Tables[0].Rows[0][10].ToString();
                        Obj_DocMast.Tag4 = ds003.Tables[0].Rows[0][11].ToString();
                        Obj_DocMast.Tag5 = ds003.Tables[0].Rows[0][12].ToString();
                        Obj_DocMast.Tag6 = ds003.Tables[0].Rows[0][13].ToString();
                        Obj_DocMast.Tag7 = ds003.Tables[0].Rows[0][14].ToString();
                        Obj_DocMast.Tag8 = ds003.Tables[0].Rows[0][15].ToString();
                        Obj_DocMast.Tag9 = ds003.Tables[0].Rows[0][16].ToString();
                        Obj_DocMast.Tag10 = ds003.Tables[0].Rows[0][17].ToString();
                        Obj_DocMast._Doc_Path = newContentNode.uuid + "/" + fileName.Replace(" ", "%20");
                        Obj_DocMast.DocDesc = Session["RWFDocName"].ToString();

                        ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                        string result = ObjClassStoreProc.InsertDocMast(Obj_DocMast.DocName, Obj_DocMast.DocDesc, Obj_DocMast.FolderCode, Obj_DocMast.DocTypeCode, Obj_DocMast.DeptCode, Obj_DocMast.Upld_By, Obj_DocMast.Upld_Dt, Obj_DocMast.Tag1, Obj_DocMast.Tag2, Obj_DocMast.Tag3, Obj_DocMast.Tag4, Obj_DocMast.Tag5, Obj_DocMast.Tag6, Obj_DocMast.Tag7, Obj_DocMast.Tag8, Obj_DocMast.Tag9, Obj_DocMast.Tag10, "", Obj_DocMast._UUID, Obj_DocMast._Doc_Path, "", Session["CompCode"].ToString(), Convert.ToDouble(ds003.Tables[0].Rows[0][24].ToString()));
                        
                        cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + Convert.ToDouble(ds003.Tables[0].Rows[0][24].ToString()) + "' where CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                        #region Validation of space if it is exceeding or not
                        SqlDataAdapter adpAvl01;
                        DataSet dsAvl01 = new DataSet();
                        double AvailableSpace = 0;

                        cmd = new SqlCommand("select TotalSpace,UsedSpace,AvailableSpace from ServerConfig where CompCode='" + Session["CompCode"].ToString() + "'", con);
                        adpAvl01 = new SqlDataAdapter(cmd);
                        dsAvl01.Reset();
                        adpAvl01.Fill(dsAvl01);
                        AvailableSpace = Convert.ToDouble(dsAvl01.Tables[0].Rows[0][2].ToString());
                        if (AvailableSpace < 0)
                        {
                            string SenderMail = "";
                            string SenderName = "";
                            string SmtpHost = "";
                            Int32 SmtpPort = 0;
                            string CredenUsername = "";
                            string CredenPwd = "";
                            string MailSub = "";
                            string MailMsg = "";
                            string MailTo = "";
                            string MailFrom = "";
                            string ContactPersonName = "";
                            string CompName = "";
                            mailing ObjMailSetup = new mailing();
                            ds01.Reset();
                            ds01 = ObjMailSetup.MailSettings();
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                SenderMail = ds01.Tables[0].Rows[0][0].ToString();
                                SenderName = ds01.Tables[0].Rows[0][1].ToString();
                                SmtpHost = ds01.Tables[0].Rows[0][2].ToString();
                                SmtpPort = Convert.ToInt32(ds01.Tables[0].Rows[0][3].ToString());
                                CredenUsername = ds01.Tables[0].Rows[0][4].ToString();
                                CredenPwd = ds01.Tables[0].Rows[0][5].ToString();
                            }
                            ds01.Reset();
                            ds01 = ObjClassStoreProc.SelectServerConfig("00000000");
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                MailFrom = ds01.Tables[0].Rows[0][8].ToString();
                            }
                            ds01.Reset();
                            ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                MailTo = ds01.Tables[0].Rows[0][8].ToString();
                                ContactPersonName = ds01.Tables[0].Rows[0][7].ToString();
                                CompName = ds01.Tables[0].Rows[0][3].ToString();
                            }

                            MailSub = "Alert: Storage Space Exceeded";
                            MailMsg = "Hello " + ContactPersonName + ",<br/><br/>You do not have storage space to upload any further Document.<br/>Please contact your System Administrator to increase the storage space or to free space by deleting old Documents.<br/><br/>Thanks,<br/>System Administrator.";
                            mailing Obj_Mail = new mailing();
                            if (Obj_Mail.SendEmail("", "admin", MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                            {
                                MailMsg = "Hello,<br/>" + CompName + " [" + Session["CompCode"].ToString() + "] has exceeded the using of storage space.<br/><br/>Thanks,<br/>System Administrator.";
                                if (Obj_Mail.SendEmail("", "admin", MailFrom, MailFrom, "", MailFrom, MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                                {

                                }
                            }
                        }
                        #endregion
                        
                        UserRights RightsObj = new UserRights();
                        DataSet dsPerm = new DataSet();
                        dsPerm.Reset();
                        dsPerm = RightsObj.FetchPermission(UpldFolderUUID, Session["CompCode"].ToString());
                        if (dsPerm.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                            {
                                RightsObj.InsertPermissionSingleData(Obj_DocMast.UUID, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                            }
                        }
                        InsertMetaTags(Obj_DocMast._UUID, Session["RWFDocName"].ToString());

                        cmd = new SqlCommand("select top 1 * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and NewDocUUID!='' and StepNo<='" + lblStage.Text + "' order by StepNo desc", con);
                        SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                        ds01.Reset();
                        adapter01.Fill(ds01);
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Obj_DocMast._UUID + "',ActualDocUUID='" + ds01.Tables[0].Rows[0][3].ToString() + "' where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "'", con);
                            cmd.ExecuteNonQuery();
                            cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + Obj_DocMast._UUID + "' where WFLogID='" + lblWFID.Text + "' and StepNo>'" + lblStage.Text + "'", con);
                            cmd.ExecuteNonQuery();
                            DataSet dsV01 = new DataSet();
                            cmd = new SqlCommand("select * from WFDoc where WFLogID='" + lblWFID.Text + "' and DocUUID='" + Obj_DocMast._UUID + "'", con);
                            SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                            adapterV01.Fill(dsV01);
                            if (dsV01.Tables[0].Rows.Count > 0)
                            {

                            }
                            else
                            {
                                cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + lblWFID.Text + "','" + Obj_DocMast._UUID + "','" + Session["CompCode"].ToString() + "')", con);
                                cmd.ExecuteNonQuery();
                                DataSet dsV02 = new DataSet();
                                cmd = new SqlCommand("select doc_id from doc_mast where uuid ='" + Obj_DocMast._UUID + "'", con);
                                SqlDataAdapter adapterV02 = new SqlDataAdapter(cmd);
                                adapterV02.Fill(dsV02);
                                cmd = new SqlCommand("update wf_log_mast set doc_id='" + dsV02.Tables[0].Rows[0][0].ToString() + "' where wf_log_id='" + lblWFID.Text + "'", con);
                                cmd.ExecuteNonQuery();
                                //Invisible the Older versions
                                DataSet dsV001 = new DataSet();
                                cmd = new SqlCommand("select * from WFDoc where WFLogID='" + lblWFID.Text + "' and DocUUID!='" + Obj_DocMast._UUID + "' order by AutoID desc", con);
                                SqlDataAdapter adapterV001 = new SqlDataAdapter(cmd);
                                adapterV001.Fill(dsV001);
                                if (dsV001.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsV001.Tables[0].Rows.Count; k++)
                                    {
                                        RightsObj.UpdatePermissions4Doc(dsV001.Tables[0].Rows[k][2].ToString(), "Document", "", "X");
                                    }
                                }
                            }
                            // Invisible the Older versions end
                        }

                        // Delete the file as the work has been done
                        File.Delete(Server.MapPath("TempDownload") + "\\" + hfDocument.Value);
                        File.Delete(Server.MapPath("TempDownload") + "\\" + Session["RWFDocName"].ToString());
                    }
                }
                userhome_bal UserHomeObj = new userhome_bal();
                /// Update this task in workflow execution using Store Procedure <WFLogTaskDtl_Update> start
                UserHomeObj.WFLogID = lblWFID.Text;
                UserHomeObj.StepNo = Convert.ToInt32(lblStage.Text);
                UserHomeObj.TaskDone_Dt = DateTime.Now;
                UserHomeObj.Task_ID = ddTask.SelectedValue;
                UserHomeObj.Comments = txtComments.Text.Trim();
                CheckAction(UserHomeObj.WFLogID, UserHomeObj.StepNo, UserHomeObj.TaskDone_Dt, UserHomeObj.Task_ID, UserHomeObj.Comments);

                ViewState["WFLogID"] = null;
                ViewState["StepNo"] = null;
                Session["WFTask"] = null;
                Response.Redirect("userhome.aspx", false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void InsertMetaTags(string DocUUID, string FileName)
        {
            try
            {
                string LicenseKey = "";
                string ServerIPAddress = "";
                // Fetch ServerConfig Details Start
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    LicenseKey = ds01.Tables[0].Rows[0][0].ToString();
                    ServerIPAddress = ds01.Tables[0].Rows[0][1].ToString();
                }
                // Fetch ServerConfig Details End

                string SaveLocation = Server.MapPath("TempDownload") + "\\";
                string Full_Path = SaveLocation + FileName;
                int Result = QP.UnlockKey(LicenseKey);
                if (Result == 1)
                {
                    QP.LoadFromFile(Full_Path, "");
                    int TotalFormFields = QP.FormFieldCount();
                    if (TotalFormFields > 0) // For Editable Form
                    {
                        SqlConnection con = Utility.GetConnection();
                        SqlCommand cmd = null;
                        con.Open();
                        DataSet ds0001 = new DataSet();
                        DataSet ds0003 = new DataSet();
                        string DBFldName = "";
                        string FrmValue = "";

                        /// Restrict to max 300 fields
                        if (TotalFormFields > 300)
                        {
                            TotalFormFields = 300;
                        }

                        cmd = new SqlCommand("insert into DocMetaValue(uuid) values('" + DocUUID + "')", con);
                        cmd.ExecuteNonQuery();
                        for (int k = 1; k <= TotalFormFields; k++)
                        {
                            DBFldName = "Tag" + k;
                            FrmValue = QP.GetFormFieldValue(k);

                            cmd = new SqlCommand("update DocMetaValue set " + DBFldName + "='" + FrmValue + "' where uuid='" + DocUUID + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        Utility.CloseConnection(con);
                    }
                }
                else
                {
                    throw new Exception("- Invalid Quick PDF license key -");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopDocProp(string DocUUID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet dsDocProp = new DataSet();
                cmd = new SqlCommand("select a.doc_id,a.doc_name,a.doc_type_id,a.dept_id,a.fld_uuid,a.upld_by,a.upld_dt,a.doc_stat,b.doc_type_name from doc_mast a,doc_type_mast b where a.doc_type_id=b.doc_type_id and a.uuid='" + DocUUID + "'", con);
                SqlDataAdapter adapterDocProp = new SqlDataAdapter(cmd);
                adapterDocProp.Fill(dsDocProp);
                if (dsDocProp.Tables[0].Rows.Count > 0)
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.UserInfoPassingUserID(dsDocProp.Tables[0].Rows[0][5].ToString());
                    lblUpldBy.Text = ds01.Tables[0].Rows[0][1].ToString() + " " + ds01.Tables[0].Rows[0][2].ToString() + " (" + ds01.Tables[0].Rows[0][3].ToString() + ")";

                    lblUpldDt.Text = dsDocProp.Tables[0].Rows[0][6].ToString();
                    lblDocStat.Text = dsDocProp.Tables[0].Rows[0][7].ToString();
                    lblDocType.Text = dsDocProp.Tables[0].Rows[0][8].ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void PopForTaskUpdate()
        {
            try
            {
                lblWFID.Text = Session["WFUpdtWFLogID"].ToString();
                lblWFName.Text = Session["WFUpdtWFName"].ToString();
                lblStage.Text = Session["WFUpdtStepNo"].ToString();
                lblAssignedDt.Text = Session["WFUpdtAssignDt"].ToString();
                lblDueDt.Text = Session["WFUpdtDueDt"].ToString();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.UserInfoPassingUserID(Session["WFUpdtAssignBy"].ToString());
                lblAssignedBy.Text = ds01.Tables[0].Rows[0][1].ToString() + " " + ds01.Tables[0].Rows[0][2].ToString() + " (" + ds01.Tables[0].Rows[0][3].ToString() + ")";

                lblDocName.Text = Session["WFUpdtDocName"].ToString();
                PopulateDropdown();
                TagDisplay(Session["AttachedDocUUID"].ToString());
            }
            catch (Exception ex)
            {

            }
        }

        protected void TagDisplay(string DocUUID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                string SelItem = "";

                con.Open();
                cmd = new SqlCommand("select doc_type_id,tag1,tag2,tag3,tag4,tag5,tag6,tag7,tag8,tag9,tag10 from doc_mast where uuid='" + DocUUID + "'", con);
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);
                Utility.CloseConnection(con);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    SelItem = ds01.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    throw new Exception("No Document Type Assigned for This Document.");
                }

                doc_mast_bal Obj_DocMastBAL = new doc_mast_bal();
                Obj_DocMastBAL.DocTypeCode = SelItem;

                DataSet ds1 = new DataSet();
                ds1 = Obj_DocMastBAL.FetchTags();
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    /// For Tag1
                    if (ds1.Tables[0].Rows[0]["tag1"].ToString() == "" || ds1.Tables[0].Rows[0]["tag1"] == null)
                    {
                        txtTag1.Text = "";
                        divTag1.Visible = false;
                    }
                    else
                    {
                        divTag1.Visible = true;
                        lblTag1.Text = ds1.Tables[0].Rows[0]["tag1"].ToString();
                        txtTag1.Text = ds01.Tables[0].Rows[0][1].ToString();
                    }
                    /// For Tag2
                    if (ds1.Tables[0].Rows[0]["tag2"].ToString() == "" || ds1.Tables[0].Rows[0]["tag2"] == null)
                    {
                        txtTag2.Text = "";
                        divTag2.Visible = false;
                    }
                    else
                    {
                        divTag2.Visible = true;
                        lblTag2.Text = ds1.Tables[0].Rows[0]["tag2"].ToString();
                        txtTag2.Text = ds01.Tables[0].Rows[0][2].ToString();
                    }
                    /// For Tag3
                    if (ds1.Tables[0].Rows[0]["tag3"].ToString() == "" || ds1.Tables[0].Rows[0]["tag3"] == null)
                    {
                        txtTag3.Text = "";
                        divTag3.Visible = false;
                    }
                    else
                    {
                        divTag3.Visible = true;
                        lblTag3.Text = ds1.Tables[0].Rows[0]["tag3"].ToString();
                        txtTag3.Text = ds01.Tables[0].Rows[0][3].ToString();
                    }
                    /// For Tag4
                    if (ds1.Tables[0].Rows[0]["tag4"].ToString() == "" || ds1.Tables[0].Rows[0]["tag4"] == null)
                    {
                        txtTag4.Text = "";
                        divTag4.Visible = false;
                    }
                    else
                    {
                        divTag4.Visible = true;
                        lblTag4.Text = ds1.Tables[0].Rows[0]["tag4"].ToString();
                        txtTag4.Text = ds01.Tables[0].Rows[0][4].ToString();
                    }
                    /// For Tag5
                    if (ds1.Tables[0].Rows[0]["tag5"].ToString() == "" || ds1.Tables[0].Rows[0]["tag5"] == null)
                    {
                        txtTag5.Text = "";
                        divTag5.Visible = false;
                    }
                    else
                    {
                        divTag5.Visible = true;
                        lblTag5.Text = ds1.Tables[0].Rows[0]["tag5"].ToString();
                        txtTag5.Text = ds01.Tables[0].Rows[0][5].ToString();
                    }
                    /// For Tag6
                    if (ds1.Tables[0].Rows[0]["tag6"].ToString() == "" || ds1.Tables[0].Rows[0]["tag6"] == null)
                    {
                        txtTag6.Text = "";
                        divTag6.Visible = false;
                    }
                    else
                    {
                        divTag6.Visible = true;
                        lblTag6.Text = ds1.Tables[0].Rows[0]["tag6"].ToString();
                        txtTag6.Text = ds01.Tables[0].Rows[0][6].ToString();
                    }
                    /// For Tag7
                    if (ds1.Tables[0].Rows[0]["tag7"].ToString() == "" || ds1.Tables[0].Rows[0]["tag7"] == null)
                    {
                        txtTag7.Text = "";
                        divTag7.Visible = false;
                    }
                    else
                    {
                        divTag7.Visible = true;
                        lblTag7.Text = ds1.Tables[0].Rows[0]["tag7"].ToString();
                        txtTag7.Text = ds01.Tables[0].Rows[0][7].ToString();
                    }
                    /// For Tag8
                    if (ds1.Tables[0].Rows[0]["tag8"].ToString() == "" || ds1.Tables[0].Rows[0]["tag8"] == null)
                    {
                        txtTag8.Text = "";
                        divTag8.Visible = false;
                    }
                    else
                    {
                        divTag8.Visible = true;
                        lblTag8.Text = ds1.Tables[0].Rows[0]["tag8"].ToString();
                        txtTag8.Text = ds01.Tables[0].Rows[0][8].ToString();
                    }
                    /// For Tag9
                    if (ds1.Tables[0].Rows[0]["tag9"].ToString() == "" || ds1.Tables[0].Rows[0]["tag9"] == null)
                    {
                        txtTag9.Text = "";
                        divTag9.Visible = false;
                    }
                    else
                    {
                        divTag9.Visible = true;
                        lblTag9.Text = ds1.Tables[0].Rows[0]["tag9"].ToString();
                        txtTag9.Text = ds01.Tables[0].Rows[0][9].ToString();
                    }
                    /// For Tag10
                    if (ds1.Tables[0].Rows[0]["tag10"].ToString() == "" || ds1.Tables[0].Rows[0]["tag10"] == null)
                    {
                        txtTag10.Text = "";
                        divTag10.Visible = false;
                    }
                    else
                    {
                        divTag10.Visible = true;
                        lblTag10.Text = ds1.Tables[0].Rows[0]["tag10"].ToString();
                        txtTag10.Text = ds01.Tables[0].Rows[0][10].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDropdown()
        {
            try
            {
                DBClass DBObj = new DBClass();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();

                //....Task
                DBObj.WF_Log_ID = lblWFID.Text;
                DBObj.Step_No = Convert.ToInt32(lblStage.Text);
                ds01.Reset();
                ds01 = DBObj.DDAssignedTask();
                ddTask.DataSource = ds01;
                ddTask.DataTextField = "task_name";
                ddTask.DataValueField = "task_id";
                ddTask.DataBind();
                for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                {
                    if (ds01.Tables[0].Rows[i][0].ToString() == "REVIEW")
                    {
                        spanCheckOut.Visible = false;
                        spanCheckIn.Visible = false;
                    }
                }

                //....Workflow
                ds01.Reset();
                ds01 = DBObj.DDWorkflow(Session["CompCode"].ToString());
                ddWFName.DataSource = ds01;
                ddWFName.DataTextField = "wf_name";
                ddWFName.DataValueField = "wf_id";
                ddWFName.DataBind();

                //....Cabinet
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinet.DataSource = ds01;
                ddCabinet.DataTextField = "cab_name";
                ddCabinet.DataValueField = "cab_uuid";
                ddCabinet.DataBind();
                PopulateDrawer(ddCabinet.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDrawer(string SelCab)
        {
            try
            {
                //....Drawer
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCab, Session["UserID"].ToString());
                ddDrawer.DataSource = ds01;
                ddDrawer.DataTextField = "drw_name";
                ddDrawer.DataValueField = "drw_uuid";
                ddDrawer.DataBind();
                if (ddDrawer.SelectedValue != "")
                {
                    PopulateFolder(ddDrawer.SelectedValue);
                }
                else
                {
                    PopulateFolder("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateFolder(string SelDrw)
        {
            try
            {
                //....Folder
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrw, Session["UserID"].ToString());
                ddFolder.DataSource = ds01;
                ddFolder.DataTextField = "fld_name";
                ddFolder.DataValueField = "fld_uuid";
                ddFolder.DataBind();
                if (ddFolder.SelectedValue != "")
                {
                    PopulateDocument(ddFolder.SelectedValue);
                }
                else
                {
                    PopulateDocument("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateDocument(string SelFld)
        {
            try
            {
                //....Document
                DBClass DBObj = new DBClass();
                DataSet ds1 = new DataSet();
                DBObj.FldID = SelFld;
                ds1 = DBObj.DDDocument();
                ddDocument.DataSource = ds1;
                ddDocument.DataTextField = "doc_name";
                ddDocument.DataValueField = "uuid";
                ddDocument.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ddCabinet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateDrawer(ddCabinet.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateFolder(ddDrawer.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateDocument(ddFolder.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void optFiling_CheckedChanged(object sender, EventArgs e)
        {
            divFiling.Visible = true;
            divLocal.Visible = false;
        }

        protected void optLocal_CheckedChanged(object sender, EventArgs e)
        {
            divFiling.Visible = false;
            divLocal.Visible = true;
        }

        protected string FetchLocation(string DeptID, string DocTypeID)
        {
            try
            {
                DBClass DBObj = new DBClass();
                DataSet ds01 = new DataSet();
                ds01 = DBObj.FetchUploadedLocation(DeptID, DocTypeID);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    return ds01.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void cmdUpdtMetaData_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] ImageData = new byte[fs.Length];
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return ImageData; //return the byte data
        }

        protected void cmdAppend_Click(object sender, EventArgs e)
        {
            try
            {
                string OutputFile = Server.MapPath("TempDownload") + "\\";
                string mFile = "";
                if (optFiling.Checked == true)
                {
                    if (ddDocument.SelectedValue==null || ddDocument.SelectedValue == "")
                    {
                        throw new Exception("Please Select the Document which you want to Append!!");
                    }
                    else
                    {
                        //Save the Selected Document into the Server's specific folder
                        // At first download the selected file from alfresco and save it to <TempDownload> Folder and then open the file
                        //Download start
                        // Initialise the reference to the spaces store
                        Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                        spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                        spacesStore.address = "SpacesStore";

                        Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                        referenceForNode.store = spacesStore;
                        referenceForNode.uuid = ddDocument.SelectedValue;//Doc UUID

                        Alfresco.ContentWebService.Reference[] obj_new = new Alfresco.ContentWebService.Reference[] { referenceForNode };
                        Alfresco.ContentWebService.Predicate sourcePredicate = new Alfresco.ContentWebService.Predicate();
                        sourcePredicate.Items = obj_new;

                        WebServiceFactory wsFA = new WebServiceFactory();
                        wsFA.UserName = Session["AdmUserID"].ToString();
                        wsFA.Ticket = Session["AdmTicket"].ToString();
                        Alfresco.ContentWebService.Content[] readResult = wsFA.getContentService().read(sourcePredicate, Constants.PROP_CONTENT);

                        String ticketURL = "?ticket=" + wsFA.Ticket;
                        String downloadURL = readResult[0].url + ticketURL;
                        Uri address = new Uri(downloadURL);

                        string url = downloadURL;
                        string newSaveFileName = Guid.NewGuid() + ".pdf";
                        string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
                        SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                        ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                        Session["RWFAppendFileName"] = newSaveFileName;
                    }
                }
                else if (optLocal.Checked == true)
                {                    
                    string fn = System.IO.Path.GetFileName(fuBrowse.PostedFile.FileName);
                    string postfix = System.IO.Path.GetExtension(fn).ToString().ToLower();                    
                    if (postfix == ".pdf")
                    {
                        mFile = Guid.NewGuid() + postfix;
                        Session["RWFAppendFileName"] = mFile;
                        fuBrowse.PostedFile.SaveAs(OutputFile + mFile);
                    }
                    else
                    {
                        throw new Exception("You Can Attach Only pdf Files.");
                    }
                }
                // Now Append the doc
                string FileName1 = hfDocument.Value;
                string FileName2 = mFile;
                string LicenseKey = "";
                string ServerIPAddress = "";
                // Fetch ServerConfig Details Start
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet dsServerConfig = new DataSet();
                dsServerConfig.Reset();
                dsServerConfig = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                if (dsServerConfig.Tables[0].Rows.Count > 0)
                {
                    LicenseKey = dsServerConfig.Tables[0].Rows[0][0].ToString();
                    ServerIPAddress = dsServerConfig.Tables[0].Rows[0][1].ToString();
                }
                // Fetch ServerConfig Details End
                int Result = QP.UnlockKey(LicenseKey);
                if (Result == 1)
                {
                    QP.LoadFromFile(OutputFile + FileName1, "");
                    int PrimaryDoc = QP.SelectedDocument();

                    QP.LoadFromFile(OutputFile + FileName2, "");
                    int SecondaryDoc = QP.SelectedDocument();

                    QP.SelectDocument(PrimaryDoc);
                    QP.MergeDocument(SecondaryDoc);
                    string NewID = Guid.NewGuid().ToString();
                    QP.SaveToFile(OutputFile + NewID + ".pdf");
                    Session["DocNewVersion"] = NewID + ".pdf";
                    hfUpdtDoc.Value = Session["DocNewVersion"].ToString();
                }
                else
                {
                    throw new Exception("Invalid Quick PDF license key");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void CheckAction(string WFLogID, int StepNo, DateTime TaskDoneDate, string TaskID, string Comments)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds001 = new DataSet();
                DataSet ds002 = new DataSet();
                DataSet ds003 = new DataSet();
                DataSet ds004 = new DataSet();
                DataSet ds005 = new DataSet();
                DataSet ds006 = new DataSet();
                con.Open();
                if (Session["AccessControl"].ToString() != "Outside")
                {
                    Session["InitiatorEmailID"] = "";
                }

                // Fetch the tasks for this particular WFLogID & Step No
                cmd = new SqlCommand("select a.* from wf_log_task a,wf_log_mast b where a.wf_log_id=b.wf_log_id and b.wf_prog_stat!='Rejected' and a.wf_log_id='" + WFLogID + "' and a.step_no='" + StepNo + "' and a.task_done_dt is null", con);
                ds001 = new DataSet();
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds001.Tables[0].Rows.Count; i++)
                    {
                        #region For Preamble Mail Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PREEMAIL")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            //Log Update
                            cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            // Preamble Mail
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleEmail ObjPreambleEmail = new PreambleEmail();
                                ObjPreambleEmail.SendPreMail(WFLogID, StepNo, "PREEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                            }
                        }
                        #endregion
                        #region For Preamble Copy Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PRECOPY")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            //Log Update
                            cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleCopy ObjPreambleCopy = new PreambleCopy();
                                string PreCPOutput = ObjPreambleCopy.PreCopy(WFLogID, StepNo, "PRECOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(),Session["UserID"].ToString());
                                CheckAction(WFLogID, StepNo, DateTime.Now, "PRECOPY", PreCPOutput);
                            }
                        }
                        #endregion
                        #region For Preamble Conditinal Mail Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PRECOND")
                        {
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            //Log Update
                            cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            // Preamble Conditional Mail
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleConditionalEmail ObjPreambleConditionalEmail = new PreambleConditionalEmail();
                                ObjPreambleConditionalEmail.SendPreCondMail(WFLogID, StepNo, "PRECOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                            }
                        }
                        #endregion
                        #region For Preamble Append Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PREAPPEND")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            //Log Update
                            cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            // Preamble Append
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleAppend ObjPreambleAppend = new PreambleAppend();
                                Session["RWFOldFile"] = ObjPreambleAppend.PreAppend(WFLogID, StepNo, "PREAPPEND", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                            }
                        }
                        #endregion

                        /// Check is there any Interactive actions or not start
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND' and task_id!='PREAPPEND' and task_id!='POSTAPPEND')", con);
                        ds003 = new DataSet();
                        ds003.Reset();
                        SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                        adapter003.Fill(ds003);
                        if (ds003.Tables[0].Rows.Count > 0)
                        {
                            if (TaskID == "")
                            {

                            }
                            else
                            {
                                if (TaskID == "APPROVE")
                                {
                                    #region
                                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and task_done_dt is null", con);
                                    DataSet dsT001 = new DataSet();
                                    SqlDataAdapter adapterT001 = new SqlDataAdapter(cmd);
                                    dsT001.Reset();
                                    adapterT001.Fill(dsT001);
                                    if (dsT001.Tables[0].Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Not Required' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + TaskID + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    return;
                                    #endregion
                                }
                                else if (TaskID == "REJECT")
                                {
                                    #region
                                    // Need to Mail to the previous stages' users
                                    if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                                    {

                                    }
                                    else
                                    {
                                        RejectEmail ObjRejectEmail = new RejectEmail();
                                        ObjRejectEmail.RejectMail(WFLogID, StepNo, Session["CompCode"].ToString());

                                        // Insert into Rejected List
                                        cmd = new SqlCommand("insert into RejectedList(wf_log_id,step_no,user_id,rejected_dt,comments,CompCode) values('" + WFLogID + "','" + StepNo + "','" + Session["UserID"].ToString() + "','" + TaskDoneDate + "','" + Comments + "','" + Session["CompCode"].ToString() + "')", con);
                                        cmd.ExecuteNonQuery();
                                        // Update wf_log_task & wf_log_dtl for previous stage start
                                        // Current Stage
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Not Started' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskDoneDate='Waiting'", con);
                                        cmd.ExecuteNonQuery();

                                        // Pervious Stage
                                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                        DataSet ds114 = new DataSet();
                                        SqlDataAdapter adapter114 = new SqlDataAdapter(cmd);
                                        ds114.Reset();
                                        adapter114.Fill(ds114);
                                        Session["WFTask"] = "REJECT";
                                        if (ds114.Tables[0].Rows.Count > 0)
                                        {
                                            cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            #region Update the Queued Date & Due Date
                                            cmd = new SqlCommand("select * from wf_dtl where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "') and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                            DataSet ds_0001 = new DataSet();
                                            SqlDataAdapter adapter_0001 = new SqlDataAdapter(cmd);
                                            adapter_0001.Fill(ds_0001);
                                            if (ds_0001.Tables[0].Rows.Count > 0)
                                            {
                                                DateTime Due_Dt2 = DateTime.Now;
                                                Due_Dt2 = Due_Dt2.AddHours(Convert.ToDouble(ds_0001.Tables[0].Rows[0][3].ToString()));
                                                cmd = new SqlCommand("update wf_log_dtl set assign_dt='" + DateTime.Now + "',due_dt='" + Due_Dt2 + "' where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                                cmd.ExecuteNonQuery();
                                            }
                                            #endregion
                                            //Log Update
                                            int MaxStep = 0;
                                            int StartStep = 0;
                                            DataSet dsMaxStep = new DataSet();
                                            cmd = new SqlCommand("select max(StepNo) from WFLog where WFLogID='" + WFLogID + "'", con);
                                            SqlDataAdapter adapterMaxStep = new SqlDataAdapter(cmd);
                                            dsMaxStep.Reset();
                                            adapterMaxStep.Fill(dsMaxStep);
                                            MaxStep = Convert.ToInt32(dsMaxStep.Tables[0].Rows[0][0].ToString());
                                            StartStep = Convert.ToInt32(StepNo) - 1;

                                            for (int kk = StartStep; kk <= MaxStep; kk++)
                                            {
                                                DataSet dsPrev = new DataSet();
                                                cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + kk + "'", con);
                                                SqlDataAdapter adapterPrev = new SqlDataAdapter(cmd);
                                                dsPrev.Reset();
                                                adapterPrev.Fill(dsPrev);
                                                if (kk <= StepNo) // for previous stages
                                                {
                                                    for (int jj = 0; jj < dsPrev.Tables[0].Rows.Count; jj++)
                                                    {
                                                        cmd = new SqlCommand("insert into WFLog(WFLogID,StepNo,UserID,TaskID,TaskDoneDate,Comments,CompCode) values('" + WFLogID + "', '" + kk + "','','" + dsPrev.Tables[0].Rows[jj][2].ToString() + "','Waiting','Waiting','" + Session["CompCode"].ToString() + "')", con);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                else // for later stages
                                                {
                                                    cmd = new SqlCommand("delete from WFLog where WFLogID='" + WFLogID + "' and StepNo='" + kk + "' and TaskDoneDate='Waiting'", con);
                                                    cmd.ExecuteNonQuery();
                                                    for (int jj = 0; jj < dsPrev.Tables[0].Rows.Count; jj++)
                                                    {
                                                        cmd = new SqlCommand("insert into WFLog(WFLogID,StepNo,UserID,TaskID,TaskDoneDate,Comments,CompCode) values('" + WFLogID + "', '" + kk + "','','" + dsPrev.Tables[0].Rows[jj][2].ToString() + "','Waiting','Waiting','" + Session["CompCode"].ToString() + "')", con);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            if (StepNo == 2)
                                            {
                                                cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Rejected' where wf_log_id='" + WFLogID + "'", con);
                                                cmd.ExecuteNonQuery();
                                            }
                                            return;
                                        }
                                        else
                                        {
                                            
                                        }
                                    }
                                    // Update wf_log_task & wf_log_dtl for previous stage end
                                    #endregion
                                }
                                else if (TaskID == "REVIEW")
                                {
                                    #region
                                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and task_done_dt is null", con);
                                    DataSet dsT002 = new DataSet();
                                    SqlDataAdapter adapterT002 = new SqlDataAdapter(cmd);
                                    dsT002.Reset();
                                    adapterT002.Fill(dsT002);
                                    if (dsT002.Tables[0].Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Not Required' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + TaskID + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    return;
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null and (task_id='PRECOPY' or task_id='PREEMAIL' or task_id='PRECOND' or task_id='PREAPPEND')", con);
                            ds004 = new DataSet();
                            SqlDataAdapter adapter004 = new SqlDataAdapter(cmd);
                            ds004.Reset();
                            adapter004.Fill(ds004);
                            if (ds004.Tables[0].Rows.Count > 0)
                            {

                            }
                            else
                            {
                                #region For Postamble Mail Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTEMAIL")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleEmail ObjPostambleEmail = new PostambleEmail();
                                    ObjPostambleEmail.SendPostMail(WFLogID, StepNo, "POSTEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Copy Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTCOPY")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();

                                    PostambleCopy ObjPostambleCopy = new PostambleCopy();
                                    string PostCPOutput = ObjPostambleCopy.PostCopy(WFLogID, StepNo, "POSTCOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "POSTCOPY", PostCPOutput);
                                }
                                #endregion
                                #region For Postamble Conditional Mail Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTCOND")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleConditionalEmail ObjPostambleConditionalEmail = new PostambleConditionalEmail();
                                    ObjPostambleConditionalEmail.SendPostCondMail(WFLogID, StepNo, "POSTCOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Append Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTAPPEND")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();

                                    PostambleAppend ObjPostambleAppend = new PostambleAppend();
                                    Session["RWFOldFile"] = ObjPostambleAppend.PostAppend(WFLogID, StepNo, "POSTAPPEND", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                }
                                #endregion
                            }
                        }
                        /// Check is there any Interactive actions or not end
                    }
                    // Update Status start
                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null", con);
                    ds005 = new DataSet();
                    SqlDataAdapter adapter005 = new SqlDataAdapter(cmd);
                    adapter005.Fill(ds005);
                    if (ds005.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                        cmd.ExecuteNonQuery();
                        // Call this method again
                        DataSet ds100 = new DataSet();
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null  and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND' and task_id!='PREAPPEND' and task_id!='POSTAPPEND')", con);
                        SqlDataAdapter adapter100 = new SqlDataAdapter(cmd);
                        adapter100.Fill(ds100);
                        if (ds100.Tables[0].Rows.Count > 0)
                        {

                        }
                        else
                        {
                            CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                            return;
                        }
                    }
                    else
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Completed' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                        cmd.ExecuteNonQuery();
                        #region Update the Queued Date & Due Date
                        cmd = new SqlCommand("select * from wf_dtl where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "') and step_no='" + (StepNo + 1) + "'", con);
                        DataSet ds_0001 = new DataSet();
                        SqlDataAdapter adapter_0001 = new SqlDataAdapter(cmd);
                        adapter_0001.Fill(ds_0001);
                        if (ds_0001.Tables[0].Rows.Count > 0)
                        {
                            DateTime Due_Dt = DateTime.Now;
                            Due_Dt = Due_Dt.AddHours(Convert.ToDouble(ds_0001.Tables[0].Rows[0][3].ToString()));
                            cmd = new SqlCommand("update wf_log_dtl set assign_dt='" + DateTime.Now + "',due_dt='" + Due_Dt + "' where wf_log_id='" + WFLogID + "' and step_no='" + (StepNo + 1) + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        #endregion
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and task_done_dt is null", con);
                        ds006 = new DataSet();
                        SqlDataAdapter adapter006 = new SqlDataAdapter(cmd);
                        adapter006.Fill(ds006);
                        if (ds006.Tables[0].Rows.Count > 0)
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Ongoing' where wf_log_id='" + WFLogID + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Completed',actual_completed_dt='" + TaskDoneDate + "' where wf_log_id='" + WFLogID + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // Update Status end

                    // Check the stage is completed or not
                    cmd = new SqlCommand("select wf_stat from wf_log_dtl where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                    ds002 = new DataSet();
                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                    adapter002.Fill(ds002);
                    if (ds002.Tables[0].Rows.Count > 0)
                    {
                        if (ds002.Tables[0].Rows[0][0].ToString() == "Completed")
                        {
                            // Call this method again
                            CheckAction(WFLogID, StepNo + 1, DateTime.Now, "", "");
                            return;
                        }
                    }

                }
                else
                {
                    cmd = new SqlCommand("update wf_log_dtl set wf_stat='Completed' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                    cmd.ExecuteNonQuery();
                    #region Update the Queued Date & Due Date
                    cmd = new SqlCommand("select * from wf_dtl where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "') and step_no='" + (StepNo + 1) + "'", con);
                    DataSet ds_0001 = new DataSet();
                    SqlDataAdapter adapter_0001 = new SqlDataAdapter(cmd);
                    adapter_0001.Fill(ds_0001);
                    if (ds_0001.Tables[0].Rows.Count > 0)
                    {
                        DateTime Due_Dt1 = DateTime.Now;
                        Due_Dt1 = Due_Dt1.AddHours(Convert.ToDouble(ds_0001.Tables[0].Rows[0][3].ToString()));
                        cmd = new SqlCommand("update wf_log_dtl set assign_dt='" + DateTime.Now + "',due_dt='" + Due_Dt1 + "' where wf_log_id='" + WFLogID + "' and step_no='" + (StepNo + 1) + "'", con);
                        cmd.ExecuteNonQuery();
                    }
                    #endregion
                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and task_done_dt is null", con);
                    ds006 = new DataSet();
                    SqlDataAdapter adapter006 = new SqlDataAdapter(cmd);
                    adapter006.Fill(ds006);
                    if (ds006.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Ongoing' where wf_log_id='" + WFLogID + "'", con);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Completed',actual_completed_dt='" + TaskDoneDate + "' where wf_log_id='" + WFLogID + "'", con);
                        cmd.ExecuteNonQuery();
                    }
                }
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                //MessageBox(ex.Message);
            }
        }

    }
}