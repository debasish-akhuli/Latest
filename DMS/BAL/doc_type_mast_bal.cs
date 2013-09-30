using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DMS.DAL;
using System.Data;

namespace DMS.BAL
{
    public class doc_type_mast_bal
    {
        // Variable Declaration
        #region
        doc_type_mast_dal OBJ_DocTypeDAL;
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
        /// This function is used to insert record into the database.
        /// For this, the <DocTypeCode>,<DocTypeName>,<Tag1>,<Tag2>,<Tag3>,<Tag4>,<Tag5>,<Tag6>,<Tag7>,<Tag8>,<Tag9>,<Tag10> are set 
        /// in the Data Access Layer (DAL) of <doc_type_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string InsertDocTypeMast()
        {
            OBJ_DocTypeDAL = new doc_type_mast_dal();
            OBJ_DocTypeDAL.DocTypeID = this._DocTypeID;
            OBJ_DocTypeDAL.DocTypeName = this._DocTypeName;
            OBJ_DocTypeDAL.Tag1 = this._Tag1;
            OBJ_DocTypeDAL.Tag2 = this._Tag2;
            OBJ_DocTypeDAL.Tag3 = this._Tag3;
            OBJ_DocTypeDAL.Tag4 = this._Tag4;
            OBJ_DocTypeDAL.Tag5 = this._Tag5;
            OBJ_DocTypeDAL.Tag6 = this._Tag6;
            OBJ_DocTypeDAL.Tag7 = this._Tag7;
            OBJ_DocTypeDAL.Tag8 = this._Tag8;
            OBJ_DocTypeDAL.Tag9 = this._Tag9;
            OBJ_DocTypeDAL.Tag10 = this._Tag10;
            OBJ_DocTypeDAL.Tag1FieldNo = this._Tag1FieldNo;
            OBJ_DocTypeDAL.Tag2FieldNo = this._Tag2FieldNo;
            OBJ_DocTypeDAL.Tag3FieldNo = this._Tag3FieldNo;
            OBJ_DocTypeDAL.Tag4FieldNo = this._Tag4FieldNo;
            OBJ_DocTypeDAL.Tag5FieldNo = this._Tag5FieldNo;
            OBJ_DocTypeDAL.Tag6FieldNo = this._Tag6FieldNo;
            OBJ_DocTypeDAL.Tag7FieldNo = this._Tag7FieldNo;
            OBJ_DocTypeDAL.Tag8FieldNo = this._Tag8FieldNo;
            OBJ_DocTypeDAL.Tag9FieldNo = this._Tag9FieldNo;
            OBJ_DocTypeDAL.Tag10FieldNo = this._Tag10FieldNo;
            OBJ_DocTypeDAL.SignFieldNo1 = this._SignFieldNo1;
            OBJ_DocTypeDAL.SignDateFieldNo1 = this._SignDateFieldNo1;
            OBJ_DocTypeDAL.SignFieldNo2 = this._SignFieldNo2;
            OBJ_DocTypeDAL.SignDateFieldNo2 = this._SignDateFieldNo2;
            OBJ_DocTypeDAL.SignFieldNo3 = this._SignFieldNo3;
            OBJ_DocTypeDAL.SignDateFieldNo3 = this._SignDateFieldNo3;

            return OBJ_DocTypeDAL.InsertDocTypeMast();
        }

        /// <summary>
        /// To update <doc_type_mast> in gridview using stored procedure, pass the values to <doc_type_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string UpdateDocType(string CompCode)
        {
            OBJ_DocTypeDAL = new doc_type_mast_dal();
            OBJ_DocTypeDAL.DocTypeID = this._DocTypeID;
            OBJ_DocTypeDAL.DocTypeName = this._DocTypeName;
            OBJ_DocTypeDAL.Tag1 = this._Tag1;
            OBJ_DocTypeDAL.Tag2 = this._Tag2;
            OBJ_DocTypeDAL.Tag3 = this._Tag3;
            OBJ_DocTypeDAL.Tag4 = this._Tag4;
            OBJ_DocTypeDAL.Tag5 = this._Tag5;
            OBJ_DocTypeDAL.Tag6 = this._Tag6;
            OBJ_DocTypeDAL.Tag7 = this._Tag7;
            OBJ_DocTypeDAL.Tag8 = this._Tag8;
            OBJ_DocTypeDAL.Tag9 = this._Tag9;
            OBJ_DocTypeDAL.Tag10 = this._Tag10;

            return OBJ_DocTypeDAL.UpdateDocType(CompCode);
        }

        /// <summary>
        /// To delete <doc_type_mast> in gridview using stored procedure, pass the values to <doc_type_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string DeleteDocType(string CompCode)
        {
            OBJ_DocTypeDAL = new doc_type_mast_dal();
            OBJ_DocTypeDAL.DocTypeID = this._DocTypeID;

            return OBJ_DocTypeDAL.DeleteDocType(CompCode);
        }
        #endregion
    }
}