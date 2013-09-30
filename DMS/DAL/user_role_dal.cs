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
    public class user_role_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        string strSQL = "";
        private string _UserCode = "";
        private string _RoleCode = "";
        private string _Labelid = "";
        #endregion

        //Property Declaration
        #region
        public String UserCode
        {
            get
            {
                return _UserCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select user");
                }
                _UserCode = value;
            }
        }
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
                    throw new Exception("Please select role");
                }
                _RoleCode = value;
            }
        }
        public String Labelid
        {
            get
            {
                return _Labelid;
            }
            set
            {
                _Labelid = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// Insert into the database --- Table Name:<user_role> Field Name:<user_id><role_id> Store Procedure Name:<UserRoleMast_Insert>
        /// And also in this procedure, there is another checking for the data is already exists or not.
        /// </summary>
        /// <returns></returns>
        public string InsertUserRoleMast()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("UserRoleMast_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@user_id"].Value = _UserCode;
            
            cmd.Parameters.Add("@role_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@role_id"].Value = _RoleCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To update <user_role> in gridview using stored procedure <UserRoleMast_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateUserRole()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("UserRoleMast_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user_id", SqlDbType.NVarChar, 255);
            cmd.Parameters["@user_id"].Value = _UserCode;

            cmd.Parameters.Add("@role_id", SqlDbType.NText);
            cmd.Parameters["@role_id"].Value = _RoleCode;

            cmd.Parameters.Add("@lbl_id", SqlDbType.NVarChar,20);
            cmd.Parameters["@lbl_id"].Value = _Labelid;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To delete <user_role> in gridview using stored procedure <UserRoleMast_Del>
        /// </summary>
        /// <returns></returns>
        public string DeleteUserRole()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("UserRoleMast_Del", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@lbl_id", SqlDbType.NVarChar,20);
            cmd.Parameters["@lbl_id"].Value = _Labelid;

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