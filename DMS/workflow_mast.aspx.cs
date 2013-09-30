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
    public partial class workflow_mast : System.Web.UI.Page
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
                    if (Session["UserType"].ToString() == "S") // Super Admin
                    {
                        PopCompany();
                        divCompany.Visible = true;
                        divMenuSuperAdmin.Visible = true;
                        divMenuAdmin.Visible = false;
                        divMenuNormal.Visible = false;
                    }
                    else if (Session["UserType"].ToString() == "A") // Admin
                    {
                        PopCompany();
                        divCompany.Visible = false;
                        divMenuSuperAdmin.Visible = false;
                        divMenuAdmin.Visible = true;
                        divMenuNormal.Visible = false;
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                    lblUser.Text = Session["UserFullName"].ToString();
                    Page.Header.DataBind();
                    /// <Session["WFId"]> is used to store the last inserted Workflow ID and
                    /// <Session["WFName"]> is for last inserted Workflow Name
                    Session["WFId"] = null;
                    Session["WFName"] = null;
                    Session["dt1"] = null;
                    Session["dt2"] = null;
                    Session["dt3"] = null;
                    Session["dt03"] = null;
                    /// Clear the incomplete workflows
                    //ClearIncompleteWF();
                    /// To populate the dropdowns
                    PopulateDropdown();
                    /// To populate the gridview
                    PopulateGridView();
                    /// To Populate Cabinet dropdown for Copy to Location
                    PopulateCabinet2();
                    /// To Populate Cabinet dropdown for Append Doc
                    PopulateCabinet3();
                    /// To Populate User dropdown for Mail ID Settings
                    PopUser();
                }
                else
                {
                    Response.Redirect("logout.aspx", false);
                }
            }
        }

        protected void ddCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            PopulateDropdown();
            PopulateCabinet2();
            PopulateCabinet3();
            PopulateGridView();
        }

        protected void PopCompany()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //....Company
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCompActiveAll();
                ddCompany.DataSource = ds01;
                ddCompany.DataTextField = "CompName";
                ddCompany.DataValueField = "CompCode";
                ddCompany.DataBind();
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

                //....Role
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.RoleBasedOnCompCode(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.RoleBasedOnCompCode(Session["CompCode"].ToString());
                }
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

                //....Department
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.DeptAllBasedOnCompCode(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.DeptAllBasedOnCompCode(Session["CompCode"].ToString());
                }
                ddDept.DataSource = ds01;
                ddDept.DataTextField = "dept_name";
                ddDept.DataValueField = "dept_id";
                ddDept.DataBind();

                //....Doc Type
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectDocTypeCompBased(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectDocTypeCompBased(Session["CompCode"].ToString());
                }
                ddDocType.DataSource = ds01;
                ddDocType.DataTextField = "doc_type_name";
                ddDocType.DataValueField = "doc_type_id";
                ddDocType.DataBind();

                //....Cabinet
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(ddCompany.SelectedValue, Session["UserID"].ToString());
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                }
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

        /// <summary>
        /// As Drawer dropdown is dependent of Cabinet, so the Drawer dropdown is populated with respect to Cabinet
        /// </summary>
        /// <param name="SelCab"></param>
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

        /// <summary>
        /// As Folder dropdown is dependent of Drawer, so the Folder dropdown is populated with respect to Drawer
        /// </summary>
        /// <param name="SelDrw"></param>
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

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<WorkflowMast_GV>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateGridView()
        {
            try
            {
                //DBClass DBObj = new DBClass();
                //DataSet ds1 = new DataSet();
                //ds1 = DBObj.GVWFMast();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.WFLMastBasedOnCompCode(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.WFLMastBasedOnCompCode(Session["CompCode"].ToString());
                }
                gvDispRec.DataSource = ds01;
                gvDispRec.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Clear incomplete workflows with Store Procedure Name:<WF_Clear>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ClearIncompleteWF()
        {
            try
            {
                //DBClass DBObj = new DBClass();
                //DataSet ds1 = new DataSet();
                //string result = DBObj.ClearIncompleteWF();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = ObjClassStoreProc.ClearIncompleteWFBasedOnCompCode(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    result = ObjClassStoreProc.ClearIncompleteWFBasedOnCompCode(Session["CompCode"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        
        /// <summary>
        /// This is used to insert the <Workflow Name, Workflow Status> into the <wf_mast> table
        /// The <wf_id> field will be generated automatically.
        /// After generating this ID, we can map this ID with <wf_dtl>'s <wf_id> field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddMaster_Click(object sender, EventArgs e)
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

                if (ddCabinet.SelectedValue == "")
                {
                    throw new Exception("Please select a Cabinet !!");
                }
                else if (ddDrawer.SelectedValue == "")
                {
                    throw new Exception("Please select a Drawer !!");
                }
                else if (ddFolder.SelectedValue == "")
                {
                    throw new Exception("Please select a Folder !!");
                }

                wf_mast_bal wfMastObj = new wf_mast_bal();
                wfMastObj.WFName = txtWFName.Text.Trim();
                wfMastObj.WFDept = ddDept.SelectedValue;
                wfMastObj.WFDocType = ddDocType.SelectedValue;
                wfMastObj.WFFolderUUID = ddFolder.SelectedValue;

                //string result = wfMastObj.InsertWFMast();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = ObjClassStoreProc.InsertWFMast(txtWFName.Text.Trim(), ddDept.SelectedValue, ddDocType.SelectedValue,"A",ddFolder.SelectedValue, ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    result = ObjClassStoreProc.InsertWFMast(txtWFName.Text.Trim(), ddDept.SelectedValue, ddDocType.SelectedValue, "A", ddFolder.SelectedValue, Session["CompCode"].ToString());
                }
                if (Convert.ToInt64(result) == -1)
                {
                    Session["WFId"] = DBNull.Value;
                    throw new Exception("Workflow name already exists!");
                }
                else if (Convert.ToInt64(result) == -2)
                {
                    Session["WFId"] = DBNull.Value;
                    throw new Exception("Workflow already assigned for this document type & department combination!");
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    /// Store the Last inserted ID into <Session["WFId"]> and workflow name into <Session["WFName"]> start
                    Session["WFId"] = result;
                    Session["WFName"] = txtWFName.Text.Trim();
                    /// Store the Last inserted ID into <Session["WFId"]> and workflow name into <Session["WFName"]> end
                    Session["dt1"] = null;
                    Session["dt2"] = null;
                }

                lblHeading.Text = "Define the Workflow details for : " + "<span style=\"color:#660066;\">\"" + txtWFName.Text.ToString() + "\"</span>";
                /// The Hidden Field <hdCounter> is used to maintain the Auto Stage No and its default value set to 1
                hdCounter.Value = "1";
               
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To populate the cabinet dropdown (for copy location)
        /// </summary>
        protected void PopulateCabinet2()
        {
            try
            {
                //....Cabinet
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(ddCompany.SelectedValue, Session["UserID"].ToString());
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                }
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

        /// <summary>
        /// As Drawer dropdown is dependent of Cabinet, so the Drawer dropdown is populated with respect to Cabinet (for copy location)
        /// </summary>
        /// <param name="SelCab"></param>
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

        /// <summary>
        /// As Folder dropdown is dependent of Drawer, so the Folder dropdown is populated with respect to Drawer (for copy location)
        /// </summary>
        /// <param name="SelDrw"></param>
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

        protected void PopulateCabinet3()
        {
            try
            {
                //....Cabinet
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(ddCompany.SelectedValue, Session["UserID"].ToString());
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                }
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
                //....Drawer
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
                //....Folder
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
                Session["SelCab"] = ddCabinet2.SelectedValue;
                Session["SelDrw"] = ddDrawer2.SelectedValue;
                Session["SelFld"] = ddFolder2.SelectedValue;

                if (Session["SelFld"].ToString() == "")
                {
                    //throw new Exception("Please Select the Folder Location Where the Document will be Saved!!");
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

        /// <summary>
        /// This is defined to add multiple tasks for a particular stage (role)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    if (txtStage1.Text.Trim() == "1")
                    {
                        if (ddRole1.SelectedValue != "INIT")
                        {
                            throw new Exception("Please select Initiator Role in the first stage!!");
                        }
                        else
                        {
                            if (ddActType1.SelectedValue != "Postamble")
                            {
                                throw new Exception("You can add Postamble Actions only for Initiator!!");
                            }
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

                    DataTable dt1 = null;
                    if (Session["dt1"] != null)
                    {
                        dt1 = (DataTable)Session["dt1"];
                    }
                    else
                    {
                        dt1 = obj_typed.CreateDTWFDtl();
                    }

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (Convert.ToInt64(dt1.Rows[i]["wf_id"]) == Convert.ToInt64(Session["WFId"]) && dt1.Rows[i]["role_id"].ToString() == ddRole1.SelectedValue)
                        {
                            throw new Exception("This Role is already added in this Workflow !!");
                        }
                    }

                    DataRow r2 = dt2.NewRow();
                    //Adding row in Data Table
                    obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                    obj_typed.StepNo = Convert.ToInt32(hdCounter.Value);
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
                                obj_typed.StepNo = Convert.ToInt32(hdCounter.Value);
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
                        throw new Exception("This task already assigned in this stage!!");
                    }
                }
                else
                {
                    throw new Exception("At first add the Workflow and then proceed!!");
                }
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
         
        /// <summary>
        /// To bind the Gridview with the Datatable to display the different stages / roles along with
        /// correspnding tasks defined in different stages / roles
        /// </summary>
        /// <param name="newRow"></param>
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
                obj_typed.StepNo = Convert.ToInt32(hdCounter.Value);
                obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                obj_typed.WFName = Session["WFName"].ToString();
                obj_typed.RoleId = ddRole1.SelectedValue;
                obj_typed.RoleName = ddRole1.SelectedItem.Text;
                obj_typed.Duration = txtTime.Text.Trim();
                obj_typed.TaskList = lblTaskList1.Text;

                dt3.Rows.Add(obj_typed.AddNewRecordDisplay(r3));
            }
            Session["dt3"] = dt3;
            gvDtl.DataSource = dt3;
            gvDtl.DataBind();
        }

        /// <summary>
        /// There are two Datatables. One for storing the multiple roles against one Workflow ID
        /// and another is for storing multiple tasks against one role / stage.
        /// In this function, these two Datatables are merged and displayed all the roles / stages defined for a single workflow and
        /// the corresponding tasks also displayed for individual role / stage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                wf_dtl_bal obj_typed = new wf_dtl_bal();
                if (Session["WFId"] != null)
                {
                    DataTable dt1 = null;
                    if (Session["dt1"] != null)
                    {
                        dt1 = (DataTable)Session["dt1"];
                    }
                    else
                    {
                        dt1 = obj_typed.CreateDTWFDtl();
                    }
                    if (dt1.Rows.Count < 10) /// As Max 10 stages are allowed in a single workflow
                    {
                        // Create a new row in the Data Table
                        DataRow r1 = dt1.NewRow();
                        //Adding row in Data Table
                        obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                        obj_typed.StepNo = Convert.ToInt32(hdCounter.Value);
                        obj_typed.RoleId = ddRole1.SelectedValue;
                        obj_typed.Duration = txtTime.Text.Trim();
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (Convert.ToInt64(dt1.Rows[i]["wf_id"]) == Convert.ToInt64(Session["WFId"]) && dt1.Rows[i]["role_id"].ToString() == ddRole1.SelectedValue)
                            {
                                throw new Exception("This Role is already added in this Workflow !!");
                            }
                        }
                        dt1.Rows.Add(obj_typed.AddNewRecordDtl(r1));
                        Session["dt1"] = dt1;

                        Session["DisplayNo"] = 1;
                        BindGvDtl(true);

                        lblTaskList1.Text = "";
                        lblWFDtls.Text = "The Stage-wise Flow for the Workflow " + "<span style=\"color:#660066;\">\"" + txtWFName.Text.ToString() + "\"</span> is as Follows:";

                        hdCounter.Value = (Convert.ToInt32(hdCounter.Value) + 1).ToString();
                        txtStage1.Text = (dt1.Rows.Count + 1).ToString();
                        txtToMail1.Text = "";
                        txtCondMsg.Text = "";
                        txtCondVal.Text = "";
                    }
                    else
                    {
                        MessageBox("You can not define more than 10 stages in a single workflow");
                    }
                }
                else
                {
                    MessageBox("At first add the workflow and then proceed");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        
        protected void gvDtl_RowEditing(object sender, GridViewEditEventArgs e)
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
        
        protected void gvDtl_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
        
        protected void gvDtl_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
        }
        
        protected void gvDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        
        /// <summary>
        /// To delete a row from the Datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDtl_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                /// Set dt1 Datatable (Workflow Details) start
                DataTable dt1 = null;
                if (Session["dt1"] != null)
                {
                    dt1 = (DataTable)Session["dt1"];
                }
                else
                {
                    dt1 = obj_typed.CreateDTWFDtl();
                }
                /// Set dt1 Datatable (Workflow Details) end
                /// Set dt2 Datatable (Workflow Task) start
                DataTable dt2 = null;
                if (Session["dt2"] != null)
                {
                    dt2 = (DataTable)Session["dt2"];
                }
                else
                {
                    dt2 = obj_typed.CreateDTWFTask();
                }
                /// Set dt2 Datatable (Workflow Task) end

                /// Delete from dt Datatable start
                GridViewRow row = (GridViewRow)gvDtl.Rows[e.RowIndex];
                DataTable dt = (DataTable)Session["dt3"];

                /// As dt is deleting, dt1 also should be deleted with the respective records
                for (int i = dt1.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr1 = dt1.Rows[i];
                    string dtitem1 = dr1["step_no"].ToString();
                    if (dtitem1 == dt.Rows[e.RowIndex][0].ToString())
                    {
                        dt1.Rows.Remove(dr1);
                    }
                }
                Session["dt1"] = dt1;

                /// As dt is deleting, dt2 also should be deleted with the respective records
                for (int i = dt2.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr2 = dt2.Rows[i];
                    string dtitem2 = dr2["step_no"].ToString();
                    if (dtitem2 == dt.Rows[e.RowIndex][0].ToString())
                    {
                        dt2.Rows.Remove(dr2);
                    }
                }
                Session["dt2"] = dt2;

                dt.Rows.RemoveAt(e.RowIndex);

                gvDtl.EditIndex = -1;
                /// Delete from dt Datatable end
                txtStage1.Text = (dt1.Rows.Count + 1).ToString();
                Session["dt3"] = dt;
                Session["DisplayNo"] = 1;
                BindGvDtl(false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// This is used to insert <role_id, step_no> into the <wf_dtl> table
        /// And also the <task_id> will be inserted into the <wf_task> table mapping with <wf_id & role_id> of <wf_dtl> table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFinalAdd_Click(object sender, EventArgs e)
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
                string result = "";
                int myCounter = 1;
                if (Session["WFId"] != null)
                {
                    if (Convert.ToInt16(txtStage1.Text) <= 1)
                    {
                        throw new Exception("Please add the Stage-wise Task Details.");
                    }
                    //wf_task_bal wfTaskObj = new wf_task_bal();
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
                        //wf_dtl_bal wfDtlObj = new wf_dtl_bal();
                        /// Insert into the database from Datatable using Store Procedure <WorkflowDtl_Insert> start

                        //wfDtlObj.WFID = Convert.ToInt64(dr["wf_id"]);
                        //wfDtlObj.StepNo = myCounter; //Convert.ToInt64(dr["step_no"]);
                        //wfDtlObj.RoleId = dr["role_id"].ToString();
                        //wfDtlObj.Duration = dr["duration"].ToString();

                        //string result = wfDtlObj.InsertWFDtl();
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            result = ObjClassStoreProc.InsertWFDtl(Convert.ToInt32(dr["wf_id"]), myCounter, dr["role_id"].ToString(), dr["duration"].ToString(), ddCompany.SelectedValue);
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            result = ObjClassStoreProc.InsertWFDtl(Convert.ToInt32(dr["wf_id"]), myCounter, dr["role_id"].ToString(), dr["duration"].ToString(), Session["CompCode"].ToString());
                        }
                        if (!string.IsNullOrEmpty(result))
                        {
                            WFDtlCntr[myCounter - 1, 0] = myCounter;
                            WFDtlCntr[myCounter - 1, 1] = Convert.ToInt32(dr["step_no"]);
                            myCounter = myCounter + 1;
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
                        /// Insert into the database from Datatable using Store Procedure <WorkflowCond_Insert> start

                        //wfTaskObj.WFID = Convert.ToInt64(dt03.Rows[p][0].ToString());
                        //wfTaskObj.StepNo = Convert.ToInt32(dt03.Rows[p][1].ToString());
                        //wfTaskObj.TaskID = dt03.Rows[p][2].ToString();
                        //wfTaskObj.FormFieldNo = Convert.ToInt32(dt03.Rows[p][3].ToString());
                        //wfTaskObj.cond_op = dt03.Rows[p][4].ToString();
                        //wfTaskObj.cond_val = dt03.Rows[p][5].ToString();
                        //wfTaskObj.AmbleMail = dt03.Rows[p][6].ToString();
                        //wfTaskObj.AmbleMsg = dt03.Rows[p][7].ToString();
                        //wfTaskObj.AmbleAttach = dt03.Rows[p][8].ToString();
                        //wfTaskObj.AmbleURL = dt03.Rows[p][9].ToString();
                        //wfTaskObj.CondMailSub = dt03.Rows[p][10].ToString();

                        //result = wfTaskObj.InsertWFCond();
                        if (Session["UserType"].ToString() == "S") // Super Admin 
                        {
                            result = ObjClassStoreProc.InsertWFCond(Convert.ToInt32(dt03.Rows[p][0].ToString()),Convert.ToInt32(dt03.Rows[p][1].ToString()),dt03.Rows[p][2].ToString(),Convert.ToInt32(dt03.Rows[p][3].ToString()),dt03.Rows[p][4].ToString(),dt03.Rows[p][5].ToString(),dt03.Rows[p][6].ToString(),dt03.Rows[p][7].ToString(),dt03.Rows[p][8].ToString(),dt03.Rows[p][9].ToString(),dt03.Rows[p][10].ToString(), ddCompany.SelectedValue);
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            result = ObjClassStoreProc.InsertWFCond(Convert.ToInt32(dt03.Rows[p][0].ToString()), Convert.ToInt32(dt03.Rows[p][1].ToString()), dt03.Rows[p][2].ToString(), Convert.ToInt32(dt03.Rows[p][3].ToString()), dt03.Rows[p][4].ToString(), dt03.Rows[p][5].ToString(), dt03.Rows[p][6].ToString(), dt03.Rows[p][7].ToString(), dt03.Rows[p][8].ToString(), dt03.Rows[p][9].ToString(), dt03.Rows[p][10].ToString(), Session["CompCode"].ToString());
                        }
                    }
                    // Conditional Part End

                    // SignDate Start
                    DataTable dt05 = null;
                    if (Session["dt05"] != null)
                    {
                        dt05 = (DataTable)Session["dt05"];
                    }
                    else
                    {
                        dt05 = obj_typed.CreateDTSignDate();
                    }
                    for (int q = 0; q < dt05.Rows.Count; q++)
                    {
                        /// Insert into the database from Datatable using Store Procedure <WFSignDate_Insert> start

                        //wfTaskObj.WFID = Convert.ToInt64(dt05.Rows[q][0].ToString());
                        //wfTaskObj.StepNo = Convert.ToInt32(dt05.Rows[q][1].ToString());

                        //result = wfTaskObj.InsertWFSignDate(Convert.ToInt32(dt05.Rows[q][2].ToString()), Convert.ToInt32(dt05.Rows[q][3].ToString()), Convert.ToInt32(dt05.Rows[q][4].ToString()), Convert.ToInt32(dt05.Rows[q][5].ToString()), Convert.ToInt32(dt05.Rows[q][6].ToString()), Convert.ToInt32(dt05.Rows[q][7].ToString()), Convert.ToInt32(dt05.Rows[q][8].ToString()), Convert.ToInt32(dt05.Rows[q][9].ToString()), Convert.ToInt32(dt05.Rows[q][10].ToString()), Convert.ToInt32(dt05.Rows[q][11].ToString()), Convert.ToInt32(dt05.Rows[q][12].ToString()), Convert.ToInt32(dt05.Rows[q][13].ToString()), Convert.ToInt32(dt05.Rows[q][14].ToString()), Convert.ToInt32(dt05.Rows[q][15].ToString()), Convert.ToInt32(dt05.Rows[q][16].ToString()), Convert.ToInt32(dt05.Rows[q][17].ToString()), Convert.ToInt32(dt05.Rows[q][18].ToString()), Convert.ToInt32(dt05.Rows[q][19].ToString()), Convert.ToInt32(dt05.Rows[q][20].ToString()), Convert.ToInt32(dt05.Rows[q][21].ToString()));
                        if (Session["UserType"].ToString() == "S") // Super Admin 
                        {
                            result = ObjClassStoreProc.InsertWFSignDate(Convert.ToInt32(dt05.Rows[q][0].ToString()), Convert.ToInt32(dt05.Rows[q][1].ToString()), Convert.ToInt32(dt05.Rows[q][2].ToString()), Convert.ToInt32(dt05.Rows[q][3].ToString()), Convert.ToInt32(dt05.Rows[q][4].ToString()), Convert.ToInt32(dt05.Rows[q][5].ToString()), Convert.ToInt32(dt05.Rows[q][6].ToString()), Convert.ToInt32(dt05.Rows[q][7].ToString()), Convert.ToInt32(dt05.Rows[q][8].ToString()), Convert.ToInt32(dt05.Rows[q][9].ToString()), Convert.ToInt32(dt05.Rows[q][10].ToString()), Convert.ToInt32(dt05.Rows[q][11].ToString()), Convert.ToInt32(dt05.Rows[q][12].ToString()), Convert.ToInt32(dt05.Rows[q][13].ToString()), Convert.ToInt32(dt05.Rows[q][14].ToString()), Convert.ToInt32(dt05.Rows[q][15].ToString()), Convert.ToInt32(dt05.Rows[q][16].ToString()), Convert.ToInt32(dt05.Rows[q][17].ToString()), Convert.ToInt32(dt05.Rows[q][18].ToString()), Convert.ToInt32(dt05.Rows[q][19].ToString()), Convert.ToInt32(dt05.Rows[q][20].ToString()), Convert.ToInt32(dt05.Rows[q][21].ToString()), ddCompany.SelectedValue);
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            result = ObjClassStoreProc.InsertWFSignDate(Convert.ToInt32(dt05.Rows[q][0].ToString()), Convert.ToInt32(dt05.Rows[q][1].ToString()), Convert.ToInt32(dt05.Rows[q][2].ToString()), Convert.ToInt32(dt05.Rows[q][3].ToString()), Convert.ToInt32(dt05.Rows[q][4].ToString()), Convert.ToInt32(dt05.Rows[q][5].ToString()), Convert.ToInt32(dt05.Rows[q][6].ToString()), Convert.ToInt32(dt05.Rows[q][7].ToString()), Convert.ToInt32(dt05.Rows[q][8].ToString()), Convert.ToInt32(dt05.Rows[q][9].ToString()), Convert.ToInt32(dt05.Rows[q][10].ToString()), Convert.ToInt32(dt05.Rows[q][11].ToString()), Convert.ToInt32(dt05.Rows[q][12].ToString()), Convert.ToInt32(dt05.Rows[q][13].ToString()), Convert.ToInt32(dt05.Rows[q][14].ToString()), Convert.ToInt32(dt05.Rows[q][15].ToString()), Convert.ToInt32(dt05.Rows[q][16].ToString()), Convert.ToInt32(dt05.Rows[q][17].ToString()), Convert.ToInt32(dt05.Rows[q][18].ToString()), Convert.ToInt32(dt05.Rows[q][19].ToString()), Convert.ToInt32(dt05.Rows[q][20].ToString()), Convert.ToInt32(dt05.Rows[q][21].ToString()), Session["CompCode"].ToString());
                        }
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
                    int[,] WFDtlCntr2 = new int[dt2.Rows.Count, 2];
                    foreach (DataRow dr in dt2.Rows)
                    {
                        for (int j = 0; j < myCounter - 1; j++)
                        {
                            if (Convert.ToInt64(dr["step_no"]) == WFDtlCntr[j, 1])
                            {
                                WFDtlCntr2[i, 1] = WFDtlCntr[j, 1];
                                WFDtlCntr2[i, 0] = WFDtlCntr[j, 0];
                                i++;
                                break;
                            }
                        }
                    }
                    i = 0;
                    foreach (DataRow dr in dt2.Rows)
                    {
                        /// Insert into the database from Datatable using Store Procedure <WorkflowTask_Insert> start

                        //wfTaskObj.WFID = Convert.ToInt64(dr["wf_id"]);
                        //wfTaskObj.StepNo = WFDtlCntr2[i, 0]; //Convert.ToInt64(dr["step_no"]);
                        //wfTaskObj.TaskID = dr["task_id"].ToString();
                        //wfTaskObj.ActTypeID = dr["acttype_id"].ToString();
                        //wfTaskObj.UUID = dr["copy_to_uuid"].ToString();
                        //wfTaskObj.AmbleMail = dr["amble_mail"].ToString();
                        //wfTaskObj.AmbleMsg = dr["amble_msg"].ToString();
                        //wfTaskObj.AmbleAttach = dr["amble_attach"].ToString();
                        //wfTaskObj.AppendDoc = dr["AppendDoc"].ToString();
                        //wfTaskObj.AmbleURL = dr["amble_url"].ToString();
                        //wfTaskObj.AmbleSub = dr["AmbleSub"].ToString();

                        //result = wfTaskObj.InsertWFTask();
                        if (Session["UserType"].ToString() == "S") // Super Admin 
                        {
                            result = ObjClassStoreProc.InsertWFTask(Convert.ToInt32(dr["wf_id"]), WFDtlCntr2[i, 0], dr["task_id"].ToString(), dr["acttype_id"].ToString(), dr["copy_to_uuid"].ToString(), dr["amble_mail"].ToString(), dr["amble_msg"].ToString(), dr["amble_attach"].ToString(), dr["AppendDoc"].ToString(), dr["amble_url"].ToString(), dr["AmbleSub"].ToString(), ddCompany.SelectedValue);
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            result = ObjClassStoreProc.InsertWFTask(Convert.ToInt32(dr["wf_id"]), WFDtlCntr2[i, 0], dr["task_id"].ToString(), dr["acttype_id"].ToString(), dr["copy_to_uuid"].ToString(), dr["amble_mail"].ToString(), dr["amble_msg"].ToString(), dr["amble_attach"].ToString(), dr["AppendDoc"].ToString(), dr["amble_url"].ToString(), dr["AmbleSub"].ToString(), Session["CompCode"].ToString());
                        }

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
                    PopulateGridView();
                    gvDtl.DataSource = null;
                    gvDtl.DataBind();
                    lblWFDtls.Text = "";
                    lblHeading.Text = "";
                    txtWFName.Text = "";
                    PopulateDropdown();
                    txtStage1.Text = "1";
                    txtTime.Text = "0";
                    lblTaskList1.Text = "";
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
                    throw new Exception("Workflow inserted successfully");
                }
                else
                {
                    throw new Exception("At first add the Workflow Master and then Stages.");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To view the selected Workflow in detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispRec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            GridViewRow row = gvDispRec.SelectedRow;
            Label lbWfid = (Label)row.FindControl("lbWfid");
            string lbWfid1 = lbWfid.Text.ToString();
            DBClass DBObj = new DBClass();
            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
            DataSet ds1 = new DataSet();
            ds1 = DBObj.DisplayWFDtl(lbWfid1, Session["CompCode"].ToString());
            if (ds1.Tables[0].Rows.Count > 0)
            {
                MsgNodet.Text = "";
                Lblwf.Text = "Workflow Name : ";
                Lbldept.Text = "Department Name : ";
                Lbldoc.Text = "Document Type : ";
                LblFld.Text = "Folder Name : ";
                lblWfname.Text = ds1.Tables[0].Rows[0]["wf_name"].ToString();
                lblWfdept.Text = ds1.Tables[0].Rows[0]["dept_name"].ToString();
                lblWfDoctype.Text = ds1.Tables[0].Rows[0]["doc_type_name"].ToString();
                //lblWfFld.Text = ds1.Tables[0].Rows[0]["fld_name"].ToString();
                lblWfFld.Text = ObjClassStoreProc.FullPathPassingWFID(lbWfid1);

                DataTable dt1 = new DataTable();

                dt1.Columns.Add("Stage", typeof(int));
                dt1.Columns.Add("Role", typeof(string));
                dt1.Columns.Add("Time (Hrs)", typeof(string));
                dt1.Columns.Add("Task", typeof(string));
                dt1.Columns.Add("Action Type", typeof(string));

                var x = (from r in ds1.Tables[0].AsEnumerable()
                         select r["step_no"]).Distinct().ToList();
                bool flag = true;
                for (int i = 0; i < x.Count; i++)
                {
                    DataRow[] r1 = ds1.Tables[0].Select("step_no=" + x[i]);

                    foreach (DataRow dr in r1)
                    {
                        DataRow r = dt1.NewRow();
                        if (flag)
                        {
                            r["Stage"] = dr["step_no"];
                            r["Role"] = dr["role_name"];
                            r["Time (Hrs)"] = dr["duration"];
                            r["Task"] = dr["task_name"];
                            r["Action Type"] = dr["action_type"];
                            flag = false;
                        }
                        else
                        {
                            r["Task"] = dr["task_name"];
                            r["Action Type"] = dr["action_type"];
                        }
                        dt1.Rows.Add(r);
                    }
                    flag = true;
                }
                gv.DataSource = dt1;
                gv.DataBind();
                dt1.Clear();
            }
            else
            {
                Lblwf.Text = "";
                Lbldept.Text = "";
                Lbldoc.Text = "";
                LblFld.Text = "";
                lblWfname.Text = "";
                lblWfdept.Text = "";
                lblWfDoctype.Text = "";
                lblWfFld.Text = "";
                gv.DataSource = null;
                gv.DataBind();
                MsgNodet.Text = "No Details Found for this Workflow!";
            }
        }

        protected void gvDispRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            gvDispRec.PageIndex = e.NewPageIndex;
            PopulateGridView();
        }
        
        protected void gvDispRec_RowEditing(object sender, GridViewEditEventArgs e)
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
                String mWFId = gvDispRec.DataKeys[e.NewEditIndex].Values["wf_id"].ToString();

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.WFRecords(mWFId);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Documents are active in this Workflow -- So can not be Edited !!");
                }
                else
                {
                    Response.Redirect("WFEditing.aspx?WFId=" + mWFId + "&EditCompCode=" + Session["CompCode"].ToString(), false);
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }
        
        protected void gvDispRec_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
                hfMsg.Value = ex.Message;
            }
        }
        
        protected void gvDispRec_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
                gvDispRec.EditIndex = -1;
                PopulateGridView();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        
        protected void gvDispRec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Finding the Dropdown control.

                    Control ctrl = e.Row.FindControl("lbl1");
                    if (ctrl != null)
                    {
                        Label lbl = ctrl as Label;
                        lbl.Text = Session["DisplayNo"].ToString();
                        Session["DisplayNo"] = Convert.ToInt32(Session["DisplayNo"]) + 1;
                    }
                }

                // For Department Dropdown
                Control ctrl1 = e.Row.FindControl("ddlEditDept");
                if (ctrl1 != null)
                {
                    DropDownList ddlEditDept = ctrl1 as DropDownList;
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DeptAllBasedOnCompCode(Session["CompCode"].ToString());
                    ddlEditDept.DataSource = ds01;
                    ddlEditDept.DataTextField = "dept_name";
                    ddlEditDept.DataValueField = "dept_id";
                    ddlEditDept.DataBind();
                    Control ctrl6 = e.Row.FindControl("hdDept");
                    if (ctrl6 != null)
                    {
                        HiddenField hdDept = ctrl6 as HiddenField;
                        for (int i = 0; i < ddlEditDept.Items.Count; i++)
                        {
                            if (ddlEditDept.Items[i].Text == hdDept.Value)
                            {
                                ddlEditDept.ClearSelection();
                                ddlEditDept.Items[i].Selected = true;
                            }
                        }
                    }
                }

                // For DocType Dropdown
                Control ctrl2 = e.Row.FindControl("ddlEditDocType");
                if (ctrl2 != null)
                {
                    DropDownList ddlEditDocType = ctrl2 as DropDownList;
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.SelectDocTypeCompBased(Session["CompCode"].ToString());
                    ddlEditDocType.DataSource = ds01;
                    ddlEditDocType.DataTextField = "doc_type_name";
                    ddlEditDocType.DataValueField = "doc_type_id";
                    ddlEditDocType.DataBind();
                    Control ctrl7 = e.Row.FindControl("hdDocType");
                    if (ctrl7 != null)
                    {
                        HiddenField hdDocType = ctrl7 as HiddenField;
                        for (int i = 0; i < ddlEditDocType.Items.Count; i++)
                        {
                            if (ddlEditDocType.Items[i].Text == hdDocType.Value)
                            {
                                ddlEditDocType.ClearSelection();
                                ddlEditDocType.Items[i].Selected = true;
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
        
        protected void gvDispRec_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                GridViewRow row = (GridViewRow)gvDispRec.Rows[e.RowIndex];
                Label lbAutoID = (Label)row.FindControl("lbWfid");

                wf_mast_bal OBJ_WFBAL = new wf_mast_bal();
                OBJ_WFBAL.WFID = Convert.ToInt64(lbAutoID.Text.Trim());

                string result = OBJ_WFBAL.DeleteWF(); // If there is any WFL started with this definition, then it will not be DELETED.

                if (Convert.ToInt64(result) > 0)
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("Data Deleted Successfully !!");
                }
                else if (Convert.ToInt64(result) == -1)
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("Documents are active in this Workflow -- So can not be Deleted !!");
                }
                else
                {
                    throw new Exception("Error in Data Deletion !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value= ex.Message;
            }
        }

        protected void PopUser()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectUserAll(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectUserAll(Session["CompCode"].ToString());
                }
                ddUser.DataSource = ds01;
                ddUser.DataTextField = "name";
                ddUser.DataValueField = "email";
                ddUser.DataBind();
                ddUser.Items.Insert(0, "init@init.com");

                ddUser1.DataSource = ds01;
                ddUser1.DataTextField = "name";
                ddUser1.DataValueField = "email";
                ddUser1.DataBind();
                ddUser1.Items.Insert(0, "init@init.com");
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

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

                Session["AmbleMailID"] = txtToMail.Text.Trim();
                Session["AmbleSub"] = txtAmbleSub.Text.Trim();
                Session["AmbleMsg"] = txtAmbleMsg.Text.Trim();
                Session["AmbleAttach"] = ddAttachMail.SelectedValue;
                Session["AmbleURL"] = ddAmbleURL.SelectedValue;

                //if (txtToMail.Text.Trim() == "")
                //{
                //    throw new Exception("Please Enter Email ID");
                //}
                //else if (txtAmbleSub.Text.Trim() == "")
                //{
                //    throw new Exception("Please Enter Email Subject");
                //}
                //else if (txtAmbleMsg.Text.Trim() == "")
                //{
                //    throw new Exception("Please Enter Email Message");
                //}
                //else
                //{
                //    Session["AmbleMailID"] = txtToMail.Text.Trim();
                //    Session["AmbleSub"] = txtAmbleSub.Text.Trim();
                //    Session["AmbleMsg"] = txtAmbleMsg.Text.Trim();
                //    Session["AmbleAttach"] = ddAttachMail.SelectedValue;
                //    Session["AmbleURL"] = ddAmbleURL.SelectedValue;
                //}
            }
            catch (Exception ex)
            {
                hfMsg.Value= ex.Message;
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
                Session["AmbleMailID"] = txtToMail1.Text.Trim();
                Session["AmbleSub"] = txtCondSub.Text.Trim();
                Session["AmbleMsg"] = txtCondMsg.Text.Trim();
                Session["AmbleAttach"] = ddAttachCondMail.SelectedValue;
                Session["AmbleURL"] = ddCondURL.SelectedValue;
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
                        gvCond.EditIndex = -1;
                        Session["dt03"] = dt03;
                        BindGvCond(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

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
                    BindGvCond(true);
                    txtCondVal.Text = "";
                    txtToMail1.Text = "";
                    txtCondMsg.Text = "";
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value=ex.Message;
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
                if (ddDocument3.SelectedValue == null)
                {
                    //throw new Exception("Please Select the Document to be Appended");
                }
                else
                {
                    Session["AppendDoc"] = ddDocument3.SelectedValue;
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
                    //Adding row in Data Table
                    obj_typed.WFID = Convert.ToInt64(Session["WFId"]);
                    obj_typed.StepNo =Convert.ToInt32(txtStage1.Text.Trim());
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
                gvCond.DataSource = dt03;
                gvCond.DataBind();
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
                GridViewRow row = (GridViewRow)gvCond.Rows[e.RowIndex];
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
                gvCond.EditIndex = -1;
                Session["dt03"] = dt03;
                BindGvCond(false);
                throw new Exception("Data Deleted Successfully !!");
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
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

    }
}