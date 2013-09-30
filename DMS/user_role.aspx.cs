using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;

namespace DMS
{
    public partial class user_role : System.Web.UI.Page
    {
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
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopCompany();
                            PopulateDropdown();
                            PopulateGridView();
                            PopulateAssociatedRole();
                            divCompany.Visible = true;
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopCompany();
                            PopulateDropdown();
                            PopulateGridView();
                            PopulateAssociatedRole();
                            divCompany.Visible = false;
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
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
            PopulateDropdown();
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

                //....User
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
                ddUser.DataValueField = "user_id";
                ddUser.DataBind();
                ddUser.Items.Insert(0, "Initiator User");

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
                ddRole.DataSource = ds01;
                ddRole.DataTextField = "role_name";
                ddRole.DataValueField = "role_id";
                ddRole.DataBind();
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
        /// The following function is used to insert a record in the Database's  <user_id> & <role_id> fields of <user_role> table
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
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    if (ddUser.SelectedItem.Text == "Initiator User")
                    {
                        result = ObjClassStoreProc.InsertUserRoleMast("00000000_00000000001", ddRole.SelectedValue, ddCompany.SelectedValue);
                    }
                    else
                    {
                        result = ObjClassStoreProc.InsertUserRoleMast(ddUser.SelectedValue, ddRole.SelectedValue, ddCompany.SelectedValue);
                    }
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    if (ddUser.SelectedItem.Text == "Initiator User")
                    {
                        result = ObjClassStoreProc.InsertUserRoleMast("00000000_00000000001", ddRole.SelectedValue, Session["CompCode"].ToString());
                    }
                    else
                    {
                        result = ObjClassStoreProc.InsertUserRoleMast(ddUser.SelectedValue, ddRole.SelectedValue, Session["CompCode"].ToString());
                    }
                }
                if (Convert.ToInt64(result) == -1)
                {
                    throw new Exception("Role already assigned!");
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    PopulateDropdown();
                    PopulateGridView();
                    MessageBox("Data inserted successfully");
                }


                //user_role_bal OBJ_UserRoleBAL = new user_role_bal();
                ///// Pass the <user_id><role_id> values to <user_role_bal>

                //OBJ_UserRoleBAL.UserCode = ddUser.SelectedValue;
                //OBJ_UserRoleBAL.RoleCode = ddRole.SelectedValue;

                //string result = OBJ_UserRoleBAL.InsertUserRoleMast();


                //if (result == null || result == "")
                //{
                //    MessageBox("Data inserted successfully");
                //    PopulateDropdown();
                //    PopulateGridView();
                //}
                //else
                //{
                //    /// In the Store Procedure -111 is set when the Role ID field is duplicate
                //    if (Convert.ToInt64(result) == -1)
                //    {
                //        throw new Exception("Role already assigned!");
                //    }
                //    else
                //    {
                //        MessageBox("Data inserted successfully");
                //        PopulateDropdown();
                //    }
                //}
                //PopulateGridView();
            }
            catch (Exception ex)
            {
                PopulateDropdown();
                PopulateGridView();
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_UserRoleMast>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateGridView()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.UserRoleListBasedOnCompCode(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.UserRoleListBasedOnCompCode(Session["CompCode"].ToString());
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

                DropDownList ddlEditUser = (DropDownList)row.FindControl("ddlEditUser");
                DropDownList ddlEditRole = (DropDownList)row.FindControl("ddlEditRole");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                user_role_bal OBJ_UserRoleBAL = new user_role_bal();
                OBJ_UserRoleBAL.UserCode = ddlEditUser.SelectedValue;
                OBJ_UserRoleBAL.RoleCode = ddlEditRole.SelectedValue;
                OBJ_UserRoleBAL.Labelid = lbAutoID.Text.Trim();

                string result = OBJ_UserRoleBAL.UpdateUserRole();

                if (result == "-222")
                {
                    throw new Exception("Data Already Exists !!");
                }
                else if (result == "-999")
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    PopulateAssociatedRole();
                    throw new Exception("Data Updated Successful !!");
                }
                else
                {
                    throw new Exception("Data Error !!");
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
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                /// For User Dropdown
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectUserAll(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectUserAll(Session["CompCode"].ToString());
                }
                Control ctrl1 = e.Row.FindControl("ddlEditUser");
                if (ctrl1 != null)
                {
                    DropDownList ddlEditUser = ctrl1 as DropDownList;
                    ddlEditUser.DataTextField = "name";
                    ddlEditUser.DataValueField = "user_id";
                    ddlEditUser.DataSource = ds01;
                    ddlEditUser.DataBind();
                    Control ctrl6 = e.Row.FindControl("hdUser");
                    if (ctrl6 != null)
                    {
                        HiddenField hdUser = ctrl6 as HiddenField;
                        for (int i = 0; i < ddlEditUser.Items.Count; i++)
                        {
                            if (ddlEditUser.Items[i].Text == hdUser.Value)
                            {
                                ddlEditUser.ClearSelection();
                                ddlEditUser.Items[i].Selected = true;
                            }
                        }
                        ddlEditUser.Items.Insert(0, "Initiator User");
                    }
                }
                /// For Role Dropdown
                Control ctrl2 = e.Row.FindControl("ddlEditRole");
                if (ctrl2 != null)
                {
                    DropDownList ddlEditRole = ctrl2 as DropDownList;
                    ds01.Reset();
                    if (Session["UserType"].ToString() == "S") // Super Admin
                    {
                        ds01 = ObjClassStoreProc.RoleBasedOnCompCode(ddCompany.SelectedValue);
                    }
                    else if (Session["UserType"].ToString() == "A") // Admin
                    {
                        ds01 = ObjClassStoreProc.RoleBasedOnCompCode(Session["CompCode"].ToString());
                    }
                    ddlEditRole.DataTextField = "role_name";
                    ddlEditRole.DataValueField = "role_id";
                    ddlEditRole.DataSource = ds01;
                    ddlEditRole.DataBind();
                    Control ctrl7 = e.Row.FindControl("hdRole");
                    if (ctrl7 != null)
                    {
                        HiddenField hdRole = ctrl7 as HiddenField;
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

                user_role_bal OBJ_UserRoleBAL = new user_role_bal();
                OBJ_UserRoleBAL.Labelid = lbAutoID.Text.Trim();
                if (lbAutoID.Text.Trim() == "INIT")
                {
                    throw new Exception("You can't delete this Data!!");
                }
                string result = OBJ_UserRoleBAL.DeleteUserRole();

                if (result == null || result == "")
                {
                    throw new Exception("Error in Data Deletion !!");
                }
                else
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("Data Deleted Successfully !!");
                }
            }
            catch (Exception ex)
            {
                //MessageBox(ex.Message);
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

        protected void ddRole_SelectedIndexChanged(object sender, EventArgs e)
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
                PopulateAssociatedRole();                
            }
            catch(Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateAssociatedRole()
        {
            DBClass DBObj = new DBClass();
            DataSet ds1 = new DataSet();
            ds1 = DBObj.GVAssociatedRole(ddRole.SelectedValue, Session["CompCode"].ToString());
            gvAssociatedRole.DataSource = ds1;
            gvAssociatedRole.DataBind();
        }

        protected void gvAssociatedRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            gvAssociatedRole.PageIndex = e.NewPageIndex;
            PopulateAssociatedRole();
        }

    }
}