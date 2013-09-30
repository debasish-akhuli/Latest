using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DMS.UTILITY;
using System.Data;

namespace DMS.DAL
{
    public class wf_task_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;

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

        // Method Declaration
        #region
        /// <summary>
        /// Insert into the database --- Table Name:<wf_task> Field Name:<wf_id><step_no><task_id><acttype_id> Store Procedure Name:<WorkflowTask_Insert>
        /// And also in this procedure, there is another checking for the data is already exists or not.
        /// </summary>
        /// <returns></returns>
        public string InsertWFTask()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("WorkflowTask_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = _WFID;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = _StepNo;

            cmd.Parameters.Add("@task_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@task_id"].Value = _TaskID;

            cmd.Parameters.Add("@acttype_id", SqlDbType.NVarChar, 20);
            cmd.Parameters["@acttype_id"].Value = _ActTypeID;

            cmd.Parameters.Add("@copy_to_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@copy_to_uuid"].Value = _UUID;

            cmd.Parameters.Add("@amble_mail", SqlDbType.NVarChar, 255);
            cmd.Parameters["@amble_mail"].Value = _AmbleMail;

            cmd.Parameters.Add("@amble_msg", SqlDbType.Text);
            cmd.Parameters["@amble_msg"].Value = _AmbleMsg;

            cmd.Parameters.Add("@amble_attach", SqlDbType.NVarChar, 20);
            cmd.Parameters["@amble_attach"].Value = _AmbleAttach;

            cmd.Parameters.Add("@AppendDoc", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AppendDoc"].Value = _AppendDoc;

            cmd.Parameters.Add("@amble_url", SqlDbType.NVarChar, 20);
            cmd.Parameters["@amble_url"].Value = _AmbleURL;

            cmd.Parameters.Add("@AmbleSub", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AmbleSub"].Value = _AmbleSub;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertWFCond()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("WorkflowCond_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = _WFID;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = _StepNo;

            cmd.Parameters.Add("@task_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@task_id"].Value = _TaskID;

            cmd.Parameters.Add("@form_field_no", SqlDbType.Int);
            cmd.Parameters["@form_field_no"].Value = _FormFieldNo;

            cmd.Parameters.Add("@cond_op", SqlDbType.NVarChar, 2);
            cmd.Parameters["@cond_op"].Value = _cond_op;

            cmd.Parameters.Add("@cond_val", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cond_val"].Value = _cond_val;

            cmd.Parameters.Add("@amble_mails", SqlDbType.NVarChar, 255);
            cmd.Parameters["@amble_mails"].Value = _AmbleMail;

            cmd.Parameters.Add("@amble_msg", SqlDbType.Text);
            cmd.Parameters["@amble_msg"].Value = _AmbleMsg;

            cmd.Parameters.Add("@amble_attach", SqlDbType.NVarChar, 20);
            cmd.Parameters["@amble_attach"].Value = _AmbleAttach;

            cmd.Parameters.Add("@amble_url", SqlDbType.NVarChar, 20);
            cmd.Parameters["@amble_url"].Value = _AmbleURL;

            cmd.Parameters.Add("@AmbleSub", SqlDbType.NVarChar, 255);
            cmd.Parameters["@AmbleSub"].Value = _CondMailSub;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        public string InsertWFSignDate(int Sign1, int Date1, int Sign2, int Date2, int Sign3, int Date3, int Sign4, int Date4, int Sign5, int Date5, int Sign6, int Date6, int Sign7, int Date7, int Sign8, int Date8, int Sign9, int Date9, int Sign10, int Date10)
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("WFSignDate_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@wf_id", SqlDbType.BigInt);
            cmd.Parameters["@wf_id"].Value = _WFID;

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = _StepNo;

            cmd.Parameters.Add("@Sign1", SqlDbType.Int);
            cmd.Parameters["@Sign1"].Value = Sign1;

            cmd.Parameters.Add("@Date1", SqlDbType.Int);
            cmd.Parameters["@Date1"].Value = Date1;

            cmd.Parameters.Add("@Sign2", SqlDbType.Int);
            cmd.Parameters["@Sign2"].Value = Sign2;

            cmd.Parameters.Add("@Date2", SqlDbType.Int);
            cmd.Parameters["@Date2"].Value = Date2;

            cmd.Parameters.Add("@Sign3", SqlDbType.Int);
            cmd.Parameters["@Sign3"].Value = Sign3;

            cmd.Parameters.Add("@Date3", SqlDbType.Int);
            cmd.Parameters["@Date3"].Value = Date3;

            cmd.Parameters.Add("@Sign4", SqlDbType.Int);
            cmd.Parameters["@Sign4"].Value = Sign4;

            cmd.Parameters.Add("@Date4", SqlDbType.Int);
            cmd.Parameters["@Date4"].Value = Date4;

            cmd.Parameters.Add("@Sign5", SqlDbType.Int);
            cmd.Parameters["@Sign5"].Value = Sign5;

            cmd.Parameters.Add("@Date5", SqlDbType.Int);
            cmd.Parameters["@Date5"].Value = Date5;

            cmd.Parameters.Add("@Sign6", SqlDbType.Int);
            cmd.Parameters["@Sign6"].Value = Sign6;

            cmd.Parameters.Add("@Date6", SqlDbType.Int);
            cmd.Parameters["@Date6"].Value = Date6;

            cmd.Parameters.Add("@Sign7", SqlDbType.Int);
            cmd.Parameters["@Sign7"].Value = Sign7;

            cmd.Parameters.Add("@Date7", SqlDbType.Int);
            cmd.Parameters["@Date7"].Value = Date7;

            cmd.Parameters.Add("@Sign8", SqlDbType.Int);
            cmd.Parameters["@Sign8"].Value = Sign8;

            cmd.Parameters.Add("@Date8", SqlDbType.Int);
            cmd.Parameters["@Date8"].Value = Date8;

            cmd.Parameters.Add("@Sign9", SqlDbType.Int);
            cmd.Parameters["@Sign9"].Value = Sign9;

            cmd.Parameters.Add("@Date9", SqlDbType.Int);
            cmd.Parameters["@Date9"].Value = Date9;

            cmd.Parameters.Add("@Sign10", SqlDbType.Int);
            cmd.Parameters["@Sign10"].Value = Sign10;

            cmd.Parameters.Add("@Date10", SqlDbType.Int);
            cmd.Parameters["@Date10"].Value = Date10;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        #endregion
    }
}