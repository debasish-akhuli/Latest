using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using DMS.UTILITY;

/// In this Class File, all the functions for generating the Dropdowns and Gridviews are defined

namespace DMS
{
    public class DBClass
    {
        // Variable Declaration
        #region
        private string _CabID = "";
        private string _DrwID = "";
        private string _FldID = "";
        private string _WF_Log_ID = "";
        private Int32 _Step_No = 0;
        private string _UserID = "";
        private string _DeptID = "";
        private string _DocTypeID = "";
        private string _EmailID = "";
        #endregion

        //Property Declaration
        #region
        public string CabID
        {
            get
            {
                return _CabID;
            }
            set
            {
                _CabID = value;
            }
        }
        public string DrwID
        {
            get
            {
                return _DrwID;
            }
            set
            {
                _DrwID = value;
            }
        }
        public string FldID
        {
            get
            {
                return _FldID;
            }
            set
            {
                _FldID = value;
            }
        }
        public string WF_Log_ID
        {
            get
            {
                return _WF_Log_ID;
            }
            set
            {
                _WF_Log_ID = value;
            }
        }
        public Int32 Step_No
        {
            get
            {
                return _Step_No;
            }
            set
            {
                _Step_No = value;
            }
        }
        public string UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }
        public string DeptID
        {
            get
            {
                return _DeptID;
            }
            set
            {
                _DeptID = value;
            }
        }
        public string DocTypeID
        {
            get
            {
                return _DocTypeID;
            }
            set
            {
                _DocTypeID = value;
            }
        }
        public string EmailID
        {
            get
            {
                return _EmailID;
            }
            set
            {
                _EmailID = value;
            }
        }
        #endregion

        /// <summary>
        /// Dropdown to populate Role Master using <Role_DD> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet DDRole()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("Role_DD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// To populate Mail Setup record using <MailSetup_Select> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet MailSetup()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("MailSetup_Select", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }
        
        /// <summary>
        /// Dropdown to populate the active workflows using <WF_DD> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet DDWorkflow(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("sp_WF_DD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Dropdown to populate Task Master using <Task_DD> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet DDTask()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Task_DD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Dropdown to populate Task Master using <AssignedTask_DD> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet DDAssignedTask()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("AssignedTask_DD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_log_id", SqlDbType.NVarChar,30);
            cmd.Parameters["@wf_log_id"].Value = _WF_Log_ID;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = _Step_No;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Gridview to populate Department Master using <DeptMast_GV> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet GVDept()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("DeptMast_GV", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Gridview to populate Role Master using <RoleMast_GV> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet GVRole()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("RoleMast_GV", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Gridview to populate User wise Role Mapping Master using <UserRoleMast_GV> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet GVUserRole()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("UserRoleMast_GV", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet GVAssociatedRole(String RoleID, string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("GVAssociatedRole", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@RoleID"].Value = RoleID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Gridview to populate Group Master using <GrpMast_GV> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet GVGroup()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("GroupMast_GV", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Gridview to populate Drawer Master using <DrawerMast_GV> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet GVDrawer(string CompCode, string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("DrawerMast_GV", con);
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

        /// <summary>
        /// Dropdown to populate Document Master using <DocumentDD> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet DDDocument()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("DocumentDD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_uuid"].Value = _FldID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet DDDocPDFOnly()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("DocPDFOnly", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_uuid"].Value = _FldID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Gridview to populate Folder Master using <FolderMast_GV> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet GVFolder(string CompCode, string UserID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("FolderMast_GV", con);
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
        
        /// <summary>
        /// Dropdown to populate Group Master using <Group_DD> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet DDGroup()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Group_DD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Gridview to populate Workflow Master using <WorkflowMast_GV> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet GVWFMast()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("WorkflowMast_GV", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// To display the Workflow in details
        /// </summary>
        /// <param name="lbWfid1"></param>
        /// <returns></returns>
        public DataSet DisplayWFDtl(string lbWfid1,string CompCode)
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

        /// <summary>
        /// Clear incomplete workflows using <WF_Clear> Store Procedure
        /// </summary>
        /// <returns></returns>
        public string ClearIncompleteWF()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("WF_Clear", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// Fetch from Document Master using <Document_Select> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet Document_Select()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Document_Select", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
            cmd.Parameters["@UserID"].Value = _UserID;

            cmd.Parameters.Add("@FldUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FldUUID"].Value = _FldID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// This function is for detecting the uploaded location for the document
        /// </summary>
        /// <param name="DeptID"></param>
        /// <param name="DocTypeID"></param>
        /// <returns></returns>
        public DataSet FetchUploadedLocation(string DeptID, string DocTypeID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("FetchUploadedLocation", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DeptID", SqlDbType.NVarChar, 5);
            cmd.Parameters["@DeptID"].Value = DeptID;

            cmd.Parameters.Add("@DocTypeID", SqlDbType.NVarChar, 10);
            cmd.Parameters["@DocTypeID"].Value = DocTypeID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        public DataSet SetUploadedLocation(string FldUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("SetUploadedLocation", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FldUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@FldUUID"].Value = FldUUID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// This function is used for inserting the meta values
        /// </summary>
        /// <param name="DocUUID"></param>
        /// <returns></returns>
        public string DocMetaValueInsert(string DocUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("DocMetaValueInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DocUUID"].Value = DocUUID;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// This function is used for updating the meta values
        /// </summary>
        /// <param name="DocUUID"></param>
        /// <param name="DBFieldName"></param>
        /// <param name="FieldValue"></param>
        /// <returns></returns>
        public string DocMetaValueUpdate(string DocUUID, string DBFieldName, string FieldValue)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("DocMetaValueUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DocUUID", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DocUUID"].Value = DocUUID;

            cmd.Parameters.Add("@DBFieldName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DBFieldName"].Value = DBFieldName;

            cmd.Parameters.Add("@FieldValue", SqlDbType.Text);
            cmd.Parameters["@FieldValue"].Value = FieldValue;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// This function is used to select all the workflow tasks to be performed for a particular stage
        /// </summary>
        /// <param name="WFLogID"></param>
        /// <param name="StepNo"></param>
        /// <returns></returns>
        public DataSet WFLogTaskSelect4WF(string WFLogID, string StepNo)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("WFLogTaskSelect4WF", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar, 30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            cmd.Parameters.Add("@StepNo", SqlDbType.Int);
            cmd.Parameters["@StepNo"].Value = StepNo;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// This function is used to update the workflow task which is performed for a particular stage
        /// </summary>
        /// <param name="TaskDoneDate"></param>
        /// <param name="Comments"></param>
        /// <param name="WFLogID"></param>
        /// <param name="StepNo"></param>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public string WFLogTaskUpdate4WF(string TaskDoneDate, string Comments, string WFLogID, string StepNo, string TaskID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("WFLogTaskUpdate4WF", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@TaskDoneDate", SqlDbType.DateTime);
            cmd.Parameters["@TaskDoneDate"].Value = TaskDoneDate;

            cmd.Parameters.Add("@Comments", SqlDbType.NVarChar, 255);
            cmd.Parameters["@Comments"].Value = Comments;

            cmd.Parameters.Add("@WFLogID", SqlDbType.NVarChar,30);
            cmd.Parameters["@WFLogID"].Value = WFLogID;

            cmd.Parameters.Add("@StepNo", SqlDbType.Int);
            cmd.Parameters["@StepNo"].Value = StepNo;

            cmd.Parameters.Add("@TaskID", SqlDbType.NVarChar, 10);
            cmd.Parameters["@TaskID"].Value = TaskID;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }




    }
}