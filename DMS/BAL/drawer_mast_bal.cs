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
    public class drawer_mast_bal
    {
        // Variable Declaration
        #region
        drawer_mast_dal OBJ_DrawerDAL;
        private string _DrawerName = "";
        private string _DrawerDesc = "";
        private string _CabinetCode = "";
        private string _Labelid = "";
        private string _UUID = "";
        #endregion

        //Property Declaration
        #region
        public String DrawerName
        {
            get
            {
                return _DrawerName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter drawer name");
                }
                _DrawerName = value;
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
        public String DrawerDesc
        {
            get
            {
                return _DrawerDesc;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter drawer description");
                }
                _DrawerDesc = value;
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
                    throw new Exception("Please select cabinet");
                }
                _CabinetCode = value;
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
        /// To update <drawer_mast> in gridview using stored procedure, pass the values to <drawer_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateDrawer()
        {
            OBJ_DrawerDAL = new drawer_mast_dal();
            OBJ_DrawerDAL.DrawerDesc = this._DrawerDesc;
            OBJ_DrawerDAL.Labelid = this._Labelid;

            return OBJ_DrawerDAL.UpdateDrawer();
        }

        /// <summary>
        /// To delete <drawer_mast> in gridview using stored procedure, pass the values to <drawer_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string DeleteDrawer()
        {
            OBJ_DrawerDAL = new drawer_mast_dal();
            OBJ_DrawerDAL.Labelid = this._Labelid;

            return OBJ_DrawerDAL.DeleteDrawer();
        }
        #endregion
    }
}