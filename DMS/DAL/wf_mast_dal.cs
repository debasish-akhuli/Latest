using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DMS.UTILITY;
using System.Data;

namespace DMS.DAL
{
    public class wf_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private Int64 _WFID = 0;
        private string _WFName = "";
        private string _WFStat = "A";
        private string _WFDept = "";
        private string _WFDocType = "";
        private string _WFFolderUUID = "";

        #endregion

        //Property Declaration
        #region
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
        public String WFName
        {
            get
            {
                return _WFName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter workflow name");
                }
                _WFName = value;
            }
        }

        public String WFDept
        {
            get
            {
                return _WFDept;
            }
            set
            {
                _WFDept = value;
            }
        }

        public String WFDocType
        {
            get
            {
                return _WFDocType;
            }
            set
            {
                _WFDocType = value;
            }
        }

        public string WFStat
        {
            get
            {
                return _WFStat;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter workflow status");
                }
                _WFStat = value;
            }
        }
        public String WFFolderUUID
        {
            get
            {
                return _WFFolderUUID;
            }
            set
            {
                _WFFolderUUID = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// Insert into the database --- Table Name:<wf_mast> Field Name:<wf_name><dept_id><doc_type_id><wf_stat> Store Procedure Name:<WorkflowMast_Insert>
        /// And also in this procedure, there is another checking for the data is already exists or not.
        /// </summary>
        /// <returns></returns>
        public string InsertWFMast()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("WorkflowMast_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.Add("@wf_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@wf_name"].Value = _WFName;

            cmd.Parameters.Add("@wf_dept", SqlDbType.NVarChar, 5);
            cmd.Parameters["@wf_dept"].Value = _WFDept;

            cmd.Parameters.Add("@wf_doctype", SqlDbType.NVarChar, 10);
            cmd.Parameters["@wf_doctype"].Value = _WFDocType;

            cmd.Parameters.Add("@wf_stat", SqlDbType.NVarChar, 1);
            cmd.Parameters["@wf_stat"].Value = _WFStat;

            cmd.Parameters.Add("@wf_folder_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@wf_folder_uuid"].Value = _WFFolderUUID;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To update the status to inactive --- Table Name:<wf_mast> Field Name:<wf_id> Store Procedure Name:<WorkflowMast_Delete>
        /// </summary>
        /// <returns></returns>
        public string DeleteWF()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("WorkflowMast_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = _WFID;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }
        #endregion
    }
}