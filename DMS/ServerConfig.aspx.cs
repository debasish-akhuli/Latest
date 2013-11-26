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
using Alfresco.AdministrationWebService;

namespace DMS
{
    public partial class ServerConfig : System.Web.UI.Page
    {
        private AdministrationService administrationService;
        private Alfresco.RepositoryWebService.Store spacesStore;
        Alfresco.RepositoryWebService.Reference reference;
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
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopulateDropdown();
                            PopulateGridView();
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                            DataSet ds01 = new DataSet();
                            ds01 = ObjClassStoreProc.UserInfoPassingUserID("admin");
                            Session["nAdmEmail"] = ds01.Tables[0].Rows[0][3].ToString();
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

        protected void CtrlChanged(Object sender, EventArgs e)
        {
            if (txtMaxNoOfUsers.Text.Trim() == "")
            {
                txtMaxNoOfUsers.Text = "0";
            }
            SetRates(ddSpace.SelectedValue, txtMaxNoOfUsers.Text.Trim());
        }

        protected void PopulateDropdown()
        {
            try
            {
                //....Bill Type
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.SelectBillTypeAll();
                ddSpace.DataSource = ds01;
                ddSpace.DataTextField = "ItemDesc";
                ddSpace.DataValueField = "ItemCode";
                ddSpace.DataBind();
                SetRates(ddSpace.SelectedValue, txtMaxNoOfUsers.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtMaxNoOfUsers.Text.Trim() == "")
                {
                    txtMaxNoOfUsers.Text = "0";
                }
                SetRates(ddSpace.SelectedValue, txtMaxNoOfUsers.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void SetRates(string ItemCode, string UserNo)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                SqlDataAdapter adapter01;
                SqlDataAdapter adapter02;
                DataSet ds01 = new DataSet();
                cmd = new SqlCommand("select ItemDesc,CAST(ROUND(ItemRate, 2) AS DECIMAL(9,2)),SpaceInKB from BillTypeMast where ItemCode='" + ItemCode + "'", con);
                adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    lblSpace.Text = ds01.Tables[0].Rows[0][2].ToString();
                    lblSpaceRate.Text = ds01.Tables[0].Rows[0][1].ToString();
                    lblSpaceRate1.Text = "$" + ds01.Tables[0].Rows[0][1].ToString() + " per month";
                }
                cmd = new SqlCommand("select CAST(ROUND(ItemRate, 2) AS DECIMAL(9,2)) from BillTypeMast where ItemCode='000'", con);
                adapter02 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter02.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    lblPerUserRate.Text = ds01.Tables[0].Rows[0][0].ToString();
                    lblPerUserRate1.Text = "$" + ds01.Tables[0].Rows[0][0].ToString() + " per month";
                }
                lblTotalRate.Text = (Convert.ToDouble(lblSpaceRate.Text) + Convert.ToDouble(lblPerUserRate.Text) * Convert.ToDouble(UserNo)).ToString();
                lblTotalRate1.Text = "$" + (Convert.ToDouble(lblSpaceRate.Text) + Convert.ToDouble(lblPerUserRate.Text) * Convert.ToDouble(UserNo)).ToString() + " per month";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateGridView()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.SelectServerConfigAll();
                gvDispRec.DataSource = ds01;
                gvDispRec.DataBind();
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

        protected void cmdAdd_Click(object sender, EventArgs e)
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
                if (txtCompName.Text.Trim() == "")
                {
                    throw new Exception("Please enter your Company Name!!");
                }
                else if (txtContactPersonName.Text.Trim() == "")
                {
                    throw new Exception("Please enter Contact Person's Name!!");
                }
                else if (txtContactNo.Text.Trim() == "")
                {
                    throw new Exception("Please enter Contact Number!!");
                }
                else if (txtEmailID.Text.Trim() == "")
                {
                    throw new Exception("Please enter a valid Email ID!!");
                }
                else if (Convert.ToInt32(txtMaxNoOfUsers.Text.Trim()) > 99)
                {
                    throw new Exception("You can enter maximum 99 Users!!");
                }

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                string SenderMail = "";
                string SenderName = "";
                string SmtpHost = "";
                Int32 SmtpPort = 0;
                string CredenUsername = "";
                string CredenPwd = "";
                string MailSub = "";
                string MailMsg = "";
                string MailTo = "";
                string MailFrom = "";
                string QuickPDFLicenseKey = "";
                string ServerIP = "";
                string DomainName = "";
                string HotlineNumber = "";
                string HotlineEmail = "";
                string CompCode = "";
                string WorkspaceName = "";
                string WorkspaceTitle = "";
                string UserID = "";

                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = Session["AdmUserID"].ToString();
                wsF.Ticket = Session["AdmTicket"].ToString();
                this.repoService = wsF.getRepositoryService();

                #region Fetch the Mail Settings from Database Start
                mailing ObjMailSetup = new mailing();
                ds01.Reset();
                ds01 = ObjMailSetup.MailSettings();
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    SenderMail = ds01.Tables[0].Rows[0][0].ToString();
                    SenderName = ds01.Tables[0].Rows[0][1].ToString();
                    SmtpHost = ds01.Tables[0].Rows[0][2].ToString();
                    SmtpPort = Convert.ToInt32(ds01.Tables[0].Rows[0][3].ToString());
                    CredenUsername = ds01.Tables[0].Rows[0][4].ToString();
                    CredenPwd = ds01.Tables[0].Rows[0][5].ToString();
                }
                #endregion

                #region Default Values
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectServerConfig("00000000");
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    QuickPDFLicenseKey = ds01.Tables[0].Rows[0][0].ToString();
                    ServerIP = ds01.Tables[0].Rows[0][1].ToString();
                    DomainName = ds01.Tables[0].Rows[0][2].ToString();
                    HotlineNumber = ds01.Tables[0].Rows[0][4].ToString();
                    HotlineEmail = ds01.Tables[0].Rows[0][5].ToString();
                }
                #endregion

                #region Generate CompCode
                CompCode = ObjClassStoreProc.SelectMaxCompCode(DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0'));
                Session["nCompCode"] = CompCode;
                #endregion

                #region Workspace
                WorkspaceName = "WS" + CompCode;
                WorkspaceTitle = WorkspaceName;
                #endregion

                #region Insert into ServerConfig
                double TotalRate = Convert.ToDouble(lblSpaceRate.Text.Trim()) + (Convert.ToDouble(lblPerUserRate.Text.Trim()) * Convert.ToDouble(txtMaxNoOfUsers.Text.Trim()));
                result = ObjClassStoreProc.InsertServerConfig(QuickPDFLicenseKey, ServerIP, DomainName, txtCompName.Text.Trim(), HotlineNumber, HotlineEmail, CompCode, txtContactPersonName.Text.Trim(), txtEmailID.Text.Trim(), txtContactNo.Text.Trim(), WorkspaceName, WorkspaceTitle, ddStatus.SelectedValue, Convert.ToDouble(lblSpace.Text), 0, Convert.ToDouble(lblSpace.Text), Convert.ToInt32(txtMaxNoOfUsers.Text.Trim()), Convert.ToDouble(lblSpaceRate.Text.Trim()), Convert.ToDouble(lblPerUserRate.Text.Trim()), TotalRate);
                #endregion

                if (Convert.ToInt64(result) == 0)
                {
                    throw new Exception("Error in Registration.");
                }
                else if (Convert.ToInt64(result) == -1)
                {
                    throw new Exception("Error in Registration.");
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    #region Department
                    result = ObjClassStoreProc.InsertDeptMast("ADM", "Administration", CompCode);
                    result = ObjClassStoreProc.InsertDeptMast("MISC", "Miscellaneous", CompCode);
                    #endregion

                    #region DocType
                    result = ObjClassStoreProc.InsertDocTypeMast("GENERAL", "General", "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, CompCode, "Non Editable");
                    #endregion

                    #region Role
                    result = ObjClassStoreProc.InsertRoleMast("INIT", "Initiator", CompCode);
                    #endregion

                    #region Role-User Mapping
                    result = ObjClassStoreProc.InsertUserRoleMast("00000000_00000000001", "INIT", CompCode);
                    #endregion

                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;

                    #region User
                    Random _r = new Random();
                    string AutoGenPwd = _r.Next(0, 1000000).ToString();
                    UserID = ObjClassStoreProc.SelectMaxUserID(CompCode).ToLower();
                    Session["nUserID"] = UserID;
                    result = ObjClassStoreProc.InsertUserMast(UserID, "Systems", "Administrator", txtEmailID.Text.Trim(), AutoGenPwd, "Administrator", "ADM", "A", "", "Y", CompCode, "A", DateTime.Now);
                    if (Convert.ToInt64(result) > 0)
                    {
                        /// Alfresco Part Start
                        WebServiceFactory wsFA = new WebServiceFactory();
                        wsFA.UserName = Session["AdmUserID"].ToString();
                        wsFA.Ticket = Session["AdmTicket"].ToString();
                        this.administrationService = wsFA.getAdministrationService();

                        Alfresco.AdministrationWebService.NewUserDetails[] newUsers = new NewUserDetails[1];
                        Alfresco.AdministrationWebService.NewUserDetails userDetails = new NewUserDetails();
                        userDetails.userName = UserID;
                        userDetails.password = AutoGenPwd;
                        Alfresco.AdministrationWebService.NamedValue[] properties = new Alfresco.AdministrationWebService.NamedValue[4];

                        /// First Name
                        Alfresco.AdministrationWebService.NamedValue namedValue = new Alfresco.AdministrationWebService.NamedValue();
                        namedValue.name = Constants.PROP_USER_FIRSTNAME;
                        namedValue.value = "Systems";
                        properties[0] = namedValue;

                        /// Last Name
                        namedValue = new Alfresco.AdministrationWebService.NamedValue();
                        namedValue.name = Constants.PROP_USER_LASTNAME;
                        namedValue.value = "Administrator";
                        properties[1] = namedValue;

                        /// EMail
                        namedValue = new Alfresco.AdministrationWebService.NamedValue();
                        namedValue.name = Constants.PROP_USER_EMAIL;
                        namedValue.value = txtEmailID.Text.Trim();
                        properties[2] = namedValue;

                        /// Title
                        namedValue = new Alfresco.AdministrationWebService.NamedValue();
                        namedValue.name = Constants.PROP_TITLE;
                        namedValue.value = "Administrator";
                        properties[3] = namedValue;
                        userDetails.properties = properties;
                        newUsers[0] = userDetails;
                        UserDetails[] responseUserDetails = administrationService.createUsers(newUsers);
                        /// Alfresco Part End

                        UserRights UserObj = new UserRights();
                        UserObj.SetUserPermissions(UserID, CompCode);
                    }
                    else if (Convert.ToInt64(result) == 0)
                    {
                        con.Open();
                        cmd = new SqlCommand("delete from ServerConfig where CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("delete from dept_mast where dept_id='ADM' and CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("delete from dept_mast where dept_id='MISC' and CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("delete from role_mast where role_id='INIT' and CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("delete from user_role where CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        throw new Exception("This Email ID is already registered.");
                    }
                    else if (Convert.ToInt64(result) < 0)
                    {
                        con.Open();
                        cmd = new SqlCommand("delete from ServerConfig where CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("delete from dept_mast where dept_id='ADM' and CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("delete from dept_mast where dept_id='MISC' and CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("delete from role_mast where role_id='INIT' and CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("delete from user_role where CompCode='" + CompCode + "'", con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        throw new Exception("User is already created for this Company.");
                    }
                    #endregion

                    #region Create the Workspace in Alfresco
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";

                    Alfresco.RepositoryWebService.Reference reference = new Alfresco.RepositoryWebService.Reference();
                    reference.store = this.spacesStore;
                    reference.path = "/app:company_home";
                    AlfCreateSpace ObjCreateSpace = new AlfCreateSpace();
                    result = ObjCreateSpace.CreateSpace(WorkspaceName, WorkspaceTitle, "/app:company_home", null, WorkspaceName, wsF.UserName, wsF.Ticket);
                    Session["nWorkspaceUUID"] = result;
                    #endregion

                    #region "TRASH" Cabinet Creation
                    reference.path = "/app:company_home/cm:" + WorkspaceName;
                    Session["nNodeUUID"] = ObjCreateSpace.CreateSpace("TRASH", "TRASH", "/app:company_home/cm:" + WorkspaceName, null, "TRASH", wsF.UserName, wsF.Ticket);
                    Session["nTRASHCabinetUUID"] = Session["nNodeUUID"];
                    result = ObjClassStoreProc.InsertCabinetMast("TRASH", "TRASH", Session["nNodeUUID"].ToString(), "X", CompCode);
                    UserRights RightsObj = new UserRights();
                    RightsObj.SetPermissions(Session["nNodeUUID"].ToString(), "Cabinet", UserID, "X", CompCode);
                    #endregion

                    #region "TRASH" Drawer Creation
                    ds01.Reset();
                    cmd = new SqlCommand("select cab_uuid from cabinet_mast where cab_name='TRASH' and CompCode='" + CompCode + "'", con);
                    SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                    adapter02.Fill(ds01);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        reference.uuid = ds01.Tables[0].Rows[0][0].ToString();
                    }
                    Session["nNodeUUID"] = ObjCreateSpace.CreateSpace("TRASH", "TRASH", null, reference.uuid, "TRASH", wsF.UserName, wsF.Ticket);
                    Session["nTRASHDrawerUUID"] = Session["nNodeUUID"];
                    result = ObjClassStoreProc.InsertDrawerMast("TRASH", "TRASH", reference.uuid, Session["nNodeUUID"].ToString(), CompCode);
                    ds01.Reset();
                    ds01 = RightsObj.FetchPermission(reference.uuid, CompCode);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(Session["nNodeUUID"].ToString(), "Drawer", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), CompCode);
                        }
                    }
                    #endregion

                    #region "TRASH" Folder Creation
                    ds01.Reset();
                    cmd = new SqlCommand("select drw_uuid from drawer_mast where drw_name='TRASH' and CompCode='" + CompCode + "'", con);
                    adapter02 = new SqlDataAdapter(cmd);
                    adapter02.Fill(ds01);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        reference.uuid = ds01.Tables[0].Rows[0][0].ToString();
                    }
                    Session["nNodeUUID"] = ObjCreateSpace.CreateSpace("TRASH", "TRASH", null, reference.uuid, "TRASH", wsF.UserName, wsF.Ticket);
                    Session["nTRASHFolderUUID"] = Session["nNodeUUID"];
                    result = ObjClassStoreProc.InsertFolderMast("TRASH", "TRASH", reference.uuid, Session["nNodeUUID"].ToString(), CompCode);
                    ds01.Reset();
                    ds01 = RightsObj.FetchPermission(reference.uuid, CompCode);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(Session["nNodeUUID"].ToString(), "Folder", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), CompCode);
                        }
                    }
                    #endregion

                    #region Send the Email
                    MailFrom = Session["nAdmEmail"].ToString();
                    MailTo = txtEmailID.Text.Trim();
                    MailSub = "Welcome to myDOCbase SaaS";
                    MailMsg = "Hello " + txtContactPersonName.Text.Trim() + ",<br/><br/>Welcome to myDOCbase.<br/><br/>Your Company Code is: " + CompCode + "<br/><br/>Your Login details are as follows:<br/>- Login ID: " + txtEmailID.Text.Trim() + "<br/>- Password: " + AutoGenPwd + "<br/><br/>Please click on the URL below to begin your wonderful journey:<br/>http://174.141.234.20/Default.aspx<br/><br/>In case of any problem, please use any of the following methods to contact us, and we shall by happy to assist you:<br/>- Email: support@mydocbase.com [Please use your Company Code in the subject-line)<br/>- Skype: probaldg<br/>- Call: 888-555-1212<br/><br/>Thanks,<br/>System Administrator.";
                    mailing Obj_Mail = new mailing();
                    if (Obj_Mail.SendEmail("", "admin", MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                    {
                        MailSub = "New Registration[" + CompCode + "]";
                        MailMsg = "Hello,<br/>One Client has registered.<br/><br/>The details of the Client are as follows:<br/>Company Name: " + txtCompName.Text.Trim() + "<br/>Company Code: " + CompCode + "<br/>Total Space: " + ddSpace.SelectedItem.Text + " [" + lblSpaceRate.Text + " per Month]<br/>Max # of User: " + txtMaxNoOfUsers.Text.Trim() + " [" + lblPerUserRate.Text.Trim() + " per User per Month]<br/>Total Rate: " + lblTotalRate.Text.Trim() + " per Month<br/>Email ID: " + txtEmailID.Text.Trim() + "<br/>Password: " + AutoGenPwd + "<br/>Contact Person: " + txtContactPersonName.Text.Trim() + "<br/>Contact #: " + txtContactNo.Text.Trim() + "<br/><br/>Thanks,<br/>System Administrator.";
                        if (Obj_Mail.SendEmail("", "admin", MailFrom, MailFrom, "", MailFrom, MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                        {

                        }
                        PopulateDropdown();
                        PopulateGridView();
                        MessageBox("Client has been registered!!");
                    }
                    else
                    {
                        MessageBox("Invalid Email ID. Please Try Again.");
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Mailbox unavailable. The server response was: unrouteable address")
                {
                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    con.Open();
                    cmd = new SqlCommand("delete from ServerConfig where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from dept_mast where dept_id='ADM' and CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from dept_mast where dept_id='MISC' and CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from role_mast where role_id='INIT' and CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from user_mast where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from user_role where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from folder_mast where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from drawer_mast where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from cabinet_mast where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    #region "TRASH" Folder Deletion
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    //create a predicate with the first CMLCreate result
                    Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = Session["nTRASHFolderUUID"].ToString(); // Node's UUID which needs to be deleted

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
                    #endregion

                    #region "TRASH" Drawer Deletion
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = Session["nTRASHDrawerUUID"].ToString(); // Node's UUID which needs to be deleted
                    sourcePredicate.Items = obj_new;
                    delete.where = sourcePredicate;
                    cmlRemove.delete = new CMLDelete[] { delete };
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getRepositoryService().update(cmlRemove);
                    #endregion

                    #region "TRASH" Cabinet Deletion
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = Session["nTRASHCabinetUUID"].ToString(); // Node's UUID which needs to be deleted
                    sourcePredicate.Items = obj_new;
                    delete.where = sourcePredicate;
                    cmlRemove.delete = new CMLDelete[] { delete };
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getRepositoryService().update(cmlRemove);
                    #endregion

                    #region Workspace Deletion
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    //create a predicate with the first CMLCreate result
                    //Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = Session["nWorkspaceUUID"].ToString(); // Node's UUID which needs to be deleted

                    //Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                    //Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                    sourcePredicate.Items = obj_new;

                    //copy content
                    //CMLDelete delete = new CMLDelete();
                    delete.where = sourcePredicate;

                    //CML cmlRemove = new CML();
                    cmlRemove.delete = new CMLDelete[] { delete };

                    //perform a CML update to delete the node

                    //WebServiceFactory wsF = new WebServiceFactory();
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getRepositoryService().update(cmlRemove);
                    #endregion

                    #region User Deletion
                    String[] usersToDelete = { Session["nUserID"].ToString() };
                    administrationService.deleteUsers(usersToDelete);
                    #endregion

                    MessageBox("Please put your valid Email ID !!");
                }
                else if (ex.Message == "The specified string is not in the form required for an e-mail address.")
                {
                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    con.Open();
                    cmd = new SqlCommand("delete from ServerConfig where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from dept_mast where dept_id='ADM' and CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from dept_mast where dept_id='MISC' and CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from role_mast where role_id='INIT' and CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from user_mast where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from user_role where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from folder_mast where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from drawer_mast where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("delete from cabinet_mast where CompCode='" + Session["nCompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    //cmd = new SqlCommand("delete from DailyUserDocStat where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    //cmd.ExecuteNonQuery();
                    con.Close();

                    #region "TRASH" Folder Deletion
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    //create a predicate with the first CMLCreate result
                    Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = Session["nTRASHFolderUUID"].ToString(); // Node's UUID which needs to be deleted

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
                    #endregion

                    #region "TRASH" Drawer Deletion
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = Session["nTRASHDrawerUUID"].ToString(); // Node's UUID which needs to be deleted
                    sourcePredicate.Items = obj_new;
                    delete.where = sourcePredicate;
                    cmlRemove.delete = new CMLDelete[] { delete };
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getRepositoryService().update(cmlRemove);
                    #endregion

                    #region "TRASH" Cabinet Deletion
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = Session["nTRASHCabinetUUID"].ToString(); // Node's UUID which needs to be deleted
                    sourcePredicate.Items = obj_new;
                    delete.where = sourcePredicate;
                    cmlRemove.delete = new CMLDelete[] { delete };
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getRepositoryService().update(cmlRemove);
                    #endregion

                    #region Workspace Deletion
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    //create a predicate with the first CMLCreate result
                    //Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = Session["nWorkspaceUUID"].ToString(); // Node's UUID which needs to be deleted

                    //Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                    //Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                    sourcePredicate.Items = obj_new;

                    //copy content
                    //CMLDelete delete = new CMLDelete();
                    delete.where = sourcePredicate;

                    //CML cmlRemove = new CML();
                    cmlRemove.delete = new CMLDelete[] { delete };

                    //perform a CML update to delete the node

                    //WebServiceFactory wsF = new WebServiceFactory();
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getRepositoryService().update(cmlRemove);
                    #endregion

                    #region User Deletion
                    String[] usersToDelete = { Session["nUserID"].ToString() };
                    administrationService.deleteUsers(usersToDelete);
                    #endregion

                    MessageBox("Please put your valid Email ID !!");
                }
                else
                {
                    MessageBox(ex.Message);
                }
            }
        }
        
    }
}