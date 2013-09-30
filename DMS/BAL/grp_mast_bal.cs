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
    public class grp_mast_bal
    {
        // Variable Declaration
        #region
        grp_mast_dal OBJ_GrpDAL;
        private Int64 _GrpCode = 0;
        private string _GrpName = "";
        #endregion

        //Property Declaration
        #region
        public Int64 GrpCode
        {
            get
            {
                return _GrpCode;
            }
            set
            {
                _GrpCode = value;
            }
        }
        public String GrpName
        {
            get
            {
                return _GrpName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter group name");
                }
                _GrpName = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// This function is used to insert record into the database.
        /// For this, the <GrpName> is set in the Data Access Layer (DAL) of <grp_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string InsertGrpMast()
        {
            OBJ_GrpDAL = new grp_mast_dal();
            OBJ_GrpDAL.GrpName = this._GrpName;

            return OBJ_GrpDAL.InsertGrpMast();
        }

        /// <summary>
        /// To update <grp_mast> in gridview using stored procedure, pass the values to <grp_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateGrp()
        {
            OBJ_GrpDAL = new grp_mast_dal();
            OBJ_GrpDAL.GrpCode = this._GrpCode;
            OBJ_GrpDAL.GrpName = this._GrpName;

            return OBJ_GrpDAL.UpdateGrp();
        }

        /// <summary>
        /// To delete <grp_mast> in gridview using stored procedure, pass the values to <grp_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string DeleteGrp()
        {
            OBJ_GrpDAL = new grp_mast_dal();
            OBJ_GrpDAL.GrpCode = this._GrpCode;

            return OBJ_GrpDAL.DeleteGrp();
        }
        #endregion
    }
}