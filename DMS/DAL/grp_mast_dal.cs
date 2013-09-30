using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS.DAL
{
    public class grp_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private Int64 _GrpCode = 0;
        private string _GrpName = "";
        #endregion

        //Property Declaration
        #region
        public Int64 GrpCode
        {
            get
            {
                return _GrpCode;
            }
            set
            {
                _GrpCode = value;
            }
        }
        public String GrpName
        {
            get
            {
                return _GrpName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter group name");
                }
                _GrpName = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// Insert into the database --- Table Name:<grp_mast> Field Name:<grp_id><grp_name> Store Procedure Name:<GrpMast_Insert>
        /// And also in this procedure, there is another checking for the data is already exists or not.
        /// </summary>
        /// <returns></returns>
        public string InsertGrpMast()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("GrpMast_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@grp_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@grp_name"].Value = _GrpName;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To update <grp_mast> in gridview using stored procedure <GrpMast_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateGrp()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("GrpMast_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@grp_id", SqlDbType.BigInt);
            cmd.Parameters["@grp_id"].Value = _GrpCode;

            cmd.Parameters.Add("@grp_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@grp_name"].Value = _GrpName;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To delete <grp_mast> in gridview using stored procedure <GrpMast_Del>
        /// </summary>
        /// <returns></returns>
        public string DeleteGrp()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("GrpMast_Del", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@grp_id", SqlDbType.BigInt);
            cmd.Parameters["@grp_id"].Value = _GrpCode;

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