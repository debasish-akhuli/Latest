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
    public class task_mast_bal
    {
        // Variable Declaration
        #region
        task_mast_dal OBJ_TaskDAL;
        private string _TaskCode = "";
        private string _TaskName = "";
        #endregion

        //Property Declaration
        #region
        public String TaskCode
        {
            get
            {
                return _TaskCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter task code");
                }
                _TaskCode = value;
            }
        }
        public String TaskName
        {
            get
            {
                return _TaskName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter task name");
                }
                _TaskName = value;
            }
        }
        #endregion

        // Method Declaration
        #region

        #endregion
    }
}