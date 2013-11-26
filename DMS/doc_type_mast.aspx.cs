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
    public partial class doc_type_mast : System.Web.UI.Page
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
                            SetFields();
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
                            SetFields();
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
            PopulateGridView();
        }

        protected void ddFormType_SelectedIndexChanged(object sender, EventArgs e)
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
                SetFields();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void SetFields()
        {
            try
            {
                if (ddFormType.SelectedValue == "Non Editable")
                {
                    divSignFieldNo1.Visible = false;
                    divSignFieldNo2.Visible = false;
                    divSignFieldNo3.Visible = false;
                    divSignDateFieldNo1.Visible = false;
                    divSignDateFieldNo2.Visible = false;
                    divSignDateFieldNo3.Visible = false;
                    txtTag1FieldNo.Text = "0";
                    txtTag2FieldNo.Text = "0";
                    txtTag3FieldNo.Text = "0";
                    txtTag4FieldNo.Text = "0";
                    txtTag5FieldNo.Text = "0";
                    txtTag6FieldNo.Text = "0";
                    txtTag7FieldNo.Text = "0";
                    txtTag8FieldNo.Text = "0";
                    txtTag9FieldNo.Text = "0";
                    txtTag10FieldNo.Text = "0";
                }
                else if (ddFormType.SelectedValue == "Editable")
                {
                    divSignFieldNo1.Visible = true;
                    divSignFieldNo2.Visible = true;
                    divSignFieldNo3.Visible = true;
                    divSignDateFieldNo1.Visible = true;
                    divSignDateFieldNo2.Visible = true;
                    divSignDateFieldNo3.Visible = true;
                    txtTag1FieldNo.Text = "0";
                    txtTag2FieldNo.Text = "0";
                    txtTag3FieldNo.Text = "0";
                    txtTag4FieldNo.Text = "0";
                    txtTag5FieldNo.Text = "0";
                    txtTag6FieldNo.Text = "0";
                    txtTag7FieldNo.Text = "0";
                    txtTag8FieldNo.Text = "0";
                    txtTag9FieldNo.Text = "0";
                    txtTag10FieldNo.Text = "0";
                }
                else if (ddFormType.SelectedValue == "eForm")
                {
                    divSignFieldNo1.Visible = false;
                    divSignFieldNo2.Visible = false;
                    divSignFieldNo3.Visible = false;
                    divSignDateFieldNo1.Visible = false;
                    divSignDateFieldNo2.Visible = false;
                    divSignDateFieldNo3.Visible = false;
                    txtTag1FieldNo.Text = "0";
                    txtTag2FieldNo.Text = "0";
                    txtTag3FieldNo.Text = "0";
                    txtTag4FieldNo.Text = "0";
                    txtTag5FieldNo.Text = "0";
                    txtTag6FieldNo.Text = "0";
                    txtTag7FieldNo.Text = "0";
                    txtTag8FieldNo.Text = "0";
                    txtTag9FieldNo.Text = "0";
                    txtTag10FieldNo.Text = "0";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        /// The following function is used to insert a record in the Database's
        /// <doc_type_id>,<doc_type_name>,<tag1>,<tag2>,<tag3>,<tag4>,<tag5>,<tag6>,<tag7>,<tag8>,<tag9>,<tag10> fields of <doc_type_mast> table
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

                doc_type_mast_bal OBJ_DocTypeBAL = new doc_type_mast_bal();
                OBJ_DocTypeBAL.DocTypeID = txtDocTypeID.Text.Trim().ToUpper();
                OBJ_DocTypeBAL.DocTypeName = txtDocTypeName.Text.Trim();
                OBJ_DocTypeBAL.Tag1 = txtTag1.Text.Trim();
                OBJ_DocTypeBAL.Tag2 = txtTag2.Text.Trim();
                OBJ_DocTypeBAL.Tag3 = txtTag3.Text.Trim();
                OBJ_DocTypeBAL.Tag4 = txtTag4.Text.Trim();
                OBJ_DocTypeBAL.Tag5 = txtTag5.Text.Trim();
                OBJ_DocTypeBAL.Tag6 = txtTag6.Text.Trim();
                OBJ_DocTypeBAL.Tag7 = txtTag7.Text.Trim();
                OBJ_DocTypeBAL.Tag8 = txtTag8.Text.Trim();
                OBJ_DocTypeBAL.Tag9 = txtTag9.Text.Trim();
                OBJ_DocTypeBAL.Tag10 = txtTag10.Text.Trim();
                OBJ_DocTypeBAL.Tag1FieldNo = Convert.ToInt32(txtTag1FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag2FieldNo = Convert.ToInt32(txtTag2FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag3FieldNo = Convert.ToInt32(txtTag3FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag4FieldNo = Convert.ToInt32(txtTag4FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag5FieldNo = Convert.ToInt32(txtTag5FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag6FieldNo = Convert.ToInt32(txtTag6FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag7FieldNo = Convert.ToInt32(txtTag7FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag8FieldNo = Convert.ToInt32(txtTag8FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag9FieldNo = Convert.ToInt32(txtTag9FieldNo.Text.Trim());
                OBJ_DocTypeBAL.Tag10FieldNo = Convert.ToInt32(txtTag10FieldNo.Text.Trim());
                OBJ_DocTypeBAL.SignFieldNo1 = Convert.ToInt32(txtSignFieldNo1.Text.Trim());
                OBJ_DocTypeBAL.SignDateFieldNo1 = Convert.ToInt32(txtSignDateFieldNo1.Text.Trim());
                OBJ_DocTypeBAL.SignFieldNo2 = Convert.ToInt32(txtSignFieldNo2.Text.Trim());
                OBJ_DocTypeBAL.SignDateFieldNo2 = Convert.ToInt32(txtSignDateFieldNo2.Text.Trim());
                OBJ_DocTypeBAL.SignFieldNo3 = Convert.ToInt32(txtSignFieldNo3.Text.Trim());
                OBJ_DocTypeBAL.SignDateFieldNo3 = Convert.ToInt32(txtSignDateFieldNo3.Text.Trim());
                
                string result = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = ObjClassStoreProc.InsertDocTypeMast(txtDocTypeID.Text.Trim().ToUpper(), txtDocTypeName.Text.Trim(), txtTag1.Text.Trim(), txtTag2.Text.Trim(), txtTag3.Text.Trim(), txtTag4.Text.Trim(), txtTag5.Text.Trim(), txtTag6.Text.Trim(), txtTag7.Text.Trim(), txtTag8.Text.Trim(), txtTag9.Text.Trim(), txtTag10.Text.Trim(), Convert.ToInt32(txtTag1FieldNo.Text.Trim()), Convert.ToInt32(txtTag2FieldNo.Text.Trim()), Convert.ToInt32(txtTag3FieldNo.Text.Trim()), Convert.ToInt32(txtTag4FieldNo.Text.Trim()), Convert.ToInt32(txtTag5FieldNo.Text.Trim()), Convert.ToInt32(txtTag6FieldNo.Text.Trim()), Convert.ToInt32(txtTag7FieldNo.Text.Trim()), Convert.ToInt32(txtTag8FieldNo.Text.Trim()), Convert.ToInt32(txtTag9FieldNo.Text.Trim()), Convert.ToInt32(txtTag10FieldNo.Text.Trim()), Convert.ToInt32(txtSignFieldNo1.Text.Trim()), Convert.ToInt32(txtSignDateFieldNo1.Text.Trim()), Convert.ToInt32(txtSignFieldNo2.Text.Trim()), Convert.ToInt32(txtSignDateFieldNo2.Text.Trim()), Convert.ToInt32(txtSignFieldNo3.Text.Trim()), Convert.ToInt32(txtSignDateFieldNo3.Text.Trim()), ddCompany.SelectedValue, ddFormType.SelectedValue);
                }
                else
                {
                    result = ObjClassStoreProc.InsertDocTypeMast(txtDocTypeID.Text.Trim().ToUpper(), txtDocTypeName.Text.Trim(), txtTag1.Text.Trim(), txtTag2.Text.Trim(), txtTag3.Text.Trim(), txtTag4.Text.Trim(), txtTag5.Text.Trim(), txtTag6.Text.Trim(), txtTag7.Text.Trim(), txtTag8.Text.Trim(), txtTag9.Text.Trim(), txtTag10.Text.Trim(), Convert.ToInt32(txtTag1FieldNo.Text.Trim()), Convert.ToInt32(txtTag2FieldNo.Text.Trim()), Convert.ToInt32(txtTag3FieldNo.Text.Trim()), Convert.ToInt32(txtTag4FieldNo.Text.Trim()), Convert.ToInt32(txtTag5FieldNo.Text.Trim()), Convert.ToInt32(txtTag6FieldNo.Text.Trim()), Convert.ToInt32(txtTag7FieldNo.Text.Trim()), Convert.ToInt32(txtTag8FieldNo.Text.Trim()), Convert.ToInt32(txtTag9FieldNo.Text.Trim()), Convert.ToInt32(txtTag10FieldNo.Text.Trim()), Convert.ToInt32(txtSignFieldNo1.Text.Trim()), Convert.ToInt32(txtSignDateFieldNo1.Text.Trim()), Convert.ToInt32(txtSignFieldNo2.Text.Trim()), Convert.ToInt32(txtSignDateFieldNo2.Text.Trim()), Convert.ToInt32(txtSignFieldNo3.Text.Trim()), Convert.ToInt32(txtSignDateFieldNo3.Text.Trim()), Session["CompCode"].ToString(), ddFormType.SelectedValue);
                }
                if (Convert.ToInt64(result)>0)
                {
                    MessageBox("Data inserted successfully");
                    PopulateDropdown();
                    PopulateGridView();
                    txtDocTypeID.Text = "";
                    txtDocTypeName.Text = "";
                    txtTag1.Text = "";
                    txtTag2.Text = "";
                    txtTag3.Text = "";
                    txtTag4.Text = "";
                    txtTag5.Text = "";
                    txtTag6.Text = "";
                    txtTag7.Text = "";
                    txtTag8.Text = "";
                    txtTag9.Text = "";
                    txtTag10.Text = "";
                    txtTag1FieldNo.Text = "0";
                    txtTag2FieldNo.Text = "0";
                    txtTag3FieldNo.Text = "0";
                    txtTag4FieldNo.Text = "0";
                    txtTag5FieldNo.Text = "0";
                    txtTag6FieldNo.Text = "0";
                    txtTag7FieldNo.Text = "0";
                    txtTag8FieldNo.Text = "0";
                    txtTag9FieldNo.Text = "0";
                    txtTag10FieldNo.Text = "0";
                    txtSignFieldNo1.Text = "0";
                    txtSignDateFieldNo1.Text = "0";
                    txtSignFieldNo2.Text = "0";
                    txtSignDateFieldNo2.Text = "0";
                    txtSignFieldNo3.Text = "0";
                    txtSignDateFieldNo3.Text = "0";
                }
                else if (Convert.ToInt64(result) == -1)
                {
                    throw new Exception("Doc Type ID already exists!");
                }
                else if (Convert.ToInt64(result) == -2)
                {
                    throw new Exception("Doc Type Name already exists!");
                }
            }
            catch (Exception ex)
            {
                PopulateGridView();
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_DocTypeMast>
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
                    ds01 = ObjClassStoreProc.SelectDocTypeCompBased(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectDocTypeCompBased(Session["CompCode"].ToString());
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
                TextBox txtEditDocTypeName = (TextBox)row.FindControl("txtEditDocTypeName");
                TextBox txtEditTag1 = (TextBox)row.FindControl("txtEditTag1");
                TextBox txtEditTag2 = (TextBox)row.FindControl("txtEditTag2");
                TextBox txtEditTag3 = (TextBox)row.FindControl("txtEditTag3");
                TextBox txtEditTag4 = (TextBox)row.FindControl("txtEditTag4");
                TextBox txtEditTag5 = (TextBox)row.FindControl("txtEditTag5");
                TextBox txtEditTag6 = (TextBox)row.FindControl("txtEditTag6");
                TextBox txtEditTag7 = (TextBox)row.FindControl("txtEditTag7");
                TextBox txtEditTag8 = (TextBox)row.FindControl("txtEditTag8");
                TextBox txtEditTag9 = (TextBox)row.FindControl("txtEditTag9");
                TextBox txtEditTag10 = (TextBox)row.FindControl("txtEditTag10");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                doc_type_mast_bal OBJ_DocTypeBAL = new doc_type_mast_bal();
                OBJ_DocTypeBAL.DocTypeID = lbAutoID.Text.Trim();
                OBJ_DocTypeBAL.DocTypeName = txtEditDocTypeName.Text.Trim();
                OBJ_DocTypeBAL.Tag1 = txtEditTag1.Text.Trim();
                OBJ_DocTypeBAL.Tag2 = txtEditTag2.Text.Trim();
                OBJ_DocTypeBAL.Tag3 = txtEditTag3.Text.Trim();
                OBJ_DocTypeBAL.Tag4 = txtEditTag4.Text.Trim();
                OBJ_DocTypeBAL.Tag5 = txtEditTag5.Text.Trim();
                OBJ_DocTypeBAL.Tag6 = txtEditTag6.Text.Trim();
                OBJ_DocTypeBAL.Tag7 = txtEditTag7.Text.Trim();
                OBJ_DocTypeBAL.Tag8 = txtEditTag8.Text.Trim();
                OBJ_DocTypeBAL.Tag9 = txtEditTag9.Text.Trim();
                OBJ_DocTypeBAL.Tag10 = txtEditTag10.Text.Trim();

                string result = OBJ_DocTypeBAL.UpdateDocType(Session["CompCode"].ToString());

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

                doc_type_mast_bal OBJ_DocTypeBAL = new doc_type_mast_bal();
                OBJ_DocTypeBAL.DocTypeID = lbAutoID.Text.Trim();
                if (OBJ_DocTypeBAL.DocTypeID == "GENERAL")
                {
                    throw new Exception("You can not Delete this Document Type.");
                }

                string result = OBJ_DocTypeBAL.DeleteDocType(Session["CompCode"].ToString());
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
                MessageBox(ex.Message);
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