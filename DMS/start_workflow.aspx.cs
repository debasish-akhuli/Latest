using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using System.Net;
using QuickPDFDLL0813;
using System.IO;
using DMS.Actions;

namespace DMS
{
    public partial class start_workflow : System.Web.UI.Page
    {
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
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");

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
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = true;
                        }
                        Session["AccessControl"] = "";
                        lblUser.Text = Session["UserFullName"].ToString();
                        Session["DocId"] = Session["DocId"];
                        /// To populate the dropdown
                        PopulateDropdown();
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
        
        protected void PopulateDropdown()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //....Department
                ds01.Reset();
                ds01 = ObjClassStoreProc.DeptAllBasedOnCompCode(Session["CompCode"].ToString());
                ddDept.DataSource = ds01;
                ddDept.DataTextField = "dept_name";
                ddDept.DataValueField = "dept_id";
                ddDept.DataBind();

                //....Doc Type
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectDocTypeCompBased(Session["CompCode"].ToString());
                ddDocType.DataSource = ds01;
                ddDocType.DataTextField = "doc_type_name";
                ddDocType.DataValueField = "doc_type_id";
                ddDocType.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// This is used to popup a message box using javascript
        /// </summary>
        /// <param name="msg"></param>
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        protected void cmdStartWF_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                string result = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //doc_mast_bal Obj_DocMast = new doc_mast_bal();                
                //Obj_DocMast.DocTypeCode = ddDocType.SelectedValue;
                //Obj_DocMast.DeptCode = ddDept.SelectedValue;
                //result = Obj_DocMast.DefinedWF();
                result = ObjClassStoreProc.DefinedWF(ddDocType.SelectedValue, ddDept.SelectedValue, Session["CompCode"].ToString());
                if (Convert.ToInt32(result) == -111) /// If the Workflow is not defined for the Dept & Doc Type Combination; default workflow will be started
                {
                    MessageBox("No Workflow is defined for this Doc Type & Department combination");
                }
                else /// If the Workflow is defined for the Dept & Doc Type Combination
                {
                    /// Store the defined Wokflow ID into <Session["WFId"]>
                    Session["WFId"] = result;
                    //Obj_DocMast.DocTypeCode = ddDocType.SelectedValue;
                    //Obj_DocMast.DeptCode = ddDept.SelectedValue;
                    //Obj_DocMast.DocID = Convert.ToInt64(Session["DocId"]);
                    //Obj_DocMast.WFID = Convert.ToInt64(Session["WFId"]);
                    //Obj_DocMast.Start_Dt = DateTime.Now;
                    //Obj_DocMast.UserID = Session["UserID"].ToString();

                    //result = Obj_DocMast.StartDefaultWF();
                    result = ObjClassStoreProc.StartDefaultWF(ddDocType.SelectedValue, ddDept.SelectedValue, DateTime.Now, Convert.ToInt16(Session["DocId"]), Convert.ToInt16(Session["WFId"]), Session["UserID"].ToString(), Session["CompCode"].ToString());
                    if (result != "") /// Start the defined workflow                    
                    {
                        /// Insert the Workflow based roles and due time into the database table name:<wf_log_dtl>
                        /// Store the <wf_log_id> into <Session["WFLogId"]>
                        /// Select the stage wise roles defined in the workflow
                        Session["WFLogId"] = result;
                        //Obj_DocMast.WFID = Convert.ToInt64(Session["WFId"]);
                        //DataSet ds1 = new DataSet();
                        //ds1 = Obj_DocMast.SelectWFDtls();
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectWFDtls(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                            {
                                //Obj_DocMast.WFLogID = Session["WFLogId"].ToString();
                                //Obj_DocMast.StepNo = Convert.ToInt32(ds1.Tables[0].Rows[i]["step_no"]);
                                //Obj_DocMast.Start_Dt = DateTime.Now;
                                //Obj_DocMast.Duration = ds1.Tables[0].Rows[i]["duration"].ToString();
                                //result = Obj_DocMast.StartDefaultWFLogDtl();
                                result = ObjClassStoreProc.StartDefaultWFLogDtl(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), DateTime.Now, ds01.Tables[0].Rows[i]["duration"].ToString(), DateTime.Now, Session["CompCode"].ToString());
                                // Insert the Log for versioning of the file Start
                                //con.Open();
                                if (Request.QueryString["NewFile"] != null)
                                {
                                    result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), Session["TemplateUUID"].ToString(), "", Session["CompCode"].ToString());
                                    if (Convert.ToInt32(result) > 0)
                                    {

                                    }
                                    result = ObjClassStoreProc.WFDocVersionUpdate(Session["WFLogId"].ToString(), 1, Session["TemplateUUID"].ToString(), Session["TemplateUUID"].ToString(), Session["CompCode"].ToString());
                                    if (Convert.ToInt32(result) > 0)
                                    {

                                    }
                                    cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + Session["TemplateUUID"].ToString() + "' where WFLogID='" + Session["WFLogId"].ToString() + "' and StepNo>'1'", con);
                                    cmd.ExecuteNonQuery();
                                    DataSet dsV01 = new DataSet();
                                    cmd = new SqlCommand("select * from WFDoc where WFLogID='" + Session["WFLogId"].ToString() + "' and DocUUID='" + Session["TemplateUUID"].ToString() + "'", con);
                                    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                    adapterV01.Fill(dsV01);
                                    if (dsV01.Tables[0].Rows.Count > 0)
                                    {

                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + Session["WFLogId"].ToString() + "','" + Session["TemplateUUID"].ToString() + "','" + Session["CompCode"].ToString() + "')", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    //cmd = new SqlCommand("insert into WFDocVersion(WFLogID,StepNo,ActualDocUUID,NewDocUUID) values('" + Obj_DocMast.WFLogID + "','" + Obj_DocMast.StepNo + "','" + Session["TemplateUUID"].ToString() + "','')", con);
                                    //cmd.ExecuteNonQuery();
                                    //cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Session["WFDocUUId"].ToString() + "' where WFLogID='" + Obj_DocMast.WFLogID + "' and StepNo='1' and ActualDocUUID='" + Session["TemplateUUID"].ToString() + "'", con);
                                    //cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), Session["SelDocUUID"].ToString(), "", Session["CompCode"].ToString());
                                    if (Convert.ToInt32(result) > 0)
                                    {

                                    }
                                    result = ObjClassStoreProc.WFDocVersionUpdate(Session["WFLogId"].ToString(), 1, Session["SelDocUUID"].ToString(), Session["SelDocUUID"].ToString(), Session["CompCode"].ToString());
                                    if (Convert.ToInt32(result) > 0)
                                    {

                                    }
                                    DataSet dsV01 = new DataSet();
                                    cmd = new SqlCommand("select * from WFDoc where WFLogID='" + Session["WFLogId"].ToString() + "' and DocUUID='" + Session["SelDocUUID"].ToString() + "'", con);
                                    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                    adapterV01.Fill(dsV01);
                                    if (dsV01.Tables[0].Rows.Count > 0)
                                    {

                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + Session["WFLogId"].ToString() + "','" + Session["SelDocUUID"].ToString() + "','" + Session["CompCode"].ToString() + "')", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    //cmd = new SqlCommand("insert into WFDocVersion(WFLogID,StepNo,ActualDocUUID,NewDocUUID) values('" + Obj_DocMast.WFLogID + "','" + Obj_DocMast.StepNo + "','" + Session["WFDocUUId"].ToString() + "','')", con);
                                    //cmd.ExecuteNonQuery();
                                    //cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Session["WFDocUUId"].ToString() + "' where WFLogID='" + Obj_DocMast.WFLogID + "' and StepNo='1' and ActualDocUUID='" + Session["WFDocUUId"].ToString() + "'", con);
                                    //cmd.ExecuteNonQuery();
                                }
                                //con.Close();
                                // Insert the Log for versioning of the file End
                            }
                        }

                        /// Insert the Workflow based roles and tasks into the database table name:<wf_log_task>
                        /// Select the stage wise tasks defined in the workflow
                        //Obj_DocMast.WFID = Convert.ToInt64(Session["WFId"]);
                        //DataSet ds2 = new DataSet();
                        //ds2 = Obj_DocMast.SelectWFTasks();
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectWFTasks(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                            {
                                //Obj_DocMast.WFLogID = Session["WFLogId"].ToString();
                                //Obj_DocMast.StepNo = Convert.ToInt32(ds2.Tables[0].Rows[i]["step_no"]);
                                //Obj_DocMast.TaskID = ds2.Tables[0].Rows[i]["task_id"].ToString();
                                //Obj_DocMast.AmbleMails = ds2.Tables[0].Rows[i]["amble_mails"].ToString();
                                //Obj_DocMast.AmbleMsg = ds2.Tables[0].Rows[i]["amble_msg"].ToString();
                                //Obj_DocMast.AmbleAttach = ds2.Tables[0].Rows[i]["amble_attach"].ToString();
                                //Obj_DocMast.AppendDoc = ds2.Tables[0].Rows[i]["AppendDocUUID"].ToString();
                                //Obj_DocMast.AmbleURL = ds2.Tables[0].Rows[i]["amble_url"].ToString();
                                //Obj_DocMast.AmbleSub = ds2.Tables[0].Rows[i]["AmbleSub"].ToString();
                                //result = Obj_DocMast.StartDefaultWFLogTask();
                                result = ObjClassStoreProc.StartDefaultWFLogTask(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), ds01.Tables[0].Rows[i]["task_id"].ToString(), ds01.Tables[0].Rows[i]["amble_mails"].ToString(), ds01.Tables[0].Rows[i]["amble_msg"].ToString(), ds01.Tables[0].Rows[i]["amble_attach"].ToString(), ds01.Tables[0].Rows[i]["AppendDocUUID"].ToString(), ds01.Tables[0].Rows[i]["amble_url"].ToString(), ds01.Tables[0].Rows[i]["AmbleSub"].ToString(), Session["CompCode"].ToString());
                            }
                            // Update the Initiator Start
                            DataSet dsI01 = new DataSet();
                            //con.Open();
                            cmd = new SqlCommand("select email from user_mast where user_id='" + Session["UserID"].ToString() + "'", con);
                            SqlDataAdapter adapterI01 = new SqlDataAdapter(cmd);
                            adapterI01.Fill(dsI01);
                            //con.Close();
                            if (dsI01.Tables[0].Rows.Count > 0)
                            {
                                //con.Open();
                                if (Session["AccessControl"].ToString() == "Outside")
                                {
                                    cmd = new SqlCommand("update wf_log_task set amble_mails=REPLACE(amble_mails,'guest@guest.com','" + Session["InitiatorEmailID"].ToString() + "') where wf_log_id='" + Session["WFLogId"].ToString() + "'", con);
                                }
                                else
                                {
                                    cmd = new SqlCommand("update wf_log_task set amble_mails=REPLACE(amble_mails,'init@init.com','" + dsI01.Tables[0].Rows[0][0].ToString() + "') where wf_log_id='" + Session["WFLogId"].ToString() + "'", con);
                                }
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            // Update the Initiator End
                            CheckAction(Session["WFLogId"].ToString(), 1, DateTime.Now, "", "");
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
                                    //con.Open();
                                    cmd = new SqlCommand("update TempDocSaving set TempDocStat='Uploaded',CreationDate='" + DateTime.Now + "' where TempDocName='" + Session["OpenDocName"].ToString() + "' and UserID='" + Session["UserID"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                }
                            }
                            Session["hfPageControl"] = null;
                            Session["AccessControl"] = null;
                            Session["NewFile"] = null;
                            Session["TemplateUUID"] = null;
                            Redirect2Home("Workflow with tasks has been started!", "home.aspx");
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
                                    //con.Open();
                                    cmd = new SqlCommand("update TempDocSaving set TempDocStat='Uploaded',CreationDate='" + DateTime.Now + "' where TempDocName='" + Session["OpenDocName"].ToString() + "' and UserID='" + Session["UserID"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                }
                            }
                            Redirect2Home("Workflow with tasks has been started!", "home.aspx");
                            PopulateDropdown();
                        }
                    }
                    Redirect2Home("Workflow with tasks has been started!", "home.aspx");
                }
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// TaskID="" for the preamble & postamble actions but will be the task code when any interactive action will be performed
        /// </summary>
        /// <param name="WFLogID"></param>
        /// <param name="StepNo"></param>
        /// <param name="TaskDoneDate"></param>
        /// <param name="TaskID"></param>
        /// <param name="Comments"></param>
        /// <returns></returns>
        protected void CheckAction(string WFLogID, int StepNo, DateTime TaskDoneDate, string TaskID, string Comments)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                DataSet ds003 = new DataSet();
                DataSet dsT001 = new DataSet();

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds002 = new DataSet();
                DataSet ds004 = new DataSet();
                DataSet ds005 = new DataSet();
                DataSet ds006 = new DataSet();
                con.Open();
                if (Session["AccessControl"].ToString() != "Outside")
                {
                    Session["InitiatorEmailID"] = "";
                }

                ds01.Reset();
                // Fetch the tasks for this particulat WFLogID & Step No
                ds01 = ObjClassStoreProc.WFLogTaskSelect(1, WFLogID, Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        #region For Preamble Email Start
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PREEMAIL")
                        {
                            // Update status in database
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), "", Session["CompCode"].ToString());
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
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PRECOPY")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), "", Session["CompCode"].ToString());
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleCopy ObjPreambleCopy = new PreambleCopy();
                                string PreCPOutput = ObjPreambleCopy.PreCopy(WFLogID, StepNo, "PRECOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                CheckAction(WFLogID, StepNo, DateTime.Now, "PRECOPY", PreCPOutput);
                            }
                        }
                        #endregion
                        #region For Preamble Conditional Email Start
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PRECOND")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), "", Session["CompCode"].ToString());
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
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PREAPPEND")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), "", Session["CompCode"].ToString());
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
                        //cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND' and task_id!='PREAPPEND' and task_id!='POSTAPPEND' and task_id!='POSTSOFTCP')", con);
                        ds003 = new DataSet();
                        ds003.Reset();
                        //SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                        //adapter003.Fill(ds003);
                        ds003 = ObjClassStoreProc.WFLogTaskSelect(2, WFLogID, Convert.ToInt16(StepNo), Session["CompCode"].ToString());
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
                                    //cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and task_done_dt is null", con);
                                    //DataSet dsT001 = new DataSet();
                                    //SqlDataAdapter adapterT001 = new SqlDataAdapter(cmd);
                                    dsT001.Reset();
                                    //adapterT001.Fill(dsT001);
                                    dsT001 = ObjClassStoreProc.WFLogTaskSelect(3, WFLogID, StepNo, Session["CompCode"].ToString());
                                    if (dsT001.Tables[0].Rows.Count > 0)
                                    {
                                        result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, Comments, TaskID, StepNo, Session["CompCode"].ToString());
                                        if (Convert.ToInt32(result) > 0)
                                        {

                                        }
                                        result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Not Required", "REJECT", StepNo, Session["CompCode"].ToString());
                                        if (Convert.ToInt32(result) > 0)
                                        {

                                        }
                                        //Log Update
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, TaskDoneDate.ToString(), Comments, TaskID, StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        if (Convert.ToInt32(result) > 0)
                                        {

                                        }
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, "Not Required", "Not Required", "REJECT", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        if (Convert.ToInt32(result) > 0)
                                        {

                                        }
                                        //cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                                        //cmd.ExecuteNonQuery();
                                        //cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Not Required' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT'", con);
                                        //cmd.ExecuteNonQuery();
                                        //Log Update
                                        //cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + TaskID + "'", con);
                                        //cmd.ExecuteNonQuery();
                                        //cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT'", con);
                                        //cmd.ExecuteNonQuery();
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
                                        result = ObjClassStoreProc.WFLogTaskUpdate(3, WFLogID, "", "", DateTime.Now, null, "", StepNo, Session["CompCode"].ToString());
                                        if (Convert.ToInt32(result) > 0)
                                        {

                                        }
                                        cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Not Started' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, TaskDoneDate.ToString(), Comments, "REJECT", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        if (Convert.ToInt32(result) > 0)
                                        {

                                        }
                                        result = ObjClassStoreProc.WFLogUpdate(3, WFLogID, "Not Required", "Not Required", "", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        if (Convert.ToInt32(result) > 0)
                                        {

                                        }
                                        //cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                                        //cmd.ExecuteNonQuery();
                                        //cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Not Started' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                                        //cmd.ExecuteNonQuery();
                                        //Log Update
                                        //cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT'", con);
                                        //cmd.ExecuteNonQuery();
                                        //cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "'", con);
                                        //cmd.ExecuteNonQuery();

                                        // Pervious Stage
                                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        DataSet ds114 = new DataSet();
                                        SqlDataAdapter adapter114 = new SqlDataAdapter(cmd);
                                        ds114.Reset();
                                        adapter114.Fill(ds114);
                                        Session["WFTask"] = "REJECT";
                                        if (ds114.Tables[0].Rows.Count > 0)
                                        {
                                            cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            //Log Update
                                            int MaxStep = 0;
                                            int StartStep = 0;
                                            DataSet dsMaxStep = new DataSet();
                                            cmd = new SqlCommand("select max(StepNo) from WFLog where WFLogID='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            SqlDataAdapter adapterMaxStep = new SqlDataAdapter(cmd);
                                            dsMaxStep.Reset();
                                            adapterMaxStep.Fill(dsMaxStep);
                                            MaxStep = Convert.ToInt32(dsMaxStep.Tables[0].Rows[0][0].ToString());
                                            StartStep = Convert.ToInt32(StepNo) - 1;

                                            for (int kk = StartStep; kk <= MaxStep; kk++)
                                            {
                                                DataSet dsPrev = new DataSet();
                                                cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + kk + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
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
                                                    cmd = new SqlCommand("delete from WFLog where WFLogID='" + WFLogID + "' and StepNo='" + kk + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                                    cmd.ExecuteNonQuery();
                                                    for (int jj = 0; jj < dsPrev.Tables[0].Rows.Count; jj++)
                                                    {
                                                        cmd = new SqlCommand("insert into WFLog(WFLogID,StepNo,UserID,TaskID,TaskDoneDate,Comments,CompCode) values('" + WFLogID + "', '" + kk + "','','" + dsPrev.Tables[0].Rows[jj][2].ToString() + "','Waiting','Waiting','" + Session["CompCode"].ToString() + "')", con);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            CheckAction(WFLogID, StepNo - 1, DateTime.Now, "", "");
                                            return;
                                        }
                                        else
                                        {
                                            //CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                        }
                                    }
                                    // Update wf_log_task & wf_log_dtl for previous stage end
                                    #endregion
                                }
                                else if (TaskID == "REVIEW")
                                {
                                    #region
                                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and task_done_dt is null and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    DataSet dsT002 = new DataSet();
                                    SqlDataAdapter adapterT002 = new SqlDataAdapter(cmd);
                                    dsT002.Reset();
                                    adapterT002.Fill(dsT002);
                                    if (dsT002.Tables[0].Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Not Required' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + TaskID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT' and CompCode='" + Session["CompCode"].ToString() + "'", con);
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
                            cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null and (task_id='PRECOPY' or task_id='PREEMAIL' or task_id='PRECOND' or task_id='PREAPPEND')", con);
                            ds004 = new DataSet();
                            SqlDataAdapter adapter004 = new SqlDataAdapter(cmd);
                            ds004.Reset();
                            adapter004.Fill(ds004);
                            if (ds004.Tables[0].Rows.Count > 0)
                            {

                            }
                            else
                            {
                                #region For Postamble Soft Copy Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTSOFTCP")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    SendPostSoftCopy(WFLogID, StepNo, "POSTSOFTCP");
                                }
                                #endregion
                                #region For Postamble Mail Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTEMAIL")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleEmail ObjPostambleEmail = new PostambleEmail();
                                    ObjPostambleEmail.SendPostMail(WFLogID, StepNo, "POSTEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Copy Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTCOPY")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Copy Action
                                    PostambleCopy ObjPostambleCopy = new PostambleCopy();
                                    string PostCPOutput = ObjPostambleCopy.PostCopy(WFLogID, StepNo, "POSTCOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "POSTCOPY", PostCPOutput);
                                }
                                #endregion
                                #region For Postamble Conditional Mail Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTCOND")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleConditionalEmail ObjPostambleConditionalEmail = new PostambleConditionalEmail();
                                    ObjPostambleConditionalEmail.SendPostCondMail(WFLogID, StepNo, "POSTCOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Append Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTAPPEND")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
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
                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null", con);
                    ds005 = new DataSet();
                    SqlDataAdapter adapter005 = new SqlDataAdapter(cmd);
                    adapter005.Fill(ds005);
                    if (ds005.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                        // Call this method again
                        DataSet ds100 = new DataSet();
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null  and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND' and task_id!='PREAPPEND' and task_id!='POSTAPPEND')", con);
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
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Completed' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null", con);
                        ds006 = new DataSet();
                        SqlDataAdapter adapter006 = new SqlDataAdapter(cmd);
                        adapter006.Fill(ds006);
                        if (ds006.Tables[0].Rows.Count > 0)
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Ongoing' where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Completed',actual_completed_dt='" + TaskDoneDate + "' where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // Update Status end

                    // Check the stage is completed or not
                    cmd = new SqlCommand("select wf_stat from wf_log_dtl where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
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
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                //MessageBox(ex.Message);
            }
        }

        protected void SendPostSoftCopy(string WFLogID, int StepNo, string TaskID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet ds001 = new DataSet();
                DataSet ds002 = new DataSet();
                string FolderName = "";
                string TEMPXFolderName = "";
                string TEMPXFolderUUID = "";

                cmd = new SqlCommand("select fld_name from folder_mast where fld_uuid in(select fld_uuid from wf_mast where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "'))", con);
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    FolderName = ds001.Tables[0].Rows[0][0].ToString();
                    TEMPXFolderName = FolderName + " TEMPX";
                }
                cmd = new SqlCommand("select fld_uuid from folder_mast where fld_name='" + TEMPXFolderName + "'", con);
                SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                adapter002.Fill(ds002);
                if (ds002.Tables[0].Rows.Count > 0)
                {
                    TEMPXFolderUUID = ds002.Tables[0].Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string DwnldFile(string DocUUID)
        {
            // At first download the selected file from alfresco and save it to <TempDownload> Folder and then open the file
            //Download start
            // Initialise the reference to the spaces store
            Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
            spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
            spacesStore.address = "SpacesStore";

            Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
            referenceForNode.store = spacesStore;
            referenceForNode.uuid = DocUUID;//Doc UUID

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
            return newSaveFileName;
            //Download end                
        }

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();

            return ImageData; //return the byte data
        }

        private void Redirect2Home(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"home.aspx\"</script>";

            Page.Controls.Add(lbl);
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
                string OnlyExt = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(DocName);
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

    }
}