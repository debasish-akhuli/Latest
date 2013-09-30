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
    public class home_dal
    {
        // Variable Declaration
        #region
        private Int64 _DocID = 0;
        private Int64 _GrpID = 0;
        private string _CabName = "";
        private string _DrwName = "";
        private string _FldName = "";
        private string _DocName = "";
        private string _UserID = "";
        #endregion

        //Property Declaration
        #region
        public Int64 DocID
        {
            get
            {
                return _DocID;
            }
            set
            {
                _DocID = value;
            }
        }
        public Int64 GrpID
        {
            get
            {
                return _GrpID;
            }
            set
            {
                _GrpID = value;
            }
        }
        public String CabName
        {
            get
            {
                return _CabName;
            }
            set
            {
                _CabName = value;
            }
        }
        public String DrwName
        {
            get
            {
                return _DrwName;
            }
            set
            {
                _DrwName = value;
            }
        }
        public String FldName
        {
            get
            {
                return _FldName;
            }
            set
            {
                _FldName = value;
            }
        }
        public String DocName
        {
            get
            {
                return _DocName;
            }
            set
            {
                _DocName = value;
            }
        }
        public String UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// Fetch the selected Document details using Store Procedure Name:<Doc_Select>
        /// </summary>
        /// <returns></returns>
        public DataSet SelectDocDtl()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Doc_Select", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@cab_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@cab_name"].Value = _CabName;

            cmd.Parameters.Add("@drw_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@drw_name"].Value = _DrwName;

            cmd.Parameters.Add("@fld_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@fld_name"].Value = _FldName;

            cmd.Parameters.Add("@doc_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@doc_name"].Value = _DocName;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// Group the selected Document using Store Procedure Name:<Doc_Group>
        /// </summary>
        /// <returns></returns>
        public string funcGroup()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("Doc_Group", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_id", SqlDbType.BigInt);
            cmd.Parameters["@doc_id"].Value = _DocID;

            cmd.Parameters.Add("@grp_id", SqlDbType.BigInt);
            cmd.Parameters["@grp_id"].Value = _GrpID;

            cmd.Parameters.Add("@user_id", SqlDbType.NVarChar,20);
            cmd.Parameters["@user_id"].Value = _UserID;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return param.Value.ToString();
        }
        #endregion
    }
}