using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using System.IO;
using DMS.UTILITY;
using System.Net;
using System.Configuration;

namespace DMS
{
    public partial class BlankTempUpload : System.Web.UI.Page
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
                    cmdAddMaster.Attributes.Add("OnClick", "javascript: return FormValidation();");
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopCompany();
                            PopulateDropdown();
                            divCompany.Visible = true;
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopCompany();
                            PopulateDropdown();
                            divCompany.Visible = false;
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            Response.Redirect("logout.aspx", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
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
        /// To populate the dropdowns
        /// </summary>
        protected void PopulateDropdown()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //....Doc Type
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectDocTypeCompBased(ddCompany.SelectedValue);
                }
                else
                {
                    ds01 = ObjClassStoreProc.SelectDocTypeCompBased(Session["CompCode"].ToString());
                }
                ddDocType.DataSource = ds01;
                ddDocType.DataTextField = "doc_type_name";
                ddDocType.DataValueField = "doc_type_id";
                ddDocType.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopCompany()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //....Company
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCompActiveAll();
                ddCompany.DataSource = ds01;
                ddCompany.DataTextField = "CompName";
                ddCompany.DataValueField = "CompCode";
                ddCompany.DataBind();

                PopCabinetDropdown();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopCabinetDropdown()
        {
            try
            {
                //....Cabinet
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(ddCompany.SelectedValue, Session["UserID"].ToString());
                }
                else
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                }
                ddCabinet1.DataSource = ds01;
                ddCabinet1.DataTextField = "cab_name";
                ddCabinet1.DataValueField = "cab_uuid";
                ddCabinet1.DataBind();
                PopulateDrawer1(ddCabinet1.SelectedValue);
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
        protected void PopulateDrawer1(string SelCabID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCabID, Session["UserID"].ToString());
                ddDrawer1.DataSource = ds01;
                ddDrawer1.DataTextField = "drw_name";
                ddDrawer1.DataValueField = "drw_uuid";
                ddDrawer1.DataBind();
                PopulateFolder1(ddDrawer1.SelectedValue);
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
        protected void PopulateFolder1(string SelDrawerUUID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrawerUUID, Session["UserID"].ToString());
                ddFolder1.DataSource = ds01;
                ddFolder1.DataTextField = "fld_name";
                ddFolder1.DataValueField = "fld_uuid";
                ddFolder1.DataBind();              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ddCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            PopulateDropdown();
            PopCabinetDropdown();
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
                if (ddCabinet1.SelectedValue == "")
                {
                    throw new Exception("Please select a Cabinet !!");
                }
                else if (ddDrawer1.SelectedValue == "")
                {
                    throw new Exception("Please select a Drawer !!");
                }
                else if (ddFolder1.SelectedValue == "")
                {
                    throw new Exception("Please select a Folder !!");
                }
                hfUUID.Value = ddFolder1.SelectedValue;
                string result = "";
                double FileSize = 0;
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    result = ObjClassStoreProc.ExistDoc(txtDocName.Text.Trim(), hfUUID.Value, ddCompany.SelectedValue);
                }
                else
                {
                    result = ObjClassStoreProc.ExistDoc(txtDocName.Text.Trim(), hfUUID.Value, Session["CompCode"].ToString());
                }


                if (Convert.ToInt32(result) == -1)
                {
                    throw new Exception("Document already exists in this folder!");
                }
                else
                {
                    /// Alfresco Part Start
                    if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "Edit")
                    {

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
                        string fileExt = file.Substring(start1, length1);
                        if (start1 == 0)
                        {
                            throw new Exception("Unrecognized document");
                        }
                        else if (fileExt == "pdf")
                        {

                        }
                        else
                        {
                            throw new Exception("Only pdf files can be uploaded");
                        }
                        /// Checking the Original File End
                    }
                    /// Checking the Alias File Start
                    String fileNameExt = "";
                    String fileName = txtDocName.Text.Trim();

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
                    FileSize = Math.Round(Convert.ToDouble(btnBrows.PostedFile.ContentLength) / 1024, 2);

                    if (FileSize > 12288)
                    {
                        throw new Exception("You can upload a Document up to 12 MB in size !!");
                    }

                    #region Validation of space if it is exceeding or not
                    //SqlConnection con = Utility.GetConnection();
                    //SqlCommand cmd = null;
                    //con.Open();
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

                    //double fileSize = Math.Round(Convert.ToDouble(btnBrows.PostedFile.ContentLength) / 1024, 2);
                    //if (fileSize >= 1024)
                    //{
                    //    fileSize = Math.Round(fileSize / 1024, 2);
                    //    FileSize = fileSize.ToString() + " MB";
                    //}
                    //else
                    //{
                    //    FileSize = fileSize.ToString() + " KB";
                    //}
                    /// Checking the Alias File End
                    // Display a wait cursor while the file is uploaded
                    //Cursor.Current = Cursors.WaitCursor;

                    // Initialise the reference to the spaces store
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
                    inputStream = btnBrows.PostedFile.InputStream;
                    int bufferSize = (int)inputStream.Length;
                    bytes = new byte[bufferSize];
                    inputStream.Read(bytes, 0, bufferSize);
                    inputStream.Close();

                    Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
                    contentFormat.mimetype = "application/pdf";

                    WebServiceFactory wsF = new WebServiceFactory();
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);

                    /// Alfresco Part End

                    /// Insert into doc_mast start
                    if (Session["UserType"].ToString() == "S") // Super Admin
                    {
                        result = ObjClassStoreProc.InsertDocMast(txtDocName.Text.Trim(), txtDocDesc.Text.Trim(), hfUUID.Value, ddDocType.SelectedValue, "NA", Session["UserID"].ToString(), DateTime.Now, "", "", "", "", "", "", "", "", "", "", "", newContentNode.uuid, newContentNode.uuid + "/" + fileName.Replace(" ", "%20"), "T", ddCompany.SelectedValue, FileSize);
                    }
                    else
                    {
                        result = ObjClassStoreProc.InsertDocMast(txtDocName.Text.Trim(), txtDocDesc.Text.Trim(), hfUUID.Value, ddDocType.SelectedValue, "NA", Session["UserID"].ToString(), DateTime.Now, "", "", "", "", "", "", "", "", "", "", "", newContentNode.uuid, newContentNode.uuid + "/" + fileName.Replace(" ", "%20"), "T", Session["CompCode"].ToString(), FileSize);
                    }
                    if (Convert.ToInt32(result) == -1)
                    {
                        throw new Exception("Document already exists in this folder!");
                    }
                    else
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + FileSize + "' where CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                        //cmd = new SqlCommand("update doc_mast set DocSize='" + FileSize + "' where uuid='" + newContentNode.uuid + "'", con);
                        //cmd.ExecuteNonQuery();
                        //cmd = new SqlCommand("update DailyUserDocStat set NoOfNewDocs=NoOfNewDocs+1 where ProcessDate='" + Convert.ToDateTime(DateTime.Now.ToShortDateString()) + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                        //cmd.ExecuteNonQuery();
                        con.Close();
                        /// Set rights for the uploaded doc
                        UserRights RightsObj = new UserRights();
                        //result = RightsObj.FetchPermission(ddCabinet1.SelectedValue);
                        //RightsObj.SetPermissions(newContentNode.uuid, "Document", Session["UserID"].ToString(), result);
                        DataSet dsPerm = new DataSet();
                        //UserRights RightsObj = new UserRights();
                        dsPerm.Reset();
                        dsPerm = RightsObj.FetchPermission(hfUUID.Value, Session["CompCode"].ToString());
                        if (dsPerm.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                            {
                                RightsObj.InsertPermissionSingleData(newContentNode.uuid, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                            }
                        }
                        txtDocName.Text = "";
                        txtDocDesc.Text = "";
                        PopulateDropdown();
                        PopCabinetDropdown();
                        throw new Exception("Document uploaded successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddCabinet1_SelectedIndexChanged(object sender, EventArgs e)
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
                PopulateDrawer1(ddCabinet1.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawer1_SelectedIndexChanged(object sender, EventArgs e)
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
                PopulateFolder1(ddDrawer1.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
    }
}