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
    public class dept_mast_bal
    {
        // Variable Declaration
        #region
        dept_mast_dal OBJ_DeptDAL;
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
        /// This function is used to insert record into the database.
        /// For this, the <DeptCode> & <DeptName> are set in the Data Access Layer (DAL) of <dept_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string InsertDeptMast()
        {
            OBJ_DeptDAL = new dept_mast_dal();
            OBJ_DeptDAL.DeptCode = this._DeptCode;
            OBJ_DeptDAL.DeptName = this._DeptName;

            return OBJ_DeptDAL.InsertDeptMast();
        }

        /// <summary>
        /// To update <dept_mast> in gridview using stored procedure, pass the values to <dept_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateDept(string CompCode)
        {
            OBJ_DeptDAL = new dept_mast_dal();
            OBJ_DeptDAL.DeptCode = this._DeptCode;
            OBJ_DeptDAL.DeptName = this._DeptName;

            return OBJ_DeptDAL.UpdateDept(CompCode);
        }

        /// <summary>
        /// To delete <dept_mast> in gridview using stored procedure, pass the values to <dept_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string DeleteDept(string CompCode)
        {
            OBJ_DeptDAL = new dept_mast_dal();
            OBJ_DeptDAL.DeptCode = this._DeptCode;

            return OBJ_DeptDAL.DeleteDept(CompCode);
        }
        #endregion
    }
}