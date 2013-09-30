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
    public partial class drawer_mast : System.Web.UI.Page
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
                MessageBox(ex.Message);
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
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
        /// The following function is used to insert a record in the Database's  <drw_name>,<drw_desc> & <cab_id> fields of <drw_mast> table
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
                if (txtDrwName.Text.LastIndexOf("\\") != -1 || txtDrwName.Text.LastIndexOf("/") != -1)
                {
                    throw new Exception("Drawer Name can not accept any special character");
                }

                drawer_mast_bal OBJ_DrawerBAL = new drawer_mast_bal();
                OBJ_DrawerBAL.DrawerName = txtDrwName.Text.Trim();
                OBJ_DrawerBAL.DrawerDesc = txtDrwDesc.Text.Trim();
                OBJ_DrawerBAL.CabinetCode = ddCabinet.SelectedValue;

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.ExistNode("Drawer", txtDrwName.Text.Trim(), ddCompany.SelectedValue, ddCabinet.SelectedValue);
                }
                else // Admin and Normal User
                {
                    ds01 = ObjClassStoreProc.ExistNode("Drawer", txtDrwName.Text.Trim(), Session["CompCode"].ToString(), ddCabinet.SelectedValue);
                }
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("Drawer name already exist, Please choose different name!");
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
                reference.uuid = ddCabinet.SelectedValue;
                if (!SearchNode(reference, txtDrwName.Text.Trim()))
                {
                    AlfCreateSpace ObjCreateSpace = new AlfCreateSpace();
                    Session["NodeUUID"] = ObjCreateSpace.CreateSpace(txtDrwName.Text.Trim(), txtDrwDesc.Text.Trim(), null, ddCabinet.SelectedValue, ddCabinet.SelectedItem.Text, wsF.UserName, wsF.Ticket);
                    string result = "";
                    if (Session["UserType"].ToString() == "S") // Super Admin
                    {
                        result = ObjClassStoreProc.InsertDrawerMast(txtDrwName.Text.Trim(), txtDrwDesc.Text.Trim(), ddCabinet.SelectedValue, Session["NodeUUID"].ToString(), ddCompany.SelectedValue);
                    }
                    else
                    {
                        result = ObjClassStoreProc.InsertDrawerMast(txtDrwName.Text.Trim(), txtDrwDesc.Text.Trim(), ddCabinet.SelectedValue, Session["NodeUUID"].ToString(), Session["CompCode"].ToString());
                    }

                    if (Convert.ToInt64(result) > 0)
                    {
                        UserRights RightsObj = new UserRights();
                        //RightsObj.SetPermissions(OBJ_DrawerBAL.UUID, "Drawer", Session["UserID"].ToString(),"X");
                        
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            ds01.Reset();
                            ds01 = RightsObj.FetchPermission(ddCabinet.SelectedValue,ddCompany.SelectedValue);
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                {
                                    RightsObj.InsertPermissionSingleData(Session["NodeUUID"].ToString(), "Drawer", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), ddCompany.SelectedValue);
                                }
                            }
                        }
                        else // Admin and Normal User
                        {
                            ds01.Reset();
                            ds01 = RightsObj.FetchPermission(ddCabinet.SelectedValue, Session["CompCode"].ToString());
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                {
                                    RightsObj.InsertPermissionSingleData(Session["NodeUUID"].ToString(), "Drawer", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                                }
                            }
                        }
                        //RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Drawer", Session["UserID"].ToString(), result);

                        MessageBox("Data inserted successfully");
                        PopulateDropdown();
                        PopulateGridView();
                        txtDrwName.Text = "";
                        txtDrwDesc.Text = "";
                        txtLocation.Text = "";
                    }
                    else if (Convert.ToInt64(result) == -1)
                    {
                        throw new Exception("Data already exists!");
                    }
                }
                else
                {
                    throw new Exception("Drawer name already exist, Please choose different name!");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_DrawerMast>
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
                    ds1 = DBObj.GVDrawer(ddCompany.SelectedValue, Session["UserID"].ToString());
                }
                else
                {
                    ds1 = DBObj.GVDrawer(Session["CompCode"].ToString(), Session["UserID"].ToString());
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
                TextBox txtEditDrwDesc = (TextBox)row.FindControl("txtEditDrwDesc");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                drawer_mast_bal OBJ_DrawerBAL = new drawer_mast_bal();
                OBJ_DrawerBAL.DrawerDesc = txtEditDrwDesc.Text.Trim();
                OBJ_DrawerBAL.Labelid = lbAutoID.Text.Trim();

                string result = OBJ_DrawerBAL.UpdateDrawer();

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
                Control ctrl1 = e.Row.FindControl("ddlEditCabinet");
                if (ctrl1 != null)
                {
                    DropDownList ddlEditCabinet = ctrl1 as DropDownList;
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                    ddlEditCabinet.DataTextField = "cab_name";
                    ddlEditCabinet.DataValueField = "cab_uuid";
                    ddlEditCabinet.DataSource = ds01;
                    ddlEditCabinet.DataBind();
                    Control ctrl6 = e.Row.FindControl("hdCabinet");
                    if (ctrl6 != null)
                    {
                        HiddenField hdCabinet = ctrl6 as HiddenField;
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
                int rIndex = e.RowIndex;
                GridViewRow row = (GridViewRow)gvDispRec.Rows[e.RowIndex];
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                // Alfresco Part Start
                string DelUUID = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                //SqlConnection con = Utility.GetConnection();
                //SqlCommand cmd = null;
                //DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                //con.Open();
                //cmd = new SqlCommand("select * from folder_mast where drw_uuid='" + lbAutoID.Text.Trim() + "'", con);
                //SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                //adapter02.Fill(ds02);
                //if (ds02.Tables[0].Rows.Count > 0)
                //{
                    //throw new Exception("At First Delete the Folders from This Drawer.");
                //}
                ds02 = ObjClassStoreProc.AllFoldersInsideDrawer(lbAutoID.Text.Trim());
                if (ds02.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("At First Delete the Folders from This Drawer.");
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

                drawer_mast_bal OBJ_DrawerBAL = new drawer_mast_bal();
                OBJ_DrawerBAL.Labelid = lbAutoID.Text.Trim();

                string result = OBJ_DrawerBAL.DeleteDrawer();

                PopulateGridView();
                gvDispRec.EditIndex = -1;
                if (Convert.ToInt32(result) > 0)
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