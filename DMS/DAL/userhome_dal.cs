using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using DMS.DAL;

namespace DMS.DAL
{
    public class userhome_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _WFLogID = "";
        private Int32 _StepNo = 0;
        private DateTime _TaskDone_Dt = DateTime.Now;
        private string _Comments = "";
        private string _Task_ID = "";
        #endregion

        //Property Declaration
        #region
        public String WFLogID
        {
            get
            {
                return _WFLogID;
            }
            set
            {
                _WFLogID = value;
            }
        }
        public int StepNo
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
        public DateTime TaskDone_Dt
        {
            get
            {
                return _TaskDone_Dt;
            }
            set
            {
                _TaskDone_Dt = value;
            }
        }
        public String Comments
        {
            get
            {
                return _Comments;
            }
            set
            {
                _Comments = value;
            }
        }
        public String Task_ID
        {
            get
            {
                return _Task_ID;
            }
            set
            {
                _Task_ID = value;
            }
        }
        #endregion

        // Method Declaration
        #region

        #endregion
    }
}