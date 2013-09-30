using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using System.Net;
using System.IO;
using DMS.BAL;
using QuickPDFDLL0813;
using System.Configuration;
using System.Collections;
using DMS.Actions;

using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;


namespace DMS
{
    public partial class FormFillup : System.Web.UI.Page
    {
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");
        #region
        /// Define reference of the Webservices
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;
        private RepositoryService repoServiceA;
        private ArrayList parentReferences = new ArrayList();

        public RepositoryService RepoService
        {
            set { repoService = value; }
        }
        public RepositoryService RepoServiceA
        {
            set { repoServiceA = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Set the session variables blank which are used to set the previous selected path start
                    Session["SelectedCabUUID"] = "";
                    Session["SelectedDrwUUID"] = "";
                    Session["SelectedFldUUID"] = "";
                    Session["SelectedDocID"] = "";
                    // Set the session variables blank which are used to set the previous selected path end
                    Page.Header.DataBind();
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = true;
                        }

                        // Get the repository service
                        // Admin Credentials start
                        WebServiceFactory wsFA = new WebServiceFactory();
                        wsFA.UserName = Session["AdmUserID"].ToString();
                        wsFA.Ticket = Session["AdmTicket"].ToString();
                        this.repoServiceA = wsFA.getRepositoryService();
                        // Admin Credentials end

                        WebServiceFactory wsF = new WebServiceFactory();
                        wsF.UserName = Session["UserID"].ToString();
                        wsF.Ticket = Session["Ticket"].ToString();
                        this.repoService = wsF.getRepositoryService();

                        // Initialise the reference to the spaces store
                        this.spacesStore = new Alfresco.RepositoryWebService.Store();
                        this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                        this.spacesStore.address = "SpacesStore";
                        /// Authentication required for Alfresco Connectivity End

                        if (Session["hfPageControl"].ToString() == "F") //For Fresh WF
                        {
                            hfPageControl.Value = "F";
                            divRunningWF.Visible = false;
                            divFreshWF.Visible = true;
                            cmdBackSearch.Visible = false;

                            Session["OpenDocName"] = Request.QueryString["docname"].ToString();
                            hfDocument.Value = Request.QueryString["docname"].ToString();
                            Session["SelDocUUID"] = Request.QueryString["DocUUID"].ToString();
                            PopDocProp(Session["SelDocUUID"].ToString(),"");
                            // Fetch Signature & Date Fields Start
                            DataSet ds = FetchSignFlds(Session["SelDocUUID"].ToString());
                            Session["dsSignFlds"] = ds;
                        }
                        else if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                        {
                            hfPageControl.Value = "FE";
                            divRunningWF.Visible = false;
                            divFreshWF.Visible = true;
                            cmdBackSearch.Visible = false;

                            Session["OpenDocName"] = Request.QueryString["docname"].ToString();
                            hfDocument.Value = Request.QueryString["docname"].ToString();
                            Session["SelDocUUID"] = Request.QueryString["DocUUID"].ToString();
                            PopDocProp("", hfDocument.Value);
                            // Fetch Signature & Date Fields Start
                            DataSet ds = FetchSignFlds(Session["SelDocUUID"].ToString());
                            Session["dsSignFlds"] = ds;
                        }
                        else if (Session["hfPageControl"].ToString() == "R") //For Running WF
                        {
                            hfPageControl.Value = "R";
                            divRunningWF.Visible = true;
                            divFreshWF.Visible = false;
                            Session["OpenDocName"] = Request.QueryString["docname"].ToString();
                            hfDocument.Value = Request.QueryString["docname"].ToString();
                            Session["SelDocUUID"] = Request.QueryString["DocUUID"].ToString();
                            Session["AttachedDocUUID"] = Request.QueryString["AttachedDocUUID"].ToString();
                            PopForTaskUpdate();
                            divLocal.Visible = false;
                        }
                        else if (Session["hfPageControl"].ToString() == "O") //For Fresh WF
                        {
                            hfPageControl.Value = "O";
                            divRunningWF.Visible = false;
                            divFreshWF.Visible = true;
                            cmdBackSearch.Visible = true;

                            Session["OpenDocName"] = Request.QueryString["docname"].ToString();
                            hfDocument.Value = Request.QueryString["docname"].ToString();
                            Session["SelDocUUID"] = Request.QueryString["DocUUID"].ToString();
                            PopDocProp(Session["SelDocUUID"].ToString(), "");
                        }
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        private DataSet FetchSignFlds(string DocUUID)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds001 = new DataSet();
            DataSet ds002 = new DataSet();
            cmd = new SqlCommand("select doc_type_id from doc_mast where uuid='" + DocUUID + "'", con);
            SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
            adapter001.Fill(ds001);
            if (ds001.Tables[0].Rows.Count > 0)
            {
                cmd = new SqlCommand("select SignFieldNo1,SignDateFieldNo1,SignFieldNo2,SignDateFieldNo2,SignFieldNo3,SignDateFieldNo3 from doc_type_mast where doc_type_id='" + ds001.Tables[0].Rows[0][0].ToString() + "'", con);
                SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                adapter002.Fill(ds002);
            }
            Utility.CloseConnection(con);
            return ds002;
        }

        /// <summary>
        /// Proceed will transfer the page to New Document Uploading Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdProceed_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds001 = new DataSet();
                SqlDataAdapter adapter001;
                SqlDataAdapter adapter002;
                SqlDataAdapter adapter003;
                con.Open();
                cmd = new SqlCommand("Select dept_id from doc_mast where uuid='" + Session["SelDocUUID"].ToString() + "'", con);
                adapter002 = new SqlDataAdapter(cmd);
                ds001.Reset();
                adapter002.Fill(ds001);
                con.Close();
                if (ds001.Tables[0].Rows[0][0].ToString() != "NA")
                {
                    con.Open();
                    cmd = new SqlCommand("select * from wf_log_mast where (wf_prog_stat='Completed' or wf_prog_stat='Rejected') and wf_log_id in(select WFLogID from WFDocVersion where (ActualDocUUID='" + Session["SelDocUUID"].ToString() + "' or NewDocUUID='" + Session["SelDocUUID"].ToString() + "'))", con);
                    adapter003 = new SqlDataAdapter(cmd);
                    ds001.Reset();
                    adapter003.Fill(ds001);
                    con.Close();
                    if (ds001.Tables[0].Rows.Count == 0)
                    {
                        Response.Redirect("ErrDisp.aspx", false);
                    }
                    else
                    {
                        if (Session["hfPageControl"].ToString() == "FE")
                        {
                            con.Open();
                            cmd = new SqlCommand("select TempDocName,UserID,DocTypeID,TempDocStat,CreationDate,TemplateUUID from TempDocSaving where TempDocName='" + hfDocument.Value + "'", con);
                            adapter001 = new SqlDataAdapter(cmd);
                            ds001.Reset();
                            adapter001.Fill(ds001);
                            con.Close();
                            if (ds001.Tables[0].Rows.Count > 0)
                            {
                                Session["SelDocUUID"] = ds001.Tables[0].Rows[0][5].ToString();
                            }
                        }
                        Response.Redirect("doc_mast.aspx?NewFile=" + hfDocument.Value + "&TemplateUUID=" + Session["SelDocUUID"].ToString(), false);
                    }
                }
                else
                {
                    if (Session["hfPageControl"].ToString() == "FE")
                    {
                        con.Open();
                        cmd = new SqlCommand("select TempDocName,UserID,DocTypeID,TempDocStat,CreationDate,TemplateUUID from TempDocSaving where TempDocName='" + hfDocument.Value + "'", con);
                        adapter001 = new SqlDataAdapter(cmd);
                        ds001.Reset();
                        adapter001.Fill(ds001);
                        con.Close();
                        if (ds001.Tables[0].Rows.Count > 0)
                        {
                            Session["SelDocUUID"] = ds001.Tables[0].Rows[0][5].ToString();
                        }
                    }
                    Response.Redirect("doc_mast.aspx?NewFile=" + hfDocument.Value + "&TemplateUUID=" + Session["SelDocUUID"].ToString(), false);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("userhome.aspx",false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdBackSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("search_list.aspx",false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        /// <summary>
        /// Update will update the assigned task along with the document with a new version
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string UpldFolderUUID = "";
                UpldFolderUUID = FetchLocation(Session["OldFileDeptID"].ToString(), Session["OldFileDocTypeID"].ToString());

                // Upload the new version into Alfresco
                //Stream inputStream;
                byte[] bytes;
                string fileName = Session["NewDocName"].ToString();

                //Initialise the reference to the spaces store
                Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                spacesStore.address = "SpacesStore";

                // Create the parent reference, the company home folder
                Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
                parentReference.store = spacesStore;
                parentReference.uuid = UpldFolderUUID; // Folder's uuid

                parentReference.associationType = Constants.ASSOC_CONTAINS;
                parentReference.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, fileName);

                // Create the properties list
                NamedValue nameProperty = new NamedValue();
                nameProperty.name = Constants.PROP_NAME;
                nameProperty.value = fileName;
                nameProperty.isMultiValue = false;

                NamedValue[] properties = new NamedValue[2];
                properties[0] = nameProperty;
                nameProperty = new NamedValue();
                nameProperty.name = Constants.PROP_TITLE;
                nameProperty.value = fileName;
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

                WebServiceFactory wsFA = new WebServiceFactory();
                wsFA.UserName = Session["AdmUserID"].ToString();
                wsFA.Ticket = Session["AdmTicket"].ToString();
                this.repoServiceA = wsFA.getRepositoryService();
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

                bytes = StreamFile(Server.MapPath("TempDownload") + "\\" + Session["RWFDocName"].ToString());
                Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
                contentFormat.mimetype = "application/pdf";

                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = Session["AdmUserID"].ToString();
                wsF.Ticket = Session["AdmTicket"].ToString();
                wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);

                // Delete the file as the work has been done
                File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewDocName"].ToString());
                File.Delete(Server.MapPath("TempDownload") + "\\" + Session["RWFDocName"].ToString());

                userhome_bal UserHomeObj = new userhome_bal();
                /// Update this task in workflow execution using Store Procedure <WFLogTaskDtl_Update> start
                UserHomeObj.WFLogID = Session["WFUpdtWFLogID"].ToString();
                UserHomeObj.StepNo = Convert.ToInt32(Session["WFUpdtStepNo"]);
                UserHomeObj.TaskDone_Dt = DateTime.Now;
                UserHomeObj.Task_ID = ddTask.SelectedValue;
                UserHomeObj.Comments = txtComments.Text.Trim();
                CheckAction(UserHomeObj.WFLogID, UserHomeObj.StepNo, UserHomeObj.TaskDone_Dt, UserHomeObj.Task_ID, UserHomeObj.Comments);

                ViewState["WFLogID"] = null;
                ViewState["StepNo"] = null;

                Response.Redirect("~/userhome.aspx", false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopDocProp(string DocUUID,string DocName)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet dsDocProp = new DataSet();
                if (DocName == "")
                {
                    cmd = new SqlCommand("select a.doc_id,a.doc_name,a.doc_type_id,a.dept_id,a.fld_uuid,a.upld_by,a.upld_dt,a.doc_stat,b.doc_type_name,(c.f_name + ' ' + c.l_name) as name from doc_mast a,doc_type_mast b,user_mast c where a.doc_type_id=b.doc_type_id and a.upld_by=c.user_id and a.uuid='" + DocUUID + "'", con);
                    SqlDataAdapter adapterDocProp = new SqlDataAdapter(cmd);
                    adapterDocProp.Fill(dsDocProp);
                    if (dsDocProp.Tables[0].Rows.Count > 0)
                    {
                        lblUpldBy.Text = dsDocProp.Tables[0].Rows[0][9].ToString();
                        lblUpldDt.Text = dsDocProp.Tables[0].Rows[0][6].ToString();
                        lblDocStat.Text = dsDocProp.Tables[0].Rows[0][7].ToString();
                        lblDocType.Text = dsDocProp.Tables[0].Rows[0][8].ToString();
                    }
                }
                else if(DocUUID=="")
                {
                    cmd = new SqlCommand("select a.TempDocName,a.UserID,a.DocTypeID,a.TempDocStat,a.CreationDate,b.doc_type_name,(c.f_name + ' ' + c.l_name) as name from TempDocSaving a,doc_type_mast b,user_mast c where a.DocTypeID=b.doc_type_id and a.upld_by=c.user_id and a.TempDocName='" + DocName + "'", con);
                    SqlDataAdapter adapterDocProp = new SqlDataAdapter(cmd);
                    adapterDocProp.Fill(dsDocProp);
                    if (dsDocProp.Tables[0].Rows.Count > 0)
                    {
                        lblUpldBy.Text = dsDocProp.Tables[0].Rows[0][6].ToString();
                        lblUpldDt.Text = dsDocProp.Tables[0].Rows[0][4].ToString();
                        lblDocStat.Text = "Still Not Uploaded";
                        lblDocType.Text = dsDocProp.Tables[0].Rows[0][5].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

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
                lblAssignedBy.Text = Session["WFUpdtAssignBy"].ToString();
                lblDocName.Text = Session["WFUpdtDocName"].ToString();
                PopulateDropdown();
                //DispDocument();
                TagDisplay(Session["AttachedDocUUID"].ToString());
            }
            catch (Exception ex)
            {

            }
        }

        protected void TagDisplay(string DocUUID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                string SelItem = "";

                con.Open();
                cmd = new SqlCommand("select doc_type_id,tag1,tag2,tag3,tag4,tag5,tag6,tag7,tag8,tag9,tag10 from doc_mast where uuid='" + DocUUID + "'", con);
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);
                Utility.CloseConnection(con);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    SelItem = ds01.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    throw new Exception("No Document Type Assigned for This Document.");
                }

                doc_mast_bal Obj_DocMastBAL = new doc_mast_bal();
                Obj_DocMastBAL.DocTypeCode = SelItem;

                DataSet ds1 = new DataSet();
                ds1 = Obj_DocMastBAL.FetchTags();
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    /// For Tag1
                    if (ds1.Tables[0].Rows[0]["tag1"].ToString() == "" || ds1.Tables[0].Rows[0]["tag1"] == null)
                    {
                        txtTag1.Text = "";
                        divTag1.Visible = false;
                    }
                    else
                    {
                        divTag1.Visible = true;
                        lblTag1.Text = ds1.Tables[0].Rows[0]["tag1"].ToString();
                        txtTag1.Text = ds01.Tables[0].Rows[0][1].ToString();
                    }
                    /// For Tag2
                    if (ds1.Tables[0].Rows[0]["tag2"].ToString() == "" || ds1.Tables[0].Rows[0]["tag2"] == null)
                    {
                        txtTag2.Text = "";
                        divTag2.Visible = false;
                    }
                    else
                    {
                        divTag2.Visible = true;
                        lblTag2.Text = ds1.Tables[0].Rows[0]["tag2"].ToString();
                        txtTag2.Text = ds01.Tables[0].Rows[0][2].ToString();
                    }
                    /// For Tag3
                    if (ds1.Tables[0].Rows[0]["tag3"].ToString() == "" || ds1.Tables[0].Rows[0]["tag3"] == null)
                    {
                        txtTag3.Text = "";
                        divTag3.Visible = false;
                    }
                    else
                    {
                        divTag3.Visible = true;
                        lblTag3.Text = ds1.Tables[0].Rows[0]["tag3"].ToString();
                        txtTag3.Text = ds01.Tables[0].Rows[0][3].ToString();
                    }
                    /// For Tag4
                    if (ds1.Tables[0].Rows[0]["tag4"].ToString() == "" || ds1.Tables[0].Rows[0]["tag4"] == null)
                    {
                        txtTag4.Text = "";
                        divTag4.Visible = false;
                    }
                    else
                    {
                        divTag4.Visible = true;
                        lblTag4.Text = ds1.Tables[0].Rows[0]["tag4"].ToString();
                        txtTag4.Text = ds01.Tables[0].Rows[0][4].ToString();
                    }
                    /// For Tag5
                    if (ds1.Tables[0].Rows[0]["tag5"].ToString() == "" || ds1.Tables[0].Rows[0]["tag5"] == null)
                    {
                        txtTag5.Text = "";
                        divTag5.Visible = false;
                    }
                    else
                    {
                        divTag5.Visible = true;
                        lblTag5.Text = ds1.Tables[0].Rows[0]["tag5"].ToString();
                        txtTag5.Text = ds01.Tables[0].Rows[0][5].ToString();
                    }
                    /// For Tag6
                    if (ds1.Tables[0].Rows[0]["tag6"].ToString() == "" || ds1.Tables[0].Rows[0]["tag6"] == null)
                    {
                        txtTag6.Text = "";
                        divTag6.Visible = false;
                    }
                    else
                    {
                        divTag6.Visible = true;
                        lblTag6.Text = ds1.Tables[0].Rows[0]["tag6"].ToString();
                        txtTag6.Text = ds01.Tables[0].Rows[0][6].ToString();
                    }
                    /// For Tag7
                    if (ds1.Tables[0].Rows[0]["tag7"].ToString() == "" || ds1.Tables[0].Rows[0]["tag7"] == null)
                    {
                        txtTag7.Text = "";
                        divTag7.Visible = false;
                    }
                    else
                    {
                        divTag7.Visible = true;
                        lblTag7.Text = ds1.Tables[0].Rows[0]["tag7"].ToString();
                        txtTag7.Text = ds01.Tables[0].Rows[0][7].ToString();
                    }
                    /// For Tag8
                    if (ds1.Tables[0].Rows[0]["tag8"].ToString() == "" || ds1.Tables[0].Rows[0]["tag8"] == null)
                    {
                        txtTag8.Text = "";
                        divTag8.Visible = false;
                    }
                    else
                    {
                        divTag8.Visible = true;
                        lblTag8.Text = ds1.Tables[0].Rows[0]["tag8"].ToString();
                        txtTag8.Text = ds01.Tables[0].Rows[0][8].ToString();
                    }
                    /// For Tag9
                    if (ds1.Tables[0].Rows[0]["tag9"].ToString() == "" || ds1.Tables[0].Rows[0]["tag9"] == null)
                    {
                        txtTag9.Text = "";
                        divTag9.Visible = false;
                    }
                    else
                    {
                        divTag9.Visible = true;
                        lblTag9.Text = ds1.Tables[0].Rows[0]["tag9"].ToString();
                        txtTag9.Text = ds01.Tables[0].Rows[0][9].ToString();
                    }
                    /// For Tag10
                    if (ds1.Tables[0].Rows[0]["tag10"].ToString() == "" || ds1.Tables[0].Rows[0]["tag10"] == null)
                    {
                        txtTag10.Text = "";
                        divTag10.Visible = false;
                    }
                    else
                    {
                        divTag10.Visible = true;
                        lblTag10.Text = ds1.Tables[0].Rows[0]["tag10"].ToString();
                        txtTag10.Text = ds01.Tables[0].Rows[0][10].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private string DwnldFile(string DocUUID)
        {
            // At first download the selected file from alfresco and save it to <TempDownload> Folder and then open the file
            //Download start
            // Initialise the reference to the spaces store
            Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
            spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
            spacesStore.address = "SpacesStore";

            Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
            referenceForNode.store = spacesStore;
            referenceForNode.uuid = DocUUID;//Doc UUID

            Alfresco.ContentWebService.Reference[] obj_new = new Alfresco.ContentWebService.Reference[] { referenceForNode };
            Alfresco.ContentWebService.Predicate sourcePredicate = new Alfresco.ContentWebService.Predicate();
            sourcePredicate.Items = obj_new;

            WebServiceFactory wsFA = new WebServiceFactory();
            wsFA.UserName = Session["AdmUserID"].ToString();
            wsFA.Ticket = Session["AdmTicket"].ToString();
            Alfresco.ContentWebService.Content[] readResult = wsFA.getContentService().read(sourcePredicate, Constants.PROP_CONTENT);

            String ticketURL = "?ticket=" + wsFA.Ticket;
            String downloadURL = readResult[0].url + ticketURL;
            Uri address = new Uri(downloadURL);

            string url = downloadURL;
            string newSaveFileName = Guid.NewGuid() + ".pdf";
            Session["RWFOldFile"] = newSaveFileName;
            string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
            SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
            ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
            return newSaveFileName;
            //Download end
        }

        protected void PopulateDropdown()
        {
            try
            {
                DBClass DBObj = new DBClass();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();

                //....Task
                DBObj.WF_Log_ID = Session["WFUpdtWFLogID"].ToString();
                DBObj.Step_No = Convert.ToInt32(Session["WFUpdtStepNo"]);
                ds01.Reset();
                ds01 = DBObj.DDAssignedTask();
                ddTask.DataSource = ds01;
                ddTask.DataTextField = "task_name";
                ddTask.DataValueField = "task_id";
                ddTask.DataBind();

                //....Cabinet
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinet.DataSource = ds01;
                ddCabinet.DataTextField = "cab_name";
                ddCabinet.DataValueField = "cab_uuid";
                ddCabinet.DataBind();
                PopulateDrawer(ddCabinet.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// As Drawer dropdown is dependent of Cabinet, so the Drawer dropdown is populated with respect to Cabinet
        /// </summary>
        /// <param name="SelCab"></param>
        protected void PopulateDrawer(string SelCab)
        {
            try
            {
                //....Drawer
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCab, Session["UserID"].ToString());
                ddDrawer.DataSource = ds01;
                ddDrawer.DataTextField = "drw_name";
                ddDrawer.DataValueField = "drw_uuid";
                ddDrawer.DataBind();
                if (ddDrawer.SelectedValue != "")
                {
                    PopulateFolder(ddDrawer.SelectedValue);
                }
                else
                {
                    PopulateFolder("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// As Folder dropdown is dependent of Drawer, so the Folder dropdown is populated with respect to Drawer
        /// </summary>
        /// <param name="SelDrw"></param>
        protected void PopulateFolder(string SelDrw)
        {
            try
            {
                //....Folder
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrw, Session["UserID"].ToString());
                ddFolder.DataSource = ds01;
                ddFolder.DataTextField = "fld_name";
                ddFolder.DataValueField = "fld_uuid";
                ddFolder.DataBind();
                if (ddFolder.SelectedValue != "")
                {
                    PopulateDocument(ddFolder.SelectedValue);
                }
                else
                {
                    PopulateDocument("");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// As Document dropdown is dependent of Folder, so the Document dropdown is populated with respect to Folder
        /// </summary>
        /// <param name="SelFld"></param>
        protected void PopulateDocument(string SelFld)
        {
            try
            {
                //....Document
                DBClass DBObj = new DBClass();
                DataSet ds1 = new DataSet();
                DBObj.FldID = SelFld;
                ds1 = DBObj.DDDocument();
                ddDocument.DataSource = ds1;
                ddDocument.DataTextField = "doc_name";
                ddDocument.DataValueField = "uuid";
                ddDocument.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ddCabinet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateDrawer(ddCabinet.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateFolder(ddDrawer.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateDocument(ddFolder.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void optFiling_CheckedChanged(object sender, EventArgs e)
        {
            divFiling.Visible = true;
            divLocal.Visible = false;
        }

        protected void optLocal_CheckedChanged(object sender, EventArgs e)
        {
            divFiling.Visible = false;
            divLocal.Visible = true;
        }

        protected string FetchLocation(string DeptID, string DocTypeID)
        {
            try
            {
                DBClass DBObj = new DBClass();
                DataSet ds01 = new DataSet();
                ds01 = DBObj.FetchUploadedLocation(DeptID, DocTypeID);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    return ds01.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void cmdUpdtMetaData_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();

            return ImageData; //return the byte data
        }

        protected void cmdAppend_Click(object sender, EventArgs e)
        {
            try
            {
                string OutputFile = Server.MapPath("TempDownload") + "\\";
                string mFile = "";
                if (optFiling.Checked == true)
                {
                    if (ddDocument.SelectedValue == null || ddDocument.SelectedValue == "")
                    {
                        throw new Exception("Please Select the Document which you want to Append!!");
                    }
                    else
                    {
                        //Save the Selected Document into the Server's specific folder
                        // At first download the selected file from alfresco and save it to <TempDownload> Folder and then open the file
                        //Download start
                        // Initialise the reference to the spaces store
                        Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                        spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                        spacesStore.address = "SpacesStore";

                        Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                        referenceForNode.store = spacesStore;
                        referenceForNode.uuid = ddDocument.SelectedValue;//Doc UUID

                        Alfresco.ContentWebService.Reference[] obj_new = new Alfresco.ContentWebService.Reference[] { referenceForNode };
                        Alfresco.ContentWebService.Predicate sourcePredicate = new Alfresco.ContentWebService.Predicate();
                        sourcePredicate.Items = obj_new;

                        WebServiceFactory wsFA = new WebServiceFactory();
                        wsFA.UserName = Session["AdmUserID"].ToString();
                        wsFA.Ticket = Session["AdmTicket"].ToString();
                        Alfresco.ContentWebService.Content[] readResult = wsFA.getContentService().read(sourcePredicate, Constants.PROP_CONTENT);

                        String ticketURL = "?ticket=" + wsFA.Ticket;
                        String downloadURL = readResult[0].url + ticketURL;
                        Uri address = new Uri(downloadURL);

                        string url = downloadURL;
                        string newSaveFileName = Guid.NewGuid() + ".pdf";
                        string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
                        SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                        ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                        Session["RWFAppendFileName"] = newSaveFileName;
                    }
                }
                else if (optLocal.Checked == true)
                {
                    string fn = System.IO.Path.GetFileName(fuBrowse.PostedFile.FileName);
                    string postfix = System.IO.Path.GetExtension(fn).ToString().ToLower();
                    if (postfix == ".pdf")
                    {
                        mFile = Guid.NewGuid() + postfix;
                        Session["RWFAppendFileName"] = mFile;
                        fuBrowse.PostedFile.SaveAs(OutputFile + mFile);
                    }
                    else
                    {
                        throw new Exception("You Can Attach Only pdf Files.");
                    }
                }
                // Now Append the doc
                string FileName1 = hfDocument.Value;
                string FileName2 = mFile;
                string LicenseKey = "";
                string ServerIPAddress = "";
                // Fetch ServerConfig Details Start
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet dsServerConfig = new DataSet();
                dsServerConfig.Reset();
                dsServerConfig = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                if (dsServerConfig.Tables[0].Rows.Count > 0)
                {
                    LicenseKey = dsServerConfig.Tables[0].Rows[0][0].ToString();
                    ServerIPAddress = dsServerConfig.Tables[0].Rows[0][1].ToString();
                }
                // Fetch ServerConfig Details End
                int Result = QP.UnlockKey(LicenseKey);
                if (Result == 1)
                {
                    QP.LoadFromFile(OutputFile + FileName1, "");
                    int PrimaryDoc = QP.SelectedDocument();

                    QP.LoadFromFile(OutputFile + FileName2, "");
                    int SecondaryDoc = QP.SelectedDocument();

                    QP.SelectDocument(PrimaryDoc);
                    QP.MergeDocument(SecondaryDoc);
                    string NewID = Guid.NewGuid().ToString();
                    QP.SaveToFile(OutputFile + NewID + ".pdf");
                    Session["DocNewVersion"] = NewID + ".pdf";
                    hfUpdtDoc.Value = Session["DocNewVersion"].ToString();
                }
                else
                {
                    throw new Exception("Invalid Quick PDF license key");
                }
            }
            catch (Exception ex)
            {

            }
        }

        // NA
        /// <summary>
        /// TaskID="" for the preamble & postamble actions but will be the task code when any interactive action will be performed
        /// </summary>
        /// <param name="WFLogID"></param>
        /// <param name="StepNo"></param>
        /// <param name="TaskDoneDate"></param>
        /// <param name="TaskID"></param>
        /// <param name="Comments"></param>
        /// <returns></returns>
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

                // Fetch the tasks for this particulat WFLogID & Step No
                cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null", con);
                ds001 = new DataSet();
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds001.Tables[0].Rows.Count; i++)
                    {
                        /// For Preamble Mail Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PREEMAIL")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            // Send Mail
                            PreambleEmail ObjPreambleEmail = new PreambleEmail();
                            ObjPreambleEmail.SendPreMail(WFLogID, StepNo, "PREEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                        }
                        /// For Preamble Mail End
                        /// For Preamble Copy Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PRECOPY")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            PreambleCopy ObjPreambleCopy = new PreambleCopy();
                            string PreCPOutput = ObjPreambleCopy.PreCopy(WFLogID, StepNo, "PRECOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                            CheckAction(WFLogID, StepNo, DateTime.Now, "PRECOPY", PreCPOutput);
                        }
                        /// For Preamble Copy End
                        /// For Preamble Conditinal Mail Start
                        if (ds001.Tables[0].Rows[i][2].ToString() == "PRECOND")
                        {
                            // Update status in database
                            cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Preamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            // Send Mail
                            PreambleConditionalEmail ObjPreambleConditionalEmail = new PreambleConditionalEmail();
                            ObjPreambleConditionalEmail.SendPreCondMail(WFLogID, StepNo, "PRECOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                        }
                        /// For Preamble Conditinal Mail End

                        /// Check is there any Interactive actions or not start
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND')", con);
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
                                // For APPROVE Task
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
                                    }
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    #endregion
                                }
                                // For REJECT Task
                                else if (TaskID == "REJECT")
                                {
                                    #region
                                    // Need to Mail to the previous stages' users
                                    RejectEmail ObjRejectEmail = new RejectEmail();
                                    ObjRejectEmail.RejectMail(WFLogID, StepNo, Session["CompCode"].ToString());

                                    // Insert into Rejected List
                                    cmd = new SqlCommand("insert into RejectedList(wf_log_id,step_no,user_id,rejected_dt,comments,CompCode) values('" + WFLogID + "','" + StepNo + "','','" + TaskDoneDate + "','" + Comments + "','" + Session["CompCode"].ToString() + "')", con);
                                    cmd.ExecuteNonQuery();
                                    // Update wf_log_task & wf_log_dtl for previous stage start
                                    // Current Stage
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                                    cmd.ExecuteNonQuery();
                                    cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                                    cmd.ExecuteNonQuery();

                                    // Pervious Stage
                                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                    DataSet ds114 = new DataSet();
                                    SqlDataAdapter adapter114 = new SqlDataAdapter(cmd);
                                    ds114.Reset();
                                    adapter114.Fill(ds114);
                                    if (ds114.Tables[0].Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        CheckAction(WFLogID, StepNo - 1, DateTime.Now, "", "");
                                    }
                                    else
                                    {
                                        CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    }
                                    // Update wf_log_task & wf_log_dtl for previous stage end
                                    #endregion
                                }
                                else if (TaskID == "REVIEW")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_done_dt is null and (task_id='PRECOPY' or task_id='PREEMAIL' or task_id='PRECOND')", con);
                            ds004 = new DataSet();
                            SqlDataAdapter adapter004 = new SqlDataAdapter(cmd);
                            ds004.Reset();
                            adapter004.Fill(ds004);
                            if (ds004.Tables[0].Rows.Count > 0)
                            {

                            }
                            else
                            {
                                /// For Postamble Mail Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTEMAIL")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleEmail ObjPostambleEmail = new PostambleEmail();
                                    ObjPostambleEmail.SendPostMail(WFLogID, StepNo, "POSTEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                /// For Postamble Mail End
                                /// For Postamble Copy Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTCOPY")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleCopy ObjPostambleCopy = new PostambleCopy();
                                    string PostCPOutput = ObjPostambleCopy.PostCopy(WFLogID, StepNo, "POSTCOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "POSTCOPY", PostCPOutput);
                                }
                                /// For Postamble Copy End
                                /// For Postamble Conditional Mail Start
                                if (ds001.Tables[0].Rows[i][2].ToString() == "POSTCOND")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds001.Tables[0].Rows[i][2].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleConditionalEmail ObjPostambleConditionalEmail = new PostambleConditionalEmail();
                                    ObjPostambleConditionalEmail.SendPostCondMail(WFLogID, StepNo, "POSTCOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                /// For Postamble Conditional Mail End
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
                        CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                    }
                    else
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Completed' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "'", con);
                        cmd.ExecuteNonQuery();

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
                        }
                    }

                }
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                //MessageBox(ex.Message);
            }
        }

    }
}