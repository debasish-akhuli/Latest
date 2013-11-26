using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using Alfresco;
using System.Net;
using System.IO;
using QuickPDFDLL0813;

namespace DMS
{
    public partial class search_list : System.Web.UI.Page
    {
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");

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
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divCompany.Visible = true;
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divCompany.Visible = false;
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divCompany.Visible = false;
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = true;
                        }
                        Page.Header.DataBind();
                        PopCompany();
                        PopulateDropdown();
                        if (Server.UrlDecode(Request.QueryString["DocType"]) != null)
                        {
                            ViewState["SDocType"] = Server.UrlDecode(Request.QueryString["DocType"]);
                            ViewState["SDocName"] = Server.UrlDecode(Request.QueryString["DocName"]);
                            /// Populate the Searched List GridView
                            PopSearchedList("Quick");
                        }
                        else
                        {
                            /// Populate the Searched List GridView
                            //PopSearchedList("Advanced");
                        }
                        if (ddDocType.SelectedItem.Text == "")
                        {
                            TagDisplay("");
                        }
                        else
                        {
                            TagDisplay(ddDocType.SelectedValue);
                        }
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
                            if (Session["UserType"].ToString() == "S") // Super Admin
                            {
                                ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(hfSelDocID.Value), ddCompany.SelectedValue);
                            }
                            else
                            {
                                ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(hfSelDocID.Value), Session["CompCode"].ToString());
                            }
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                Session["OpenDocName"] = ds01.Tables[0].Rows[0][1].ToString();
                                Session["OpenDocUUID"] = ds01.Tables[0].Rows[0][4].ToString();
                                string FormType = "";
                                FormType = ds01.Tables[0].Rows[0][27].ToString();
                                if (FormType == "eForm")
                                {
                                    string DocExtension = "";
                                    DocExtension = ds01.Tables[0].Rows[0][1].ToString().Substring(ds01.Tables[0].Rows[0][1].ToString().Length - 4, 4);
                                    if (DocExtension == ".pdf")
                                    {
                                        ViewDoc(Session["OpenDocUUID"].ToString());
                                    }
                                    else
                                    {
                                        Response.Redirect("eFormOpening.aspx?DocID=" + hfSelDocID.Value, false);
                                    }
                                }
                                else
                                {
                                    ViewDoc(Session["OpenDocUUID"].ToString());
                                }
                            }
                        }
                    }
                }
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
            }
            catch (Exception ex)
            {

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
                ddDocType.Items.Insert(0,"");
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

        protected void ddCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDropdown();
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
                if (Session["OpenDocName"].ToString().LastIndexOf(".pdf") == -1)
                {
                    //newSaveFileName = Session["OpenDocName"].ToString() + "_" + Session["UserID"].ToString() + ".pdf";
                    DownloadFile();
                }
                else
                {
                    newSaveFileName = Session["OpenDocName"].ToString().Substring(0, Session["OpenDocName"].ToString().Length - 4) + "_" + Session["UserID"].ToString() + ".pdf";

                    string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
                    SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                    ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                    QP.LoadFromFile(file_name, "");
                    // Count the total number of form fields in the file
                    int TotalFormFields = QP.FormFieldCount();
                    // delete them using the DeleteFormField function
                    for (int i = TotalFormFields; i > TotalFormFields - 1; i--)
                    {
                        QP.DeleteFormField(i);
                    }
                    QP.SaveToFile(file_name);
                    //Download end
                    Session["OpenDocName"] = null;
                    Session["OpenDocUUID"] = null;
                    Session["hfPageControl"] = "O";
                    Response.Redirect("FormFillup.aspx?docname=" + newSaveFileName + "&DocUUID=" + DocUUID, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void DownloadFile()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(hfSelDocID.Value), ddCompany.SelectedValue);
                }
                else
                {
                    ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(hfSelDocID.Value), Session["CompCode"].ToString());
                }
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    DownldDoc(ds01.Tables[0].Rows[0][4].ToString(), ds01.Tables[0].Rows[0][1].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void DownldDoc(string DocUUID, string DocName)
        {
            try
            {
                DocName = DocName.Replace(" ", "_");
                // At first download the selected file from alfresco and save it to <TempDownload> Folder
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
                // Now download the file to local drive
                FileInfo file = new FileInfo(file_name);
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                FileType ObjFileType = new FileType();
                Response.ContentType = ObjFileType.GetFileType(file.Extension.ToLower());
                Response.TransmitFile(file.FullName);
                Response.End();
                //File.Delete(file_name);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the searched list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopSearchedList(string SType)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                string Str = "";

                if (SType == "Quick")
                {
                    //cmd = new SqlCommand("select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e where a.doc_type_id=b.doc_type_id and a.fld_uuid=c.fld_uuid and c.drw_uuid=d.drw_uuid and d.cab_uuid=e.cab_uuid and a.doc_id in(select a.doc_id from doc_mast a,UserRights b where a.uuid=b.NodeUUID and b.Permission!='X' and b.UserID='" + Session["UserID"].ToString() + "') and b.doc_type_id='" + ViewState["SDocType"].ToString() + "' and a.doc_name like('%" + ViewState["SDocName"].ToString() + "%')", con);
                    //cmd = new SqlCommand("select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and a.doc_id in(select a.doc_id from doc_mast a,UserRights b where a.uuid=b.NodeUUID and b.Permission!='X' and b.UserID='" + Session["UserID"].ToString() + "') and b.doc_type_id='" + ViewState["SDocType"].ToString() + "' and a.doc_name like('%" + ViewState["SDocName"].ToString() + "%') and a.CompCode='" + Session["CompCode"].ToString() + "'", con);
                    if (Session["UserType"].ToString() == "S") // Super Admin
                    {
                        cmd = new SqlCommand("select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name,convert(varchar, a.upld_dt, 23) as upld_dt from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and b.doc_type_id='" + ViewState["SDocType"].ToString() + "' and a.doc_name like('%" + ViewState["SDocName"].ToString() + "%') and a.CompCode='" + ddCompany.SelectedValue + "' order by a.upld_dt desc,a.doc_name", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name,convert(varchar, a.upld_dt, 23) as upld_dt from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e,UserRights f where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and a.uuid=f.NodeUUID and a.CompCode=f.CompCode and f.Permission!='X' and f.UserID='" + Session["UserID"].ToString() + "' and b.doc_type_id='" + ViewState["SDocType"].ToString() + "' and a.doc_name like('%" + ViewState["SDocName"].ToString() + "%') and a.CompCode='" + Session["CompCode"].ToString() + "' order by a.upld_dt desc,a.doc_name", con);
                    }
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    gvSearchedList.DataSource = ds;
                    gvSearchedList.DataBind();
                }
                else
                {
                    if (ddDocType.SelectedItem.Text == "")
                    {
                        //Str = "select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and a.doc_id in(select a.doc_id from doc_mast a,UserRights b where a.uuid=b.NodeUUID and b.Permission!='X' and b.UserID='" + Session["UserID"].ToString() + "') and a.doc_name like('%" + txtDocName.Text.Trim() + "%') and a.CompCode='" + Session["CompCode"].ToString() + "' and a.tag1 like('%" + txtTag1.Text.Trim() + "%') and a.tag2 like('%" + txtTag2.Text.Trim() + "%') and a.tag3 like('%" + txtTag3.Text.Trim() + "%') and a.tag4 like('%" + txtTag4.Text.Trim() + "%') and a.tag5 like('%" + txtTag5.Text.Trim() + "%') and a.tag6 like('%" + txtTag6.Text.Trim() + "%') and a.tag7 like('%" + txtTag7.Text.Trim() + "%') and a.tag8 like('%" + txtTag8.Text.Trim() + "%') and a.tag9 like('%" + txtTag9.Text.Trim() + "%') and a.tag10 like('%" + txtTag10.Text.Trim() + "%')";
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            Str = "select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name,convert(varchar, a.upld_dt, 23) as upld_dt from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and a.doc_name like('%" + txtDocName.Text.Trim() + "%') and a.CompCode='" + ddCompany.SelectedValue + "' and a.tag1 like('%" + txtTag1.Text.Trim() + "%') and a.tag2 like('%" + txtTag2.Text.Trim() + "%') and a.tag3 like('%" + txtTag3.Text.Trim() + "%') and a.tag4 like('%" + txtTag4.Text.Trim() + "%') and a.tag5 like('%" + txtTag5.Text.Trim() + "%') and a.tag6 like('%" + txtTag6.Text.Trim() + "%') and a.tag7 like('%" + txtTag7.Text.Trim() + "%') and a.tag8 like('%" + txtTag8.Text.Trim() + "%') and a.tag9 like('%" + txtTag9.Text.Trim() + "%') and a.tag10 like('%" + txtTag10.Text.Trim() + "%') order by a.upld_dt desc,a.doc_name";
                        }
                        else
                        {
                            Str = "select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name,convert(varchar, a.upld_dt, 23) as upld_dt from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e,UserRights f where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and a.uuid=f.NodeUUID and a.CompCode=f.CompCode and f.Permission!='X' and f.UserID='" + Session["UserID"].ToString() + "' and a.doc_name like('%" + txtDocName.Text.Trim() + "%') and a.CompCode='" + Session["CompCode"].ToString() + "' and a.tag1 like('%" + txtTag1.Text.Trim() + "%') and a.tag2 like('%" + txtTag2.Text.Trim() + "%') and a.tag3 like('%" + txtTag3.Text.Trim() + "%') and a.tag4 like('%" + txtTag4.Text.Trim() + "%') and a.tag5 like('%" + txtTag5.Text.Trim() + "%') and a.tag6 like('%" + txtTag6.Text.Trim() + "%') and a.tag7 like('%" + txtTag7.Text.Trim() + "%') and a.tag8 like('%" + txtTag8.Text.Trim() + "%') and a.tag9 like('%" + txtTag9.Text.Trim() + "%') and a.tag10 like('%" + txtTag10.Text.Trim() + "%') order by a.upld_dt desc,a.doc_name";
                        }
                    }
                    else
                    {
                        //Str = "select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and a.doc_id in(select a.doc_id from doc_mast a,UserRights b where a.uuid=b.NodeUUID and b.Permission!='X' and b.UserID='" + Session["UserID"].ToString() + "') and b.doc_type_id='" + ddDocType.SelectedValue + "' and a.doc_name like('%" + txtDocName.Text.Trim() + "%') and a.CompCode='" + Session["CompCode"].ToString() + "' and a.tag1 like('%" + txtTag1.Text.Trim() + "%') and a.tag2 like('%" + txtTag2.Text.Trim() + "%') and a.tag3 like('%" + txtTag3.Text.Trim() + "%') and a.tag4 like('%" + txtTag4.Text.Trim() + "%') and a.tag5 like('%" + txtTag5.Text.Trim() + "%') and a.tag6 like('%" + txtTag6.Text.Trim() + "%') and a.tag7 like('%" + txtTag7.Text.Trim() + "%') and a.tag8 like('%" + txtTag8.Text.Trim() + "%') and a.tag9 like('%" + txtTag9.Text.Trim() + "%') and a.tag10 like('%" + txtTag10.Text.Trim() + "%')";
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            Str = "select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name,convert(varchar, a.upld_dt, 23) as upld_dt from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and b.doc_type_id='" + ddDocType.SelectedValue + "' and a.doc_name like('%" + txtDocName.Text.Trim() + "%') and a.CompCode='" + ddCompany.SelectedValue + "' and a.tag1 like('%" + txtTag1.Text.Trim() + "%') and a.tag2 like('%" + txtTag2.Text.Trim() + "%') and a.tag3 like('%" + txtTag3.Text.Trim() + "%') and a.tag4 like('%" + txtTag4.Text.Trim() + "%') and a.tag5 like('%" + txtTag5.Text.Trim() + "%') and a.tag6 like('%" + txtTag6.Text.Trim() + "%') and a.tag7 like('%" + txtTag7.Text.Trim() + "%') and a.tag8 like('%" + txtTag8.Text.Trim() + "%') and a.tag9 like('%" + txtTag9.Text.Trim() + "%') and a.tag10 like('%" + txtTag10.Text.Trim() + "%') order by a.upld_dt desc,a.doc_name";
                        }
                        else
                        {
                            Str = "select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name,convert(varchar, a.upld_dt, 23) as upld_dt from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e,UserRights f where a.doc_type_id=b.doc_type_id and a.CompCode=b.CompCode and a.fld_uuid=c.fld_uuid and a.CompCode=c.CompCode and c.drw_uuid=d.drw_uuid and a.CompCode=d.CompCode and d.cab_uuid=e.cab_uuid and a.CompCode=e.CompCode and a.uuid=f.NodeUUID and a.CompCode=f.CompCode and f.Permission!='X' and f.UserID='" + Session["UserID"].ToString() + "' and b.doc_type_id='" + ddDocType.SelectedValue + "' and a.doc_name like('%" + txtDocName.Text.Trim() + "%') and a.CompCode='" + Session["CompCode"].ToString() + "' and a.tag1 like('%" + txtTag1.Text.Trim() + "%') and a.tag2 like('%" + txtTag2.Text.Trim() + "%') and a.tag3 like('%" + txtTag3.Text.Trim() + "%') and a.tag4 like('%" + txtTag4.Text.Trim() + "%') and a.tag5 like('%" + txtTag5.Text.Trim() + "%') and a.tag6 like('%" + txtTag6.Text.Trim() + "%') and a.tag7 like('%" + txtTag7.Text.Trim() + "%') and a.tag8 like('%" + txtTag8.Text.Trim() + "%') and a.tag9 like('%" + txtTag9.Text.Trim() + "%') and a.tag10 like('%" + txtTag10.Text.Trim() + "%') order by a.upld_dt desc,a.doc_name";
                        }
                    }
                    cmd = new SqlCommand(Str, con);
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    gvSearchedList.DataSource = ds;
                    gvSearchedList.DataBind();
                }
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Pass to BAL to set the corresponding Tags with respect to Doc Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TagDisplay(ddDocType.SelectedValue);
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
                if (SelItem == "")
                {
                    txtTag1.Text = "";
                    divTag1.Visible = false;
                    txtTag2.Text = "";
                    divTag2.Visible = false;
                    txtTag3.Text = "";
                    divTag3.Visible = false;
                    txtTag4.Text = "";
                    divTag4.Visible = false;
                    txtTag5.Text = "";
                    divTag5.Visible = false;
                    txtTag6.Text = "";
                    divTag6.Visible = false;
                    txtTag7.Text = "";
                    divTag7.Visible = false;
                    txtTag8.Text = "";
                    divTag8.Visible = false;
                    txtTag9.Text = "";
                    divTag9.Visible = false;
                    txtTag10.Text = "";
                    divTag10.Visible = false;
                }
                else
                {
                    //doc_mast_bal Obj_DocMastBAL = new doc_mast_bal();
                    //Obj_DocMastBAL.DocTypeCode = SelItem;

                    //DataSet ds1 = new DataSet();
                    //ds1 = Obj_DocMastBAL.FetchTags();

                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    ds01.Reset();
                    if (Session["UserType"].ToString() == "S") // Super Admin
                    {
                        ds01 = ObjClassStoreProc.SelectTagBasedOnDocTypeCompCode(SelItem, ddCompany.SelectedValue);
                    }
                    else
                    {
                        ds01 = ObjClassStoreProc.SelectTagBasedOnDocTypeCompCode(SelItem, Session["CompCode"].ToString());
                    }
                    
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvSearchedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                /// For Download
                GridViewRow row = gvSearchedList.SelectedRow;
                Label lbDownloadPath = (Label)row.FindControl("lbDownloadPath");
                HyperLink cmdDownload = (HyperLink)row.FindControl("cmdDownload");
                cmdDownload.NavigateUrl = lbDownloadPath.Text;

                /// For Displaying the Comments against the document
                Label lbDocID = (Label)row.FindControl("lbDocID");
                string lbDocID1 = lbDocID.Text.ToString();

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

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                string StartDocID = "";
                cmd = new SqlCommand("select doc_id from doc_mast where uuid in(select ActualDocUUID from WFDocVersion where WFLogID in(select WFLogID from WFDocVersion where NewDocUUID in(select uuid from doc_mast where doc_id='" + lbDocID1 + "')) and StepNo='1')", con);
                ds001 = new DataSet();
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    StartDocID = ds001.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    StartDocID = lbDocID1;
                }

                cmd = new SqlCommand("select wf_log_id,wf_id from wf_log_mast where doc_id='" + lbDocID1 + "'", con);
                ds01 = new DataSet();
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);

                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        WFID = Convert.ToInt64(ds01.Tables[0].Rows[i][1].ToString());
                        //cmd = new SqlCommand("select a.step_no,b.task_name,a.task_done_dt,a.comments from wf_log_task a,task_mast b where a.task_id=b.task_id and (a.task_id not like('PRE%') and a.task_id not like('POST%')) and a.wf_log_id='" + ds01.Tables[0].Rows[i][0].ToString() + "' and a.comments!='Not Required' order by a.step_no", con);
                        cmd = new SqlCommand("select a.StepNo,b.task_name,a.TaskDoneDate,a.Comments,a.UserID from WFLog a,task_mast b where a.TaskID=b.task_id and a.TaskID not like('PRE%') and a.TaskID not like('POST%') and a.TaskDoneDate!='Not Required' and a.WFLogID='" + ds01.Tables[0].Rows[i][0].ToString() + "'", con);
                        ds02 = new DataSet();
                        SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                        adapter02.Fill(ds02);

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
                                        //r["Comment"] = ds02.Tables[0].Rows[j][3].ToString();
                                        //r["Action"] = ds02.Tables[0].Rows[j][1].ToString();
                                        //cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id in(select user_id from user_role where role_id in(select role_id from wf_dtl where wf_id='" + WFID + "' and step_no='" + ds02.Tables[0].Rows[j][0].ToString() + "'))", con);
                                        if (ds02.Tables[0].Rows[j][4].ToString() == "")
                                        {
                                            #region For the User who is responsible for this stage
                                            cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id in(select user_id from user_role where role_id in(select role_id from wf_dtl where step_no='" + ds02.Tables[0].Rows[j][0].ToString() + "' and wf_id in(select wf_id from wf_log_mast where wf_log_id='" + ds01.Tables[0].Rows[i][0].ToString() + "')))", con);
                                            DataSet ds003 = new DataSet();
                                            SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                                            adapter003.Fill(ds003);
                                            r["User"] = ds003.Tables[0].Rows[0][0].ToString();
                                            #endregion
                                            //r["User"] = "";
                                        }
                                        else
                                        {
                                            cmd = new SqlCommand("select (f_name + ' ' + l_name + ' (' + user_id + ')') as name from user_mast where user_id='" + ds02.Tables[0].Rows[j][4].ToString() + "'", con);
                                            ds03 = new DataSet();
                                            SqlDataAdapter adapter03 = new SqlDataAdapter(cmd);
                                            adapter03.Fill(ds03);

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

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PopSearchedList("Advanced");                
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

    }
}