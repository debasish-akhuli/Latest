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
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;

namespace DMS
{
    public partial class doc_mast : System.Web.UI.Page
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
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Header.DataBind();
                if (!IsPostBack)
                {
                    // UF is used for "Uploaded Folder" which is set from the e-Filing System's Upload button
                    if (Request.QueryString["UF"] != null)
                    {
                        if (Request.QueryString["UF"].ToString() != "")
                        {
                            
                        }
                        else
                        {
                            // Set the session variables blank which are used to set the previous selected path start
                            Session["SelectedCabUUID"] = "";
                            Session["SelectedDrwUUID"] = "";
                            Session["SelectedFldUUID"] = "";
                            Session["SelectedDocID"] = "";
                            // Set the session variables blank which are used to set the previous selected path end
                        }
                    }
                    else
                    {
                        // Set the session variables blank which are used to set the previous selected path start
                        Session["SelectedCabUUID"] = "";
                        Session["SelectedDrwUUID"] = "";
                        Session["SelectedFldUUID"] = "";
                        Session["SelectedDocID"] = "";
                        // Set the session variables blank which are used to set the previous selected path end
                    }
                    if (Request.QueryString["NewFile"] != null)
                    {
                        if (Request.QueryString["NewFile"].ToString() != "")
                        {
                            Session["NewFile"] = Request.QueryString["NewFile"].ToString();
                            Session["TemplateUUID"] = Request.QueryString["TemplateUUID"].ToString();
                        }
                    }
                    
                    cmdAddMaster.Attributes.Add("OnClick", "javascript: return FormValidation();");
                    cmdAddMaster.Attributes.Add("onclick", "document.body.style.cursor = 'wait';");
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
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
                        PopulateDropdown();
                        // For uploading a doc in a pre selected folder
                        if (Request.QueryString["UF"] != null)
                        {
                            if (Request.QueryString["UF"].ToString() != "")
                            {
                                SetUploadedLocation(Request.QueryString["UF"].ToString());
                            }
                        }

                        TagDisplay(ddDocType.SelectedValue);
                        // Fetch the Metadata values start
                        if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                        {
                            divBrowse.Visible = false;
                            SetDocDetails(Session["NewFile"].ToString(), Session["TemplateUUID"].ToString());
                        }
                        else
                        {
                            if (ddDocType.SelectedValue == "GENERAL")
                            {
                                divUpldLoc.Visible = false;
                                divUpldCabinet.Visible = true;
                                divUpldDrawer.Visible = true;
                                divUpldFolder.Visible = true;
                                PopulateCabinet();
                            }
                            else
                            {
                                divUpldLoc.Visible = true;
                                divUpldCabinet.Visible = false;
                                divUpldDrawer.Visible = false;
                                divUpldFolder.Visible = false;
                            }
                            divBrowse.Visible = true;
                        }
                        // Fetch the Metadata values end
                        #region For Checked In
                        if (Request.QueryString["CIDocUUID"] != null)
                        {
                            if (Request.QueryString["CIDocUUID"].ToString() != "")
                            {
                                string CIDocName = "";
                                
                                DataSet ds01 = new DataSet();
                                ds01.Reset();
                                ds01 = ObjClassStoreProc.DocDetails(Request.QueryString["CIDocUUID"].ToString(), Session["CompCode"].ToString());
                                if (ds01.Tables[0].Rows.Count > 0)
                                {
                                    CIDocName = ds01.Tables[0].Rows[0][1].ToString();
                                    Session["CIDocTypeID"] = ds01.Tables[0].Rows[0][2].ToString();
                                    Session["CIDeptID"] = ds01.Tables[0].Rows[0][3].ToString();
                                    hfUUID.Value = ds01.Tables[0].Rows[0][5].ToString();
                                    txtTag1.Text = ds01.Tables[0].Rows[0][8].ToString();
                                    txtTag2.Text = ds01.Tables[0].Rows[0][9].ToString();
                                    txtTag3.Text = ds01.Tables[0].Rows[0][10].ToString();
                                    txtTag4.Text = ds01.Tables[0].Rows[0][11].ToString();
                                    txtTag5.Text = ds01.Tables[0].Rows[0][12].ToString();
                                    txtTag6.Text = ds01.Tables[0].Rows[0][13].ToString();
                                    txtTag7.Text = ds01.Tables[0].Rows[0][14].ToString();
                                    txtTag8.Text = ds01.Tables[0].Rows[0][15].ToString();
                                    txtTag9.Text = ds01.Tables[0].Rows[0][16].ToString();
                                    txtTag10.Text = ds01.Tables[0].Rows[0][17].ToString();
                                    txtDocDesc.Text = ds01.Tables[0].Rows[0][20].ToString();
                                    TagDisplay(Session["CIDocTypeID"].ToString());
                                    lblDocName.Text = ObjFetchOnlyNameORExtension.FetchOnlyDocName(CIDocName) + "." + ObjFetchOnlyNameORExtension.FetchOnlyDocExt(CIDocName);
                                    Session["OldDocName"] = ObjFetchOnlyNameORExtension.FetchOnlyDocName(CIDocName) + " ARCHIVE " + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "." + ObjFetchOnlyNameORExtension.FetchOnlyDocExt(CIDocName);

                                    txtDocName.Visible = false;
                                    divDocType.Visible = false;
                                    divDept.Visible = false;
                                    divUpldCabinet.Visible = false;
                                    divUpldDrawer.Visible = false;
                                    divUpldFolder.Visible = false;
                                    divUpldLoc.Visible = false;
                                    ddDocType.Enabled = false;
                                    ddDept.Enabled = false;
                                    txtDocDesc.Enabled = false;
                                }
                            }
                        }
                        #endregion

                        #region For Uploading a Document which came from Folder Sniffer Program
                        if (Request.QueryString["Source"] != null)
                        {
                            if (Request.QueryString["Source"].ToString() == "FS")
                            {
                                txtDocName.Visible = false;
                                lblDocName.Text = Request.QueryString["DocName"].ToString();
                                txtDocName.Text = Request.QueryString["DocName"].ToString();
                                divBrowse.Visible = false;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                    DataSet ds001 = new DataSet();
                    ds001 = ObjClassStoreProc.UserInfoPassingUserID(Session["UserID"].ToString());
                    ddDept.SelectedValue = ds001.Tables[0].Rows[0][6].ToString();
                    DeptSettings();
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Set the Doc Details in the corresponding fields for editable form
        /// </summary>
        protected void SetDocDetails(string FileName, string ActualFileUUID)
        {
            try
            {
                // Fetch Actual File's DocType Start
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                if (Session["AccessControl"].ToString() != "")
                {
                    //Dept Setting
                    string mDeptName = "";
                    string mActualDocName = "";
                    ds01 = ObjClassStoreProc.DocDetails(ActualFileUUID, Session["CompCode"].ToString());
                    mActualDocName = ds01.Tables[0].Rows[0][1].ToString();
                    mDeptName=mActualDocName.Substring(0, mActualDocName.IndexOf('_'));

                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DeptDetails(mDeptName);
                    ddDept.SelectedValue = ds01.Tables[0].Rows[0][0].ToString();
                                        
                    divBrowse.Visible = false;                    
                    txtDocDesc.Text = FileName;
                    ddDocType.Enabled = false;
                    ddDept.Enabled = false;
                    txtDocName.Enabled = false;
                    txtDocDesc.Enabled = false;
                    txtTag1.Enabled = false;
                    txtTag2.Enabled = false;
                    txtTag3.Enabled = false;
                    txtTag4.Enabled = false;
                    txtTag5.Enabled = false;
                    txtTag6.Enabled = false;
                    txtTag7.Enabled = false;
                    txtTag8.Enabled = false;
                    txtTag9.Enabled = false;
                    txtTag10.Enabled = false;
                }
                //Doc Type Setting
                ds01.Reset();
                ds01 = ObjClassStoreProc.DocDetails(ActualFileUUID, Session["CompCode"].ToString());
                ddDocType.SelectedValue = ds01.Tables[0].Rows[0][2].ToString();
                FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
                TagDisplay(ddDocType.SelectedValue);
                // Fetch Actual File's DocType End
                // Fetch Start
                string Full_Path = Server.MapPath("TempDownload") + "\\" + FileName;
                string LicenseKey = "";
                string ServerIPAddress = "";
                // Fetch ServerConfig Details Start
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    LicenseKey = ds01.Tables[0].Rows[0][0].ToString();
                    ServerIPAddress = ds01.Tables[0].Rows[0][1].ToString();
                }
                // Fetch ServerConfig Details End
                int Result = QP.UnlockKey(LicenseKey);
                if (Result == 1)
                {
                    QP.LoadFromFile(Full_Path, "");
                    txtDocName.Text = FileName;

                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DocTypeDetails(ddDocType.SelectedValue, Session["CompCode"].ToString());
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        // Here 12 to 21 are the field nos in the database
                        // For Tag1
                        if (ds01.Tables[0].Rows[0][12].ToString() != "0")
                        {
                            txtTag1.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][12].ToString()));
                        }
                        else
                        {
                            txtTag1.Text = "";
                        }

                        // For Tag2
                        if (ds01.Tables[0].Rows[0][13].ToString() != "0")
                        {
                            txtTag2.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][13].ToString()));
                        }
                        else
                        {
                            txtTag2.Text = "";
                        }

                        // For Tag3
                        if (ds01.Tables[0].Rows[0][14].ToString() != "0")
                        {
                            txtTag3.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][14].ToString()));
                        }
                        else
                        {
                            txtTag3.Text = "";
                        }

                        // For Tag4
                        if (ds01.Tables[0].Rows[0][15].ToString() != "0")
                        {
                            txtTag4.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][15].ToString()));
                        }
                        else
                        {
                            txtTag4.Text = "";
                        }

                        // For Tag5
                        if (ds01.Tables[0].Rows[0][16].ToString() != "0")
                        {
                            txtTag5.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][16].ToString()));
                        }
                        else
                        {
                            txtTag5.Text = "";
                        }

                        // For Tag6
                        if (ds01.Tables[0].Rows[0][17].ToString() != "0")
                        {
                            txtTag6.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][17].ToString()));
                        }
                        else
                        {
                            txtTag6.Text = "";
                        }

                        // For Tag7
                        if (ds01.Tables[0].Rows[0][18].ToString() != "0")
                        {
                            txtTag7.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][18].ToString()));
                        }
                        else
                        {
                            txtTag7.Text = "";
                        }

                        // For Tag8
                        if (ds01.Tables[0].Rows[0][19].ToString() != "0")
                        {
                            txtTag8.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][19].ToString()));
                        }
                        else
                        {
                            txtTag8.Text = "";
                        }

                        // For Tag9
                        if (ds01.Tables[0].Rows[0][20].ToString() != "0")
                        {
                            txtTag9.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][20].ToString()));
                        }
                        else
                        {
                            txtTag9.Text = "";
                        }

                        // For Tag10
                        if (ds01.Tables[0].Rows[0][21].ToString() != "0")
                        {
                            txtTag10.Text = QP.GetFormFieldValue(Convert.ToInt32(ds01.Tables[0].Rows[0][21].ToString()));
                        }
                        else
                        {
                            txtTag10.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox("- Invalid license key -");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To populate the dropdowns
        /// </summary>
        protected void PopulateDropdown()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //....Department 
                ds01.Reset();
                ds01 = ObjClassStoreProc.DeptAllBasedOnCompCode(Session["CompCode"].ToString());
                ddDept.DataSource = ds01;
                ddDept.DataTextField = "dept_name";
                ddDept.DataValueField = "dept_id";
                ddDept.DataBind();                

                //....Doc Type
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectDocTypeCompBased(Session["CompCode"].ToString());
                ddDocType.DataSource = ds01;
                ddDocType.DataTextField = "doc_type_name";
                ddDocType.DataValueField = "doc_type_id";
                ddDocType.DataBind();

                // For uploading a doc in a pre selected folder
                if (Request.QueryString["UF"] != null)
                {
                    if (Request.QueryString["UF"].ToString() != "")
                    {
                        ddDept.SelectedValue = "MISC";
                        ddDocType.SelectedValue = "GENERAL";
                    }
                }

                // Fetch the Location
                FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
                DocTypeSettings();
                DeptSettings();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// This is used to popup a message box using javascript
        /// </summary>
        /// <param name="msg"></param>
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
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

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] ImageData = new byte[fs.Length];
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return ImageData;
        }

        /// <summary>
        /// To populate the cabinet dropdown
        /// </summary>
        protected void PopulateCabinet()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.CabinetPermissionM(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinet.DataSource = ds01;
                ddCabinet.DataTextField = "cab_name";
                ddCabinet.DataValueField = "cab_uuid";
                ddCabinet.DataBind();
                // For uploading a doc in a pre selected folder
                if (Request.QueryString["UF"] != null)
                {
                    if (Request.QueryString["UF"].ToString() != "")
                    {
                        ddCabinet.SelectedValue = Request.QueryString["UC"].ToString();
                    }
                }
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
        protected void PopulateDrawer(string SelCabID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerPermissionM(SelCabID, Session["UserID"].ToString());
                ddDrawer.DataSource = ds01;
                ddDrawer.DataTextField = "drw_name";
                ddDrawer.DataValueField = "drw_uuid";
                ddDrawer.DataBind();
                // For uploading a doc in a pre selected folder
                if (Request.QueryString["UF"] != null)
                {
                    if (Request.QueryString["UF"].ToString() != "")
                    {
                        ddDrawer.SelectedValue = Request.QueryString["UD"].ToString();
                    }
                }
                PopulateFolder(ddDrawer.SelectedValue);
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
        protected void PopulateFolder(string SelDrawerUUID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderPermissionM(SelDrawerUUID, Session["UserID"].ToString());
                ddFolder.DataSource = ds01;
                ddFolder.DataTextField = "fld_name";
                ddFolder.DataValueField = "fld_uuid";
                ddFolder.DataBind();
                // For uploading a doc in a pre selected folder
                if (Request.QueryString["UF"] != null)
                {
                    if (Request.QueryString["UF"].ToString() != "")
                    {
                        ddFolder.SelectedValue = Request.QueryString["UF"].ToString();
                    }
                }
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
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
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
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                PopulateFolder(ddDrawer.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void UploadRevisedVersion()
        {
            try
            {
                string UDocType = Session["CIDocTypeID"].ToString();
                string UDept = Session["CIDeptID"].ToString();
                txtDocName.Text = lblDocName.Text;
                string DocNameToBeUploaded = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                double FileSize = 0;

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();

                FileSize = Math.Round(Convert.ToDouble(btnBrows.PostedFile.ContentLength) / 1024, 2);
                #region Validation of space if it is exceeding or not
                SqlDataAdapter adpAvl01;
                DataSet dsAvl01 = new DataSet();
                double AvailableSpace = 0;

                cmd = new SqlCommand("select TotalSpace,UsedSpace,AvailableSpace from ServerConfig where CompCode='" + Session["CompCode"].ToString() + "'", con);
                adpAvl01 = new SqlDataAdapter(cmd);
                dsAvl01.Reset();
                adpAvl01.Fill(dsAvl01);
                AvailableSpace = Convert.ToDouble(dsAvl01.Tables[0].Rows[0][2].ToString());
                if (FileSize > AvailableSpace)
                {
                    throw new Exception("You do not have enough space to upload this Document. Please contact with Administrator.");
                }
                #endregion

                // Initialise the reference to the spaces store
                this.spacesStore = new Alfresco.RepositoryWebService.Store();
                this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                this.spacesStore.address = "SpacesStore";
                //create a predicate with the first CMLCreate result
                Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                referenceForNode.store = this.spacesStore;
                referenceForNode.uuid = Request.QueryString["CIDocUUID"].ToString(); // Selected Doc's UUID

                Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                sourcePredicate.Items = obj_new;

                // Create the properties list
                NamedValue[] properties1 = new NamedValue[2];

                NamedValue nameProperty1 = new NamedValue();
                nameProperty1.name = Constants.PROP_NAME;
                nameProperty1.value = Session["OldDocName"].ToString();
                nameProperty1.isMultiValue = false;
                properties1[0] = nameProperty1;

                nameProperty1 = new NamedValue();
                nameProperty1.name = Constants.PROP_TITLE;
                nameProperty1.value = Session["OldDocName"].ToString();
                nameProperty1.isMultiValue = false;
                properties1[1] = nameProperty1;

                // cml update
                CMLUpdate update = new CMLUpdate();
                update.property = properties1;
                update.where = sourcePredicate;

                CML cmlUpdate = new CML();
                cmlUpdate.update = new CMLUpdate[] { update };

                WebServiceFactory wsF1 = new WebServiceFactory();
                wsF1.UserName = Session["AdmUserID"].ToString();
                wsF1.Ticket = Session["AdmTicket"].ToString();
                wsF1.getRepositoryService().update(cmlUpdate);
                // Alfresco Part End

                // For Checked In
                result = ObjClassStoreProc.UpdateDocMast(1, Session["OldDocName"].ToString(), "", "", "", "", "", DateTime.Now, "", "", "", "", "", "", "", "", "", "", "", Request.QueryString["CIDocUUID"].ToString(), "", Session["CompCode"].ToString());
                
                result = ObjClassStoreProc.UpdatePermissionAllUser("X", Request.QueryString["CIDocUUID"].ToString(), "admin", Session["CompCode"].ToString());
                
                String fileName = "";
                string fileExt = "";
                /// Insert into the database using Store Procedure <DocMast_Insert> start
                if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                {
                    int start = txtDocName.Text.Trim().LastIndexOf(".") + 1;
                    if (start == 0)
                    {
                        DocNameToBeUploaded = txtDocName.Text.Trim() + ".pdf";
                    }
                    else
                    {
                        int length = txtDocName.Text.Trim().Length - start;
                        string fileNameExt = txtDocName.Text.Trim().Substring(start, length);
                        if (fileNameExt == "pdf")
                        {
                            DocNameToBeUploaded = txtDocName.Text.Trim();
                        }
                        else
                        {
                            throw new Exception("Only pdf files will be uploaded");
                        }
                    }
                }
                else
                {
                    DocNameToBeUploaded = txtDocName.Text.Trim();
                }
                if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                {
                    /// Checking the Alias File Start
                    String fileNameExt = "";
                    fileName = txtDocName.Text.Trim();

                    if (fileName == null || fileName.Equals(""))
                    {
                        return;
                    }

                    int start = fileName.LastIndexOf(".") + 1;
                    int length = fileName.Length - start;
                    fileNameExt = fileName.Substring(start, length);
                    if (fileNameExt == "pdf")
                    {

                    }
                    else
                    {
                        if (start == 0)
                        {
                            fileName = fileName + ".pdf";
                            fileNameExt = "pdf";
                            Session["FileNameExt"] = fileNameExt;
                        }
                        else if (start > 1)
                        {
                            throw new Exception("Only pdf files will be uploaded");
                        }
                        else
                        {
                            throw new Exception("Invalid document Name");
                        }
                    }
                    /// Checking the Alias File End
                }
                else
                {
                    /// Checking the Original File Start
                    String file = btnBrows.FileName;

                    if (file == null || file.Equals(""))
                    {
                        return;
                    }

                    int start1 = file.LastIndexOf(".") + 1;
                    int length1 = file.Length - start1;
                    fileExt = file.Substring(start1, length1);
                    if (start1 == 0)
                    {
                        throw new Exception("Unrecognized document");
                    }
                    /// Checking the Original File End
                    /// Checking the Alias File Start
                    String fileNameExt = "";
                    fileName = txtDocName.Text.Trim();

                    if (fileName == null || fileName.Equals(""))
                    {
                        return;
                    }

                    int start = fileName.LastIndexOf(".") + 1;
                    int length = fileName.Length - start;
                    fileNameExt = fileName.Substring(start, length);
                    string ActualFileName = "";
                    if (start == 0)
                    {
                        fileName = fileName + "." + fileExt;
                    }
                    else
                    {
                        ActualFileName = fileName.Substring(0, start-1);
                        fileName = ActualFileName + "." + fileExt;
                    }
                    Session["FileNameExt"] = fileExt;
                    /// Checking the Alias File End
                }
                DocNameToBeUploaded = fileName;
                FileSize = Math.Round(Convert.ToDouble(btnBrows.PostedFile.ContentLength) / 1024, 2);

                #region Alfresco Part Start
                Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                spacesStore.address = "SpacesStore";

                // Create the parent reference, the company home folder
                Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
                parentReference.store = spacesStore;
                parentReference.uuid = this.hfUUID.Value; // Folder's uuid

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

                // Open the file and convert to byte array 
                Stream inputStream;
                byte[] bytes;
                Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
                if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                {
                    bytes = StreamFile(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                    contentFormat.mimetype = "application/pdf";
                }
                else
                {
                    inputStream = btnBrows.PostedFile.InputStream;
                    int bufferSize = (int)inputStream.Length;
                    bytes = new byte[bufferSize];
                    inputStream.Read(bytes, 0, bufferSize);
                    inputStream.Close();
                    FileType ObjFileType = new FileType();
                    contentFormat.mimetype = ObjFileType.GetFileType(Session["FileNameExt"].ToString());// "application/pdf";
                }

                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = Session["AdmUserID"].ToString();
                wsF.Ticket = Session["AdmTicket"].ToString();
                wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);
                #endregion

                result = ObjClassStoreProc.InsertDocMast(DocNameToBeUploaded, txtDocDesc.Text.Trim(), hfUUID.Value, ddDocType.SelectedValue, ddDept.SelectedValue, Session["UserID"].ToString(), DateTime.Now, txtTag1.Text.Trim(), txtTag2.Text.Trim(), txtTag3.Text.Trim(), txtTag4.Text.Trim(), txtTag5.Text.Trim(), txtTag6.Text.Trim(), txtTag7.Text.Trim(), txtTag8.Text.Trim(), txtTag9.Text.Trim(), txtTag10.Text.Trim(), "", newContentNode.uuid, newContentNode.uuid + "/" + fileName.Replace(" ", "%20"), "CI", Session["CompCode"].ToString(), FileSize);
                if (Convert.ToInt32(result) == -1)
                {
                    throw new Exception("Document already exists in this folder!");
                }
                else
                {
                    cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + FileSize + "' where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    UserRights RightsObj = new UserRights();
                    ds01.Reset();
                    ds01 = RightsObj.FetchPermission(hfUUID.Value, Session["CompCode"].ToString());
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(newContentNode.uuid, "Document", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                        }
                    }

                    /// Call function for Meta Tags insertion
                    if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                    {
                        InsertMetaTags(newContentNode.uuid, Session["NewFile"].ToString());
                    }
                    else
                    {
                        InsertMetaTags(newContentNode.uuid, fileName);
                    }
                    /// Start the Default Workflow according to the Department & Doc Type combination start
                    /// Store the Last inserted Doc ID into <Session["DocId"]>
                    Session["DocId"] = result;
                    result = ObjClassStoreProc.DefinedWF(ddDocType.SelectedValue, ddDept.SelectedValue,Session["CompCode"].ToString());
                    if (Convert.ToInt32(result) == -111) /// If the Workflow is not defined for the Dept & Doc Type Combination; default workflow will be assigned to admin
                    {
                        Redirect2Home("Document uploaded successfully!", "home.aspx");
                        PopulateDropdown();
                        txtDocName.Text = "";
                        txtDocDesc.Text = "";
                        txtTag1.Text = "";
                        txtTag2.Text = "";
                        txtTag3.Text = "";
                        txtTag4.Text = "";
                        txtTag5.Text = "";
                        txtTag6.Text = "";
                        txtTag7.Text = "";
                        txtTag8.Text = "";
                        txtTag9.Text = "";
                        txtTag10.Text = "";
                    }
                    else /// If the Workflow is defined for the Dept & Doc Type Combination
                    {
                        /// Store the defined Wokflow ID into <Session["WFId"]>
                        Session["WFId"] = result;
                        result = ObjClassStoreProc.StartDefaultWF(ddDocType.SelectedValue, ddDept.SelectedValue, DateTime.Now, Convert.ToInt16(Session["DocId"]), Convert.ToInt16(Session["WFId"]), Session["UserID"].ToString(), Session["CompCode"].ToString());
                        if (result != "") /// Start the default defined workflow
                        {
                            /// Insert the Workflow based roles and due time into the database table name:<wf_log_dtl>
                            /// Store the <wf_log_id> into <Session["WFLogId"]>
                            /// Select the stage wise roles defined in the workflow
                            Session["WFLogId"] = result;
                            ds01.Reset();
                            ds01 = ObjClassStoreProc.SelectWFDtls(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                {
                                    result = ObjClassStoreProc.StartDefaultWFLogDtl(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), DateTime.Now, ds01.Tables[0].Rows[i]["duration"].ToString(),DateTime.Now, Session["CompCode"].ToString());
                                    // Insert the Log for versioning of the file Start
                                    if (Request.QueryString["NewFile"] != null)
                                    {
                                        result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(),Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]),Session["TemplateUUID"].ToString(),"", Session["CompCode"].ToString());
                                        result = ObjClassStoreProc.WFDocVersionUpdate(Session["WFLogId"].ToString(), 1, Session["TemplateUUID"].ToString(), newContentNode.uuid, Session["CompCode"].ToString());
                                    }
                                    else
                                    {
                                        result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), newContentNode.uuid, "", Session["CompCode"].ToString());
                                        result = ObjClassStoreProc.WFDocVersionUpdate(Session["WFLogId"].ToString(), 1, newContentNode.uuid, newContentNode.uuid, Session["CompCode"].ToString());
                                    }
                                    // Insert the Log for versioning of the file End
                                }
                            }

                            /// Insert the Workflow based roles and tasks into the database table name:<wf_log_task>
                            /// Select the stage wise tasks defined in the workflow
                            ds01.Reset();
                            ds01 = ObjClassStoreProc.SelectWFTasks(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                {
                                    result = ObjClassStoreProc.StartDefaultWFLogTask(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), ds01.Tables[0].Rows[i]["task_id"].ToString(), ds01.Tables[0].Rows[i]["amble_mails"].ToString(), ds01.Tables[0].Rows[i]["amble_msg"].ToString(), ds01.Tables[0].Rows[i]["amble_attach"].ToString(), ds01.Tables[0].Rows[i]["AppendDocUUID"].ToString(), ds01.Tables[0].Rows[i]["amble_url"].ToString(), ds01.Tables[0].Rows[i]["AmbleSub"].ToString(), Session["CompCode"].ToString());
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
                                        result = ObjClassStoreProc.WFLogTaskUpdate(1, Session["WFLogId"].ToString(), "guest@guest.com", Session["InitiatorEmailID"].ToString(), DateTime.Now, "", "",0, Session["CompCode"].ToString());
                                    }
                                    else
                                    {
                                        result = ObjClassStoreProc.WFLogTaskUpdate(1, Session["WFLogId"].ToString(), "init@init.com", dsI01.Tables[0].Rows[0][0].ToString(), DateTime.Now, "", "",0, Session["CompCode"].ToString());
                                    }
                                }
                                // Update the Initiator End
                                CheckAction(Session["WFLogId"].ToString(), 1, DateTime.Now, "", "");
                                if (Session["NewFile"] == null)
                                {

                                }
                                else
                                {
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                }
                                Session["WFTask"] = null;
                                // Update the Doc's Stat in TempDocSaving table
                                if (Session["hfPageControl"] != null)
                                {
                                    if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                    {
                                        result = ObjClassStoreProc.TempDocSavingUpdate(Session["OpenDocName"].ToString(), Session["UserID"].ToString(), DateTime.Now, "Uploaded", Session["CompCode"].ToString());
                                    }
                                }
                                Session["hfPageControl"] = null;
                                Session["AccessControl"] = null;
                                Session["NewFile"] = null;
                                Session["TemplateUUID"] = null;
                                RedirectMessageBox("Document uploaded successfully & the default workflow with tasks has been assigned!", "userhome.aspx");
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
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                }
                                // Update the Doc's Stat in TempDocSaving table
                                if (Session["hfPageControl"] != null)
                                {
                                    if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                    {
                                        result = ObjClassStoreProc.TempDocSavingUpdate(Session["OpenDocName"].ToString(), Session["UserID"].ToString(), DateTime.Now, "Uploaded", Session["CompCode"].ToString());
                                    }
                                }
                                MessageBox("Document uploaded successfully & the default workflow has been assigned!");
                                PopulateDropdown();
                                txtDocName.Text = "";
                                txtDocDesc.Text = "";
                                txtTag1.Text = "";
                                txtTag2.Text = "";
                                txtTag3.Text = "";
                                txtTag4.Text = "";
                                txtTag5.Text = "";
                                txtTag6.Text = "";
                                txtTag7.Text = "";
                                txtTag8.Text = "";
                                txtTag9.Text = "";
                                txtTag10.Text = "";
                            }
                        }
                        else
                        {
                            File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                            File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                            // Update the Doc's Stat in TempDocSaving table
                            if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                            {
                                result = ObjClassStoreProc.TempDocSavingUpdate(Session["OpenDocName"].ToString(), Session["UserID"].ToString(), DateTime.Now, "Uploaded", Session["CompCode"].ToString());
                            }
                            MessageBox("Document uploaded successfully but error in assigning the defined workflow!");
                            PopulateDropdown();
                            txtDocName.Text = "";
                            txtDocDesc.Text = "";
                            txtTag1.Text = "";
                            txtTag2.Text = "";
                            txtTag3.Text = "";
                            txtTag4.Text = "";
                            txtTag5.Text = "";
                            txtTag6.Text = "";
                            txtTag7.Text = "";
                            txtTag8.Text = "";
                            txtTag9.Text = "";
                            txtTag10.Text = "";
                        }
                        /// Start the Default Workflow according to the Department & Doc Type combination end
                    }
                }
                TagDisplay(ddDocType.SelectedValue);
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// The following function is used to insert a record in the Database's
        /// <doc_name>,<doc_type_id>,<dept_id>,<upld_by>,<upld_dt>,<tag1>,<tag2>,<tag3>,<tag4>,<tag5>,<tag6>,<tag7>,<tag8>,<tag9>,<tag10> fields of <doc_mast> table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddMaster_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                String fileName = "";
                Stream inputStream;
                double FileSize = 0;
                byte[] bytes = null;
                string DocNameToBeUploaded = "";
                string result = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                FileType ObjFileType = new FileType();
                DataSet ds01 = new DataSet();

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                // Check the file is pdf or not because if there is any Append function in the WFL
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectAppendTaskInWFL(ddDept.SelectedValue, ddDocType.SelectedValue, Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    if (ObjFetchOnlyNameORExtension.FetchOnlyDocExt(txtDocName.Text) != "pdf")
                    {
                        throw new Exception("You can upload pdf File only as the defined Workflow has the Append option!!");
                    }
                }

                if (Request.QueryString["CIDocUUID"] != null)
                {
                    if (Request.QueryString["CIDocUUID"].ToString() != "")
                    {
                        if (ObjFetchOnlyNameORExtension.FetchOnlyDocExt(btnBrows.FileName) == ObjFetchOnlyNameORExtension.FetchOnlyDocExt(lblDocName.Text))
                        {
                            UploadRevisedVersion();
                        }
                        else
                        {
                            throw new Exception("Mismatched File Type");
                        }                        
                    }
                }
                else
                {
                    if (Session["NewFile"] != null)
                    {
                        FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
                        if (lblLocation.Text == "")
                        {
                            throw new Exception("The Uploaded Location is Not Set for This Document. Please Contact to Admin.");
                        }
                    }
                    else
                    {
                        if (divUpldCabinet.Visible == true)
                        {
                            if (ddFolder.SelectedValue == "")
                            {

                            }
                            else
                            {
                                hfUUID.Value = ddFolder.SelectedValue;
                            }
                            if (hfUUID.Value == "")
                            {
                                throw new Exception("Please select the Folder");
                            }
                        }
                        else
                        {
                            FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
                            if (lblLocation.Text == "")
                            {
                                throw new Exception("The Uploaded Location is Not Set for This Document. Please Contact to Admin.");
                            }
                        }
                    }
                    
                    /// Insert into the database using Store Procedure <DocMast_Insert> start
                    if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                    {
                        int start = txtDocName.Text.Trim().LastIndexOf(".") + 1;
                        if (start == 0)
                        {
                            DocNameToBeUploaded = txtDocName.Text.Trim() + ".pdf";
                        }
                        else
                        {
                            int length = txtDocName.Text.Trim().Length - start;
                            string fileNameExt = txtDocName.Text.Trim().Substring(start, length);
                            if (fileNameExt == "pdf")
                            {
                                DocNameToBeUploaded = txtDocName.Text.Trim();
                            }
                            else
                            {
                                throw new Exception("Only pdf files will be uploaded");
                            }
                        }
                    }
                    else
                    {
                        DocNameToBeUploaded = txtDocName.Text.Trim();
                    }
                    doc_mast_bal Obj_DocMast = new doc_mast_bal();
                    Obj_DocMast.DocDesc = txtDocDesc.Text.Trim();
                    Obj_DocMast.DocTypeCode = ddDocType.SelectedValue;
                    Obj_DocMast.DeptCode = ddDept.SelectedValue;
                    Obj_DocMast.FolderCode = hfUUID.Value;
                    Obj_DocMast.Upld_By = Session["UserID"].ToString();
                    Obj_DocMast.Upld_Dt = DateTime.Now;
                    Obj_DocMast.Tag1 = txtTag1.Text.Trim();
                    Obj_DocMast.Tag2 = txtTag2.Text.Trim();
                    Obj_DocMast.Tag3 = txtTag3.Text.Trim();
                    Obj_DocMast.Tag4 = txtTag4.Text.Trim();
                    Obj_DocMast.Tag5 = txtTag5.Text.Trim();
                    Obj_DocMast.Tag6 = txtTag6.Text.Trim();
                    Obj_DocMast.Tag7 = txtTag7.Text.Trim();
                    Obj_DocMast.Tag8 = txtTag8.Text.Trim();
                    Obj_DocMast.Tag9 = txtTag9.Text.Trim();
                    Obj_DocMast.Tag10 = txtTag10.Text.Trim();
                    result = ObjClassStoreProc.ExistDoc(DocNameToBeUploaded, hfUUID.Value, Session["CompCode"].ToString());
                    if (Convert.ToInt32(result) == -1)
                    {
                        throw new Exception("Document already exists in this folder!");
                    }
                    else
                    {
                        if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                        {
                            /// Checking the Alias File Start
                            String fileNameExt = "";
                            fileName = txtDocName.Text.Trim();

                            if (fileName == null || fileName.Equals(""))
                            {
                                return;
                            }

                            int start = fileName.LastIndexOf(".") + 1;
                            int length = fileName.Length - start;
                            fileNameExt = fileName.Substring(start, length);
                            if (fileNameExt == "pdf")
                            {

                            }
                            else
                            {
                                if (start == 0)
                                {
                                    fileName = fileName + ".pdf";
                                    fileNameExt = "pdf";
                                    Session["FileNameExt"] = fileNameExt;
                                }
                                else if (start > 1)
                                {
                                    throw new Exception("Only pdf files will be uploaded");
                                }
                                else
                                {
                                    throw new Exception("Invalid document Name");
                                }
                            }
                            /// Checking the Alias File End
                            DataSet dsFileSize01 = new DataSet();
                            cmd = new SqlCommand("select DocSize from doc_mast where uuid='" + Session["TemplateUUID"].ToString() + "'", con);
                            SqlDataAdapter adpFileSize01 = new SqlDataAdapter(cmd);
                            adpFileSize01.Fill(dsFileSize01);
                            if (dsFileSize01.Tables[0].Rows.Count > 0)
                            {
                                FileSize = Convert.ToDouble(dsFileSize01.Tables[0].Rows[0][0].ToString());
                            }
                        }
                        else
                        {
                            //For Folder Sniffer start
                            if (Request.QueryString["Source"] != null)
                            {
                                if (Request.QueryString["Source"].ToString() == "FS")
                                {
                                    fileName = Request.QueryString["DocName"].ToString();
                                }
                            }
                            // For Folder Sniffer End
                            else
                            {
                                /// Checking the Original File Start
                                String file = btnBrows.FileName;

                                if (file == null || file.Equals(""))
                                {
                                    return;
                                }
                                int start1 = file.LastIndexOf(".") + 1;
                                if (start1 == 0)
                                {
                                    throw new Exception("Unrecognized document");
                                }
                                // Checking the Original File End
                                //Checking the Alias File Start
                                fileName = txtDocName.Text.Trim();
                                if (fileName == null || fileName.Equals(""))
                                {
                                    return;
                                }
                                int start = fileName.LastIndexOf(".") + 1;
                                if (start == 0)
                                {
                                    throw new Exception("Enter the correct extension of the file!!");
                                }
                                else
                                {

                                }
                                Session["FileNameExt"] = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(fileName);
                                /// Checking the Alias File End
                            }
                            FileSize = Math.Round(Convert.ToDouble(btnBrows.PostedFile.ContentLength) / 1024, 2);
                        }
                        DocNameToBeUploaded = fileName;

                        if (FileSize > 12288)
                        {
                            throw new Exception("You can upload a Document up to 12 MB in size !!");
                        }

                        #region Validation of space if it is exceeding or not
                        SqlDataAdapter adpAvl01;
                        DataSet dsAvl01 = new DataSet();
                        double AvailableSpace = 0;

                        cmd = new SqlCommand("select TotalSpace,UsedSpace,AvailableSpace from ServerConfig where CompCode='" + Session["CompCode"].ToString() + "'", con);
                        adpAvl01 = new SqlDataAdapter(cmd);
                        dsAvl01.Reset();
                        adpAvl01.Fill(dsAvl01);
                        AvailableSpace=Convert.ToDouble(dsAvl01.Tables[0].Rows[0][2].ToString());
                        if (FileSize > AvailableSpace)
                        {
                            throw new Exception("You do not have enough space to upload this Document. Please contact with Administrator.");
                        }
                        #endregion

                        #region Alfresco Part Start
                        Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                        spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                        spacesStore.address = "SpacesStore";

                        // Create the parent reference, the company home folder
                        Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
                        parentReference.store = spacesStore;
                        parentReference.uuid = this.hfUUID.Value; // Folder's uuid

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

                        // Open the file and convert to byte array                         
                        Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
                        
                        if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                        {
                            bytes = StreamFile(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                            contentFormat.mimetype = "application/pdf";
                        }
                        else
                        {
                            //For Folder Sniffer start
                            if (Request.QueryString["Source"] != null)
                            {
                                if (Request.QueryString["Source"].ToString() == "FS")
                                {
                                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                                    bytes = encoding.GetBytes(Request.QueryString["FileData"].ToString());
                                    contentFormat.mimetype = ObjFileType.GetFileType(Request.QueryString["DocName"].ToString());
                                }
                            }
                            // For Folder Sniffer End
                            else
                            {
                                inputStream = btnBrows.PostedFile.InputStream;
                                int bufferSize = (int)inputStream.Length;
                                bytes = new byte[bufferSize];
                                inputStream.Read(bytes, 0, bufferSize);
                                inputStream.Close();
                                contentFormat.mimetype = ObjFileType.GetFileType(Session["FileNameExt"].ToString());// "application/pdf";
                            }
                        }

                        WebServiceFactory wsF = new WebServiceFactory();
                        wsF.UserName = Session["AdmUserID"].ToString();
                        wsF.Ticket = Session["AdmTicket"].ToString();
                        wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);
                        #endregion

                        result = ObjClassStoreProc.InsertDocMast(DocNameToBeUploaded, txtDocDesc.Text.Trim(), hfUUID.Value, ddDocType.SelectedValue, ddDept.SelectedValue, Session["UserID"].ToString(), DateTime.Now, txtTag1.Text.Trim(), txtTag2.Text.Trim(), txtTag3.Text.Trim(), txtTag4.Text.Trim(), txtTag5.Text.Trim(), txtTag6.Text.Trim(), txtTag7.Text.Trim(), txtTag8.Text.Trim(), txtTag9.Text.Trim(), txtTag10.Text.Trim(), "", newContentNode.uuid, newContentNode.uuid + "/" + fileName.Replace(" ", "%20"), "N", Session["CompCode"].ToString(), FileSize);
                        if (Convert.ToInt32(result) == -1)
                        {
                            throw new Exception("Document already exists in this folder!");
                        }
                        else
                        {
                            #region Maintaining DocLog
                            string MaxID = ObjClassStoreProc.MaxAutoID4DocLog(newContentNode.uuid, Session["CompCode"].ToString());
                            string InsertDocLog = ObjClassStoreProc.InsertDocLog(MaxID, newContentNode.uuid, newContentNode.uuid, Session["UserID"].ToString(), "Document has been uploaded by ", Session["CompCode"].ToString());
                            #endregion
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + FileSize + "' where CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();

                            UserRights RightsObj = new UserRights();
                            ds01.Reset();
                            ds01 = RightsObj.FetchPermission(hfUUID.Value, Session["CompCode"].ToString());
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                {
                                    RightsObj.InsertPermissionSingleData(newContentNode.uuid, "Document", ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                                }
                            }

                            /// Call function for Meta Tags insertion
                            if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                            {
                                InsertMetaTags(newContentNode.uuid, Session["NewFile"].ToString());
                            }
                            else
                            {
                                InsertMetaTags(newContentNode.uuid, fileName);
                            }
                            /// Start the Default Workflow according to the Department & Doc Type combination start
                            /// Store the Last inserted Doc ID into <Session["DocId"]>
                            Session["DocId"] = result;
                            result = ObjClassStoreProc.DefinedWF(ddDocType.SelectedValue, ddDept.SelectedValue, Session["CompCode"].ToString());
                            if (Convert.ToInt32(result) == -111) /// If the Workflow is not defined for the Dept & Doc Type Combination; default workflow will be assigned to admin
                            {
                                Redirect2Home("Document uploaded successfully!", "home.aspx");
                                PopulateDropdown();
                                txtDocName.Text = "";
                                txtDocDesc.Text = "";
                                txtTag1.Text = "";
                                txtTag2.Text = "";
                                txtTag3.Text = "";
                                txtTag4.Text = "";
                                txtTag5.Text = "";
                                txtTag6.Text = "";
                                txtTag7.Text = "";
                                txtTag8.Text = "";
                                txtTag9.Text = "";
                                txtTag10.Text = "";
                            }
                            else /// If the Workflow is defined for the Dept & Doc Type Combination
                            {
                                /// Store the defined Wokflow ID into <Session["WFId"]>
                                Session["WFId"] = result;
                                result = ObjClassStoreProc.StartDefaultWF(ddDocType.SelectedValue, ddDept.SelectedValue, DateTime.Now, Convert.ToInt16(Session["DocId"]), Convert.ToInt16(Session["WFId"]), Session["UserID"].ToString(), Session["CompCode"].ToString());
                                if (result != "") /// Start the default defined workflow
                                {
                                    /// Insert the Workflow based roles and due time into the database table name:<wf_log_dtl>
                                    /// Store the <wf_log_id> into <Session["WFLogId"]>
                                    /// Select the stage wise roles defined in the workflow
                                    Session["WFLogId"] = result;
                                    ds01.Reset();
                                    ds01 = ObjClassStoreProc.SelectWFDtls(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                                    if (ds01.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                        {
                                            result = ObjClassStoreProc.StartDefaultWFLogDtl(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), DateTime.Now, ds01.Tables[0].Rows[i]["duration"].ToString(), DateTime.Now, Session["CompCode"].ToString());
                                            // Insert the Log for versioning of the file Start
                                            if (Request.QueryString["NewFile"] != null)
                                            {
                                                result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), Session["TemplateUUID"].ToString(), "", Session["CompCode"].ToString());
                                                if (Convert.ToInt32(result) > 0)
                                                {

                                                }
                                                result = ObjClassStoreProc.WFDocVersionUpdate(Session["WFLogId"].ToString(), 1, Session["TemplateUUID"].ToString(), Session["TemplateUUID"].ToString(), Session["CompCode"].ToString());
                                                if (Convert.ToInt32(result) > 0)
                                                {

                                                }
                                                cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + Session["TemplateUUID"].ToString() + "' where WFLogID='" + Session["WFLogId"].ToString() + "' and StepNo>'1'", con);
                                                cmd.ExecuteNonQuery();

                                                DataSet dsV01 = new DataSet();
                                                cmd = new SqlCommand("select * from WFDoc where WFLogID='" + Session["WFLogId"].ToString() + "' and DocUUID='" + newContentNode.uuid + "'", con);
                                                SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                                adapterV01.Fill(dsV01);
                                                if (dsV01.Tables[0].Rows.Count > 0)
                                                {

                                                }
                                                else
                                                {
                                                    cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + Session["WFLogId"].ToString() + "','" + newContentNode.uuid + "','" + Session["CompCode"].ToString() + "')", con);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            else
                                            {
                                                result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), newContentNode.uuid, "", Session["CompCode"].ToString());
                                                result = ObjClassStoreProc.WFDocVersionUpdate(Session["WFLogId"].ToString(), 1, newContentNode.uuid, newContentNode.uuid, Session["CompCode"].ToString());
                                                DataSet dsV01 = new DataSet();
                                                cmd = new SqlCommand("select * from WFDoc where WFLogID='" + Session["WFLogId"].ToString() + "' and DocUUID='" + newContentNode.uuid + "'", con);
                                                SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                                adapterV01.Fill(dsV01);
                                                if (dsV01.Tables[0].Rows.Count > 0)
                                                {

                                                }
                                                else
                                                {
                                                    cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + Session["WFLogId"].ToString() + "','" + newContentNode.uuid + "','" + Session["CompCode"].ToString() + "')", con);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            // Insert the Log for versioning of the file End
                                        }
                                    }

                                    /// Insert the Workflow based roles and tasks into the database table name:<wf_log_task>
                                    /// Select the stage wise tasks defined in the workflow
                                    ds01.Reset();
                                    ds01 = ObjClassStoreProc.SelectWFTasks(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                                    if (ds01.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                                        {
                                            result = ObjClassStoreProc.StartDefaultWFLogTask(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), ds01.Tables[0].Rows[i]["task_id"].ToString(), ds01.Tables[0].Rows[i]["amble_mails"].ToString(), ds01.Tables[0].Rows[i]["amble_msg"].ToString(), ds01.Tables[0].Rows[i]["amble_attach"].ToString(), ds01.Tables[0].Rows[i]["AppendDocUUID"].ToString(), ds01.Tables[0].Rows[i]["amble_url"].ToString(), ds01.Tables[0].Rows[i]["AmbleSub"].ToString(), Session["CompCode"].ToString());
                                        }

                                        #region Update the Initiator Start
                                        DataSet dsI01 = new DataSet();
                                        cmd = new SqlCommand("select email from user_mast where user_id='" + Session["UserID"].ToString() + "'", con);
                                        SqlDataAdapter adapterI01 = new SqlDataAdapter(cmd);
                                        adapterI01.Fill(dsI01);
                                        if (dsI01.Tables[0].Rows.Count > 0)
                                        {
                                            if (Session["AccessControl"].ToString() == "Outside")
                                            {
                                                result = ObjClassStoreProc.WFLogTaskUpdate(1,Session["WFLogId"].ToString(), "guest@guest.com", Session["InitiatorEmailID"].ToString(),DateTime.Now,"","",0, Session["CompCode"].ToString());
                                            }
                                            else
                                            {
                                                result = ObjClassStoreProc.WFLogTaskUpdate(1, Session["WFLogId"].ToString(), "init@init.com", dsI01.Tables[0].Rows[0][0].ToString(), DateTime.Now, "", "",0, Session["CompCode"].ToString());
                                            }
                                        }
                                        #endregion
                                        CheckAction(Session["WFLogId"].ToString(), 1, DateTime.Now, "", "");
                                        if (Session["NewFile"] == null)
                                        {

                                        }
                                        else
                                        {
                                            File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                            File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                        }
                                        Session["WFTask"] = null;
                                        // Update the Doc's Stat in TempDocSaving table
                                        if (Session["hfPageControl"] != null)
                                        {
                                            if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                            {
                                                result = ObjClassStoreProc.TempDocSavingUpdate(Session["OpenDocName"].ToString(), Session["UserID"].ToString(), DateTime.Now, "Uploaded", Session["CompCode"].ToString());
                                                if (Convert.ToInt32(result) > 0)
                                                {

                                                }
                                            }
                                        }
                                        Session["hfPageControl"] = null;
                                        Session["AccessControl"] = null;
                                        Session["NewFile"] = null;
                                        Session["TemplateUUID"] = null;
                                        RedirectMessageBox("Document uploaded successfully & the default workflow with tasks has been assigned!", "userhome.aspx");
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
                                            if (Session["NewFile"] != null && Session["OpenDocName"] != null)
                                            {
                                                if (File.Exists(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString()))
                                                {
                                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                                }
                                                if (File.Exists(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString()))
                                                {
                                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                                }
                                            }
                                        }
                                        // Update the Doc's Stat in TempDocSaving table
                                        if (Session["hfPageControl"] != null)
                                        {
                                            if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                            {
                                                result = ObjClassStoreProc.TempDocSavingUpdate(Session["OpenDocName"].ToString(), Session["UserID"].ToString(), DateTime.Now, "Uploaded", Session["CompCode"].ToString());
                                                if (Convert.ToInt32(result) > 0)
                                                {

                                                }
                                            }
                                        }
                                        RedirectMessageBox("Document uploaded successfully!", "userhome.aspx");
                                        PopulateDropdown();
                                        txtDocName.Text = "";
                                        txtDocDesc.Text = "";
                                        txtTag1.Text = "";
                                        txtTag2.Text = "";
                                        txtTag3.Text = "";
                                        txtTag4.Text = "";
                                        txtTag5.Text = "";
                                        txtTag6.Text = "";
                                        txtTag7.Text = "";
                                        txtTag8.Text = "";
                                        txtTag9.Text = "";
                                        txtTag10.Text = "";
                                    }
                                }
                                else
                                {
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["NewFile"].ToString());
                                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                                    // Update the Doc's Stat in TempDocSaving table
                                    if (Session["hfPageControl"].ToString() == "FE") //For Fresh WF but exist doc
                                    {
                                        result = ObjClassStoreProc.TempDocSavingUpdate(Session["OpenDocName"].ToString(), Session["UserID"].ToString(), DateTime.Now, "Uploaded", Session["CompCode"].ToString());
                                        if (Convert.ToInt32(result) > 0)
                                        {

                                        }
                                    }
                                    MessageBox("Document uploaded successfully but error in assigning the defined workflow!");
                                    PopulateDropdown();
                                    txtDocName.Text = "";
                                    txtDocDesc.Text = "";
                                    txtTag1.Text = "";
                                    txtTag2.Text = "";
                                    txtTag3.Text = "";
                                    txtTag4.Text = "";
                                    txtTag5.Text = "";
                                    txtTag6.Text = "";
                                    txtTag7.Text = "";
                                    txtTag8.Text = "";
                                    txtTag9.Text = "";
                                    txtTag10.Text = "";
                                }
                                /// Start the Default Workflow according to the Department & Doc Type combination end
                            }
                        }
                        TagDisplay(ddDocType.SelectedValue);
                        Session["AccessControl"] = "";
                        Session["InitiatorEmailID"] = "";
                    }
                    Utility.CloseConnection(con);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.Flush();
        }

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
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                DataSet ds003 = new DataSet();
                DataSet dsT001 = new DataSet();

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds002 = new DataSet();
                DataSet ds004 = new DataSet();
                DataSet ds005 = new DataSet();
                DataSet ds006 = new DataSet();
                con.Open();
                if (Session["AccessControl"].ToString() != "Outside")
                {
                    Session["InitiatorEmailID"] = "";
                }

                ds01.Reset();
                ds01 = ObjClassStoreProc.WFLogTaskSelect(1, WFLogID, Convert.ToInt16(StepNo),Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        #region For Preamble Email Start
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PREEMAIL")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo),"", Session["CompCode"].ToString());
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
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PRECOPY")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo),"", Session["CompCode"].ToString());
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
                        #region For Preamble Conditional Email Start
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PRECOND")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo),"", Session["CompCode"].ToString());
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
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PREAPPEND")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo),"", Session["CompCode"].ToString());
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
                        ds003 = new DataSet();
                        ds003.Reset();
                        ds003 = ObjClassStoreProc.WFLogTaskSelect(2, WFLogID, Convert.ToInt16(StepNo), Session["CompCode"].ToString());
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
                                    dsT001.Reset();
                                    dsT001 = ObjClassStoreProc.WFLogTaskSelect(3, WFLogID, StepNo, Session["CompCode"].ToString());
                                    if (dsT001.Tables[0].Rows.Count > 0)
                                    {
                                        result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, Comments, TaskID, StepNo, Session["CompCode"].ToString());
                                        result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Not Required", "REJECT", StepNo, Session["CompCode"].ToString());
                                        //Log Update
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, TaskDoneDate.ToString(), Comments, TaskID, StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, "Not Required", "Not Required", "REJECT", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                    }
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    return;
                                    #endregion
                                }
                                // For REJECT Task
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
                                        result = ObjClassStoreProc.WFLogTaskUpdate(3, WFLogID, "", "", DateTime.Now, null, "", StepNo, Session["CompCode"].ToString());
                                        cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Not Started' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();  
                                        //Log Update
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, TaskDoneDate.ToString(), Comments, "REJECT", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        result = ObjClassStoreProc.WFLogUpdate(3, WFLogID, "Not Required", "Not Required", "", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        
                                        // Pervious Stage
                                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        DataSet ds114 = new DataSet();
                                        SqlDataAdapter adapter114 = new SqlDataAdapter(cmd);
                                        ds114.Reset();
                                        adapter114.Fill(ds114);
                                        Session["WFTask"] = "REJECT";
                                        if (ds114.Tables[0].Rows.Count > 0)
                                        {
                                            cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            //Log Update
                                            int MaxStep = 0;
                                            int StartStep = 0;
                                            DataSet dsMaxStep = new DataSet();
                                            cmd = new SqlCommand("select max(StepNo) from WFLog where WFLogID='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            SqlDataAdapter adapterMaxStep = new SqlDataAdapter(cmd);
                                            dsMaxStep.Reset();
                                            adapterMaxStep.Fill(dsMaxStep);
                                            MaxStep = Convert.ToInt32(dsMaxStep.Tables[0].Rows[0][0].ToString());
                                            StartStep = Convert.ToInt32(StepNo) - 1;

                                            for (int kk = StartStep; kk <= MaxStep; kk++)
                                            {
                                                DataSet dsPrev = new DataSet();
                                                cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + kk + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
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
                                                    cmd = new SqlCommand("delete from WFLog where WFLogID='" + WFLogID + "' and StepNo='" + kk + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                                    cmd.ExecuteNonQuery();
                                                    for (int jj = 0; jj < dsPrev.Tables[0].Rows.Count; jj++)
                                                    {
                                                        cmd = new SqlCommand("insert into WFLog(WFLogID,StepNo,UserID,TaskID,TaskDoneDate,Comments,CompCode) values('" + WFLogID + "', '" + kk + "','','" + dsPrev.Tables[0].Rows[jj][2].ToString() + "','Waiting','Waiting','" + Session["CompCode"].ToString() + "')", con);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            CheckAction(WFLogID, StepNo - 1, DateTime.Now, "", "");
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
                                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and task_done_dt is null and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    DataSet dsT002 = new DataSet();
                                    SqlDataAdapter adapterT002 = new SqlDataAdapter(cmd);
                                    dsT002.Reset();
                                    adapterT002.Fill(dsT002);
                                    if (dsT002.Tables[0].Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Not Required' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + TaskID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT' and CompCode='" + Session["CompCode"].ToString() + "'", con);
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
                            cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null and (task_id='PRECOPY' or task_id='PREEMAIL' or task_id='PRECOND' or task_id='PREAPPEND')", con);
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
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTEMAIL")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleEmail ObjPostambleEmail = new PostambleEmail();
                                    ObjPostambleEmail.SendPostMail(WFLogID, StepNo, "POSTEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Copy Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTCOPY")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Copy Action
                                    PostambleCopy ObjPostambleCopy = new PostambleCopy();
                                    string PostCPOutput = ObjPostambleCopy.PostCopy(WFLogID, StepNo, "POSTCOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "POSTCOPY", PostCPOutput);
                                }
                                #endregion
                                #region For Postamble Conditional Mail Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTCOND")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleConditionalEmail ObjPostambleConditionalEmail = new PostambleConditionalEmail();
                                    ObjPostambleConditionalEmail.SendPostCondMail(WFLogID, StepNo, "POSTCOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Append Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTAPPEND")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
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
                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null", con);
                    ds005 = new DataSet();
                    SqlDataAdapter adapter005 = new SqlDataAdapter(cmd);
                    adapter005.Fill(ds005);
                    if (ds005.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                        // Call this method again
                        DataSet ds100 = new DataSet();
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null  and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND' and task_id!='PREAPPEND' and task_id!='POSTAPPEND')", con);
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
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Completed' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null", con);
                        ds006 = new DataSet();
                        SqlDataAdapter adapter006 = new SqlDataAdapter(cmd);
                        adapter006.Fill(ds006);
                        if (ds006.Tables[0].Rows.Count > 0)
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Ongoing' where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Completed',actual_completed_dt='" + TaskDoneDate + "' where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // Update Status end

                    // Check the stage is completed or not
                    cmd = new SqlCommand("select wf_stat from wf_log_dtl where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
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
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                //MessageBox(ex.Message);
            }
        }

        protected void ddDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                DocTypeSettings();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void DocTypeSettings()
        {
            try
            {
                FetchWFL(ddDept.SelectedValue, ddDocType.SelectedValue);
                if (ddDocType.SelectedValue == "GENERAL")
                {
                    divUpldLoc.Visible = false;
                    divUpldCabinet.Visible = true;
                    divUpldDrawer.Visible = true;
                    divUpldFolder.Visible = true;
                    PopulateCabinet();
                }
                else
                {
                    divUpldLoc.Visible = true;
                    divUpldCabinet.Visible = false;
                    divUpldDrawer.Visible = false;
                    divUpldFolder.Visible = false;
                    FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
                }

                TagDisplay(ddDocType.SelectedValue);
                if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                {
                    divBrowse.Visible = false;
                    SetDocDetails(Session["NewFile"].ToString(), Session["TemplateUUID"].ToString());
                }
                else
                {
                    //For Folder Sniffer start
                    if (Request.QueryString["Source"] != null)
                    {
                        if (Request.QueryString["Source"].ToString() == "FS")
                        {
                            divBrowse.Visible = false;
                        }
                    }
                    // For Folder Sniffer End
                    else
                    {
                        divBrowse.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                DeptSettings();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void DeptSettings()
        {
            try
            {
                FetchWFL(ddDept.SelectedValue, ddDocType.SelectedValue);
                if (ddDocType.SelectedValue == "GENERAL")
                {
                    divUpldLoc.Visible = false;
                    divUpldCabinet.Visible = true;
                    divUpldDrawer.Visible = true;
                    divUpldFolder.Visible = true;
                    PopulateCabinet();
                }
                else
                {
                    divUpldLoc.Visible = true;
                    divUpldCabinet.Visible = false;
                    divUpldDrawer.Visible = false;
                    divUpldFolder.Visible = false;
                    FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
                }

                TagDisplay(ddDocType.SelectedValue);
                if (Session["NewFile"] != null && Session["TemplateUUID"] != null)
                {
                    divBrowse.Visible = false;
                    SetDocDetails(Session["NewFile"].ToString(), Session["TemplateUUID"].ToString());
                }
                else
                {
                    //For Folder Sniffer start
                    if (Request.QueryString["Source"] != null)
                    {
                        if (Request.QueryString["Source"].ToString() == "FS")
                        {
                            divBrowse.Visible = false;
                        }
                    }
                    // For Folder Sniffer End
                    else
                    {
                        divBrowse.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Function by which the selected doc type's corresponding tags will be displayed
        /// </summary>
        /// <param name="SelItem"></param>
        protected void TagDisplay(string SelItem)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectTagBasedOnDocTypeCompCode(SelItem,Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    /// For Tag1
                    if (ds01.Tables[0].Rows[0]["tag1"].ToString() == "" || ds01.Tables[0].Rows[0]["tag1"] == null)
                    {
                        txtTag1.Text = "";
                        divTag1.Visible = false;
                    }
                    else
                    {
                        divTag1.Visible = true;
                        lblTag1.Text = ds01.Tables[0].Rows[0]["tag1"].ToString();
                    }
                    /// For Tag2
                    if (ds01.Tables[0].Rows[0]["tag2"].ToString() == "" || ds01.Tables[0].Rows[0]["tag2"] == null)
                    {
                        txtTag2.Text = "";
                        divTag2.Visible = false;
                    }
                    else
                    {
                        divTag2.Visible = true;
                        lblTag2.Text = ds01.Tables[0].Rows[0]["tag2"].ToString();
                    }
                    /// For Tag3
                    if (ds01.Tables[0].Rows[0]["tag3"].ToString() == "" || ds01.Tables[0].Rows[0]["tag3"] == null)
                    {
                        txtTag3.Text = "";
                        divTag3.Visible = false;
                    }
                    else
                    {
                        divTag3.Visible = true;
                        lblTag3.Text = ds01.Tables[0].Rows[0]["tag3"].ToString();
                    }
                    /// For Tag4
                    if (ds01.Tables[0].Rows[0]["tag4"].ToString() == "" || ds01.Tables[0].Rows[0]["tag4"] == null)
                    {
                        txtTag4.Text = "";
                        divTag4.Visible = false;
                    }
                    else
                    {
                        divTag4.Visible = true;
                        lblTag4.Text = ds01.Tables[0].Rows[0]["tag4"].ToString();
                    }
                    /// For Tag5
                    if (ds01.Tables[0].Rows[0]["tag5"].ToString() == "" || ds01.Tables[0].Rows[0]["tag5"] == null)
                    {
                        txtTag5.Text = "";
                        divTag5.Visible = false;
                    }
                    else
                    {
                        divTag5.Visible = true;
                        lblTag5.Text = ds01.Tables[0].Rows[0]["tag5"].ToString();
                    }
                    /// For Tag6
                    if (ds01.Tables[0].Rows[0]["tag6"].ToString() == "" || ds01.Tables[0].Rows[0]["tag6"] == null)
                    {
                        txtTag6.Text = "";
                        divTag6.Visible = false;
                    }
                    else
                    {
                        divTag6.Visible = true;
                        lblTag6.Text = ds01.Tables[0].Rows[0]["tag6"].ToString();
                    }
                    /// For Tag7
                    if (ds01.Tables[0].Rows[0]["tag7"].ToString() == "" || ds01.Tables[0].Rows[0]["tag7"] == null)
                    {
                        txtTag7.Text = "";
                        divTag7.Visible = false;
                    }
                    else
                    {
                        divTag7.Visible = true;
                        lblTag7.Text = ds01.Tables[0].Rows[0]["tag7"].ToString();
                    }
                    /// For Tag8
                    if (ds01.Tables[0].Rows[0]["tag8"].ToString() == "" || ds01.Tables[0].Rows[0]["tag8"] == null)
                    {
                        txtTag8.Text = "";
                        divTag8.Visible = false;
                    }
                    else
                    {
                        divTag8.Visible = true;
                        lblTag8.Text = ds01.Tables[0].Rows[0]["tag8"].ToString();
                    }
                    /// For Tag9
                    if (ds01.Tables[0].Rows[0]["tag9"].ToString() == "" || ds01.Tables[0].Rows[0]["tag9"] == null)
                    {
                        txtTag9.Text = "";
                        divTag9.Visible = false;
                    }
                    else
                    {
                        divTag9.Visible = true;
                        lblTag9.Text = ds01.Tables[0].Rows[0]["tag9"].ToString();
                    }
                    /// For Tag10
                    if (ds01.Tables[0].Rows[0]["tag10"].ToString() == "" || ds01.Tables[0].Rows[0]["tag10"] == null)
                    {
                        txtTag10.Text = "";
                        divTag10.Visible = false;
                    }
                    else
                    {
                        divTag10.Visible = true;
                        lblTag10.Text = ds01.Tables[0].Rows[0]["tag10"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdFillMetaData_Click(object sender, EventArgs e)
        {
            try
            {
                string LicenseKey = "";
                string ServerIPAddress = "";
                // Fetch ServerConfig Details Start
                DBClass DBObj = new DBClass();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    LicenseKey = ds01.Tables[0].Rows[0][0].ToString();
                    ServerIPAddress = ds01.Tables[0].Rows[0][1].ToString();
                }
                // Fetch ServerConfig Details End
                if ((btnBrows.PostedFile != null) && (btnBrows.PostedFile.ContentLength > 0))
                {
                    string fn = System.IO.Path.GetFileName(btnBrows.PostedFile.FileName);
                    string postfix = System.IO.Path.GetExtension(fn).ToString().ToLower();
                    if (postfix == ".pdf")
                    {
                        // Upload the file in the server (tmpUpload folder) start
                        string SaveLocation = ConfigurationManager.AppSettings["UpldFromFolderKey"] + "\\";
                        btnBrows.PostedFile.SaveAs(SaveLocation + btnBrows.FileName);
                        // Upload the file in the server (tmpUpload folder) end

                        string Full_Path = SaveLocation + btnBrows.FileName;
                        int Result = QP.UnlockKey(LicenseKey);
                        if (Result == 1)
                        {
                            QP.LoadFromFile(Full_Path, "");
                            int TotalFormFields = QP.FormFieldCount();
                            if (TotalFormFields > 0) // For Editable Form
                            {
                                SqlConnection con = Utility.GetConnection();
                                SqlCommand cmd = null;
                                con.Open();
                                DataSet ds0001 = new DataSet();
                                DataSet ds0003 = new DataSet();
                                cmd = new SqlCommand("select * from doc_type_mast where doc_type_id='" + ddDocType.SelectedValue + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                SqlDataAdapter adapter0003 = new SqlDataAdapter(cmd);
                                adapter0003.Fill(ds0003);
                                if (ds0003.Tables[0].Rows.Count > 0)
                                {
                                    // Here 12 to 21 are the field nos in the database
                                    // For Tag1
                                    if (ds0003.Tables[0].Rows[0][12].ToString() != "0")
                                    {
                                        txtTag1.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][12].ToString()));
                                    }
                                    else
                                    {
                                        txtTag1.Text = "";
                                    }

                                    // For Tag2
                                    if (ds0003.Tables[0].Rows[0][13].ToString() != "0")
                                    {
                                        txtTag2.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][13].ToString()));
                                    }
                                    else
                                    {
                                        txtTag2.Text = "";
                                    }

                                    // For Tag3
                                    if (ds0003.Tables[0].Rows[0][14].ToString() != "0")
                                    {
                                        txtTag3.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][14].ToString()));
                                    }
                                    else
                                    {
                                        txtTag3.Text = "";
                                    }

                                    // For Tag4
                                    if (ds0003.Tables[0].Rows[0][15].ToString() != "0")
                                    {
                                        txtTag4.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][15].ToString()));
                                    }
                                    else
                                    {
                                        txtTag4.Text = "";
                                    }

                                    // For Tag5
                                    if (ds0003.Tables[0].Rows[0][16].ToString() != "0")
                                    {
                                        txtTag5.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][16].ToString()));
                                    }
                                    else
                                    {
                                        txtTag5.Text = "";
                                    }

                                    // For Tag6
                                    if (ds0003.Tables[0].Rows[0][17].ToString() != "0")
                                    {
                                        txtTag6.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][17].ToString()));
                                    }
                                    else
                                    {
                                        txtTag6.Text = "";
                                    }

                                    // For Tag7
                                    if (ds0003.Tables[0].Rows[0][18].ToString() != "0")
                                    {
                                        txtTag7.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][18].ToString()));
                                    }
                                    else
                                    {
                                        txtTag7.Text = "";
                                    }

                                    // For Tag8
                                    if (ds0003.Tables[0].Rows[0][19].ToString() != "0")
                                    {
                                        txtTag8.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][19].ToString()));
                                    }
                                    else
                                    {
                                        txtTag8.Text = "";
                                    }

                                    // For Tag9
                                    if (ds0003.Tables[0].Rows[0][20].ToString() != "0")
                                    {
                                        txtTag9.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][20].ToString()));
                                    }
                                    else
                                    {
                                        txtTag9.Text = "";
                                    }

                                    // For Tag10
                                    if (ds0003.Tables[0].Rows[0][21].ToString() != "0")
                                    {
                                        txtTag10.Text = QP.GetFormFieldValue(Convert.ToInt32(ds0003.Tables[0].Rows[0][21].ToString()));
                                    }
                                    else
                                    {
                                        txtTag10.Text = "";
                                    }

                                }
                                Utility.CloseConnection(con);
                            }
                        }
                        else
                        {
                            MessageBox("- Invalid license key -");
                        }
                    }
                    else
                    {
                        throw new Exception("Please select pdf file only !");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void InsertMetaTags(string DocUUID, string FileName)
        {
            try
            {
                string result;
                string LicenseKey = "";
                string ServerIPAddress = "";
                // Fetch ServerConfig Details Start
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    LicenseKey = ds01.Tables[0].Rows[0][0].ToString();
                    ServerIPAddress = ds01.Tables[0].Rows[0][1].ToString();
                }
                // Fetch ServerConfig Details End

                string SaveLocation = Server.MapPath("TempDownload") + "\\";
                string Full_Path = SaveLocation + FileName;
                int Result = QP.UnlockKey(LicenseKey);
                if (Result == 1)
                {
                    QP.LoadFromFile(Full_Path, "");
                    int TotalFormFields = QP.FormFieldCount();
                    if (TotalFormFields > 0) // For Editable Form
                    {
                        SqlConnection con = Utility.GetConnection();
                        SqlCommand cmd = null;
                        con.Open();
                        string DBFldName = "";
                        string FrmValue = "";

                        /// Restrict to max 300 fields
                        if (TotalFormFields > 300)
                        {
                            TotalFormFields = 300;
                        }
                        result = ObjClassStoreProc.DocMetaValueInsert(DocUUID,"",Session["CompCode"].ToString());
                        for (int k = 1; k <= TotalFormFields; k++)
                        {
                            DBFldName = "Tag" + k;
                            FrmValue = QP.GetFormFieldValue(k);
                            cmd = new SqlCommand("update DocMetaValue set " + DBFldName + "='" + FrmValue + "' where uuid='" + DocUUID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        Utility.CloseConnection(con);
                    }
                }
                else
                {
                    throw new Exception("- Invalid Quick PDF license key -");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void SetUploadedLocation(string FldUUID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.SetUploadedLocation(FldUUID,Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    hfUUID.Value = ds01.Tables[0].Rows[0][0].ToString();
                    lblLocation.Text = ds01.Tables[0].Rows[0][3].ToString() + " >> " + ds01.Tables[0].Rows[0][2].ToString() + " >> " + ds01.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    //lblLocation.Text = "";
                }
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
                if (ddDocType.SelectedValue == "GENERAL")
                {
                    divUpldLoc.Visible = false;
                    divUpldCabinet.Visible = true;
                    divUpldDrawer.Visible = true;
                    divUpldFolder.Visible = true;
                    PopulateCabinet();
                }
                else
                {
                    divUpldLoc.Visible = true;
                    divUpldCabinet.Visible = false;
                    divUpldDrawer.Visible = false;
                    divUpldFolder.Visible = false;

                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01 = ObjClassStoreProc.FetchUploadedLocation(DeptID, DocTypeID, Session["CompCode"].ToString());
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        hfUUID.Value = ds01.Tables[0].Rows[0][0].ToString();
                        lblLocation.Text = ds01.Tables[0].Rows[0][3].ToString() + " >> " + ds01.Tables[0].Rows[0][2].ToString() + " >> " + ds01.Tables[0].Rows[0][1].ToString();
                    }
                    else
                    {
                        lblLocation.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void cmdViewWFL_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                string lbWfid1 = hfWFLID.Value;
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.DisplayWFDtl(lbWfid1, Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    MsgNodet.Text = "";
                    Lblwf.Text = "Workflow Name : ";
                    Lbldept.Text = "Department Name : ";
                    Lbldoc.Text = "Document Type : ";
                    LblFld.Text = "Folder Name : ";
                    lblWfname.Text = ds01.Tables[0].Rows[0]["wf_name"].ToString();
                    lblWfdept.Text = ds01.Tables[0].Rows[0]["dept_name"].ToString();
                    lblWfDoctype.Text = ds01.Tables[0].Rows[0]["doc_type_name"].ToString();
                    lblWfFld.Text = ds01.Tables[0].Rows[0]["fld_name"].ToString();

                    DataTable dt1 = new DataTable();

                    dt1.Columns.Add("Stage", typeof(int));
                    dt1.Columns.Add("Role", typeof(string));

                    var x = (from r in ds01.Tables[0].AsEnumerable()
                             select r["step_no"]).Distinct().ToList();
                    bool flag = true;
                    for (int i = 0; i < x.Count; i++)
                    {
                        DataRow[] r1 = ds01.Tables[0].Select("step_no=" + x[i]);

                        foreach (DataRow dr in r1)
                        {
                            DataRow r = dt1.NewRow();
                            if (flag)
                            {
                                r["Stage"] = dr["step_no"];
                                r["Role"] = dr["role_name"];
                                flag = false;
                                dt1.Rows.Add(r);
                            }
                            else
                            {
                                
                            }
                        }
                        flag = true;
                    }
                    gv.DataSource = dt1;
                    gv.DataBind();
                    dt1.Clear();
                }
                else
                {
                    Lblwf.Text = "";
                    Lbldept.Text = "";
                    Lbldoc.Text = "";
                    LblFld.Text = "";
                    lblWfname.Text = "";
                    lblWfdept.Text = "";
                    lblWfDoctype.Text = "";
                    lblWfFld.Text = "";
                    gv.DataSource = null;
                    gv.DataBind();
                    MsgNodet.Text = "No Details Found for this Workflow!";
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void FetchWFL(string DeptID, string DocTypeID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.FetchUploadedLocation(DeptID, DocTypeID, Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    hfWFLID.Value = ds01.Tables[0].Rows[0][4].ToString();
                    lblWFL.Text = ds01.Tables[0].Rows[0][5].ToString();
                }
                else
                {
                    lblWFL.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected string GenNewDocName(string DocName)
        {
            try
            {
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet dsDocExist = new DataSet();
                SqlDataAdapter adapterDocExist = new SqlDataAdapter(cmd);
                string WithoutVerDocName = "";
                string NewDocName = "";
                string DocVersion = "";
                string NewDocVersion = "";
                string OnlyName = ObjFetchOnlyNameORExtension.FetchOnlyDocName(DocName);
                string OnlyExt = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(DocName);
                if (OnlyName.LastIndexOf("_V0") != -1)
                {
                    WithoutVerDocName = OnlyName.Substring(0, OnlyName.Length - 4);
                    DocVersion = OnlyName.Substring(WithoutVerDocName.Length + 2, 2);
                    NewDocVersion = (Convert.ToInt32(DocVersion) + 1).ToString().PadLeft(2, '0');
                    NewDocName = WithoutVerDocName + "_V" + NewDocVersion + "." + OnlyExt;
                }
                else
                {
                    NewDocName = OnlyName + "_V01." + OnlyExt;
                }
                return NewDocName;
            }
            catch (Exception ex)
            {
                return "Error V";
            }
        }

        private string DwnldFile(string DocUUID)
        {
            string DocExtension = "";
            FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();

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
            // Get the extension
            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
            DataSet ds01 = new DataSet();
            ds01.Reset();
            ds01 = ObjClassStoreProc.DocDetails(DocUUID, Session["CompCode"].ToString());
            if (ds01.Tables[0].Rows.Count > 0)
            {
                DocExtension = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(ds01.Tables[0].Rows[0][1].ToString());
            }
            string newSaveFileName = GenNewDocName(ds01.Tables[0].Rows[0][1].ToString());
            string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
            SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
            ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
            return newSaveFileName;
            //Download end                
        }
    }
}