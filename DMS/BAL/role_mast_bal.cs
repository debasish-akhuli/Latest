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
    public class role_mast_bal
    {
        // Variable Declaration
        #region
        role_mast_dal OBJ_RoleDAL;
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
        /// This function is used to insert record into the database.
        /// For this, the <RoleCode> & <RoleName> are set in the Data Access Layer (DAL) of <role_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string InsertRoleMast()
        {
            OBJ_RoleDAL = new role_mast_dal();
            OBJ_RoleDAL.RoleCode = this._RoleCode;
            OBJ_RoleDAL.RoleName = this._RoleName;

            return OBJ_RoleDAL.InsertRoleMast();
        }

        /// <summary>
        /// To update <role_mast> in gridview using stored procedure, pass the values to <role_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateRole(string CompCode)
        {
            OBJ_RoleDAL = new role_mast_dal();
            OBJ_RoleDAL.RoleCode = this._RoleCode;
            OBJ_RoleDAL.RoleName = this._RoleName;

            return OBJ_RoleDAL.UpdateRole(CompCode);
        }

        /// <summary>
        /// To delete <role_mast> in gridview using stored procedure, pass the values to <role_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string Deleterole(string CompCode)
        {
            OBJ_RoleDAL = new role_mast_dal();
            OBJ_RoleDAL.RoleCode = this._RoleCode;

            return OBJ_RoleDAL.Deleterole(CompCode);
        }
        #endregion
    }
}