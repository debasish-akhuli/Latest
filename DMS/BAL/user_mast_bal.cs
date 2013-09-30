using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using DMS.DAL;
using System.Text.RegularExpressions;

namespace DMS.BAL
{
    public class user_mast_bal
    {
        // Variable Declaration
        #region
        user_mast_dal OBJ_UserDAL;
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
                else
                {
                    string email = value;
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(email);
                    if (match.Success)
                    {

                    }
                    else
                    {
                        throw new Exception("Please enter valid email id");
                    }
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
        /// This function is used to update user's new password
        /// </summary>
        /// <returns></returns>
        public string UpdtUserMast()
        {
            OBJ_UserDAL = new user_mast_dal();
            OBJ_UserDAL.UserID = this._UserID;
            OBJ_UserDAL.UserPwd = this._UserPwd;
            OBJ_UserDAL.PwdStat = this._PwdStat;

            return OBJ_UserDAL.UpdtUserMast();
        }
        #endregion
    }
}