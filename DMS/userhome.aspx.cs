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
using System.IO;
using QuickPDFDLL0813;

namespace DMS
{
    public partial class userhome : System.Web.UI.Page
    {
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");
        //PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDFDLL0813.dll");

        public RepositoryService RepoService
        {
            set { repoService = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Header.DataBind();
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
                        lblUser.Text = Session["UserFullName"].ToString();
                        Session["dt"] = null;
                        Session["dt1"] = null;
                        /// Populate the To Do List GridView
                        PopToDo();
                        /// Populate the completed Jobs List GridView
                        PopCompletedJobs();
                        /// Populate the started workflow List GridView
                        PopStartedWF();
                        // Populate the Recent Docs Gridview
                        PopRecentDocs();
                        PopRejectedJobs();
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
                            if (Session["CanChangePwd"].ToString() == "N")
                            {
                                menuGenHome.Visible = false;
                                menuGenSystem.Visible = false;
                            }
                            else
                            {
                                menuGenHome.Visible = true;
                                menuGenSystem.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                }
                else
                {
                    string eventTarget1 = this.Request["__EVENTTARGET1"];
                    string eventArgument1 = this.Request["__EVENTARGUMENT1"];
                    string eventTarget2 = this.Request["__EVENTTARGET2"];
                    string eventArgument2 = this.Request["__EVENTARGUMENT2"];

                    if (eventTarget2 != String.Empty && eventTarget2 == "callPostBack2")
                    {
                        if (eventArgument2 != String.Empty && eventArgument2 == "RecentDocs")
                        {
                            Session["hfPageControl"] = "FE";
                            Response.Redirect("FormFillup.aspx?docname=" + hfTempDocName.Value + "&DocUUID=", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        #region ToDo List
        /// <summary>
        /// Populate the GridView to display the to do list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopToDo()
        {
            try
            {
                string WFID = "";
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                Session["ds3"] = null;
                cmd = new SqlCommand("select * from wf_dtl where CompCode='" + Session["CompCode"].ToString() + "' and wf_id in(select wf_id from wf_log_mast) and role_id in(select role_id from user_role where user_id='" + Session["UserID"].ToString() + "')", con);
                DataSet ds = new DataSet();
                DataSet ds01 = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds, "Table1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        WFID += WFID == "" ? "'" + ds.Tables[0].Rows[i][0].ToString() + "'" : "," + "'" + ds.Tables[0].Rows[i][0].ToString() + "'";
                    }
                }
                cmd = new SqlCommand("select * from wf_log_mast where wf_id in(" + WFID + ")", con);

                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);
                adapter1.Fill(ds, "Table2");
                DataTable dt4 = new DataTable();
                dt4.Columns.Add("WF_Log_ID", typeof(string));
                dt4.Columns.Add("Step_No", typeof(int));
                dt4.Columns.Add("WF_ID", typeof(Int64));
                DataRow r4;

                /// Merge the two tables
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() == ds.Tables[1].Rows[j][2].ToString())
                        {
                            r4 = dt4.NewRow();
                            r4["WF_ID"] = ds.Tables[0].Rows[i][0].ToString();
                            r4["WF_Log_ID"] = ds.Tables[1].Rows[j][0].ToString();
                            r4["Step_No"] = ds.Tables[0].Rows[i][1].ToString();
                            dt4.Rows.Add(r4);
                        }
                    }
                }

                DataSet ds3 = new DataSet();
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    cmd = new SqlCommand("select c.wf_log_id,a.wf_name,b.step_no,b.assign_dt,b.due_dt,c.started_by,d.doc_id,d.doc_name,d.download_path from wf_mast a,wf_log_dtl b, wf_log_mast c,doc_mast d where c.doc_id=d.doc_id and a.wf_id='" + dt4.Rows[i][2].ToString() + "' and b.wf_stat<>'Completed' and b.wf_log_id='" + dt4.Rows[i][0].ToString() + "' and b.step_no='" + dt4.Rows[i][1].ToString() + "' and c.wf_log_id='" + dt4.Rows[i][0].ToString() + "' and (c.wf_prog_stat='Started' or c.wf_prog_stat='Ongoing') order by c.start_dt", con);

                    SqlDataAdapter adapter3 = new SqlDataAdapter(cmd);
                    adapter3.Fill(ds3, "Table" + i);
                    if (i > 0)
                    {
                        ds3.Tables[0].Merge(ds3.Tables[i]);
                    }
                }


                /// Create a fresh ds02 to display the to do list whose previous stages are completed
                DataTable ds02 = new DataTable();
                ds02 = ds3.Tables[0].Clone();
                for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
                {

                    if (Convert.ToInt32(ds3.Tables[0].Rows[k]["step_no"]) > 1)
                    {
                        cmd = new SqlCommand("select * from wf_log_dtl where wf_log_id='" + ds3.Tables[0].Rows[k][0].ToString() + "' and step_no='" + (Convert.ToInt32(ds3.Tables[0].Rows[k][2].ToString()) - 1).ToString() + "' and wf_stat='Completed' order by due_dt", con);
                        ds01 = new DataSet();
                        adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds01);
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            ds02.ImportRow(ds3.Tables[0].Rows[k]);
                        }
                    }
                    else
                    {
                        ds02.ImportRow(ds3.Tables[0].Rows[k]);
                    }
                }
                gvToDo.DataSource = ds02;
                gvToDo.DataBind();
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvToDo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvToDo.SelectedRow;
                LinkButton lnkBtn = (LinkButton)row.FindControl("lnkTask");
                if (lnkBtn.CommandArgument == "ShowTask")
                {
                    Label lbAutoID = (Label)row.FindControl("lbAutoID");
                    Session["WFUpdtWFLogID"] = lbAutoID.Text.ToString();

                    Label lbWFName = (Label)row.FindControl("lbWFName");
                    Session["WFUpdtWFName"] = lbWFName.Text;

                    Label lbStepNo = (Label)row.FindControl("lbStepNo");
                    Session["WFUpdtStepNo"] = lbStepNo.Text;

                    Label lbAssignedDt = (Label)row.FindControl("lbAssignedDt");
                    Session["WFUpdtAssignDt"] = lbAssignedDt.Text;

                    Label lbDueDt = (Label)row.FindControl("lbDueDt");
                    Session["WFUpdtDueDt"] = lbDueDt.Text;

                    Label lbAssignedBy = (Label)row.FindControl("lbAssignedBy");
                    Session["WFUpdtAssignBy"] = lbAssignedBy.Text;

                    Label lbDocID = (Label)row.FindControl("lbDocID");
                    Session["WFUpdtDocID"] = lbDocID.Text;

                    Label lbDocName = (Label)row.FindControl("lbDocName");
                    Session["WFUpdtDocName"] = lbDocName.Text;

                    Label lbDownloadPath = (Label)row.FindControl("lbDownloadPath");
                    Session["WFUpdtDownloadPath"] = lbDownloadPath.Text;

                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    DataSet ds01 = new DataSet();
                    string NewVersionDocUUID = "";
                    string DocURL = "";

                    string LicenseKey = "";
                    string ServerIPAddress = "";
                    // Fetch ServerConfig Details Start
                    DataSet dsServerConfig = new DataSet();
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    dsServerConfig.Reset();
                    dsServerConfig = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                    if (dsServerConfig.Tables[0].Rows.Count > 0)
                    {
                        LicenseKey = dsServerConfig.Tables[0].Rows[0][0].ToString();
                        ServerIPAddress = dsServerConfig.Tables[0].Rows[0][1].ToString();
                    }
                    // Fetch ServerConfig Details End

                    string ActDocUUID = "";
                    con.Open();
                    cmd = new SqlCommand("select top 1 * from WFDocVersion where WFLogID='" + Session["WFUpdtWFLogID"].ToString() + "' and StepNo='" + Session["WFUpdtStepNo"].ToString() + "'", con);
                    SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                    ds01.Reset();
                    adapter02.Fill(ds01);
                    con.Close();
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        NewVersionDocUUID = ds01.Tables[0].Rows[0][3].ToString();
                        ActDocUUID = ds01.Tables[0].Rows[0][2].ToString();
                    }
                    //DocURL = DwnldFile(NewVersionDocUUID);

                    #region Checking the attached doc is eForm or not
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DocDetails(ActDocUUID,Session["CompCode"].ToString());
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        if (ds01.Tables[0].Rows[0][27].ToString() == "eForm")
                        {
                            Session["hfPageControl"] = "R";
                            Response.Redirect("eFormWFL.aspx?DocUUID=" + ActDocUUID, false);
                        }
                        else
                        {
                            DocURL = DwnldFile(ActDocUUID);
                            Session["hfPageControl"] = "R";
                            Response.Redirect("FormFill.aspx?docname=" + DocURL + "&DocUUID=" + ActDocUUID + "&AttachedDocUUID=" + ActDocUUID, false);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion

        #region Incomplete Documents
        /// <summary>
        /// Populate the GridView to display the Recent Docs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopRecentDocs()
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds02 = new DataSet();
                con.Open();
                cmd = new SqlCommand("select a.TempDocName,a.UserID,a.DocTypeID,a.TempDocStat,a.CreationDate,b.doc_type_name from TempDocSaving a,doc_type_mast b where a.DocTypeID=b.doc_type_id and a.UserID='" + Session["UserID"].ToString() + "' and a.TempDocStat='Not Uploaded' order by a.CreationDate desc", con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds02);
                gvRecentDocs.DataSource = ds02;
                gvRecentDocs.DataBind();
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvRecentDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["hfPageControl"] = "FE";
                GridViewRow row = gvRecentDocs.SelectedRow;
                Label lbTempDocName = (Label)row.FindControl("lbTempDocName");
                string lbTempDocName1 = lbTempDocName.Text.ToString();
                //SqlConnection con = Utility.GetConnection();
                //SqlCommand cmd = null;
                //con.Open();
                //cmd = new SqlCommand("select a.step_no,a.task_id,b.task_name,c.assign_dt,c.due_dt,a.task_done_dt from wf_log_task a,task_mast b,wf_log_dtl c where a.task_id=b.task_id and a.step_no=c.step_no and a.wf_log_id=c.wf_log_id and a.wf_log_id='" + lbWFLogID1 + "' order by a.step_no", con);
                //DataSet ds001 = new DataSet();
                //SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                //adapter001.Fill(ds001);
                //Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvRecentDocs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)gvRecentDocs.Rows[e.RowIndex];
                Label lbTempDocName = (Label)row.FindControl("lbTempDocName");

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                // Delete the file from Database
                cmd = new SqlCommand("delete from TempDocSaving where TempDocName='" + lbTempDocName.Text + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                Utility.CloseConnection(con);
                // Delete the file from Temporary Folder
                File.Delete(Server.MapPath("TempDownload") + "\\" + lbTempDocName.Text);
                PopRecentDocs();
                throw new Exception("Document has been removed successfully !!");
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        /// <summary>
        /// Javascript onclick function is getting bound to open the document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecentDocs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lbTempDocName");
                    LinkButton lnkbtn = (LinkButton)e.Row.FindControl("lnkTempDocName");
                    lnkbtn.Attributes.Add("onclick", "javascript:return RecentDocs('" + lbl.Text + "')");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion

        #region Workflows In Progress
        /// <summary>
        /// Populate the GridView to display the Started by the Logged in User Workflow list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopStartedWF()
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds02 = new DataSet();
                con.Open();
                cmd = new SqlCommand("select a.wf_log_id,a.wf_id,b.wf_name,c.doc_id,c.doc_name,a.wf_prog_stat from wf_log_mast a,wf_mast b,doc_mast c where a.wf_id=b.wf_id and a.doc_id=c.doc_id and a.started_by='" + Session["UserID"].ToString() + "' and a.wf_prog_stat='Ongoing' order by a.start_dt desc", con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds02);
                gvStartedWF.DataSource = ds02;
                gvStartedWF.DataBind();
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvStartedWF_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string mAction = "";
                string mStage = "";
                GridViewRow row = gvStartedWF.SelectedRow;
                Label lbWFLogID = (Label)row.FindControl("lbWFLogID");
                string lbWFLogID1 = lbWFLogID.Text.ToString();
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                cmd = new SqlCommand("select a.StepNo,b.task_name,a.UserID,a.TaskDoneDate,a.Comments from WFLog a,task_mast b where a.TaskID=b.task_id and a.TaskID not like('PRE%') and a.TaskID not like('POST%') and a.TaskDoneDate!='Not Required' and a.WFLogID='" + lbWFLogID1 + "'", con);
                DataSet ds001 = new DataSet();
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    MsgNodet.Text = "";
                    DataTable dt1 = new DataTable();
                    dt1.Columns.Add("Stage", typeof(int));
                    dt1.Columns.Add("Action", typeof(string));
                    dt1.Columns.Add("User", typeof(string));
                    dt1.Columns.Add("Completion Date", typeof(string));
                    dt1.Columns.Add("Comments", typeof(string));
                    for (int i = 0; i < ds001.Tables[0].Rows.Count; i++)
                    {
                        if (i >= 1 && (ds001.Tables[0].Rows[i][0].ToString() == ds001.Tables[0].Rows[i - 1][0].ToString()))
                        {

                        }
                        else
                        {
                            if (mStage == "2" && mAction == "Reject")
                            {
                                break;
                            }
                            else
                            {
                                DataRow r = dt1.NewRow();
                                r["Stage"] = ds001.Tables[0].Rows[i][0].ToString();
                                mStage = ds001.Tables[0].Rows[i][0].ToString();
                                // for waiting
                                if (ds001.Tables[0].Rows[i][3].ToString() == "Waiting" && ds001.Tables[0].Rows[i][4].ToString() == "Waiting")
                                {
                                    r["Action"] = "";
                                    mAction = "";
                                    r["Comments"] = "";
                                }
                                else
                                {
                                    r["Action"] = ds001.Tables[0].Rows[i][1].ToString();
                                    mAction = ds001.Tables[0].Rows[i][1].ToString();
                                    r["Comments"] = ds001.Tables[0].Rows[i][4].ToString();
                                }
                                if (ds001.Tables[0].Rows[i][2].ToString() == "")
                                {
                                    r["User"] = ds001.Tables[0].Rows[i][2].ToString();
                                }
                                else
                                {
                                    cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    DataSet ds002 = new DataSet();
                                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                                    adapter002.Fill(ds002);
                                    r["User"] = ds002.Tables[0].Rows[0][0].ToString();
                                }
                                r["Completion Date"] = ds001.Tables[0].Rows[i][3].ToString();
                                dt1.Rows.Add(r);
                            }
                        }
                    }
                    gv.DataSource = dt1;
                    gv.DataBind();
                    dt1.Clear();
                }
                else
                {
                    gv.DataSource = null;
                    gv.DataBind();
                    MsgNodet.Text = "No Details Found for this Document!";
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion

        #region Completed Workflows
        /// <summary>
        /// Populate the GridView to display the completed jobs list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopCompletedJobs()
        {
            try
            {
                string WFID = "";
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                Session["ds3"] = null;
                cmd = new SqlCommand("select * from wf_dtl where CompCode='" + Session["CompCode"].ToString() + "' and wf_id in(select wf_id from wf_log_mast) and role_id in(select role_id from user_role where user_id='" + Session["UserID"].ToString() + "')", con);
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds, "Table1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        WFID += WFID == "" ? "'" + ds.Tables[0].Rows[i][0].ToString() + "'" : "," + "'" + ds.Tables[0].Rows[i][0].ToString() + "'";
                    }
                }
                if (WFID == "")
                {
                    WFID = "''";
                }
                cmd = new SqlCommand("select * from wf_log_mast where wf_id in(" + WFID + ")", con);

                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);
                adapter1.Fill(ds, "Table2");
                DataTable dt4 = new DataTable();
                dt4.Columns.Add("WF_Log_ID", typeof(string));
                dt4.Columns.Add("Step_No", typeof(int));
                dt4.Columns.Add("WF_ID", typeof(Int64));
                DataRow r4;


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() == ds.Tables[1].Rows[j][2].ToString())
                        {
                            r4 = dt4.NewRow();
                            r4["WF_ID"] = ds.Tables[0].Rows[i][0].ToString();
                            r4["WF_Log_ID"] = ds.Tables[1].Rows[j][0].ToString();
                            r4["Step_No"] = ds.Tables[0].Rows[i][1].ToString();
                            dt4.Rows.Add(r4);
                        }
                    }
                }

                DataSet ds3 = new DataSet();
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    //cmd = new SqlCommand("select c.wf_log_id,a.wf_name,b.step_no,b.assign_dt,b.due_dt,c.started_by,d.doc_name,d.download_path from wf_mast a,wf_log_dtl b, wf_log_mast c,doc_mast d where c.doc_id=d.doc_id and a.wf_id='" + dt4.Rows[i][2].ToString() + "' and b.wf_stat='Completed' and b.wf_log_id='" + dt4.Rows[i][0].ToString() + "' and b.step_no='" + dt4.Rows[i][1].ToString() + "' and c.wf_log_id='" + dt4.Rows[i][0].ToString() + "' order by c.start_dt", con);
                    cmd = new SqlCommand("select c.wf_log_id,a.wf_name,b.step_no,b.assign_dt,b.due_dt,c.started_by,d.doc_name,d.download_path,c.actual_completed_dt from wf_mast a,wf_log_dtl b, wf_log_mast c,doc_mast d where c.doc_id=d.doc_id and a.wf_id='" + dt4.Rows[i][2].ToString() + "' and b.wf_stat='Completed' and b.wf_log_id='" + dt4.Rows[i][0].ToString() + "' and b.step_no='" + dt4.Rows[i][1].ToString() + "' and c.wf_log_id='" + dt4.Rows[i][0].ToString() + "' order by c.actual_completed_dt desc", con);
                    SqlDataAdapter adapter3 = new SqlDataAdapter(cmd);
                    adapter3.Fill(ds3, "Table" + i);
                    if (i > 0)
                    {
                        ds3.Tables[0].Merge(ds3.Tables[i]);
                    }
                }

                //select a.wf_log_id,b.wf_name,c.doc_name,a.wf_prog_stat from wf_log_mast a,wf_mast b,doc_mast c where a.wf_id=b.wf_id and a.doc_id=c.doc_id and a.started_by='" + Session["UserID"].ToString() + "'
                //select a.wf_log_id,c.wf_name,null as step_no,a.start_dt as assign_dt,a.due_dt,a.started_by,b.doc_name,b.download_path,a.actual_completed_dt from wf_log_mast a,doc_mast b,wf_mast c where a.doc_id=b.doc_id and a.wf_id=c.wf_id and a.wf_prog_stat='Completed' and a.started_by='" + Session["UserID"].ToString() + "' order by a.actual_completed_dt desc
                Session["ds3"] = ds3;
                ds3 = null;
                ds3 = (DataSet)Session["ds3"];

                cmd = new SqlCommand("select a.wf_log_id,c.wf_name,null as step_no,a.start_dt as assign_dt,a.due_dt,a.started_by,b.doc_name,b.download_path,a.actual_completed_dt from wf_log_mast a,doc_mast b,wf_mast c where a.doc_id=b.doc_id and a.wf_id=c.wf_id and a.wf_prog_stat='Completed' and a.started_by='" + Session["UserID"].ToString() + "' order by a.actual_completed_dt desc", con);
                DataSet ds01 = new DataSet();
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);
                ds3.Merge(ds01);
                gvCompleted.DataSource = ds3;
                gvCompleted.DataBind();
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvCompleted_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string mAction = "";
                string mStage = "";
                GridViewRow row = gvCompleted.SelectedRow;
                Label lbWFLogID = (Label)row.FindControl("lbWFLogID");
                string lbWFLogID1 = lbWFLogID.Text.ToString();
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                cmd = new SqlCommand("select a.StepNo,b.task_name,a.UserID,a.TaskDoneDate,a.Comments from WFLog a,task_mast b where a.TaskID=b.task_id and a.TaskID not like('PRE%') and a.TaskID not like('POST%') and a.TaskDoneDate!='Not Required' and a.WFLogID='" + lbWFLogID1 + "'", con);
                DataSet ds001 = new DataSet();
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    MsgNodet.Text = "";
                    DataTable dt1 = new DataTable();
                    dt1.Columns.Add("Stage", typeof(int));
                    dt1.Columns.Add("Action", typeof(string));
                    dt1.Columns.Add("User", typeof(string));
                    dt1.Columns.Add("Completion Date", typeof(string));
                    dt1.Columns.Add("Comments", typeof(string));
                    for (int i = 0; i < ds001.Tables[0].Rows.Count; i++)
                    {
                        if (i >= 1 && (ds001.Tables[0].Rows[i][0].ToString() == ds001.Tables[0].Rows[i - 1][0].ToString()))
                        {

                        }
                        else
                        {
                            if (mStage == "2" && mAction == "Reject")
                            {
                                break;
                            }
                            else
                            {
                                DataRow r = dt1.NewRow();
                                r["Stage"] = ds001.Tables[0].Rows[i][0].ToString();
                                mStage = ds001.Tables[0].Rows[i][0].ToString();
                                // for waiting
                                if (ds001.Tables[0].Rows[i][3].ToString() == "Waiting" && ds001.Tables[0].Rows[i][4].ToString() == "Waiting")
                                {
                                    r["Action"] = "";
                                    mAction = "";
                                    r["Comments"] = "";
                                }
                                else
                                {
                                    r["Action"] = ds001.Tables[0].Rows[i][1].ToString();
                                    mAction = ds001.Tables[0].Rows[i][1].ToString();
                                    r["Comments"] = ds001.Tables[0].Rows[i][4].ToString();
                                }
                                if (ds001.Tables[0].Rows[i][2].ToString() == "")
                                {
                                    #region For the User who is responsible for this stage
                                    cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id in(select user_id from user_role where role_id in(select role_id from wf_dtl where step_no='" + ds001.Tables[0].Rows[i][0].ToString() + "' and wf_id in(select wf_id from wf_log_mast where wf_log_id='" + lbWFLogID1 + "')))", con);
                                    DataSet ds003 = new DataSet();
                                    SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                                    adapter003.Fill(ds003);
                                    r["User"] = ds003.Tables[0].Rows[0][0].ToString();
                                    #endregion
                                }
                                else
                                {
                                    cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    DataSet ds002 = new DataSet();
                                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                                    adapter002.Fill(ds002);
                                    r["User"] = ds002.Tables[0].Rows[0][0].ToString();
                                }
                                r["Completion Date"] = ds001.Tables[0].Rows[i][3].ToString();
                                dt1.Rows.Add(r);
                            }
                        }
                    }
                    gv.DataSource = dt1;
                    gv.DataBind();
                    dt1.Clear();
                }
                else
                {
                    gv.DataSource = null;
                    gv.DataBind();
                    MsgNodet.Text = "No Details Found for this Document!";
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion

        #region Rejected Workflows
        protected void PopRejectedJobs()
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds02 = new DataSet();
                con.Open();
                cmd = new SqlCommand("select a.wf_log_id,b.doc_name,a.start_dt,a.started_by,a.wf_id,a.doc_id,c.wf_name,a.wf_prog_stat from wf_log_mast a,doc_mast b,wf_mast c where a.doc_id=b.doc_id and a.wf_id=c.wf_id and a.started_by='" + Session["UserID"].ToString() + "' and a.wf_prog_stat='Rejected' order by a.start_dt desc", con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds02);
                gvRejectedWFL.DataSource = ds02;
                gvRejectedWFL.DataBind();
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvRejectedWFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string mAction = "";
                string mStage = "";
                GridViewRow row = gvRejectedWFL.SelectedRow;
                Label lbWFLogID = (Label)row.FindControl("lbWFLogID");
                string lbWFLogID1 = lbWFLogID.Text.ToString();
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                cmd = new SqlCommand("select a.StepNo,b.task_name,a.UserID,a.TaskDoneDate,a.Comments from WFLog a,task_mast b where a.TaskID=b.task_id and a.TaskID not like('PRE%') and a.TaskID not like('POST%') and a.TaskDoneDate!='Not Required' and a.WFLogID='" + lbWFLogID1 + "'", con);
                DataSet ds0001 = new DataSet();
                SqlDataAdapter adapter0001 = new SqlDataAdapter(cmd);
                adapter0001.Fill(ds0001);
                if (ds0001.Tables[0].Rows.Count > 0)
                {
                    MsgNodet1.Text = "";
                    DataTable dt01 = new DataTable();
                    dt01.Columns.Add("Stage", typeof(int));
                    dt01.Columns.Add("Action", typeof(string));
                    dt01.Columns.Add("User", typeof(string));
                    dt01.Columns.Add("Completion Date", typeof(string));
                    dt01.Columns.Add("Comments", typeof(string));
                    for (int i = 0; i < ds0001.Tables[0].Rows.Count; i++)
                    {
                        if (i >= 1 && (ds0001.Tables[0].Rows[i][0].ToString() == ds0001.Tables[0].Rows[i - 1][0].ToString()))
                        {

                        }
                        else
                        {
                            if (mStage == "2" && mAction == "Reject")
                            {
                                break;
                            }
                            else
                            {
                                DataRow r = dt01.NewRow();
                                r["Stage"] = ds0001.Tables[0].Rows[i][0].ToString();
                                mStage = ds0001.Tables[0].Rows[i][0].ToString();
                                // for waiting
                                if (ds0001.Tables[0].Rows[i][3].ToString() == "Waiting" && ds0001.Tables[0].Rows[i][4].ToString() == "Waiting")
                                {
                                    r["Action"] = "";
                                    mAction = "";
                                    r["Comments"] = "";
                                }
                                else
                                {
                                    r["Action"] = ds0001.Tables[0].Rows[i][1].ToString();
                                    mAction = ds0001.Tables[0].Rows[i][1].ToString();
                                    r["Comments"] = ds0001.Tables[0].Rows[i][4].ToString();
                                }
                                if (ds0001.Tables[0].Rows[i][2].ToString() == "")
                                {
                                    r["User"] = ds0001.Tables[0].Rows[i][2].ToString();
                                }
                                else
                                {
                                    cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id='" + ds0001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    DataSet ds002 = new DataSet();
                                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                                    adapter002.Fill(ds002);
                                    r["User"] = ds002.Tables[0].Rows[0][0].ToString();
                                }
                                r["Completion Date"] = ds0001.Tables[0].Rows[i][3].ToString();
                                dt01.Rows.Add(r);
                            }
                        }
                    }
                    gvRejectHist.DataSource = dt01;
                    gvRejectHist.DataBind();
                    dt01.Clear();
                }
                else
                {
                    gvRejectHist.DataSource = null;
                    gvRejectHist.DataBind();
                    MsgNodet1.Text = "No Details Found for this Document!";
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }
        
        private string DwnldFile(string DocUUID)
        {
            string DocExtension = "";
            DataSet dsDocDtls = new DataSet();
            FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
            
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
            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
            dsDocDtls.Reset();
            dsDocDtls = ObjClassStoreProc.DocDetails(DocUUID, Session["CompCode"].ToString());

            if (dsDocDtls.Tables[0].Rows.Count > 0)
            {
                DocExtension = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(dsDocDtls.Tables[0].Rows[0][1].ToString());
            }
            string newSaveFileName = Guid.NewGuid() + "." + DocExtension;
            Session["RWFOldFile"] = newSaveFileName;
            string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
            SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
            ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
            return newSaveFileName;
            //Download end                
        }

        protected void cmdDwnload_Click(object sender, EventArgs e)
        {

        }
    }
}