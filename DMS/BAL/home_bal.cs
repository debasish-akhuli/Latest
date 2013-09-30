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
    public class home_bal
    {
        // Variable Declaration
        #region
        home_dal Obj_HomeDAL;
        private Int64 _DocID = 0;
        private Int64 _GrpID = 0;
        private string _CabName = "";
        private string _DrwName = "";
        private string _FldName = "";
        private string _DocName = "";
        private string _UserID = "";
        #endregion

        //Property Declaration
        #region
        public Int64 DocID
        {
            get
            {
                return _DocID;
            }
            set
            {
                _DocID = value;
            }
        }
        public Int64 GrpID
        {
            get
            {
                return _GrpID;
            }
            set
            {
                _GrpID = value;
            }
        }
        public String CabName
        {
            get
            {
                return _CabName;
            }
            set
            {
                _CabName = value;
            }
        }
        public String DrwName
        {
            get
            {
                return _DrwName;
            }
            set
            {
                _DrwName = value;
            }
        }
        public String FldName
        {
            get
            {
                return _FldName;
            }
            set
            {
                _FldName = value;
            }
        }
        public String DocName
        {
            get
            {
                return _DocName;
            }
            set
            {
                _DocName = value;
            }
        }
        public String UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// This function is used to fetch the selected Document details
        /// </summary>
        /// <returns></returns>
        public DataSet SelectDocDtl()
        {
            Obj_HomeDAL = new home_dal();
            Obj_HomeDAL.CabName = this._CabName;
            Obj_HomeDAL.DrwName = this._DrwName;
            Obj_HomeDAL.FldName = this._FldName;
            Obj_HomeDAL.DocName = this._DocName;

            return Obj_HomeDAL.SelectDocDtl();
        }

        /// <summary>
        /// This function is used to Group the selected Document
        /// </summary>
        /// <returns></returns>
        public string funcGroup()
        {
            Obj_HomeDAL = new home_dal();
            Obj_HomeDAL.DocID = this._DocID;
            Obj_HomeDAL.GrpID = this._GrpID;
            Obj_HomeDAL.UserID = this._UserID;

            return Obj_HomeDAL.funcGroup();
        }
        #endregion

    }
}