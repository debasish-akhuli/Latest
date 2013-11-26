using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using DMS.UTILITY;
using QuickPDFDLL0813;
using System.Net;
using System.Configuration;
using DMS.Actions;
using DynamicFormula;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;

namespace DMS
{
    public partial class eFormWFL : System.Web.UI.Page
    {
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;
        private RepositoryService repoServiceA;

        public RepositoryService RepoService
        {
            set { repoService = value; }
        }
        public RepositoryService RepoServiceA
        {
            set { repoServiceA = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Header.DataBind();
                if (!IsPostBack)
                {
                    // Set the session variables blank which are used to set the previous selected path start
                    Session["SelectedCabUUID"] = "";
                    Session["SelectedDrwUUID"] = "";
                    Session["SelectedFldUUID"] = "";
                    Session["SelectedDocID"] = "";
                    // Set the session variables blank which are used to set the previous selected path end
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = true;
                            if (Session["CanChangePwd"].ToString() == "N")
                            {
                                menuGenHome.Visible = false;
                                menuGenSystem.Visible = false;
                            }
                            else
                            {
                                menuGenHome.Visible = true;
                                menuGenSystem.Visible = true;
                            }
                        }
                        lblUser.Text = Session["UserFullName"].ToString();
                        PopForTaskUpdate();
                        DisplayApprovals();
                        Session["SelDocUUID"] = Request.QueryString["DocUUID"].ToString();
                        Session["StageNo"] = lblStage.Text;
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                }
                PopulateFormDetails(Request.QueryString["DocUUID"].ToString());
                PopulateDesign();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopForTaskUpdate()
        {
            try
            {
                lblWFID.Text = Session["WFUpdtWFLogID"].ToString();
                lblWFName.Text = Session["WFUpdtWFName"].ToString();
                lblStage.Text = Session["WFUpdtStepNo"].ToString();
                lblAssignedDt.Text = Session["WFUpdtAssignDt"].ToString();
                lblDueDt.Text = Session["WFUpdtDueDt"].ToString();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.UserInfoPassingUserID(Session["WFUpdtAssignBy"].ToString());
                lblAssignedBy.Text = ds01.Tables[0].Rows[0][1].ToString() + " " + ds01.Tables[0].Rows[0][2].ToString() + " (" + ds01.Tables[0].Rows[0][3].ToString() + ")";

                lblDocName.Text = Session["WFUpdtDocName"].ToString();
                PopulateDropdown();
                TagDisplay(Session["AttachedDocUUID"].ToString());
            }
            catch (Exception ex)
            {

            }
        }

        protected void PopulateDropdown()
        {
            try
            {
                DBClass DBObj = new DBClass();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();

                //....Task
                DBObj.WF_Log_ID = lblWFID.Text;
                DBObj.Step_No = Convert.ToInt32(lblStage.Text);
                ds01.Reset();
                ds01 = DBObj.DDAssignedTask();
                ddTask.DataSource = ds01;
                ddTask.DataTextField = "task_name";
                ddTask.DataValueField = "task_id";
                ddTask.DataBind();
                //for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                //{
                //    if (ds01.Tables[0].Rows[i][0].ToString() == "REVIEW")
                //    {
                //        //spanCheckOut.Visible = false;
                //        //spanCheckIn.Visible = false;
                //    }
                //}

                //....Workflow
                ds01.Reset();
                ds01 = DBObj.DDWorkflow(Session["CompCode"].ToString());
                ddWFName.DataSource = ds01;
                ddWFName.DataTextField = "wf_name";
                ddWFName.DataValueField = "wf_id";
                ddWFName.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void DisplayApprovals()
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();

                Int64 WFID = 0;
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataSet ds03 = new DataSet();

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Stage No", typeof(string));
                dt1.Columns.Add("User Name", typeof(string));
                dt1.Columns.Add("Date", typeof(DateTime));
                dt1.Columns.Add("Comment", typeof(string));
                dt1.Columns.Add("Action", typeof(string));

                cmd = new SqlCommand("select a.wf_id,b.doc_name from wf_log_mast a, doc_mast b where a.doc_id=b.doc_id and a.wf_log_id='" + lblWFID.Text + "'", con);
                ds01 = new DataSet();
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);

                if (ds01.Tables[0].Rows.Count > 0)
                {
                    DataSet ds0001 = new DataSet();
                    cmd = new SqlCommand("select doc_name from doc_mast where uuid in(select ActualDocUUID from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "')", con);
                    SqlDataAdapter adapter0001 = new SqlDataAdapter(cmd);
                    adapter0001.Fill(ds0001);
                    //lblAttachedDocName.Text = ds0001.Tables[0].Rows[0][0].ToString();
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        WFID = Convert.ToInt64(ds01.Tables[0].Rows[i][0].ToString());
                        cmd = new SqlCommand("select a.StepNo,b.task_name,a.TaskDoneDate,a.Comments,a.UserID from WFLog a,task_mast b where a.TaskID=b.task_id and a.TaskID not like('PRE%') and a.TaskID not like('POST%') and (a.TaskDoneDate!='Not Required' and a.TaskDoneDate!='Waiting') and a.WFLogID='" + lblWFID.Text + "'", con);
                        ds02 = new DataSet();
                        SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                        adapter02.Fill(ds02);

                        if (ds02.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds02.Tables[0].Rows.Count; j++)
                            {
                                DataRow r = dt1.NewRow();
                                r["Stage No"] = ds02.Tables[0].Rows[j][0].ToString();
                                r["Date"] = ds02.Tables[0].Rows[j][2].ToString();
                                r["Comment"] = ds02.Tables[0].Rows[j][3].ToString();

                                DataSet ds0002 = new DataSet();
                                cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo='" + ds02.Tables[0].Rows[j][0].ToString() + "'", con);
                                SqlDataAdapter adapter0002 = new SqlDataAdapter(cmd);
                                adapter0002.Fill(ds0002);
                                if (ds0002.Tables[0].Rows[0][4].ToString() == "Yes")
                                {
                                    r["Action"] = ds02.Tables[0].Rows[j][1].ToString() + " & Uploaded a New Document";
                                }
                                else
                                {
                                    r["Action"] = ds02.Tables[0].Rows[j][1].ToString();
                                }
                                cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id='" + ds02.Tables[0].Rows[j][4].ToString() + "'", con);
                                ds03 = new DataSet();
                                SqlDataAdapter adapter03 = new SqlDataAdapter(cmd);
                                adapter03.Fill(ds03);

                                if (ds03.Tables[0].Rows.Count > 0)
                                {
                                    r["User Name"] = ds03.Tables[0].Rows[0][0].ToString();
                                }
                                dt1.Rows.Add(r);
                            }
                        }
                    }
                }
                Utility.CloseConnection(con);

                gvComment.DataSource = dt1;
                gvComment.DataBind();
                dt1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void TagDisplay(string DocUUID)
        {
            try
            {
                //SqlConnection con = Utility.GetConnection();
                //SqlCommand cmd = null;
                //DataSet ds01 = new DataSet();
                //DataSet ds02 = new DataSet();
                //string SelItem = "";

                //con.Open();
                //cmd = new SqlCommand("select doc_type_id,tag1,tag2,tag3,tag4,tag5,tag6,tag7,tag8,tag9,tag10 from doc_mast where uuid='" + DocUUID + "'", con);
                //SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                //adapter01.Fill(ds01);
                //Utility.CloseConnection(con);
                //if (ds01.Tables[0].Rows.Count > 0)
                //{
                //    SelItem = ds01.Tables[0].Rows[0][0].ToString();
                //}
                //else
                //{
                //    throw new Exception("No Document Type Assigned for This Document.");
                //}

                //doc_mast_bal Obj_DocMastBAL = new doc_mast_bal();
                //Obj_DocMastBAL.DocTypeCode = SelItem;

                //DataSet ds1 = new DataSet();
                //ds1 = Obj_DocMastBAL.FetchTags();
                //if (ds1.Tables[0].Rows.Count > 0)
                //{
                //    /// For Tag1
                //    if (ds1.Tables[0].Rows[0]["tag1"].ToString() == "" || ds1.Tables[0].Rows[0]["tag1"] == null)
                //    {
                //        txtTag1.Text = "";
                //    }
                //    else
                //    {
                //        txtTag1.Text = ds01.Tables[0].Rows[0][1].ToString();
                //    }
                //    /// For Tag2
                //    if (ds1.Tables[0].Rows[0]["tag2"].ToString() == "" || ds1.Tables[0].Rows[0]["tag2"] == null)
                //    {
                //        txtTag2.Text = "";
                //    }
                //    else
                //    {
                //        txtTag2.Text = ds01.Tables[0].Rows[0][2].ToString();
                //    }
                //    /// For Tag3
                //    if (ds1.Tables[0].Rows[0]["tag3"].ToString() == "" || ds1.Tables[0].Rows[0]["tag3"] == null)
                //    {
                //        txtTag3.Text = "";
                //    }
                //    else
                //    {
                //        txtTag3.Text = ds01.Tables[0].Rows[0][3].ToString();
                //    }
                //    /// For Tag4
                //    if (ds1.Tables[0].Rows[0]["tag4"].ToString() == "" || ds1.Tables[0].Rows[0]["tag4"] == null)
                //    {
                //        txtTag4.Text = "";
                //    }
                //    else
                //    {
                //        txtTag4.Text = ds01.Tables[0].Rows[0][4].ToString();
                //    }
                //    /// For Tag5
                //    if (ds1.Tables[0].Rows[0]["tag5"].ToString() == "" || ds1.Tables[0].Rows[0]["tag5"] == null)
                //    {
                //        txtTag5.Text = "";
                //    }
                //    else
                //    {
                //        txtTag5.Text = ds01.Tables[0].Rows[0][5].ToString();
                //    }
                //    /// For Tag6
                //    if (ds1.Tables[0].Rows[0]["tag6"].ToString() == "" || ds1.Tables[0].Rows[0]["tag6"] == null)
                //    {
                //        txtTag6.Text = "";
                //    }
                //    else
                //    {
                //        txtTag6.Text = ds01.Tables[0].Rows[0][6].ToString();
                //    }
                //    /// For Tag7
                //    if (ds1.Tables[0].Rows[0]["tag7"].ToString() == "" || ds1.Tables[0].Rows[0]["tag7"] == null)
                //    {
                //        txtTag7.Text = "";
                //    }
                //    else
                //    {
                //        txtTag7.Text = ds01.Tables[0].Rows[0][7].ToString();
                //    }
                //    /// For Tag8
                //    if (ds1.Tables[0].Rows[0]["tag8"].ToString() == "" || ds1.Tables[0].Rows[0]["tag8"] == null)
                //    {
                //        txtTag8.Text = "";
                //    }
                //    else
                //    {
                //        txtTag8.Text = ds01.Tables[0].Rows[0][8].ToString();
                //    }
                //    /// For Tag9
                //    if (ds1.Tables[0].Rows[0]["tag9"].ToString() == "" || ds1.Tables[0].Rows[0]["tag9"] == null)
                //    {
                //        txtTag9.Text = "";
                //    }
                //    else
                //    {
                //        txtTag9.Text = ds01.Tables[0].Rows[0][9].ToString();
                //    }
                //    /// For Tag10
                //    if (ds1.Tables[0].Rows[0]["tag10"].ToString() == "" || ds1.Tables[0].Rows[0]["tag10"] == null)
                //    {
                //        txtTag10.Text = "";
                //    }
                //    else
                //    {
                //        txtTag10.Text = ds01.Tables[0].Rows[0][10].ToString();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected string GenNewDocName(string DocName)
        {
            try
            {
                string WithoutVerDocName = "";
                string NewDocName = "";
                string DocVersion = "";
                string NewDocVersion = "";
                if (DocName.LastIndexOf("_V0") != -1)
                {
                    WithoutVerDocName = DocName.Substring(0, DocName.Length - 4);
                    DocVersion = DocName.Substring(WithoutVerDocName.Length + 2, 2);
                    NewDocVersion = (Convert.ToInt32(DocVersion) + 1).ToString().PadLeft(2, '0');
                    NewDocName = WithoutVerDocName + "_V" + NewDocVersion;
                }
                else
                {
                    NewDocName = DocName + "_V01";
                }
                return NewDocName;
            }
            catch (Exception ex)
            {
                return "Error V";
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                string TravellingDocName = "";
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                string UpldFolderUUID = "";
                string DocID = "";
                DataSet ds001 = new DataSet();
                DataSet ds002 = new DataSet();
                DataSet ds003 = new DataSet();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = "";
                doc_mast_bal Obj_DocMast = new doc_mast_bal();
                UserRights RightsObj = new UserRights();
                DataSet dsPerm = new DataSet();
                SqlDataAdapter adpAvl01;
                DataSet dsAvl01 = new DataSet();
                double AvailableSpace = 0;
                DataSet ds01 = new DataSet();

                con.Open();
                cmd = new SqlCommand("select fld_uuid from wf_mast where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + lblWFID.Text + "')", con);
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                UpldFolderUUID = ds001.Tables[0].Rows[0][0].ToString();

                DataSet ds0001 = new DataSet();
                cmd = new SqlCommand("select * from doc_mast where uuid in(select ActualDocUUID from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "')", con);
                SqlDataAdapter adapter0001 = new SqlDataAdapter(cmd);
                adapter0001.Fill(ds0001);
                TravellingDocName = ds0001.Tables[0].Rows[0][1].ToString();
                if (ds0001.Tables[0].Rows[0][18].ToString() == "Check Out")
                {
                    if (btnBrowseNewVer.FileName == null || btnBrowseNewVer.FileName.Equals(""))
                    {
                        throw new Exception("Please Check In with a Document and then Proceed");
                    }
                }

                // If Reject
                if (ddTask.SelectedValue == "REJECT")
                {

                }
                else
                {
                    //New Version Uploading Part Start
                    //if (hfSelCheckIn.Value == "OptNew")
                    //{
                    //    UploadRevisedVersion();
                    //}
                    //New Version Uploading Part End

                    cmd = new SqlCommand("select wf_id,doc_id from wf_log_mast where wf_log_id='" + lblWFID.Text + "'", con);
                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                    adapter002.Fill(ds002);
                    DocID = ds002.Tables[0].Rows[0][1].ToString();

                    string fileName = GenNewDocName(TravellingDocName);
                    SqlDataAdapter adapterDocExist;
                    DataSet dsDocExist = new DataSet();
                    cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                    adapterDocExist = new SqlDataAdapter(cmd);
                    dsDocExist.Reset();
                    adapterDocExist.Fill(dsDocExist);
                    if (dsDocExist.Tables[0].Rows.Count > 0)
                    {
                        fileName = GenNewDocName(fileName);
                    }
                    cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                    adapterDocExist = new SqlDataAdapter(cmd);
                    dsDocExist.Reset();
                    adapterDocExist.Fill(dsDocExist);
                    if (dsDocExist.Tables[0].Rows.Count > 0)
                    {
                        fileName = GenNewDocName(fileName);
                    }
                    cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                    adapterDocExist = new SqlDataAdapter(cmd);
                    dsDocExist.Reset();
                    adapterDocExist.Fill(dsDocExist);
                    if (dsDocExist.Tables[0].Rows.Count > 0)
                    {
                        fileName = GenNewDocName(fileName);
                    }
                    cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                    adapterDocExist = new SqlDataAdapter(cmd);
                    dsDocExist.Reset();
                    adapterDocExist.Fill(dsDocExist);
                    if (dsDocExist.Tables[0].Rows.Count > 0)
                    {
                        fileName = GenNewDocName(fileName);
                    }
                    cmd = new SqlCommand("select * from doc_mast where doc_name='" + fileName + "'", con);
                    adapterDocExist = new SqlDataAdapter(cmd);
                    dsDocExist.Reset();
                    adapterDocExist.Fill(dsDocExist);
                    if (dsDocExist.Tables[0].Rows.Count > 0)
                    {
                        fileName = GenNewDocName(fileName);
                    }

                    //if (File.Exists(Server.MapPath("TempDownload") + "\\" + fileName))
                    //{
                    //    File.Delete(Server.MapPath("TempDownload") + "\\" + fileName);
                    //}

                    //File.Copy(Server.MapPath("TempDownload") + "\\" + hfDocument.Value, Server.MapPath("TempDownload") + "\\" + fileName);
                    Session["RWFDocName"] = fileName;
                    string AutoGenID = Guid.NewGuid().ToString();

                    SaveDocANDMetaValues(AutoGenID,Session["eFormTemplateUUID"].ToString(),Session["CompCode"].ToString());


                    ////Initialise the reference to the spaces store
                    //Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                    //spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    //spacesStore.address = "SpacesStore";

                    //// Create the parent reference, the company home folder
                    //Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
                    //parentReference.store = spacesStore;
                    //parentReference.uuid = UpldFolderUUID; // Folder's uuid

                    //parentReference.associationType = Constants.ASSOC_CONTAINS;
                    //parentReference.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, fileName);

                    //// Create the properties list
                    //NamedValue nameProperty = new NamedValue();
                    //nameProperty.name = Constants.PROP_NAME;
                    //nameProperty.value = fileName;
                    //nameProperty.isMultiValue = false;

                    //NamedValue[] properties = new NamedValue[2];
                    //properties[0] = nameProperty;
                    //nameProperty = new NamedValue();
                    //nameProperty.name = Constants.PROP_TITLE;
                    //nameProperty.value = fileName;
                    //nameProperty.isMultiValue = false;
                    //properties[1] = nameProperty;

                    //// Create the CML create object
                    //CMLCreate create = new CMLCreate();
                    //create.parent = parentReference;
                    //create.id = "1";
                    //create.type = Constants.TYPE_CONTENT;
                    //create.property = properties;

                    //// Create and execute the cml statement
                    //CML cml = new CML();
                    //cml.create = new CMLCreate[] { create };
                    //// Get the repository service
                    //// Admin Credentials start
                    //WebServiceFactory wsFA = new WebServiceFactory();
                    //wsFA.UserName = Session["AdmUserID"].ToString();
                    //wsFA.Ticket = Session["AdmTicket"].ToString();
                    //this.repoServiceA = wsFA.getRepositoryService();
                    //// Admin Credentials end
                    //UpdateResult[] updateResult = repoServiceA.update(cml);

                    //// work around to cast Alfresco.RepositoryWebService.Reference to
                    //// Alfresco.ContentWebService.Reference 
                    //Alfresco.RepositoryWebService.Reference rwsRef = updateResult[0].destination;
                    //Alfresco.ContentWebService.Reference newContentNode = new Alfresco.ContentWebService.Reference();
                    //newContentNode.path = rwsRef.path;
                    //newContentNode.uuid = rwsRef.uuid;
                    //Alfresco.ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
                    //cwsStore.address = "SpacesStore";
                    //spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    //newContentNode.store = cwsStore;

                    //bytes = StreamFile(Server.MapPath("TempDownload") + "\\" + Session["RWFDocName"].ToString());
                    //Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
                    //FileType ObjFileType = new FileType();
                    //contentFormat.mimetype = ObjFileType.GetFileType(ObjFetchOnlyNameORExtension.FetchOnlyDocExt(hfDocument.Value));

                    //WebServiceFactory wsF = new WebServiceFactory();
                    //wsF.UserName = Session["AdmUserID"].ToString();
                    //wsF.Ticket = Session["AdmTicket"].ToString();
                    //wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);

                    //Upload the document into the DB
                    cmd = new SqlCommand("select * from doc_mast where doc_id='" + DocID + "'", con);
                    SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                    adapter003.Fill(ds003);

                    ds01.Reset();
                    Obj_DocMast.DocName = Session["RWFDocName"].ToString();
                    Obj_DocMast.DocTypeCode = ds003.Tables[0].Rows[0][2].ToString();
                    Obj_DocMast.DeptCode = ds003.Tables[0].Rows[0][3].ToString();
                    Obj_DocMast._UUID = AutoGenID;
                    Obj_DocMast.FolderCode = UpldFolderUUID;
                    Obj_DocMast.Upld_By = Session["UserID"].ToString();
                    Obj_DocMast.Upld_Dt = DateTime.Now;
                    Obj_DocMast.Tag1 = ds003.Tables[0].Rows[0][8].ToString();
                    Obj_DocMast.Tag2 = ds003.Tables[0].Rows[0][9].ToString();
                    Obj_DocMast.Tag3 = ds003.Tables[0].Rows[0][10].ToString();
                    Obj_DocMast.Tag4 = ds003.Tables[0].Rows[0][11].ToString();
                    Obj_DocMast.Tag5 = ds003.Tables[0].Rows[0][12].ToString();
                    Obj_DocMast.Tag6 = ds003.Tables[0].Rows[0][13].ToString();
                    Obj_DocMast.Tag7 = ds003.Tables[0].Rows[0][14].ToString();
                    Obj_DocMast.Tag8 = ds003.Tables[0].Rows[0][15].ToString();
                    Obj_DocMast.Tag9 = ds003.Tables[0].Rows[0][16].ToString();
                    Obj_DocMast.Tag10 = ds003.Tables[0].Rows[0][17].ToString();
                    Obj_DocMast._Doc_Path = AutoGenID + "/" + fileName.Replace(" ", "%20");
                    Obj_DocMast.DocDesc = ds003.Tables[0].Rows[0][20].ToString(); //Session["RWFDocName"].ToString();

                    result = ObjClassStoreProc.InsertDocMast(Obj_DocMast.DocName, Obj_DocMast.DocDesc, Obj_DocMast.FolderCode, Obj_DocMast.DocTypeCode, Obj_DocMast.DeptCode, Obj_DocMast.Upld_By, Obj_DocMast.Upld_Dt, Obj_DocMast.Tag1, Obj_DocMast.Tag2, Obj_DocMast.Tag3, Obj_DocMast.Tag4, Obj_DocMast.Tag5, Obj_DocMast.Tag6, Obj_DocMast.Tag7, Obj_DocMast.Tag8, Obj_DocMast.Tag9, Obj_DocMast.Tag10, "", Obj_DocMast._UUID, Obj_DocMast._Doc_Path, "", Session["CompCode"].ToString(), Convert.ToDouble(ds003.Tables[0].Rows[0][24].ToString()));

                    cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + Convert.ToDouble(ds003.Tables[0].Rows[0][24].ToString()) + "' where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    #region Validation of space if it is exceeding or not
                    cmd = new SqlCommand("select TotalSpace,UsedSpace,AvailableSpace from ServerConfig where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    adpAvl01 = new SqlDataAdapter(cmd);
                    dsAvl01.Reset();
                    adpAvl01.Fill(dsAvl01);
                    AvailableSpace = Convert.ToDouble(dsAvl01.Tables[0].Rows[0][2].ToString());
                    if (AvailableSpace < 0)
                    {
                        string SenderMail = "";
                        string SenderName = "";
                        string SmtpHost = "";
                        Int32 SmtpPort = 0;
                        string CredenUsername = "";
                        string CredenPwd = "";
                        string MailSub = "";
                        string MailMsg = "";
                        string MailTo = "";
                        string MailFrom = "";
                        string ContactPersonName = "";
                        string CompName = "";
                        mailing ObjMailSetup = new mailing();
                        ds01.Reset();
                        ds01 = ObjMailSetup.MailSettings();
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            SenderMail = ds01.Tables[0].Rows[0][0].ToString();
                            SenderName = ds01.Tables[0].Rows[0][1].ToString();
                            SmtpHost = ds01.Tables[0].Rows[0][2].ToString();
                            SmtpPort = Convert.ToInt32(ds01.Tables[0].Rows[0][3].ToString());
                            CredenUsername = ds01.Tables[0].Rows[0][4].ToString();
                            CredenPwd = ds01.Tables[0].Rows[0][5].ToString();
                        }
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectServerConfig("00000000");
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            MailFrom = ds01.Tables[0].Rows[0][8].ToString();
                        }
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            MailTo = ds01.Tables[0].Rows[0][8].ToString();
                            ContactPersonName = ds01.Tables[0].Rows[0][7].ToString();
                            CompName = ds01.Tables[0].Rows[0][3].ToString();
                        }

                        MailSub = "Alert: Storage Space Exceeded";
                        MailMsg = "Hello " + ContactPersonName + ",<br/><br/>You do not have storage space to upload any further Document.<br/>Please contact your System Administrator to increase the storage space or to free space by deleting old Documents.<br/><br/>Thanks,<br/>System Administrator.";
                        mailing Obj_Mail = new mailing();
                        if (Obj_Mail.SendEmail("", "admin", MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                        {
                            MailMsg = "Hello,<br/>" + CompName + " [" + Session["CompCode"].ToString() + "] has exceeded the using of storage space.<br/><br/>Thanks,<br/>System Administrator.";
                            if (Obj_Mail.SendEmail("", "admin", MailFrom, MailFrom, "", MailFrom, MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                            {

                            }
                        }
                    }
                    #endregion

                    dsPerm.Reset();
                    dsPerm = RightsObj.FetchPermission(UpldFolderUUID, Session["CompCode"].ToString());
                    if (dsPerm.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(Obj_DocMast.UUID, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                        }
                    }

                    DataTable dtFields = null;
                    dtFields = (DataTable)Session["dtFields"];
                    //InsertMetaTags(Obj_DocMast._UUID, Session["RWFDocName"].ToString());
                    #region Update Metadata Fields
                    //con.Open();
                    DataSet ds0003 = new DataSet();
                    cmd = new SqlCommand("select * from doc_type_mast where doc_type_id in(select doc_type_id from doc_mast where uuid='" + Request.QueryString["DocUUID"].ToString() + "') and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    SqlDataAdapter adapter0003 = new SqlDataAdapter(cmd);
                    adapter0003.Fill(ds0003);
                    if (ds0003.Tables[0].Rows.Count > 0)
                    {
                        // Here 12 to 21 are the field nos in the database
                        // For Tag1
                        if (ds0003.Tables[0].Rows[0][12].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][12].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][12].ToString()) - 1]["ControlID"].ToString());
                                txtTag1.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][12].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][12].ToString()) - 1]["ControlID"].ToString());
                                txtTag1.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag1.Text = "";
                        }

                        // For Tag2
                        if (ds0003.Tables[0].Rows[0][13].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][13].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][13].ToString()) - 1]["ControlID"].ToString());
                                txtTag2.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][13].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][13].ToString()) - 1]["ControlID"].ToString());
                                txtTag2.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag2.Text = "";
                        }

                        // For Tag3
                        if (ds0003.Tables[0].Rows[0][14].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][14].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][14].ToString()) - 1]["ControlID"].ToString());
                                txtTag3.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][14].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][14].ToString()) - 1]["ControlID"].ToString());
                                txtTag3.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag3.Text = "";
                        }

                        // For Tag4
                        if (ds0003.Tables[0].Rows[0][15].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][15].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][15].ToString()) - 1]["ControlID"].ToString());
                                txtTag4.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][15].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][15].ToString()) - 1]["ControlID"].ToString());
                                txtTag4.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag4.Text = "";
                        }

                        // For Tag5
                        if (ds0003.Tables[0].Rows[0][16].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][16].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][16].ToString()) - 1]["ControlID"].ToString());
                                txtTag5.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][16].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][16].ToString()) - 1]["ControlID"].ToString());
                                txtTag5.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag5.Text = "";
                        }

                        // For Tag6
                        if (ds0003.Tables[0].Rows[0][17].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][17].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][17].ToString()) - 1]["ControlID"].ToString());
                                txtTag6.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][17].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][17].ToString()) - 1]["ControlID"].ToString());
                                txtTag6.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag6.Text = "";
                        }

                        // For Tag7
                        if (ds0003.Tables[0].Rows[0][18].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][18].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][18].ToString()) - 1]["ControlID"].ToString());
                                txtTag7.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][18].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][18].ToString()) - 1]["ControlID"].ToString());
                                txtTag7.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag7.Text = "";
                        }

                        // For Tag8
                        if (ds0003.Tables[0].Rows[0][19].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][19].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][19].ToString()) - 1]["ControlID"].ToString());
                                txtTag8.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][19].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][19].ToString()) - 1]["ControlID"].ToString());
                                txtTag8.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag8.Text = "";
                        }

                        // For Tag9
                        if (ds0003.Tables[0].Rows[0][20].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][20].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][20].ToString()) - 1]["ControlID"].ToString());
                                txtTag9.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][20].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][20].ToString()) - 1]["ControlID"].ToString());
                                txtTag9.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag9.Text = "";
                        }

                        // For Tag10
                        if (ds0003.Tables[0].Rows[0][21].ToString() != "0")
                        {
                            if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][21].ToString()) - 1]["ControlType"].ToString() == "T")
                            {
                                TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][21].ToString()) - 1]["ControlID"].ToString());
                                txtTag10.Text = tb.Text;
                            }
                            else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][21].ToString()) - 1]["ControlType"].ToString() == "D")
                            {
                                DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][21].ToString()) - 1]["ControlID"].ToString());
                                txtTag10.Text = ddL.Text;
                            }
                        }
                        else
                        {
                            txtTag10.Text = "";
                        }

                    }
                    //con.Close();
                    #endregion

                    cmd = new SqlCommand("select top 1 * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and NewDocUUID!='' and StepNo<='" + lblStage.Text + "' order by StepNo desc", con);
                    SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                    ds01.Reset();
                    adapter01.Fill(ds01);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Obj_DocMast._UUID + "',ActualDocUUID='" + ds01.Tables[0].Rows[0][3].ToString() + "' where WFLogID='" + lblWFID.Text + "' and StepNo='" + lblStage.Text + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + Obj_DocMast._UUID + "' where WFLogID='" + lblWFID.Text + "' and StepNo>'" + lblStage.Text + "'", con);
                        cmd.ExecuteNonQuery();
                        DataSet dsV01 = new DataSet();
                        cmd = new SqlCommand("select * from WFDoc where WFLogID='" + lblWFID.Text + "' and DocUUID='" + Obj_DocMast._UUID + "'", con);
                        SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                        adapterV01.Fill(dsV01);
                        if (dsV01.Tables[0].Rows.Count > 0)
                        {

                        }
                        else
                        {
                            cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + lblWFID.Text + "','" + Obj_DocMast._UUID + "','" + Session["CompCode"].ToString() + "')", con);
                            cmd.ExecuteNonQuery();
                            DataSet dsV02 = new DataSet();
                            cmd = new SqlCommand("select doc_id from doc_mast where uuid ='" + Obj_DocMast._UUID + "'", con);
                            SqlDataAdapter adapterV02 = new SqlDataAdapter(cmd);
                            adapterV02.Fill(dsV02);
                            cmd = new SqlCommand("update wf_log_mast set doc_id='" + dsV02.Tables[0].Rows[0][0].ToString() + "' where wf_log_id='" + lblWFID.Text + "'", con);
                            cmd.ExecuteNonQuery();
                            //Invisible the Older versions
                            DataSet dsV001 = new DataSet();
                            cmd = new SqlCommand("select * from WFDoc where WFLogID='" + lblWFID.Text + "' and DocUUID!='" + Obj_DocMast._UUID + "' order by AutoID desc", con);
                            SqlDataAdapter adapterV001 = new SqlDataAdapter(cmd);
                            adapterV001.Fill(dsV001);
                            if (dsV001.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsV001.Tables[0].Rows.Count; k++)
                                {
                                    RightsObj.UpdatePermissions4Doc(dsV001.Tables[0].Rows[k][2].ToString(), "Document", "", "X");
                                }
                            }
                        }
                        // Invisible the Older versions end
                    }

                    // Delete the file as the work has been done
                    //File.Delete(Server.MapPath("TempDownload") + "\\" + hfDocument.Value);
                    //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["RWFDocName"].ToString());
                }
                userhome_bal UserHomeObj = new userhome_bal();
                /// Update this task in workflow execution using Store Procedure <WFLogTaskDtl_Update> start
                UserHomeObj.WFLogID = lblWFID.Text;
                UserHomeObj.StepNo = Convert.ToInt32(lblStage.Text);
                UserHomeObj.TaskDone_Dt = DateTime.Now;
                UserHomeObj.Task_ID = ddTask.SelectedValue;
                UserHomeObj.Comments = txtComments.Text.Trim();
                CheckAction(UserHomeObj.WFLogID, UserHomeObj.StepNo, UserHomeObj.TaskDone_Dt, UserHomeObj.Task_ID, UserHomeObj.Comments);

                SqlDataAdapter adp01 = new SqlDataAdapter();
                DataSet ds00001 = new DataSet();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd = new SqlCommand("select * from wf_log_mast where wf_log_id='" + lblWFID.Text + "'", con);
                ds00001.Reset();
                adp01 = new SqlDataAdapter(cmd);
                adp01.Fill(ds00001);
                if (ds00001.Tables[0].Rows[0][8].ToString() == "Completed")
                {
                    #region Convert the eForm to a PDF and save
                    //cmd = new SqlCommand("select * from wf_log_mast where wf_log_id='" + lblWFID.Text + "'", con);
                    //adp01 = new SqlDataAdapter(cmd);
                    string DocUUID = "";
                    string DocName = "";
                    string FolderUUID = "";
                    double FileSize = 0;
                    string AttachedDocID = "";

                    AttachedDocID = ds00001.Tables[0].Rows[0][1].ToString();
                    ds00001.Reset();
                    ds00001 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(AttachedDocID), Session["CompCode"].ToString());
                    DocUUID = ds00001.Tables[0].Rows[0][4].ToString();
                    DocName = ds00001.Tables[0].Rows[0][1].ToString();
                    FolderUUID = ds00001.Tables[0].Rows[0][5].ToString();

                    #region PDF Creation
                    Font font8 = FontFactory.GetFont("ARIAL", 10);
                    DataTable dt;
                    dt = new DataTable();
                    dt = CreateDT4PDF(DocUUID);

                    string FileName = "";
                    FileName = DocName + ".pdf";
                    result = ObjClassStoreProc.ExistDoc(FileName, FolderUUID, Session["CompCode"].ToString());
                    if (Convert.ToInt32(result) == -1)
                    {
                        FileName = DocName + "_" + GetTimestamp(DateTime.Now) + ".pdf";
                    }

                    string TotStr = DocName;
                    Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 42, 35);
                    PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("CreatedPDFs") + "\\" + FileName, FileMode.Create));
                    Paragraph paragraph = new Paragraph(TotStr);
                    paragraph.Alignment = Element.ALIGN_CENTER;
                    paragraph.Font.SetColor(0, 0, 255);
                    paragraph.Font.Size = 14;
                    doc.Open();
                    doc.Add(paragraph);

                    if (dt != null)
                    {
                        PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
                        PdfPCell PdfPCell = null;
                        for (int rows = 0; rows < dt.Rows.Count; rows++)
                        {
                            for (int column = 0; column < dt.Columns.Count; column++)
                            {
                                PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
                                PdfTable.AddCell(PdfPCell);
                                PdfPCell.Border = 1;
                            }
                        }
                        PdfTable.SpacingBefore = 35f; // Give some space after the text or it may overlap the table
                        doc.Add(PdfTable);
                    }
                    doc.Close();

                    //string filepath = Server.MapPath("CreatedPDFs") + "\\" + FileName;
                    //FileInfo myfile = new FileInfo(filepath);
                    //if (myfile.Exists)
                    //{
                    //    Response.ClearContent();
                    //    Response.AddHeader("Content-Disposition", "attachment; filename=" + myfile.Name.Replace(" ", "_"));
                    //    Response.AddHeader("Content-Length", myfile.Length.ToString());
                    //    Response.ContentType = ReturnExtension(myfile.Extension.ToLower());
                    //    Response.TransmitFile(myfile.FullName);
                    //    //Response.End();
                    //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //}
                    #endregion

                    #region Upload the PDF in Alfresco
                    byte[] bytes;

                    Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                    spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    spacesStore.address = "SpacesStore";

                    // Create the parent reference, the company home folder
                    Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
                    parentReference.store = spacesStore;
                    parentReference.uuid = FolderUUID; // Folder's uuid

                    parentReference.associationType = Constants.ASSOC_CONTAINS;
                    parentReference.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, FileName);

                    // Create the properties list
                    NamedValue nameProperty = new NamedValue();
                    nameProperty.name = Constants.PROP_NAME;
                    nameProperty.value = FileName;
                    nameProperty.isMultiValue = false;

                    NamedValue[] properties = new NamedValue[2];
                    properties[0] = nameProperty;
                    nameProperty = new NamedValue();
                    nameProperty.name = Constants.PROP_TITLE;
                    nameProperty.value = FileName;
                    nameProperty.isMultiValue = false;
                    properties[1] = nameProperty;

                    // Create the CML create object
                    CMLCreate create = new CMLCreate();
                    create.parent = parentReference;
                    create.id = "1";
                    create.type = Constants.TYPE_CONTENT;
                    create.property = properties;

                    // Create and execute the cml statement
                    CML cml = new CML();
                    cml.create = new CMLCreate[] { create };
                    // Get the repository service
                    // Admin Credentials start
                    WebServiceFactory wsF = new WebServiceFactory();
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    this.repoServiceA = wsF.getRepositoryService();
                    // Admin Credentials end
                    UpdateResult[] updateResult = repoServiceA.update(cml);

                    // work around to cast Alfresco.RepositoryWebService.Reference to
                    // Alfresco.ContentWebService.Reference 
                    Alfresco.RepositoryWebService.Reference rwsRef = updateResult[0].destination;
                    Alfresco.ContentWebService.Reference newContentNode = new Alfresco.ContentWebService.Reference();
                    newContentNode.path = rwsRef.path;
                    newContentNode.uuid = rwsRef.uuid;
                    Alfresco.ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
                    cwsStore.address = "SpacesStore";
                    spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    newContentNode.store = cwsStore;

                    bytes = StreamFile(Server.MapPath("CreatedPDFs") + "\\" + FileName);
                    Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
                    FileType ObjFileType = new FileType();
                    contentFormat.mimetype = ObjFileType.GetFileType(ObjFetchOnlyNameORExtension.FetchOnlyDocExt(FileName));

                    wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);
                    #endregion

                    FileSize = Math.Round(Convert.ToDouble(bytes.Length) / 1024, 2);

                    #region Update the SQL Server Data
                    #region Update ServerConfig & Insert into doc_mast
                    string LastInsertedDocID = "";
                    Obj_DocMast.DocName = FileName;
                    Obj_DocMast.DocTypeCode = ds00001.Tables[0].Rows[0][2].ToString();
                    Obj_DocMast.DeptCode = ds00001.Tables[0].Rows[0][3].ToString();
                    Obj_DocMast._UUID = newContentNode.uuid;
                    Obj_DocMast.FolderCode = FolderUUID;
                    Obj_DocMast.Upld_By = Session["UserID"].ToString();
                    Obj_DocMast.Upld_Dt = DateTime.Now;
                    Obj_DocMast.Tag1 = ds00001.Tables[0].Rows[0][8].ToString();
                    Obj_DocMast.Tag2 = ds00001.Tables[0].Rows[0][9].ToString();
                    Obj_DocMast.Tag3 = ds00001.Tables[0].Rows[0][10].ToString();
                    Obj_DocMast.Tag4 = ds00001.Tables[0].Rows[0][11].ToString();
                    Obj_DocMast.Tag5 = ds00001.Tables[0].Rows[0][12].ToString();
                    Obj_DocMast.Tag6 = ds00001.Tables[0].Rows[0][13].ToString();
                    Obj_DocMast.Tag7 = ds00001.Tables[0].Rows[0][14].ToString();
                    Obj_DocMast.Tag8 = ds00001.Tables[0].Rows[0][15].ToString();
                    Obj_DocMast.Tag9 = ds00001.Tables[0].Rows[0][16].ToString();
                    Obj_DocMast.Tag10 = ds00001.Tables[0].Rows[0][17].ToString();
                    Obj_DocMast._Doc_Path = newContentNode.uuid + "/" + FileName.Replace(" ", "%20");
                    Obj_DocMast.DocDesc = ds00001.Tables[0].Rows[0][20].ToString();

                    result = ObjClassStoreProc.InsertDocMast(Obj_DocMast.DocName, Obj_DocMast.DocDesc, Obj_DocMast.FolderCode, Obj_DocMast.DocTypeCode, Obj_DocMast.DeptCode, Obj_DocMast.Upld_By, Obj_DocMast.Upld_Dt, Obj_DocMast.Tag1, Obj_DocMast.Tag2, Obj_DocMast.Tag3, Obj_DocMast.Tag4, Obj_DocMast.Tag5, Obj_DocMast.Tag6, Obj_DocMast.Tag7, Obj_DocMast.Tag8, Obj_DocMast.Tag9, Obj_DocMast.Tag10, "", Obj_DocMast._UUID, Obj_DocMast._Doc_Path, "W", Session["CompCode"].ToString(), FileSize);
                    LastInsertedDocID = result;
                    cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + FileSize + "' where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    #region Validation of space if it is exceeding or not
                    cmd = new SqlCommand("select TotalSpace,UsedSpace,AvailableSpace from ServerConfig where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    adpAvl01 = new SqlDataAdapter(cmd);
                    dsAvl01.Reset();
                    adpAvl01.Fill(dsAvl01);
                    AvailableSpace = Convert.ToDouble(dsAvl01.Tables[0].Rows[0][2].ToString());
                    if (AvailableSpace < 0)
                    {
                        string SenderMail = "";
                        string SenderName = "";
                        string SmtpHost = "";
                        Int32 SmtpPort = 0;
                        string CredenUsername = "";
                        string CredenPwd = "";
                        string MailSub = "";
                        string MailMsg = "";
                        string MailTo = "";
                        string MailFrom = "";
                        string ContactPersonName = "";
                        string CompName = "";
                        mailing ObjMailSetup = new mailing();
                        ds01.Reset();
                        ds01 = ObjMailSetup.MailSettings();
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            SenderMail = ds01.Tables[0].Rows[0][0].ToString();
                            SenderName = ds01.Tables[0].Rows[0][1].ToString();
                            SmtpHost = ds01.Tables[0].Rows[0][2].ToString();
                            SmtpPort = Convert.ToInt32(ds01.Tables[0].Rows[0][3].ToString());
                            CredenUsername = ds01.Tables[0].Rows[0][4].ToString();
                            CredenPwd = ds01.Tables[0].Rows[0][5].ToString();
                        }
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectServerConfig("00000000");
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            MailFrom = ds01.Tables[0].Rows[0][8].ToString();
                        }
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            MailTo = ds01.Tables[0].Rows[0][8].ToString();
                            ContactPersonName = ds01.Tables[0].Rows[0][7].ToString();
                            CompName = ds01.Tables[0].Rows[0][3].ToString();
                        }

                        MailSub = "Alert: Storage Space Exceeded";
                        MailMsg = "Hello " + ContactPersonName + ",<br/><br/>You do not have storage space to upload any further Document.<br/>Please contact your System Administrator to increase the storage space or to free space by deleting old Documents.<br/><br/>Thanks,<br/>System Administrator.";
                        mailing Obj_Mail = new mailing();
                        if (Obj_Mail.SendEmail("", "admin", MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                        {
                            MailMsg = "Hello,<br/>" + CompName + " [" + Session["CompCode"].ToString() + "] has exceeded the using of storage space.<br/><br/>Thanks,<br/>System Administrator.";
                            if (Obj_Mail.SendEmail("", "admin", MailFrom, MailFrom, "", MailFrom, MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                            {

                            }
                        }
                    }
                    #endregion
                    #endregion

                    #region Insert & Update DocMetaValue
                    //string DBFldName = "";
                    cmd = new SqlCommand("select * from DocMetaValue where uuid='" + DocUUID + "'", con);
                    adp01 = new SqlDataAdapter(cmd);
                    ds00001.Reset();
                    adp01.Fill(ds00001);
                    if (ds00001.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("insert into DocMetaValue values('" + Obj_DocMast._UUID + "', '" + ds00001.Tables[0].Rows[0][1].ToString() + "','" + ds00001.Tables[0].Rows[0][2].ToString() + "','" + ds00001.Tables[0].Rows[0][3].ToString() + "','" + ds00001.Tables[0].Rows[0][4].ToString() + "','" + ds00001.Tables[0].Rows[0][5].ToString() + "','" + ds00001.Tables[0].Rows[0][6].ToString() + "','" + ds00001.Tables[0].Rows[0][7].ToString() + "','" + ds00001.Tables[0].Rows[0][8].ToString() + "','" + ds00001.Tables[0].Rows[0][9].ToString() + "','" + ds00001.Tables[0].Rows[0][10].ToString() + "','" + ds00001.Tables[0].Rows[0][11].ToString() + "','" + ds00001.Tables[0].Rows[0][12].ToString() + "','" + ds00001.Tables[0].Rows[0][13].ToString() + "','" + ds00001.Tables[0].Rows[0][14].ToString() + "','" + ds00001.Tables[0].Rows[0][15].ToString() + "','" + ds00001.Tables[0].Rows[0][16].ToString() + "','" + ds00001.Tables[0].Rows[0][17].ToString() + "','" + ds00001.Tables[0].Rows[0][18].ToString() + "','" + ds00001.Tables[0].Rows[0][19].ToString() + "','" + ds00001.Tables[0].Rows[0][20].ToString() + "','" + ds00001.Tables[0].Rows[0][21].ToString() + "','" + ds00001.Tables[0].Rows[0][22].ToString() + "','" + ds00001.Tables[0].Rows[0][23].ToString() + "','" + ds00001.Tables[0].Rows[0][24].ToString() + "','" + ds00001.Tables[0].Rows[0][25].ToString() + "','" + ds00001.Tables[0].Rows[0][26].ToString() + "','" + ds00001.Tables[0].Rows[0][27].ToString() + "','" + ds00001.Tables[0].Rows[0][28].ToString() + "','" + ds00001.Tables[0].Rows[0][29].ToString() + "','" + ds00001.Tables[0].Rows[0][30].ToString() + "','" + ds00001.Tables[0].Rows[0][31].ToString() + "','" + ds00001.Tables[0].Rows[0][32].ToString() + "','" + ds00001.Tables[0].Rows[0][33].ToString() + "','" + ds00001.Tables[0].Rows[0][34].ToString() + "','" + ds00001.Tables[0].Rows[0][35].ToString() + "','" + ds00001.Tables[0].Rows[0][36].ToString() + "','" + ds00001.Tables[0].Rows[0][37].ToString() + "','" + ds00001.Tables[0].Rows[0][38].ToString() + "','" + ds00001.Tables[0].Rows[0][39].ToString() + "','" + ds00001.Tables[0].Rows[0][40].ToString() + "','" + ds00001.Tables[0].Rows[0][41].ToString() + "','" + ds00001.Tables[0].Rows[0][42].ToString() + "','" + ds00001.Tables[0].Rows[0][43].ToString() + "','" + ds00001.Tables[0].Rows[0][44].ToString() + "','" + ds00001.Tables[0].Rows[0][45].ToString() + "','" + ds00001.Tables[0].Rows[0][46].ToString() + "','" + ds00001.Tables[0].Rows[0][47].ToString() + "','" + ds00001.Tables[0].Rows[0][48].ToString() + "','" + ds00001.Tables[0].Rows[0][49].ToString() + "','" + ds00001.Tables[0].Rows[0][50].ToString() + "','" + ds00001.Tables[0].Rows[0][51].ToString() + "','" + ds00001.Tables[0].Rows[0][52].ToString() + "','" + ds00001.Tables[0].Rows[0][53].ToString() + "','" + ds00001.Tables[0].Rows[0][54].ToString() + "','" + ds00001.Tables[0].Rows[0][55].ToString() + "','" + ds00001.Tables[0].Rows[0][56].ToString() + "','" + ds00001.Tables[0].Rows[0][57].ToString() + "','" + ds00001.Tables[0].Rows[0][58].ToString() + "','" + ds00001.Tables[0].Rows[0][59].ToString() + "','" + ds00001.Tables[0].Rows[0][60].ToString() + "','" + ds00001.Tables[0].Rows[0][61].ToString() + "','" + ds00001.Tables[0].Rows[0][62].ToString() + "','" + ds00001.Tables[0].Rows[0][63].ToString() + "','" + ds00001.Tables[0].Rows[0][64].ToString() + "','" + ds00001.Tables[0].Rows[0][65].ToString() + "','" + ds00001.Tables[0].Rows[0][66].ToString() + "','" + ds00001.Tables[0].Rows[0][67].ToString() + "','" + ds00001.Tables[0].Rows[0][68].ToString() + "','" + ds00001.Tables[0].Rows[0][69].ToString() + "','" + ds00001.Tables[0].Rows[0][70].ToString() + "','" + ds00001.Tables[0].Rows[0][71].ToString() + "','" + ds00001.Tables[0].Rows[0][72].ToString() + "','" + ds00001.Tables[0].Rows[0][73].ToString() + "','" + ds00001.Tables[0].Rows[0][74].ToString() + "','" + ds00001.Tables[0].Rows[0][75].ToString() + "','" + ds00001.Tables[0].Rows[0][76].ToString() + "','" + ds00001.Tables[0].Rows[0][77].ToString() + "','" + ds00001.Tables[0].Rows[0][78].ToString() + "','" + ds00001.Tables[0].Rows[0][79].ToString() + "','" + ds00001.Tables[0].Rows[0][80].ToString() + "','" + ds00001.Tables[0].Rows[0][81].ToString() + "','" + ds00001.Tables[0].Rows[0][82].ToString() + "','" + ds00001.Tables[0].Rows[0][83].ToString() + "','" + ds00001.Tables[0].Rows[0][84].ToString() + "','" + ds00001.Tables[0].Rows[0][85].ToString() + "','" + ds00001.Tables[0].Rows[0][86].ToString() + "','" + ds00001.Tables[0].Rows[0][87].ToString() + "','" + ds00001.Tables[0].Rows[0][88].ToString() + "','" + ds00001.Tables[0].Rows[0][89].ToString() + "','" + ds00001.Tables[0].Rows[0][90].ToString() + "','" + ds00001.Tables[0].Rows[0][91].ToString() + "','" + ds00001.Tables[0].Rows[0][92].ToString() + "','" + ds00001.Tables[0].Rows[0][93].ToString() + "','" + ds00001.Tables[0].Rows[0][94].ToString() + "','" + ds00001.Tables[0].Rows[0][95].ToString() + "','" + ds00001.Tables[0].Rows[0][96].ToString() + "','" + ds00001.Tables[0].Rows[0][97].ToString() + "','" + ds00001.Tables[0].Rows[0][98].ToString() + "','" + ds00001.Tables[0].Rows[0][99].ToString() + "','" + ds00001.Tables[0].Rows[0][100].ToString() + "', '" + ds00001.Tables[0].Rows[0][101].ToString() + "','" + ds00001.Tables[0].Rows[0][102].ToString() + "','" + ds00001.Tables[0].Rows[0][103].ToString() + "','" + ds00001.Tables[0].Rows[0][104].ToString() + "','" + ds00001.Tables[0].Rows[0][105].ToString() + "','" + ds00001.Tables[0].Rows[0][106].ToString() + "','" + ds00001.Tables[0].Rows[0][107].ToString() + "','" + ds00001.Tables[0].Rows[0][108].ToString() + "','" + ds00001.Tables[0].Rows[0][109].ToString() + "','" + ds00001.Tables[0].Rows[0][110].ToString() + "','" + ds00001.Tables[0].Rows[0][111].ToString() + "','" + ds00001.Tables[0].Rows[0][112].ToString() + "','" + ds00001.Tables[0].Rows[0][113].ToString() + "','" + ds00001.Tables[0].Rows[0][114].ToString() + "','" + ds00001.Tables[0].Rows[0][115].ToString() + "','" + ds00001.Tables[0].Rows[0][116].ToString() + "','" + ds00001.Tables[0].Rows[0][117].ToString() + "','" + ds00001.Tables[0].Rows[0][118].ToString() + "','" + ds00001.Tables[0].Rows[0][119].ToString() + "','" + ds00001.Tables[0].Rows[0][120].ToString() + "','" + ds00001.Tables[0].Rows[0][121].ToString() + "','" + ds00001.Tables[0].Rows[0][122].ToString() + "','" + ds00001.Tables[0].Rows[0][123].ToString() + "','" + ds00001.Tables[0].Rows[0][124].ToString() + "','" + ds00001.Tables[0].Rows[0][125].ToString() + "','" + ds00001.Tables[0].Rows[0][126].ToString() + "','" + ds00001.Tables[0].Rows[0][127].ToString() + "','" + ds00001.Tables[0].Rows[0][128].ToString() + "','" + ds00001.Tables[0].Rows[0][129].ToString() + "','" + ds00001.Tables[0].Rows[0][130].ToString() + "','" + ds00001.Tables[0].Rows[0][131].ToString() + "','" + ds00001.Tables[0].Rows[0][132].ToString() + "','" + ds00001.Tables[0].Rows[0][133].ToString() + "','" + ds00001.Tables[0].Rows[0][134].ToString() + "','" + ds00001.Tables[0].Rows[0][135].ToString() + "','" + ds00001.Tables[0].Rows[0][136].ToString() + "','" + ds00001.Tables[0].Rows[0][137].ToString() + "','" + ds00001.Tables[0].Rows[0][138].ToString() + "','" + ds00001.Tables[0].Rows[0][139].ToString() + "','" + ds00001.Tables[0].Rows[0][140].ToString() + "','" + ds00001.Tables[0].Rows[0][141].ToString() + "','" + ds00001.Tables[0].Rows[0][142].ToString() + "','" + ds00001.Tables[0].Rows[0][143].ToString() + "','" + ds00001.Tables[0].Rows[0][144].ToString() + "','" + ds00001.Tables[0].Rows[0][145].ToString() + "','" + ds00001.Tables[0].Rows[0][146].ToString() + "','" + ds00001.Tables[0].Rows[0][147].ToString() + "','" + ds00001.Tables[0].Rows[0][148].ToString() + "','" + ds00001.Tables[0].Rows[0][149].ToString() + "','" + ds00001.Tables[0].Rows[0][150].ToString() + "','" + ds00001.Tables[0].Rows[0][151].ToString() + "','" + ds00001.Tables[0].Rows[0][152].ToString() + "','" + ds00001.Tables[0].Rows[0][153].ToString() + "','" + ds00001.Tables[0].Rows[0][154].ToString() + "','" + ds00001.Tables[0].Rows[0][155].ToString() + "','" + ds00001.Tables[0].Rows[0][156].ToString() + "','" + ds00001.Tables[0].Rows[0][157].ToString() + "','" + ds00001.Tables[0].Rows[0][158].ToString() + "','" + ds00001.Tables[0].Rows[0][159].ToString() + "','" + ds00001.Tables[0].Rows[0][160].ToString() + "','" + ds00001.Tables[0].Rows[0][161].ToString() + "','" + ds00001.Tables[0].Rows[0][162].ToString() + "','" + ds00001.Tables[0].Rows[0][163].ToString() + "','" + ds00001.Tables[0].Rows[0][164].ToString() + "','" + ds00001.Tables[0].Rows[0][165].ToString() + "','" + ds00001.Tables[0].Rows[0][166].ToString() + "','" + ds00001.Tables[0].Rows[0][167].ToString() + "','" + ds00001.Tables[0].Rows[0][168].ToString() + "','" + ds00001.Tables[0].Rows[0][169].ToString() + "','" + ds00001.Tables[0].Rows[0][170].ToString() + "','" + ds00001.Tables[0].Rows[0][171].ToString() + "','" + ds00001.Tables[0].Rows[0][172].ToString() + "','" + ds00001.Tables[0].Rows[0][173].ToString() + "','" + ds00001.Tables[0].Rows[0][174].ToString() + "','" + ds00001.Tables[0].Rows[0][175].ToString() + "','" + ds00001.Tables[0].Rows[0][176].ToString() + "','" + ds00001.Tables[0].Rows[0][177].ToString() + "','" + ds00001.Tables[0].Rows[0][178].ToString() + "','" + ds00001.Tables[0].Rows[0][179].ToString() + "','" + ds00001.Tables[0].Rows[0][180].ToString() + "','" + ds00001.Tables[0].Rows[0][181].ToString() + "','" + ds00001.Tables[0].Rows[0][182].ToString() + "','" + ds00001.Tables[0].Rows[0][183].ToString() + "','" + ds00001.Tables[0].Rows[0][184].ToString() + "','" + ds00001.Tables[0].Rows[0][185].ToString() + "','" + ds00001.Tables[0].Rows[0][186].ToString() + "','" + ds00001.Tables[0].Rows[0][187].ToString() + "','" + ds00001.Tables[0].Rows[0][188].ToString() + "','" + ds00001.Tables[0].Rows[0][189].ToString() + "','" + ds00001.Tables[0].Rows[0][190].ToString() + "','" + ds00001.Tables[0].Rows[0][191].ToString() + "','" + ds00001.Tables[0].Rows[0][192].ToString() + "','" + ds00001.Tables[0].Rows[0][193].ToString() + "','" + ds00001.Tables[0].Rows[0][194].ToString() + "','" + ds00001.Tables[0].Rows[0][195].ToString() + "','" + ds00001.Tables[0].Rows[0][196].ToString() + "','" + ds00001.Tables[0].Rows[0][197].ToString() + "','" + ds00001.Tables[0].Rows[0][198].ToString() + "','" + ds00001.Tables[0].Rows[0][199].ToString() + "','" + ds00001.Tables[0].Rows[0][200].ToString() + "', '" + ds00001.Tables[0].Rows[0][201].ToString() + "','" + ds00001.Tables[0].Rows[0][202].ToString() + "','" + ds00001.Tables[0].Rows[0][203].ToString() + "','" + ds00001.Tables[0].Rows[0][204].ToString() + "','" + ds00001.Tables[0].Rows[0][205].ToString() + "','" + ds00001.Tables[0].Rows[0][206].ToString() + "','" + ds00001.Tables[0].Rows[0][207].ToString() + "','" + ds00001.Tables[0].Rows[0][208].ToString() + "','" + ds00001.Tables[0].Rows[0][209].ToString() + "','" + ds00001.Tables[0].Rows[0][210].ToString() + "','" + ds00001.Tables[0].Rows[0][211].ToString() + "','" + ds00001.Tables[0].Rows[0][212].ToString() + "','" + ds00001.Tables[0].Rows[0][213].ToString() + "','" + ds00001.Tables[0].Rows[0][214].ToString() + "','" + ds00001.Tables[0].Rows[0][215].ToString() + "','" + ds00001.Tables[0].Rows[0][216].ToString() + "','" + ds00001.Tables[0].Rows[0][217].ToString() + "','" + ds00001.Tables[0].Rows[0][218].ToString() + "','" + ds00001.Tables[0].Rows[0][219].ToString() + "','" + ds00001.Tables[0].Rows[0][220].ToString() + "','" + ds00001.Tables[0].Rows[0][221].ToString() + "','" + ds00001.Tables[0].Rows[0][222].ToString() + "','" + ds00001.Tables[0].Rows[0][223].ToString() + "','" + ds00001.Tables[0].Rows[0][224].ToString() + "','" + ds00001.Tables[0].Rows[0][225].ToString() + "','" + ds00001.Tables[0].Rows[0][226].ToString() + "','" + ds00001.Tables[0].Rows[0][227].ToString() + "','" + ds00001.Tables[0].Rows[0][228].ToString() + "','" + ds00001.Tables[0].Rows[0][229].ToString() + "','" + ds00001.Tables[0].Rows[0][230].ToString() + "','" + ds00001.Tables[0].Rows[0][231].ToString() + "','" + ds00001.Tables[0].Rows[0][232].ToString() + "','" + ds00001.Tables[0].Rows[0][233].ToString() + "','" + ds00001.Tables[0].Rows[0][234].ToString() + "','" + ds00001.Tables[0].Rows[0][235].ToString() + "','" + ds00001.Tables[0].Rows[0][236].ToString() + "','" + ds00001.Tables[0].Rows[0][237].ToString() + "','" + ds00001.Tables[0].Rows[0][238].ToString() + "','" + ds00001.Tables[0].Rows[0][239].ToString() + "','" + ds00001.Tables[0].Rows[0][240].ToString() + "','" + ds00001.Tables[0].Rows[0][241].ToString() + "','" + ds00001.Tables[0].Rows[0][242].ToString() + "','" + ds00001.Tables[0].Rows[0][243].ToString() + "','" + ds00001.Tables[0].Rows[0][244].ToString() + "','" + ds00001.Tables[0].Rows[0][245].ToString() + "','" + ds00001.Tables[0].Rows[0][246].ToString() + "','" + ds00001.Tables[0].Rows[0][247].ToString() + "','" + ds00001.Tables[0].Rows[0][248].ToString() + "','" + ds00001.Tables[0].Rows[0][249].ToString() + "','" + ds00001.Tables[0].Rows[0][250].ToString() + "','" + ds00001.Tables[0].Rows[0][251].ToString() + "','" + ds00001.Tables[0].Rows[0][252].ToString() + "','" + ds00001.Tables[0].Rows[0][253].ToString() + "','" + ds00001.Tables[0].Rows[0][254].ToString() + "','" + ds00001.Tables[0].Rows[0][255].ToString() + "','" + ds00001.Tables[0].Rows[0][256].ToString() + "','" + ds00001.Tables[0].Rows[0][257].ToString() + "','" + ds00001.Tables[0].Rows[0][258].ToString() + "','" + ds00001.Tables[0].Rows[0][259].ToString() + "','" + ds00001.Tables[0].Rows[0][260].ToString() + "','" + ds00001.Tables[0].Rows[0][261].ToString() + "','" + ds00001.Tables[0].Rows[0][262].ToString() + "','" + ds00001.Tables[0].Rows[0][263].ToString() + "','" + ds00001.Tables[0].Rows[0][264].ToString() + "','" + ds00001.Tables[0].Rows[0][265].ToString() + "','" + ds00001.Tables[0].Rows[0][266].ToString() + "','" + ds00001.Tables[0].Rows[0][267].ToString() + "','" + ds00001.Tables[0].Rows[0][268].ToString() + "','" + ds00001.Tables[0].Rows[0][269].ToString() + "','" + ds00001.Tables[0].Rows[0][270].ToString() + "','" + ds00001.Tables[0].Rows[0][271].ToString() + "','" + ds00001.Tables[0].Rows[0][272].ToString() + "','" + ds00001.Tables[0].Rows[0][273].ToString() + "','" + ds00001.Tables[0].Rows[0][274].ToString() + "','" + ds00001.Tables[0].Rows[0][275].ToString() + "','" + ds00001.Tables[0].Rows[0][276].ToString() + "','" + ds00001.Tables[0].Rows[0][277].ToString() + "','" + ds00001.Tables[0].Rows[0][278].ToString() + "','" + ds00001.Tables[0].Rows[0][279].ToString() + "','" + ds00001.Tables[0].Rows[0][280].ToString() + "','" + ds00001.Tables[0].Rows[0][281].ToString() + "','" + ds00001.Tables[0].Rows[0][282].ToString() + "','" + ds00001.Tables[0].Rows[0][283].ToString() + "','" + ds00001.Tables[0].Rows[0][284].ToString() + "','" + ds00001.Tables[0].Rows[0][285].ToString() + "','" + ds00001.Tables[0].Rows[0][286].ToString() + "','" + ds00001.Tables[0].Rows[0][287].ToString() + "','" + ds00001.Tables[0].Rows[0][288].ToString() + "','" + ds00001.Tables[0].Rows[0][289].ToString() + "','" + ds00001.Tables[0].Rows[0][290].ToString() + "','" + ds00001.Tables[0].Rows[0][291].ToString() + "','" + ds00001.Tables[0].Rows[0][292].ToString() + "','" + ds00001.Tables[0].Rows[0][293].ToString() + "','" + ds00001.Tables[0].Rows[0][294].ToString() + "','" + ds00001.Tables[0].Rows[0][295].ToString() + "','" + ds00001.Tables[0].Rows[0][296].ToString() + "','" + ds00001.Tables[0].Rows[0][297].ToString() + "','" + ds00001.Tables[0].Rows[0][298].ToString() + "','" + ds00001.Tables[0].Rows[0][299].ToString() + "','" + ds00001.Tables[0].Rows[0][300].ToString() + "','" + ds00001.Tables[0].Rows[0][301].ToString() + "','" + ds00001.Tables[0].Rows[0][302].ToString() + "','" + ds00001.Tables[0].Rows[0][303].ToString() + "','" + ds00001.Tables[0].Rows[0][304].ToString() + "')", con);
                        cmd.ExecuteNonQuery();
                        //result = ObjClassStoreProc.DocMetaValueInsert(Obj_DocMast._UUID, ds00001.Tables[0].Rows[0][1].ToString(), Session["CompCode"].ToString());
                        //cmd = new SqlCommand("update DocMetaValue set UpdatedBy='" + ds00001.Tables[0].Rows[0][2].ToString() + "',UpdatedOn='" + ds00001.Tables[0].Rows[0][3].ToString() + "' where uuid='" + Obj_DocMast._UUID + "'", con);
                        //cmd.ExecuteNonQuery();
                        //for (int i = 0; i < ds00001.Tables[0].Rows.Count; i++)
                        //{
                        //    DBFldName = "Tag" + (i + 1).ToString();
                        //    cmd = new SqlCommand("update DocMetaValue set " + DBFldName + "='" + TermValues[i].ToString() + "' where uuid='" + Obj_DocMast._UUID + "'", con);
                        //    cmd.ExecuteNonQuery();
                        //}
                    }
                    #endregion

                    #region Update UserRights
                    dsPerm.Reset();
                    dsPerm = RightsObj.FetchPermission(FolderUUID, Session["CompCode"].ToString());
                    if (dsPerm.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(Obj_DocMast.UUID, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                        }
                    }
                    cmd = new SqlCommand("select * from UserRights where NodeUUID='" + DocUUID + "' and UserID in(select user_id from user_mast where UserType='N')", con);
                    adp01 = new SqlDataAdapter(cmd);
                    ds00001.Reset();
                    adp01.Fill(ds00001);
                    if (ds00001.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds00001.Tables[0].Rows.Count; i++)
                        {
                            cmd = new SqlCommand("update UserRights set Permission='X' where NodeUUID='" + ds00001.Tables[0].Rows[i][0].ToString() + "' and UserID='" + ds00001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #region Update wf_log_mast
                    cmd = new SqlCommand("update wf_log_mast set doc_id='" + LastInsertedDocID + "' where wf_log_id='" + lblWFID.Text + "'", con);
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Update WFDocVersion
                    cmd = new SqlCommand("select top 1* from WFDocVersion where WFLogID='" + lblWFID.Text + "' order by StepNo desc", con);
                    adp01 = new SqlDataAdapter(cmd);
                    ds00001.Reset();
                    adp01.Fill(ds00001);
                    if (ds00001.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds00001.Tables[0].Rows.Count; i++)
                        {
                            cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Obj_DocMast.UUID + "' where WFLogID='" + lblWFID.Text + "' and StepNo='" + ds00001.Tables[0].Rows[i][1].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #region Update WFDoc
                    cmd = new SqlCommand("update WFDoc set DocUUID='" + Obj_DocMast.UUID + "' where WFLogID='" + lblWFID.Text + "' and DocUUID='" + DocUUID + "'", con);
                    cmd.ExecuteNonQuery();
                    #endregion
                    #endregion

                    #endregion
                }
                
                ViewState["WFLogID"] = null;
                ViewState["StepNo"] = null;
                Session["WFTask"] = null;
                Response.Redirect("userhome.aspx", false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected DataTable CreateDT4PDF(string DocUUID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataTable dtFields = null;
                string TemplateUUID = "";
                ds02.Reset();
                ds02 = ObjClassStoreProc.DocMetaValueDetails(DocUUID);
                if (ds02.Tables[0].Rows.Count > 0)
                {
                    TemplateUUID = ds02.Tables[0].Rows[0][1].ToString();
                }
                ds01.Reset();
                ds01 = ObjClassStoreProc.DocDetails(TemplateUUID, Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.ControlDetails(TemplateUUID);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        dtFields = CreateDT4Fields();
                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                        {
                            DataRow r = dtFields.NewRow();
                            dtFields.Rows.Add(AddNewInDTFields(r, Convert.ToInt32(ds01.Tables[0].Rows[i][1].ToString()), ds01.Tables[0].Rows[i][2].ToString(), ds01.Tables[0].Rows[i][3].ToString(), ds01.Tables[0].Rows[i][4].ToString(), ds01.Tables[0].Rows[i][6].ToString(), Convert.ToInt32(ds01.Tables[0].Rows[i][7].ToString()), Convert.ToInt32(ds01.Tables[0].Rows[i][8].ToString()), Convert.ToInt32(ds01.Tables[0].Rows[i][9].ToString()), ds01.Tables[0].Rows[i][10].ToString(), ds01.Tables[0].Rows[i][5].ToString(), ds01.Tables[0].Rows[i][11].ToString()));
                        }
                    }
                    else
                    {
                        throw new Exception("No Control found in this Form !!");
                    }
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("LabelField", typeof(string));
                dt.Columns.Add("ValueField", typeof(string));
                DataRow dr;

                for (int i = 0; i < dtFields.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["LabelField"] = dtFields.Rows[i]["LabelDesc"].ToString();
                    dr["ValueField"] = ds02.Tables[0].Rows[0][i + 4].ToString();
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        private String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] ImageData = new byte[fs.Length];
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return ImageData; //return the byte data
        }

        protected void cmdStart_Click(object sender, EventArgs e)
        {
            try
            {
                doc_mast_bal Obj_DocMast = new doc_mast_bal();
                UserRights RightsObj = new UserRights();
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                string result = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();

                con.Open();
                cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Redirected' where wf_log_id='" + lblWFID.Text + "'", con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("select dept_id,doc_type_id from wf_mast where wf_id='" + ddWFName.SelectedValue + "'", con);
                DataSet ds0001 = new DataSet();
                SqlDataAdapter adapter0001 = new SqlDataAdapter(cmd);
                adapter0001.Fill(ds0001);
                if (ds0001.Tables[0].Rows.Count > 0)
                {
                    Obj_DocMast.DocTypeCode = ds0001.Tables[0].Rows[0][1].ToString();
                    Obj_DocMast.DeptCode = ds0001.Tables[0].Rows[0][0].ToString();
                    result = ObjClassStoreProc.DefinedWF(Obj_DocMast.DocTypeCode, Obj_DocMast.DeptCode, Session["CompCode"].ToString());
                    if (Convert.ToInt32(result) == -111) /// If the Workflow is not defined for the Dept & Doc Type Combination; default workflow will be started
                    {

                    }
                    else /// If the Workflow is defined for the Dept & Doc Type Combination
                    {
                        DataSet ds0002 = new DataSet();
                        ds0002.Reset();
                        ds0002 = ObjClassStoreProc.DocDetails(Request.QueryString["DocUUID"].ToString(), Session["CompCode"].ToString());

                        if (ds0002.Tables[0].Rows.Count > 0)
                        {
                            Session["DocId"] = ds0002.Tables[0].Rows[0][0].ToString();
                        }

                        /// Store the defined Wokflow ID into <Session["WFId"]>
                        Session["WFId"] = result;
                        Obj_DocMast.DocID = Convert.ToInt64(Session["DocId"]);
                        Obj_DocMast.WFID = Convert.ToInt64(ddWFName.SelectedValue);
                        Obj_DocMast.Start_Dt = DateTime.Now;
                        Obj_DocMast.UserID = Session["UserID"].ToString();

                        result = ObjClassStoreProc.StartDefaultWF(Obj_DocMast.DocTypeCode, Obj_DocMast.DeptCode, DateTime.Now, Convert.ToInt16(Session["DocId"]), Convert.ToInt16(Session["WFId"]), Session["UserID"].ToString(), Session["CompCode"].ToString());
                        if (result != "") /// Start the defined workflow                    
                        {
                            /// Insert the Workflow based roles and due time into the database table name:<wf_log_dtl>
                            /// Store the <wf_log_id> into <Session["WFLogId"]>
                            /// Select the stage wise roles defined in the workflow
                            Session["WFLogId"] = result;
                            Obj_DocMast.WFID = Convert.ToInt64(Session["WFId"]);
                            DataSet ds1 = new DataSet();
                            ds1 = ObjClassStoreProc.SelectWFDtls(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                                {
                                    Obj_DocMast.WFLogID = Session["WFLogId"].ToString();
                                    Obj_DocMast.StepNo = Convert.ToInt32(ds1.Tables[0].Rows[i]["step_no"]);
                                    Obj_DocMast.Start_Dt = DateTime.Now;
                                    Obj_DocMast.Duration = ds1.Tables[0].Rows[i]["duration"].ToString();
                                    result = ObjClassStoreProc.StartDefaultWFLogDtl(Session["WFLogId"].ToString(), Convert.ToInt16(Obj_DocMast.StepNo), DateTime.Now, Obj_DocMast.Duration, DateTime.Now, Session["CompCode"].ToString());
                                    // Insert the Log for versioning of the file Start
                                    cmd = new SqlCommand("insert into WFDocVersion(WFLogID,StepNo,ActualDocUUID,NewDocUUID,CompCode) values('" + Obj_DocMast.WFLogID + "','" + Obj_DocMast.StepNo + "','" + Request.QueryString["DocUUID"].ToString() + "','','" + Session["CompCode"].ToString() + "')", con);
                                    cmd.ExecuteNonQuery();
                                    cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + Request.QueryString["DocUUID"].ToString() + "' where WFLogID='" + Obj_DocMast.WFLogID + "' and StepNo>'1'", con);
                                    cmd.ExecuteNonQuery();
                                    //Invisible the Older versions
                                    DataSet dsV01 = new DataSet();
                                    cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo<'" + lblStage.Text + "' order by StepNo", con);
                                    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                    adapterV01.Fill(dsV01);
                                    if (dsV01.Tables[0].Rows.Count > 0)
                                    {
                                        for (int kk = 0; kk < dsV01.Tables[0].Rows.Count; kk++)
                                        {
                                            RightsObj.UpdatePermissions4Doc(dsV01.Tables[0].Rows[kk][3].ToString(), "Document", "", "X");
                                        }
                                    }

                                    //if (Request.QueryString["NewFile"] != null)
                                    //{
                                    //    cmd = new SqlCommand("insert into WFDocVersion(WFLogID,StepNo,ActualDocUUID,NewDocUUID,CompCode) values('" + Obj_DocMast.WFLogID + "','" + Obj_DocMast.StepNo + "','" + Request.QueryString["DocUUID"].ToString() + "','" + Request.QueryString["DocUUID"].ToString() + "','" + Session["CompCode"].ToString() + "')", con);
                                    //    cmd.ExecuteNonQuery();
                                    //    cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Session["WFDocUUId"].ToString() + "' where WFLogID='" + Obj_DocMast.WFLogID + "' and StepNo='1' and ActualDocUUID='" + Session["TemplateUUID"].ToString() + "'", con);
                                    //    cmd.ExecuteNonQuery();
                                    //    //Invisible the Older versions
                                    //    DataSet dsV01 = new DataSet();
                                    //    cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo<'" + lblStage.Text + "' order by StepNo", con);
                                    //    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                    //    adapterV01.Fill(dsV01);
                                    //    if (dsV01.Tables[0].Rows.Count > 0)
                                    //    {
                                    //        for (int kk = 0; kk < dsV01.Tables[0].Rows.Count; kk++)
                                    //        {
                                    //            RightsObj.UpdatePermissions4Doc(dsV01.Tables[0].Rows[kk][3].ToString(), "Document", "", "X");
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    cmd = new SqlCommand("insert into WFDocVersion(WFLogID,StepNo,ActualDocUUID,NewDocUUID,CompCode) values('" + Obj_DocMast.WFLogID + "','" + Obj_DocMast.StepNo + "','" + Request.QueryString["DocUUID"].ToString() + "','','" + Session["CompCode"].ToString() + "')", con);
                                    //    cmd.ExecuteNonQuery();
                                    //    cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + Request.QueryString["DocUUID"].ToString() + "' where WFLogID='" + Obj_DocMast.WFLogID + "' and StepNo='1' and ActualDocUUID='" + Request.QueryString["DocUUID"].ToString() + "'", con);
                                    //    cmd.ExecuteNonQuery();
                                    //    //Invisible the Older versions
                                    //    DataSet dsV01 = new DataSet();
                                    //    cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + lblWFID.Text + "' and StepNo<'" + lblStage.Text + "' order by StepNo", con);
                                    //    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                    //    adapterV01.Fill(dsV01);
                                    //    if (dsV01.Tables[0].Rows.Count > 0)
                                    //    {
                                    //        for (int kk = 0; kk < dsV01.Tables[0].Rows.Count; kk++)
                                    //        {
                                    //            RightsObj.UpdatePermissions4Doc(dsV01.Tables[0].Rows[kk][3].ToString(), "Document", "", "X");
                                    //        }
                                    //    }
                                    //}
                                    // Insert the Log for versioning of the file End
                                }
                            }

                            /// Insert the Workflow based roles and tasks into the database table name:<wf_log_task>
                            /// Select the stage wise tasks defined in the workflow
                            Obj_DocMast.WFID = Convert.ToInt64(Session["WFId"]);
                            DataSet ds2 = new DataSet();
                            ds2 = ObjClassStoreProc.SelectWFTasks(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                {
                                    Obj_DocMast.WFLogID = Session["WFLogId"].ToString();
                                    Obj_DocMast.StepNo = Convert.ToInt32(ds2.Tables[0].Rows[i]["step_no"]);
                                    Obj_DocMast.TaskID = ds2.Tables[0].Rows[i]["task_id"].ToString();
                                    Obj_DocMast.AmbleMails = ds2.Tables[0].Rows[i]["amble_mails"].ToString();
                                    Obj_DocMast.AmbleMsg = ds2.Tables[0].Rows[i]["amble_msg"].ToString();
                                    Obj_DocMast.AmbleAttach = ds2.Tables[0].Rows[i]["amble_attach"].ToString();
                                    Obj_DocMast.AppendDoc = ds2.Tables[0].Rows[i]["AppendDocUUID"].ToString();
                                    Obj_DocMast.AmbleURL = ds2.Tables[0].Rows[i]["amble_url"].ToString();
                                    Obj_DocMast.AmbleSub = ds2.Tables[0].Rows[i]["AmbleSub"].ToString();
                                    result = ObjClassStoreProc.StartDefaultWFLogTask(Session["WFLogId"].ToString(), Convert.ToInt16(Obj_DocMast.StepNo), Obj_DocMast.TaskID, Obj_DocMast.AmbleMails, Obj_DocMast.AmbleMsg, Obj_DocMast.AmbleAttach, Obj_DocMast.AppendDoc, Obj_DocMast.AmbleURL, Obj_DocMast.AmbleSub, Session["CompCode"].ToString());
                                }
                                // Update the Initiator Start
                                DataSet dsI01 = new DataSet();
                                cmd = new SqlCommand("select email from user_mast where user_id='" + Session["UserID"].ToString() + "'", con);
                                SqlDataAdapter adapterI01 = new SqlDataAdapter(cmd);
                                adapterI01.Fill(dsI01);
                                if (dsI01.Tables[0].Rows.Count > 0)
                                {
                                    if (Session["AccessControl"].ToString() == "Outside")
                                    {
                                        cmd = new SqlCommand("update wf_log_task set amble_mails=REPLACE(amble_mails,'guest@guest.com','" + Session["InitiatorEmailID"].ToString() + "') where wf_log_id='" + Session["WFLogId"].ToString() + "'", con);
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("update wf_log_task set amble_mails=REPLACE(amble_mails,'init@init.com','" + dsI01.Tables[0].Rows[0][0].ToString() + "') where wf_log_id='" + Session["WFLogId"].ToString() + "'", con);
                                    }
                                    cmd.ExecuteNonQuery();
                                }
                                // Update the Initiator End
                                CheckAction(Obj_DocMast.WFLogID, 1, DateTime.Now, "", "");
                                if (Session["NewFile"] == null)
                                {

                                }
                                else
                                {
                                    //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                    //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                }
                                Session["WFTask"] = null;
                                // Update the Doc's Stat in TempDocSaving table
                                if (Session["hfPageControl"] != null)
                                {
                                    if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                    {
                                        cmd = new SqlCommand("update TempDocSaving set TempDocStat='Uploaded',CreationDate='" + DateTime.Now + "' where TempDocName='" + Session["OpenDocName"].ToString() + "' and UserID='" + Session["UserID"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                Session["hfPageControl"] = null;
                                Session["AccessControl"] = null;
                                Session["NewFile"] = null;
                                Session["TemplateUUID"] = null;
                                Redirect2Dashboard("Workflow with tasks has been re-assigned!", "userhome.aspx");
                            }
                            else
                            {
                                // For uploading a doc in a pre selected folder
                                if (Request.QueryString["UF"] != null)
                                {
                                    if (Request.QueryString["UF"].ToString() != "")
                                    {

                                    }
                                }
                                else
                                {
                                    //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                    //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                }
                                // Update the Doc's Stat in TempDocSaving table
                                if (Session["hfPageControl"] != null)
                                {
                                    if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                    {
                                        cmd = new SqlCommand("update TempDocSaving set TempDocStat='Uploaded',CreationDate='" + DateTime.Now + "' where TempDocName='" + Session["OpenDocName"].ToString() + "' and UserID='" + Session["UserID"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                Redirect2Dashboard("Workflow with tasks has been re-assigned!", "userhome.aspx");
                            }
                        }
                        Redirect2Dashboard("Workflow with tasks has been re-assigned!", "userhome.aspx");
                    }
                    Utility.CloseConnection(con);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void CheckAction(string WFLogID, int StepNo, DateTime TaskDoneDate, string TaskID, string Comments)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds001 = new DataSet();
                DataSet ds002 = new DataSet();
                DataSet ds003 = new DataSet();
                DataSet ds004 = new DataSet();
                DataSet ds005 = new DataSet();
                DataSet ds006 = new DataSet();
                con.Open();
                if (Session["AccessControl"].ToString() != "Outside")
                {
                    Session["InitiatorEmailID"] = "";
                }

                // Fetch the tasks for this particular WFLogID & Step No
                cmd = new SqlCommand("select a.* from wf_log_task a,wf_log_mast b where a.wf_log_id=b.wf_log_id and b.wf_prog_stat!='Rejected' and a.wf_log_id='" + WFLogID + "' and a.step_no='" + StepNo + "' and a.task_done_dt is null", con);
                ds001 = new DataSet();
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds001.Tables[0].Rows.Count; i++)
                    {
                        #region For Preamble Mail Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PREEMAIL")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            //Log Update
                            cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            // Preamble Mail
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleEmail ObjPreambleEmail = new PreambleEmail();
                                ObjPreambleEmail.SendPreMail(WFLogID, StepNo, "PREEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                            }
                        }
                        #endregion
                        #region For Preamble Copy Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PRECOPY")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            //Log Update
                            cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();

                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleCopy ObjPreambleCopy = new PreambleCopy();
                                string PreCPOutput = ObjPreambleCopy.PreCopy(WFLogID, StepNo, "PRECOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                CheckAction(WFLogID, StepNo, DateTime.Now, "PRECOPY", PreCPOutput);
                            }
                        }
                        #endregion
                        #region For Preamble Conditinal Mail Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PRECOND")
                        {
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            //Log Update
                            cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            // Preamble Conditional Mail
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleConditionalEmail ObjPreambleConditionalEmail = new PreambleConditionalEmail();
                                ObjPreambleConditionalEmail.SendPreCondMail(WFLogID, StepNo, "PRECOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                            }
                        }
                        #endregion
                        #region For Preamble Append Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PREAPPEND")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            //Log Update
                            cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            // Preamble Append
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleAppend ObjPreambleAppend = new PreambleAppend();
                                Session["RWFOldFile"] = ObjPreambleAppend.PreAppend(WFLogID, StepNo, "PREAPPEND", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                            }
                        }
                        #endregion

                        /// Check is there any Interactive actions or not start
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND' and task_id!='PREAPPEND' and task_id!='POSTAPPEND')", con);
                        ds003 = new DataSet();
                        ds003.Reset();
                        SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                        adapter003.Fill(ds003);
                        if (ds003.Tables[0].Rows.Count > 0)
                        {
                            if (TaskID == "")
                            {

                            }
                            else
                            {
                                if (TaskID == "APPROVE")
                                {
                                    #region
                                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and task_done_dt is null", con);
                                    DataSet dsT001 = new DataSet();
                                    SqlDataAdapter adapterT001 = new SqlDataAdapter(cmd);
                                    dsT001.Reset();
                                    adapterT001.Fill(dsT001);
                                    if (dsT001.Tables[0].Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Not Required' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + TaskID + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    return;
                                    #endregion
                                }
                                else if (TaskID == "REJECT")
                                {
                                    #region
                                    // Need to Mail to the previous stages' users
                                    if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                                    {

                                    }
                                    else
                                    {
                                        RejectEmail ObjRejectEmail = new RejectEmail();
                                        ObjRejectEmail.RejectMail(WFLogID, StepNo, Session["CompCode"].ToString());

                                        // Insert into Rejected List
                                        cmd = new SqlCommand("insert into RejectedList(wf_log_id,step_no,user_id,rejected_dt,comments,CompCode) values('" + WFLogID + "','" + StepNo + "','" + Session["UserID"].ToString() + "','" + TaskDoneDate + "','" + Comments + "','" + Session["CompCode"].ToString() + "')", con);
                                        cmd.ExecuteNonQuery();
                                        // Update wf_log_task & wf_log_dtl for previous stage start
                                        // Current Stage
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Not Started' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskDoneDate='Waiting'", con);
                                        cmd.ExecuteNonQuery();

                                        // Pervious Stage
                                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                        DataSet ds114 = new DataSet();
                                        SqlDataAdapter adapter114 = new SqlDataAdapter(cmd);
                                        ds114.Reset();
                                        adapter114.Fill(ds114);
                                        Session["WFTask"] = "REJECT";
                                        if (ds114.Tables[0].Rows.Count > 0)
                                        {
                                            cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            #region Update the Queued Date & Due Date
                                            cmd = new SqlCommand("select * from wf_dtl where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "') and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                            DataSet ds_0001 = new DataSet();
                                            SqlDataAdapter adapter_0001 = new SqlDataAdapter(cmd);
                                            adapter_0001.Fill(ds_0001);
                                            if (ds_0001.Tables[0].Rows.Count > 0)
                                            {
                                                DateTime Due_Dt2 = DateTime.Now;
                                                Due_Dt2 = Due_Dt2.AddHours(Convert.ToDouble(ds_0001.Tables[0].Rows[0][3].ToString()));
                                                cmd = new SqlCommand("update wf_log_dtl set assign_dt='" + DateTime.Now + "',due_dt='" + Due_Dt2 + "' where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                                cmd.ExecuteNonQuery();
                                            }
                                            #endregion
                                            //Log Update
                                            int MaxStep = 0;
                                            int StartStep = 0;
                                            DataSet dsMaxStep = new DataSet();
                                            cmd = new SqlCommand("select max(StepNo) from WFLog where WFLogID='" + WFLogID + "'", con);
                                            SqlDataAdapter adapterMaxStep = new SqlDataAdapter(cmd);
                                            dsMaxStep.Reset();
                                            adapterMaxStep.Fill(dsMaxStep);
                                            MaxStep = Convert.ToInt32(dsMaxStep.Tables[0].Rows[0][0].ToString());
                                            StartStep = Convert.ToInt32(StepNo) - 1;

                                            for (int kk = StartStep; kk <= MaxStep; kk++)
                                            {
                                                DataSet dsPrev = new DataSet();
                                                cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + kk + "'", con);
                                                SqlDataAdapter adapterPrev = new SqlDataAdapter(cmd);
                                                dsPrev.Reset();
                                                adapterPrev.Fill(dsPrev);
                                                if (kk <= StepNo) // for previous stages
                                                {
                                                    for (int jj = 0; jj < dsPrev.Tables[0].Rows.Count; jj++)
                                                    {
                                                        cmd = new SqlCommand("insert into WFLog(WFLogID,StepNo,UserID,TaskID,TaskDoneDate,Comments,CompCode) values('" + WFLogID + "', '" + kk + "','','" + dsPrev.Tables[0].Rows[jj][2].ToString() + "','Waiting','Waiting','" + Session["CompCode"].ToString() + "')", con);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                else // for later stages
                                                {
                                                    cmd = new SqlCommand("delete from WFLog where WFLogID='" + WFLogID + "' and StepNo='" + kk + "' and TaskDoneDate='Waiting'", con);
                                                    cmd.ExecuteNonQuery();
                                                    for (int jj = 0; jj < dsPrev.Tables[0].Rows.Count; jj++)
                                                    {
                                                        cmd = new SqlCommand("insert into WFLog(WFLogID,StepNo,UserID,TaskID,TaskDoneDate,Comments,CompCode) values('" + WFLogID + "', '" + kk + "','','" + dsPrev.Tables[0].Rows[jj][2].ToString() + "','Waiting','Waiting','" + Session["CompCode"].ToString() + "')", con);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            if (StepNo == 2)
                                            {
                                                cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Rejected' where wf_log_id='" + WFLogID + "'", con);
                                                cmd.ExecuteNonQuery();
                                            }
                                            return;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    // Update wf_log_task & wf_log_dtl for previous stage end
                                    #endregion
                                }
                                else if (TaskID == "REVIEW")
                                {
                                    #region
                                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and task_done_dt is null", con);
                                    DataSet dsT002 = new DataSet();
                                    SqlDataAdapter adapterT002 = new SqlDataAdapter(cmd);
                                    dsT002.Reset();
                                    adapterT002.Fill(dsT002);
                                    if (dsT002.Tables[0].Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Not Required' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + TaskID + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    return;
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null and (task_id='PRECOPY' or task_id='PREEMAIL' or task_id='PRECOND' or task_id='PREAPPEND')", con);
                            ds004 = new DataSet();
                            SqlDataAdapter adapter004 = new SqlDataAdapter(cmd);
                            ds004.Reset();
                            adapter004.Fill(ds004);
                            if (ds004.Tables[0].Rows.Count > 0)
                            {

                            }
                            else
                            {
                                #region For Postamble Mail Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTEMAIL")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleEmail ObjPostambleEmail = new PostambleEmail();
                                    ObjPostambleEmail.SendPostMail(WFLogID, StepNo, "POSTEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Copy Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTCOPY")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();

                                    PostambleCopy ObjPostambleCopy = new PostambleCopy();
                                    string PostCPOutput = ObjPostambleCopy.PostCopy(WFLogID, StepNo, "POSTCOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "POSTCOPY", PostCPOutput);
                                }
                                #endregion
                                #region For Postamble Conditional Mail Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTCOND")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleConditionalEmail ObjPostambleConditionalEmail = new PostambleConditionalEmail();
                                    ObjPostambleConditionalEmail.SendPostCondMail(WFLogID, StepNo, "POSTCOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Append Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTAPPEND")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();

                                    PostambleAppend ObjPostambleAppend = new PostambleAppend();
                                    Session["RWFOldFile"] = ObjPostambleAppend.PostAppend(WFLogID, StepNo, "POSTAPPEND", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                }
                                #endregion
                            }
                        }
                        /// Check is there any Interactive actions or not end
                    }
                    // Update Status start
                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null", con);
                    ds005 = new DataSet();
                    SqlDataAdapter adapter005 = new SqlDataAdapter(cmd);
                    adapter005.Fill(ds005);
                    if (ds005.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                        cmd.ExecuteNonQuery();
                        // Call this method again
                        DataSet ds100 = new DataSet();
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null  and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND' and task_id!='PREAPPEND' and task_id!='POSTAPPEND')", con);
                        SqlDataAdapter adapter100 = new SqlDataAdapter(cmd);
                        adapter100.Fill(ds100);
                        if (ds100.Tables[0].Rows.Count > 0)
                        {

                        }
                        else
                        {
                            CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                            return;
                        }
                    }
                    else
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Completed' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                        cmd.ExecuteNonQuery();
                        #region Update the Queued Date & Due Date
                        cmd = new SqlCommand("select * from wf_dtl where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "') and step_no='" + (StepNo + 1) + "'", con);
                        DataSet ds_0001 = new DataSet();
                        SqlDataAdapter adapter_0001 = new SqlDataAdapter(cmd);
                        adapter_0001.Fill(ds_0001);
                        if (ds_0001.Tables[0].Rows.Count > 0)
                        {
                            DateTime Due_Dt = DateTime.Now;
                            Due_Dt = Due_Dt.AddHours(Convert.ToDouble(ds_0001.Tables[0].Rows[0][3].ToString()));
                            cmd = new SqlCommand("update wf_log_dtl set assign_dt='" + DateTime.Now + "',due_dt='" + Due_Dt + "' where wf_log_id='" + WFLogID + "' and step_no='" + (StepNo + 1) + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        #endregion
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and task_done_dt is null", con);
                        ds006 = new DataSet();
                        SqlDataAdapter adapter006 = new SqlDataAdapter(cmd);
                        adapter006.Fill(ds006);
                        if (ds006.Tables[0].Rows.Count > 0)
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Ongoing' where wf_log_id='" + WFLogID + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Completed',actual_completed_dt='" + TaskDoneDate + "' where wf_log_id='" + WFLogID + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // Update Status end

                    // Check the stage is completed or not
                    cmd = new SqlCommand("select wf_stat from wf_log_dtl where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                    ds002 = new DataSet();
                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                    adapter002.Fill(ds002);
                    if (ds002.Tables[0].Rows.Count > 0)
                    {
                        if (ds002.Tables[0].Rows[0][0].ToString() == "Completed")
                        {
                            // Call this method again
                            CheckAction(WFLogID, StepNo + 1, DateTime.Now, "", "");
                            return;
                        }
                    }

                }
                else
                {
                    cmd = new SqlCommand("update wf_log_dtl set wf_stat='Completed' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                    cmd.ExecuteNonQuery();
                    #region Update the Queued Date & Due Date
                    cmd = new SqlCommand("select * from wf_dtl where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "') and step_no='" + (StepNo + 1) + "'", con);
                    DataSet ds_0001 = new DataSet();
                    SqlDataAdapter adapter_0001 = new SqlDataAdapter(cmd);
                    adapter_0001.Fill(ds_0001);
                    if (ds_0001.Tables[0].Rows.Count > 0)
                    {
                        DateTime Due_Dt1 = DateTime.Now;
                        Due_Dt1 = Due_Dt1.AddHours(Convert.ToDouble(ds_0001.Tables[0].Rows[0][3].ToString()));
                        cmd = new SqlCommand("update wf_log_dtl set assign_dt='" + DateTime.Now + "',due_dt='" + Due_Dt1 + "' where wf_log_id='" + WFLogID + "' and step_no='" + (StepNo + 1) + "'", con);
                        cmd.ExecuteNonQuery();
                    }
                    #endregion
                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and task_done_dt is null", con);
                    ds006 = new DataSet();
                    SqlDataAdapter adapter006 = new SqlDataAdapter(cmd);
                    adapter006.Fill(ds006);
                    if (ds006.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Ongoing' where wf_log_id='" + WFLogID + "'", con);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Completed',actual_completed_dt='" + TaskDoneDate + "' where wf_log_id='" + WFLogID + "'", con);
                        cmd.ExecuteNonQuery();



                        //First check there is any more Task pending or the Workflow is completed
                        //FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                        //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                        //string result = "";
                        //doc_mast_bal Obj_DocMast = new doc_mast_bal();
                        //UserRights RightsObj = new UserRights();
                        //DataSet dsPerm = new DataSet();
                        //SqlDataAdapter adpAvl01;
                        //DataSet dsAvl01 = new DataSet();
                        //double AvailableSpace = 0;
                        //DataSet ds01 = new DataSet();


                        



                    }
                }
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                //MessageBox(ex.Message);
            }
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        private void Redirect2Dashboard(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"userhome.aspx\"</script>";

            Page.Controls.Add(lbl);
        }

        private void Redirect2Home(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"home.aspx\"</script>";

            Page.Controls.Add(lbl);
        }

        private void RedirectMessageBox(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"userhome.aspx\"</script>";

            Page.Controls.Add(lbl);
        }

        protected void SaveDocANDMetaValues(string DocUUID, string eFormTemplateUUID, string CompCode)
        {
            try
            {
                #region Capture the values
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataTable dtFields = null;
                dtFields = (DataTable)Session["dtFields"];

                double result1;
                bool IsDouble = false;
                string TotStr = "";
                for (int i = 0; i < dtFields.Rows.Count; i++)
                {
                    if (dtFields.Rows[i]["ControlType"].ToString() == "T")
                    {
                        if (dtFields.Rows[i]["DataType"].ToString() == "Numeric")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[i]["ControlID"].ToString()); //(TextBox)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                            if (tb != null)
                            {
                                TotStr += tb.ID + "=" + tb.Text + "~";
                                IsDouble = double.TryParse(tb.Text.Trim(), out result1);
                                if (IsDouble == false)
                                {
                                    throw new Exception("Please enter numeric value in " + dtFields.Rows[i]["LabelDesc"].ToString() + " Field !!");
                                }
                                else
                                {
                                    if (Convert.ToDouble(tb.Text.Trim()) < Convert.ToDouble(dtFields.Rows[i]["MinVal"].ToString()))
                                    {
                                        throw new Exception("Please enter proper value in " + dtFields.Rows[i]["LabelDesc"].ToString() + " Field !!");
                                    }
                                    else if (Convert.ToDouble(tb.Text.Trim()) > Convert.ToDouble(dtFields.Rows[i]["MaxVal"].ToString()))
                                    {
                                        throw new Exception("Please enter proper value in " + dtFields.Rows[i]["LabelDesc"].ToString() + " Field !!");
                                    }
                                }
                            }
                        }
                        else if (dtFields.Rows[i]["DataType"].ToString() == "Formula")
                        {
                            TextBox tb = (TextBox)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                            if (tb != null)
                            {
                                #region Calculating the Formula
                                string Controls4Formula = dtFields.Rows[i]["Controls4Formula"].ToString();
                                string Formula = dtFields.Rows[i]["Formula"].ToString();
                                string[] Controls = Controls4Formula.ToString().Split(',');
                                for (int j = 0; j < Controls.Length; j++)
                                {
                                    if (Controls[j].ToString() != "")
                                    {
                                        TextBox tb1 = (TextBox)tblMain.FindControl(Controls[j].ToString());
                                        Formula = Formula.Replace(Controls[j].ToString(), tb1.Text.Trim());
                                    }
                                }
                                Formula f = new Formula(Formula);
                                tb.Text = f.evaluate(new System.Collections.Hashtable()).ToString();
                                #endregion
                                TotStr += tb.ID + "=" + tb.Text + "~";
                            }
                        }
                        else
                        {
                            TextBox tb = (TextBox)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                            if (tb != null)
                            {
                                TotStr += tb.ID + "=" + tb.Text + "~";
                            }
                        }
                    }
                    else if (dtFields.Rows[i]["ControlType"].ToString() == "D")
                    {
                        DropDownList ddL = (DropDownList)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                        if (ddL != null)
                        {
                            TotStr += ddL.ID + "=" + ddL.Text + "~";
                        }
                    }
                }
                #endregion
                #region MetaValues update
                string TotalStr = TotStr;
                int StartPos = 0;
                int EndPos = 0;
                string NthVal = "";
                string DBFldName = "";
                string[] TermValues;
                TermValues = new string[dtFields.Rows.Count];
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                string result = "";

                result = ObjClassStoreProc.DocMetaValueInsert(DocUUID, eFormTemplateUUID, CompCode);
                con.Open();
                for (int i = 0; i < TotalStr.Length; i++)
                {
                    StartPos = TotalStr.IndexOf('=');
                    EndPos = TotalStr.IndexOf('~');
                    NthVal = TotalStr.Substring(StartPos + 1, EndPos - StartPos - 1);
                    TermValues[i] = NthVal;
                    TotalStr = TotalStr.Substring(EndPos + 1, TotalStr.Length - EndPos - 1);

                    DBFldName = "Tag" + (i + 1).ToString();
                    cmd = new SqlCommand("update DocMetaValue set " + DBFldName + "='" + TermValues[i].ToString() + "' where uuid='" + DocUUID + "'", con);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                Utility.CloseConnection(con);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        protected void FetchLocation(string DeptID, string DocTypeID)
        {
            try
            {
                //divUpldLoc.Visible = true;

                //ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                //DataSet ds01 = new DataSet();
                //ds01 = ObjClassStoreProc.FetchUploadedLocation(DeptID, DocTypeID, Session["CompCode"].ToString());
                //if (ds01.Tables[0].Rows.Count > 0)
                //{
                //    hfUUID.Value = ds01.Tables[0].Rows[0][0].ToString();
                //    lblLocation.Text = ds01.Tables[0].Rows[0][3].ToString() + " >> " + ds01.Tables[0].Rows[0][2].ToString() + " >> " + ds01.Tables[0].Rows[0][1].ToString();
                //}
                //else
                //{
                //    lblLocation.Text = "";
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable CreateDT4Fields()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SerialNo", typeof(Int32));
            dt.Columns.Add("LabelID", typeof(string));
            dt.Columns.Add("ControlID", typeof(string));
            dt.Columns.Add("ControlType", typeof(string));
            dt.Columns.Add("DataType", typeof(string));
            dt.Columns.Add("MaxLength", typeof(Int32));
            dt.Columns.Add("MinVal", typeof(Int32));
            dt.Columns.Add("MaxVal", typeof(Int32));
            dt.Columns.Add("Formula", typeof(string));
            dt.Columns.Add("LabelDesc", typeof(string));
            dt.Columns.Add("Controls4Formula", typeof(string));
            return dt;
        }

        public DataRow AddNewInDTFields(DataRow r, int SerialNo, string LabelID, string ControlID, string ControlType, string DataType, int MaxLength, int MinVal, int MaxVal, string Formula, string LabelDesc, string Controls4Formula)
        {
            r["SerialNo"] = SerialNo;
            r["LabelID"] = LabelID;
            r["ControlID"] = ControlID;
            r["ControlType"] = ControlType;
            r["DataType"] = DataType;
            r["MaxLength"] = MaxLength;
            r["MinVal"] = MinVal;
            r["MaxVal"] = MaxVal;
            r["Formula"] = Formula;
            r["LabelDesc"] = LabelDesc;
            r["Controls4Formula"] = Controls4Formula;
            return r;
        }

        protected void PopulateDesign()
        {
            try
            {
                if (hfControls != null)
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    DataTable dtFields = null;
                    ds01 = ObjClassStoreProc.DocMetaValueDetails(Request.QueryString["DocUUID"].ToString());

                    dtFields = (DataTable)Session["dtFields"];
                    for (int i = 0; i < dtFields.Rows.Count; i++)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc = new TableCell();
                        TableCell tc1 = new TableCell();

                        Label lbl = new Label();
                        lbl.ID = dtFields.Rows[i]["LabelID"].ToString();
                        lbl.Text = dtFields.Rows[i]["LabelDesc"].ToString();

                        tr = new TableRow();
                        tc = new TableCell();
                        tc.Controls.Add(lbl);

                        if (dtFields.Rows[i]["ControlType"].ToString() == "T")
                        {
                            tc1 = new TableCell();

                            TextBox tb = new TextBox();
                            tb.ID = dtFields.Rows[i]["ControlID"].ToString();
                            tb.Width = 200;
                            if (dtFields.Rows[i]["DataType"].ToString() == "Text")
                            {
                                tb.MaxLength = Convert.ToInt32(dtFields.Rows[i]["MaxLength"].ToString());
                            }
                            tb.Text = ds01.Tables[0].Rows[0][i + 4].ToString();
                            tc1.Controls.Add(tb);
                        }
                        else if (dtFields.Rows[i]["ControlType"].ToString() == "D")
                        {
                            tc1 = new TableCell();
                            DropDownList dpl = new DropDownList();
                            dpl.ID = dtFields.Rows[i]["ControlID"].ToString();

                            // Finding the corresponding SessionID
                            string totStr = dtFields.Rows[i]["ControlID"].ToString();
                            int totLength = totStr.Length;
                            int DDSessionID = Convert.ToInt16(totStr.Substring(totStr.LastIndexOf('_') + 1, (totLength - totStr.LastIndexOf('_') - 1)));

                            string[] DDItems = Session[(DDSessionID + 1).ToString()].ToString().Split('~');
                            for (int j = 0; j < DDItems.Length; j++)
                            {
                                dpl.Items.Add(DDItems[j]);
                            }
                            dpl.SelectedItem.Text = ds01.Tables[0].Rows[0][i + 4].ToString();
                            //if (hfButtonStatus.Value == "")
                            //{
                            //    ds01.Reset();
                            //    ds01 = ObjClassStoreProc.DropdownItems(Request.QueryString["FormID"].ToString(), dtFields.Rows[i]["ControlID"].ToString());
                            //    if (ds01.Tables[0].Rows.Count > 0)
                            //    {
                            //        for (int j = 0; j < ds01.Tables[0].Rows.Count; j++)
                            //        {
                            //            dpl.Items.Add(ds01.Tables[0].Rows[j][4].ToString());
                            //        }
                            //    }
                            //}
                            //else
                            //{

                            //}
                            tc1.Controls.Add(dpl);
                        }
                        tr.Cells.Add(tc);
                        tr.Cells.Add(tc1);
                        tblMain.Rows.Add(tr);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void PopulateFormDetails(string DcUUID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataTable dtFields = null;
                string DocUUID = "";
                ds01.Reset();
                ds01 = ObjClassStoreProc.DocMetaValueDetails(DcUUID);
                Session["eFormTemplateUUID"] = ds01.Tables[0].Rows[0][1].ToString();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DocDetails(DcUUID, Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    //txtDocName.Text = ds01.Tables[0].Rows[0][1].ToString();
                    Session["ddDocTypeSel"] = ds01.Tables[0].Rows[0][2].ToString();
                    DocUUID = ds01.Tables[0].Rows[0][4].ToString();
                    //Session["eFormTemplateUUID"] = DocUUID;
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.ControlDetails(Session["eFormTemplateUUID"].ToString());
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        dtFields = CreateDT4Fields();
                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                        {
                            DataRow r = dtFields.NewRow();
                            dtFields.Rows.Add(AddNewInDTFields(r, Convert.ToInt32(ds01.Tables[0].Rows[i][1].ToString()), ds01.Tables[0].Rows[i][2].ToString(), ds01.Tables[0].Rows[i][3].ToString(), ds01.Tables[0].Rows[i][4].ToString(), ds01.Tables[0].Rows[i][6].ToString(), Convert.ToInt32(ds01.Tables[0].Rows[i][7].ToString()), Convert.ToInt32(ds01.Tables[0].Rows[i][8].ToString()), Convert.ToInt32(ds01.Tables[0].Rows[i][9].ToString()), ds01.Tables[0].Rows[i][10].ToString(), ds01.Tables[0].Rows[i][5].ToString(), ds01.Tables[0].Rows[i][11].ToString()));
                            if (ds01.Tables[0].Rows[i][4].ToString() == "D")
                            {
                                string totStr = ds01.Tables[0].Rows[i][3].ToString();
                                int totLength = totStr.Length;
                                int DDSessionID = Convert.ToInt16(totStr.Substring(totStr.LastIndexOf('_') + 1, (totLength - totStr.LastIndexOf('_') - 1)));

                                ds02.Reset();
                                ds02 = ObjClassStoreProc.DropdownItems(Session["eFormTemplateUUID"].ToString(), dtFields.Rows[i]["ControlID"].ToString());
                                if (ds02.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds02.Tables[0].Rows.Count; j++)
                                    {
                                        if (Session[(DDSessionID + 1).ToString()] != null)
                                        {
                                            Session[(DDSessionID + 1).ToString()] += "~" + ds02.Tables[0].Rows[j][4].ToString();
                                        }
                                        else
                                        {
                                            Session[(DDSessionID + 1).ToString()] += ds02.Tables[0].Rows[j][4].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No Control found in this Form !!");
                    }
                }
                else
                {
                    throw new Exception("No record found !!");
                }

                Session["dtFields"] = dtFields;
                //gvFields.DataSource = dtFields;
                //gvFields.DataBind();
                tblMain.Controls.Clear();

                for (int i = 0; i < dtFields.Rows.Count; i++)
                {
                    if (dtFields.Rows[i]["ControlType"].ToString() == "T")
                    {
                        hfControls.Value += "T";
                    }
                    else if (dtFields.Rows[i]["ControlType"].ToString() == "D")
                    {
                        hfControls.Value += "D";
                    }
                }

                ////Populate ddPosition DropDown
                //ddPosition.Items.Clear();
                //dtFields = (DataTable)Session["dtFields"];
                //ddPosition.Items.Add("At the Beginning");
                //for (int i = 0; i < dtFields.Rows.Count; i++)
                //{
                //    ddPosition.Items.Add("After " + dtFields.Rows[i]["LabelDesc"].ToString());
                //}
                //ddPosition.SelectedIndex = dtFields.Rows.Count;

                ////Populate ddLabelDesc Dropdown
                //ddLabelDesc.Items.Clear();
                //ddLabelDesc.DataSource = dtFields;
                //ddLabelDesc.DataTextField = "LabelDesc";
                //ddLabelDesc.DataValueField = "ControlID";
                //ddLabelDesc.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.Flush();
        }

    }
}