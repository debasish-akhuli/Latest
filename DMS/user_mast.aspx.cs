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
using Alfresco.AdministrationWebService;
using Alfresco.RepositoryWebService;
using DMS.UTILITY;
using System.Text.RegularExpressions;

namespace DMS
{
    public partial class user_mast : System.Web.UI.Page
    {
        private AdministrationService administrationService;
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;
        Alfresco.RepositoryWebService.Reference reference;

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
                            divCompany.Visible = true;
                            divUserType.Visible = true;
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
                            divCompany.Visible = false;
                            divUserType.Visible = false;
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
        /// The following function is used to insert a record in the Database's  <user_id>,<f_name>,<l_name>,<email>,<user_pwd>,<user_title>,<user_dept>,<user_stat> fields of <user_mast> table
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

                user_mast_bal OBJ_UserBAL = new user_mast_bal();
                OBJ_UserBAL.FName = txtFName.Text.Trim();
                OBJ_UserBAL.LName = txtLName.Text.Trim();
                OBJ_UserBAL.EMail = txtMail.Text.Trim();
                OBJ_UserBAL.UserPwd = txtPwd.Text.Trim();
                OBJ_UserBAL.Title = txtTitle.Text.Trim();
                OBJ_UserBAL.Dept = ddDept.SelectedValue;
                OBJ_UserBAL.Stat = ddStat.SelectedValue;
                OBJ_UserBAL.PwdStat = "";
                OBJ_UserBAL.CanChangePwd = ddCanChangePwd.SelectedValue;

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                /// .Net & SQL Server Part Start
                DataSet ds01 = new DataSet();
                string result = "";
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    txtUserID.Text = ObjClassStoreProc.SelectMaxUserID(ddCompany.SelectedValue);
                    result = ObjClassStoreProc.InsertUserMast(txtUserID.Text.Trim().ToLower(), txtFName.Text.Trim(), txtLName.Text.Trim(), txtMail.Text.Trim(), txtPwd.Text.Trim(), txtTitle.Text.Trim(), ddDept.SelectedValue, ddStat.SelectedValue, "", ddCanChangePwd.SelectedValue, ddCompany.SelectedValue, ddUserType.SelectedValue,DateTime.Now);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    #region Check total no of users which are already in the system
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                    cmd = new SqlCommand("select count(*) as Tot from user_mast where CompCode='" + Session["CompCode"].ToString() + "' and user_stat='A'", con);
                    int TotUser = Convert.ToInt32(cmd.ExecuteScalar());
                    if (TotUser < Convert.ToInt32(ds01.Tables[0].Rows[0][16].ToString()))
                    {

                    }
                    else
                    {
                        throw new Exception("You need to change your Billing option. Please contact with Administrator.");
                    }
                    #endregion

                    txtUserID.Text = ObjClassStoreProc.SelectMaxUserID(Session["CompCode"].ToString());
                    result = ObjClassStoreProc.InsertUserMast(txtUserID.Text.Trim().ToLower(), txtFName.Text.Trim(), txtLName.Text.Trim(), txtMail.Text.Trim(), txtPwd.Text.Trim(), txtTitle.Text.Trim(), ddDept.SelectedValue, ddStat.SelectedValue, "", ddCanChangePwd.SelectedValue, Session["CompCode"].ToString(), "N",DateTime.Now);
                    con.Close();
                }
                
                if (Convert.ToInt64(result) >0)
                {
                    /// Alfresco Part Start
                    WebServiceFactory wsFA = new WebServiceFactory();
                    wsFA.UserName = Session["AdmUserID"].ToString();
                    wsFA.Ticket = Session["AdmTicket"].ToString();
                    this.administrationService = wsFA.getAdministrationService();

                    Alfresco.AdministrationWebService.NewUserDetails[] newUsers = new NewUserDetails[1];
                    Alfresco.AdministrationWebService.NewUserDetails userDetails = new NewUserDetails();
                    userDetails.userName = txtUserID.Text.Trim().ToLower();
                    userDetails.password = txtPwd.Text.Trim();
                    Alfresco.AdministrationWebService.NamedValue[] properties = new Alfresco.AdministrationWebService.NamedValue[4];

                    /// First Name
                    Alfresco.AdministrationWebService.NamedValue namedValue = new Alfresco.AdministrationWebService.NamedValue();
                    namedValue.name = Constants.PROP_USER_FIRSTNAME;
                    namedValue.value = txtFName.Text.Trim();
                    properties[0] = namedValue;

                    /// Last Name
                    namedValue = new Alfresco.AdministrationWebService.NamedValue();
                    namedValue.name = Constants.PROP_USER_LASTNAME;
                    namedValue.value = txtLName.Text.Trim();
                    properties[1] = namedValue;

                    /// EMail
                    namedValue = new Alfresco.AdministrationWebService.NamedValue();
                    namedValue.name = Constants.PROP_USER_EMAIL;
                    namedValue.value = txtMail.Text.Trim();
                    properties[2] = namedValue;

                    /// Title
                    namedValue = new Alfresco.AdministrationWebService.NamedValue();
                    namedValue.name = Constants.PROP_TITLE;
                    namedValue.value = txtTitle.Text.Trim();
                    properties[3] = namedValue;
                    userDetails.properties = properties;
                    newUsers[0] = userDetails;
                    UserDetails[] responseUserDetails = administrationService.createUsers(newUsers);
                    /// Alfresco Part End
                    
                    UserRights UserObj = new UserRights();
                    if (ddDefaultPermission.SelectedValue == "Y")
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            UserObj.SetUserPermissions(txtUserID.Text.Trim().ToLower(), ddCompany.SelectedValue);
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            UserObj.SetUserPermissions(txtUserID.Text.Trim().ToLower(), Session["CompCode"].ToString());
                        }
                    }
                    else
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            UserObj.SetUserNoPermission(txtUserID.Text.Trim().ToLower(), ddCompany.SelectedValue);
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            UserObj.SetUserNoPermission(txtUserID.Text.Trim().ToLower(), Session["CompCode"].ToString());
                        }
                    }
                    txtUserID.Text = "";
                    txtFName.Text = "";
                    txtLName.Text = "";
                    txtMail.Text = "";
                    txtPwd.Text = "";
                    txtTitle.Text = "";
                    PopCompany();
                    PopulateDropdown();
                    PopulateGridView();
                    throw new Exception("Data inserted successfully");
                }
                else if (Convert.ToInt64(result) == 0)
                {
                    throw new Exception("This Email ID is already registered.");
                }
                else if (Convert.ToInt64(result) < 0)
                {
                    throw new Exception("Admin User is already created for this Company.");
                }
                /// .Net & SQL Server Part End
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_UserMast>
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
                    ds01 = ObjClassStoreProc.SelectUserAll(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectUserAll(Session["CompCode"].ToString());
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

                DropDownList ddlEditDept = (DropDownList)row.FindControl("ddlEditDept");
                DropDownList ddlEditStat = (DropDownList)row.FindControl("ddlEditStat");
                TextBox txtEditFName = (TextBox)row.FindControl("txtEditFName");
                TextBox txtEditLName = (TextBox)row.FindControl("txtEditLName");
                TextBox txtEditEmailID = (TextBox)row.FindControl("txtEditEmailID");
                TextBox txtEditUserTitle = (TextBox)row.FindControl("txtEditUserTitle");
                Label lbAutoID = (Label)row.FindControl("lbAutoID");

                #region Validation
                string email = txtEditEmailID.Text.Trim();
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                {

                }
                else
                {
                    throw new Exception("Please enter valid Email ID !!");
                }
                if (txtEditFName.Text.Trim() == "")
                {
                    throw new Exception("Please enter First Name !!");
                }
                else if (txtEditLName.Text.Trim() == "")
                {
                    throw new Exception("Please enter Last Name !!");
                }
                else if (txtEditEmailID.Text.Trim() == "")
                {
                    throw new Exception("Please enter Email ID !!");
                }
                else if (txtEditUserTitle.Text.Trim() == "")
                {
                    throw new Exception("Please enter Job Title !!");
                }
                #endregion

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.UserDetailsPassingUserIDANDEmailID(lbAutoID.Text, txtEditEmailID.Text.Trim());
                if (ds01.Tables[0].Rows.Count>0)
                {
                    gvDispRec.EditIndex = -1;
                    PopulateGridView();
                    throw new Exception("This Email ID is already registered !!");
                }

                ds01.Reset();
                ds01 = ObjClassStoreProc.UserInfoPassingUserID(lbAutoID.Text);
                if (ds01.Tables[0].Rows[0][11].ToString() == "A")
                {
                    if (ddlEditStat.SelectedValue == "I")
                    {
                        gvDispRec.EditIndex = -1;
                        PopulateGridView();
                        throw new Exception("You can't de-activate Admin !!");
                    }
                }
                if (ds01.Tables[0].Rows[0][7].ToString() == "I")
                {
                    if (ddlEditStat.SelectedValue == "A")
                    {
                        #region Check total no of users which are already in the system
                        
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                        con.Open();
                        cmd = new SqlCommand("select count(*) as Tot from user_mast where CompCode='" + Session["CompCode"].ToString() + "' and user_stat='A'", con);
                        int TotUser = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                        if (TotUser < Convert.ToInt32(ds01.Tables[0].Rows[0][16].ToString()))
                        {

                        }
                        else
                        {
                            throw new Exception("You need to change your Billing option. Please contact with Administrator.");
                        }
                        #endregion
                    }
                }

                #region Alfresco Part Start
                WebServiceFactory wsFA = new WebServiceFactory();
                wsFA.UserName = Session["AdmUserID"].ToString();
                wsFA.Ticket = Session["AdmTicket"].ToString();
                this.administrationService = wsFA.getAdministrationService();

                UserDetails[] updateUserList = new UserDetails[1];
                UserDetails updateUser = new UserDetails();
                updateUser.userName = lbAutoID.Text.Trim();
                Alfresco.AdministrationWebService.NamedValue[] updateProperties = new Alfresco.AdministrationWebService.NamedValue[3];

                updateProperties[0] = new Alfresco.AdministrationWebService.NamedValue();
                updateProperties[0].name = Constants.PROP_USER_FIRSTNAME;
                updateProperties[0].value = txtEditFName.Text.Trim();

                updateProperties[1] = new Alfresco.AdministrationWebService.NamedValue();
                updateProperties[1].name = Constants.PROP_USER_LASTNAME;
                updateProperties[1].value = txtEditLName.Text.Trim();

                updateProperties[2] = new Alfresco.AdministrationWebService.NamedValue();
                updateProperties[2].name = Constants.PROP_USER_EMAIL;
                updateProperties[2].value = txtEditEmailID.Text.Trim();

                updateUser.properties = updateProperties;
                updateUserList[0] = updateUser;

                administrationService.updateUsers(updateUserList);
                #endregion

                string result = ObjClassStoreProc.UpdateUserMast(lbAutoID.Text.Trim(), Session["CompCode"].ToString(), txtEditFName.Text.Trim(), txtEditLName.Text.Trim(), txtEditEmailID.Text.Trim(), txtEditUserTitle.Text.Trim(), ddlEditDept.SelectedValue, ddlEditStat.SelectedValue);
                gvDispRec.EditIndex = -1;
                PopulateGridView();
                throw new Exception("Data Updated Successfully !!");
            }
            catch (Exception ex)
            {
                hfMsg.Value=ex.Message;
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
                /// For Department Dropdown
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

                // For Status Dropdown
                Control ctrl11 = e.Row.FindControl("ddlEditStat");
                if (ctrl11 != null)
                {
                    DropDownList ddlEditStat = ctrl11 as DropDownList;
                    Control ctrl16 = e.Row.FindControl("hdStat");
                    if (ctrl16 != null)
                    {
                        HiddenField hdStat = ctrl16 as HiddenField;
                        if (hdStat.Value == "A")
                        {
                            ddlEditStat.ClearSelection();
                            ddlEditStat.Items[0].Selected = true;
                        }
                        else if (hdStat.Value == "I")
                        {
                            ddlEditStat.ClearSelection();
                            ddlEditStat.Items[1].Selected = true;
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