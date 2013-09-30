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
    public class folder_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _FolderName = "";
        private string _FolderDesc = "";
        private string _DrawerCode = "";
        private string _Labelid = "";
        private string _UUID = "";
        #endregion

        //Property Declaration
        #region
        public String FolderName
        {
            get
            {
                return _FolderName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter folder name");
                }
                _FolderName = value;
            }
        }
        public String UUID
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
        public String FolderDesc
        {
            get
            {
                return _FolderDesc;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter folder description");
                }
                _FolderDesc = value;
            }
        }
        public String DrawerCode
        {
            get
            {
                return _DrawerCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select drawer");
                }
                _DrawerCode = value;
            }
        }
        public String Labelid
        {
            get
            {
                return _Labelid;
            }
            set
            {
                _Labelid = value;
            }
        }
        #endregion

        // Method Declaration
        #region

        /// <summary>
        /// To update <folder_mast> in gridview using stored procedure <FolderMast_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateFolder()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("FolderMast_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@fld_desc", SqlDbType.NText);
            cmd.Parameters["@fld_desc"].Value = _FolderDesc;

            cmd.Parameters.Add("@lbl_id", SqlDbType.NVarChar,255);
            cmd.Parameters["@lbl_id"].Value = _Labelid;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To delete <folder_mast> in gridview using stored procedure <FolderMast_Del>
        /// </summary>
        /// <returns></returns>
        public string DeleteFolder()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("FolderMast_Del", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@lbl_id", SqlDbType.NVarChar,255);
            cmd.Parameters["@lbl_id"].Value = _Labelid;

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