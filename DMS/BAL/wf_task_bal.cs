using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DMS.UTILITY;
using System.Data;
using DMS.DAL;

namespace DMS.BAL
{
    public class wf_task_bal
    {
        // Variable Declaration
        #region
        wf_task_dal wfTaskObj;
        private Int64 _WFID = 0;
        private Int64 _StepNo = 1;
        private string _TaskID = "";
        private string _ActTypeID = "";
        private string _UUID = "";
        private string _AmbleMail = "";
        private string _AmbleMsg = "";
        private Int32 _FormFieldNo = 1;
        private string _cond_op = "";
        private string _cond_val = "";
        private string _AmbleAttach = "";
        private string _AppendDoc = "";
        private string _AmbleURL = "";
        private string _AmbleSub = "";
        private string _CondMailSub = "";

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

        public String TaskID
        {
            get
            {
                return _TaskID;
            }
            set
            {
                _TaskID = value;
            }
        }
        public String ActTypeID
        {
            get
            {
                return _ActTypeID;
            }
            set
            {
                _ActTypeID = value;
            }
        }
        public string UUID
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
        public string AmbleMail
        {
            get
            {
                return _AmbleMail;
            }
            set
            {
                _AmbleMail = value;
            }
        }
        public string AmbleMsg
        {
            get
            {
                return _AmbleMsg;
            }
            set
            {
                _AmbleMsg = value;
            }
        }
        public string AmbleAttach
        {
            get
            {
                return _AmbleAttach;
            }
            set
            {
                _AmbleAttach = value;
            }
        }
        public string AmbleURL
        {
            get
            {
                return _AmbleURL;
            }
            set
            {
                _AmbleURL = value;
            }
        }
        public string AmbleSub
        {
            get
            {
                return _AmbleSub;
            }
            set
            {
                _AmbleSub = value;
            }
        }
        public string CondMailSub
        {
            get
            {
                return _CondMailSub;
            }
            set
            {
                _CondMailSub = value;
            }
        }
        public string AppendDoc
        {
            get
            {
                return _AppendDoc;
            }
            set
            {
                _AppendDoc = value;
            }
        }
        public Int32 FormFieldNo
        {
            get
            {
                return _FormFieldNo;
            }
            set
            {
                _FormFieldNo = value;
            }
        }
        public string cond_op
        {
            get
            {
                return _cond_op;
            }
            set
            {
                _cond_op = value;
            }
        }
        public string cond_val
        {
            get
            {
                return _cond_val;
            }
            set
            {
                _cond_val = value;
            }
        }
        #endregion

        #region
        /// <summary>
        /// This function is used to insert record into the database.
        /// For this, the <WFID>,<StepNo>,<TaskID>,<ActTypeID> are set in the Data Access Layer (DAL) of <wf_task_dal>
        /// </summary>
        /// <returns></returns>
        public string InsertWFTask()
        {
            wfTaskObj = new wf_task_dal();
            wfTaskObj.WFID = _WFID;
            wfTaskObj.StepNo = _StepNo;
            wfTaskObj.TaskID = _TaskID;
            wfTaskObj.ActTypeID = _ActTypeID;
            wfTaskObj.UUID = _UUID;
            wfTaskObj.AmbleMail = _AmbleMail;
            wfTaskObj.AmbleMsg = _AmbleMsg;
            wfTaskObj.AmbleAttach = _AmbleAttach;
            wfTaskObj.AppendDoc = _AppendDoc;
            wfTaskObj.AmbleURL = _AmbleURL;
            wfTaskObj.AmbleSub = _AmbleSub;

            return wfTaskObj.InsertWFTask();
        }

        public string InsertWFCond()
        {
            wfTaskObj = new wf_task_dal();
            wfTaskObj.WFID = _WFID;
            wfTaskObj.StepNo = _StepNo;
            wfTaskObj.TaskID = _TaskID;
            wfTaskObj.FormFieldNo = _FormFieldNo;
            wfTaskObj.cond_op = _cond_op;
            wfTaskObj.cond_val = _cond_val;
            wfTaskObj.AmbleMail = _AmbleMail;
            wfTaskObj.AmbleMsg = _AmbleMsg;
            wfTaskObj.AmbleAttach = _AmbleAttach;
            wfTaskObj.AmbleURL = _AmbleURL;
            wfTaskObj.CondMailSub = _CondMailSub;

            return wfTaskObj.InsertWFCond();
        }

        public string InsertWFSignDate(int Sign1, int Date1, int Sign2, int Date2, int Sign3, int Date3, int Sign4, int Date4, int Sign5, int Date5, int Sign6, int Date6, int Sign7, int Date7, int Sign8, int Date8, int Sign9, int Date9, int Sign10, int Date10)
        {
            wfTaskObj = new wf_task_dal();
            wfTaskObj.WFID = _WFID;
            wfTaskObj.StepNo = _StepNo;

            return wfTaskObj.InsertWFSignDate(Sign1, Date1, Sign2, Date2, Sign3, Date3, Sign4, Date4, Sign5, Date5, Sign6, Date6, Sign7, Date7, Sign8, Date8, Sign9, Date9, Sign10,Date10);
        }
        #endregion
    }
}