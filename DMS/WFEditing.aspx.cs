using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DMS.BAL;
using Microsoft.Web.Services3.Referral;
using DMS.UTILITY;

namespace DMS
{
    public partial class WFEditing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                    if (Session["UserType"].ToString() == "S" || Session["UserType"].ToString() == "A")
                    {
                        if (Session["UserType"].ToString() == "S")
                        {
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A")
                        {
                            divMenuAdmin.Visible = true;
                            divMenuSuperAdmin.Visible = false;
                        }
                        
                        divMenuNormal.Visible = false;
                        lblUser.Text = Session["UserFullName"].ToString();
                        Page.Header.DataBind();
                        Session["WFId"] = Request.QueryString["WFId"].ToString();
                        Session["EditCompCode"] = Request.QueryString["EditCompCode"].ToString();
                        PopulateDropdown();
                        PopWFDetails();
                        FetchWFMasterData(Session["WFId"].ToString());
                        Session["WFName"] = null;
                        Session["dt1"] = null;
                        Session["dt2"] = null;
                        Session["dt3"] = null;
                        Session["dt03"] = null;
                        /// To Populate Cabinet dropdown for Copy to Location
                        PopulateCabinet2();
                        PopulateCabinetCopy(); // For the copy option in the popup
                        PopulateCabinetAppend(); // For the Appended Doc in the popup
                        /// To Populate Cabinet dropdown for Append Doc
                        PopulateCabinet3();
                        /// To Populate User dropdown for Mail ID Settings
                        PopUser();
                        divCopyLocation.Visible = false;
                        divAmbleMail.Visible = false;
                        divCondMail.Visible = false;
                        divAppendedDoc.Visible = false;
                        divUpdateButton.Visible = false;
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                }
                else
                {
                    Response.Redirect("logout.aspx", false);
                }
            }
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
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

                //....Cabinet
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinet.DataSource = ds01;
                ddCabinet.DataTextField = "cab_name";
                ddCabinet.DataValueField = "cab_uuid";
                ddCabinet.DataBind();
                PopulateDrawer(ddCabinet.SelectedValue);

                //....Role
                ds01.Reset();
                ds01 = ObjClassStoreProc.RoleBasedOnCompCode(Session["CompCode"].ToString());
                ddRole1.DataSource = ds01;
                ddRole1.DataTextField = "role_name";
                ddRole1.DataValueField = "role_id";
                ddRole1.DataBind();
                SetRoleBasedSettings();

                //....Task
                ds01.Reset();
                ds01 = ObjClassStoreProc.TaskList();
                ddTask1.DataSource = ds01;
                ddTask1.DataTextField = "task_name";
                ddTask1.DataValueField = "task_id";
                ddTask1.DataBind();
                SetTaskBasedSettings();
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void FetchWFMasterData(string WFID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FetchWFMasterData(WFID);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    txtWFName.Text = ds01.Tables[0].Rows[0][1].ToString();
                    Session["WFName"] = txtWFName.Text.Trim();
                    for (int i = 0; i < ddDept.Items.Count; i++)
                    {
                        if (ddDept.Items[i].Value == ds01.Tables[0].Rows[0][2].ToString())
                        {
                            ddDept.ClearSelection();
                            ddDept.Items[i].Selected = true;
                        }
                    }
                    for (int i = 0; i < ddDocType.Items.Count; i++)
                    {
                        if (ddDocType.Items[i].Value == ds01.Tables[0].Rows[0][3].ToString())
                        {
                            ddDocType.ClearSelection();
                            ddDocType.Items[i].Selected = true;
                        }
                    }
                    ds02.Reset();
                    ds02 = ObjClassStoreProc.FetchCabinetDrawerUUID(ds01.Tables[0].Rows[0][5].ToString());
                    if (ds02.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ddCabinet.Items.Count; i++)
                        {
                            if (ddCabinet.Items[i].Value == ds02.Tables[0].Rows[0][0].ToString())
                            {
                                ddCabinet.ClearSelection();
                                ddCabinet.Items[i].Selected = true;
                            }
                        }
                        PopulateDrawer(ddCabinet.SelectedValue);

                        for (int i = 0; i < ddDrawer.Items.Count; i++)
                        {
                            if (ddDrawer.Items[i].Value == ds02.Tables[0].Rows[0][1].ToString())
                            {
                                ddDrawer.ClearSelection();
                                ddDrawer.Items[i].Selected = true;
                            }
                        }
                        PopulateFolder(ddDrawer.SelectedValue);

                        for (int i = 0; i < ddFolder.Items.Count; i++)
                        {
                            if (ddFolder.Items[i].Value == ds01.Tables[0].Rows[0][5].ToString())
                            {
                                ddFolder.ClearSelection();
                                ddFolder.Items[i].Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void cmdUpdateMaster_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                gvWFTasks.DataSource = null;
                gvWFTasks.DataBind();
                divCopyLocation.Visible = false;
                divAmbleMail.Visible = false;
                divCondMail.Visible = false;
                divAppendedDoc.Visible = false;
                divUpdateButton.Visible = false;

                wf_mast_bal wfMastObj = new wf_mast_bal();
                wfMastObj.WFName = txtWFName.Text.Trim();
                wfMastObj.WFDept = ddDept.SelectedValue;
                wfMastObj.WFDocType = ddDocType.SelectedValue;
                wfMastObj.WFFolderUUID = ddFolder.SelectedValue;

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = ObjClassStoreProc.UpdateWFMast(Convert.ToInt32(Session["WFId"].ToString()), txtWFName.Text.Trim(), ddDept.SelectedValue, ddDocType.SelectedValue, ddFolder.SelectedValue,Session["CompCode"].ToString());
                if (result == "-1")
                {
                    throw new Exception("Workflow name already exists !!");
                }
                else if (result == "-2")
                {
                    throw new Exception("Workflow already assigned for this document type & department combination !!");
                }
                else if (result == "1")
                {
                    Session["WFName"] = txtWFName.Text.Trim();
                    Session["dt1"] = null;
                    Session["dt2"] = null;
                    throw new Exception("Master Data Updated Successfully !!");
                }
                else
                {
                    throw new Exception("Master Data Updation Error !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopWFDetails()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.WFDetails(Convert.ToInt32(Session["WFId"].ToString()), Session["CompCode"].ToString());
                gvWFDetails.DataSource = ds01;
                gvWFDetails.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvWFDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            GridViewRow row = gvWFDetails.SelectedRow;
            Label lbStepNo = (Label)row.FindControl("lbStepNo");
            Session["StepNo"] = lbStepNo.Text;
            PopWFTasks(Session["WFId"].ToString(), Session["StepNo"].ToString());
            divCopyLocation.Visible = false;
            divAmbleMail.Visible = false;
            divCondMail.Visible = false;
            divAppendedDoc.Visible = false;
            divUpdateButton.Visible = false;
        }

        protected void gvWFDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                Session["StepNo"] = gvWFDetails.DataKeys[e.NewEditIndex].Values["step_no"].ToString();

                divCopyLocation.Visible = false;
                divAmbleMail.Visible = false;
                divCondMail.Visible = false;
                divAppendedDoc.Visible = false;
                divUpdateButton.Visible = false;

                if (Session["StepNo"].ToString() == "1")
                {
                    throw new Exception("Initiator Role can not be edited !!");
                }
                else
                {
                    gvWFDetails.EditIndex = e.NewEditIndex;
                    PopWFDetails();
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value=ex.Message;
            }
        }

        protected void gvWFDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    gvWFDetails.EditIndex = -1;
                    PopWFDetails();
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                int rIndex = e.RowIndex;
                GridViewRow row = (GridViewRow)gvWFDetails.Rows[e.RowIndex];

                DropDownList ddlEditRole = (DropDownList)row.FindControl("ddlEditRole");
                TextBox txtEditDuration = (TextBox)row.FindControl("txtEditDuration");

                Label lbStepNo = (Label)row.FindControl("lbStepNo");

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = ObjClassStoreProc.UpdateWFDetails(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(lbStepNo.Text), ddlEditRole.SelectedValue, txtEditDuration.Text.Trim());
                gvWFDetails.EditIndex = -1;
                PopWFDetails();
                if (result == "-1")
                {
                    throw new Exception("This Role already exists !!");
                }
                else if (result == "-2")
                {
                    throw new Exception("Please select Initiator Role in the first stage !!");
                }
                else if (result == "1")
                {
                    throw new Exception("Data Updated Successfully !!");
                }
                else
                {
                    throw new Exception("Updation Error !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void gvWFDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                gvWFDetails.EditIndex = -1;
                PopWFDetails();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvWFDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // For Role Dropdown
                Control ctrl1 = e.Row.FindControl("ddlEditRole");
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                if (ctrl1 != null)
                {
                    DropDownList ddlEditRole = ctrl1 as DropDownList;
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.RoleBasedOnCompCode(Session["CompCode"].ToString());
                    ddlEditRole.DataSource = ds01;
                    ddlEditRole.DataTextField = "role_name";
                    ddlEditRole.DataValueField = "role_id";
                    ddlEditRole.DataBind();
                    Control ctrl6 = e.Row.FindControl("hdRole");
                    if (ctrl6 != null)
                    {
                        HiddenField hdRole = ctrl6 as HiddenField;
                        for (int i = 0; i < ddlEditRole.Items.Count; i++)
                        {
                            if (ddlEditRole.Items[i].Text == hdRole.Value)
                            {
                                ddlEditRole.ClearSelection();
                                ddlEditRole.Items[i].Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvWFDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    gvWFDetails.EditIndex = -1;
                    PopWFDetails();
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                gvWFTasks.DataSource = null;
                gvWFTasks.DataBind();
                divCopyLocation.Visible = false;
                divAmbleMail.Visible = false;
                divCondMail.Visible = false;
                divAppendedDoc.Visible = false;
                divUpdateButton.Visible = false;

                GridViewRow row = (GridViewRow)gvWFDetails.Rows[e.RowIndex];
                Label lbStepNo = (Label)row.FindControl("lbStepNo");

                if (lbStepNo.Text == "1")
                {
                    gvWFDetails.EditIndex = -1;
                    PopWFDetails();
                    throw new Exception("1st stage can not be deleted !!");
                }
                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = ObjClassStoreProc.DeleteWFDetails(Convert.ToInt32(Session["WFId"].ToString()),Convert.ToInt32(lbStepNo.Text));
                if (result == "1")
                {
                    gvWFDetails.EditIndex = -1;
                    PopWFDetails();
                    throw new Exception("Data Deleted Successfully !!");
                }
                else
                {
                    throw new Exception("Error in Data Deletion !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void PopWFTasks(string WFID, string StepNo)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.WFTasks(Convert.ToInt32(WFID), Convert.ToInt32(StepNo));
                gvWFTasks.DataSource = ds01;
                gvWFTasks.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvWFTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            GridViewRow row = gvWFTasks.SelectedRow;
            Label lbStepNo = (Label)row.FindControl("lbStepNo");
            Label lbTaskID = (Label)row.FindControl("lbTaskID");
            Session["TaskID"] = lbTaskID.Text;
        }

        protected void gvWFTasks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    gvWFTasks.EditIndex = -1;
                    PopWFTasks(Request.QueryString["WFId"].ToString(), Session["StepNo"].ToString());
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                String mTaskID = gvWFTasks.DataKeys[e.NewEditIndex].Values["task_id"].ToString();
                Session["TaskID"] = mTaskID;
                if (mTaskID == "REJECT")
                {
                    divCopyLocation.Visible = false;
                    divAmbleMail.Visible = false;
                    divCondMail.Visible = false;
                    divAppendedDoc.Visible = false;
                    divUpdateButton.Visible = false;
                    throw new Exception("REJECT task can not be edited !!");
                }
                else if (mTaskID == "APPROVE" || mTaskID == "REVIEW")
                {
                    divCopyLocation.Visible = false;
                    divAmbleMail.Visible = false;
                    divCondMail.Visible = false;
                    divAppendedDoc.Visible = false;
                    divUpdateButton.Visible = false;
                    gvWFTasks.EditIndex = e.NewEditIndex;
                    PopWFTasks(Request.QueryString["WFId"].ToString(), Session["StepNo"].ToString());
                }
                else if (mTaskID == "POSTAPPEND" || mTaskID == "PREAPPEND")
                {
                    divCopyLocation.Visible = false;
                    divAmbleMail.Visible = false;
                    divCondMail.Visible = false;
                    divAppendedDoc.Visible = true;
                    divUpdateButton.Visible = true;
                    PopSettings(Session["WFId"].ToString(),Session["StepNo"].ToString(),mTaskID);
                }
                else if (mTaskID == "POSTCOND" || mTaskID == "PRECOND")
                {
                    divCopyLocation.Visible = false;
                    divAmbleMail.Visible = false;
                    divCondMail.Visible = true;
                    divAppendedDoc.Visible = false;
                    divUpdateButton.Visible = false;
                    PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), mTaskID);
                }
                else if (mTaskID == "POSTCOPY" || mTaskID == "PRECOPY")
                {
                    divCopyLocation.Visible = true;
                    divAmbleMail.Visible = false;
                    divCondMail.Visible = false;
                    divAppendedDoc.Visible = false;
                    divUpdateButton.Visible = true;
                    PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), mTaskID);
                }
                else if (mTaskID == "POSTEMAIL" || mTaskID == "PREEMAIL")
                {
                    divCopyLocation.Visible = false;
                    divAmbleMail.Visible = true;
                    divCondMail.Visible = false;
                    divAppendedDoc.Visible = false;
                    divUpdateButton.Visible = true;
                    PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), mTaskID);
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value= ex.Message;
            }
        }

        protected void gvWFTasks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    gvWFTasks.EditIndex = -1;
                    PopWFTasks(Request.QueryString["WFId"].ToString(), Session["StepNo"].ToString());
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                int rIndex = e.RowIndex;
                GridViewRow row = (GridViewRow)gvWFTasks.Rows[e.RowIndex];

                DropDownList ddlEditTask = (DropDownList)row.FindControl("ddlEditTask");

                Label lbStepNo = (Label)row.FindControl("lbStepNo");
                Label lbTaskID = (Label)row.FindControl("lbTaskID");

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = "";
                result = ObjClassStoreProc.ChngAppRvw(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(lbStepNo.Text), lbTaskID.Text, ddlEditTask.SelectedValue);
                gvWFTasks.EditIndex = -1;
                PopWFTasks(Session["WFId"].ToString(), Session["StepNo"].ToString());
                if (result == "1")
                {
                    throw new Exception("Data Updated Successfully !!");
                }
                else
                {
                    throw new Exception("Updation Error !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void gvWFTasks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                gvWFTasks.EditIndex = -1;
                PopWFTasks(Session["WFId"].ToString(), Session["StepNo"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        
        protected void gvWFTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // For Task Dropdown
                Control ctrl1 = e.Row.FindControl("ddlEditTask");
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                if (ctrl1 != null)
                {
                    DropDownList ddlEditTask = ctrl1 as DropDownList;
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.AppRvwTaskList();
                    ddlEditTask.DataSource = ds01;
                    ddlEditTask.DataTextField = "task_name";
                    ddlEditTask.DataValueField = "task_id";
                    ddlEditTask.DataBind();
                    Control ctrl6 = e.Row.FindControl("hdTask");
                    if (ctrl6 != null)
                    {
                        HiddenField hdTask = ctrl6 as HiddenField;
                        for (int i = 0; i < ddlEditTask.Items.Count; i++)
                        {
                            if (ddlEditTask.Items[i].Text == hdTask.Value)
                            {
                                ddlEditTask.ClearSelection();
                                ddlEditTask.Items[i].Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvWFTasks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    gvWFTasks.EditIndex = -1;
                    PopWFTasks(Request.QueryString["WFId"].ToString(), Session["StepNo"].ToString());
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                GridViewRow row = (GridViewRow)gvWFTasks.Rows[e.RowIndex];
                Label lbStepNo = (Label)row.FindControl("lbStepNo");
                Label lbTaskID = (Label)row.FindControl("lbTaskID");
                if (lbTaskID.Text == "APPROVE" || lbTaskID.Text == "REJECT" || lbTaskID.Text == "REVIEW")
                {
                    throw new Exception("Interactive tasks can not be deleted !!");
                }
                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = ObjClassStoreProc.DeleteWFTasks(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(lbStepNo.Text), lbTaskID.Text);
                if (result == "1")
                {
                    gvWFTasks.EditIndex = -1;
                    PopWFTasks(Session["WFId"].ToString(), Session["StepNo"].ToString());
                    throw new Exception("Data Deleted Successfully !!");
                }
                else
                {
                    throw new Exception("Error in Data Deletion !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void PopSettings(string WFID,string StepNo,String TaskID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                #region POSTAPPEND or PREAPPEND
                if (TaskID == "POSTAPPEND" || TaskID == "PREAPPEND")
                {
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.WFTaskDetails(Convert.ToInt32(WFID), Convert.ToInt32(StepNo), TaskID);
                    ds02.Reset();
                    ds02 = ObjClassStoreProc.DocDetails(ds01.Tables[0].Rows[0][8].ToString(), Session["CompCode"].ToString());
                    string DocUUID = ds02.Tables[0].Rows[0][4].ToString();
                    string FolderUUID = ds02.Tables[0].Rows[0][5].ToString();

                    ds02.Reset();
                    ds02 = ObjClassStoreProc.FetchCabinetDrawerUUID(FolderUUID);
                    if (ds02.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ddCabinet3.Items.Count; i++)
                        {
                            if (ddCabinet3.Items[i].Value == ds02.Tables[0].Rows[0][0].ToString())
                            {
                                ddCabinet3.ClearSelection();
                                ddCabinet3.Items[i].Selected = true;
                            }
                        }
                        PopulateDrawer3(ddCabinet3.SelectedValue);

                        for (int i = 0; i < ddDrawer3.Items.Count; i++)
                        {
                            if (ddDrawer3.Items[i].Value == ds02.Tables[0].Rows[0][1].ToString())
                            {
                                ddDrawer3.ClearSelection();
                                ddDrawer3.Items[i].Selected = true;
                            }
                        }
                        PopulateFolder3(ddDrawer3.SelectedValue);

                        for (int i = 0; i < ddFolder3.Items.Count; i++)
                        {
                            if (ddFolder3.Items[i].Value == ds01.Tables[0].Rows[0][5].ToString())
                            {
                                ddFolder3.ClearSelection();
                                ddFolder3.Items[i].Selected = true;
                            }
                        }
                        PopulateDoc3(ddFolder3.SelectedValue);

                        for (int i = 0; i < ddDocument3.Items.Count; i++)
                        {
                            if (ddDocument3.Items[i].Value == DocUUID)
                            {
                                ddDocument3.ClearSelection();
                                ddDocument3.Items[i].Selected = true;
                            }
                        }
                    }

                }
                #endregion
                #region POSTCOND or PRECOND
                else if (TaskID == "POSTCOND" || TaskID == "PRECOND")
                {
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.WFCondDetails(Convert.ToInt32(WFID), Convert.ToInt32(StepNo), TaskID);
                    gvCond.DataSource = ds01;
                    gvCond.DataBind();
                    //txtFormFieldNo.Text = ds01.Tables[0].Rows[0][3].ToString();

                    //if (ds01.Tables[0].Rows[0][4].ToString() == "<")
                    //{
                    //    ddCondOp.ClearSelection();
                    //    ddCondOp.Items[0].Selected = true;
                    //}
                    //else if (ds01.Tables[0].Rows[0][4].ToString() == ">")
                    //{
                    //    ddCondOp.ClearSelection();
                    //    ddCondOp.Items[1].Selected = true;
                    //}
                    //else if (ds01.Tables[0].Rows[0][4].ToString() == "=")
                    //{
                    //    ddCondOp.ClearSelection();
                    //    ddCondOp.Items[2].Selected = true;
                    //}
                    //else if (ds01.Tables[0].Rows[0][4].ToString() == "!=")
                    //{
                    //    ddCondOp.ClearSelection();
                    //    ddCondOp.Items[3].Selected = true;
                    //}

                    //txtCondVal.Text = ds01.Tables[0].Rows[0][5].ToString();
                    //txtToMail1.Text = ds01.Tables[0].Rows[0][6].ToString();
                    //txtCondSub.Text = ds01.Tables[0].Rows[0][10].ToString();
                    //txtCondMsg.Text = ds01.Tables[0].Rows[0][7].ToString();

                    //if (ds01.Tables[0].Rows[0][8].ToString() == "Yes")
                    //{
                    //    ddAttachCondMail.ClearSelection();
                    //    ddAttachCondMail.Items[0].Selected = true;
                    //}
                    //else if (ds01.Tables[0].Rows[0][8].ToString() == "No")
                    //{
                    //    ddAttachCondMail.ClearSelection();
                    //    ddAttachCondMail.Items[1].Selected = true;
                    //}

                    //if (ds01.Tables[0].Rows[0][9].ToString() == "Yes")
                    //{
                    //    ddCondURL.ClearSelection();
                    //    ddCondURL.Items[0].Selected = true;
                    //}
                    //else if (ds01.Tables[0].Rows[0][9].ToString() == "No")
                    //{
                    //    ddCondURL.ClearSelection();
                    //    ddCondURL.Items[1].Selected = true;
                    //}
                }
                #endregion
                #region POSTCOPY or PRECOPY
                else if (TaskID == "POSTCOPY" || TaskID == "PRECOPY")
                {
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.WFTaskDetails(Convert.ToInt32(WFID), Convert.ToInt32(StepNo), TaskID);
                    ds02.Reset();
                    ds02 = ObjClassStoreProc.FetchCabinetDrawerUUID(ds01.Tables[0].Rows[0][4].ToString());
                    if (ds02.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ddCabinet2.Items.Count; i++)
                        {
                            if (ddCabinet2.Items[i].Value == ds02.Tables[0].Rows[0][0].ToString())
                            {
                                ddCabinet2.ClearSelection();
                                ddCabinet2.Items[i].Selected = true;
                            }
                        }
                        PopulateDrawer2(ddCabinet2.SelectedValue);

                        for (int i = 0; i < ddDrawer2.Items.Count; i++)
                        {
                            if (ddDrawer2.Items[i].Value == ds02.Tables[0].Rows[0][1].ToString())
                            {
                                ddDrawer2.ClearSelection();
                                ddDrawer2.Items[i].Selected = true;
                            }
                        }
                        PopulateFolder2(ddDrawer2.SelectedValue);

                        for (int i = 0; i < ddFolder2.Items.Count; i++)
                        {
                            if (ddFolder2.Items[i].Value == ds01.Tables[0].Rows[0][5].ToString())
                            {
                                ddFolder2.ClearSelection();
                                ddFolder2.Items[i].Selected = true;
                            }
                        }
                    }
                }
                #endregion
                #region POSTEMAIL or PREEMAIL
                else if (TaskID == "POSTEMAIL" || TaskID == "PREEMAIL")
                {
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.WFTaskDetails(Convert.ToInt32(WFID), Convert.ToInt32(StepNo), TaskID);
                    txtToMail.Text=ds01.Tables[0].Rows[0][5].ToString();
                    txtAmbleSub.Text=ds01.Tables[0].Rows[0][10].ToString();
                    txtAmbleMsg.Text=ds01.Tables[0].Rows[0][6].ToString();
                    
                    if (ds01.Tables[0].Rows[0][7].ToString() == "Yes")
                    {
                        ddAttachMail.ClearSelection();
                        ddAttachMail.Items[0].Selected = true;
                    }
                    else if (ds01.Tables[0].Rows[0][7].ToString() == "No")
                    {
                        ddAttachMail.ClearSelection();
                        ddAttachMail.Items[1].Selected = true;
                    }

                    if (ds01.Tables[0].Rows[0][9].ToString() == "Yes")
                    {
                        ddAmbleURL.ClearSelection();
                        ddAmbleURL.Items[0].Selected = true;
                    }
                    else if (ds01.Tables[0].Rows[0][9].ToString() == "No")
                    {
                        ddAmbleURL.ClearSelection();
                        ddAmbleURL.Items[1].Selected = true;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void cmdUpdateTaskSettings_Click(object sender, EventArgs e)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = "";
                
                if (Session["TaskID"].ToString() == "POSTAPPEND")
                {
                    result = ObjClassStoreProc.UpdateWFTasks(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), "POSTAPPEND","","","","",ddDocument3.SelectedValue,"","");
                }
                else if (Session["TaskID"].ToString() == "PREAPPEND")
                {
                    result = ObjClassStoreProc.UpdateWFTasks(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), "PREAPPEND", "", "", "", "", ddDocument3.SelectedValue, "", "");
                }
                else if (Session["TaskID"].ToString() == "POSTCOND")
                {
                    result = ObjClassStoreProc.UpdateWFCond(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), "POSTCOND", txtFormFieldNo.Text.Trim(),ddCondOp.SelectedValue,txtCondVal.Text.Trim(), txtToMail1.Text.Trim(), txtCondMsg.Text.Trim(), ddAttachCondMail.SelectedValue, ddCondURL.SelectedValue, txtCondSub.Text.Trim());
                }
                else if (Session["TaskID"].ToString() == "PRECOND")
                {
                    result = ObjClassStoreProc.UpdateWFCond(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), "PRECOND", txtFormFieldNo.Text.Trim(), ddCondOp.SelectedValue, txtCondVal.Text.Trim(), txtToMail1.Text.Trim(), txtCondMsg.Text.Trim(), ddAttachCondMail.SelectedValue, ddCondURL.SelectedValue, txtCondSub.Text.Trim());
                }
                else if (Session["TaskID"].ToString() == "POSTCOPY")
                {
                    result = ObjClassStoreProc.UpdateWFTasks(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), "POSTCOPY", ddFolder2.SelectedValue, "", "", "","", "", "");
                }
                else if (Session["TaskID"].ToString() == "PRECOPY")
                {
                    result = ObjClassStoreProc.UpdateWFTasks(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), "PRECOPY", ddFolder2.SelectedValue, "", "", "", "", "", "");
                }
                else if (Session["TaskID"].ToString() == "POSTEMAIL")
                {
                    result = ObjClassStoreProc.UpdateWFTasks(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), "POSTEMAIL", "", txtToMail.Text.Trim(), txtAmbleMsg.Text.Trim(), ddAttachMail.SelectedValue, "", ddAmbleURL.SelectedValue, txtAmbleSub.Text.Trim());
                }
                else if (Session["TaskID"].ToString() == "PREEMAIL")
                {
                    result = ObjClassStoreProc.UpdateWFTasks(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), "PREEMAIL", "", txtToMail.Text.Trim(), txtAmbleMsg.Text.Trim(), ddAttachMail.SelectedValue, "", ddAmbleURL.SelectedValue, txtAmbleSub.Text.Trim());
                }
                divCopyLocation.Visible = false;
                divAmbleMail.Visible = false;
                divCondMail.Visible = false;
                divAppendedDoc.Visible = false;
                divUpdateButton.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }

        protected void PopUser()
        {
            try
            {
                //....User
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectUserAll(Session["CompCode"].ToString());
                // For Amble Email in editing
                ddUser.DataSource = ds01;
                ddUser.DataTextField = "name";
                ddUser.DataValueField = "email";
                ddUser.DataBind();
                // For Cond Email in editing
                ddUser1.DataSource = ds01;
                ddUser1.DataTextField = "name";
                ddUser1.DataValueField = "email";
                ddUser1.DataBind();
                // For Amble Email in popup
                ddUserAmble.DataSource = ds01;
                ddUserAmble.DataTextField = "name";
                ddUserAmble.DataValueField = "email";
                ddUserAmble.DataBind();
                // For Cond Email in popup
                ddUser1Cond.DataSource = ds01;
                ddUser1Cond.DataTextField = "name";
                ddUser1Cond.DataValueField = "email";
                ddUser1Cond.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdAddList1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                gvWFTasks.DataSource = null;
                gvWFTasks.DataBind();
                divCopyLocation.Visible = false;
                divAmbleMail.Visible = false;
                divCondMail.Visible = false;
                divAppendedDoc.Visible = false;
                divUpdateButton.Visible = false;

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                wf_dtl_bal obj_typed = new wf_dtl_bal();
                if (Session["WFId"] != null)
                {
                    DataTable dt2 = null;
                    if (Session["dt2"] != null)
                    {
                        dt2 = (DataTable)Session["dt2"];
                    }
                    else
                    {
                        dt2 = obj_typed.CreateDTWFTask();
                    }

                    ds01.Reset();
                    ds01 = ObjClassStoreProc.MaxStepNoInWF(Session["WFId"].ToString());
                    int mMaxStepNo = Convert.ToInt32(ds01.Tables[0].Rows[0][0].ToString());
                    if ((Convert.ToInt32(txtStage1.Text.Trim()) <= 1) || (Convert.ToInt32(txtStage1.Text.Trim()) > mMaxStepNo + 1))
                    {
                        if ((Convert.ToInt32(txtStage1.Text.Trim()) == 1))
                        {
                            throw new Exception("1st Stage can not be inserted !!");
                        }
                        else
                        {
                            throw new Exception("Invalid Stage Number !!");
                        }
                    }

                    if (ddTask1.SelectedValue == "PREEMAIL" || ddTask1.SelectedValue == "POSTEMAIL")
                    {
                        if (Session["AmbleMailID"] == null)
                        {
                            throw new Exception("Please add the Email details for this task!!");
                        }
                        else if (Session["AmbleMailID"].ToString() == "")
                        {
                            throw new Exception("Please add the Email details for this task!!");
                        }
                    }
                    else if (ddTask1.SelectedValue == "PREAPPEND" || ddTask1.SelectedValue == "POSTAPPEND")
                    {
                        if (Session["AppendDoc"] == null)
                        {
                            throw new Exception("Please add the Document which needs to be appended!!");
                        }
                        else if (Session["AppendDoc"].ToString() == "")
                        {
                            throw new Exception("Please add the Document which needs to be appended!!");
                        }
                    }
                    else if (ddTask1.SelectedValue == "PRECOPY" || ddTask1.SelectedValue == "POSTCOPY")
                    {
                        if (Session["SelFldUUID"] == null)
                        {
                            throw new Exception("Please select the location where the Document will be copied!!");
                        }
                        else if (Session["SelFldUUID"].ToString() == "")
                        {
                            throw new Exception("Please select the location where the Document will be copied!!");
                        }
                    }

                    DataRow r2 = dt2.NewRow();
                    //Adding row in Data Table
                    obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                    obj_typed.StepNo = Convert.ToInt32(txtStage1.Text.Trim());//Convert.ToInt32(hdCounter.Value);
                    obj_typed.TaskID = ddTask1.SelectedValue;
                    obj_typed.ActTypeID = ddActType1.SelectedValue;
                    if (Session["SelFldUUID"] != null)
                    {
                        obj_typed.UUID = Session["SelFldUUID"].ToString();
                    }
                    else
                    {
                        obj_typed.UUID = "";
                    }
                    if (Session["AmbleMailID"] != null)
                    {
                        obj_typed.AmbleMail = Session["AmbleMailID"].ToString();
                    }
                    else
                    {
                        obj_typed.AmbleMail = "";
                    }
                    if (Session["AmbleSub"] != null)
                    {
                        obj_typed.AmbleSub = Session["AmbleSub"].ToString();
                    }
                    else
                    {
                        obj_typed.AmbleSub = "";
                    }
                    if (Session["AmbleMsg"] != null)
                    {
                        obj_typed.AmbleMsg = Session["AmbleMsg"].ToString();
                    }
                    else
                    {
                        obj_typed.AmbleMsg = "";
                    }
                    if (Session["AmbleAttach"] != null)
                    {
                        obj_typed.mail_attach = Session["AmbleAttach"].ToString();
                    }
                    else
                    {
                        obj_typed.mail_attach = "";
                    }
                    if (Session["AmbleURL"] != null)
                    {
                        obj_typed.mail_url = Session["AmbleURL"].ToString();
                    }
                    else
                    {
                        obj_typed.mail_url = "";
                    }
                    if (Session["AppendDoc"] != null)
                    {
                        obj_typed.AppendDoc = Session["AppendDoc"].ToString();
                    }
                    else
                    {
                        obj_typed.AppendDoc = "";
                    }

                    /// Checking for the task already entered for this stage or not
                    validation_bal obj_validation = new validation_bal();
                    if (!obj_validation.isDupTask(dt2, obj_typed.WFID, obj_typed.StepNo, obj_typed.TaskID, obj_typed.ActTypeID))
                    {
                        if (ddTask1.SelectedValue == "APPROVE" || ddTask1.SelectedValue == "REVIEW")
                        {
                            if (!obj_validation.isAlreadyThere(dt2, obj_typed.WFID, obj_typed.StepNo, obj_typed.TaskID))
                            {
                                /// Create a new row in the Data Table
                                dt2.Rows.Add(obj_typed.AddNewRecordTask(r2));
                                Session["dt2"] = dt2;
                                Session["SelFldUUID"] = "";
                                Session["AppendDoc"] = null;
                                lblTaskList1.Text += lblTaskList1.Text == "" ? ddTask1.SelectedItem.Text : ", " + ddTask1.SelectedItem.Text;
                                // Add the REJECT Task automatically
                                r2 = dt2.NewRow();
                                obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                                obj_typed.StepNo = Convert.ToInt32(txtStage1.Text.Trim());//Convert.ToInt32(hdCounter.Value);
                                obj_typed.TaskID = "REJECT";
                                obj_typed.ActTypeID = "Interactive";
                                obj_typed.UUID = "";
                                obj_typed.AmbleMail = "";
                                obj_typed.AmbleMsg = "";
                                obj_typed.mail_attach = "";
                                obj_typed.AppendDoc = "";
                                obj_typed.mail_url = "";
                                obj_typed.AmbleSub = "";
                                dt2.Rows.Add(obj_typed.AddNewRecordTask(r2));
                                Session["dt2"] = dt2;
                                Session["SelFldUUID"] = "";
                                Session["AppendDoc"] = null;
                                lblTaskList1.Text += lblTaskList1.Text == "" ? "Reject" : ", " + "Reject";
                            }
                            else
                            {
                                if (ddTask1.SelectedValue == "APPROVE")
                                {
                                    throw new Exception("Review is already assigned in this stage!!");
                                }
                                else if (ddTask1.SelectedValue == "REVIEW")
                                {
                                    throw new Exception("Approve is already assigned in this stage!!");
                                }
                            }
                        }
                        else
                        {
                            /// Create a new row in the Data Table
                            dt2.Rows.Add(obj_typed.AddNewRecordTask(r2));
                            Session["dt2"] = dt2;
                            Session["SelFldUUID"] = "";
                            Session["AppendDoc"] = null;
                            lblTaskList1.Text += lblTaskList1.Text == "" ? ddTask1.SelectedItem.Text : ", " + ddTask1.SelectedItem.Text;
                        }
                    }
                    else
                    {
                        throw new Exception("This task already assigned for this stage!");
                    }
                }
                else
                {
                    throw new Exception("At first add the Workflow and then proceed");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdAddDtl_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                gvWFTasks.DataSource = null;
                gvWFTasks.DataBind();
                divCopyLocation.Visible = false;
                divAmbleMail.Visible = false;
                divCondMail.Visible = false;
                divAppendedDoc.Visible = false;
                divUpdateButton.Visible = false;

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                wf_dtl_bal obj_typed = new wf_dtl_bal();
                if (Session["WFId"] != null)
                {
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.TotalStepNoInWF(Session["WFId"].ToString());
                    if(ds01.Tables[0].Rows.Count>=10)
                    {
                        throw new Exception("You can not define more than 10 stages in a single workflow !!");
                    }

                    DataTable dt001 = null;
                    dt001 = obj_typed.CreateDTWFDtl();

                    // Create a new row in the Data Table
                    DataRow r1 = dt001.NewRow();
                    //Adding row in Data Table
                    obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                    obj_typed.StepNo = Convert.ToInt32(txtStage1.Text.Trim());//Convert.ToInt32(hdCounter.Value);
                    obj_typed.RoleId = ddRole1.SelectedValue;
                    obj_typed.Duration = txtTime.Text.Trim();
                    dt001.Rows.Add(obj_typed.AddNewRecordDtl(r1));
                    Session["dt1"] = dt001;

                    Session["DisplayNo"] = 1;
                    BindGvDtl(true);
                    
                    AddStageDetails();
                    PopWFDetails();
                    FetchWFMasterData(Session["WFId"].ToString());
                    txtToMail1.Text = "";
                    txtCondMsg.Text = "";
                    txtCondVal.Text = "";
                    lblTaskList1.Text = "";
                }
                else
                {
                    MessageBox("Stage updation Error !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private void BindGvDtl(bool newRow)
        {
            wf_dtl_bal obj_typed = new wf_dtl_bal();
            DataTable dt3 = null;
            if (Session["dt3"] != null)
            {
                dt3 = (DataTable)Session["dt3"];
            }
            else
            {
                dt3 = obj_typed.CreateDTWFDisplay();
            }

            if (newRow)
            {
                DataRow r3 = dt3.NewRow();
                //Adding row in Data Table
                obj_typed.StepNo = Convert.ToInt32(txtStage1.Text.Trim());//Convert.ToInt32(hdCounter.Value);
                obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                obj_typed.WFName = txtWFName.Text.Trim(); //Session["WFName"].ToString();
                obj_typed.RoleId = ddRole1.SelectedValue;
                obj_typed.RoleName = ddRole1.SelectedItem.Text;
                obj_typed.Duration = txtTime.Text.Trim();
                obj_typed.TaskList = lblTaskList1.Text;

                dt3.Rows.Add(obj_typed.AddNewRecordDisplay(r3));
            }
            Session["dt3"] = dt3;
        }

        protected void AddStageDetails()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                int myCounter = 1;
                myCounter = Convert.ToInt32(txtStage1.Text.Trim());
                if (Session["WFId"] != null)
                {
                    if (Convert.ToInt16(txtStage1.Text) <= 1)
                    {
                        throw new Exception("Invalid Stage No.");
                    }

                    // need to alter the stages
                    result = ObjClassStoreProc.WFStageAlteration(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(txtStage1.Text.Trim()));

                    wf_task_bal wfTaskObj = new wf_task_bal();
                    wf_dtl_bal obj_typed = new wf_dtl_bal();
                    DataTable dt1 = null;
                    if (Session["dt1"] != null)
                    {
                        dt1 = (DataTable)Session["dt1"];
                    }
                    else
                    {
                        dt1 = obj_typed.CreateDTWFDtl();
                    }

                    int[,] WFDtlCntr = new int[dt1.Rows.Count, 2];
                    int i = 0;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        result = ObjClassStoreProc.InsertWFDtl(Convert.ToInt32(dr["wf_id"]), myCounter, dr["role_id"].ToString(), dr["duration"].ToString(), Session["CompCode"].ToString());
                        if (!string.IsNullOrEmpty(result))
                        {

                        }
                        else
                        {
                            Session["WFId"] = DBNull.Value;
                            throw new Exception("Data Insertion Error in Detail Part!");
                        }
                    }

                    // Conditional Part Start
                    DataTable dt03 = null;
                    if (Session["dt03"] != null)
                    {
                        dt03 = (DataTable)Session["dt03"];
                    }
                    else
                    {
                        dt03 = obj_typed.CreateDTCondDisplay();
                    }
                    for (int p = 0; p < dt03.Rows.Count; p++)
                    {
                        result = ObjClassStoreProc.InsertWFCond(Convert.ToInt32(dt03.Rows[p][0].ToString()), Convert.ToInt32(dt03.Rows[p][1].ToString()), dt03.Rows[p][2].ToString(), Convert.ToInt32(dt03.Rows[p][3].ToString()), dt03.Rows[p][4].ToString(), dt03.Rows[p][5].ToString(), dt03.Rows[p][6].ToString(), dt03.Rows[p][7].ToString(), dt03.Rows[p][8].ToString(), dt03.Rows[p][9].ToString(), dt03.Rows[p][10].ToString(), Session["CompCode"].ToString());
                    }
                    // Conditional Part End

                    DataTable dt2 = null;
                    if (Session["dt2"] != null)
                    {
                        dt2 = (DataTable)Session["dt2"];
                    }
                    else
                    {
                        dt2 = obj_typed.CreateDTWFTask();
                    }
                    i = 0;
                    foreach (DataRow dr in dt2.Rows)
                    {
                        result = ObjClassStoreProc.InsertWFTask(Convert.ToInt32(dr["wf_id"]), myCounter, dr["task_id"].ToString(), dr["acttype_id"].ToString(), dr["copy_to_uuid"].ToString(), dr["amble_mail"].ToString(), dr["amble_msg"].ToString(), dr["amble_attach"].ToString(), dr["AppendDoc"].ToString(), dr["amble_url"].ToString(), dr["AmbleSub"].ToString(), Session["CompCode"].ToString());

                        if (!string.IsNullOrEmpty(result))
                        {
                            i++;
                        }
                        else
                        {
                            Session["WFId"] = DBNull.Value;
                            throw new Exception("Data Insertion Error in Task Part!");
                        }
                    }
                    txtWFName.Text = "";
                    PopulateDropdown();
                    txtStage1.Text = "2";
                    txtTime.Text = "0";
                    Session["SelFldUUID"] = "";
                    Session["AmbleMailID"] = null;
                    Session["AmbleSub"] = null;
                    Session["AmbleMsg"] = null;
                    Session["AmbleAttach"] = null;
                    Session["AmbleURL"] = null;
                    Session["AppendDoc"] = null;
                    txtToMail1.Text = "";
                    txtCondSub.Text = "";
                    txtAmbleSub.Text = "";
                    txtCondMsg.Text = "";
                    txtCondVal.Text = "";
                    Session["dt1"] = null;
                    Session["dt2"] = null;
                    Session["dt3"] = null;
                    Session["dt03"] = null;
                    Session["dt05"] = null;
                    throw new Exception("Workflow updated successfully");
                }
                else
                {
                    throw new Exception("Updation Error !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddCabinet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
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
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateFolder(ddDrawer.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddRole1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            SetRoleBasedSettings();
        }

        protected void SetRoleBasedSettings()
        {
            try
            {
                if (ddRole1.SelectedValue == "INIT")
                {
                    txtTime.Text = "0";
                }
                else
                {
                    txtTime.Text = "168";
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddTask1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            SetTaskBasedSettings();
        }

        protected void SetTaskBasedSettings()
        {
            try
            {
                if (ddTask1.SelectedValue == "POSTAPPEND")
                {
                    ddActType1.SelectedValue = "Postamble";
                    cmdAddCopyLoc.Visible = false;
                    cmdAddAmbleMail.Visible = false;
                    cmdAddCondMail.Visible = false;
                    cmdAppendDoc.Visible = true;
                    cmdSign.Visible = false;
                }
                else if (ddTask1.SelectedValue == "PREAPPEND")
                {
                    ddActType1.SelectedValue = "Preamble";
                    cmdAddCopyLoc.Visible = false;
                    cmdAddAmbleMail.Visible = false;
                    cmdAddCondMail.Visible = false;
                    cmdAppendDoc.Visible = true;
                    cmdSign.Visible = false;
                }
                else if (ddTask1.SelectedValue == "POSTCOND")
                {
                    ddActType1.SelectedValue = "Postamble";
                    cmdAddCopyLoc.Visible = false;
                    cmdAddAmbleMail.Visible = false;
                    cmdAddCondMail.Visible = true;
                    cmdAppendDoc.Visible = false;
                    cmdSign.Visible = false;
                }
                else if (ddTask1.SelectedValue == "PRECOND")
                {
                    ddActType1.SelectedValue = "Preamble";
                    cmdAddCopyLoc.Visible = false;
                    cmdAddAmbleMail.Visible = false;
                    cmdAddCondMail.Visible = true;
                    cmdAppendDoc.Visible = false;
                    cmdSign.Visible = false;
                }
                else if (ddTask1.SelectedValue == "POSTCOPY")
                {
                    ddActType1.SelectedValue = "Postamble";
                    cmdAddCopyLoc.Visible = true;
                    cmdAddAmbleMail.Visible = false;
                    cmdAddCondMail.Visible = false;
                    cmdAppendDoc.Visible = false;
                    cmdSign.Visible = false;
                }
                else if (ddTask1.SelectedValue == "PRECOPY")
                {
                    ddActType1.SelectedValue = "Preamble";
                    cmdAddCopyLoc.Visible = true;
                    cmdAddAmbleMail.Visible = false;
                    cmdAddCondMail.Visible = false;
                    cmdAppendDoc.Visible = false;
                    cmdSign.Visible = false;
                }
                else if (ddTask1.SelectedValue == "POSTEMAIL")
                {
                    ddActType1.SelectedValue = "Postamble";
                    cmdAddCopyLoc.Visible = false;
                    cmdAddAmbleMail.Visible = true;
                    cmdAddCondMail.Visible = false;
                    cmdAppendDoc.Visible = false;
                    cmdSign.Visible = false;
                }
                else if (ddTask1.SelectedValue == "PREEMAIL")
                {
                    ddActType1.SelectedValue = "Preamble";
                    cmdAddCopyLoc.Visible = false;
                    cmdAddAmbleMail.Visible = true;
                    cmdAddCondMail.Visible = false;
                    cmdAppendDoc.Visible = false;
                    cmdSign.Visible = false;
                }
                else if (ddTask1.SelectedValue == "POSTSOFTCP")
                {
                    ddActType1.SelectedValue = "Postamble";
                    cmdAddCopyLoc.Visible = false;
                    cmdAddAmbleMail.Visible = false;
                    cmdAddCondMail.Visible = false;
                    cmdAppendDoc.Visible = false;
                    cmdSign.Visible = false;
                }
                else
                {
                    ddActType1.SelectedValue = "Interactive";
                    cmdAddCopyLoc.Visible = false;
                    cmdAddAmbleMail.Visible = false;
                    cmdAddCondMail.Visible = false;
                    cmdAppendDoc.Visible = false;
                    cmdSign.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        #region In popup, Amble Email feature
        protected void cmdAmbMailSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                if (txtToMailAmble.Text.Trim() == "")
                {
                    throw new Exception("Please Enter Email ID");
                }
                else if (txtAmbleSubAmble.Text.Trim() == "")
                {
                    throw new Exception("Please Enter Email Subject");
                }
                else if (txtAmbleMsgAmble.Text.Trim() == "")
                {
                    throw new Exception("Please Enter Email Message");
                }
                else
                {
                    Session["AmbleMailID"] = txtToMailAmble.Text.Trim();
                    Session["AmbleSub"] = txtAmbleSubAmble.Text.Trim();
                    Session["AmbleMsg"] = txtAmbleMsgAmble.Text.Trim();
                    Session["AmbleAttach"] = ddAttachMailAmble.SelectedValue;
                    Session["AmbleURL"] = ddAmbleURLAmble.SelectedValue;
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void cmdAmbMailCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                Session["AmbleMailID"] = null;
                Session["AmbleSub"] = null;
                Session["AmbleMsg"] = null;
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion
        
        #region In popup, Conditional Email feature > Edit Update in the Gridview
        protected void cmdAddCondCond_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                if (txtToMail1Cond.Text.Trim() == "")
                {
                    throw new Exception("Please Enter Email ID");
                }
                else if (txtCondMsgCond.Text.Trim() == "")
                {
                    throw new Exception("Please Enter Email Message");
                }
                else
                {
                    BindGvCondInPopup(true);
                    txtCondValCond.Text = "";
                    txtToMail1Cond.Text = "";
                    txtCondMsgCond.Text = "";
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void gvCondCond_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                GridViewRow row = (GridViewRow)gvCondCond.Rows[e.RowIndex];
                Label lbAutoID1 = (Label)row.FindControl("lbAutoID1");
                Label lbAutoID2 = (Label)row.FindControl("lbAutoID2");
                Label lbAutoID3 = (Label)row.FindControl("lbAutoID3");
                Label lbAutoID4 = (Label)row.FindControl("lbAutoID4");
                Label lbAutoID5 = (Label)row.FindControl("lbAutoID5");
                Label lbAutoID6 = (Label)row.FindControl("lbAutoID6");
                Label lbAutoID7 = (Label)row.FindControl("lbAutoID7");
                Label lbAutoID8 = (Label)row.FindControl("lbAutoID8");

                DataTable dt03 = null;
                DataRow r3 = null;
                dt03 = (DataTable)Session["dt03"];
                foreach (DataRow row1 in dt03.Rows)
                {
                    if ((row1[0].ToString() == lbAutoID1.Text) && (row1[1].ToString() == lbAutoID2.Text) && (row1[2].ToString() == lbAutoID3.Text) && (row1[3].ToString() == lbAutoID4.Text) && (row1[4].ToString() == lbAutoID5.Text) && (row1[5].ToString() == lbAutoID6.Text) && (row1[6].ToString() == lbAutoID7.Text) && (row1[7].ToString() == lbAutoID8.Text))
                    {
                        r3 = row1;
                        dt03.Rows.Remove(r3);
                        break;
                    }
                }
                gvCondCond.EditIndex = -1;
                Session["dt03"] = dt03;
                BindGvCond(false);
                throw new Exception("Data Deleted Successfully !!");
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        private void BindGvCond(bool newRow)
        {
            try
            {
                wf_dtl_bal obj_typed = new wf_dtl_bal();
                DataTable dt03 = null;
                if (Session["dt03"] != null)
                {
                    dt03 = (DataTable)Session["dt03"];
                }
                else
                {
                    dt03 = obj_typed.CreateDTCondDisplay();
                }

                if (newRow)
                {
                    DataRow r3 = dt03.NewRow();
                    obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                    obj_typed.StepNo = Convert.ToInt32(txtStage1.Text.Trim());
                    obj_typed.TaskID = ddTask1.SelectedValue;
                    obj_typed.FormFieldNo = Convert.ToInt32(txtFormFieldNo.Text.Trim());
                    obj_typed.cond_op = ddCondOp.SelectedValue;
                    obj_typed.cond_val = txtCondVal.Text.Trim();
                    obj_typed.mail_to = txtToMail1.Text.Trim();
                    obj_typed.mail_msg = txtCondMsg.Text.Trim();
                    obj_typed.mail_attach = ddAttachCondMail.SelectedValue;
                    obj_typed.mail_url = ddCondURL.SelectedValue;
                    obj_typed.CondMailSub = txtCondSub.Text.Trim();
                    dt03.Rows.Add(obj_typed.AddNewCondDisplay(r3));
                }
                Session["dt03"] = dt03;
                gvCondCond.DataSource = dt03;
                gvCondCond.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private void BindGvCondInPopup(bool newRow)
        {
            try
            {
                wf_dtl_bal obj_typed = new wf_dtl_bal();
                DataTable dt03 = null;
                if (Session["dt03"] != null)
                {
                    dt03 = (DataTable)Session["dt03"];
                }
                else
                {
                    dt03 = obj_typed.CreateDTCondDisplay();
                }

                if (newRow)
                {
                    DataRow r3 = dt03.NewRow();
                    obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                    obj_typed.StepNo = Convert.ToInt32(txtStage1.Text.Trim());
                    obj_typed.TaskID = ddTask1.SelectedValue;
                    obj_typed.FormFieldNo = Convert.ToInt32(txtFormFieldNoCond.Text.Trim());
                    obj_typed.cond_op = ddCondOpCond.SelectedValue;
                    obj_typed.cond_val = txtCondValCond.Text.Trim();
                    obj_typed.mail_to = txtToMail1Cond.Text.Trim();
                    obj_typed.mail_msg = txtCondMsgCond.Text.Trim();
                    obj_typed.mail_attach = ddAttachCondMailCond.SelectedValue;
                    obj_typed.mail_url = ddCondURLCond.SelectedValue;
                    obj_typed.CondMailSub = txtCondSubCond.Text.Trim();
                    dt03.Rows.Add(obj_typed.AddNewCondDisplay(r3));
                }
                Session["dt03"] = dt03;
                gvCondCond.DataSource = dt03;
                gvCondCond.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdCondMailSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                Session["AmbleMailID"] = txtToMail1Cond.Text.Trim();
                Session["AmbleSub"] = txtCondSubCond.Text.Trim();
                Session["AmbleMsg"] = txtCondMsgCond.Text.Trim();
                Session["AmbleAttach"] = ddAttachCondMailCond.SelectedValue;
                Session["AmbleURL"] = ddCondURLCond.SelectedValue;
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void cmdCondMailCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                DataTable dt03 = null;
                DataRow r3 = null;
                dt03 = (DataTable)Session["dt03"];
                foreach (DataRow row1 in dt03.Rows)
                {
                    if (row1[1].ToString() == txtStage1.Text)
                    {
                        r3 = row1;
                        dt03.Rows.Remove(r3);
                        gvCondCond.EditIndex = -1;
                        Session["dt03"] = dt03;
                        BindGvCondInPopup(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion

        #region In Editing, Conditional Email feature > Edit Update in the Gridview
        protected void cmdAddCond_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), Session["TaskID"].ToString());
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                if (txtToMail1.Text.Trim() == "")
                {
                    throw new Exception("Please Enter Email ID");
                }
                else if (txtCondMsg.Text.Trim() == "")
                {
                    throw new Exception("Please Enter Email Message");
                }
                else
                {
                    //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    string result = "";
                    result = ObjClassStoreProc.InsertWFCond(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(Session["StepNo"].ToString()), Session["TaskID"].ToString(), Convert.ToInt32(txtFormFieldNo.Text.Trim()), ddCondOp.SelectedValue, txtCondVal.Text.Trim(), txtToMail1.Text, txtCondMsg.Text.Trim(), ddAttachCondMail.SelectedValue, ddCondURL.SelectedValue, txtCondSub.Text.Trim(), Session["CompCode"].ToString());
                    PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), Session["TaskID"].ToString());
                    txtCondVal.Text = "";
                    txtToMail1.Text = "";
                    txtCondMsg.Text = "";
                    throw new Exception("Data Inserted Successfully !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }
        
        protected void gvCond_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            GridViewRow row = gvCond.SelectedRow;
        }

        protected void gvCond_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                gvCond.EditIndex = e.NewEditIndex;
                String mTaskID = gvCond.DataKeys[e.NewEditIndex].Values["task_id"].ToString();
                PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), mTaskID);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvCond_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    gvCond.EditIndex = -1;
                    PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), Session["TaskID"].ToString());
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                int rIndex = e.RowIndex;
                GridViewRow row = (GridViewRow)gvCond.Rows[e.RowIndex];

                Label lblWFID = (Label)row.FindControl("lbAutoID1");
                Label lblStepNo = (Label)row.FindControl("lbAutoID2");
                Label lblTaskID = (Label)row.FindControl("lbAutoID3");
                Label lblFormFieldNo = (Label)row.FindControl("lbAutoID4");
                Label lblCondOp = (Label)row.FindControl("lbAutoID5");
                Label lblCondVal = (Label)row.FindControl("lbAutoID6");
                TextBox lblCondAmbleSub = (TextBox)row.FindControl("txtEditCondAmbleSub");
                TextBox lblCondAmbleMails = (TextBox)row.FindControl("txtEditCondAmbleMails");
                TextBox lblCondAmbleMsg = (TextBox)row.FindControl("txtEditCondAmbleMsg");
                DropDownList ddlEditCondAttachment = (DropDownList)row.FindControl("ddlEditCondAttachment");
                DropDownList ddlEditCondURL = (DropDownList)row.FindControl("ddlEditCondURL");

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = "";
                result = ObjClassStoreProc.UpdateWFCond(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(lblStepNo.Text), lblTaskID.Text, lblFormFieldNo.Text, lblCondOp.Text, lblCondVal.Text, lblCondAmbleMails.Text, lblCondAmbleMsg.Text, ddlEditCondAttachment.SelectedValue, ddlEditCondURL.SelectedValue, lblCondAmbleSub.Text);
                gvCond.EditIndex = -1;
                PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), Session["TaskID"].ToString());
                if (result == "1")
                {
                    throw new Exception("Data Updated Successfully !!");
                }
                else
                {
                    throw new Exception("Updation Error !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void gvCond_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                gvCond.EditIndex = -1;
                PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), Session["TaskID"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvCond_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // For Cond Attachment Dropdown
                Control ctrl01 = e.Row.FindControl("hdCondAttachment");
                if (ctrl01 != null)
                {
                    DropDownList ddlEditCondAttachment = ctrl01 as DropDownList;
                    Control ctrl02 = e.Row.FindControl("hdCondAttachment");
                    if (ctrl02 != null)
                    {
                        HiddenField hdCondAttachment = ctrl02 as HiddenField;
                        if (hdCondAttachment.Value == "Yes")
                        {
                            ddlEditCondAttachment.ClearSelection();
                            ddlEditCondAttachment.Items[0].Selected = true;
                        }
                        else if (hdCondAttachment.Value == "No")
                        {
                            ddlEditCondAttachment.ClearSelection();
                            ddlEditCondAttachment.Items[1].Selected = true;
                        }
                    }
                }
                // For Cond URL Dropdown
                Control ctrl03 = e.Row.FindControl("hdCondURL");
                if (ctrl03 != null)
                {
                    DropDownList ddlEditCondURL = ctrl03 as DropDownList;
                    Control ctrl04 = e.Row.FindControl("hdCondURL");
                    if (ctrl04 != null)
                    {
                        HiddenField hdCondURL = ctrl04 as HiddenField;
                        if (hdCondURL.Value == "Yes")
                        {
                            ddlEditCondURL.ClearSelection();
                            ddlEditCondURL.Items[0].Selected = true;
                        }
                        else if (hdCondURL.Value == "No")
                        {
                            ddlEditCondURL.ClearSelection();
                            ddlEditCondURL.Items[1].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvCond_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds_01 = new DataSet();
                ds_01 = ObjClassStoreProc.WFRecords(Request.QueryString["WFId"].ToString());
                if (ds_01.Tables[0].Rows.Count > 0)
                {
                    gvCond.EditIndex = -1;
                    PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), Session["TaskID"].ToString());
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }

                GridViewRow row = (GridViewRow)gvCond.Rows[e.RowIndex];
                Label lblWFID = (Label)row.FindControl("lbAutoID1");
                Label lblStepNo = (Label)row.FindControl("lbAutoID2");
                Label lblTaskID = (Label)row.FindControl("lbAutoID3");
                Label lblFormFieldNo = (Label)row.FindControl("lbAutoID4");
                Label lblCondOp = (Label)row.FindControl("lbAutoID5");
                Label lblCondVal = (Label)row.FindControl("lbAutoID6");

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = "";
                result = ObjClassStoreProc.DeleteWFCond(Convert.ToInt32(Session["WFId"].ToString()), Convert.ToInt32(lblStepNo.Text), lblTaskID.Text, Convert.ToInt32(lblFormFieldNo.Text), lblCondOp.Text, lblCondVal.Text);
                if (result == "1")
                {
                    gvCond.EditIndex = -1;
                    PopSettings(Session["WFId"].ToString(), Session["StepNo"].ToString(), Session["TaskID"].ToString());
                    throw new Exception("Data Deleted Successfully !!");
                }
                else
                {
                    throw new Exception("Error in Data Deletion !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }
        #endregion

        #region In Editing, Copy feature > Populating Cabinet, Drawer & Folder
        protected void PopulateCabinet2()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinet2.DataSource = ds01;
                ddCabinet2.DataTextField = "cab_name";
                ddCabinet2.DataValueField = "cab_uuid";
                ddCabinet2.DataBind();
                PopulateDrawer2(ddCabinet2.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDrawer2(string SelCab)
        {
            try
            {
                //....Drawer
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCab, Session["UserID"].ToString());
                ddDrawer2.DataSource = ds01;
                ddDrawer2.DataTextField = "drw_name";
                ddDrawer2.DataValueField = "drw_uuid";
                ddDrawer2.DataBind();
                if (ddDrawer2.SelectedValue != "")
                {
                    PopulateFolder2(ddDrawer2.SelectedValue);
                }
                else
                {
                    PopulateFolder2("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateFolder2(string SelDrw)
        {
            try
            {
                //....Folder
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrw, Session["UserID"].ToString());
                ddFolder2.DataSource = ds01;
                ddFolder2.DataTextField = "fld_name";
                ddFolder2.DataValueField = "fld_uuid";
                ddFolder2.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ddCabinet2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateDrawer2(ddCabinet2.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawer2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateFolder2(ddDrawer2.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion

        #region In popup, Copy feature > Populating Cabinet, Drawer & Folder
        protected void PopulateCabinetCopy()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinetCopy.DataSource = ds01;
                ddCabinetCopy.DataTextField = "cab_name";
                ddCabinetCopy.DataValueField = "cab_uuid";
                ddCabinetCopy.DataBind();
                PopulateDrawerCopy(ddCabinetCopy.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDrawerCopy(string SelCab)
        {
            try
            {
                //....Drawer
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCab, Session["UserID"].ToString());
                ddDrawerCopy.DataSource = ds01;
                ddDrawerCopy.DataTextField = "drw_name";
                ddDrawerCopy.DataValueField = "drw_uuid";
                ddDrawerCopy.DataBind();
                if (ddDrawerCopy.SelectedValue != "")
                {
                    PopulateFolderCopy(ddDrawerCopy.SelectedValue);
                }
                else
                {
                    PopulateFolderCopy("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateFolderCopy(string SelDrw)
        {
            try
            {
                //....Folder
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrw, Session["UserID"].ToString());
                ddFolderCopy.DataSource = ds01;
                ddFolderCopy.DataTextField = "fld_name";
                ddFolderCopy.DataValueField = "fld_uuid";
                ddFolderCopy.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ddCabinetCopy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateDrawerCopy(ddCabinetCopy.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawerCopy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateFolderCopy(ddDrawerCopy.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdLocClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                Session["SelCab"] = "";
                Session["SelDrw"] = "";
                Session["SelFld"] = "";
                Session["SelCab"] = ddCabinetCopy.SelectedValue;
                Session["SelDrw"] = ddDrawerCopy.SelectedValue;
                Session["SelFld"] = ddFolderCopy.SelectedValue;

                if (Session["SelFld"].ToString() == "")
                {
                    throw new Exception("Please Select the Folder Location Where the Document will be Saved!!");
                }
                else
                {
                    Session["SelFldUUID"] = Session["SelFld"].ToString();
                    Session["SelCab"] = "";
                    Session["SelDrw"] = "";
                    Session["SelFld"] = "";
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }
        #endregion

        #region In Editing, Append feature > Populating Cabinet, Drawer, Folder & Document
        protected void PopulateCabinet3()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinet3.DataSource = ds01;
                ddCabinet3.DataTextField = "cab_name";
                ddCabinet3.DataValueField = "cab_uuid";
                ddCabinet3.DataBind();
                PopulateDrawer3(ddCabinet3.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDrawer3(string SelCab)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCab, Session["UserID"].ToString());
                ddDrawer3.DataSource = ds01;
                ddDrawer3.DataTextField = "drw_name";
                ddDrawer3.DataValueField = "drw_uuid";
                ddDrawer3.DataBind();
                if (ddDrawer3.SelectedValue != "")
                {
                    PopulateFolder3(ddDrawer3.SelectedValue);
                }
                else
                {
                    PopulateFolder3("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateFolder3(string SelDrw)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrw, Session["UserID"].ToString());
                ddFolder3.DataSource = ds01;
                ddFolder3.DataTextField = "fld_name";
                ddFolder3.DataValueField = "fld_uuid";
                ddFolder3.DataBind();
                if (ddFolder3.SelectedValue != "")
                {
                    PopulateDoc3(ddFolder3.SelectedValue);
                }
                else
                {
                    PopulateDoc3("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateDoc3(string SelFld)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.PDFDocBasedOnFolder(SelFld);
                ddDocument3.DataSource = ds01;
                ddDocument3.DataTextField = "doc_name";
                ddDocument3.DataValueField = "uuid";
                ddDocument3.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ddCabinet3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateDrawer3(ddCabinet3.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawer3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateFolder3(ddDrawer3.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddFolder3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateDoc3(ddFolder3.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        #endregion

        #region In popup, Append feature > Populating Cabinet, Drawer, Folder & Document
        protected void PopulateCabinetAppend()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinetAppend.DataSource = ds01;
                ddCabinetAppend.DataTextField = "cab_name";
                ddCabinetAppend.DataValueField = "cab_uuid";
                ddCabinetAppend.DataBind();
                PopulateDrawerAppend(ddCabinetAppend.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDrawerAppend(string SelCab)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCab, Session["UserID"].ToString());
                ddDrawerAppend.DataSource = ds01;
                ddDrawerAppend.DataTextField = "drw_name";
                ddDrawerAppend.DataValueField = "drw_uuid";
                ddDrawerAppend.DataBind();
                if (ddDrawerAppend.SelectedValue != "")
                {
                    PopulateFolderAppend(ddDrawerAppend.SelectedValue);
                }
                else
                {
                    PopulateFolderAppend("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateFolderAppend(string SelDrw)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrw, Session["UserID"].ToString());
                ddFolderAppend.DataSource = ds01;
                ddFolderAppend.DataTextField = "fld_name";
                ddFolderAppend.DataValueField = "fld_uuid";
                ddFolderAppend.DataBind();
                if (ddFolderAppend.SelectedValue != "")
                {
                    PopulateDocAppend(ddFolderAppend.SelectedValue);
                }
                else
                {
                    PopulateDocAppend("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateDocAppend(string SelFld)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.PDFDocBasedOnFolder(SelFld);
                ddDocumentAppend.DataSource = ds01;
                ddDocumentAppend.DataTextField = "doc_name";
                ddDocumentAppend.DataValueField = "uuid";
                ddDocumentAppend.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ddCabinetAppend_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateDrawerAppend(ddCabinetAppend.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawerAppend_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateFolderAppend(ddDrawerAppend.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddFolderAppend_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateDocAppend(ddFolderAppend.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdAppendDocSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                if (ddDocumentAppend.SelectedValue == null)
                {
                    throw new Exception("Please Select the Document to be Appended");
                }
                else
                {
                    Session["AppendDoc"] = ddDocumentAppend.SelectedValue;
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void cmdAppendDocCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                Session["AppendDoc"] = null;
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }
        #endregion

        protected void cmdSignDateSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                wf_dtl_bal obj_typed = new wf_dtl_bal();
                DataTable dt05 = null;
                if (Session["dt05"] != null)
                {
                    dt05 = (DataTable)Session["dt05"];
                }
                else
                {
                    dt05 = obj_typed.CreateDTSignDate();
                }

                DataRow r5 = dt05.NewRow();
                //Adding row in Data Table
                obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                obj_typed.StepNo = Convert.ToInt32(txtStage1.Text.Trim());
                dt05.Rows.Add(obj_typed.AddNewCondDisplay(r5, Convert.ToInt16(txtSign1.Text.Trim()), Convert.ToInt16(txtDate1.Text.Trim()), Convert.ToInt16(txtSign2.Text.Trim()), Convert.ToInt16(txtDate2.Text.Trim()), Convert.ToInt16(txtSign3.Text.Trim()), Convert.ToInt16(txtDate3.Text.Trim()), Convert.ToInt16(txtSign4.Text.Trim()), Convert.ToInt16(txtDate4.Text.Trim()), Convert.ToInt16(txtSign5.Text.Trim()), Convert.ToInt16(txtDate5.Text.Trim()), Convert.ToInt16(txtSign6.Text.Trim()), Convert.ToInt16(txtDate6.Text.Trim()), Convert.ToInt16(txtSign7.Text.Trim()), Convert.ToInt16(txtDate7.Text.Trim()), Convert.ToInt16(txtSign8.Text.Trim()), Convert.ToInt16(txtDate8.Text.Trim()), Convert.ToInt16(txtSign9.Text.Trim()), Convert.ToInt16(txtDate9.Text.Trim()), Convert.ToInt16(txtSign10.Text.Trim()), Convert.ToInt16(txtDate10.Text.Trim())));
                Session["dt05"] = dt05;
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void cmdSignDateCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }


    }
}