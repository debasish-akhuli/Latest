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
    public class cabinet_mast_bal
    {
        #region Variable Declaration
        cabinet_mast_dal OBJ_CabinetDAL;
        private string _CabinetCode = "";
        private string _CabinetName = "";
        private string _CabinetDesc = "";
        private string _UUID = "";
        private string _DefaultPermission = "";
        #endregion
                
        #region Property Declaration
        public string CabinetCode
        {
            get
            {
                return _CabinetCode;
            }
            set
            {
                _CabinetCode = value;
            }
        }
        public String CabinetName
        {
            get
            {
                return _CabinetName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter cabinet name");
                }
                _CabinetName = value;
            }
        }
        public String UUID
        {
            get
            {
                return _UUID;
            }
            set
            {
                _UUID = value;
            }
        }
        public String DefaultPermission
        {
            get
            {
                return _DefaultPermission;
            }
            set
            {
                _DefaultPermission = value;
            }
        }
        public String CabinetDesc
        {
            get
            {
                return _CabinetDesc;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter cabinet description");
                }
                _CabinetDesc = value;
            }
        }
        #endregion

        #region Method Declaration
        /// <summary>
        /// This function is used to insert record into the database.
        /// For this, the <CabinetName> & <CabinetDesc> are set in the Data Access Layer (DAL) of <cabinet_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string InsertCabinetMast()
        {
            OBJ_CabinetDAL = new cabinet_mast_dal();
            OBJ_CabinetDAL.CabinetName = this._CabinetName;
            OBJ_CabinetDAL.CabinetDesc = this._CabinetDesc;
            OBJ_CabinetDAL.UUID = this._UUID;
            OBJ_CabinetDAL.DefaultPermission = this._DefaultPermission;

            return OBJ_CabinetDAL.InsertCabinetMast();
        }

        /// <summary>
        /// To update <cabinet_mast> in gridview using stored procedure, pass the values to <cabinet_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateCabinet()
        {
            OBJ_CabinetDAL = new cabinet_mast_dal();
            OBJ_CabinetDAL.CabinetCode = this._CabinetCode;
            OBJ_CabinetDAL.CabinetDesc = this._CabinetDesc;

            return OBJ_CabinetDAL.UpdateCabinet();
        }

        /// <summary>
        /// To delete <cabinet_mast> in gridview using stored procedure, pass the values to <cabinet_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string DeleteCabinet()
        {
            OBJ_CabinetDAL = new cabinet_mast_dal();
            OBJ_CabinetDAL.CabinetCode = this._CabinetCode;

            return OBJ_CabinetDAL.DeleteCabinet();
        }
        #endregion
    }
}