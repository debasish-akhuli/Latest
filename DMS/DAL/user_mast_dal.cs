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
    public class user_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _UserID = "";
        private string _FName = "";
        private string _LName = "";
        private string _EMail = "";
        private string _UserPwd = "";
        private string _Title = "";
        private string _Dept = "";
        private string _Stat = "";
        private string _PwdStat = "";
        private string _CanChangePwd = "";
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
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter user id");
                }
                _UserID = value;
            }
        }
        public String FName
        {
            get
            {
                return _FName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter first name");
                }
                _FName = value;
            }
        }
        public String LName
        {
            get
            {
                return _LName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter last name");
                }
                _LName = value;
            }
        }
        public String EMail
        {
            get
            {
                return _EMail;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter email id");
                }
                _EMail = value;
            }
        }
        public String UserPwd
        {
            get
            {
                return _UserPwd;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter password");
                }
                _UserPwd = value;
            }
        }
        public String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter title");
                }
                _Title = value;
            }
        }
        public String Dept
        {
            get
            {
                return _Dept;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select department");
                }
                _Dept = value;
            }
        }
        public String Stat
        {
            get
            {
                return _Stat;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select status");
                }
                _Stat = value;
            }
        }
        public String PwdStat
        {
            get
            {
                return _PwdStat;
            }
            set
            {
                _PwdStat = value;
            }
        }
        public String CanChangePwd
        {
            get
            {
                return _CanChangePwd;
            }
            set
            {
                _CanChangePwd = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        
        /// <summary>
        /// Update into the database --- Table Name:<user_mast> Field Name:<user_id><user_pwd>
        /// Store Procedure Name:<UserPwd_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdtUserMast()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("UserPwd_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@user_id"].Value = _UserID;

            cmd.Parameters.Add("@user_pwd", SqlDbType.NVarChar, 100);
            cmd.Parameters["@user_pwd"].Value = _UserPwd;

            cmd.Parameters.Add("@PwdStat", SqlDbType.NVarChar, 50);
            cmd.Parameters["@PwdStat"].Value = _PwdStat;

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