using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS
{
    public class user_rights_dal
    {
        // Variable Declaration
        #region
        private Int64 _CabID = 0;
        private Int64 _DrwID = 0;
        private Int64 _FldID = 0;
        #endregion

        //Property Declaration
        #region
        public Int64 CabID
        {
            get
            {
                return _CabID;
            }
            set
            {
                _CabID = value;
            }
        }
        public Int64 DrwID
        {
            get
            {
                return _DrwID;
            }
            set
            {
                _DrwID = value;
            }
        }
        public Int64 FldID
        {
            get
            {
                return _FldID;
            }
            set
            {
                _FldID = value;
            }
        }
        #endregion
        
        /// <summary>
        /// Gridview to populate Doc Master using <DocMast_GV> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet GVDoc(string selectedF)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();

            cmd = new SqlCommand("DocMast_DepGV", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@fld_uuid", SqlDbType.NVarChar,255);
            cmd.Parameters["@fld_uuid"].Value =selectedF;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }



    }
}