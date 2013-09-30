using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS.DAL
{
    public class dept_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _DeptCode = "";
        private string _DeptName = "";
        #endregion

        //Property Declaration
        #region
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
                    throw new Exception("Please enter department code");
                }
                _DeptCode = value;
            }
        }
        public String DeptName
        {
            get
            {
                return _DeptName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter department name");
                }
                _DeptName = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// Insert into the database --- Table Name:<dept_mast> Field Name:<dept_id><dept_name> Store Procedure Name:<DeptMast_Insert>
        /// And also in this procedure, there is another checking for the data is already exists or not.
        /// </summary>
        /// <returns></returns>
        public string InsertDeptMast()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("DeptMast_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
            cmd.Parameters["@dept_id"].Value = _DeptCode;

            cmd.Parameters.Add("@dept_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@dept_name"].Value = _DeptName;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To update <dept_mast> in gridview using stored procedure <DeptMast_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateDept(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("DeptMast_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
            cmd.Parameters["@dept_id"].Value = _DeptCode;

            cmd.Parameters.Add("@dept_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@dept_name"].Value = _DeptName;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To delete <dept_mast> in gridview using stored procedure <DeptMast_Del>
        /// </summary>
        /// <returns></returns>
        public string DeleteDept(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("DeptMast_Del", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@dept_id", SqlDbType.NVarChar, 5);
            cmd.Parameters["@dept_id"].Value = _DeptCode;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

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