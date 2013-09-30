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
    public partial class role_mast : System.Web.UI.Page
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
        /// The following function is used to insert a record in the Database's  <role_id> & <role_name> fields of <role_mast> table
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

                role_mast_bal OBJ_RoleBAL = new role_mast_bal();
                OBJ_RoleBAL.RoleCode = txtRoleCode.Text.Trim().ToUpper();
                OBJ_RoleBAL.RoleName = txtRoleName.Text.Trim();

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = ObjClassStoreProc.InsertRoleMast(txtRoleCode.Text.Trim().ToUpper(), txtRoleName.Text.Trim(), ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    result = ObjClassStoreProc.InsertRoleMast(txtRoleCode.Text.Trim().ToUpper(), txtRoleName.Text.Trim(), Session["CompCode"].ToString());
                }
                if (Convert.ToInt64(result) == -1)
                {
                    throw new Exception("Role Code already exists!");
                }
                else if (Convert.ToInt64(result) == -2)
                {
                    throw new Exception("Role Name already exists!");
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    txtRoleCode.Text = "";
                    txtRoleName.Text = "";
                    PopulateDropdown();
                    PopulateGridView();
                    MessageBox("Data inserted successfully");
                }

                //role_mast_bal OBJ_RoleBAL = new role_mast_bal();
                ///// Pass the <role_id><role_name> values to <role_mast_bal>

                //OBJ_RoleBAL.RoleCode = txtRoleCode.Text.Trim().ToUpper();
                //OBJ_RoleBAL.RoleName = txtRoleName.Text.Trim();

                //result = OBJ_RoleBAL.InsertRoleMast();


                //if (result == null || result == "")
                //{
                //    MessageBox("Data inserted successfully");
                //    txtRoleCode.Text = "";
                //    txtRoleName.Text = "";
                //}
                //else
                //{
                //    /// In the Store Procedure -111 is set when the ID field is duplicate &
                //    /// -222 is set when the Name field is duplicate
                //    if (Convert.ToInt64(result) == -1)
                //    {
                //        throw new Exception("Role Code already exists!");
                //    }
                //    else if (Convert.ToInt64(result) == -2)
                //    {
                //        throw new Exception("Role Name already exists!");
                //    }
                //    else
                //    {
                //        MessageBox("Data inserted successfully");
                //        txtRoleCode.Text = "";
                //        txtRoleName.Text = "";
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                PopulateGridView();
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_RoleMast>RoleBasedOnCompCode
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
                    ds01 = ObjClassStoreProc.RoleBasedOnCompCode(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.RoleBasedOnCompCode(Session["CompCode"].ToString());
                }
                gvDispRec.DataSource = ds01;
                gvDispRec.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
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
                gvDispRec.EditIndex = e.NewEditIndex;
                PopulateGridView();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
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
                int rIndex = e.RowIndex;
                GridViewRow row = (GridViewRow)gvDispRec.Rows[e.RowIndex];
                TextBox txtEditRoleName = (TextBox)row.FindControl("txtEditRoleName");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                role_mast_bal OBJ_RoleBAL = new role_mast_bal();
                OBJ_RoleBAL.RoleCode = lbAutoID.Text.Trim();
                OBJ_RoleBAL.RoleName = txtEditRoleName.Text.Trim();
                if (lbAutoID.Text.Trim() == "INIT")
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("You can't edit this Role!!");
                }
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = OBJ_RoleBAL.UpdateRole(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    result = OBJ_RoleBAL.UpdateRole(Session["CompCode"].ToString());
                }

                if (result == "-222")
                {
                    throw new Exception("Data Already Exists !!");
                }
                else if (result == "-999")
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("Data Updated Successfully !!");
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
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                role_mast_bal OBJ_RoleBAL = new role_mast_bal();
                OBJ_RoleBAL.RoleCode = lbAutoID.Text.Trim();
                if (lbAutoID.Text.Trim() == "INIT")
                {
                    throw new Exception("You can't delete this Role!!");
                }
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = OBJ_RoleBAL.Deleterole(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    result = OBJ_RoleBAL.Deleterole(Session["CompCode"].ToString());
                }

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
    }
}