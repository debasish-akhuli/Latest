using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS.DAL
{
    public class doc_type_mast_dal
    {
        // Variable Declaration
        #region
        SqlCommand cmd;
        private string _DocTypeID = "";
        private string _DocTypeName = "";
        private string _Tag1 = "";
        private string _Tag2 = "";
        private string _Tag3 = "";
        private string _Tag4 = "";
        private string _Tag5 = "";
        private string _Tag6 = "";
        private string _Tag7 = "";
        private string _Tag8 = "";
        private string _Tag9 = "";
        private string _Tag10 = "";
        private Int32 _Tag1FieldNo = 0;
        private Int32 _Tag2FieldNo = 0;
        private Int32 _Tag3FieldNo = 0;
        private Int32 _Tag4FieldNo = 0;
        private Int32 _Tag5FieldNo = 0;
        private Int32 _Tag6FieldNo = 0;
        private Int32 _Tag7FieldNo = 0;
        private Int32 _Tag8FieldNo = 0;
        private Int32 _Tag9FieldNo = 0;
        private Int32 _Tag10FieldNo = 0;
        private Int32 _SignFieldNo1 = 0;
        private Int32 _SignDateFieldNo1 = 0;
        private Int32 _SignFieldNo2 = 0;
        private Int32 _SignDateFieldNo2 = 0;
        private Int32 _SignFieldNo3 = 0;
        private Int32 _SignDateFieldNo3 = 0;
        #endregion

        //Property Declaration
        #region
        public String DocTypeID
        {
            get
            {
                return _DocTypeID;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter document type id");
                }
                _DocTypeID = value;
            }
        }
        public String DocTypeName
        {
            get
            {
                return _DocTypeName;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter document type name");
                }
                _DocTypeName = value;
            }
        }
        public string Tag1
        {
            get { return _Tag1; }
            set { _Tag1 = value; }
        }
        public string Tag2
        {
            get { return _Tag2; }
            set { _Tag2 = value; }
        }
        public string Tag3
        {
            get { return _Tag3; }
            set { _Tag3 = value; }
        }
        public string Tag4
        {
            get { return _Tag4; }
            set { _Tag4 = value; }
        }
        public string Tag5
        {
            get { return _Tag5; }
            set { _Tag5 = value; }
        }
        public string Tag6
        {
            get { return _Tag6; }
            set { _Tag6 = value; }
        }
        public string Tag7
        {
            get { return _Tag7; }
            set { _Tag7 = value; }
        }
        public string Tag8
        {
            get { return _Tag8; }
            set { _Tag8 = value; }
        }
        public string Tag9
        {
            get { return _Tag9; }
            set { _Tag9 = value; }
        }
        public string Tag10
        {
            get { return _Tag10; }
            set { _Tag10 = value; }
        }
        public Int32 Tag1FieldNo
        {
            get { return _Tag1FieldNo; }
            set
            {
                _Tag1FieldNo = value;
            }
        }
        public Int32 Tag2FieldNo
        {
            get { return _Tag2FieldNo; }
            set
            {
                _Tag2FieldNo = value;
            }
        }
        public Int32 Tag3FieldNo
        {
            get { return _Tag3FieldNo; }
            set
            {
                _Tag3FieldNo = value;
            }
        }
        public Int32 Tag4FieldNo
        {
            get { return _Tag4FieldNo; }
            set
            {
                _Tag4FieldNo = value;
            }
        }
        public Int32 Tag5FieldNo
        {
            get { return _Tag5FieldNo; }
            set
            {
                _Tag5FieldNo = value;
            }
        }
        public Int32 Tag6FieldNo
        {
            get { return _Tag6FieldNo; }
            set
            {
                _Tag6FieldNo = value;
            }
        }
        public Int32 Tag7FieldNo
        {
            get { return _Tag7FieldNo; }
            set
            {
                _Tag7FieldNo = value;
            }
        }
        public Int32 Tag8FieldNo
        {
            get { return _Tag8FieldNo; }
            set
            {
                _Tag8FieldNo = value;
            }
        }
        public Int32 Tag9FieldNo
        {
            get { return _Tag9FieldNo; }
            set
            {
                _Tag9FieldNo = value;
            }
        }
        public Int32 Tag10FieldNo
        {
            get { return _Tag10FieldNo; }
            set
            {
                _Tag10FieldNo = value;
            }
        }
        public Int32 SignFieldNo1
        {
            get { return _SignFieldNo1; }
            set
            {
                _SignFieldNo1 = value;
            }
        }
        public Int32 SignDateFieldNo1
        {
            get { return _SignDateFieldNo1; }
            set
            {
                _SignDateFieldNo1 = value;
            }
        }
        public Int32 SignFieldNo2
        {
            get { return _SignFieldNo2; }
            set
            {
                _SignFieldNo2 = value;
            }
        }
        public Int32 SignDateFieldNo2
        {
            get { return _SignDateFieldNo2; }
            set
            {
                _SignDateFieldNo2 = value;
            }
        }
        public Int32 SignFieldNo3
        {
            get { return _SignFieldNo3; }
            set
            {
                _SignFieldNo3 = value;
            }
        }
        public Int32 SignDateFieldNo3
        {
            get { return _SignDateFieldNo3; }
            set
            {
                _SignDateFieldNo3 = value;
            }
        }
        #endregion

        // Method Declaration
        #region
        /// <summary>
        /// Insert into the database --- Table Name:<doc_type_mast> 
        /// Field Name:<doc_type_id><doc_type_name><tag1><tag2><tag3><tag4><tag5><tag6><tag7><tag8><tag9><tag10><SignFieldNo1><SignDateFieldNo1><SignFieldNo2><SignDateFieldNo2><SignFieldNo3><SignDateFieldNo3>
        /// Store Procedure Name:<Insert_DocTypeMast>
        /// And also in this procedure, there is another checking for the data is already exists or not.
        /// </summary>
        /// <returns></returns>
        public string InsertDocTypeMast()
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("DocTypeMast_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = _DocTypeID;

            cmd.Parameters.Add("@doc_type_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@doc_type_name"].Value = _DocTypeName;

            cmd.Parameters.Add("@tag1", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag1"].Value = _Tag1;

            cmd.Parameters.Add("@tag2", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag2"].Value = _Tag2;

            cmd.Parameters.Add("@tag3", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag3"].Value = _Tag3;

            cmd.Parameters.Add("@tag4", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag4"].Value = _Tag4;

            cmd.Parameters.Add("@tag5", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag5"].Value = _Tag5;

            cmd.Parameters.Add("@tag6", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag6"].Value = _Tag6;

            cmd.Parameters.Add("@tag7", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag7"].Value = _Tag7;

            cmd.Parameters.Add("@tag8", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag8"].Value = _Tag8;

            cmd.Parameters.Add("@tag9", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag9"].Value = _Tag9;

            cmd.Parameters.Add("@tag10", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag10"].Value = _Tag10;

            cmd.Parameters.Add("@tag1fieldno", SqlDbType.Int);
            cmd.Parameters["@tag1fieldno"].Value = _Tag1FieldNo;

            cmd.Parameters.Add("@tag2fieldno", SqlDbType.Int);
            cmd.Parameters["@tag2fieldno"].Value = _Tag2FieldNo;

            cmd.Parameters.Add("@tag3fieldno", SqlDbType.Int);
            cmd.Parameters["@tag3fieldno"].Value = _Tag3FieldNo;

            cmd.Parameters.Add("@tag4fieldno", SqlDbType.Int);
            cmd.Parameters["@tag4fieldno"].Value = _Tag4FieldNo;

            cmd.Parameters.Add("@tag5fieldno", SqlDbType.Int);
            cmd.Parameters["@tag5fieldno"].Value = _Tag5FieldNo;

            cmd.Parameters.Add("@tag6fieldno", SqlDbType.Int);
            cmd.Parameters["@tag6fieldno"].Value = _Tag6FieldNo;

            cmd.Parameters.Add("@tag7fieldno", SqlDbType.Int);
            cmd.Parameters["@tag7fieldno"].Value = _Tag7FieldNo;

            cmd.Parameters.Add("@tag8fieldno", SqlDbType.Int);
            cmd.Parameters["@tag8fieldno"].Value = _Tag8FieldNo;

            cmd.Parameters.Add("@tag9fieldno", SqlDbType.Int);
            cmd.Parameters["@tag9fieldno"].Value = _Tag9FieldNo;

            cmd.Parameters.Add("@tag10fieldno", SqlDbType.Int);
            cmd.Parameters["@tag10fieldno"].Value = _Tag10FieldNo;

            cmd.Parameters.Add("@SignFieldNo1", SqlDbType.Int);
            cmd.Parameters["@SignFieldNo1"].Value = _SignFieldNo1;

            cmd.Parameters.Add("@SignDateFieldNo1", SqlDbType.Int);
            cmd.Parameters["@SignDateFieldNo1"].Value = _SignDateFieldNo1;

            cmd.Parameters.Add("@SignFieldNo2", SqlDbType.Int);
            cmd.Parameters["@SignFieldNo2"].Value = _SignFieldNo2;

            cmd.Parameters.Add("@SignDateFieldNo2", SqlDbType.Int);
            cmd.Parameters["@SignDateFieldNo2"].Value = _SignDateFieldNo2;

            cmd.Parameters.Add("@SignFieldNo3", SqlDbType.Int);
            cmd.Parameters["@SignFieldNo3"].Value = _SignFieldNo3;

            cmd.Parameters.Add("@SignDateFieldNo3", SqlDbType.Int);
            cmd.Parameters["@SignDateFieldNo3"].Value = _SignDateFieldNo3;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To update <doc_type_mast> in gridview using stored procedure <DocTypeMast_Update>
        /// </summary>
        /// <returns></returns>
        public string UpdateDocType(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("DocTypeMast_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = _DocTypeID;

            cmd.Parameters.Add("@doc_type_name", SqlDbType.NVarChar, 255);
            cmd.Parameters["@doc_type_name"].Value = _DocTypeName;

            cmd.Parameters.Add("@tag1", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag1"].Value = _Tag1;

            cmd.Parameters.Add("@tag2", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag2"].Value = _Tag2;

            cmd.Parameters.Add("@tag3", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag3"].Value = _Tag3;

            cmd.Parameters.Add("@tag4", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag4"].Value = _Tag4;

            cmd.Parameters.Add("@tag5", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag5"].Value = _Tag5;

            cmd.Parameters.Add("@tag6", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag6"].Value = _Tag6;

            cmd.Parameters.Add("@tag7", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag7"].Value = _Tag7;

            cmd.Parameters.Add("@tag8", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag8"].Value = _Tag8;

            cmd.Parameters.Add("@tag9", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag9"].Value = _Tag9;

            cmd.Parameters.Add("@tag10", SqlDbType.NVarChar, 50);
            cmd.Parameters["@tag10"].Value = _Tag10;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

            SqlParameter param = cmd.Parameters.Add("@iApplicationID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            Utility.CloseConnection(con);

            return param.Value.ToString();
        }

        /// <summary>
        /// To delete <doc_type_mast> in gridview using stored procedure <DocTypeMast_Del>
        /// </summary>
        /// <returns></returns>
        public string DeleteDocType(string CompCode)
        {
            SqlConnection con = Utility.GetConnection();
            cmd = new SqlCommand("DocTypeMast_Del", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@doc_type_id", SqlDbType.NVarChar, 10);
            cmd.Parameters["@doc_type_id"].Value = _DocTypeID;

            cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
            cmd.Parameters["@CompCode"].Value = CompCode;

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