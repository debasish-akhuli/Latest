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
    public class drawer_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _DrawerName = "";
        private string _DrawerDesc = "";
        private string _CabinetCode = "";
        private string _Labelid = "";
        private string _UUID = "";
        #endregion

        //Property Declaration
        #region
        public String DrawerName
        {
            get
            {
                return _DrawerName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter drawer name");
                }
                _DrawerName = value;
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
        public String DrawerDesc
        {
            get
            {
                return _DrawerDesc;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter drawer description");
                }
                _DrawerDesc = value;
            }
        }
        public String CabinetCode
        {
            get
            {
                return _CabinetCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select cabinet");
                }
                _CabinetCode = value;
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
        /// To update <drawer_mast> in gridview using stored procedure <DrawerMast_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateDrawer()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("DrawerMast_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@drw_desc", SqlDbType.NText);
            cmd.Parameters["@drw_desc"].Value = _DrawerDesc;

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
        /// To delete <drawer_mast> in gridview using stored procedure <DrawerMast_Del>
        /// </summary>
        /// <returns></returns>
        public string DeleteDrawer()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("DrawerMast_Del", con);
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