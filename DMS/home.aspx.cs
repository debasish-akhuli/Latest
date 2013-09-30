using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
#region Using the Namespaces of Alfresco and the Webservices
/// Alfresco is an another project where the Webservices are defined. We just need to use those defined Webservice References and
/// then we can get the methods for those Webservices.
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using Alfresco.DictionaryServiceWebService;
using Alfresco.AdministrationWebService;
using Alfresco.AuthoringWebService;
using Alfresco.ActionWebService;
using Alfresco.ClassificationWebService;
using Alfresco.AuthenticationWebService;
using Alfresco.AccessControlWebService;
using DMS.UTILITY;
using DMS.BAL;
#endregion

namespace DMS
{
    /// At first, The Cabinet section will be displayed when the page will be opened.
    /// Then according to the clicked Cabinet, corresponding Drawers will be displayed and according to the clicked Drawer, corresponding
    /// Folders will be displayed and according to the clicked Folder, corresponding Documents will be displayed.
    /// Then the operations can be performed according to the selected document.
    /// <GenUserDtl()> is used to display the User Identity Panel.

    public partial class home : System.Web.UI.Page
    {
        #region Define reference of the Webservices
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;
        private RepositoryService repoServiceA;
        private ContentService contentService;
        private DictionaryService dictionaryService;
        private AdministrationService administrationService;
        private AuthoringService authoringService;

        private Alfresco.RepositoryWebService.Reference currentReference;
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
            Page.Header.DataBind();            
            if (!IsPostBack)
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {
                    hfUserID.Value = Session["UserID"].ToString();
                    lblUser.Text = Session["UserFullName"].ToString();
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
                    cmdView.Attributes.Add("onclick", "document.body.style.cursor = 'wait';");
                    cmdDel.Attributes.Add("onclick", "document.body.style.cursor = 'wait';");
                    cmdEmail.Attributes.Add("onclick", "document.body.style.cursor = 'wait';");
                    cmdCopy.Attributes.Add("onclick", "document.body.style.cursor = 'wait';");
                    cmdMove.Attributes.Add("onclick", "document.body.style.cursor = 'wait';");
                    /// To populate the dropdown
                    PopulateDropdown();
                    /// Function to Populate the User Identity Part
                    GenUserDtl();
                    // Function to Populate the Cabinet portion
                    PopCab();
                    // to set the previous selected path
                    Session["svReq2DispPopDrw"] = "";
                    Session["svReq2DispPopFld"] = "";
                    Session["svReq2DispPopDoc"] = "";
                    #region
                    if (Session["SelectedCabUUID"] != null)
                    {
                        gvCabinet.DataBind();
                        if (gvCabinet.DataKeys != null)
                        {
                            for (int i = 0; i < gvCabinet.DataKeys.Count; i++)
                            {
                                if (Session["SelectedCabUUID"].ToString() == gvCabinet.DataKeys[i].Value.ToString())
                                {
                                    gvCabinet.SelectedIndex = i;
                                    PopDrw(Session["SelectedCabUUID"].ToString());
                                    if (Session["SelectedDrwUUID"] != null)
                                    {
                                        gvDrawer.DataBind();
                                        if (gvDrawer.DataKeys != null)
                                        {
                                            for (int j = 0; j < gvDrawer.DataKeys.Count; j++)
                                            {
                                                if (Session["SelectedDrwUUID"].ToString() == gvDrawer.DataKeys[j].Value.ToString())
                                                {
                                                    gvDrawer.SelectedIndex = j;
                                                    PopFld(Session["SelectedDrwUUID"].ToString());
                                                    if (Session["SelectedFldUUID"] != null)
                                                    {
                                                        gvFolder.DataBind();
                                                        if (gvFolder.DataKeys != null)
                                                        {
                                                            for (int k = 0; k < gvFolder.DataKeys.Count; k++)
                                                            {
                                                                if (Session["SelectedFldUUID"].ToString() == gvFolder.DataKeys[k].Value.ToString())
                                                                {
                                                                    gvFolder.SelectedIndex = k;
                                                                    PopDoc(Session["SelectedFldUUID"].ToString());
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        gvFolder.SelectedIndex = -1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        gvDrawer.SelectedIndex = -1;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        gvCabinet.SelectedIndex = -1;
                    }
                    #endregion
                    hfSelDocPermission.Value = "X";
                }
                else
                {
                    Response.Redirect("logout.aspx", false);
                }
            }
            else
            {
                string eventTarget1 = this.Request["__EVENTTARGET1"];
                string eventArgument1 = this.Request["__EVENTARGUMENT1"];
                string eventTarget2 = this.Request["__EVENTTARGET2"];
                string eventArgument2 = this.Request["__EVENTARGUMENT2"];

                if (eventTarget2 != String.Empty && eventTarget2 == "callPostBack5")
                {
                    if (eventArgument2 != String.Empty && eventArgument2 == "OpenDoc")
                    {
                        ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                        DataSet ds01 = new DataSet();
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(hfSelDocID.Value), Session["CompCode"].ToString());

                        if (ds01.Tables[0].Rows.Count>0)
                        {
                            Session["VSDocName"] = ds01.Tables[0].Rows[0][1].ToString();
                            Session["SelDocUUID"] = ds01.Tables[0].Rows[0][4].ToString();
                            ViewDoc(Session["SelDocUUID"].ToString());
                        }
                    }
                    else if (eventArgument2 != String.Empty && eventArgument2 == "DownldDoc")
                    {
                        ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                        DataSet ds01 = new DataSet();
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(hfSelDocID.Value), Session["CompCode"].ToString());
                        if (ds01.Tables[0].Rows.Count>0)
                        {
                            DownldDoc(ds01.Tables[0].Rows[0][4].ToString(), ds01.Tables[0].Rows[0][1].ToString());
                        }
                    }

                }
            }
        }

        protected void DownldDoc(string DocUUID,string DocName)
        {
            try
            {
                DocName = DocName.Replace(" ","_");
                //Download start
                Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                spacesStore.address = "SpacesStore";

                Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                referenceForNode.store = spacesStore;
                referenceForNode.uuid = DocUUID; //Doc UUID

                Alfresco.ContentWebService.Reference[] obj_new = new Alfresco.ContentWebService.Reference[] { referenceForNode };
                Alfresco.ContentWebService.Predicate sourcePredicate = new Alfresco.ContentWebService.Predicate();
                sourcePredicate.Items = obj_new;

                // Admin Credentials start                
                WebServiceFactory wsFA = new WebServiceFactory();
                wsFA.UserName = Session["AdmUserID"].ToString();
                wsFA.Ticket = Session["AdmTicket"].ToString();
                // Admin Credentials end
                Alfresco.ContentWebService.Content[] readResult = wsFA.getContentService().read(sourcePredicate, Constants.PROP_CONTENT);

                String ticketURL = "?ticket=" + wsFA.Ticket;
                String downloadURL = readResult[0].url + ticketURL;
                Uri address = new Uri(downloadURL);

                string url = downloadURL;
                string newSaveFileName = "";
                int start = DocName.LastIndexOf(".") + 1;
                int length = DocName.Length - start;
                string fileNameExt = DocName.Substring(start, length);
                if (fileNameExt == "")
                {
                    
                }
                else
                {
                    newSaveFileName = DocName.Substring(0, DocName.Length - fileNameExt.Length - 1) + "_" + Session["UserID"].ToString() + "." + fileNameExt;
                }
                string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
                SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                //Download end
                //Now download the file to local drive
                FileInfo file = new FileInfo(file_name);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AppendHeader("Content-Disposition", "attachment; filename =" + file.Name);
                Response.AppendHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/download";
                Response.WriteFile(file.FullName);
                Response.Flush();
                Response.Close();
                Response.End();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopCab()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                gvCabinet.DataSource = ds01;
                gvCabinet.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopDrw(string SelCabID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCabID, Session["UserID"].ToString());
                gvDrawer.DataSource = ds01;
                gvDrawer.DataBind();
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    gvDrawer.SelectedIndex = -1;
                }
                else
                {
                    if (Session["svReq2DispPopDrw"].ToString() == "Y")
                    {
                        throw new Exception("The Selected Cabinet is Empty");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopFld(string SelDrwID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrwID, Session["UserID"].ToString());
                gvFolder.DataSource = ds01;
                gvFolder.DataBind();
                if (SelDrwID == "")
                {

                }
                else
                {
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        gvFolder.SelectedIndex = -1;
                    }
                    else
                    {
                        if (Session["svReq2DispPopFld"].ToString() == "Y")
                        {
                            throw new Exception("The Selected Drawer is Empty");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopDoc(string SelFldID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(SelFldID, Session["UserID"].ToString());
                gvDocument.DataSource = ds01;
                gvDocument.DataBind();
                if (SelFldID == "")
                {

                }
                else
                {
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        gvDocument.SelectedIndex = -1;
                    }
                    else
                    {
                        if (Session["svReq2DispPopDoc"].ToString() == "Y")
                        {
                            throw new Exception("The Selected Folder is Empty");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateDropdown()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //....Doc Type
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectDocTypeCompBased(Session["CompCode"].ToString());
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
        protected void gvCabinet_SelectedIndexChanged(object sender, EventArgs e)
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
                GridViewRow row = gvCabinet.SelectedRow;
                Label lblCabinet = (Label)row.FindControl("lblCabinet");
                Session["VSCab"] = lblCabinet.Text;
                Session["SelectedCabUUID"] = lblCabinet.Text;
                Session["SelectedDrwUUID"] = "";
                Session["SelectedFldUUID"] = "";
                Session["SelectedDocID"] = "";
                Session["VSDoc"] = "";
                Session["VSFld"] = "";
                Session["VSDrw"] = "";
                hfDocument.Value = "";
                Session["svReq2DispPopDrw"] = "Y";
                Session["svReq2DispPopFld"] = "";
                Session["svReq2DispPopDoc"] = "";
                PopDrw(Session["VSCab"].ToString());
                PopFld(Session["VSDrw"].ToString());
                PopDoc(Session["VSFld"].ToString());
            }
            catch (Exception ex)
            {
                PopDoc(Session["VSFld"].ToString());
                PopFld(Session["VSDrw"].ToString());
                hfMsg1.Value = ex.Message;
            }
        }
        protected void gvDrawer_SelectedIndexChanged(object sender, EventArgs e)
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
                GridViewRow row = gvDrawer.SelectedRow;
                Label lblDrawer = (Label)row.FindControl("lblDrawer");
                Session["VSDrw"] = lblDrawer.Text;
                Session["SelectedDrwUUID"] = lblDrawer.Text;
                Session["SelectedFldUUID"] = "";
                Session["SelectedDocID"] = "";
                Session["VSDoc"] = "";
                Session["VSFld"] = "";
                hfDocument.Value = "";
                Session["svReq2DispPopDrw"] = "Y";
                Session["svReq2DispPopFld"] = "Y";
                Session["svReq2DispPopDoc"] = "";
                PopFld(Session["VSDrw"].ToString());
                PopDoc(Session["VSFld"].ToString());
            }
            catch (Exception ex)
            {
                PopDoc(Session["VSFld"].ToString());
                hfMsg1.Value = ex.Message;
            }
        }
        protected void gvFolder_SelectedIndexChanged(object sender, EventArgs e)
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
                GridViewRow row = gvFolder.SelectedRow;
                Label lblFolder = (Label)row.FindControl("lblFolder");
                Session["VSFld"] = lblFolder.Text;
                Session["SelectedFldUUID"] = lblFolder.Text;
                Session["SelectedDocID"] = "";
                UserRights RightsObj = new UserRights();
                hfSelFldPermission.Value = RightsObj.FetchPermission4User(lblFolder.Text.ToString(), Session["UserID"].ToString());
                Session["VSDoc"] = "";
                hfDocument.Value = "";
                Session["svReq2DispPopDrw"] = "Y";
                Session["svReq2DispPopFld"] = "Y";
                Session["svReq2DispPopDoc"] = "Y";
                PopDoc(Session["VSFld"].ToString());
            }
            catch (Exception ex)
            {
                hfMsg1.Value = ex.Message;
            }
        }
        protected void gvDocument_SelectedIndexChanged(object sender, EventArgs e)
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
                GridViewRow row = gvDocument.SelectedRow;
                Label lblDocument = (Label)row.FindControl("lblDocument");
                Session["VSDoc"] = lblDocument.Text;
                Label lblDocumentName = (Label)row.FindControl("lblDocumentName");
                Session["VSDocName"] = lblDocumentName.Text;
                Session["SelectedDocID"] = lblDocument.Text;

                if (Session["VSCab"] != null && Session["VSDrw"] != null && Session["VSFld"] != null && Session["VSDoc"] != null)
                {
                    home_bal Obj_HomeBAL = new home_bal();
                    Obj_HomeBAL.CabName = Session["VSCab"].ToString(); //Cabinet UUID
                    Obj_HomeBAL.DrwName = Session["VSDrw"].ToString(); //Drawer UUID
                    Obj_HomeBAL.FldName = Session["VSFld"].ToString(); //Folder UUID
                    Obj_HomeBAL.DocName = Session["VSDoc"].ToString(); //Document ID
                    hf_SelDocID.Value = Session["VSDoc"].ToString();
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(Session["VSDoc"].ToString()), Session["CompCode"].ToString());

                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        hfDocument.Value = ds01.Tables[0].Rows[0][0].ToString();
                        if (ds01.Tables[0].Rows[0][19].ToString() != "")
                        {
                            lblUpldBy.Text= ds01.Tables[0].Rows[0][26].ToString();
                            lblUpldDt.Text= ds01.Tables[0].Rows[0][7].ToString();
                            lblDocStat.Text= ds01.Tables[0].Rows[0][18].ToString();
                            lblDocType.Text = ds01.Tables[0].Rows[0][25].ToString();
                            lbDocName.Text = ds01.Tables[0].Rows[0][1].ToString();
                            hfDocExt.Value = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(lbDocName.Text);
                            double fileSize = Convert.ToDouble(ds01.Tables[0].Rows[0][24].ToString());
                            string FileSize = "";
                            if (fileSize >= 1024)
                            {
                                fileSize = Math.Round(fileSize / 1024, 2);
                                FileSize = fileSize.ToString() + " MB";
                            }
                            else
                            {
                                FileSize = fileSize.ToString() + " KB";
                            }
                            lblDocSize.Text = FileSize;
                            DataSet ds001 = new DataSet();
                            ds001.Reset();
                            ds001 = ObjClassStoreProc.FullPathPassingFolderUUID(Session["VSFld"].ToString());
                            if (ds001.Tables[0].Rows.Count > 0)
                            {
                                lbPath.Text = ds001.Tables[0].Rows[0][2].ToString() + "\\" + ds001.Tables[0].Rows[0][1].ToString() + "\\" + ds001.Tables[0].Rows[0][0].ToString();
                            }

                            lblDocUUID.Text = ds01.Tables[0].Rows[0][4].ToString();
                            Session["TempDocTypeID"] = ds01.Tables[0].Rows[0][2].ToString();
                            Session["SelDocUUID"] = ds01.Tables[0].Rows[0][4].ToString();
                            Session["SelDocName"] = Session["VSDocName"].ToString();
                            hfSelDocStat.Value = ds01.Tables[0].Rows[0][18].ToString();
                            hfCheckedOutBy.Value = ds01.Tables[0].Rows[0][21].ToString();
                            ////For Full Name of the User
                            ds001.Reset();
                            ds001 = ObjClassStoreProc.FullNamePassingUserID(hfCheckedOutBy.Value);
                            if (ds001.Tables[0].Rows.Count > 0)
                            {
                                hfCheckedOutByFullName.Value = ds001.Tables[0].Rows[0][0].ToString();
                            }
                            UserRights RightsObj = new UserRights();
                            hfSelDocPermission.Value = RightsObj.FetchPermission4User(Session["SelDocUUID"].ToString(), Session["UserID"].ToString());

                            string LicenseKey = "";
                            string ServerIPAddress = "";
                            string DomainName = "";
                            //// Fetch ServerConfig Details Start
                            DataSet ds_01 = new DataSet();
                            ds_01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
                            if (ds_01.Tables[0].Rows.Count > 0)
                            {
                                LicenseKey = ds_01.Tables[0].Rows[0][0].ToString();
                                ServerIPAddress = ds_01.Tables[0].Rows[0][1].ToString();
                                DomainName = ds_01.Tables[0].Rows[0][2].ToString();
                            }
                            // Fetch ServerConfig Details End
                            string strURL = "";
                            strURL = "http://" + DomainName + "/Default.aspx?URLDocUUID=" + Session["SelDocUUID"].ToString() + "&UC=6a800c19d0014966809c8838507";
                            hfDocURL.Value = strURL;
                        }
                        else
                        {
                            throw new Exception("This document can not be viewed");
                        }
                    }
                    else
                    {
                        throw new Exception("Please select a document");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 0; i < (e.Row.Cells.Count); i++)
                    {
                        string DocID = ((DataRowView)e.Row.DataItem)["doc_id"].ToString();
                        string DocName = ((DataRowView)e.Row.DataItem)["doc_name"].ToString();
                        int start = DocName.LastIndexOf(".") + 1;
                        int length = DocName.Length - start;
                        string fileNameExt = DocName.Substring(start, length);
                        if (fileNameExt == "pdf")
                        {
                            e.Row.Attributes.Add("ondblclick", "Javascript: rowDblClick(" + DocID + ");");
                        }
                        else
                        {
                            e.Row.Attributes.Add("ondblclick", "Javascript: DocDownld(" + DocID + ");");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value= ex.Message;
            }
        }

        protected void cmdView_Click(object sender, EventArgs e)
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
                if (hfSelDocStat.Value == "Check Out")
                {
                    if (Session["UserID"].ToString() == "admin")
                    {

                    }
                    else if (Session["UserID"].ToString() == hfCheckedOutBy.Value) // If Checked Out by this User
                    {

                    }
                    else
                    {
                        throw new Exception("This Document is Checked Out by " + hfCheckedOutByFullName.Value);
                    }
                }
                string DocName = Session["VSDocName"].ToString();
                int start = DocName.LastIndexOf(".") + 1;
                int length = DocName.Length - start;
                string fileNameExt = DocName.Substring(start, length);
                if (fileNameExt == "pdf")
                {
                    ViewDoc(Session["SelDocUUID"].ToString());
                }
                else
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(Session["VSDoc"].ToString()), Session["CompCode"].ToString());
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        DownldDoc(ds01.Tables[0].Rows[0][4].ToString(), ds01.Tables[0].Rows[0][1].ToString());
                    }
                }
                
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void ViewDoc(string DocUUID)
        {
            try
            {
                // At first download the selected file from alfresco and save it to <TempDownload> Folder and then open the file
                //Download start
                // Initialise the reference to the spaces store
                Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                spacesStore.address = "SpacesStore"; 

                Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                referenceForNode.store = spacesStore;
                referenceForNode.uuid = DocUUID; //Doc UUID

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
                string newSaveFileName = "";
                if (Session["VSDocName"].ToString().LastIndexOf(".pdf") == -1)
                {
                    newSaveFileName = Session["VSDocName"].ToString() + "_" + Session["UserID"].ToString() + ".pdf";
                }
                else
                {
                    newSaveFileName = Session["VSDocName"].ToString().Substring(0, Session["VSDocName"].ToString().Length - 4) + "_" + Session["UserID"].ToString() + ".pdf";
                }
                string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
                SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                //Download end
                Session["hfPageControl"] = "F";
                Response.Redirect("FormFillup.aspx?docname=" + newSaveFileName + "&DocUUID=" + DocUUID, false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To check out the selected document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCheckOut_Click(object sender, EventArgs e)
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
                if (Session["VSCab"] != null && Session["VSDrw"] != null && Session["VSFld"] != null && Session["VSDoc"]!=null)
                {
                    home_bal Obj_HomeBAL = new home_bal();
                    Obj_HomeBAL.CabName = Session["VSCab"].ToString();
                    Obj_HomeBAL.DrwName = Session["VSDrw"].ToString();
                    Obj_HomeBAL.FldName = Session["VSFld"].ToString();
                    Obj_HomeBAL.DocName = Session["VSDoc"].ToString();

                    DataSet ds1 = new DataSet();
                    ds1 = Obj_HomeBAL.SelectDocDtl();
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0][2].ToString() == "Check Out")
                        {
                            throw new Exception("This document is already Checked Out");
                        }
                        else
                        {
                            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                            string result = ObjClassStoreProc.DocCheckInCheckOut(Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString()), "Check Out", Session["UserID"].ToString());
                            if (Convert.ToInt32(result) > 0)
                            {
                                DataSet ds01 = new DataSet();
                                ds01.Reset();
                                #region Maintaining DocLog
                                string ActualDocUUID=ds1.Tables[0].Rows[0][4].ToString();
                                string MaxID = ObjClassStoreProc.MaxAutoID4DocLog(ActualDocUUID, Session["CompCode"].ToString());
                                string InsertDocLog = ObjClassStoreProc.InsertDocLog(MaxID, ds1.Tables[0].Rows[0][4].ToString(), ds1.Tables[0].Rows[0][4].ToString(), Session["UserID"].ToString(), "Document has been Checked Out by ", Session["CompCode"].ToString());
                                #endregion
                                ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                                gvDocument.DataSource = ds01;
                                gvDocument.DataBind();
                                UpdatePanel4.Update();
                                throw new Exception("This Document has been Checked Out");
                            }
                        }
                    }
                    else
                    {
                        //throw new Exception("Please select a Document");
                    }
                }
                else
                {
                    //throw new Exception("Please select a Document");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        /// <summary>
        /// To check in the selected document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCheckIn_Click(object sender, EventArgs e)
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
                if (Session["VSCab"] != null && Session["VSDrw"] != null && Session["VSFld"] != null && Session["VSDoc"] != null)
                {
                    home_bal Obj_HomeBAL = new home_bal();
                    Obj_HomeBAL.CabName = Session["VSCab"].ToString();
                    Obj_HomeBAL.DrwName = Session["VSDrw"].ToString();
                    Obj_HomeBAL.FldName = Session["VSFld"].ToString();
                    Obj_HomeBAL.DocName = Session["VSDoc"].ToString();

                    DataSet ds1 = new DataSet();
                    ds1 = Obj_HomeBAL.SelectDocDtl();
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        ViewState["SelDocID"] = Convert.ToInt64(ds1.Tables[0].Rows[0][0]);
                        lblDocNameCheckIn.Text = Session["VSDocName"].ToString();
                        if (ds1.Tables[0].Rows[0][2].ToString() == "Check In")
                        {
                            MsgNodetCheckIn.Text = ""; //"This document is already checked in";
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        MessageBox("Please select a document");
                    }
                }
                else
                {
                    MessageBox("Please select a document");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void btnComments_Click(object sender, EventArgs e)
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
                string mAction = "";
                string mStage = "";
                Int64 WFID = 0;
                DataSet ds01 = new DataSet();
                DataSet ds001 = new DataSet();
                DataSet ds02 = new DataSet();
                DataSet ds03 = new DataSet();
                DataTable dt1 = new DataTable();

                dt1.Columns.Add("Stage", typeof(string));
                dt1.Columns.Add("User", typeof(string));
                dt1.Columns.Add("Date", typeof(string));
                dt1.Columns.Add("Comments", typeof(string));
                dt1.Columns.Add("Action", typeof(string));

                string StartDocID = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();

                StartDocID = Session["VSDoc"].ToString();
                ds01.Reset();
                ds01 = ObjClassStoreProc.WFLogDetailsPassingDocID(StartDocID);

                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        WFID = Convert.ToInt64(ds01.Tables[0].Rows[i][1].ToString());
                        ds02.Reset();
                        ds02 = ObjClassStoreProc.IntTaskWithCommPassingWFLogID(ds01.Tables[0].Rows[i][0].ToString());

                        if (ds02.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds02.Tables[0].Rows.Count; j++)
                            {
                                if (j >= 1 && (ds02.Tables[0].Rows[j][0].ToString() == ds02.Tables[0].Rows[j - 1][0].ToString()))
                                {

                                }
                                else
                                {
                                    if (mStage == "2" && mAction == "Reject")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        DataRow r = dt1.NewRow();
                                        r["Stage"] = ds02.Tables[0].Rows[j][0].ToString();
                                        mStage = ds02.Tables[0].Rows[j][0].ToString();
                                        // for waiting
                                        if (ds02.Tables[0].Rows[j][2].ToString() == "Waiting" && ds02.Tables[0].Rows[j][3].ToString() == "Waiting")
                                        {
                                            r["Action"] = "";
                                            mAction = "";
                                            r["Comments"] = "";
                                        }
                                        else
                                        {
                                            r["Action"] = ds02.Tables[0].Rows[j][1].ToString();
                                            mAction = ds02.Tables[0].Rows[j][1].ToString();
                                            r["Comments"] = ds02.Tables[0].Rows[j][3].ToString();
                                        }
                                        r["Date"] = ds02.Tables[0].Rows[j][2].ToString();
                                        if (ds02.Tables[0].Rows[j][4].ToString() == "")
                                        {
                                            #region For the User who is responsible for this stage
                                            DataSet ds003 = new DataSet();
                                            ds003.Reset();
                                            ds003 = ObjClassStoreProc.UserPassingWFLogIDANDStep(ds01.Tables[0].Rows[i][0].ToString(), ds02.Tables[0].Rows[j][0].ToString());
                                            r["User"] = ds003.Tables[0].Rows[0][0].ToString();
                                            #endregion
                                        }
                                        else
                                        {
                                            ds03.Reset();
                                            ds03 = ObjClassStoreProc.FullNamePassingUserID(ds02.Tables[0].Rows[j][4].ToString());
                                            if (ds03.Tables[0].Rows.Count > 0)
                                            {
                                                r["User"] = ds03.Tables[0].Rows[0][0].ToString();
                                            }
                                        }
                                        dt1.Rows.Add(r);
                                    }
                                }
                            }
                        }
                    }
                }

                gvComment.DataSource = dt1;
                gvComment.DataBind();
                dt1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To check in the selected document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCheckIn_Click(object sender, EventArgs e)
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
                if (hfSelCheckIn.Value == "OptExist")
                {
                    if (MsgNodetCheckIn.Text != "This document is already checked in")
                    {
                        ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                        string result = ObjClassStoreProc.DocCheckInCheckOut(Convert.ToInt32(ViewState["SelDocID"].ToString()), "Check In", Session["UserID"].ToString());
                        if (Convert.ToInt32(result) > 0)
                        {
                            DataSet ds01 = new DataSet();
                            ds01.Reset();
                            #region Maintaining DocLog
                            string ActualDocUUID="";
                            ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt32(ViewState["SelDocID"].ToString()), Session["CompCode"].ToString());
                            ActualDocUUID = ds01.Tables[0].Rows[0][4].ToString();
                            string MaxID = ObjClassStoreProc.MaxAutoID4DocLog(ActualDocUUID, Session["CompCode"].ToString());
                            string InsertDocLog = ObjClassStoreProc.InsertDocLog(MaxID, ActualDocUUID, ActualDocUUID, Session["UserID"].ToString(), "Without any modification the Document has been Checked In by ", Session["CompCode"].ToString());
                            #endregion
                            ds01.Reset();
                            ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                            gvDocument.DataSource = ds01;
                            gvDocument.DataBind();
                            UpdatePanel4.Update();
                            MessageBox("This document has been checked in");
                        }
                    }
                    else
                    {
                        MsgNodetCheckIn.Text = "This document is already checked in";
                    }
                }
                else if (hfSelCheckIn.Value == "OptNew")
                {
                    Response.Redirect("doc_mast.aspx?CIDocUUID=" + lblDocUUID.Text, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To copy the selected document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
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
                MsgNodetCopy.Text = "";
                if (Session["VSCab"] != null && Session["VSDrw"] != null && Session["VSFld"] != null && Session["VSDoc"] != null)
                {
                    home_bal Obj_HomeBAL = new home_bal();
                    Obj_HomeBAL.CabName = Session["VSCab"].ToString();
                    Obj_HomeBAL.DrwName = Session["VSDrw"].ToString();
                    Obj_HomeBAL.FldName = Session["VSFld"].ToString();
                    Obj_HomeBAL.DocName = Session["VSDoc"].ToString();

                    DataSet ds1 = new DataSet();
                    ds1 = Obj_HomeBAL.SelectDocDtl();
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        ViewState["SelDocID"] = Convert.ToInt64(ds1.Tables[0].Rows[0][0]);
                        ViewState["SelDocUUID"] = ds1.Tables[0].Rows[0][4].ToString();
                        lblDocNameCopy.Text = Session["VSDocName"].ToString();
                        if (ds1.Tables[0].Rows[0][2].ToString() == "Check Out")
                        {
                            MsgNodetCopy.Text = "";// "This document has been checked out";
                        }
                        else
                        {
                            PopulateCabinet1();
                        }
                    }
                    else
                    {
                        MessageBox("Please select a document");
                    }
                }
                else
                {
                    MessageBox("Please select a document");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdCopy_Click(object sender, EventArgs e)
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
                if (MsgNodetCopy.Text == "This document has been checked out")
                {
                    throw new Exception("This Document is Checked Out -- so can not be Copied!!");
                }
                else
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    string result = "";
                    /// Check the doc is already there or not start
                    DataSet ds0001 = new DataSet();
                    DataSet ds0003 = new DataSet();
                    ds0003.Reset();
                    ds0003 = ObjClassStoreProc.DocDetailsPassingDocIDANDFldUUID(ViewState["SelDocID"].ToString(), ddFolder1.SelectedValue);
                    if (ds0003.Tables[0].Rows.Count > 0)
                    {
                        throw new Exception("This document is already in the selected folder");
                    }
                    /// Check the doc is already is there or not end


                    ds0003.Reset();
                    ds0003 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt32(ViewState["SelDocID"].ToString()), Session["CompCode"].ToString());
                    double FileSize = Convert.ToDouble(ds0003.Tables[0].Rows[0][24].ToString());
                    #region Validation of space if it is exceeding or not
                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    con.Open();
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
                        throw new Exception("You do not have enough space to make a copy of this Document. Please contact with Administrator.");
                    }
                    #endregion
                    
                    // Initialise the reference to the spaces store
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";
                    //create a predicate with the first CMLCreate result
                    Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = ViewState["SelDocUUID"].ToString(); // Selected Doc's UUID

                    Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                    Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                    sourcePredicate.Items = obj_new;

                    //create a reference from the second CMLCreate performed for space
                    Alfresco.RepositoryWebService.Reference referenceForTargetSpace = new Alfresco.RepositoryWebService.Reference();
                    referenceForTargetSpace.store = this.spacesStore;
                    referenceForTargetSpace.uuid = ddFolder1.SelectedValue; // Selected Location's (Folder's) UUID

                    //reference for the target space
                    Alfresco.RepositoryWebService.ParentReference targetSpace = new Alfresco.RepositoryWebService.ParentReference();
                    targetSpace.store =this.spacesStore;
                    targetSpace.uuid = referenceForTargetSpace.uuid;
                    targetSpace.associationType = Constants.ASSOC_CONTAINS;
                    targetSpace.childName = Session["VSDocName"].ToString(); // Selected Doc's Name

                    //copy content
                    CMLCopy copy = new CMLCopy();
                    copy.where = sourcePredicate;
                    copy.to = targetSpace;

                    CML cmlCopy = new CML();
                    cmlCopy.copy = new CMLCopy[] { copy };

                    //perform a CML update to copy the node
                    WebServiceFactory wsF = new WebServiceFactory();
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getRepositoryService().update(cmlCopy);

                    SearchNode ObjSearchNode = new SearchNode();
                    string CopiedDocUUID = ObjSearchNode.ExistNode(ddFolder1.SelectedValue, targetSpace.childName, Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                    
                    // .Net & SQL Server Coding Start
                    ds0001.Reset();
                    ds0001 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt32(ViewState["SelDocID"].ToString()), Session["CompCode"].ToString());
                    if (ds0001.Tables[0].Rows.Count > 0)
                    {
                        result = ObjClassStoreProc.ExistDoc(ds0001.Tables[0].Rows[0][1].ToString(), ddFolder1.SelectedValue, Session["CompCode"].ToString());
                        if (Convert.ToInt32(result) == -1)
                        {
                            throw new Exception("Document already exists in this folder!");
                        }
                        else
                        {
                            result = ObjClassStoreProc.InsertDocMast(ds0001.Tables[0].Rows[0][1].ToString(), ds0001.Tables[0].Rows[0][1].ToString(), ddFolder1.SelectedValue, ds0001.Tables[0].Rows[0][2].ToString(), ds0001.Tables[0].Rows[0][3].ToString(), Session["UserID"].ToString(), DateTime.Now, ds0001.Tables[0].Rows[0][8].ToString(), ds0001.Tables[0].Rows[0][9].ToString(), ds0001.Tables[0].Rows[0][10].ToString(), ds0001.Tables[0].Rows[0][11].ToString(), ds0001.Tables[0].Rows[0][12].ToString(), ds0001.Tables[0].Rows[0][13].ToString(), ds0001.Tables[0].Rows[0][14].ToString(), ds0001.Tables[0].Rows[0][15].ToString(), ds0001.Tables[0].Rows[0][16].ToString(), ds0001.Tables[0].Rows[0][17].ToString(), "", CopiedDocUUID, targetSpace.uuid + "/" + ds0001.Tables[0].Rows[0][1].ToString().Replace(" ", "%20"), "C", Session["CompCode"].ToString(), Convert.ToDouble(ds0001.Tables[0].Rows[0][24].ToString()));
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
                                DataSet ds01 = new DataSet();
                                ds01.Reset();
                                #region Maintaining DocLog
                                string ActualDocUUID = "";
                                ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt32(ViewState["SelDocID"].ToString()), Session["CompCode"].ToString());
                                ActualDocUUID = ds01.Tables[0].Rows[0][4].ToString();
                                string MaxID = ObjClassStoreProc.MaxAutoID4DocLog(ActualDocUUID, Session["CompCode"].ToString());
                                string InsertDocLog = ObjClassStoreProc.InsertDocLog(MaxID, ActualDocUUID, referenceForNode.uuid, Session["UserID"].ToString(), "Document has been copied by ", Session["CompCode"].ToString());
                                #endregion
                                cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + Convert.ToDouble(ds0001.Tables[0].Rows[0][24].ToString()) + "' where CompCode='" + Session["CompCode"].ToString() + "'", con);
                                cmd.ExecuteNonQuery();
                                cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + Session["CompCode"].ToString() + "'", con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                                DataSet dsPerm = new DataSet();
                                dsPerm.Reset();
                                UserRights RightsObj = new UserRights();
                                dsPerm = RightsObj.FetchPermission(ddFolder1.SelectedValue, Session["CompCode"].ToString());
                                if (dsPerm.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                                    {
                                        RightsObj.InsertPermissionSingleData(CopiedDocUUID, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                                    }
                                }
                                Response.Redirect("home.aspx",false);
                            }
                        }
                    }
                    Utility.CloseConnection(con);
                    /// .Net & SQL Server Coding End
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void PopUser()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectUserAll(Session["CompCode"].ToString());
                ddUser.DataSource = ds01;
                ddUser.DataTextField = "name";
                ddUser.DataValueField = "email";
                ddUser.DataBind();
                ddUser.Items.Insert(0, "init@init.com");
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To mail the selected document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEmail_Click(object sender, EventArgs e)
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
                if (Session["VSCab"] != null && Session["VSDrw"] != null && Session["VSFld"] != null && Session["VSDoc"] != null)
                {
                    home_bal Obj_HomeBAL = new home_bal();
                    Obj_HomeBAL.CabName = Session["VSCab"].ToString();
                    Obj_HomeBAL.DrwName = Session["VSDrw"].ToString();
                    Obj_HomeBAL.FldName = Session["VSFld"].ToString();
                    Obj_HomeBAL.DocName = Session["VSDoc"].ToString();

                    DataSet ds1 = new DataSet();
                    ds1 = Obj_HomeBAL.SelectDocDtl();
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        ViewState["SelDocID"] = Convert.ToInt64(ds1.Tables[0].Rows[0][0]);
                        ViewState["SelDocUUID"] =ds1.Tables[0].Rows[0][4].ToString();
                        lblDocNameMail.Text = Session["VSDocName"].ToString();
                        lblDocNameAttach.Text = Session["VSDocName"].ToString();
                        PopUser();
                    }
                    else
                    {
                        MessageBox("Please select a document");
                    }
                }
                else
                {
                    MessageBox("Please select a document");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdEmail_Click(object sender, EventArgs e)
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
                if (hfEmailStat.Value == "Y")
                {
                    cmdEmail.Enabled = false;
                    string SenderMail = "";
                    string SenderName = "";
                    string SmtpHost = "";
                    Int32 SmtpPort = 0;
                    string CredenUsername = "";
                    string CredenPwd = "";
                    string MailTo = "";
                    string MailSub = "";
                    string MailMsg = "";
                    string MailFrom = "";
                    string AssignedBy = "";
                    DataSet ds05 = new DataSet();
                    DataSet ds001 = new DataSet();
                    FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();

                    // Initialise the reference to the spaces store
                    Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                    spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                    spacesStore.address = "SpacesStore";

                    Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                    referenceForNode.store = spacesStore;
                    referenceForNode.uuid = Session["SelDocUUID"].ToString();//Doc UUID

                    Alfresco.ContentWebService.Reference[] obj_new = new Alfresco.ContentWebService.Reference[] { referenceForNode };
                    Alfresco.ContentWebService.Predicate sourcePredicate = new Alfresco.ContentWebService.Predicate();
                    sourcePredicate.Items = obj_new;

                    // Admin Credentials start
                    WebServiceFactory wsFA = new WebServiceFactory();
                    wsFA.UserName = Session["AdmUserID"].ToString();
                    wsFA.Ticket = Session["AdmTicket"].ToString();
                    // Admin Credentials end
                    Alfresco.ContentWebService.Content[] readResult = wsFA.getContentService().read(sourcePredicate, Constants.PROP_CONTENT);

                    String ticketURL = "?ticket=" + wsFA.Ticket;
                    String downloadURL = readResult[0].url + ticketURL;
                    Uri address = new Uri(downloadURL);
                    string url = downloadURL;
                    string newSaveFileName = "";
                    //newSaveFileName = ObjFetchOnlyNameORExtension.FetchOnlyDocName(Session["VSDocName"].ToString()) + "_" + Session["UserID"].ToString() + "." + ObjFetchOnlyNameORExtension.FetchOnlyDocExt(Session["VSDocName"].ToString());
                    newSaveFileName = ObjFetchOnlyNameORExtension.FetchOnlyDocName(Session["VSDocName"].ToString()) + "_" + GetTimestamp(DateTime.Now) + "." + ObjFetchOnlyNameORExtension.FetchOnlyDocExt(Session["VSDocName"].ToString());
                    
                    string file_name = Server.MapPath("eMailDocs") + "\\" + newSaveFileName;
                    SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                    ObjSaveFileFromURL.SaveFile4mURL(file_name, url);

                    mailing ObjMailSetup = new mailing();
                    ds001 = ObjMailSetup.MailSettings();
                    if (ds001.Tables[0].Rows.Count > 0)
                    {
                        SenderMail = ds001.Tables[0].Rows[0][0].ToString();
                        SenderName = ds001.Tables[0].Rows[0][1].ToString();
                        SmtpHost = ds001.Tables[0].Rows[0][2].ToString();
                        SmtpPort = Convert.ToInt32(ds001.Tables[0].Rows[0][3].ToString());
                        CredenUsername = ds001.Tables[0].Rows[0][4].ToString();
                        CredenPwd = ds001.Tables[0].Rows[0][5].ToString();
                    }

                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    ds05.Reset();
                    ds05 = ObjClassStoreProc.UserInfoPassingUserID(Session["UserID"].ToString());
                    if (ds05.Tables[0].Rows.Count > 0)
                    {
                        MailFrom = ds05.Tables[0].Rows[0][3].ToString();
                        AssignedBy = ds05.Tables[0].Rows[0][1].ToString() + " " + ds05.Tables[0].Rows[0][2].ToString();
                    }

                    //AssignedBy = Session["UserID"].ToString();

                    MailSub = txtSubject.Text.Trim();
                    MailMsg = txtMsg.Text.Trim();
                    MailTo = txtToMail.Text.Trim();
                    mailing Obj_Mail = new mailing();
                    try
                    {
                        Obj_Mail.SendEmail(file_name, AssignedBy, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd);
                        cmdEmail.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        cmdEmail.Enabled = true;
                        throw new Exception(ex.Message);
                    }
                    //Redirect2Home("Document uploaded successfully!", "home.aspx");
                    //Response.Redirect("home.aspx", false);
                    throw new Exception("The Document has been emailed successfully !!");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value=ex.Message;
            }
        }
        
        /// <summary>
        /// To move the selected document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMove_Click(object sender, EventArgs e)
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
                MsgNodetCopy.Text = "";
                if (Session["VSCab"] != null && Session["VSDrw"] != null && Session["VSFld"] != null && Session["VSDoc"] != null)
                {
                    home_bal Obj_HomeBAL = new home_bal();
                    Obj_HomeBAL.CabName = Session["VSCab"].ToString();
                    Obj_HomeBAL.DrwName = Session["VSDrw"].ToString();
                    Obj_HomeBAL.FldName = Session["VSFld"].ToString();
                    Obj_HomeBAL.DocName = Session["VSDoc"].ToString();

                    DataSet ds1 = new DataSet();
                    ds1 = Obj_HomeBAL.SelectDocDtl();
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        ViewState["SelDocID"] = Convert.ToInt64(ds1.Tables[0].Rows[0][0]);
                        ViewState["SelDocUUID"] = ds1.Tables[0].Rows[0][4].ToString();
                        lblDocNameMove.Text = Session["VSDocName"].ToString();
                        if (ds1.Tables[0].Rows[0][2].ToString() == "Check Out")
                        {
                            MsgNodetCopy.Text = "";// "This document has been checked out";
                        }
                        else
                        {
                            PopulateCabinet2();
                        }
                    }
                    else
                    {
                        MessageBox("Please select a document");
                    }
                }
                else
                {
                    MessageBox("Please select a document");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdMove_Click(object sender, EventArgs e)
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
                if (MsgNodetCopy.Text == "This document has been checked out")
                {
                    throw new Exception("This Document is Checked Out -- so can not be Moved!!");
                }
                else
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    string result = "";
                    /// Check the selected doc is in a running workflow or not start
                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    con.Open();
                    DataSet ds0001 = new DataSet();
                    DataSet ds0002 = new DataSet();
                    DataSet ds0003 = new DataSet();
                    cmd = new SqlCommand("select * from wf_log_mast where doc_id='" + ViewState["SelDocID"].ToString() + "' and wf_prog_stat='Completed'", con);
                    SqlDataAdapter adapter0002 = new SqlDataAdapter(cmd);
                    adapter0002.Fill(ds0002);
                    if (ds0002.Tables[0].Rows.Count > 0)
                    {
                        throw new Exception("This document is in a running workflow. You can not move this now.");
                    }
                    ds0003.Reset();
                    ds0003 = ObjClassStoreProc.DocDetailsPassingDocIDANDFldUUID(ViewState["SelDocID"].ToString(), ddFolder2.SelectedValue);
                    if (ds0003.Tables[0].Rows.Count > 0)
                    {
                        throw new Exception("This document is already in the selected folder");
                    }
                    /// Check the doc is already is there or not end

                    // Initialise the reference to the spaces store
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";

                    Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = ViewState["SelDocUUID"].ToString(); // Selected Doc's UUID

                    Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                    Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                    sourcePredicate.Items = obj_new;

                    Alfresco.RepositoryWebService.Reference referenceForTargetSpace = new Alfresco.RepositoryWebService.Reference();
                    referenceForTargetSpace.store = this.spacesStore;
                    referenceForTargetSpace.uuid = ddFolder2.SelectedValue; //ViewState["SelLocUUID"].ToString(); // Selected folder's UUID

                    //reference for the target space
                    Alfresco.RepositoryWebService.ParentReference targetSpace = new Alfresco.RepositoryWebService.ParentReference();
                    targetSpace.store = this.spacesStore;
                    targetSpace.uuid = referenceForTargetSpace.uuid;
                    targetSpace.associationType = Constants.ASSOC_CONTAINS;
                    targetSpace.childName = Session["VSDocName"].ToString(); // Selected Doc's Name

                    //copy content
                    CMLMove move = new CMLMove();
                    move.where = sourcePredicate;
                    move.to = targetSpace;

                    CML cmlMove = new CML();
                    cmlMove.move = new CMLMove[] { move };
                    
                    //perform a CML update to copy the node
                    WebServiceFactory wsF = new WebServiceFactory();
                    wsF.UserName = Session["AdmUserID"].ToString();
                    wsF.Ticket = Session["AdmTicket"].ToString();
                    wsF.getRepositoryService().update(cmlMove);

                    /// .Net & SQL Server Coding Start
                    ds0001.Reset();
                    ds0001 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt32(ViewState["SelDocID"].ToString()),Session["CompCode"].ToString());
                    if (ds0001.Tables[0].Rows.Count > 0)
                    {
                        result = ObjClassStoreProc.ExistDoc(ds0001.Tables[0].Rows[0][1].ToString(), ddFolder2.SelectedValue, Session["CompCode"].ToString());
                        if (Convert.ToInt32(result) == -1)
                        {
                            throw new Exception("Document already exists in this folder!");
                        }
                        else
                        {
                            cmd = new SqlCommand("delete from doc_mast where doc_id='" + ViewState["SelDocID"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                            result = ObjClassStoreProc.InsertDocMast(ds0001.Tables[0].Rows[0][1].ToString(), ds0001.Tables[0].Rows[0][1].ToString(), ddFolder2.SelectedValue, ds0001.Tables[0].Rows[0][2].ToString(), ds0001.Tables[0].Rows[0][3].ToString(), Session["UserID"].ToString(), DateTime.Now, ds0001.Tables[0].Rows[0][8].ToString(), ds0001.Tables[0].Rows[0][9].ToString(), ds0001.Tables[0].Rows[0][10].ToString(), ds0001.Tables[0].Rows[0][11].ToString(), ds0001.Tables[0].Rows[0][12].ToString(), ds0001.Tables[0].Rows[0][13].ToString(), ds0001.Tables[0].Rows[0][14].ToString(), ds0001.Tables[0].Rows[0][15].ToString(), ds0001.Tables[0].Rows[0][16].ToString(), ds0001.Tables[0].Rows[0][17].ToString(), "", referenceForNode.uuid, targetSpace.uuid + "/" + ds0001.Tables[0].Rows[0][1].ToString().Replace(" ", "%20"), "M", Session["CompCode"].ToString(), Convert.ToDouble(ds0001.Tables[0].Rows[0][24].ToString()));
                            if (Convert.ToInt32(result) == -1)
                            {
                                throw new Exception("Document already exists in this folder!");
                            }
                            else
                            {
                                Response.Redirect("home.aspx", false);
                            }
                        }
                    }
                    Utility.CloseConnection(con);
                    /// .Net & SQL Server Coding End
                    
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void btnGroup_Click(object sender, EventArgs e)
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
                if (Session["VSCab"] != null && Session["VSDrw"] != null && Session["VSFld"] != null && Session["VSDoc"] != null)
                {
                    home_bal Obj_HomeBAL = new home_bal();
                    Obj_HomeBAL.CabName = Session["VSCab"].ToString();
                    Obj_HomeBAL.DrwName = Session["VSDrw"].ToString();
                    Obj_HomeBAL.FldName = Session["VSFld"].ToString();
                    Obj_HomeBAL.DocName = Session["VSDoc"].ToString();

                    DataSet ds1 = new DataSet();
                    ds1 = Obj_HomeBAL.SelectDocDtl();
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        ViewState["SelDocID"] = Convert.ToInt64(ds1.Tables[0].Rows[0][0]);
                        PopGVGroup();
                        lblDocName.Text = Session["VSDocName"].ToString();
                    }
                    else
                    {
                        MessageBox("Please select a document");
                    }
                }
                else
                {
                    MessageBox("Please select a document");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdGroup_Click(object sender, EventArgs e)
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
                home_bal Obj_HmBAL = new home_bal();
                Obj_HmBAL.DocID = Convert.ToInt64(ViewState["SelDocID"]);
                Obj_HmBAL.GrpID = Convert.ToInt64(hfSelGrpID.Value);
                Obj_HmBAL.UserID = Session["UserID"].ToString();
                string result = Obj_HmBAL.funcGroup();
                if (Convert.ToInt32(result) > 0)
                {
                    Response.Redirect("home.aspx", true);
                    MessageBox("This document has been grouped");
                }
                else if (Convert.ToInt32(result) == -111)
                {
                    MsgNodet.Text ="This document is already in this group";
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvGroup_SelectedIndexChanged(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //Checking whether the Row is Data Row
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Finding the Dropdown control.
                    Control ctrl = e.Row.FindControl("lbGrpid");
                    if (ctrl != null)
                    {
                        Label dd = ctrl as Label;
                        LinkButton lnkGrp = (LinkButton)e.Row.FindControl("lnkGrp");
                        lnkGrp.OnClientClick = string.Format("msgDisp('{0}');", dd.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopGVGroup()
        {
            try
            {
                DBClass DBObj = new DBClass();
                DataSet ds01 = new DataSet();
                ds01 = DBObj.GVGroup();
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    gvGroup.DataSource = ds01;
                    gvGroup.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdUpload_Click(object sender, EventArgs e)
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
                if (Session["VSFld"] != null && Session["VSFld"].ToString() != "")
                {
                    Response.Redirect("doc_mast.aspx?UC=" + Session["VSCab"].ToString() + "&UD=" + Session["VSDrw"].ToString() + "&UF=" + Session["VSFld"].ToString(), true);
                }
                else
                {
                    throw new Exception("Please select a Folder where you want to upload the Document");
                }
            }
            catch (Exception ex)
            {
                hfMsg.Value = ex.Message;
            }
        }

        protected void cmdWorkflow_Click(object sender, EventArgs e)
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
                if (Session["VSCab"] != null && Session["VSDrw"] != null && Session["VSFld"] != null && Session["VSDoc"] != null)
                {
                    home_bal Obj_HomeBAL = new home_bal();
                    Obj_HomeBAL.CabName = Session["VSCab"].ToString();
                    Obj_HomeBAL.DrwName = Session["VSDrw"].ToString();
                    Obj_HomeBAL.FldName = Session["VSFld"].ToString();
                    Obj_HomeBAL.DocName = Session["VSDoc"].ToString();

                    DataSet ds1 = new DataSet();
                    ds1 = Obj_HomeBAL.SelectDocDtl();
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0][2].ToString() == "Check Out")
                        {
                            MessageBox("This Document is Checked Out by " + hfCheckedOutByFullName.Value);
                        }
                        else
                        {
                            Session["DocId"] = ds1.Tables[0].Rows[0][0].ToString();
                            Session["WFDocUUId"]=ds1.Tables[0].Rows[0][4].ToString();
                            Response.Redirect("start_workflow.aspx", true);
                        }
                    }
                    else
                    {
                        MessageBox("Please select a document");
                    }
                }
                else
                {
                    MessageBox("Please select a document");
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

        private void MsgIndicator(string msg)
        {
            Label lbl1 = new Label();
            lbl1.Text = "<script language='javascript'>" + Environment.NewLine + "ModalPopups.Indicator(\"idIndicator1\", \"Please wait 3 seconds\", '" + msg + "' +\", {width:300, height:100}); setTimeout('ModalPopups.Close(\"idIndicator1\");', 3000);</script>";
            Page.Controls.Add(lbl1);
        }
        
        /// <summary>
        /// This Webservice includes the following methods:
        /// changePassword, createUsers, deleteUsers, getUser, updateUsers
        /// </summary>
        protected void GenUserDtl()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.UserInfoPassingEmailID(Session["UserEmail"].ToString());
                lblUserName.Text = ds01.Tables[0].Rows[0][1].ToString() + " " + ds01.Tables[0].Rows[0][2].ToString();
                lblUserTitle.Text = ds01.Tables[0].Rows[0][5].ToString();
                lblUserDept.Text = ds01.Tables[0].Rows[0][13].ToString();
            }
            catch (Exception ex)
            {

            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("search_list.aspx?DocType=" + ddDocType.SelectedValue + "&DocName=" + txtDocName.Text.Trim(),true);
        }
        
        /// <summary>
        /// To populate the cabinet dropdown (for copy)
        /// </summary>
        protected void PopulateCabinet1()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
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
        /// As Drawer dropdown is dependent of Cabinet, so the Drawer dropdown is populated with respect to Cabinet (for copy)
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
        /// As Folder dropdown is dependent of Drawer, so the Folder dropdown is populated with respect to Drawer (for copy)
        /// </summary>
        /// <param name="SelDrw"></param>
        protected void PopulateFolder1(string SelDrwID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrwID, Session["UserID"].ToString());
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

        /// <summary>
        /// To populate the cabinet dropdown (for move)
        /// </summary>
        protected void PopulateCabinet2()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinet2.DataSource = ds01;
                ddCabinet2.DataTextField = "cab_name";
                ddCabinet2.DataValueField = "cab_uuid";
                ddCabinet2.DataBind();
                PopulateDrawer2(ddCabinet2.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// As Drawer dropdown is dependent of Cabinet, so the Drawer dropdown is populated with respect to Cabinet (for move)
        /// </summary>
        /// <param name="SelCab"></param>
        protected void PopulateDrawer2(string SelCabID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(SelCabID, Session["UserID"].ToString());
                ddDrawer2.DataSource = ds01;
                ddDrawer2.DataTextField = "drw_name";
                ddDrawer2.DataValueField = "drw_uuid";
                ddDrawer2.DataBind();
                PopulateFolder2(ddDrawer2.SelectedValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// As Folder dropdown is dependent of Drawer, so the Folder dropdown is populated with respect to Drawer (for move)
        /// </summary>
        /// <param name="SelDrw"></param>
        protected void PopulateFolder2(string SelDrwID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(SelDrwID, Session["UserID"].ToString());
                ddFolder2.DataSource = ds01;
                ddFolder2.DataTextField = "fld_name";
                ddFolder2.DataValueField = "fld_uuid";
                ddFolder2.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        protected void ddCabinet2_SelectedIndexChanged(object sender, EventArgs e)
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
                PopulateDrawer2(ddCabinet2.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawer2_SelectedIndexChanged(object sender, EventArgs e)
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
                PopulateFolder2(ddDrawer2.SelectedValue);
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
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                string NewDocName = "";
                string OnlyName = ObjFetchOnlyNameORExtension.FetchOnlyDocName(DocName);
                string OnlyExt = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(DocName);
                NewDocName = OnlyName + "_" + GetTimestamp(DateTime.Now) + "." + OnlyExt;
                return NewDocName;
            }
            catch (Exception ex)
            {
                return "Error V";
            }
        }

        private String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        private void Redirect2Home(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"home.aspx\"</script>";

            Page.Controls.Add(lbl);
        }

        protected void cmdDel_Click(object sender, EventArgs e)
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
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                string TargetFldUUID = "";
                string MoveDocName = "";
                string UUID = "";
                DataSet ds003 = new DataSet();
                UserRights ObjRights = new UserRights();
                DataSet ds0001 = new DataSet();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();

                if (Session["VSDoc"] == null || Session["VSDoc"].ToString() == "")
                {
                    if (Session["VSFld"] == null || Session["VSFld"].ToString() == "")
                    {
                        if (Session["VSDrw"] == null || Session["VSDrw"].ToString() == "")
                        {
                            if (Session["VSCab"] == null || Session["VSCab"].ToString() == "")
                            {
                                throw new Exception("Please Select Cabinet / Drawer / Folder / Document which one you want to Delete!!");
                            }
                            else
                            {
                                #region For deletion of Cabinets
                                ds0001 = ObjRights.FetchPermissions(Session["VSCab"].ToString(), Session["UserID"].ToString());
                                if (ds0001 == null)
                                {
                                    Utility.CloseConnection(con);
                                    throw new Exception("You have no Rights to Delete this Cabinet!!");
                                }
                                else
                                {
                                    if (ds0001.Tables[0].Rows[0][1].ToString() == "D")
                                    {
                                        // Alfresco Part Start
                                        DataSet ds01 = new DataSet();
                                        DataSet ds02 = new DataSet();

                                        cmd = new SqlCommand("select * from drawer_mast where cab_uuid='" + Session["VSCab"].ToString() + "'", con);
                                        SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                                        adapter02.Fill(ds02);
                                        if (ds02.Tables[0].Rows.Count > 0)
                                        {
                                            Utility.CloseConnection(con);
                                            throw new Exception("At first Delete the Drawers from this Cabinet!!");
                                        }
                                        // Initialise the reference to the spaces store
                                        this.spacesStore = new Alfresco.RepositoryWebService.Store();
                                        this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                                        this.spacesStore.address = "SpacesStore";
                                        //create a predicate with the first CMLCreate result
                                        Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                                        referenceForNode.store = this.spacesStore;
                                        referenceForNode.uuid = Session["VSCab"].ToString(); // Selected Doc's / Folder's UUID

                                        Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                                        Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                                        sourcePredicate.Items = obj_new;

                                        //delete content
                                        CMLDelete delete = new CMLDelete();
                                        delete.where = sourcePredicate;

                                        CML cmlRemove = new CML();
                                        cmlRemove.delete = new CMLDelete[] { delete };

                                        //perform a CML update to delete the node
                                        WebServiceFactory wsF = new WebServiceFactory();
                                        wsF.UserName = Session["AdmUserID"].ToString();
                                        wsF.Ticket = Session["AdmTicket"].ToString();
                                        wsF.getRepositoryService().update(cmlRemove);
                                        // Alfresco Part End

                                        cabinet_mast_bal OBJ_CabinetBAL = new cabinet_mast_bal();
                                        OBJ_CabinetBAL.CabinetCode = Session["VSCab"].ToString();

                                        string result = OBJ_CabinetBAL.DeleteCabinet();
                                        if (result == null || result == "")
                                        {
                                            Utility.CloseConnection(con);
                                            throw new Exception("Error in Data Deletion!!");
                                        }
                                        else
                                        {
                                            // Delete the UUID related info from all the tables
                                            cmd = new SqlCommand("delete from UserRights where NodeUUID='" + Session["VSCab"].ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            Utility.CloseConnection(con);
                                            throw new Exception("The Selected Cabinet has been Deleted Successfully!!");
                                        }
                                    }
                                    else
                                    {
                                        Utility.CloseConnection(con);
                                        throw new Exception("You have no Rights to Delete this Cabinet!!");
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            #region For deletion of Drawers
                            ds0001 = ObjRights.FetchPermissions(Session["VSDrw"].ToString(), Session["UserID"].ToString());
                            if (ds0001 == null)
                            {
                                Utility.CloseConnection(con);
                                throw new Exception("You have no Rights to Delete this Drawer!!");
                            }
                            else
                            {
                                if (ds0001.Tables[0].Rows[0][1].ToString() == "D")
                                {
                                    // Alfresco Part Start
                                    DataSet ds01 = new DataSet();
                                    DataSet ds02 = new DataSet();

                                    ds02.Reset();
                                    ds02 = ObjClassStoreProc.AllFoldersInsideDrawer(Session["VSDrw"].ToString());
                                    if (ds02.Tables[0].Rows.Count > 0)
                                    {
                                        Utility.CloseConnection(con);
                                        throw new Exception("At first Delete the Folders from this Drawer!!");
                                    }
                                    // Initialise the reference to the spaces store
                                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                                    this.spacesStore.address = "SpacesStore";
                                    //create a predicate with the first CMLCreate result
                                    Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                                    referenceForNode.store = this.spacesStore;
                                    referenceForNode.uuid = Session["VSDrw"].ToString(); // Selected Doc's / Folder's UUID

                                    Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                                    Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                                    sourcePredicate.Items = obj_new;

                                    //delete content
                                    CMLDelete delete = new CMLDelete();
                                    delete.where = sourcePredicate;

                                    CML cmlRemove = new CML();
                                    cmlRemove.delete = new CMLDelete[] { delete };

                                    //perform a CML update to delete the node
                                    WebServiceFactory wsF = new WebServiceFactory();
                                    wsF.UserName = Session["AdmUserID"].ToString();
                                    wsF.Ticket = Session["AdmTicket"].ToString();
                                    wsF.getRepositoryService().update(cmlRemove);
                                    // Alfresco Part End

                                    drawer_mast_bal OBJ_DrawerBAL = new drawer_mast_bal();
                                    OBJ_DrawerBAL.Labelid = Session["VSDrw"].ToString();

                                    string result = OBJ_DrawerBAL.DeleteDrawer();
                                    if (Convert.ToInt32(result) > 0)
                                    {
                                        // Delete the UUID related info from all the tables
                                        cmd = new SqlCommand("delete from UserRights where NodeUUID='" + Session["VSDrw"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        Utility.CloseConnection(con);
                                        throw new Exception("The Selected Drawer has been Deleted Successfully!!");
                                    }
                                    else
                                    {
                                        Utility.CloseConnection(con);
                                        throw new Exception("Error in Data Deletion!!");
                                    }
                                }
                                else
                                {
                                    Utility.CloseConnection(con);
                                    throw new Exception("You have no Rights to Delete this Drawer!!");
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region For deletion of Folders
                        con.Open();
                        ds0001 = ObjRights.FetchPermissions(Session["VSFld"].ToString(), Session["UserID"].ToString());
                        if (ds0001 == null)
                        {
                            throw new Exception("You have no Rights to Delete this Folder!!");
                        }
                        else
                        {
                            if (ds0001.Tables[0].Rows[0][1].ToString() == "D")
                            {
                                // Alfresco Part Start
                                DataSet ds01 = new DataSet();
                                DataSet ds02 = new DataSet();

                                ds02.Reset();
                                ds02 = ObjClassStoreProc.AllDocumentsInsideFolder(Session["VSFld"].ToString());
                                if (ds02.Tables[0].Rows.Count > 0)
                                {
                                    throw new Exception("At first Delete the Documents from this Folder!!");
                                }
                                else
                                {
                                    DataSet ds03 = new DataSet();
                                    ds03.Reset();
                                    ds03 = ObjClassStoreProc.FolderNamePassingFolderUUID(Session["VSFld"].ToString());
                                    if (ds03.Tables[0].Rows.Count > 0)
                                    {
                                        if (ds03.Tables[0].Rows[0][0].ToString().ToUpper() == "TRASH" || ds03.Tables[0].Rows[0][0].ToString().ToUpper() == "STORAGE")
                                        {
                                            throw new Exception("This Folder can not be Deleted!!");
                                        }
                                    }
                                }
                                // Initialise the reference to the spaces store
                                this.spacesStore = new Alfresco.RepositoryWebService.Store();
                                this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                                this.spacesStore.address = "SpacesStore";
                                //create a predicate with the first CMLCreate result
                                Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                                referenceForNode.store = this.spacesStore;
                                referenceForNode.uuid = Session["VSFld"].ToString(); // Selected Doc's / Folder's UUID

                                Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                                Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                                sourcePredicate.Items = obj_new;

                                //delete content
                                CMLDelete delete = new CMLDelete();
                                delete.where = sourcePredicate;

                                CML cmlRemove = new CML();
                                cmlRemove.delete = new CMLDelete[] { delete };

                                //perform a CML update to delete the node
                                WebServiceFactory wsF = new WebServiceFactory();
                                wsF.UserName = Session["AdmUserID"].ToString();
                                wsF.Ticket = Session["AdmTicket"].ToString();
                                wsF.getRepositoryService().update(cmlRemove);
                                // Alfresco Part End

                                folder_mast_bal OBJ_FolderBAL = new folder_mast_bal();
                                OBJ_FolderBAL.Labelid = Session["VSFld"].ToString();

                                string result = OBJ_FolderBAL.DeleteFolder();
                                if (Convert.ToInt32(result) > 0)
                                {
                                    // Delete the UUID related info from all the tables
                                    cmd = new SqlCommand("delete from UserRights where NodeUUID='" + Session["VSFld"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    Utility.CloseConnection(con);
                                    throw new Exception("The Selected Folder has been Deleted Successfully!!");
                                }
                                else
                                {
                                    throw new Exception("You have no Rights to Delete this Folder!!");
                                }
                            }
                            else
                            {
                                throw new Exception("You have no Rights to Delete this Folder!!");
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    #region For deletion of Documents
                    DBClass DBObj = new DBClass();
                    DataSet ds01 = new DataSet();
                    cmd = new SqlCommand("select * from wf_log_mast where doc_id='" + Session["VSDoc"].ToString() + "'", con);
                    SqlDataAdapter adapter_003 = new SqlDataAdapter(cmd);
                    ds003.Reset();
                    adapter_003.Fill(ds003);
                    if (ds003.Tables[0].Rows.Count > 0)
                    {
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                        gvDocument.DataSource = ds01;
                        gvDocument.DataBind();
                        UpdatePanel4.Update();
                        throw new Exception("This Document is in a Workflow -- so can not be Deleted!!");
                    }
                    cmd = new SqlCommand("select uuid,doc_name,doc_stat from doc_mast where doc_id='" + Session["VSDoc"].ToString() + "'", con);
                    SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                    ds003.Reset();
                    adapter003.Fill(ds003);
                    if (ds003.Tables[0].Rows.Count > 0)
                    {
                        if (ds003.Tables[0].Rows[0][2].ToString() == "Check Out")
                        {
                            ds01.Reset();
                            ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                            gvDocument.DataSource = ds01;
                            gvDocument.DataBind();
                            UpdatePanel4.Update();
                            throw new Exception("This Document is Checked Out -- so can not be Deleted!!");
                        }
                        else
                        {
                            UUID = ds003.Tables[0].Rows[0][0].ToString();
                            MoveDocName = ds003.Tables[0].Rows[0][1].ToString();
                        }
                    }

                    
                    con.Open();
                    ds0001 = ObjRights.FetchPermissions(UUID, Session["UserID"].ToString());
                    if (ds0001 == null)
                    {
                        Utility.CloseConnection(con);
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                        gvDocument.DataSource = ds01;
                        gvDocument.DataBind();
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            gvDocument.SelectedIndex = -1;
                        }
                        UpdatePanel4.Update();
                        Session["VSDoc"] = "";
                        throw new Exception("You have no Rights to Delete this Document!!");
                    }
                    else
                    {
                        if (ds0001.Tables[0].Rows[0][1].ToString() == "D")
                        {
                            DataSet ds02 = new DataSet();
                            DataSet ds_02 = new DataSet();
                            Int32 result = 0;

                            cmd = new SqlCommand("select fld_name from folder_mast where fld_uuid in(select fld_uuid from doc_mast where uuid='" + UUID + "')", con);
                            SqlDataAdapter adapter_02 = new SqlDataAdapter(cmd);
                            adapter_02.Fill(ds_02);
                            if (ds_02.Tables[0].Rows.Count > 0)
                            {
                                this.spacesStore = new Alfresco.RepositoryWebService.Store();
                                this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                                this.spacesStore.address = "SpacesStore";
                                //create a predicate with the first CMLCreate result
                                Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                                referenceForNode.store = this.spacesStore;
                                referenceForNode.uuid = UUID; // Selected Doc's / Folder's UUID

                                Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                                Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                                sourcePredicate.Items = obj_new;

                                if (ds_02.Tables[0].Rows[0][0].ToString() == "TRASH")
                                {
                                    #region For Permanent deletion start
                                    CMLDelete delete = new CMLDelete();
                                    delete.where = sourcePredicate;

                                    CML cmlRemove = new CML();
                                    cmlRemove.delete = new CMLDelete[] { delete };

                                    WebServiceFactory wsF1 = new WebServiceFactory();
                                    wsF1.UserName = Session["AdmUserID"].ToString();
                                    wsF1.Ticket = Session["AdmTicket"].ToString();
                                    wsF1.getRepositoryService().update(cmlRemove);

                                    double FileSize = 0;
                                    cmd = new SqlCommand("select * from doc_mast where uuid='" + UUID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    SqlDataAdapter adapter_0001 = new SqlDataAdapter(cmd);
                                    DataSet ds_0001 = new DataSet();
                                    adapter_0001.Fill(ds_0001);
                                    if (ds_0001.Tables[0].Rows.Count > 0)
                                    {
                                        FileSize = Convert.ToDouble(ds_0001.Tables[0].Rows[0][24].ToString());
                                    }
                                    cmd = new SqlCommand("delete from doc_mast where uuid='" + UUID + "' and DocUpldType='D'", con);
                                    result = cmd.ExecuteNonQuery();
                                    cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace-'" + FileSize + "' where CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();

                                    if (result > 0)
                                    {
                                        Utility.CloseConnection(con);
                                        ds01.Reset();
                                        ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                                        gvDocument.DataSource = ds01;
                                        gvDocument.DataBind();
                                        if (ds01.Tables[0].Rows.Count > 0)
                                        {
                                            gvDocument.SelectedIndex = -1;
                                        }
                                        UpdatePanel4.Update();
                                        Session["VSDoc"] = "";
                                        throw new Exception("The Selected Document has been Deleted Successfully!!");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Move to TRASH Folder
                                    cmd = new SqlCommand("select fld_uuid from folder_mast where fld_name='TRASH' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                                    adapter02.Fill(ds02);
                                    if (ds02.Tables[0].Rows.Count > 0)
                                    {
                                        TargetFldUUID = ds02.Tables[0].Rows[0][0].ToString();
                                    }
                                    //create a reference from the second CMLCreate performed for space
                                    Alfresco.RepositoryWebService.Reference referenceForTargetSpace = new Alfresco.RepositoryWebService.Reference();
                                    referenceForTargetSpace.store = this.spacesStore;
                                    referenceForTargetSpace.uuid = TargetFldUUID; // Selected folder's UUID

                                    //reference for the target space
                                    Alfresco.RepositoryWebService.ParentReference targetSpace = new Alfresco.RepositoryWebService.ParentReference();
                                    targetSpace.store = this.spacesStore;
                                    targetSpace.uuid = referenceForTargetSpace.uuid;
                                    targetSpace.associationType = Constants.ASSOC_CONTAINS;
                                    targetSpace.childName = GenNewDocName(MoveDocName); // Selected Doc's Name

                                    cmd = new SqlCommand("update doc_mast set doc_name='" + targetSpace.childName + "', uuid='" + UUID + "',fld_uuid='" + TargetFldUUID + "',DocUpldType='D' where doc_id='" + Session["VSDoc"].ToString() + "'", con);
                                    result = cmd.ExecuteNonQuery();

                                    //move content
                                    CMLMove move = new CMLMove();
                                    move.where = sourcePredicate;
                                    move.to = targetSpace;

                                    CML cmlMove = new CML();
                                    cmlMove.move = new CMLMove[] { move };

                                    WebServiceFactory wsF = new WebServiceFactory();
                                    wsF.UserName = Session["AdmUserID"].ToString();
                                    wsF.Ticket = Session["AdmTicket"].ToString();
                                    wsF.getRepositoryService().update(cmlMove);
                                    // Alfresco Part End

                                    folder_mast_bal OBJ_FolderBAL = new folder_mast_bal();
                                    OBJ_FolderBAL.Labelid = UUID;

                                    if (result > 0)
                                    {
                                        Utility.CloseConnection(con);
                                        ds01.Reset();
                                        ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                                        gvDocument.DataSource = ds01;
                                        gvDocument.DataBind();
                                        if (ds01.Tables[0].Rows.Count > 0)
                                        {
                                            gvDocument.SelectedIndex = -1;
                                        }
                                        UpdatePanel4.Update();
                                        Session["VSDoc"] = "";
                                        throw new Exception("The Selected Document has been Deleted Successfully!!");
                                    }
                                    else
                                    {
                                        Utility.CloseConnection(con);
                                        ds01.Reset();
                                        ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                                        gvDocument.DataSource = ds01;
                                        gvDocument.DataBind();
                                        if (ds01.Tables[0].Rows.Count > 0)
                                        {
                                            gvDocument.SelectedIndex = -1;
                                        }
                                        UpdatePanel4.Update();
                                        Session["VSDoc"] = "";
                                        throw new Exception("You have no Rights to Delete this Document!!");
                                    }
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            Utility.CloseConnection(con);
                            ds01.Reset();
                            ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(Session["VSFld"].ToString(), Session["UserID"].ToString());
                            gvDocument.DataSource = ds01;
                            gvDocument.DataBind();
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                gvDocument.SelectedIndex = -1;
                            }
                            UpdatePanel4.Update();
                            Session["VSDoc"] = "";
                            throw new Exception("You have no Rights to Delete this Document!!");
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "")
                {
                    Response.Redirect("home.aspx", false);
                }
                else
                {
                    MessageBox(ex.Message);
                }
            }
        }

    }
}