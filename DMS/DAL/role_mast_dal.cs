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
    public class role_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _RoleCode = "";
        private string _RoleName = "";
        #endregion

        //Property Declaration
        #region
        public String RoleCode
        {
            get
            {
                return _RoleCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter role code");
                }
                _RoleCode = value;
            }
        }
        public String RoleName
        {
            get
            {
                return _RoleName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter role name");
                }
                _RoleName = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// Insert into the database --- Table Name:<role_mast> Field Name:<role_id><role_name> Store Procedure Name:<RoleMast_Insert>
        /// And also in this procedure, there is another checking for the data is already exists or not.
        /// </summary>
        /// <returns></returns>
        public string InsertRoleMast()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("RoleMast_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@role_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@role_id"].Value = _RoleCode;

            cmd.Parameters.Add("@role_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@role_name"].Value = _RoleName;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To update <role_mast> in gridview using stored procedure <RoleMast_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateRole(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("RoleMast_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@role_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@role_id"].Value = _RoleCode;

            cmd.Parameters.Add("@role_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@role_name"].Value = _RoleName;

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
        /// To delete <role_mast> in gridview using stored procedure <RoleMast_Del>
        /// </summary>
        /// <returns></returns>
        public string Deleterole(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("RoleMast_Del", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@role_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@role_id"].Value = _RoleCode;

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