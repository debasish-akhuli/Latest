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
    public class cabinet_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _CabinetCode = "";
        private string _CabinetName = "";
        private string _CabinetDesc = "";
        private string _UUID = "";
        private string _DefaultPermission = "";
        #endregion

        //Property Declaration
        #region
        public string CabinetCode
        {
            get
            {
                return _CabinetCode;
            }
            set
            {
                _CabinetCode = value;
            }
        }
        public String CabinetName
        {
            get
            {
                return _CabinetName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter cabinet name");
                }
                _CabinetName = value;
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
        public String DefaultPermission
        {
            get
            {
                return _DefaultPermission;
            }
            set
            {
                _DefaultPermission = value;
            }
        }
        public String CabinetDesc
        {
            get
            {
                return _CabinetDesc;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter cabinet description");
                }
                _CabinetDesc = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// Insert into the database --- Table Name:<cabinet_mast> Field Name:<cabinet_id><cabinet_name><cabinet_desc> Store Procedure Name:<CabinetMast_Insert>
        /// And also in this procedure, there is another checking for the data is already exists or not.
        /// </summary>
        /// <returns></returns>
        public string InsertCabinetMast()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("CabinetMast_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@cab_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cab_name"].Value = _CabinetName;

            cmd.Parameters.Add("@cab_desc", SqlDbType.Text, 1000);
            cmd.Parameters["@cab_desc"].Value = _CabinetDesc;

            cmd.Parameters.Add("@cab_uuid", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cab_uuid"].Value = _UUID;

            cmd.Parameters.Add("@DefaultPermission", SqlDbType.NVarChar, 50);
            cmd.Parameters["@DefaultPermission"].Value = _DefaultPermission;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To update <cabinet_mast> in gridview using stored procedure <CabinetMast_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateCabinet()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("CabinetMast_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@cab_uuid", SqlDbType.NVarChar,255);
            cmd.Parameters["@cab_uuid"].Value = _CabinetCode;

            cmd.Parameters.Add("@cab_desc", SqlDbType.Text);
            cmd.Parameters["@cab_desc"].Value = _CabinetDesc;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To delete <cabinet_mast> in gridview using stored procedure <CabinetMast_Del>
        /// </summary>
        /// <returns></returns>
        public string DeleteCabinet()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("sp_CabinetDelete", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@cab_uuid", SqlDbType.NVarChar,255);
            cmd.Parameters["@cab_uuid"].Value = _CabinetCode;

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