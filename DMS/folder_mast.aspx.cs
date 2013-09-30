using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using Alfresco.RepositoryWebService;
using Alfresco;
using DMS.UTILITY;

namespace DMS
{
    public partial class folder_mast : System.Web.UI.Page
    {
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;

        public RepositoryService RepoService
        {
            set { repoService = value; }
        }

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
                    cmdAddMaster.Attributes.Add("OnClick", "javascript: return FormValidation();");
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        Page.Header.DataBind();
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopulateDropdown();
                            PopulateGridView();
                            divCompany.Visible = true;
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopulateDropdown();
                            PopulateGridView();
                            divCompany.Visible = false;
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopulateDropdown();
                            PopulateGridView();
                            divCompany.Visible = false;
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
                //....Company
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCompActiveAll();
                ddCompany.DataSource = ds01;
                ddCompany.DataTextField = "CompName";
                ddCompany.DataValueField = "CompCode";
                ddCompany.DataBind();

                PopCabinetDropdown();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopCabinetDropdown()
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
                else
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                }
                ddCabinet.DataSource = ds01;
                ddCabinet.DataTextField = "cab_name";
                ddCabinet.DataValueField = "cab_uuid";
                ddCabinet.DataBind();
                ddCabinet.Items.Insert(0, "");
                PopulateDrawer(ddCabinet.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDrawer(string SelCabUUID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCabUUID, Session["UserID"].ToString());
                ddDrawer.DataSource = ds01;
                ddDrawer.DataTextField = "drw_name";
                ddDrawer.DataValueField = "drw_uuid";
                ddDrawer.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            PopCabinetDropdown();
            PopulateGridView();
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
        /// The following function is used to insert a record in the Database's  <fld_name>,<fld_desc> & <drw_id> fields of <fld_mast> table
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
                if (txtFldName.Text.LastIndexOf("\\") != -1 || txtFldName.Text.LastIndexOf("/") != -1)
                {
                    throw new Exception("Folder Name can not accept any special character");
                }

                folder_mast_bal OBJ_FolderBAL = new folder_mast_bal();
                OBJ_FolderBAL.FolderName = txtFldName.Text.Trim();
                OBJ_FolderBAL.FolderDesc = txtFldDesc.Text.Trim();
                OBJ_FolderBAL.DrawerCode = ddDrawer.SelectedValue;

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.ExistNode("Folder", txtFldName.Text.Trim(), ddCompany.SelectedValue, ddDrawer.SelectedValue);
                }
                else // Admin and Normal User
                {
                    ds01 = ObjClassStoreProc.ExistNode("Folder", txtFldName.Text.Trim(), Session["CompCode"].ToString(), ddDrawer.SelectedValue);
                }
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Folder name already exist, Please choose different name!");
                }

                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = Session["AdmUserID"].ToString();
                wsF.Ticket = Session["AdmTicket"].ToString();
                this.repoService = wsF.getRepositoryService();
                // Initialise the reference to the spaces store
                this.spacesStore = new Alfresco.RepositoryWebService.Store();
                this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                this.spacesStore.address = "SpacesStore";

                Alfresco.RepositoryWebService.Reference reference = new Alfresco.RepositoryWebService.Reference();
                reference.store = this.spacesStore;
                reference.uuid = ddDrawer.SelectedValue;
                if (!SearchNode(reference, txtFldName.Text.Trim()))
                {
                    AlfCreateSpace ObjCreateSpace = new AlfCreateSpace();
                    Session["NodeUUID"] = ObjCreateSpace.CreateSpace(txtFldName.Text.Trim(), txtFldDesc.Text.Trim(), null, ddDrawer.SelectedValue, ddDrawer.SelectedItem.Text, wsF.UserName, wsF.Ticket);
                    string result = "";
                    if (Session["UserType"].ToString() == "S") // Super Admin
                    {
                        result = ObjClassStoreProc.InsertFolderMast(txtFldName.Text.Trim(), txtFldDesc.Text.Trim(), ddDrawer.SelectedValue, Session["NodeUUID"].ToString(), ddCompany.SelectedValue);
                    }
                    else
                    {
                        result = ObjClassStoreProc.InsertFolderMast(txtFldName.Text.Trim(), txtFldDesc.Text.Trim(), ddDrawer.SelectedValue, Session["NodeUUID"].ToString(), Session["CompCode"].ToString());
                    }
                    if (Convert.ToInt64(result) > 0)
                    {
                        //RightsObj.SetPermissions(OBJ_FolderBAL.UUID, "Folder", Session["UserID"].ToString(),"X");
                        UserRights RightsObj = new UserRights();
                        //result = RightsObj.FetchPermission(ddDrawer.SelectedValue);
                        //RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Folder", Session["UserID"].ToString(), result);
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            ds01.Reset();
                            ds01 = RightsObj.FetchPermission(ddDrawer.SelectedValue, ddCompany.SelectedValue);
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                {
                                    RightsObj.InsertPermissionSingleData(Session["NodeUUID"].ToString(), "Folder", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), ddCompany.SelectedValue);
                                }
                            }
                        }
                        else // Admin and Normal User
                        {
                            ds01.Reset();
                            ds01 = RightsObj.FetchPermission(ddDrawer.SelectedValue, Session["CompCode"].ToString());
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                {
                                    RightsObj.InsertPermissionSingleData(Session["NodeUUID"].ToString(), "Folder", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                                }
                            }
                        }
                        // For the TEMPX Folder
                        if (chkTemp.Checked == true)
                        {
                            string TempFolderName = txtFldName.Text.Trim() + " TEMPX";
                            string TempFolderDesc = "Temporary Folder for " + txtFldName.Text.Trim();
                            if (!SearchNode(reference, TempFolderName))
                            {
                                Session["NodeUUID"] = ObjCreateSpace.CreateSpace(TempFolderName, TempFolderDesc, null, ddDrawer.SelectedValue, ddDrawer.SelectedItem.Text, wsF.UserName, wsF.Ticket);
                                if (Session["UserType"].ToString() == "S") // Super Admin
                                {
                                    result = ObjClassStoreProc.InsertFolderMast(TempFolderName, TempFolderDesc, ddDrawer.SelectedValue, Session["NodeUUID"].ToString(), ddCompany.SelectedValue);
                                }
                                else
                                {
                                    result = ObjClassStoreProc.InsertFolderMast(TempFolderName, TempFolderDesc, ddDrawer.SelectedValue, Session["NodeUUID"].ToString(), Session["CompCode"].ToString());
                                }
                                //RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Folder", Session["UserID"].ToString(), "X");
                                if (Session["UserType"].ToString() == "S") // Super Admin
                                {
                                    RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Folder", Session["UserID"].ToString(), "X", ddCompany.SelectedValue);
                                }
                                else // Admin and Normal User
                                {
                                    RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Folder", Session["UserID"].ToString(), "X", Session["CompCode"].ToString());
                                }
                                //result = RightsObj.FetchPermission(ddDrawer.SelectedValue);
                                //RightsObj.SetPermissions(OBJ_FolderBAL.UUID, "Folder", Session["UserID"].ToString(), result);
                            }
                        }

                        PopulateGridView();
                        PopulateDropdown();
                        txtFldName.Text = "";
                        txtFldDesc.Text = "";
                        txtLocation.Text = "";
                        MessageBox("Data inserted successfully");
                    }
                    else
                    {
                        throw new Exception("Data already exists!");
                    }
                }
                else
                {
                    throw new Exception("Folder name already exist, Please choose different name!");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_FolderMast>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateGridView()
        {
            try
            {
                DBClass DBObj = new DBClass();
                DataSet ds1 = new DataSet();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds1 = DBObj.GVFolder(ddCompany.SelectedValue, Session["UserID"].ToString());
                }
                else
                {
                    ds1 = DBObj.GVFolder(Session["CompCode"].ToString(), Session["UserID"].ToString());
                }
                gvDispRec.DataSource = ds1;
                gvDispRec.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                gvDispRec.EditIndex = e.NewEditIndex;
                PopulateGridView();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To update a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                int rIndex = e.RowIndex;
                GridViewRow row = (GridViewRow)gvDispRec.Rows[e.RowIndex];
                TextBox txtEditFldDesc = (TextBox)row.FindControl("txtEditFldDesc");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                folder_mast_bal OBJ_FolderBAL = new folder_mast_bal();
                OBJ_FolderBAL.FolderDesc = txtEditFldDesc.Text.Trim();
                OBJ_FolderBAL.Labelid = lbAutoID.Text.Trim();

                string result = OBJ_FolderBAL.UpdateFolder();

                gvDispRec.EditIndex = -1;
                PopulateGridView();
                if (Convert.ToInt32(result) > 0)
                {
                    throw new Exception("Data Updated Successfully !!");
                }
                else
                {
                    throw new Exception("Error in updating the record !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        /// <summary>
        /// To cancel after clicking on edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Row updating event in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispRec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Control ctrl1 = e.Row.FindControl("ddlEditDrawer");
                DropDownList ddlEditDrawer = ctrl1 as DropDownList;
                Control ctrl2 = e.Row.FindControl("ddlEditCabinet");
                DropDownList ddlEditCabinet = ctrl2 as DropDownList;
                if (ctrl2 != null)
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                    ddlEditCabinet.DataTextField = "cab_name";
                    ddlEditCabinet.DataValueField = "cab_uuid";
                    ddlEditCabinet.DataSource = ds01;
                    ddlEditCabinet.DataBind();
                    Control ctrl3 = e.Row.FindControl("hdCabinet");
                    if (ctrl3 != null)
                    {
                        HiddenField hdCabinet = ctrl3 as HiddenField;
                        for (int i = 0; i < ddlEditCabinet.Items.Count; i++)
                        {
                            if (ddlEditCabinet.Items[i].Text == hdCabinet.Value)
                            {
                                ddlEditCabinet.ClearSelection();
                                ddlEditCabinet.Items[i].Selected = true;
                            }
                        }
                    }
                }
                if (ctrl1 != null)
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(ddlEditCabinet.SelectedValue, Session["UserID"].ToString());
                    ddlEditDrawer.DataTextField = "drw_name";
                    ddlEditDrawer.DataValueField = "drw_uuid";
                    ddlEditDrawer.DataSource = ds01;
                    ddlEditDrawer.DataBind();
                    Control ctrl6 = e.Row.FindControl("hdDrawer");
                    if (ctrl6 != null)
                    {
                        HiddenField hdDrawer = ctrl6 as HiddenField;
                        for (int i = 0; i < ddlEditDrawer.Items.Count; i++)
                        {
                            if (ddlEditDrawer.Items[i].Text == hdDrawer.Value)
                            {
                                ddlEditDrawer.ClearSelection();
                                ddlEditDrawer.Items[i].Selected = true;
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

        protected void ddlEditCabinet_SelectedIndexChanged(object sender, EventArgs e)
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
                DropDownList drp1, drp2;

                GridViewRow gvr = (GridViewRow)((DropDownList)sender).Parent.Parent;

                int intRowIndex;
                intRowIndex = gvr.RowIndex;

                drp1 = (DropDownList)gvDispRec.Rows[intRowIndex].FindControl("ddlEditCabinet");

                drp2 = (DropDownList)gvDispRec.Rows[intRowIndex].FindControl("ddlEditDrawer");

                DropDownList ddlEditCabinet = drp1 as DropDownList;
                DropDownList ddlEditDrawer = drp2 as DropDownList;

                if (drp1 != null)
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(ddlEditCabinet.SelectedValue, Session["UserID"].ToString());
                    ddlEditDrawer.DataTextField = "drw_name";
                    ddlEditDrawer.DataValueField = "drw_uuid";
                    ddlEditDrawer.DataSource = ds01;
                    ddlEditDrawer.DataBind();
                    Control ctrl6 = gvDispRec.Rows[intRowIndex].FindControl("hdDrawer");
                    if (ctrl6 != null)
                    {
                        HiddenField hdDrawer = ctrl6 as HiddenField;
                        for (int i = 0; i < ddlEditDrawer.Items.Count; i++)
                        {
                            if (ddlEditDrawer.Items[i].Text == hdDrawer.Value)
                            {
                                ddlEditDrawer.ClearSelection();
                                ddlEditDrawer.Items[i].Selected = true;
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

        /// <summary>
        /// To delete a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                //SqlConnection con = Utility.GetConnection();
                //SqlCommand cmd = null;
                DataSet ds01 = new DataSet();
                //DataSet ds02 = new DataSet();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();

                int rIndex = e.RowIndex;
                GridViewRow row = (GridViewRow)gvDispRec.Rows[e.RowIndex];
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                //con.Open();
                //DataSet ds03 = new DataSet();
                //cmd = new SqlCommand("select fld_name from folder_mast where fld_uuid='" + lbAutoID.Text.Trim() + "'", con);
                //SqlDataAdapter adapter03 = new SqlDataAdapter(cmd);
                //adapter03.Fill(ds03);
                //if (ds03.Tables[0].Rows.Count > 0)
                //{
                    //if (ds03.Tables[0].Rows[0][0].ToString().ToUpper() == "TRASH" || ds03.Tables[0].Rows[0][0].ToString().ToUpper() == "STORAGE")
                    //{
                        //throw new Exception("This Folder can not be Deleted.");
                    //}
                //}
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderNamePassingFolderUUID(lbAutoID.Text.Trim());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    if (ds01.Tables[0].Rows[0][0].ToString().ToUpper() == "TRASH" || ds01.Tables[0].Rows[0][0].ToString().ToUpper() == "STORAGE")
                    {
                        throw new Exception("This Folder can not be Deleted.");
                    }
                }

                // Alfresco Part Start
                string DelUUID = "";

                //cmd = new SqlCommand("select * from doc_mast where fld_uuid='" + lbAutoID.Text.Trim() + "'", con);
                //SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                //adapter02.Fill(ds02);
                //if (ds02.Tables[0].Rows.Count > 0)
                //{
                //    throw new Exception("At First Delete the Documents from This Folder.");
                //}
                ds01.Reset();
                ds01 = ObjClassStoreProc.AllDocumentsInsideFolder(lbAutoID.Text.Trim());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("At First Delete the Documents from This Folder.");
                }

                DelUUID = lbAutoID.Text.Trim();
                // Initialise the reference to the spaces store
                this.spacesStore = new Alfresco.RepositoryWebService.Store();
                this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                this.spacesStore.address = "SpacesStore";
                //create a predicate with the first CMLCreate result
                Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                referenceForNode.store = this.spacesStore;
                referenceForNode.uuid = DelUUID; // Selected Doc's / Folder's UUID

                Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                sourcePredicate.Items = obj_new;

                //copy content
                CMLDelete delete = new CMLDelete();
                delete.where = sourcePredicate;

                CML cmlRemove = new CML();
                cmlRemove.delete = new CMLDelete[] { delete };

                //perform a CML update to delete the node
                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = Session["AdmUserID"].ToString();
                wsF.Ticket = Session["AdmTicket"].ToString();
                wsF.getRepositoryService().update(cmlRemove);
                // Alfresco Part End

                folder_mast_bal OBJ_FolderBAL = new folder_mast_bal();
                OBJ_FolderBAL.Labelid = lbAutoID.Text.Trim();

                string result = OBJ_FolderBAL.DeleteFolder();

                PopulateGridView();
                gvDispRec.EditIndex = -1;
                if (Convert.ToInt32(result)>0)
                {
                    throw new Exception("Data Deleted Successfully !!"); 
                }
                else
                {
                    throw new Exception("Error in Data Deletion !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value =ex.Message;
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
        
        private bool SearchNode(Alfresco.RepositoryWebService.Reference childReference,string NodeName)
        {
            try
            {
                bool ExistFlag = false;
                // Query for the children of the reference
                QueryResult result = this.repoService.queryChildren(childReference);
                if (result.resultSet.rows != null)
                {
                    foreach (ResultSetRow row in result.resultSet.rows)
                    {
                        // only interested in folders
                        if (row.node.type.Contains("folder") != true)
                        {
                            foreach (NamedValue namedValue in row.columns)
                            {
                                if (namedValue.name.Contains("name") == true)
                                {
                                    if (namedValue.name == NodeName)
                                    {
                                        ExistFlag = true;
                                    }
                                }
                            }
                        }
                    }
                }
                return ExistFlag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}