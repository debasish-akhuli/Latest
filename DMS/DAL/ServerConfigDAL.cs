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
    public class ServerConfigDAL
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
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
        /// To update <ServerConfig> using stored procedure <ServerConfigUpdate>
        /// </summary>
        /// <returns></returns>
        public string UpdateServerConfig()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("ServerConfigUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@QuickPDFLicenseKey", SqlDbType.NVarChar, 255);
            cmd.Parameters["@QuickPDFLicenseKey"].Value = _QuickPDFLicenseKey;

            cmd.Parameters.Add("@ServerIP", SqlDbType.NVarChar, 255);
            cmd.Parameters["@ServerIP"].Value = _ServerIP;

            cmd.Parameters.Add("@DomainName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@DomainName"].Value = _DomainName;

            cmd.Parameters.Add("@CompName", SqlDbType.NVarChar, 255);
            cmd.Parameters["@CompName"].Value = _CompName;

            cmd.Parameters.Add("@HotlineNumber", SqlDbType.NVarChar, 255);
            cmd.Parameters["@HotlineNumber"].Value = _HotlineNumber;

            cmd.Parameters.Add("@HotlineEmail", SqlDbType.NVarChar, 255);
            cmd.Parameters["@HotlineEmail"].Value = _HotlineEmail;

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