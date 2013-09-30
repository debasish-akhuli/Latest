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
    public class wf_mast_bal
    {
        // Variable Declaration
        #region
        wf_mast_dal wfMastObj;
        private Int64 _WFID = 0;
        private string _WFName = "";
        private string _WFStat = "A";
        private string _WFDept = "";
        private string _WFDocType = "";
        private string _WFFolderUUID = "";

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

        public String WFName
        {
            get
            {
                return _WFName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter workflow name");
                }
                _WFName = value;
            }
        }

        public String WFDept
        {
            get
            {
                return _WFDept;
            }
            set
            {
                _WFDept = value;
            }
        }

        public String WFDocType
        {
            get
            {
                return _WFDocType;
            }
            set
            {
                _WFDocType = value;
            }
        }

        public String WFFolderUUID
        {
            get
            {
                return _WFFolderUUID;
            }
            set
            {
                _WFFolderUUID = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// This function is used to insert record into the database.
        /// For this, the <WFName> & <WFStat> are set in the Data Access Layer (DAL) of <wf_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string InsertWFMast()
        {
            wfMastObj = new wf_mast_dal();
            wfMastObj.WFName = _WFName;
            wfMastObj.WFDept = _WFDept;
            wfMastObj.WFDocType = _WFDocType;
            wfMastObj.WFStat = _WFStat;
            wfMastObj.WFFolderUUID = _WFFolderUUID;

            return wfMastObj.InsertWFMast();
        }

        /// <summary>
        /// This function is used to delete (here the status will be inactive) record into the database.
        /// For this, the <WFID> is set in the Data Access Layer (DAL) of <wf_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string DeleteWF()
        {
            wfMastObj = new wf_mast_dal();
            wfMastObj.WFID = _WFID;

            return wfMastObj.DeleteWF();
        }
        #endregion
    }
}