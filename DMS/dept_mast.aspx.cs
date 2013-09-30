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
    public partial class dept_mast : System.Web.UI.Page
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
        /// The following function is used to insert a record in the Database's  <dept_id> & <dept_name> fields of <dept_mast> table
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

                dept_mast_bal OBJ_DeptBAL = new dept_mast_bal();
                OBJ_DeptBAL.DeptCode = txtDeptCode.Text.Trim().ToUpper();
                OBJ_DeptBAL.DeptName = txtDeptName.Text.Trim();

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = ObjClassStoreProc.InsertDeptMast(txtDeptCode.Text.Trim().ToUpper(), txtDeptName.Text.Trim(), ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    result = ObjClassStoreProc.InsertDeptMast(txtDeptCode.Text.Trim().ToUpper(), txtDeptName.Text.Trim(), Session["CompCode"].ToString());
                }
                if (Convert.ToInt64(result) == -1)
                {
                    throw new Exception("Department ID already exists!");
                }
                else if (Convert.ToInt64(result) == -2)
                {
                    throw new Exception("Department Name already exists!");
                }
                else if (Convert.ToInt64(result) >0)
                {
                    txtDeptCode.Text = "";
                    txtDeptName.Text = "";
                    PopulateDropdown();
                    PopulateGridView();
                    throw new Exception("Data inserted successfully");
                }
            }
            catch (Exception ex)
            {
                PopulateGridView();
                MessageBox(ex.Message);
            }
        }
        
        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<DeptMast_GV>
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
                    ds01 = ObjClassStoreProc.DeptAllBasedOnCompCode(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.DeptAllBasedOnCompCode(Session["CompCode"].ToString());
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
                TextBox txtEditDeptName = (TextBox)row.FindControl("txtEditDeptName");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                dept_mast_bal OBJ_DeptBAL = new dept_mast_bal();
                OBJ_DeptBAL.DeptCode = lbAutoID.Text.Trim();
                OBJ_DeptBAL.DeptName = txtEditDeptName.Text.Trim();
                if (lbAutoID.Text.Trim() == "ADM" || lbAutoID.Text.Trim() == "MISC")
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("You can't edit this record !!");
                }
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = OBJ_DeptBAL.UpdateDept(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    result = OBJ_DeptBAL.UpdateDept(Session["CompCode"].ToString());
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
                    throw new Exception("Data Updation Error !!");
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

                dept_mast_bal OBJ_DeptBAL = new dept_mast_bal();
                OBJ_DeptBAL.DeptCode = lbAutoID.Text.Trim();
                if (lbAutoID.Text.Trim() == "ADM" || lbAutoID.Text.Trim() == "MISC")
                {
                    throw new Exception("You can't delete this record !!");
                }
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = OBJ_DeptBAL.DeleteDept(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    result = OBJ_DeptBAL.DeleteDept(Session["CompCode"].ToString());
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

    }
}