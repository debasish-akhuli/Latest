using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using DMS.DAL;

namespace DMS.BAL
{
    public class user_role_bal
    {
        // Variable Declaration
        #region
        user_role_dal OBJ_UserRoleDAL;
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
        /// This function is used to insert record into the database.
        /// For this, the <UserCode> & <RoleCode> are set in the Data Access Layer (DAL) of <user_role_dal>
        /// </summary>
        /// <returns></returns>
        public string InsertUserRoleMast()
        {
            OBJ_UserRoleDAL = new user_role_dal();
            OBJ_UserRoleDAL.UserCode = this._UserCode;
            OBJ_UserRoleDAL.RoleCode = this._RoleCode;

            return OBJ_UserRoleDAL.InsertUserRoleMast();
        }

        /// <summary>
        /// To update <user_role> in gridview using stored procedure, pass the values to <user_role_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateUserRole()
        {
            OBJ_UserRoleDAL = new user_role_dal();
            OBJ_UserRoleDAL.UserCode = this._UserCode;
            OBJ_UserRoleDAL.RoleCode = this._RoleCode;
            OBJ_UserRoleDAL.Labelid = this._Labelid;

            return OBJ_UserRoleDAL.UpdateUserRole();
        }

        /// <summary>
        /// To delete <user_role> in gridview using stored procedure, pass the values to <user_role_dal>
        /// </summary>
        /// <returns></returns>
        public string DeleteUserRole()
        {
            OBJ_UserRoleDAL = new user_role_dal();
            OBJ_UserRoleDAL.Labelid = this._Labelid;

            return OBJ_UserRoleDAL.DeleteUserRole();
        }
        #endregion
    }
}