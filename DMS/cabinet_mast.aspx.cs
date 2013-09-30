using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using Alfresco;
using Alfresco.RepositoryWebService;
using DMS.UTILITY;

namespace DMS
{
    public partial class cabinet_mast : System.Web.UI.Page
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
                            PopulateGridView();
                            lblUser.Text = Session["UserFullName"].ToString();
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
                        Response.Redirect("logout.aspx", true);
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

        protected void ddCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            PopulateGridView();
        }

        private bool isExist(string name)
        {
            // Get the repository service
            WebServiceFactory wsF = new WebServiceFactory();
            wsF.UserName = Session["AdmUserID"].ToString();
            wsF.Ticket = Session["AdmTicket"].ToString();
            this.repoService = wsF.getRepositoryService();

            Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
            spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
            spacesStore.address = "SpacesStore";

            // Create a query object
            Alfresco.RepositoryWebService.Query query = new Alfresco.RepositoryWebService.Query();
            query.language = Alfresco.RepositoryWebService.QueryLanguageEnum.lucene;
            query.statement = "Path:\"/\" AND @cm\\:name:\"" + name + "\"";
            Alfresco.RepositoryWebService.QueryResult result = this.repoService.query(spacesStore, query, true);

            if (result.resultSet.rows != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// The following function is used to insert a record in the Database's  <cab_name> & <cab_desc> fields of <cabinet_mast> table
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
                if (txtCabName.Text.LastIndexOf("\\") != -1 || txtCabName.Text.LastIndexOf("/") != -1)
                {
                    throw new Exception("Cabinet Name can not accept any special character");
                }

                cabinet_mast_bal OBJ_CabinetBAL = new cabinet_mast_bal();
                OBJ_CabinetBAL.CabinetName = txtCabName.Text.Trim();
                OBJ_CabinetBAL.CabinetDesc = txtCabDesc.Text.Trim();

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.ExistNode("Cabinet", txtCabName.Text.Trim(),ddCompany.SelectedValue, "");
                }
                else // Admin and Normal User
                {
                    ds01 = ObjClassStoreProc.ExistNode("Cabinet", txtCabName.Text.Trim(), Session["CompCode"].ToString(), "");
                }                
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Cabinet name already exist, Please choose different name!");
                }

                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectServerConfig(ddCompany.SelectedValue);
                }
                else // Admin and Normal User
                {
                    ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                }
                string mWorkspaceName = ds01.Tables[0].Rows[0][10].ToString();
                string mWorkspaceTitle = ds01.Tables[0].Rows[0][11].ToString();
                

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
                //reference.path = "/app:company_home/cm:Office";
                reference.path = "/app:company_home/cm:" + mWorkspaceName;
                if (!SearchNode(reference, txtCabName.Text.Trim()))
                {
                    AlfCreateSpace ObjCreateSpace = new AlfCreateSpace();
                    Session["NodeUUID"] = ObjCreateSpace.CreateSpace(txtCabName.Text.Trim(), txtCabDesc.Text.Trim(), "/app:company_home/cm:" + mWorkspaceName, null, txtCabName.Text.Trim(), wsF.UserName, wsF.Ticket);
                    //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    ds01.Reset();
                    string result = "";
                    if (Session["UserType"].ToString() == "S") // Super Admin
                    {
                        result = ObjClassStoreProc.InsertCabinetMast(txtCabName.Text.Trim(), txtCabDesc.Text.Trim(), Session["NodeUUID"].ToString(), ddPermission.SelectedValue, ddCompany.SelectedValue);
                    }
                    else // Admin and Normal User
                    {
                        result = ObjClassStoreProc.InsertCabinetMast(txtCabName.Text.Trim(), txtCabDesc.Text.Trim(), Session["NodeUUID"].ToString(), ddPermission.SelectedValue, Session["CompCode"].ToString());
                    }
                    
                    
                    //cabinet_mast_bal OBJ_CabinetBAL = new cabinet_mast_bal();
                    /// Pass the <cabinet_name><cabinet_desc> values to <cabinet_mast_bal>

                    //OBJ_CabinetBAL.CabinetName = txtCabName.Text.Trim();
                    //OBJ_CabinetBAL.CabinetDesc = txtCabDesc.Text.Trim();
                    //OBJ_CabinetBAL.UUID =Session["NodeUUID"].ToString();
                    //OBJ_CabinetBAL.DefaultPermission = ddPermission.SelectedValue;
                    //string result = OBJ_CabinetBAL.InsertCabinetMast();
                    /// In the Store Procedure -111 is set when the Name field is duplicate
                    if (Convert.ToInt64(result) == -1)
                    {
                        throw new Exception("Cabinet name already exists!");
                    }
                    else if (Convert.ToInt64(result) >0)
                    {
                        //UserRights RightsObj = new UserRights();
                        //RightsObj.SetPermissions(OBJ_CabinetBAL.UUID, "Cabinet", Session["UserID"].ToString(),"X");
                        UserRights RightsObj = new UserRights();
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Cabinet", Session["UserID"].ToString(), ddPermission.SelectedValue, ddCompany.SelectedValue);
                        }
                        else // Admin and Normal User
                        {
                            RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Cabinet", Session["UserID"].ToString(), ddPermission.SelectedValue, Session["CompCode"].ToString());
                        }
                        //RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Cabinet", Session["UserID"].ToString(), ddPermission.SelectedValue);
                        PopulateGridView();
                        MessageBox("Data inserted successfully");
                        txtCabName.Text = "";
                        txtCabDesc.Text = "";
                    }                    
                }
                else
                {
                    throw new Exception("Cabinet name already exist, Please choose different name!");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_CabinetMast>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateGridView()
        {
            try
            {
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
                gvDispRec.DataSource = ds01;
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
                TextBox txtEditCabDesc = (TextBox)row.FindControl("txtEditCabDesc");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                cabinet_mast_bal OBJ_CabinetBAL = new cabinet_mast_bal();
                OBJ_CabinetBAL.CabinetCode =lbAutoID.Text.Trim();
                OBJ_CabinetBAL.CabinetDesc = txtEditCabDesc.Text.Trim();

                string result = OBJ_CabinetBAL.UpdateCabinet();

                if (Convert.ToInt32(result) > 0)
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("Data Updated Successfully !!");
                }
                else
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("Error in updating the record !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value=ex.Message;
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
                GridViewRow row = (GridViewRow)gvDispRec.Rows[e.RowIndex];
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                // Alfresco Part Start
                string DelUUID = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds02 = new DataSet();
                ds02 = ObjClassStoreProc.IsCabinetEmpty(lbAutoID.Text.Trim());
                if (ds02.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("At First Delete the Drawers from This Cabinet.");
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

                cabinet_mast_bal OBJ_CabinetBAL = new cabinet_mast_bal();
                OBJ_CabinetBAL.CabinetCode = lbAutoID.Text.Trim();

                string result = OBJ_CabinetBAL.DeleteCabinet();
                gvDispRec.EditIndex = -1;
                PopulateGridView();
                if (result == null || result == "")
                {
                    throw new Exception("Error in Data Deletion !!");
                }
                else
                {
                    throw new Exception("Data Deleted Successfully !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
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

        private bool SearchNode(Alfresco.RepositoryWebService.Reference childReference, string NodeName)
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