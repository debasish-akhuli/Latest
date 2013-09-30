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
    public class mail_setup_bal
    {
        // Variable Declaration
        #region
        mail_setup_dal OBJ_MailSetupDAL;
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
                if (value==0)
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
        /// To update <mail_setup> using stored procedure, pass the values to <mail_setup_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateMailSetup()
        {
            OBJ_MailSetupDAL = new mail_setup_dal();
            OBJ_MailSetupDAL.SenderMail = this._SenderMail;
            OBJ_MailSetupDAL.SenderName = this._SenderName;
            OBJ_MailSetupDAL.SMTPHost = this._SMTPHost;
            OBJ_MailSetupDAL.SMTPPort = this._SMTPPort;
            OBJ_MailSetupDAL.CredenUName = this._CredenUName;
            OBJ_MailSetupDAL.CredenPwd = this._CredenPwd;

            return OBJ_MailSetupDAL.UpdateMailSetup();
        }

        #endregion
    }
}