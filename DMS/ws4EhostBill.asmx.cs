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
using System.Web.Services;

namespace DMS
{
    /// <summary>
    /// Summary description for ws4EhostBill
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ws4EhostBill : System.Web.Services.WebService
    {
        private AdministrationService administrationService;
        private Alfresco.RepositoryWebService.Store spacesStore;
        Alfresco.RepositoryWebService.Reference reference;
        private RepositoryService repoService;
        public RepositoryService RepoService
        {
            set { repoService = value; }
        }
        
        [WebMethod(EnableSession = true)]
        public string AddCompany(string CompanyName, string AdminEmailID, string ServerIPAddress, string DomainName, double SpaceInKB, int MaxNoOfUsers)
        {
            string RegStat = "0";
            CompanyName = CompanyName.Replace(',', ' ');
            string CompanyCode = "";
            string Password = "";
            string result = RegStat;
            AuthenticationUtils authUtil = new AuthenticationUtils();
            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
            DataSet ds01 = new DataSet();
            ds01 = ObjClassStoreProc.UserInfoPassingUserID("admin");
            authUtil.startSession("admin", ds01.Tables[0].Rows[0][4].ToString());
            Session["AdmUserID"] = authUtil.UserName;
            Session["AdmTicket"] = authUtil.Ticket;
            Session["AdmEmail"] = ds01.Tables[0].Rows[0][3].ToString();
            if (authUtil.IsSessionValid)
            {
                if (AddCompanyInfo(CompanyName, AdminEmailID, ServerIPAddress, DomainName, SpaceInKB, MaxNoOfUsers) == "True")
                {
                    RegStat = "1";
                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    con.Open();
                    SqlDataAdapter adapter01;
                    DataSet ds02 = new DataSet();
                    cmd = new SqlCommand("select a.CompCode,b.user_pwd from ServerConfig a,user_mast b where a.EmailID=b.email and a.EmailID='" + AdminEmailID + "'", con);
                    adapter01 = new SqlDataAdapter(cmd);
                    ds02.Reset();
                    adapter01.Fill(ds02);
                    CompanyCode = ds02.Tables[0].Rows[0][0].ToString();
                    Password = ds02.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    RegStat = "0";
                }
            }
            else
            {
                RegStat = "0";
            }
            result = RegStat + "," + Password + "," + CompanyCode;
            return result;
        }

        protected string AddCompanyInfo(string CompanyName, string AdminEmailID, string ServerIPAddress, string DomainName, double SpaceInKB, int MaxNoOfUsers)
        {
            string Flag = "False";
            try
            {
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
                string mDomainName = "";
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
                    mDomainName = ds01.Tables[0].Rows[0][2].ToString();
                    HotlineNumber = ds01.Tables[0].Rows[0][4].ToString();
                    HotlineEmail = ds01.Tables[0].Rows[0][5].ToString();
                }
                #endregion

                #region Generate CompCode
                CompCode = ObjClassStoreProc.SelectMaxCompCode(DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0'));
                Session["CompCode"] = CompCode;
                #endregion

                #region Workspace
                WorkspaceName = "WS" + CompCode;
                WorkspaceTitle = WorkspaceName;
                #endregion

                #region Insert into ServerConfig
                //double TotalRate = Convert.ToDouble(lblSpaceRate.Text.Trim()) + (Convert.ToDouble(lblPerUserRate.Text.Trim()) * Convert.ToDouble(txtMaxNoOfUsers.Text.Trim()));
                result = ObjClassStoreProc.InsertServerConfig(QuickPDFLicenseKey, ServerIPAddress, DomainName, CompanyName, HotlineNumber, HotlineEmail, CompCode, "", AdminEmailID, "", WorkspaceName, WorkspaceTitle, "Active", Convert.ToDouble(SpaceInKB), 0, Convert.ToDouble(SpaceInKB), Convert.ToInt32(MaxNoOfUsers), 0, 0, 0);
                #endregion

                if (Convert.ToInt64(result) == 0)
                {
                    return "False";
                }
                else if (Convert.ToInt64(result) == -1)
                {
                    return "False";
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    #region Department
                    result = ObjClassStoreProc.InsertDeptMast("ADM", "Administration", CompCode);
                    result = ObjClassStoreProc.InsertDeptMast("MISC", "Miscellaneous", CompCode);
                    #endregion

                    #region DocType
                    result = ObjClassStoreProc.InsertDocTypeMast("GENERAL", "General", "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, CompCode, "Other");
                    #endregion

                    #region Role
                    result = ObjClassStoreProc.InsertRoleMast("INIT", "Initiator", CompCode);
                    #endregion

                    #region Role-User Mapping
                    result = ObjClassStoreProc.InsertUserRoleMast("00000000_00000000001", "INIT", CompCode);
                    #endregion

                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;

                    Random _r = new Random();
                    string AutoGenPwd = _r.Next(0, 1000000).ToString();
                    UserID = ObjClassStoreProc.SelectMaxUserID(CompCode).ToLower();

                    #region User
                    //Random _r = new Random();
                    //string AutoGenPwd = _r.Next(0, 1000000).ToString();
                    //UserID = ObjClassStoreProc.SelectMaxUserID(CompCode).ToLower();
                    Session["UserID"] = UserID;
                    result = ObjClassStoreProc.InsertUserMast(UserID, "Systems", "Administrator", AdminEmailID, AutoGenPwd, "Administrator", "ADM", "A", "", "Y", CompCode, "A", DateTime.Now);
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
                        namedValue.value = AdminEmailID;
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
                        return "False";
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
                        return "False";
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
                    Session["WorkspaceUUID"] = result;

                    con.Open();
                    cmd = new SqlCommand("update ServerConfig set WorkspaceUUID='" + Session["WorkspaceUUID"].ToString() + "' where CompCode='" + CompCode + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    #endregion

                    #region "TRASH" Cabinet Creation
                    reference.path = "/app:company_home/cm:" + WorkspaceName;
                    Session["NodeUUID"] = ObjCreateSpace.CreateSpace("TRASH", "TRASH", "/app:company_home/cm:" + WorkspaceName, null, "TRASH", wsF.UserName, wsF.Ticket);
                    Session["TRASHCabinetUUID"] = Session["NodeUUID"];
                    result = ObjClassStoreProc.InsertCabinetMast("TRASH", "TRASH", Session["NodeUUID"].ToString(), "X", CompCode);
                    UserRights RightsObj = new UserRights();
                    RightsObj.SetPermissions(Session["NodeUUID"].ToString(), "Cabinet", UserID, "X", CompCode);
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
                    Session["NodeUUID"] = ObjCreateSpace.CreateSpace("TRASH", "TRASH", null, reference.uuid, "TRASH", wsF.UserName, wsF.Ticket);
                    Session["TRASHDrawerUUID"] = Session["NodeUUID"];
                    result = ObjClassStoreProc.InsertDrawerMast("TRASH", "TRASH", reference.uuid, Session["NodeUUID"].ToString(), CompCode);
                    ds01.Reset();
                    ds01 = RightsObj.FetchPermission(reference.uuid, CompCode);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(Session["NodeUUID"].ToString(), "Drawer", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), CompCode);
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
                    Session["NodeUUID"] = ObjCreateSpace.CreateSpace("TRASH", "TRASH", null, reference.uuid, "TRASH", wsF.UserName, wsF.Ticket);
                    Session["TRASHFolderUUID"] = Session["NodeUUID"];
                    result = ObjClassStoreProc.InsertFolderMast("TRASH", "TRASH", reference.uuid, Session["NodeUUID"].ToString(), CompCode);
                    ds01.Reset();
                    ds01 = RightsObj.FetchPermission(reference.uuid, CompCode);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(Session["NodeUUID"].ToString(), "Folder", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), CompCode);
                        }
                    }
                    #endregion

                    Flag = "True";
                }
                return Flag;
            }
            catch (Exception ex)
            {
                return "False";
            }
        }

        [WebMethod(EnableSession = true)]
        public string DeleteCompany(string CompanyCode)
        {
            string DelStat = "0";
            string result = DelStat;
            AuthenticationUtils authUtil = new AuthenticationUtils();
            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
            DataSet ds01 = new DataSet();
            ds01 = ObjClassStoreProc.UserInfoPassingUserID("admin");
            authUtil.startSession("admin", ds01.Tables[0].Rows[0][4].ToString());
            if (authUtil.IsSessionValid)
            {
                if (DelCompInfo(CompanyCode, authUtil.UserName, authUtil.Ticket) == "True")
                {
                    DelStat = "1";
                }
                else
                {
                    DelStat = "0";
                }
            }
            else
            {
                DelStat = "0";
            }
            result = DelStat;
            return result;
        }

        protected string DelCompInfo(string CompanyCode, string AdmUserID, string AdmTicket)
        {
            string Flag = "False";
            try
            {
                #region Alfresco Part
                #region Workspace Deletion
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                SqlDataAdapter adapter01;
                DataSet ds01 = new DataSet();
                cmd = new SqlCommand("select WorkspaceUUID from ServerConfig where CompCode='" + CompanyCode + "'", con);
                adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                string WorkspaceUUID = ds01.Tables[0].Rows[0][0].ToString();
                
                this.spacesStore = new Alfresco.RepositoryWebService.Store();
                this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                this.spacesStore.address = "SpacesStore";

                Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                referenceForNode.store = this.spacesStore;
                referenceForNode.uuid = WorkspaceUUID; // Node's UUID which needs to be deleted

                Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                sourcePredicate.Items = obj_new;

                //delete content
                CMLDelete delete = new CMLDelete();
                delete.where = sourcePredicate;

                CML cmlRemove = new CML();
                cmlRemove.delete = new CMLDelete[] { delete };

                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = AdmUserID;
                wsF.Ticket = AdmTicket;
                wsF.getRepositoryService().update(cmlRemove);
                #endregion

                #region User Deletion
                SqlDataAdapter adapter001;
                cmd = new SqlCommand("select user_id from user_mast where CompCode='" + CompanyCode + "'", con);
                adapter001 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter001.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    WebServiceFactory wsFA = new WebServiceFactory();
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        wsFA.UserName = AdmUserID;
                        wsFA.Ticket = AdmTicket;
                        this.administrationService = wsFA.getAdministrationService();
                        String[] usersToDelete = { ds01.Tables[0].Rows[i][0].ToString() };
                        administrationService.deleteUsers(usersToDelete);
                    }
                }
                #endregion
                #endregion
                #region SQL Server Part
                
                cmd = new SqlCommand("delete from cabinet_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from dept_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from doc_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from doc_type_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from DocLog where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from DocMetaValue where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from drawer_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from folder_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from PermissionLog where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from RejectedList where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from role_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from ServerConfig where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from TempDocSaving where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from user_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from user_role where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from UserRights where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from wf_cond where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from wf_dtl where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from wf_log_dtl where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from wf_log_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from wf_log_task where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from wf_mast where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from wf_task where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from WFDoc where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from WFDocVersion where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from WFLog where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("delete from WFSignDate where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                #endregion
                Flag = "True";
                return Flag;
            }
            catch (Exception ex)
            {
                return "False";
            }
        }

        [WebMethod(EnableSession = true)]
        public string UpdateCompany(string CompanyCode, string CompanyName, string ServerIPAddress, string DomainName, string Status, double SpaceInKB, int MaxNoOfUsers)
        {
            string UpdateStat = "0";
            string result = UpdateStat;
            if (UpdateCompInfo(CompanyCode, CompanyName, ServerIPAddress, DomainName, Status, SpaceInKB, MaxNoOfUsers) == "True")
            {
                UpdateStat = "1";
            }
            else
            {
                UpdateStat = "0";
            }

            result = UpdateStat;
            return result;
        }

        protected string UpdateCompInfo(string CompanyCode, string CompanyName, string ServerIPAddress, string DomainName, string Status, double SpaceInKB, int MaxNoOfUsers)
        {
            string Flag = "False";
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                con.Open();
                #region Check total no of users & Occupied Space which are already in the system
                cmd = new SqlCommand("select count(*) as Tot from user_mast where CompCode='" + CompanyCode + "' and user_stat='A'", con);
                int TotUser = Convert.ToInt32(cmd.ExecuteScalar());
                if (TotUser <= Convert.ToInt32(MaxNoOfUsers))
                {

                }
                else
                {
                    throw new Exception("You need to inactivate some users first!!");
                }

                cmd = new SqlCommand("select UsedSpace as UsedSpace from ServerConfig where CompCode='" + CompanyCode + "'", con);
                double UsedSpace = Convert.ToDouble(cmd.ExecuteScalar());
                if (UsedSpace <= SpaceInKB)
                {

                }
                else
                {
                    throw new Exception("This client has already occupied more space than your selected one!!");
                }
                #endregion

                cmd = new SqlCommand("update ServerConfig set CompName='" + CompanyName + "',ServerIP='" + ServerIPAddress + "',DomainName='" + DomainName + "',Status='" + Status + "',TotalSpace='" + SpaceInKB.ToString() + "',MaxNoOfUsers='" + MaxNoOfUsers.ToString() + "' where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace-UsedSpace where CompCode='" + CompanyCode + "'", con);
                cmd.ExecuteNonQuery();

                Flag = "True";
                return Flag;
            }
            catch (Exception ex)
            {
                return "False";
            }
        }

        [WebMethod(EnableSession = true)]
        public string ListCompany(string CompanyCode)
        {
            string result = "";
            string CompanyName = "";
            string AdminEmailID = "";
            string ServerIPAddress = "";
            string DomainName = "";
            string Status = "";
            string TotalSpaceInKB = "";
            string UsedSpaceInKB = "";
            string AvailableSpaceInKB = "";
            string MaxNoOfUsers = "";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            SqlDataAdapter adapter01;
            DataSet ds02 = new DataSet();
            cmd = new SqlCommand("select CompName,EmailID,ServerIP,DomainName,Status,ROUND(TotalSpace,2) as TotalSpace,ROUND(UsedSpace,2) as UsedSpace,ROUND(AvailableSpace, 2) as AvailableSpace,MaxNoOfUsers from ServerConfig where CompCode='" + CompanyCode + "'", con);
            adapter01 = new SqlDataAdapter(cmd);
            ds02.Reset();
            adapter01.Fill(ds02);
            if (ds02.Tables[0].Rows.Count > 0)
            {
                CompanyName = ds02.Tables[0].Rows[0][0].ToString().Replace(',', ' ');;
                AdminEmailID = ds02.Tables[0].Rows[0][1].ToString();
                ServerIPAddress = ds02.Tables[0].Rows[0][2].ToString();
                DomainName = ds02.Tables[0].Rows[0][3].ToString();
                Status = ds02.Tables[0].Rows[0][4].ToString();
                TotalSpaceInKB = ds02.Tables[0].Rows[0][5].ToString();
                UsedSpaceInKB = ds02.Tables[0].Rows[0][6].ToString();
                AvailableSpaceInKB = ds02.Tables[0].Rows[0][7].ToString();
                MaxNoOfUsers = ds02.Tables[0].Rows[0][8].ToString();
                result = CompanyName + "," + AdminEmailID + "," + ServerIPAddress + "," + DomainName + "," + Status + "," + TotalSpaceInKB + "," + UsedSpaceInKB + "," + AvailableSpaceInKB + "," + MaxNoOfUsers;
            }
            else
            {
                result = "";
            }
            
            return result;
        }



    }
}
