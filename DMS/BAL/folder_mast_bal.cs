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
    public class folder_mast_bal
    {
        // Variable Declaration
        #region
        folder_mast_dal OBJ_FolderDAL;
        private string _FolderName = "";
        private string _FolderDesc = "";
        private string _CabinetCode = "";
        private string _DrawerCode = "";
        private string _Labelid = "";
        private string _UUID = "";
        #endregion

        //Property Declaration
        #region
        public String FolderName
        {
            get
            {
                return _FolderName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter folder name");
                }
                _FolderName = value;
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
        public String FolderDesc
        {
            get
            {
                return _FolderDesc;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter folder description");
                }
                _FolderDesc = value;
            }
        }
        public String CabinetCode
        {
            get
            {
                return _CabinetCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select a cabinet");
                }
                _CabinetCode = value;
            }
        }
        public String DrawerCode
        {
            get
            {
                return _DrawerCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select a drawer");
                }
                _DrawerCode = value;
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
        /// To update <folder_mast> in gridview using stored procedure, pass the values to <folder_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateFolder()
        {
            OBJ_FolderDAL = new folder_mast_dal();
            OBJ_FolderDAL.FolderDesc = this._FolderDesc;
            OBJ_FolderDAL.Labelid = this._Labelid;

            return OBJ_FolderDAL.UpdateFolder();
        }

        /// <summary>
        /// To delete <folder_mast> in gridview using stored procedure, pass the values to <folder_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string DeleteFolder()
        {
            OBJ_FolderDAL = new folder_mast_dal();
            OBJ_FolderDAL.Labelid = this._Labelid;

            return OBJ_FolderDAL.DeleteFolder();
        }
        #endregion
    }
}