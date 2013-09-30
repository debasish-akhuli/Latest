using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using DMS.DAL;

namespace DMS.DAL
{
    public class doc_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _UserID = "";
        private string _WFLogID = "";
        private Int64 _DocID = 0;
        private Int64 _WFID = 0;
        private DateTime _Start_Dt = DateTime.Now;
        private DateTime _Due_Dt = DateTime.Now;
        private string _Duration = "";
        private int _StepNo = 0;
        private string _TaskID = "";
        private string _DocName = "";
        private string _DocDesc = "";
        private string _FolderCode = "";
        private string _DocTypeCode = "";
        private string _DeptCode = "";
        private string _Upld_By = "";
        private DateTime _Upld_Dt = DateTime.Now;
        public string _Tag1 = "";
        public string _Tag2 = "";
        public string _Tag3 = "";
        public string _Tag4 = "";
        public string _Tag5 = "";
        public string _Tag6 = "";
        public string _Tag7 = "";
        public string _Tag8 = "";
        public string _Tag9 = "";
        public string _Tag10 = "";
        public string _Download_Path = "";
        public string _Doc_Path = "";
        public string _UUID = "";
        public string _AmbleMails = "";
        public string _AmbleMsg = "";
        public string _AmbleAttach = "";
        public string _AppendDoc = "";
        public string _AmbleURL = "";
        public string _AmbleSub = "";
        #endregion

        //Property Declaration
        #region
        public String UserID
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
        public String WFLogID
        {
            get
            {
                return _WFLogID;
            }
            set
            {
                _WFLogID = value;
            }
        }
        public String Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
            }
        }
        public Int64 DocID
        {
            get
            {
                return _DocID;
            }
            set
            {
                _DocID = value;
            }
        }
        public String FolderCode
        {
            get
            {
                return _FolderCode;
            }
            set
            {
                _FolderCode = value;
            }
        }
        public Int64 WFID
        {
            get
            {
                return _WFID;
            }
            set
            {
                _WFID = value;
            }
        }
        public DateTime Start_Dt
        {
            get
            {
                return _Start_Dt;
            }
            set
            {
                _Start_Dt = value;
            }
        }
        public DateTime Due_Dt
        {
            get
            {
                return _Due_Dt;
            }
            set
            {
                _Due_Dt = value;
            }
        }
        public int StepNo
        {
            get
            {
                return _StepNo;
            }
            set
            {
                _StepNo = value;
            }
        }
        public string TaskID
        {
            get
            {
                return _TaskID;
            }
            set
            {
                _TaskID = value;
            }
        }
        public String DocName
        {
            get
            {
                return _DocName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter document name");
                }
                _DocName = value;
            }
        }
        public String DocDesc
        {
            get
            {
                return _DocDesc;
            }
            set
            {
                _DocDesc = value;
            }
        }
        public String DocTypeCode
        {
            get
            {
                return _DocTypeCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select document type");
                }
                _DocTypeCode = value;
            }
        }
        public String DeptCode
        {
            get
            {
                return _DeptCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select department");
                }
                _DeptCode = value;
            }
        }
        public String Upld_By
        {
            get
            {
                return _Upld_By;
            }
            set
            {
                _Upld_By = value;
            }
        }
        public DateTime Upld_Dt
        {
            get
            {
                return _Upld_Dt;
            }
            set
            {
                _Upld_Dt = value;
            }
        }
        public String Tag1
        {
            get
            {
                return _Tag1;
            }
            set
            {
                _Tag1 = value;
            }
        }
        public String Tag2
        {
            get
            {
                return _Tag2;
            }
            set
            {
                _Tag2 = value;
            }
        }
        public String Tag3
        {
            get
            {
                return _Tag3;
            }
            set
            {
                _Tag3 = value;
            }
        }
        public String Tag4
        {
            get
            {
                return _Tag4;
            }
            set
            {
                _Tag4 = value;
            }
        }
        public String Tag5
        {
            get
            {
                return _Tag5;
            }
            set
            {
                _Tag5 = value;
            }
        }
        public String Tag6
        {
            get
            {
                return _Tag6;
            }
            set
            {
                _Tag6 = value;
            }
        }
        public String Tag7
        {
            get
            {
                return _Tag7;
            }
            set
            {
                _Tag7 = value;
            }
        }
        public String Tag8
        {
            get
            {
                return _Tag8;
            }
            set
            {
                _Tag8 = value;
            }
        }
        public String Tag9
        {
            get
            {
                return _Tag9;
            }
            set
            {
                _Tag9 = value;
            }
        }
        public String Tag10
        {
            get
            {
                return _Tag10;
            }
            set
            {
                _Tag10 = value;
            }
        }
        public String Download_Path
        {
            get
            {
                return _Download_Path;
            }
            set
            {
                _Download_Path = value;
            }
        }
        public String Doc_Path
        {
            get
            {
                return _Doc_Path;
            }
            set
            {
                _Doc_Path = value;
            }
        }
        public String UUID
        {
            get
            {
                return _UUID;
            }
            set
            {
                _UUID = value;
            }
        }
        public String AmbleMails
        {
            get
            {
                return _AmbleMails;
            }
            set
            {
                _AmbleMails = value;
            }
        }
        public String AmbleMsg
        {
            get
            {
                return _AmbleMsg;
            }
            set
            {
                _AmbleMsg = value;
            }
        }
        public String AmbleAttach
        {
            get
            {
                return _AmbleAttach;
            }
            set
            {
                _AmbleAttach = value;
            }
        }
        public String AmbleURL
        {
            get
            {
                return _AmbleURL;
            }
            set
            {
                _AmbleURL = value;
            }
        }
        public String AmbleSub
        {
            get
            {
                return _AmbleSub;
            }
            set
            {
                _AmbleSub = value;
            }
        }
        public String AppendDoc
        {
            get
            {
                return _AppendDoc;
            }
            set
            {
                _AppendDoc = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        public string ExistDoc()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("Doc_Exists", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_name", SqlDbType.Text);
            cmd.Parameters["@doc_name"].Value = _DocName;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar,255);
            cmd.Parameters["@fld_uuid"].Value = _FolderCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }


        public string ExistDoc4Sniffer()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("Doc_Exists4Sniffer", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_name", SqlDbType.Text);
            cmd.Parameters["@doc_name"].Value = _DocName;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        //public string InsertDocMast()
        //{
        //    Download_Path = "http://127.0.0.1:8080/share/proxy/alfresco/api/node/content/workspace/SpacesStore/"+ _Doc_Path + "?a=true";
        //    SqlConnection con = Utility.GetConnection();
        //    cmd = new SqlCommand("DocMast_Insert", con);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.Add("@doc_name", SqlDbType.Text);
        //    cmd.Parameters["@doc_name"].Value = _DocName;

        //    cmd.Parameters.Add("@doc_desc", SqlDbType.Text);
        //    cmd.Parameters["@doc_desc"].Value = _DocDesc;

        //    cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar,255);
        //    cmd.Parameters["@fld_uuid"].Value =_FolderCode;

        //    cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar,10);
        //    cmd.Parameters["@doc_type_id"].Value = _DocTypeCode;

        //    cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
        //    cmd.Parameters["@dept_id"].Value = _DeptCode;

        //    cmd.Parameters.Add("@upld_by", SqlDbType.NVarChar, 20);
        //    cmd.Parameters["@upld_by"].Value = _Upld_By;

        //    cmd.Parameters.Add("@upld_dt", SqlDbType.SmallDateTime);
        //    cmd.Parameters["@upld_dt"].Value = _Upld_Dt;

        //    cmd.Parameters.Add("@tag1", SqlDbType.Text);
        //    cmd.Parameters["@tag1"].Value = _Tag1;

        //    cmd.Parameters.Add("@tag2", SqlDbType.Text);
        //    cmd.Parameters["@tag2"].Value = _Tag2;

        //    cmd.Parameters.Add("@tag3", SqlDbType.Text);
        //    cmd.Parameters["@tag3"].Value = _Tag3;

        //    cmd.Parameters.Add("@tag4", SqlDbType.Text);
        //    cmd.Parameters["@tag4"].Value = _Tag4;

        //    cmd.Parameters.Add("@tag5", SqlDbType.Text);
        //    cmd.Parameters["@tag5"].Value = _Tag5;

        //    cmd.Parameters.Add("@tag6", SqlDbType.Text);
        //    cmd.Parameters["@tag6"].Value = _Tag6;

        //    cmd.Parameters.Add("@tag7", SqlDbType.Text);
        //    cmd.Parameters["@tag7"].Value = _Tag7;

        //    cmd.Parameters.Add("@tag8", SqlDbType.Text);
        //    cmd.Parameters["@tag8"].Value = _Tag8;

        //    cmd.Parameters.Add("@tag9", SqlDbType.Text);
        //    cmd.Parameters["@tag9"].Value = _Tag9;

        //    cmd.Parameters.Add("@tag10", SqlDbType.Text);
        //    cmd.Parameters["@tag10"].Value = _Tag10;

        //    cmd.Parameters.Add("@Download_Path", SqlDbType.Text);
        //    cmd.Parameters["@Download_Path"].Value = _Download_Path;

        //    cmd.Parameters.Add("@uuid", SqlDbType.NVarChar,255);
        //    cmd.Parameters["@uuid"].Value = _UUID;

        //    SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
        //    param.Direction = ParameterDirection.Output;

        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    Utility.CloseConnection(con);

        //    return param.Value.ToString();
        //}

        /// <summary>
        /// Fetch the corresponding tags for the selected doc type using Store Procedure Name:<Tag_Select>
        /// </summary>
        /// <returns></returns>
        public DataSet FetchTags()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Tag_Select", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar,10);
            cmd.Parameters["@doc_type_id"].Value = _DocTypeCode;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Fetch the defined default workflow using Store Procedure Name:<Select_DefaultWF>
        /// </summary>
        /// <returns></returns>
        public string DefinedWF()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("Select_DefaultWF", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = _DocTypeCode;

            cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
            cmd.Parameters["@dept_id"].Value = _DeptCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// Start the defined default workflow using Store Procedure Name:<Start_DefaultWF>
        /// </summary>
        /// <returns></returns>
        public string StartDefaultWF()
        {
            /// Populate the Workflow ID in customized format (XXXXXXXXXX/XXXXX/XX/XXXX/XXXXX) and store it in <_WFLogID> variable
            _WFLogID=PopulateWFID(_DocTypeCode,_DeptCode,_Start_Dt);

            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("Start_DefaultWFLogMast", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_log_id", SqlDbType.NVarChar,30);
            cmd.Parameters["@wf_log_id"].Value = _WFLogID;

            cmd.Parameters.Add("@doc_id", SqlDbType.BigInt);
            cmd.Parameters["@doc_id"].Value = _DocID;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = _WFID;

            cmd.Parameters.Add("@start_dt", SqlDbType.SmallDateTime);
            cmd.Parameters["@start_dt"].Value = _Start_Dt;

            cmd.Parameters.Add("@user_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@user_id"].Value = _UserID;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.NVarChar,30);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// Function to generate ID in a customized format (XXXXXXXXXX/XXXXX/XX/XXXX/XXXXX)
        /// </summary>
        /// <returns></returns>
        protected string PopulateWFID(string mDocTypeCode, string mDeptCode, DateTime mStartDt)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;

            string mMonth = (mStartDt.Month.ToString().PadLeft(2,'0'));
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

        /// <summary>
        /// Fetch the corresponding roles for the workflow using Store Procedure Name:<Roles_Select>
        /// </summary>
        /// <returns></returns>
        public DataSet SelectWFDtls()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Roles_Select", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = _WFID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Fetch the corresponding tasks for the workflow using Store Procedure Name:<Tasks_Select>
        /// </summary>
        /// <returns></returns>
        public DataSet SelectWFTasks()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Tasks_Select", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = _WFID;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Start the defined default workflow's Roles using Store Procedure Name:<Start_DefaultWFLogDtl>
        /// </summary>
        /// <returns></returns>
        public string StartDefaultWFLogDtl()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("Start_DefaultWFLogDtl", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_log_id", SqlDbType.NVarChar, 30);
            cmd.Parameters["@wf_log_id"].Value = _WFLogID;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = _StepNo;

            cmd.Parameters.Add("@assign_dt", SqlDbType.SmallDateTime);
            cmd.Parameters["@assign_dt"].Value = _Start_Dt;

            cmd.Parameters.Add("@due_dt", SqlDbType.SmallDateTime);
            cmd.Parameters["@due_dt"].Value = _Due_Dt;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.NVarChar, 30);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// Start the defined default workflow's Tasks using Store Procedure Name:<Start_DefaultWFLogTask>
        /// </summary>
        /// <returns></returns>
        public string StartDefaultWFLogTask()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("Start_DefaultWFLogTask", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_log_id", SqlDbType.NVarChar, 30);
            cmd.Parameters["@wf_log_id"].Value = _WFLogID;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = _StepNo;

            cmd.Parameters.Add("@task_id", SqlDbType.NVarChar,10);
            cmd.Parameters["@task_id"].Value = _TaskID;

            cmd.Parameters.Add("@AmbleMails", SqlDbType.NVarChar,255);
            cmd.Parameters["@AmbleMails"].Value = _AmbleMails;

            cmd.Parameters.Add("@AmbleMsg", SqlDbType.Text);
            cmd.Parameters["@AmbleMsg"].Value = _AmbleMsg;

            cmd.Parameters.Add("@AmbleAttach", SqlDbType.NVarChar, 20);
            cmd.Parameters["@AmbleAttach"].Value = _AmbleAttach;

            cmd.Parameters.Add("@AppendDoc", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AppendDoc"].Value = _AppendDoc;

            cmd.Parameters.Add("@AmbleURL", SqlDbType.NVarChar, 20);
            cmd.Parameters["@AmbleURL"].Value = _AmbleURL;

            cmd.Parameters.Add("@AmbleSub", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AmbleSub"].Value = _AmbleSub;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.NVarChar, 30);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }
        #endregion
    }
}