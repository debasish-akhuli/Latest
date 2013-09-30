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
    public class mail_setup_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _SenderMail = "";
        private string _SenderName = "";
        private string _SMTPHost = "";
        private Int64 _SMTPPort = 0;
        private string _CredenUName = "";
        private string _CredenPwd = "";
        #endregion

        //Property Declaration
        #region
        public string SenderMail
        {
            get
            {
                return _SenderMail;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Sender Mail ID");
                }
                _SenderMail = value;
            }
        }
        public String SenderName
        {
            get
            {
                return _SenderName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Sender Display Name");
                }
                _SenderName = value;
            }
        }
        public String SMTPHost
        {
            get
            {
                return _SMTPHost;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter SMTP Host");
                }
                _SMTPHost = value;
            }
        }
        public Int64 SMTPPort
        {
            get
            {
                return _SMTPPort;
            }
            set
            {
                // validate the input
                if (value == 0)
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter SMTP Port");
                }
                _SMTPPort = value;
            }
        }
        public String CredenUName
        {
            get
            {
                return _CredenUName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Network Credential Username");
                }
                _CredenUName = value;
            }
        }
        public String CredenPwd
        {
            get
            {
                return _CredenPwd;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Network Credential Password");
                }
                _CredenPwd = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// To update <mail_setup> using stored procedure <MailSetup_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateMailSetup()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("MailSetup_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@sender_mail", SqlDbType.NVarChar,255);
            cmd.Parameters["@sender_mail"].Value = _SenderMail;

            cmd.Parameters.Add("@sender_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@sender_name"].Value = _SenderName;

            cmd.Parameters.Add("@smtp_host", SqlDbType.NVarChar, 255);
            cmd.Parameters["@smtp_host"].Value = _SMTPHost;

            cmd.Parameters.Add("@smtp_port", SqlDbType.BigInt);
            cmd.Parameters["@smtp_port"].Value = _SMTPPort;

            cmd.Parameters.Add("@creden_username", SqlDbType.NVarChar, 255);
            cmd.Parameters["@creden_username"].Value = _CredenUName;

            cmd.Parameters.Add("@creden_pwd", SqlDbType.NVarChar, 255);
            cmd.Parameters["@creden_pwd"].Value = _CredenPwd;

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