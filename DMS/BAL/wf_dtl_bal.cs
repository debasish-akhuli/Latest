using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DMS.DAL;

namespace DMS.BAL
{
    public class wf_dtl_bal
    {
        // Variable Declaration
        #region
        wf_dtl_dal wfDtlObj;
        private Int64 _WFID = 0;
        private string _WFName = "";
        private Int32 _StepNo = 1;
        private string _TaskID = "";
        private string _ActTypeID = "";
        private string _RoleId = "";
        private string _RoleName = "";
        private string _Duration = "";
        private string _TaskList = "";
        private string _UUID = "";
        private string _AmbleMail = "";
        private string _AmbleSub = "";
        private string _AmbleMsg = "";
        private Int32 _FormFieldNo = 1;
        private string _cond_op = "";
        private string _cond_val = "";
        private string _mail_to = "";
        private string _mail_msg = "";
        private string _mail_attach = "";
        private string _mail_url = "";
        private string _AppendDoc = "";
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

        public string WFName
        {
            get
            {
                return _WFName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Workflow Name is Empty!");
                }
                _WFName = value;
            }
        }

        public Int32 StepNo
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

        public string TaskID
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

        public string ActTypeID
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

        public string RoleId
        {
            get
            {
                return _RoleId;
            }
            set
            {
                _RoleId = value;
            }
        }
        public string RoleName
        {
            get
            {
                return _RoleName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Role Name is Empty!");
                }
                _RoleName = value;
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
        public string TaskList
        {
            get
            {
                return _TaskList;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Task List is Empty!");
                }
                _TaskList = value;
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
        public string mail_to
        {
            get
            {
                return _mail_to;
            }
            set
            {
                _mail_to = value;
            }
        }
        public string mail_msg
        {
            get
            {
                return _mail_msg;
            }
            set
            {
                _mail_msg = value;
            }
        }
        public string mail_attach
        {
            get
            {
                return _mail_attach;
            }
            set
            {
                _mail_attach = value;
            }
        }
        public string mail_url
        {
            get
            {
                return _mail_url;
            }
            set
            {
                _mail_url = value;
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
                
        #endregion

        #region
        /// <summary>
        /// Creating the Datatable of Workflow Task
        /// </summary>
        /// <returns></returns>
        public DataTable CreateDTWFTask()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("wf_id", typeof(Int64));
            dt.Columns.Add("step_no", typeof(Int32));
            dt.Columns.Add("task_id", typeof(string));
            dt.Columns.Add("acttype_id", typeof(string));
            dt.Columns.Add("copy_to_uuid", typeof(string));
            dt.Columns.Add("amble_mail", typeof(string));
            dt.Columns.Add("amble_msg", typeof(string));
            dt.Columns.Add("amble_attach", typeof(string));
            dt.Columns.Add("AppendDoc", typeof(string));
            dt.Columns.Add("amble_url", typeof(string));
            dt.Columns.Add("AmbleSub", typeof(string));
            return dt;
        }

        /// <summary>
        /// Creating the Datatable of Workflow Type Detail
        /// </summary>
        /// <returns></returns>
        public DataTable CreateDTWFDtl()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("wf_id", typeof(Int64));
            dt.Columns.Add("step_no", typeof(Int32));
            dt.Columns.Add("role_id", typeof(string));
            dt.Columns.Add("duration", typeof(string));
            return dt;
        }

        /// <summary>
        /// Creating the Datatable of Workflow Display
        /// </summary>
        /// <returns></returns>
        public DataTable CreateDTWFDisplay()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("step_no", typeof(Int32));
            dt.Columns.Add("wf_id", typeof(Int64));
            dt.Columns.Add("wf_name", typeof(string));
            dt.Columns.Add("role_id", typeof(string));
            dt.Columns.Add("role_name", typeof(string));
            dt.Columns.Add("duration", typeof(string));
            dt.Columns.Add("Task", typeof(string));
            return dt;
        }

        /// <summary>
        /// Creating the Datatable of Workflow Conditions
        /// </summary>
        /// <returns></returns>
        public DataTable CreateDTCondDisplay()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("wf_id", typeof(Int64));
            dt.Columns.Add("step_no", typeof(Int32));
            dt.Columns.Add("task_id", typeof(string));
            dt.Columns.Add("form_field_no", typeof(Int32));
            dt.Columns.Add("cond_op", typeof(string));
            dt.Columns.Add("cond_val", typeof(string));
            dt.Columns.Add("amble_mails", typeof(string));
            dt.Columns.Add("amble_msg", typeof(string));
            dt.Columns.Add("amble_attach", typeof(string));
            dt.Columns.Add("amble_url", typeof(string));
            dt.Columns.Add("CondMailSub", typeof(string));
            return dt;
        }

        public DataTable CreateDTSignDate()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("wf_id", typeof(Int64));
            dt.Columns.Add("step_no", typeof(Int32));
            dt.Columns.Add("Sign1", typeof(Int32));
            dt.Columns.Add("Date1", typeof(Int32));
            dt.Columns.Add("Sign2", typeof(Int32));
            dt.Columns.Add("Date2", typeof(Int32));
            dt.Columns.Add("Sign3", typeof(Int32));
            dt.Columns.Add("Date3", typeof(Int32));
            dt.Columns.Add("Sign4", typeof(Int32));
            dt.Columns.Add("Date4", typeof(Int32));
            dt.Columns.Add("Sign5", typeof(Int32));
            dt.Columns.Add("Date5", typeof(Int32));
            dt.Columns.Add("Sign6", typeof(Int32));
            dt.Columns.Add("Date6", typeof(Int32));
            dt.Columns.Add("Sign7", typeof(Int32));
            dt.Columns.Add("Date7", typeof(Int32));
            dt.Columns.Add("Sign8", typeof(Int32));
            dt.Columns.Add("Date8", typeof(Int32));
            dt.Columns.Add("Sign9", typeof(Int32));
            dt.Columns.Add("Date9", typeof(Int32));
            dt.Columns.Add("Sign10", typeof(Int32));
            dt.Columns.Add("Date10", typeof(Int32));

            return dt;
        }

        public DataRow AddNewCondDisplay(DataRow r3)
        {
            r3["wf_id"] = _WFID;
            r3["step_no"] = _StepNo;
            r3["task_id"] = _TaskID;
            r3["form_field_no"] = _FormFieldNo;
            r3["cond_op"] = _cond_op;
            r3["cond_val"] = _cond_val;
            r3["amble_mails"] = _mail_to;
            r3["amble_msg"] = _mail_msg;
            r3["amble_attach"] = _mail_attach;
            r3["amble_url"] = _mail_url;
            r3["CondMailSub"] = _CondMailSub;
            return r3;
        }

        public DataRow AddNewCondDisplay(DataRow r5,int Sign1,int Date1,int Sign2,int Date2,int Sign3,int Date3,int Sign4,int Date4,int Sign5,int Date5,int Sign6,int Date6,int Sign7,int Date7,int Sign8,int Date8,int Sign9,int Date9,int Sign10,int Date10)
        {
            r5["wf_id"] = _WFID;
            r5["step_no"] = _StepNo;
            r5["Sign1"] = Sign1;
            r5["Date1"] = Date1;
            r5["Sign2"] = Sign2;
            r5["Date2"] = Date2;
            r5["Sign3"] = Sign3;
            r5["Date3"] = Date3;
            r5["Sign4"] = Sign4;
            r5["Date4"] = Date4;
            r5["Sign5"] = Sign5;
            r5["Date5"] = Date5;
            r5["Sign6"] = Sign6;
            r5["Date6"] = Date6;
            r5["Sign7"] = Sign7;
            r5["Date7"] = Date7;
            r5["Sign8"] = Sign8;
            r5["Date8"] = Date8;
            r5["Sign9"] = Sign9;
            r5["Date9"] = Date9;
            r5["Sign10"] = Sign10;
            r5["Date10"] = Date10;
            return r5;
        }

        public DataRow AddNewRecordTask(DataRow r)
        {
            r["wf_id"] = _WFID;
            r["step_no"] = _StepNo;
            r["task_id"] = _TaskID;
            r["acttype_id"] = _ActTypeID;
            r["copy_to_uuid"] = _UUID;
            r["amble_mail"] = _AmbleMail;
            r["amble_msg"] = _AmbleMsg;
            r["amble_attach"] = _mail_attach;
            r["AppendDoc"] = _AppendDoc;
            r["amble_url"] = _mail_url;
            r["AmbleSub"] = _AmbleSub;
            return r;
        }

        public DataRow AddNewRecordDtl(DataRow r)
        {
            r["wf_id"] = _WFID;
            r["step_no"] = _StepNo;
            r["role_id"] = _RoleId;
            r["duration"] = _Duration;
            return r;
        }

        public DataRow AddNewRecordDisplay(DataRow r3)
        {
            r3["step_no"] = _StepNo;
            r3["wf_id"] = _WFID;
            r3["wf_name"] = _WFName;
            r3["role_id"] = _RoleId;
            r3["role_name"] = _RoleName;
            r3["duration"] = _Duration;
            r3["Task"] = _TaskList;
            return r3;
        }

        #endregion

        
    }
}