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
    public class ServerConfigBAL
    {
        // Variable Declaration
        #region
        ServerConfigDAL OBJ_ServerConfigDAL;
        private string _QuickPDFLicenseKey = "";
        private string _ServerIP = "";
        private string _DomainName = "";
        private string _CompName = "";
        private string _HotlineNumber = "";
        private string _HotlineEmail = "";
        #endregion

        //Property Declaration
        #region
        public string QuickPDFLicenseKey
        {
            get
            {
                return _QuickPDFLicenseKey;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Quick PDF License Key");
                }
                _QuickPDFLicenseKey = value;
            }
        }
        public String ServerIP
        {
            get
            {
                return _ServerIP;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Server IP Address");
                }
                _ServerIP = value;
            }
        }
        public String DomainName
        {
            get
            {
                return _DomainName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Domain Name");
                }
                _DomainName = value;
            }
        }
        public String CompName
        {
            get
            {
                return _CompName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Company Name");
                }
                _CompName = value;
            }
        }
        public String HotlineNumber
        {
            get
            {
                return _HotlineNumber;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Hotline Number");
                }
                _HotlineNumber = value;
            }
        }
        public String HotlineEmail
        {
            get
            {
                return _HotlineEmail;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter Hotline Email");
                }
                _HotlineEmail = value;
            }
        }
        #endregion

        // Method Declaration
        #region

        /// <summary>
        /// To update <ServerConfig> using stored procedure, pass the values to <ServerConfigDAL>
        /// </summary>
        /// <returns></returns>
        public string UpdateServerConfig()
        {
            OBJ_ServerConfigDAL = new ServerConfigDAL();
            OBJ_ServerConfigDAL.QuickPDFLicenseKey = this._QuickPDFLicenseKey;
            OBJ_ServerConfigDAL.ServerIP = this._ServerIP;
            OBJ_ServerConfigDAL.DomainName = this._DomainName;
            OBJ_ServerConfigDAL.CompName = this._CompName;
            OBJ_ServerConfigDAL.HotlineNumber = this._HotlineNumber;
            OBJ_ServerConfigDAL.HotlineEmail = this._HotlineEmail;

            return OBJ_ServerConfigDAL.UpdateServerConfig();
        }

        #endregion
    }
}