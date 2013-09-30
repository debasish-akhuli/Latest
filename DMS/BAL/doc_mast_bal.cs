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
    public class doc_mast_bal
    {
        // Variable Declaration
        #region
        doc_mast_dal Obj_DocMastDAL;
        private string _UserID = "";
        private string _WFLogID = "";
        private Int64 _DocID = 0;
        private Int64 _WFID = 0;
        private DateTime _Start_Dt = DateTime.Now;
        private DateTime _Due_Dt = DateTime.Now;
        static DateTime _Calc_Due_Dt = DateTime.Now;
        private string _Duration = "";
        private int _StepNo = 0;
        private string _TaskID = "";
        private string _DocName = "";
        private string _DocDesc = "";
        private string _DocTypeCode = "";
        private string _DeptCode = "";
        private string _CabinetCode = "";
        private string _DrawerCode = "";
        private string _FolderCode = "";
        private string _Upld_By = "";
        private DateTime _Upld_Dt = DateTime.Now;
        public string _Tag1 = "";
        public string _Tag2 = "";
        public string _Tag3 = "";
        public string _Tag4 = "";
        public string _Tag5 = "";
        public string _Tag6 = "";
        public string _Tag7 = "";
        public string _Tag8 = "";
        public string _Tag9 = "";
        public string _Tag10 = "";
        public string _Download_Path = "";
        public string _Doc_Path = "";
        public string _UUID = "";
        public string _AmbleMails = "";
        public string _AmbleMsg = "";
        public string _AmbleAttach = "";
        public string _AppendDoc = "";
        public string _AmbleURL = "";
        public string _AmbleSub = "";
        #endregion

        //Property Declaration
        #region
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
        public String WFLogID
        {
            get
            {
                return _WFLogID;
            }
            set
            {
                _WFLogID = value;
            }
        }
        public String Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
            }
        }
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
        public Int64 WFID
        {
            get
            {
                return _WFID;
            }
            set
            {
                _WFID = value;
            }
        }
        public DateTime Start_Dt
        {
            get
            {
                return _Start_Dt;
            }
            set
            {
                _Start_Dt = value;
            }
        }
        public DateTime Due_Dt
        {
            get
            {
                return _Due_Dt;
            }
            set
            {
                _Due_Dt = value;
            }
        }
        static DateTime Calc_Due_Dt
        {
            get
            {
                return _Calc_Due_Dt;
            }
            set
            {
                _Calc_Due_Dt = value;
            }
        }
        public int StepNo
        {
            get
            {
                return _StepNo;
            }
            set
            {
                _StepNo = value;
            }
        }
        public string TaskID
        {
            get
            {
                return _TaskID;
            }
            set
            {
                _TaskID = value;
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
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please enter document name");
                }
                _DocName = value;
            }
        }
        public String DocDesc
        {
            get
            {
                return _DocDesc;
            }
            set
            {
                _DocDesc = value;
            }
        }
        public String DocTypeCode
        {
            get
            {
                return _DocTypeCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select document type");
                }
                _DocTypeCode = value;
            }
        }
        public String DeptCode
        {
            get
            {
                return _DeptCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select department");
                }
                _DeptCode = value;
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
                    throw new Exception("Please select a cabinet");
                }
                _CabinetCode = value;
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
                    throw new Exception("Please select a drawer");
                }
                _DrawerCode = value;
            }
        }
        public String FolderCode
        {
            get
            {
                return _FolderCode;
            }
            set
            {
                // validate the input
                if (string.IsNullOrEmpty(value))
                {
                    // throw exception, or do whatever
                    throw new Exception("Please select a folder");
                }
                _FolderCode = value;
            }
        }
        public String Upld_By
        {
            get
            {
                return _Upld_By;
            }
            set
            {
                _Upld_By = value;
            }
        }
        public DateTime Upld_Dt
        {
            get
            {
                return _Upld_Dt;
            }
            set
            {
                _Upld_Dt = value;
            }
        }
        public String Tag1
        {
            get
            {
                return _Tag1;
            }
            set
            {
                _Tag1 = value;
            }
        }
        public String Tag2
        {
            get
            {
                return _Tag2;
            }
            set
            {
                _Tag2 = value;
            }
        }
        public String Tag3
        {
            get
            {
                return _Tag3;
            }
            set
            {
                _Tag3 = value;
            }
        }
        public String Tag4
        {
            get
            {
                return _Tag4;
            }
            set
            {
                _Tag4 = value;
            }
        }
        public String Tag5
        {
            get
            {
                return _Tag5;
            }
            set
            {
                _Tag5 = value;
            }
        }
        public String Tag6
        {
            get
            {
                return _Tag6;
            }
            set
            {
                _Tag6 = value;
            }
        }
        public String Tag7
        {
            get
            {
                return _Tag7;
            }
            set
            {
                _Tag7 = value;
            }
        }
        public String Tag8
        {
            get
            {
                return _Tag8;
            }
            set
            {
                _Tag8 = value;
            }
        }
        public String Tag9
        {
            get
            {
                return _Tag9;
            }
            set
            {
                _Tag9 = value;
            }
        }
        public String Tag10
        {
            get
            {
                return _Tag10;
            }
            set
            {
                _Tag10 = value;
            }
        }
        public String Download_Path
        {
            get
            {
                return _Download_Path;
            }
            set
            {
                _Download_Path = value;
            }
        }
        public String Doc_Path
        {
            get
            {
                return _Doc_Path;
            }
            set
            {
                _Doc_Path = value;
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
        public String AmbleMails
        {
            get
            {
                return _AmbleMails;
            }
            set
            {
                _AmbleMails = value;
            }
        }
        public String AmbleMsg
        {
            get
            {
                return _AmbleMsg;
            }
            set
            {
                _AmbleMsg = value;
            }
        }
        public String AmbleAttach
        {
            get
            {
                return _AmbleAttach;
            }
            set
            {
                _AmbleAttach = value;
            }
        }
        public String AmbleURL
        {
            get
            {
                return _AmbleURL;
            }
            set
            {
                _AmbleURL = value;
            }
        }
        public String AmbleSub
        {
            get
            {
                return _AmbleSub;
            }
            set
            {
                _AmbleSub = value;
            }
        }
        public String AppendDoc
        {
            get
            {
                return _AppendDoc;
            }
            set
            {
                _AppendDoc = value;
            }
        }
        #endregion

        // Method Declaration
        #region

        public string ExistDoc()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.DocName = this._DocName;
            Obj_DocMastDAL.FolderCode = this._FolderCode;

            return Obj_DocMastDAL.ExistDoc();
        }

        public string ExistDoc4Sniffer()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.DocName = this._DocName;

            return Obj_DocMastDAL.ExistDoc4Sniffer();
        }

        //public string InsertDocMast()
        //{
        //    Obj_DocMastDAL = new doc_mast_dal();
        //    Obj_DocMastDAL.DocName = this._DocName;
        //    Obj_DocMastDAL.DocDesc = this._DocDesc;
        //    Obj_DocMastDAL.DocTypeCode = this._DocTypeCode;
        //    Obj_DocMastDAL.DeptCode = this._DeptCode;
        //    Obj_DocMastDAL.FolderCode = this._FolderCode;
        //    Obj_DocMastDAL.Upld_By = this._Upld_By;
        //    Obj_DocMastDAL.Upld_Dt = this._Upld_Dt;
        //    Obj_DocMastDAL.Tag1 = this._Tag1;
        //    Obj_DocMastDAL.Tag2 = this._Tag2;
        //    Obj_DocMastDAL.Tag3 = this._Tag3;
        //    Obj_DocMastDAL.Tag4 = this._Tag4;
        //    Obj_DocMastDAL.Tag5 = this._Tag5;
        //    Obj_DocMastDAL.Tag6 = this._Tag6;
        //    Obj_DocMastDAL.Tag7 = this._Tag7;
        //    Obj_DocMastDAL.Tag8 = this._Tag8;
        //    Obj_DocMastDAL.Tag9 = this._Tag9;
        //    Obj_DocMastDAL.Tag10 = this._Tag10;
        //    Obj_DocMastDAL.Download_Path = this._Download_Path;
        //    Obj_DocMastDAL.Doc_Path = this._Doc_Path;
        //    Obj_DocMastDAL.UUID = this._UUID;

        //    return Obj_DocMastDAL.InsertDocMast();
        //}

        /// <summary>
        /// This function is used to set the corresponding Tags with respect to Doc Type
        /// </summary>
        /// <returns></returns>
        public DataSet FetchTags()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.DocTypeCode = this._DocTypeCode;

            return Obj_DocMastDAL.FetchTags();
        }

        /// <summary>
        /// This function is used to set the corresponding Tags with respect to Doc Type
        /// </summary>
        /// <returns></returns>
        public string DefinedWF()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.DocTypeCode = this._DocTypeCode;
            Obj_DocMastDAL.DeptCode = this._DeptCode;

            return Obj_DocMastDAL.DefinedWF();
        }

        /// <summary>
        /// This function is used to start the default workflow according to the department & doc type combination.
        /// For this, the <DocID>,<WFID>,<Start_Dt> are set in the Data Access Layer (DAL) of <doc_mast_dal>
        /// </summary>
        /// <returns></returns>
        public string StartDefaultWF()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.DocTypeCode = this._DocTypeCode;
            Obj_DocMastDAL.DeptCode = this._DeptCode;
            Obj_DocMastDAL.DocID = this._DocID;
            Obj_DocMastDAL.WFID = this._WFID;
            Obj_DocMastDAL.Start_Dt = this._Start_Dt;
            Obj_DocMastDAL.UserID = this._UserID;

            return Obj_DocMastDAL.StartDefaultWF();
        }

        /// <summary>
        /// This function is used to select the corresponding Roles with respect to Workflow
        /// </summary>
        /// <returns></returns>
        public DataSet SelectWFDtls()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.WFID = this.WFID;

            return Obj_DocMastDAL.SelectWFDtls();
        }

        /// <summary>
        /// This function is used to select the corresponding Tasks with respect to Workflow
        /// </summary>
        /// <returns></returns>
        public DataSet SelectWFTasks()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.WFID = this.WFID;

            return Obj_DocMastDAL.SelectWFTasks();
        }

        /// <summary>
        /// This function is used to insert the corresponding Roles with respect to Workflow
        /// </summary>
        /// <returns></returns>
        public string StartDefaultWFLogDtl()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.WFLogID = this._WFLogID;
            Obj_DocMastDAL.StepNo = this._StepNo;
            if (Obj_DocMastDAL.StepNo == 1)
            {
                Obj_DocMastDAL.Start_Dt = _Start_Dt;
                _Calc_Due_Dt = _Start_Dt;
            }
            else
            {
                Obj_DocMastDAL.Start_Dt = _Calc_Due_Dt;
            }
            Obj_DocMastDAL.Due_Dt = _Calc_Due_Dt.AddHours(Convert.ToDouble(this._Duration));
            _Calc_Due_Dt = Obj_DocMastDAL.Due_Dt;
            return Obj_DocMastDAL.StartDefaultWFLogDtl();
        }

        /// <summary>
        /// This function is used to insert the corresponding Tasks with respect to Workflow
        /// </summary>
        /// <returns></returns>
        public string StartDefaultWFLogTask()
        {
            Obj_DocMastDAL = new doc_mast_dal();
            Obj_DocMastDAL.WFLogID = this.WFLogID;
            Obj_DocMastDAL.StepNo = this.StepNo;
            Obj_DocMastDAL.TaskID = this.TaskID;
            Obj_DocMastDAL.AmbleMails = this.AmbleMails;
            Obj_DocMastDAL.AmbleMsg = this.AmbleMsg;
            Obj_DocMastDAL.AmbleAttach = this.AmbleAttach;
            Obj_DocMastDAL.AppendDoc = this.AppendDoc;
            Obj_DocMastDAL.AmbleURL = this.AmbleURL;
            Obj_DocMastDAL.AmbleSub = this.AmbleSub;

            return Obj_DocMastDAL.StartDefaultWFLogTask();
        }
        #endregion
    }
}