using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS
{
    public class ClassStoreProc
    {
        public DataSet PopStartedWF()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select a.wf_log_id,a.wf_id,b.wf_name,c.doc_id,c.doc_name,a.wf_prog_stat,a.started_by from wf_log_mast a,wf_mast b,doc_mast c where a.wf_id=b.wf_id and a.doc_id=c.doc_id
            cmd = new SqlCommand("sp_PopStartedWF", con);
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet gvStartedWF(string WFLogID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select a.step_no,a.task_id,b.task_name,c.assign_dt,c.due_dt,a.task_done_dt from wf_log_task a,task_mast b,wf_log_dtl c where a.task_id=b.task_id and a.step_no=c.step_no and a.wf_log_id=c.wf_log_id and a.wf_log_id=@WFLogID order by a.step_no
            cmd = new SqlCommand("sp_gvStartedWF", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet IsCabinetEmpty(string CabUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from drawer_mast where cab_uuid=@CabUUID
            cmd = new SqlCommand("sp_IsCabinetEmpty", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CabUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@CabUUID"].Value = CabUUID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet UserInfoPassingEmailID(string EmailID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from user_mast where email=@EmailID
            cmd = new SqlCommand("sp_UserInfoPassingEmailID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EmailID"].Value = EmailID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet AssignedRoleID(string EmailID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from user_role where user_id in(select user_id from user_mast where email='" + EmailID + "')
            cmd = new SqlCommand("sp_AssignedRoleID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EmailID"].Value = EmailID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet CheckCompStat(string EmailID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from ServerConfig where CompCode in(select CompCode from user_mast where email='" + EmailID + "')
            cmd = new SqlCommand("sp_CheckCompStat", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EmailID"].Value = EmailID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet SelectCompAll()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from ServerConfig where CompCode not in('00000000') order by CompName
            cmd = new SqlCommand("sp_SelectCompAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet UserInfoPassingUserID(string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from user_mast where user_id=@UserID
            cmd = new SqlCommand("sp_UserInfoPassingUserID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DocDetails(string DocUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from doc_mast where uuid=@DocUUID
            cmd = new SqlCommand("sp_DocDetailsPassingDocUUID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DocUUID"].Value = DocUUID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DocTypeDetails(string DocTypeID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from doc_type_mast where doc_type_id=@DocTypeID and CompCode=@CompCode
            cmd = new SqlCommand("sp_DocTypeDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DocTypeID", SqlDbType.NVarChar, 10);
            cmd.Parameters["@DocTypeID"].Value = DocTypeID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DeptDetails(string DeptName)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select dept_id from dept_mast where dept_name =@DeptName
            cmd = new SqlCommand("sp_DeptDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DeptName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DeptName"].Value = DeptName;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string InsertServerConfig(string QuickPDFLicenseKey, string ServerIP, string DomainName, string CompName, string HotlineNumber, string HotlineEmail, string CompCode, string ContactPersonName, string EmailID, string PhoneNo, string WorkspaceName, string WorkspaceTitle, string Status, double TotalSpace, double UsedSpace, double AvailableSpace, int MaxNoOfUsers, double SpaceRate, double UserRate, double TotalRate)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_ServerConfigInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@QuickPDFLicenseKey", SqlDbType.NVarChar, 255);
            cmd.Parameters["@QuickPDFLicenseKey"].Value = QuickPDFLicenseKey;

            cmd.Parameters.Add("@ServerIP", SqlDbType.NVarChar, 255);
            cmd.Parameters["@ServerIP"].Value = ServerIP;

            cmd.Parameters.Add("@DomainName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DomainName"].Value = DomainName;

            cmd.Parameters.Add("@CompName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@CompName"].Value = CompName;

            cmd.Parameters.Add("@HotlineNumber", SqlDbType.NVarChar, 255);
            cmd.Parameters["@HotlineNumber"].Value = HotlineNumber;

            cmd.Parameters.Add("@HotlineEmail", SqlDbType.NVarChar, 255);
            cmd.Parameters["@HotlineEmail"].Value = HotlineEmail;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@ContactPersonName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@ContactPersonName"].Value = ContactPersonName;

            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EmailID"].Value = EmailID;

            cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar, 255);
            cmd.Parameters["@PhoneNo"].Value = PhoneNo;

            cmd.Parameters.Add("@WorkspaceName", SqlDbType.NVarChar, 100);
            cmd.Parameters["@WorkspaceName"].Value = WorkspaceName;

            cmd.Parameters.Add("@WorkspaceTitle", SqlDbType.NVarChar, 100);
            cmd.Parameters["@WorkspaceTitle"].Value = WorkspaceTitle;

            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 10);
            cmd.Parameters["@Status"].Value = Status;

            cmd.Parameters.Add("@TotalSpace", SqlDbType.Float);
            cmd.Parameters["@TotalSpace"].Value = TotalSpace;

            cmd.Parameters.Add("@UsedSpace", SqlDbType.Float);
            cmd.Parameters["@UsedSpace"].Value = UsedSpace;

            cmd.Parameters.Add("@AvailableSpace", SqlDbType.Float);
            cmd.Parameters["@AvailableSpace"].Value = AvailableSpace;

            cmd.Parameters.Add("@MaxNoOfUsers", SqlDbType.BigInt);
            cmd.Parameters["@MaxNoOfUsers"].Value = MaxNoOfUsers;

            cmd.Parameters.Add("@SpaceRate", SqlDbType.Float);
            cmd.Parameters["@SpaceRate"].Value = SpaceRate;

            cmd.Parameters.Add("@UserRate", SqlDbType.Float);
            cmd.Parameters["@UserRate"].Value = UserRate;

            cmd.Parameters.Add("@TotalRate", SqlDbType.Float);
            cmd.Parameters["@TotalRate"].Value = TotalRate;

            cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime);
            cmd.Parameters["@CreationDate"].Value = DateTime.Now;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string UpdateServerConfig(string QuickPDFLicenseKey, string ServerIP, string DomainName, string CompName, string HotlineNumber, string HotlineEmail, string CompCode, string ContactPersonName, string EmailID, string PhoneNo, string WorkspaceName, string WorkspaceTitle, string Status)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_ServerConfigUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@QuickPDFLicenseKey", SqlDbType.NVarChar, 255);
            cmd.Parameters["@QuickPDFLicenseKey"].Value = QuickPDFLicenseKey;

            cmd.Parameters.Add("@ServerIP", SqlDbType.NVarChar, 255);
            cmd.Parameters["@ServerIP"].Value = ServerIP;

            cmd.Parameters.Add("@DomainName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DomainName"].Value = DomainName;

            cmd.Parameters.Add("@CompName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@CompName"].Value = CompName;

            cmd.Parameters.Add("@HotlineNumber", SqlDbType.NVarChar, 255);
            cmd.Parameters["@HotlineNumber"].Value = HotlineNumber;

            cmd.Parameters.Add("@HotlineEmail", SqlDbType.NVarChar, 255);
            cmd.Parameters["@HotlineEmail"].Value = HotlineEmail;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@ContactPersonName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@ContactPersonName"].Value = ContactPersonName;

            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EmailID"].Value = EmailID;

            cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar, 255);
            cmd.Parameters["@PhoneNo"].Value = PhoneNo;

            cmd.Parameters.Add("@WorkspaceName", SqlDbType.NVarChar, 100);
            cmd.Parameters["@WorkspaceName"].Value = WorkspaceName;

            cmd.Parameters.Add("@WorkspaceTitle", SqlDbType.NVarChar, 100);
            cmd.Parameters["@WorkspaceTitle"].Value = WorkspaceTitle;

            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 10);
            cmd.Parameters["@Status"].Value = Status;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet SelectServerConfig(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //SELECT * FROM ServerConfig where CompCode=@CompCode
            cmd = new SqlCommand("sp_ServerConfigSelect", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet SelectServerConfigAll()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("sp_ServerConfigSelectAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet SelectBillTypeAll()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("sp_SelectBillTypeAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DeptAllBasedOnCompCode(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_DeptAllBasedOnCompCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet SelectCompActiveAll()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_ServerConfigSelectActiveAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string SelectMaxUserID(string CompCode)
        {
            //SqlConnection con = Utility.GetConnection();
            //SqlCommand cmd = null;
            //con.Open();
            //cmd = new SqlCommand("sp_SelectMaxUserID", con);
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 5);
            //cmd.Parameters["@CompCode"].Value = CompCode+"_";

            //DataSet ds = new DataSet();
            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //adapter.Fill(ds);
            //Utility.CloseConnection(con);
            //return ds;

            string MaxUserID = "";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("SELECT MAX(user_id) FROM user_mast where user_id like('" + CompCode + "_" + "%')", con);
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                MaxUserID = CompCode + "_" + "00000000001";
            }
            else
            {
                string mMaxTotalUserID = (string)cmd.ExecuteScalar();
                int LastSlashPos = mMaxTotalUserID.LastIndexOf("_");
                mMaxTotalUserID = mMaxTotalUserID.Substring(LastSlashPos + 1, 11);
                int mMaxTotalUserID1 = Int32.Parse(mMaxTotalUserID) + 1;
                mMaxTotalUserID = mMaxTotalUserID1.ToString().PadLeft(11, '0');
                MaxUserID = CompCode + "_" + mMaxTotalUserID;
            }
            Utility.CloseConnection(con);
            return MaxUserID;
        }

        public string InsertUserMast(string UserID, string FName, string LName, string EMail, string UserPwd, string Title, string Dept, string Stat, string PwdStat, string CanChangePwd, string CompCode, string UserType, DateTime CreationDt)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_UserMastInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@user_id"].Value = UserID;

            cmd.Parameters.Add("@f_name", SqlDbType.NVarChar, 100);
            cmd.Parameters["@f_name"].Value = FName;

            cmd.Parameters.Add("@l_name", SqlDbType.NVarChar, 100);
            cmd.Parameters["@l_name"].Value = LName;

            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 255);
            cmd.Parameters["@email"].Value = EMail;

            cmd.Parameters.Add("@user_pwd", SqlDbType.NVarChar, 100);
            cmd.Parameters["@user_pwd"].Value = UserPwd;

            cmd.Parameters.Add("@user_title", SqlDbType.NVarChar, 100);
            cmd.Parameters["@user_title"].Value = Title;

            cmd.Parameters.Add("@user_dept", SqlDbType.NVarChar, 5);
            cmd.Parameters["@user_dept"].Value = Dept;

            cmd.Parameters.Add("@user_stat", SqlDbType.NVarChar, 1);
            cmd.Parameters["@user_stat"].Value = Stat;

            cmd.Parameters.Add("@PwdStat", SqlDbType.NVarChar, 50);
            cmd.Parameters["@PwdStat"].Value = PwdStat;

            cmd.Parameters.Add("@CanChangePwd", SqlDbType.NVarChar, 1);
            cmd.Parameters["@CanChangePwd"].Value = CanChangePwd;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@UserType", SqlDbType.NVarChar, 1);
            cmd.Parameters["@UserType"].Value = UserType;

            cmd.Parameters.Add("@CreationDt", SqlDbType.DateTime);
            cmd.Parameters["@CreationDt"].Value = CreationDt;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet SelectUserAll(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("sp_SelectUserAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string InsertDeptMast(string DeptCode, string DeptName, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_DeptMastInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
            cmd.Parameters["@dept_id"].Value = DeptCode;

            cmd.Parameters.Add("@dept_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@dept_name"].Value = DeptName;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }
        
        public string InsertCabinetMast(string CabinetName, string CabinetDesc, string UUID, string DefaultPermission, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_CabinetMastInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@cab_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cab_name"].Value = CabinetName;

            cmd.Parameters.Add("@cab_desc", SqlDbType.Text, 1000);
            cmd.Parameters["@cab_desc"].Value = CabinetDesc;

            cmd.Parameters.Add("@cab_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cab_uuid"].Value = UUID;

            cmd.Parameters.Add("@DefaultPermission", SqlDbType.NVarChar, 50);
            cmd.Parameters["@DefaultPermission"].Value = DefaultPermission;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }
        
        public string InsertDrawerMast(string DrawerName, string DrawerDesc, string CabinetCode, string drw_uuid, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_DrawerMastInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@drw_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@drw_name"].Value = DrawerName;

            cmd.Parameters.Add("@drw_desc", SqlDbType.NText);
            cmd.Parameters["@drw_desc"].Value = DrawerDesc;

            cmd.Parameters.Add("@cab_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cab_uuid"].Value = CabinetCode;

            cmd.Parameters.Add("@drw_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@drw_uuid"].Value = drw_uuid;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertFolderMast(string FolderName, string FolderDesc, string DrawerCode, string fld_uuid, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_FolderMastInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@fld_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_name"].Value = FolderName;

            cmd.Parameters.Add("@fld_desc", SqlDbType.NText);
            cmd.Parameters["@fld_desc"].Value = FolderDesc;

            cmd.Parameters.Add("@drw_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@drw_uuid"].Value = DrawerCode;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_uuid"].Value = fld_uuid;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertDocTypeMast(string DocTypeID, string DocTypeName, string Tag1, string Tag2, string Tag3, string Tag4, string Tag5, string Tag6, string Tag7, string Tag8, string Tag9, string Tag10, int Tag1FieldNo, int Tag2FieldNo, int Tag3FieldNo, int Tag4FieldNo, int Tag5FieldNo, int Tag6FieldNo, int Tag7FieldNo, int Tag8FieldNo, int Tag9FieldNo, int Tag10FieldNo, int SignFieldNo1, int SignDateFieldNo1, int SignFieldNo2, int SignDateFieldNo2, int SignFieldNo3, int SignDateFieldNo3, string CompCode, string FormType)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_DocTypeInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = DocTypeID;

            cmd.Parameters.Add("@doc_type_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@doc_type_name"].Value = DocTypeName;

            cmd.Parameters.Add("@tag1", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag1"].Value = Tag1;

            cmd.Parameters.Add("@tag2", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag2"].Value = Tag2;

            cmd.Parameters.Add("@tag3", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag3"].Value = Tag3;

            cmd.Parameters.Add("@tag4", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag4"].Value = Tag4;

            cmd.Parameters.Add("@tag5", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag5"].Value = Tag5;

            cmd.Parameters.Add("@tag6", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag6"].Value = Tag6;

            cmd.Parameters.Add("@tag7", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag7"].Value = Tag7;

            cmd.Parameters.Add("@tag8", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag8"].Value = Tag8;

            cmd.Parameters.Add("@tag9", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag9"].Value = Tag9;

            cmd.Parameters.Add("@tag10", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag10"].Value = Tag10;

            cmd.Parameters.Add("@tag1fieldno", SqlDbType.Int);
            cmd.Parameters["@tag1fieldno"].Value = Tag1FieldNo;

            cmd.Parameters.Add("@tag2fieldno", SqlDbType.Int);
            cmd.Parameters["@tag2fieldno"].Value = Tag2FieldNo;

            cmd.Parameters.Add("@tag3fieldno", SqlDbType.Int);
            cmd.Parameters["@tag3fieldno"].Value = Tag3FieldNo;

            cmd.Parameters.Add("@tag4fieldno", SqlDbType.Int);
            cmd.Parameters["@tag4fieldno"].Value = Tag4FieldNo;

            cmd.Parameters.Add("@tag5fieldno", SqlDbType.Int);
            cmd.Parameters["@tag5fieldno"].Value = Tag5FieldNo;

            cmd.Parameters.Add("@tag6fieldno", SqlDbType.Int);
            cmd.Parameters["@tag6fieldno"].Value = Tag6FieldNo;

            cmd.Parameters.Add("@tag7fieldno", SqlDbType.Int);
            cmd.Parameters["@tag7fieldno"].Value = Tag7FieldNo;

            cmd.Parameters.Add("@tag8fieldno", SqlDbType.Int);
            cmd.Parameters["@tag8fieldno"].Value = Tag8FieldNo;

            cmd.Parameters.Add("@tag9fieldno", SqlDbType.Int);
            cmd.Parameters["@tag9fieldno"].Value = Tag9FieldNo;

            cmd.Parameters.Add("@tag10fieldno", SqlDbType.Int);
            cmd.Parameters["@tag10fieldno"].Value = Tag10FieldNo;

            cmd.Parameters.Add("@SignFieldNo1", SqlDbType.Int);
            cmd.Parameters["@SignFieldNo1"].Value = SignFieldNo1;

            cmd.Parameters.Add("@SignDateFieldNo1", SqlDbType.Int);
            cmd.Parameters["@SignDateFieldNo1"].Value = SignDateFieldNo1;

            cmd.Parameters.Add("@SignFieldNo2", SqlDbType.Int);
            cmd.Parameters["@SignFieldNo2"].Value = SignFieldNo2;

            cmd.Parameters.Add("@SignDateFieldNo2", SqlDbType.Int);
            cmd.Parameters["@SignDateFieldNo2"].Value = SignDateFieldNo2;

            cmd.Parameters.Add("@SignFieldNo3", SqlDbType.Int);
            cmd.Parameters["@SignFieldNo3"].Value = SignFieldNo3;

            cmd.Parameters.Add("@SignDateFieldNo3", SqlDbType.Int);
            cmd.Parameters["@SignDateFieldNo3"].Value = SignDateFieldNo3;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@FormType", SqlDbType.NVarChar, 50);
            cmd.Parameters["@FormType"].Value = FormType;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string ExistDoc(string DocName, string FolderCode, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_DocExists", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_name", SqlDbType.Text);
            cmd.Parameters["@doc_name"].Value = DocName;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_uuid"].Value = FolderCode;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertDocMast(string doc_name, string doc_desc, string fld_uuid, string doc_type_id, string dept_id, string upld_by, DateTime upld_dt, string tag1, string tag2, string tag3, string tag4, string tag5, string tag6, string tag7, string tag8, string tag9, string tag10, string Download_Path, string uuid, string Doc_Path, string DocUpldType, string CompCode, double FileSize)
        {
            Download_Path = "http://127.0.0.1:8080/share/proxy/alfresco/api/node/content/workspace/SpacesStore/" + Doc_Path + "?a=true";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_DocMastInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_name", SqlDbType.Text);
            cmd.Parameters["@doc_name"].Value = doc_name;

            cmd.Parameters.Add("@doc_desc", SqlDbType.Text);
            cmd.Parameters["@doc_desc"].Value = doc_desc;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_uuid"].Value = fld_uuid;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = doc_type_id;

            cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
            cmd.Parameters["@dept_id"].Value = dept_id;

            cmd.Parameters.Add("@upld_by", SqlDbType.NVarChar, 20);
            cmd.Parameters["@upld_by"].Value = upld_by;

            cmd.Parameters.Add("@upld_dt", SqlDbType.SmallDateTime);
            cmd.Parameters["@upld_dt"].Value = upld_dt;

            cmd.Parameters.Add("@tag1", SqlDbType.Text);
            cmd.Parameters["@tag1"].Value = tag1;

            cmd.Parameters.Add("@tag2", SqlDbType.Text);
            cmd.Parameters["@tag2"].Value = tag2;

            cmd.Parameters.Add("@tag3", SqlDbType.Text);
            cmd.Parameters["@tag3"].Value = tag3;

            cmd.Parameters.Add("@tag4", SqlDbType.Text);
            cmd.Parameters["@tag4"].Value = tag4;

            cmd.Parameters.Add("@tag5", SqlDbType.Text);
            cmd.Parameters["@tag5"].Value = tag5;

            cmd.Parameters.Add("@tag6", SqlDbType.Text);
            cmd.Parameters["@tag6"].Value = tag6;

            cmd.Parameters.Add("@tag7", SqlDbType.Text);
            cmd.Parameters["@tag7"].Value = tag7;

            cmd.Parameters.Add("@tag8", SqlDbType.Text);
            cmd.Parameters["@tag8"].Value = tag8;

            cmd.Parameters.Add("@tag9", SqlDbType.Text);
            cmd.Parameters["@tag9"].Value = tag9;

            cmd.Parameters.Add("@tag10", SqlDbType.Text);
            cmd.Parameters["@tag10"].Value = tag10;

            cmd.Parameters.Add("@Download_Path", SqlDbType.Text);
            cmd.Parameters["@Download_Path"].Value = Download_Path;

            cmd.Parameters.Add("@uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@uuid"].Value = uuid;

            cmd.Parameters.Add("@DocUpldType", SqlDbType.NVarChar, 2);
            cmd.Parameters["@DocUpldType"].Value = DocUpldType;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@DocSizeInKB", SqlDbType.Float);
            cmd.Parameters["@DocSizeInKB"].Value = FileSize;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet SelectDocTypeCompBased(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("sp_DocTypeSelectCompBased", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet SelectCabinetAll(string CompCode,string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            //cmd = new SqlCommand("Cabinet_Select", con);
            cmd = new SqlCommand("sp_SelectCabinetAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet CabinetPermissionM(string CompCode, string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_CabinetPermissionM", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DrawerAllSelectCabinetBased(string cab_uuid, string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_DrawerAllSelectCabinetBased", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@cab_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cab_uuid"].Value = cab_uuid;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DrawerPermissionM(string cab_uuid, string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_DrawerPermissionM", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@cab_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cab_uuid"].Value = cab_uuid;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet FolderAllSelectDrawerBased(string DrawerUUID, string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_FolderAllSelectDrawerBased", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DrawerUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DrawerUUID"].Value = DrawerUUID;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet FolderPermissionM(string DrawerUUID, string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_FolderPermissionM", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DrawerUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DrawerUUID"].Value = DrawerUUID;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet SelectDocsAllBasedOnFolder(string FldUUID, string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_SelectDocsAllBasedOnFolder", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@FldUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FldUUID"].Value = FldUUID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DocDetailsSelectPassingDocID(int DocID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select a.*,b.doc_type_name,(c.f_name + ' ' + c.l_name + ' (' + c.user_id + ')') as UploadedBy from doc_mast a,doc_type_mast b, user_mast c where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.upld_by=c.user_id and a.doc_id=@DocID and a.CompCode=@CompCode
            cmd = new SqlCommand("sp_DocDetailsSelectPassingDocID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DocID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DocID"].Value = DocID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet SelectTagBasedOnDocTypeCompCode(string doc_type_id, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_TagSelect", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = doc_type_id;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string DefinedWF(string doc_type_id, string dept_id, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_SelectDefaultWF", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = doc_type_id;

            cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
            cmd.Parameters["@dept_id"].Value = dept_id;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string StartDefaultWF(string DocTypeCode, string DeptCode, DateTime start_dt, int doc_id, int wf_id, string user_id, string CompCode)
        {
            /// Populate the Workflow ID in customized format (XXXXXXXXXX/XXXXX/XX/XXXX/XXXXX) and store it in <_WFLogID> variable
            string wf_log_id = PopulateWFID(DocTypeCode, DeptCode, start_dt);

            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_StartDefaultWFLogMast", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_log_id", SqlDbType.NVarChar, 30);
            cmd.Parameters["@wf_log_id"].Value = wf_log_id;

            cmd.Parameters.Add("@doc_id", SqlDbType.BigInt);
            cmd.Parameters["@doc_id"].Value = doc_id;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = wf_id;

            cmd.Parameters.Add("@start_dt", SqlDbType.SmallDateTime);
            cmd.Parameters["@start_dt"].Value = start_dt;

            cmd.Parameters.Add("@user_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@user_id"].Value = user_id;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.NVarChar, 30);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        protected string PopulateWFID(string mDocTypeCode, string mDeptCode, DateTime mStartDt)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            string mMonth = (mStartDt.Month.ToString().PadLeft(2, '0'));
            string mYear = (mStartDt.Year.ToString().PadLeft(4, '0'));
            string mIDAbbr = "";
            string mCustomID = "";
            mDocTypeCode = mDocTypeCode.ToString().PadLeft(10, '0');
            mIDAbbr = mDocTypeCode + "/";
            mDeptCode = mDeptCode.ToString().PadLeft(5, '0');
            mIDAbbr += mDeptCode + "/";
            mIDAbbr += mMonth + "/" + mYear + "/";

            con.Open();
            cmd = new SqlCommand("SELECT MAX(wf_log_id) FROM wf_log_mast where wf_log_id like('" + mIDAbbr + "%')", con);
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                mCustomID = mIDAbbr + "00001";
            }
            else
            {
                mCustomID = (string)cmd.ExecuteScalar();
                int LastSlashPos = mCustomID.LastIndexOf("/");
                mCustomID = mCustomID.Substring(LastSlashPos + 1, 5);
                int mCustomID1 = Int32.Parse(mCustomID) + 1;
                mCustomID = mCustomID1.ToString().PadLeft(5, '0');
                mCustomID = mIDAbbr + mCustomID;
            }

            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);
            return mCustomID;
        }

        public DataSet SelectWFDtls(int wf_id, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_RoleSelect", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = wf_id;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string StartDefaultWFLogDtl(string wf_log_id, int step_no, DateTime assign_dt, string Duration, DateTime due_dt, string CompCode)
        {
            DateTime Calc_Due_Dt= DateTime.Now;
            if (step_no == 1)
            {
                Calc_Due_Dt = assign_dt;
            }
            else
            {
                assign_dt = Calc_Due_Dt;
            }
            due_dt = Calc_Due_Dt.AddHours(Convert.ToDouble(Duration));
            Calc_Due_Dt = due_dt;

            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_StartDefaultWFLogDtl", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_log_id", SqlDbType.NVarChar, 30);
            cmd.Parameters["@wf_log_id"].Value = wf_log_id;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = step_no;

            cmd.Parameters.Add("@assign_dt", SqlDbType.SmallDateTime);
            cmd.Parameters["@assign_dt"].Value = assign_dt;

            cmd.Parameters.Add("@due_dt", SqlDbType.SmallDateTime);
            cmd.Parameters["@due_dt"].Value = due_dt;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.NVarChar, 30);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet SelectWFTasks(int wf_id, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_TaskSelect", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = wf_id;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string StartDefaultWFLogTask(string wf_log_id, int step_no, string task_id, string AmbleMails, string AmbleMsg, string AmbleAttach, string AppendDoc, string AmbleURL, string AmbleSub, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_StartDefaultWFLogTask", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_log_id", SqlDbType.NVarChar, 30);
            cmd.Parameters["@wf_log_id"].Value = wf_log_id;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = step_no;

            cmd.Parameters.Add("@task_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@task_id"].Value = task_id;

            cmd.Parameters.Add("@AmbleMails", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AmbleMails"].Value = AmbleMails;

            cmd.Parameters.Add("@AmbleMsg", SqlDbType.Text);
            cmd.Parameters["@AmbleMsg"].Value = AmbleMsg;

            cmd.Parameters.Add("@AmbleAttach", SqlDbType.NVarChar, 20);
            cmd.Parameters["@AmbleAttach"].Value = AmbleAttach;

            cmd.Parameters.Add("@AppendDoc", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AppendDoc"].Value = AppendDoc;

            cmd.Parameters.Add("@AmbleURL", SqlDbType.NVarChar, 20);
            cmd.Parameters["@AmbleURL"].Value = AmbleURL;

            cmd.Parameters.Add("@AmbleSub", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AmbleSub"].Value = AmbleSub;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.NVarChar, 30);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet SetUploadedLocation(string FldUUID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_SetUploadedLocation", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FldUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FldUUID"].Value = FldUUID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet FetchUploadedLocation(string DeptID, string DocTypeID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_FetchUploadedLocation", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DeptID", SqlDbType.NVarChar, 5);
            cmd.Parameters["@DeptID"].Value = DeptID;

            cmd.Parameters.Add("@DocTypeID", SqlDbType.NVarChar, 10);
            cmd.Parameters["@DocTypeID"].Value = DocTypeID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DisplayWFDtl(string lbWfid1, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_DisplayWFDtl_DS", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wfid", SqlDbType.BigInt);
            cmd.Parameters["@wfid"].Value = lbWfid1;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string DocMetaValueInsert(string DocUUID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_DocMetaValueInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DocUUID"].Value = DocUUID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string UpdateDocMast(int UpdateOnQuery, string doc_name, string doc_desc, string fld_uuid, string doc_type_id, string dept_id, string upld_by, DateTime upld_dt, string tag1, string tag2, string tag3, string tag4, string tag5, string tag6, string tag7, string tag8, string tag9, string tag10, string Download_Path, string uuid, string Doc_Path, string CompCode)
        {
            Download_Path = "http://127.0.0.1:8080/share/proxy/alfresco/api/node/content/workspace/SpacesStore/" + Doc_Path + "?a=true";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_DocMastUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UpdateOnQuery", SqlDbType.Int);
            cmd.Parameters["@UpdateOnQuery"].Value = UpdateOnQuery;

            cmd.Parameters.Add("@doc_name", SqlDbType.Text);
            cmd.Parameters["@doc_name"].Value = doc_name;

            cmd.Parameters.Add("@doc_desc", SqlDbType.Text);
            cmd.Parameters["@doc_desc"].Value = doc_desc;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_uuid"].Value = fld_uuid;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = doc_type_id;

            cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
            cmd.Parameters["@dept_id"].Value = dept_id;

            cmd.Parameters.Add("@upld_by", SqlDbType.NVarChar, 20);
            cmd.Parameters["@upld_by"].Value = upld_by;

            cmd.Parameters.Add("@upld_dt", SqlDbType.SmallDateTime);
            cmd.Parameters["@upld_dt"].Value = upld_dt;

            cmd.Parameters.Add("@tag1", SqlDbType.Text);
            cmd.Parameters["@tag1"].Value = tag1;

            cmd.Parameters.Add("@tag2", SqlDbType.Text);
            cmd.Parameters["@tag2"].Value = tag2;

            cmd.Parameters.Add("@tag3", SqlDbType.Text);
            cmd.Parameters["@tag3"].Value = tag3;

            cmd.Parameters.Add("@tag4", SqlDbType.Text);
            cmd.Parameters["@tag4"].Value = tag4;

            cmd.Parameters.Add("@tag5", SqlDbType.Text);
            cmd.Parameters["@tag5"].Value = tag5;

            cmd.Parameters.Add("@tag6", SqlDbType.Text);
            cmd.Parameters["@tag6"].Value = tag6;

            cmd.Parameters.Add("@tag7", SqlDbType.Text);
            cmd.Parameters["@tag7"].Value = tag7;

            cmd.Parameters.Add("@tag8", SqlDbType.Text);
            cmd.Parameters["@tag8"].Value = tag8;

            cmd.Parameters.Add("@tag9", SqlDbType.Text);
            cmd.Parameters["@tag9"].Value = tag9;

            cmd.Parameters.Add("@tag10", SqlDbType.Text);
            cmd.Parameters["@tag10"].Value = tag10;

            cmd.Parameters.Add("@Download_Path", SqlDbType.Text);
            cmd.Parameters["@Download_Path"].Value = Download_Path;

            cmd.Parameters.Add("@uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@uuid"].Value = uuid;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string UpdatePermissionAllUser(string Permission, string NodeUUID, string UserID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_UpdatePermissionAllUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Permission", SqlDbType.NVarChar, 50);
            cmd.Parameters["@Permission"].Value = Permission;

            cmd.Parameters.Add("@NodeUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NodeUUID"].Value = NodeUUID;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet SelectCabUUIDPassingFolderUUID(string FolderUUID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_SelectCabUUIDPassingFolderUUID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FolderUUID"].Value = FolderUUID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string WFDocVersionInsert(string WFLogID,int StepNo,string ActualDocUUID,string NewDocUUID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WFDocVersionInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            cmd.Parameters.Add("@StepNo", SqlDbType.Int);
            cmd.Parameters["@StepNo"].Value = StepNo;

            cmd.Parameters.Add("@ActualDocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@ActualDocUUID"].Value = ActualDocUUID;

            cmd.Parameters.Add("@NewDocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NewDocUUID"].Value = NewDocUUID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);
            return param.Value.ToString();
        }

        public string WFDocVersionUpdate(string WFLogID,int StepNo,string ActualDocUUID,string NewDocUUID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WFDocVersionUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            cmd.Parameters.Add("@StepNo", SqlDbType.Int);
            cmd.Parameters["@StepNo"].Value = StepNo;

            cmd.Parameters.Add("@ActualDocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@ActualDocUUID"].Value = ActualDocUUID;

            cmd.Parameters.Add("@NewDocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NewDocUUID"].Value = NewDocUUID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);
            return param.Value.ToString();
        }

        public string WFLogTaskUpdate(int UpdateOnQuery, string WFLogID, string EMailNeedsToReplace, string EmailReplacedBy, DateTime task_done_dt, string comments, string task_id, int StepNo, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WFLogTaskUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UpdateOnQuery", SqlDbType.Int);
            cmd.Parameters["@UpdateOnQuery"].Value = UpdateOnQuery;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            cmd.Parameters.Add("@EMailNeedsToReplace", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EMailNeedsToReplace"].Value = EMailNeedsToReplace;

            cmd.Parameters.Add("@EmailReplacedBy", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EmailReplacedBy"].Value = EmailReplacedBy;

            cmd.Parameters.Add("@task_done_dt", SqlDbType.DateTime);
            cmd.Parameters["@task_done_dt"].Value = task_done_dt;

            cmd.Parameters.Add("@comments", SqlDbType.NVarChar, 255);
            cmd.Parameters["@comments"].Value = comments;

            cmd.Parameters.Add("@task_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@task_id"].Value = task_id;

            cmd.Parameters.Add("@StepNo", SqlDbType.Int);
            cmd.Parameters["@StepNo"].Value = StepNo;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);
            return param.Value.ToString();
        }

        public string TempDocSavingUpdate(string TempDocName, string UserID, DateTime CreationDate, string TempDocStat, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_TempDocSavingUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@TempDocName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@TempDocName"].Value = TempDocName;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime);
            cmd.Parameters["@CreationDate"].Value = CreationDate;

            cmd.Parameters.Add("@TempDocStat", SqlDbType.NVarChar, 20);
            cmd.Parameters["@TempDocStat"].Value = TempDocStat;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);
            return param.Value.ToString();
        }

        public DataSet SelectAppendTaskInWFL(string DeptCode, string DocTypeCode, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_SelectAppendTaskInWFL", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DeptCode", SqlDbType.NVarChar, 5);
            cmd.Parameters["@DeptCode"].Value = DeptCode;

            cmd.Parameters.Add("@DocTypeCode", SqlDbType.NVarChar, 10);
            cmd.Parameters["@DocTypeCode"].Value = DocTypeCode;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet WFLogTaskSelect(int SelectOnQuery, string WFLogID, int StepNo, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WFLogTaskSelect", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SelectOnQuery", SqlDbType.Int);
            cmd.Parameters["@SelectOnQuery"].Value = SelectOnQuery;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            cmd.Parameters.Add("@StepNo", SqlDbType.Int);
            cmd.Parameters["@StepNo"].Value = StepNo;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string WFLogUpdate(int UpdateOnQuery, string WFLogID, string TaskDoneDate, string Comments, string TaskID, int StepNo,string UserID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WFLogUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UpdateOnQuery", SqlDbType.Int);
            cmd.Parameters["@UpdateOnQuery"].Value = UpdateOnQuery;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            cmd.Parameters.Add("@TaskDoneDate", SqlDbType.NVarChar, 255);
            cmd.Parameters["@TaskDoneDate"].Value = TaskDoneDate;

            cmd.Parameters.Add("@Comments", SqlDbType.NVarChar, 255);
            cmd.Parameters["@Comments"].Value = Comments;

            cmd.Parameters.Add("@TaskID", SqlDbType.NVarChar, 10);
            cmd.Parameters["@TaskID"].Value = TaskID;

            cmd.Parameters.Add("@StepNo", SqlDbType.Int);
            cmd.Parameters["@StepNo"].Value = StepNo;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);
            return param.Value.ToString();
        }

        public string InsertRoleMast(string role_id, string role_name, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_RoleInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@role_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@role_id"].Value = role_id;

            cmd.Parameters.Add("@role_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@role_name"].Value = role_name;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet RoleBasedOnCompCode(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_RoleBasedOnCompCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet UserRoleListBasedOnCompCode(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_UserRoleListBasedOnCompCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string InsertUserRoleMast(string user_id, string role_id, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_UserRoleInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@user_id"].Value = user_id;

            cmd.Parameters.Add("@role_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@role_id"].Value = role_id;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet TaskList()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_TaskList", con);
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet PDFDocBasedOnFolder(string fld_uuid)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_PDFDocBasedOnFolder", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_uuid"].Value = fld_uuid;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet WFLMastBasedOnCompCode(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_WFLMastBasedOnCompCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string ClearIncompleteWFBasedOnCompCode(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_ClearIncompleteWFBasedOnCompCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertWFMast(string wf_name, string wf_dept, string wf_doctype, string wf_stat, string wf_folder_uuid, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            cmd = new SqlCommand("sp_WFLMastInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@wf_name"].Value = wf_name;

            cmd.Parameters.Add("@wf_dept", SqlDbType.NVarChar, 5);
            cmd.Parameters["@wf_dept"].Value = wf_dept;

            cmd.Parameters.Add("@wf_doctype", SqlDbType.NVarChar, 10);
            cmd.Parameters["@wf_doctype"].Value = wf_doctype;

            cmd.Parameters.Add("@wf_stat", SqlDbType.NVarChar, 1);
            cmd.Parameters["@wf_stat"].Value = wf_stat;

            cmd.Parameters.Add("@wf_folder_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@wf_folder_uuid"].Value = wf_folder_uuid;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertWFDtl(int wf_id, int step_no, string role_id, string duration, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WorkflowDtlInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = wf_id;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = step_no;

            cmd.Parameters.Add("@role_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@role_id"].Value = role_id;

            cmd.Parameters.Add("@duration", SqlDbType.NVarChar, 10);
            cmd.Parameters["@duration"].Value = duration;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertWFCond(int wf_id, int step_no, string task_id, int form_field_no, string cond_op, string cond_val, string amble_mails, string amble_msg, string amble_attach, string amble_url, string AmbleSub, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WorkflowCondInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = wf_id;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = step_no;

            cmd.Parameters.Add("@task_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@task_id"].Value = task_id;

            cmd.Parameters.Add("@form_field_no", SqlDbType.Int);
            cmd.Parameters["@form_field_no"].Value = form_field_no;

            cmd.Parameters.Add("@cond_op", SqlDbType.NVarChar, 2);
            cmd.Parameters["@cond_op"].Value = cond_op;

            cmd.Parameters.Add("@cond_val", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cond_val"].Value = cond_val;

            cmd.Parameters.Add("@amble_mails", SqlDbType.NVarChar, 255);
            cmd.Parameters["@amble_mails"].Value = amble_mails;

            cmd.Parameters.Add("@amble_msg", SqlDbType.Text);
            cmd.Parameters["@amble_msg"].Value = amble_msg;

            cmd.Parameters.Add("@amble_attach", SqlDbType.NVarChar, 20);
            cmd.Parameters["@amble_attach"].Value = amble_attach;

            cmd.Parameters.Add("@amble_url", SqlDbType.NVarChar, 20);
            cmd.Parameters["@amble_url"].Value = amble_url;

            cmd.Parameters.Add("@AmbleSub", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AmbleSub"].Value = AmbleSub;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertWFSignDate(int wf_id, int step_no, int Sign1, int Date1, int Sign2, int Date2, int Sign3, int Date3, int Sign4, int Date4, int Sign5, int Date5, int Sign6, int Date6, int Sign7, int Date7, int Sign8, int Date8, int Sign9, int Date9, int Sign10, int Date10, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WFSignDateInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = wf_id;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = step_no;

            cmd.Parameters.Add("@Sign1", SqlDbType.Int);
            cmd.Parameters["@Sign1"].Value = Sign1;

            cmd.Parameters.Add("@Date1", SqlDbType.Int);
            cmd.Parameters["@Date1"].Value = Date1;

            cmd.Parameters.Add("@Sign2", SqlDbType.Int);
            cmd.Parameters["@Sign2"].Value = Sign2;

            cmd.Parameters.Add("@Date2", SqlDbType.Int);
            cmd.Parameters["@Date2"].Value = Date2;

            cmd.Parameters.Add("@Sign3", SqlDbType.Int);
            cmd.Parameters["@Sign3"].Value = Sign3;

            cmd.Parameters.Add("@Date3", SqlDbType.Int);
            cmd.Parameters["@Date3"].Value = Date3;

            cmd.Parameters.Add("@Sign4", SqlDbType.Int);
            cmd.Parameters["@Sign4"].Value = Sign4;

            cmd.Parameters.Add("@Date4", SqlDbType.Int);
            cmd.Parameters["@Date4"].Value = Date4;

            cmd.Parameters.Add("@Sign5", SqlDbType.Int);
            cmd.Parameters["@Sign5"].Value = Sign5;

            cmd.Parameters.Add("@Date5", SqlDbType.Int);
            cmd.Parameters["@Date5"].Value = Date5;

            cmd.Parameters.Add("@Sign6", SqlDbType.Int);
            cmd.Parameters["@Sign6"].Value = Sign6;

            cmd.Parameters.Add("@Date6", SqlDbType.Int);
            cmd.Parameters["@Date6"].Value = Date6;

            cmd.Parameters.Add("@Sign7", SqlDbType.Int);
            cmd.Parameters["@Sign7"].Value = Sign7;

            cmd.Parameters.Add("@Date7", SqlDbType.Int);
            cmd.Parameters["@Date7"].Value = Date7;

            cmd.Parameters.Add("@Sign8", SqlDbType.Int);
            cmd.Parameters["@Sign8"].Value = Sign8;

            cmd.Parameters.Add("@Date8", SqlDbType.Int);
            cmd.Parameters["@Date8"].Value = Date8;

            cmd.Parameters.Add("@Sign9", SqlDbType.Int);
            cmd.Parameters["@Sign9"].Value = Sign9;

            cmd.Parameters.Add("@Date9", SqlDbType.Int);
            cmd.Parameters["@Date9"].Value = Date9;

            cmd.Parameters.Add("@Sign10", SqlDbType.Int);
            cmd.Parameters["@Sign10"].Value = Sign10;

            cmd.Parameters.Add("@Date10", SqlDbType.Int);
            cmd.Parameters["@Date10"].Value = Date10;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);
            return param.Value.ToString();
        }

        public string InsertWFTask(int wf_id, int step_no, string task_id, string acttype_id, string copy_to_uuid, string amble_mail, string amble_msg, string amble_attach, string AppendDoc, string amble_url, string AmbleSub, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_WorkflowTaskInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = wf_id;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = step_no;

            cmd.Parameters.Add("@task_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@task_id"].Value = task_id;

            cmd.Parameters.Add("@acttype_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@acttype_id"].Value = acttype_id;

            cmd.Parameters.Add("@copy_to_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@copy_to_uuid"].Value = copy_to_uuid;

            cmd.Parameters.Add("@amble_mail", SqlDbType.NVarChar, 255);
            cmd.Parameters["@amble_mail"].Value = amble_mail;

            cmd.Parameters.Add("@amble_msg", SqlDbType.Text);
            cmd.Parameters["@amble_msg"].Value = amble_msg;

            cmd.Parameters.Add("@amble_attach", SqlDbType.NVarChar, 20);
            cmd.Parameters["@amble_attach"].Value = amble_attach;

            cmd.Parameters.Add("@AppendDoc", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AppendDoc"].Value = AppendDoc;

            cmd.Parameters.Add("@amble_url", SqlDbType.NVarChar, 20);
            cmd.Parameters["@amble_url"].Value = amble_url;

            cmd.Parameters.Add("@AmbleSub", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AmbleSub"].Value = AmbleSub;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet NormalUserListBasedOnCompCode(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_NormalUserListBasedOnCompCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet AdminUserListBasedOnCompCode(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_AdminUserListBasedOnCompCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet ExistNode(string NodeType, string NodeName, string CompCode, string RootNodeUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("sp_ExistNode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@NodeType", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NodeType"].Value = NodeType;

            cmd.Parameters.Add("@NodeName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NodeName"].Value = NodeName;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@RootNodeUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@RootNodeUUID"].Value = RootNodeUUID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public string SelectMaxCompCode(string YYYYMM)
        {
            string MaxCompCode = "";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("SELECT MAX(CompCode) FROM ServerConfig where CompCode like('" + YYYYMM + "%')", con);
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                MaxCompCode = YYYYMM + "01";
            }
            else
            {
                string mMaxTotalCompCode = (string)cmd.ExecuteScalar();
                //int LastSlashPos = mMaxTotalCompCode.LastIndexOf("_");
                mMaxTotalCompCode = mMaxTotalCompCode.Substring(6, 2);
                int mMaxTotalCompCode1 = Int32.Parse(mMaxTotalCompCode) + 1;
                mMaxTotalCompCode = mMaxTotalCompCode1.ToString().PadLeft(2, '0');
                MaxCompCode = YYYYMM + mMaxTotalCompCode;
            }
            Utility.CloseConnection(con);
            return MaxCompCode;
        }

        public string MaxAutoID4DocLog(string ActualDocUUID,string CompCode)
        {
            string MaxAutoID = "";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("SELECT MAX(AutoID) FROM DocLog where ActualDocUUID='" + ActualDocUUID + "' and CompCode='" + CompCode + "'", con);
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                MaxAutoID = "1";
            }
            else
            {
                Int64 mMaxAutoID = (Int64)cmd.ExecuteScalar();
                int mMaxAutoID1 = Int32.Parse(mMaxAutoID.ToString()) + 1;
                MaxAutoID = mMaxAutoID1.ToString();
            }
            Utility.CloseConnection(con);
            return MaxAutoID;
        }

        public string InsertDocLog(string AutoID, string ActualDocUUID, string NewDocUUID, string UserID, string Comments, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_InsertDocLog", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AutoID", SqlDbType.BigInt);
            cmd.Parameters["@AutoID"].Value =Convert.ToInt64(AutoID);

            cmd.Parameters.Add("@ActualDocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@ActualDocUUID"].Value = ActualDocUUID;

            cmd.Parameters.Add("@NewDocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NewDocUUID"].Value = NewDocUUID;

            cmd.Parameters.Add("@OperatedOn", SqlDbType.SmallDateTime);
            cmd.Parameters["@OperatedOn"].Value = DateTime.Now;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@Comments", SqlDbType.NVarChar, 255);
            cmd.Parameters["@Comments"].Value = Comments;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet AllFoldersInsideDrawer(string DrawerUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from folder_mast where drw_uuid='" + DrawerUUID + "'
            cmd = new SqlCommand("sp_AllFoldersInsideDrawer", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DrawerUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DrawerUUID"].Value = DrawerUUID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet FolderNamePassingFolderUUID(string FolderUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select fld_name from folder_mast where fld_uuid='" + FolderUUID + "'
            cmd = new SqlCommand("sp_FolderNamePassingFolderUUID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FolderUUID"].Value = FolderUUID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet AllDocumentsInsideFolder(string FolderUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from doc_mast where fld_uuid='" + FolderUUID + "'
            cmd = new SqlCommand("sp_AllDocumentsInsideFolder", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FolderUUID"].Value = FolderUUID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet FullPathPassingFolderUUID(string FolderUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select a.fld_name,b.drw_name,c.cab_name from folder_mast a,drawer_mast b,cabinet_mast c where a.drw_uuid=b.drw_uuid and b.cab_uuid=c.cab_uuid and a.fld_uuid='" + FolderUUID + "'
            cmd = new SqlCommand("sp_FullPathPassingFolderUUID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FolderUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FolderUUID"].Value = FolderUUID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet FullNamePassingUserID(string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id='" + UserID + "'
            cmd = new SqlCommand("sp_FullNamePassingUserID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet WFLogDetailsPassingDocID(string DocID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select wf_log_id,wf_id from wf_log_mast where doc_id='" + DocID + "'
            cmd = new SqlCommand("sp_WFLogDetailsPassingDocID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DocID", SqlDbType.BigInt);
            cmd.Parameters["@DocID"].Value =Convert.ToInt64(DocID);

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet IntTaskWithCommPassingWFLogID(string WFLogID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select a.StepNo,b.task_name,a.TaskDoneDate,a.Comments,a.UserID from WFLog a,task_mast b where a.TaskID=b.task_id and a.TaskID not like('PRE%') and a.TaskID not like('POST%') and a.TaskDoneDate!='Not Required' and a.WFLogID='" + WFLogID + "'
            cmd = new SqlCommand("sp_IntTaskWithCommPassingWFLogID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet UserPassingWFLogIDANDStep(string WFLogID,string StepNo)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id in(select user_id from user_role where role_id in(select role_id from wf_dtl where step_no='" + StepNo + "' and wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "')))
            cmd = new SqlCommand("sp_UserPassingWFLogIDANDStep", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value =WFLogID;

            cmd.Parameters.Add("@StepNo", SqlDbType.Int);
            cmd.Parameters["@StepNo"].Value = Convert.ToInt64(StepNo);

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet DocDetailsPassingDocIDANDFldUUID(string DocID, string FolderUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from doc_mast where doc_id='" + DocID + "' and fld_uuid='" + FolderUUID + "'
            cmd = new SqlCommand("sp_DocDetailsPassingDocIDANDFldUUID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DocID", SqlDbType.BigInt);
            cmd.Parameters["@DocID"].Value =Convert.ToInt64(DocID);

            cmd.Parameters.Add("@FolderUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FolderUUID"].Value = FolderUUID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public string UpdateUserMast(string UserID, string CompCode, string FirstName, string LastName, string EmailID, string UserTitle, string DeptCode, string UserStatus)
        {
            //SqlConnection con = Utility.GetConnection();
            //SqlCommand cmd = null;
            //con.Open();
            //cmd = new SqlCommand("update user_mast set f_name='" + FirstName + "',l_name='" + LastName + "',email='" + EmailID + "',user_title='" + UserTitle + "',user_dept='" + DeptCode + "',user_stat='" + UserStatus + "' where user_id='" + UserID + "' and CompCode='" + CompCode + "'", con);
            //cmd.ExecuteNonQuery();
            //Utility.CloseConnection(con);
            //return "1";


            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            //update user_mast set f_name='" + FirstName + "',l_name='" + LastName + "',email='" + EmailID + "',user_title='" + UserTitle + "',user_dept='" + DeptCode + "',user_stat='" + UserStatus + "' where user_id='" + UserID + "' and CompCode='" + CompCode + "'
            cmd = new SqlCommand("sp_UpdateUserMast", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100);
            cmd.Parameters["@FirstName"].Value = FirstName;

            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 100);
            cmd.Parameters["@LastName"].Value = LastName;

            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EmailID"].Value = EmailID;

            cmd.Parameters.Add("@UserTitle", SqlDbType.NVarChar, 100);
            cmd.Parameters["@UserTitle"].Value = UserTitle;

            cmd.Parameters.Add("@DeptCode", SqlDbType.NVarChar, 5);
            cmd.Parameters["@DeptCode"].Value = DeptCode;

            cmd.Parameters.Add("@UserStatus", SqlDbType.NVarChar, 1);
            cmd.Parameters["@UserStatus"].Value = UserStatus;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public DataSet UserDetailsPassingUserIDANDEmailID(string UserID, string EmailID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            //select * from user_mast where email='" + EmailID + "' and user_id!='" + UserID + "'
            cmd = new SqlCommand("sp_UserDetailsPassingUserIDANDEmailID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@EmailID"].Value = EmailID;

            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public string FullPathPassingWFID(string WFID)
        {
            //SqlConnection con = Utility.GetConnection();
            //SqlCommand cmd = null;
            //con.Open();
            //DataSet ds_V01 = new DataSet();
            //SqlDataAdapter adapter_V01;
            //string FolderUUID = "";
            //string FullPath = "";

            //ds_V01.Reset();
            //cmd = new SqlCommand("select fld_uuid from wf_mast where wf_id='" + WFID + "'", con);
            //adapter_V01 = new SqlDataAdapter(cmd);
            //adapter_V01.Fill(ds_V01);
            //FolderUUID = ds_V01.Tables[0].Rows[0][0].ToString();

            //ds_V01.Reset();
            //ds_V01 = FullPathPassingFolderUUID(FolderUUID);
            //FullPath = ds_V01.Tables[0].Rows[0][2].ToString() + " >> " + ds_V01.Tables[0].Rows[0][1].ToString() + " >> " + ds_V01.Tables[0].Rows[0][0].ToString();

            //Utility.CloseConnection(con);
            //return FullPath;




            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01;
            string FolderUUID = "";
            string FullPath = "";

            //select fld_uuid from wf_mast where wf_id='" + WFID + "'
            cmd = new SqlCommand("sp_FullPathPassingWFID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WFID", SqlDbType.BigInt);
            cmd.Parameters["@WFID"].Value =Convert.ToInt64(WFID);

            ds_V01 = new DataSet();
            adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            FolderUUID = ds_V01.Tables[0].Rows[0][0].ToString();

            ds_V01.Reset();
            ds_V01 = FullPathPassingFolderUUID(FolderUUID);
            FullPath = ds_V01.Tables[0].Rows[0][2].ToString() + " >> " + ds_V01.Tables[0].Rows[0][1].ToString() + " >> " + ds_V01.Tables[0].Rows[0][0].ToString();
            Utility.CloseConnection(con);
            return FullPath;
        }

        public string UserRightsInsert(string NodeUUID, string NodeType, string UserID, string Permission, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_UserRightsInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@NodeUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NodeUUID"].Value = NodeUUID;

            cmd.Parameters.Add("@NodeType", SqlDbType.NVarChar, 50);
            cmd.Parameters["@NodeType"].Value = NodeType;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@Permission", SqlDbType.NVarChar, 50);
            cmd.Parameters["@Permission"].Value = Permission;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@UpdatedOn", SqlDbType.DateTime);
            cmd.Parameters["@UpdatedOn"].Value = DateTime.Now;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string UserRightsUpdate4UploadingUser(string NodeUUID, string NodeType, string UserID, string Permission, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("sp_UserRightsUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@NodeUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NodeUUID"].Value = NodeUUID;

            cmd.Parameters.Add("@NodeType", SqlDbType.NVarChar, 50);
            cmd.Parameters["@NodeType"].Value = NodeType;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = UserID;

            cmd.Parameters.Add("@Permission", SqlDbType.NVarChar, 50);
            cmd.Parameters["@Permission"].Value = Permission;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            cmd.Parameters.Add("@UpdatedOn", SqlDbType.DateTime);
            cmd.Parameters["@UpdatedOn"].Value = DateTime.Now;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string DocCheckInCheckOut(int doc_id, string DocStat, string CheckedOutBy)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Doc_CheckInOut", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_id", SqlDbType.BigInt);
            cmd.Parameters["@doc_id"].Value = doc_id;

            cmd.Parameters.Add("@DocStat", SqlDbType.NVarChar, 10);
            cmd.Parameters["@DocStat"].Value = DocStat;

            cmd.Parameters.Add("@CheckedOutBy", SqlDbType.NVarChar, 20);
            cmd.Parameters["@CheckedOutBy"].Value = CheckedOutBy;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return param.Value.ToString();
        }

        public DataSet DocTypeDetailsPassingDocUUID(string DocUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select * from doc_type_mast where doc_type_id in(select doc_type_id from doc_mast where uuid='" + DocUUID + "')", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        

        #region For Workflow Editing
        public DataSet WFRecords(string WFID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select * from wf_log_mast where wf_id='" + WFID + "' and (wf_prog_stat!='Completed' and wf_prog_stat!='Rejected')", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet AppRvwTaskList()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("SELECT task_id,task_name FROM task_mast where task_id in('APPROVE','REVIEW') ORDER BY task_name", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public string UpdateWFMast(int wf_id, string WFName, string DeptCode, string DocTypeCode, string FolderUUID,string CompCode)
        {
            string UpdateStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01;
            ds_V01.Reset();
            cmd = new SqlCommand("select * from wf_mast where wf_name='" + WFName + "' and CompCode='" + CompCode + "' and wf_id!='" + wf_id + "'", con);
            adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            if (ds_V01.Tables[0].Rows.Count > 0) // For duplicate WFName
            {
                UpdateStat = "-1";
            }
            else
            {
                ds_V01.Reset();
                cmd = new SqlCommand("select * from wf_mast where dept_id='" + DeptCode + "' and doc_type_id='" + DocTypeCode + "' and CompCode='" + CompCode + "' and wf_id!='" + wf_id + "'", con);
                adapter_V01 = new SqlDataAdapter(cmd);
                adapter_V01.Fill(ds_V01);
                if (ds_V01.Tables[0].Rows.Count > 0) // Duplicate Dept & DocType
                {
                    UpdateStat = "-2";
                }
                else
                {
                    cmd = new SqlCommand("update wf_mast set wf_name='" + WFName + "',dept_id='" + DeptCode + "',doc_type_id='" + DocTypeCode + "',fld_uuid='" + FolderUUID + "' where wf_id='" + wf_id + "'", con);
                    cmd.ExecuteNonQuery();
                    UpdateStat = "1";
                }
            }

            Utility.CloseConnection(con);
            return UpdateStat;
        }

        public DataSet FetchWFMasterData(string WFID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select * from wf_mast where wf_id='" + WFID + "'", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet FetchCabinetDrawerUUID(string FolderUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select drw_uuid from folder_mast where fld_uuid='" + FolderUUID + "'", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            if (ds_V01.Tables[0].Rows.Count > 0)
            {
                cmd = new SqlCommand("select cab_uuid,drw_uuid from drawer_mast where drw_uuid='" + ds_V01.Tables[0].Rows[0][0].ToString() + "'", con);
                ds_V01.Reset();
                adapter_V01 = new SqlDataAdapter(cmd);
                adapter_V01.Fill(ds_V01);
            }
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet WFDetails(int WFID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select a.wf_id,a.step_no,a.role_id,a.duration,b.role_name from wf_dtl a,role_mast b where a.role_id=b.role_id and a.CompCode=b.CompCode and a.wf_id='" + WFID + "' and a.CompCode='" + CompCode + "' and b.CompCode='" + CompCode + "'", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public string UpdateWFDetails(int WFID, int StepNo, string RoleID, string Duration)
        {
            string UpdateStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01;
            ds_V01.Reset();
            cmd = new SqlCommand("select * from wf_dtl where role_id='" + RoleID + "' and wf_id='" + WFID + "' and step_no!='" + StepNo + "'", con);
            adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            if (ds_V01.Tables[0].Rows.Count > 0) // For duplicate Role
            {
                UpdateStat = "-1";
            }
            else
            {
                if (StepNo == 1)
                {
                    if (RoleID != "INIT")
                    {
                        UpdateStat = "-2";
                    }
                }
                else
                {
                    cmd = new SqlCommand("update wf_dtl set role_id='" + RoleID + "',duration='" + Duration + "' where wf_id='" + WFID + "' and step_no='" + StepNo + "'", con);
                    cmd.ExecuteNonQuery();
                    UpdateStat = "1";
                }
            }

            Utility.CloseConnection(con);
            return UpdateStat;
        }

        public string DeleteWFDetails(int WFID, int StepNo)
        {
            string DeleteStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            SqlDataAdapter adapter_V01;

            // For wf_dtl table
            cmd = new SqlCommand("delete from wf_dtl where wf_id='" + WFID + "' and step_no='" + StepNo + "'", con);
            cmd.ExecuteNonQuery();
            ds_V01.Reset();
            cmd = new SqlCommand("select * from wf_dtl where wf_id='" + WFID + "' and step_no>'" + StepNo + "' order by step_no", con);
            adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            if (ds_V01.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds_V01.Tables[0].Rows.Count; i++)
                {
                    cmd = new SqlCommand("update wf_dtl set step_no='" + (Convert.ToInt32(ds_V01.Tables[0].Rows[i][1].ToString()) - 1).ToString() + "' where wf_id='" + WFID + "' and step_no='" + ds_V01.Tables[0].Rows[i][1].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                }
            }
            // For wf_task table
            cmd = new SqlCommand("delete from wf_task where wf_id='" + WFID + "' and step_no='" + StepNo + "'", con);
            cmd.ExecuteNonQuery();
            ds_V01.Reset();
            cmd = new SqlCommand("select * from wf_task where wf_id='" + WFID + "' and step_no>'" + StepNo + "' order by step_no", con);
            adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            if (ds_V01.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds_V01.Tables[0].Rows.Count; i++)
                {
                    cmd = new SqlCommand("update wf_task set step_no='" + (Convert.ToInt32(ds_V01.Tables[0].Rows[i][1].ToString()) - 1).ToString() + "' where wf_id='" + WFID + "' and step_no='" + ds_V01.Tables[0].Rows[i][1].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                }
            }
            // For wf_cond table
            cmd = new SqlCommand("delete from wf_cond where wf_id='" + WFID + "' and step_no='" + StepNo + "'", con);
            cmd.ExecuteNonQuery();
            ds_V01.Reset();
            cmd = new SqlCommand("select * from wf_cond where wf_id='" + WFID + "' and step_no>'" + StepNo + "' order by step_no", con);
            adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            if (ds_V01.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds_V01.Tables[0].Rows.Count; i++)
                {
                    cmd = new SqlCommand("update wf_cond set step_no='" + (Convert.ToInt32(ds_V01.Tables[0].Rows[i][1].ToString()) - 1).ToString() + "' where wf_id='" + WFID + "' and step_no='" + ds_V01.Tables[0].Rows[i][1].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                }
            }
            DeleteStat = "1";

            Utility.CloseConnection(con);
            return DeleteStat;
        }

        public DataSet WFTasks(int WFID, int StepNo)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select a.wf_id,a.step_no,a.task_id,a.action_type,a.copy_to_uuid,a.amble_mails,a.amble_msg,a.amble_attach,a.AppendDocUUID,a.amble_url,a.AmbleSub,b.task_name from wf_task a,task_mast b where a.task_id=b.task_id and a.wf_id='" + WFID + "' and a.step_no='" + StepNo + "'", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public string DeleteWFTasks(int WFID, int StepNo, string TaskID)
        {
            string DeleteStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("delete from wf_task where wf_id='" + WFID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("delete from wf_cond where wf_id='" + WFID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
            cmd.ExecuteNonQuery();

            DeleteStat = "1";

            Utility.CloseConnection(con);
            return DeleteStat;
        }

        public DataSet WFTaskDetails(int WFID, int StepNo, string TaskID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select a.wf_id,a.step_no,a.task_id,a.action_type,a.copy_to_uuid,a.amble_mails,a.amble_msg,a.amble_attach,a.AppendDocUUID,a.amble_url,a.AmbleSub,b.task_name from wf_task a,task_mast b where a.task_id=b.task_id and a.wf_id='" + WFID + "' and a.step_no='" + StepNo + "' and a.task_id='" + TaskID + "'", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet WFCondDetails(int WFID, int StepNo, string TaskID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select a.wf_id,a.step_no,a.task_id,a.form_field_no,a.cond_op,a.cond_val,a.amble_mails,a.amble_msg,a.amble_attach,a.amble_url,a.AmbleSub,b.task_name from wf_cond a,task_mast b where a.task_id=b.task_id and a.wf_id='" + WFID + "' and a.step_no='" + StepNo + "' and a.task_id='" + TaskID + "'", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public string UpdateWFTasks(int WFID, int StepNo, string TaskID, string CopyToUUID, string AmbleMailID, string AmbleMsg, string AmbleAttach, string AppendedDocUUID, string AmbleURL, string AmbleSub)
        {
            string UpdateStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("update wf_task set copy_to_uuid='" + CopyToUUID + "',amble_mails='" + AmbleMailID + "',amble_msg='" + AmbleMsg + "',amble_attach='" + AmbleAttach + "',AppendDocUUID='" + AppendedDocUUID + "',amble_url='" + AmbleURL + "',AmbleSub='" + AmbleSub + "' where wf_id='" + WFID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
            cmd.ExecuteNonQuery();
            UpdateStat = "1";

            Utility.CloseConnection(con);
            return UpdateStat;
        }

        public string UpdateWFCond(int WFID, int StepNo, string TaskID, string FormFieldNo, string CondOp, string CondVal, string AmbleMailID, string AmbleMsg, string AmbleAttach, string AmbleURL, string AmbleSub)
        {
            string UpdateStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("update wf_cond set amble_mails='" + AmbleMailID + "',amble_msg='" + AmbleMsg + "',amble_attach='" + AmbleAttach + "',amble_url='" + AmbleURL + "',AmbleSub='" + AmbleSub + "' where wf_id='" + WFID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "' and form_field_no='" + FormFieldNo + "' and cond_op='" + CondOp + "' and cond_val='" + CondVal + "'", con);
            cmd.ExecuteNonQuery();
            UpdateStat = "1";

            Utility.CloseConnection(con);
            return UpdateStat;
        }

        public DataSet MaxStepNoInWF(string WFID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select max(step_no) from wf_dtl where wf_id='" + WFID + "'", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public DataSet TotalStepNoInWF(string WFID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();
            cmd = new SqlCommand("select * from wf_dtl where wf_id='" + WFID + "'", con);
            SqlDataAdapter adapter_V01 = new SqlDataAdapter(cmd);
            adapter_V01.Fill(ds_V01);
            Utility.CloseConnection(con);
            return ds_V01;
        }

        public string WFStageAlteration(int WFID, int StepNo)
        {
            string AlterStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds_V01 = new DataSet();

            ds_V01.Reset();
            ds_V01 = MaxStepNoInWF(WFID.ToString());
            int mMaxStepNo = Convert.ToInt32(ds_V01.Tables[0].Rows[0][0].ToString());
            if (StepNo <= mMaxStepNo)
            {
                for (int i = mMaxStepNo; i >= StepNo; i--)
                {
                    // wf_cond table
                    cmd = new SqlCommand("update wf_cond set step_no='" + (i + 1).ToString() + "' where wf_id='" + WFID + "' and step_no='" + i.ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    // wf_dtl table
                    cmd = new SqlCommand("update wf_dtl set step_no='" + (i + 1).ToString() + "' where wf_id='" + WFID + "' and step_no='" + i.ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    //wf_task table
                    cmd = new SqlCommand("update wf_task set step_no='" + (i + 1).ToString() + "' where wf_id='" + WFID + "' and step_no='" + i.ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                }
            }

            AlterStat = "1";

            Utility.CloseConnection(con);
            return AlterStat;
        }

        public string DeleteWFCond(int WFID, int StepNo, string TaskID, int FormFieldNo, string CondOp, string CondVal)
        {
            string DeleteStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("delete from wf_cond where wf_id='" + WFID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "' and form_field_no='" + FormFieldNo + "' and cond_op='" + CondOp + "' and cond_val='" + CondVal + "'", con);
            cmd.ExecuteNonQuery();

            DeleteStat = "1";

            Utility.CloseConnection(con);
            return DeleteStat;
        }

        public string ChngAppRvw(int WFID, int StepNo, string OldTaskID, string NewTaskID)
        {
            string UpdateStat = "0";
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("update wf_task set task_id='" + NewTaskID + "' where wf_id='" + WFID + "' and step_no='" + StepNo + "' and task_id='" + OldTaskID + "'", con);
            cmd.ExecuteNonQuery();
            UpdateStat = "1";

            Utility.CloseConnection(con);
            return UpdateStat;
        }
        #endregion

    }
}