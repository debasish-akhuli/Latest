using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DMS.UTILITY;
using System.Data;

namespace DMS.DAL
{
    public class wf_dtl_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;

        private Int64 _WFID = 0;
        private Int64 _StepNo = 1;
        private string _RoleID = "";
        private string _Duration = "";
        private int _SignFldNo = 0;
        private int _SignDtFldNo = 0;

        #endregion

        //Property Declaration
        #region

        public Int64 WFID
        {
            get
            {
                return _WFID;
            }
            set
            {                
                _WFID = value;
            }
        }

        public Int64 StepNo
        {
            get
            {
                return _StepNo;
            }
            set
            {
                _StepNo = value;
            }
        }

        public String RoleID
        {
            get
            {
                return _RoleID;
            }
            set
            {
                _RoleID = value;
            }
        }
        public string Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Due time is Empty!");
                }
                _Duration = value;
            }
        }
        public int SignFldNo
        {
            get
            {
                return _SignFldNo;
            }
            set
            {
                _SignFldNo = value;
            }
        }
        public int SignDtFldNo
        {
            get
            {
                return _SignDtFldNo;
            }
            set
            {
                _SignDtFldNo = value;
            }
        }
        #endregion

        // Method Declaration
        #region

        #endregion
    }
}