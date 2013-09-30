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
    public partial class task_mast : System.Web.UI.Page
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
                        if (Session["UserID"].ToString() == "admin")
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopulateGridView();
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
        /// The following function is used to insert a record in the Database's  <task_id> & <task_name> fields of <task_mast> table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddMaster_Click(object sender, EventArgs e)
        {
            try
            {
                task_mast_bal OBJ_TaskBAL = new task_mast_bal();
                /// Pass the <task_id><task_name> values to <task_mast_bal>

                OBJ_TaskBAL.TaskCode = txtTaskCode.Text.Trim().ToUpper();
                OBJ_TaskBAL.TaskName = txtTaskName.Text.Trim();

                string result = OBJ_TaskBAL.InsertTaskMast();


                if (result == null || result == "")
                {
                    MessageBox("Data inserted successfully");
                    txtTaskCode.Text = "";
                    txtTaskName.Text = "";
                }
                else
                {
                    /// In the Store Procedure -111 is set when the ID field is duplicate &
                    /// -222 is set when the Name field is duplicate
                    if (Convert.ToInt64(result) == -111)
                    {
                        throw new Exception("Task Code already exists!");
                    }
                    else if (Convert.ToInt64(result) == -222)
                    {
                        throw new Exception("Task Name already exists!");
                    }
                    else
                    {
                        MessageBox("Data inserted successfully");
                        txtTaskCode.Text = "";
                        txtTaskName.Text = "";
                    }
                }
                PopulateGridView();
            }
            catch (Exception ex)
            {
                PopulateGridView();
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_TaskMast>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateGridView()
        {
            try
            {
                DBClass DBObj = new DBClass();
                DataSet ds1 = new DataSet();
                ds1 = DBObj.GVTask();
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
                int rIndex = e.RowIndex;
                GridViewRow row = (GridViewRow)gvDispRec.Rows[e.RowIndex];
                TextBox txtEditTaskName = (TextBox)row.FindControl("txtEditTaskName");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                task_mast_bal OBJ_TaskBAL = new task_mast_bal();
                OBJ_TaskBAL.TaskCode = lbAutoID.Text.Trim();
                OBJ_TaskBAL.TaskName = txtEditTaskName.Text.Trim();

                string result = OBJ_TaskBAL.UpdateTask();

                if (result == "-222")
                {
                    MessageBox("Data Already Exists !!");
                }
                else if (result == "-999")
                {
                    MessageBox("Data Updated Successfully !!");
                }
                else
                {
                    MessageBox("Data Updation Error !!");
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
        /// To cancel after clicking on edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispRec_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
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
                GridViewRow row = (GridViewRow)gvDispRec.Rows[e.RowIndex];
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                task_mast_bal OBJ_TaskBAL = new task_mast_bal();
                OBJ_TaskBAL.TaskCode = lbAutoID.Text.Trim();

                string result = OBJ_TaskBAL.DeleteTask();

                if (result == null || result == "")
                {
                    MessageBox("Error in Data Deletion !!");
                }
                else
                {
                    MessageBox("Data Deleted Successfully !!");
                }

                gvDispRec.EditIndex = -1;
                PopulateGridView();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvDispRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDispRec.PageIndex = e.NewPageIndex;
            PopulateGridView();
        }

    }
}