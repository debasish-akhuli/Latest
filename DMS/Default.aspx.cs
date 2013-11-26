using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Alfresco;
using DMS.UTILITY;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.IO;

namespace DMS
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set the session variables blank which are used to set the previous selected path start
                Session["SelectedCabUUID"] = "";
                Session["SelectedDrwUUID"] = "";
                Session["SelectedFldUUID"] = "";
                Session["SelectedDocID"] = "";
                // Set the session variables blank which are used to set the previous selected path end
                cmdLogin.Attributes.Add("onclick", "document.body.style.cursor = 'wait';");

                if (Request.QueryString["UC"] != null)
                {
                    if (Request.QueryString["UC"].ToString() == "6a800c19d0014966809c8838507")
                    {
                        Session["uid"] = "guest1";
                        // Fetch Guest Password Start
                        ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                        DataSet ds01 = new DataSet();
                        ds01 = ObjClassStoreProc.UserInfoPassingUserID(Session["uid"].ToString());
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            Session["pwd"] = ds01.Tables[0].Rows[0][4].ToString();
                        }
                        // Fetch Guest Password End
                        Session["URLDocUUID"] = Request.QueryString["URLDocUUID"].ToString();
                        LoginUser();
                    }
                    else
                    {
                        Session["uid"] = "";
                        Session["pwd"] = "";
                        Session["URLDocUUID"] = null;
                    }
                }
                else
                {
                    if (Request.QueryString["Source"]== "FS")
                    {
                        if (Session["UserID"] != null && Session["UserID"].ToString() != "")
                        {
                            byte[] bytes;
                            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                            bytes =encoding.GetBytes(Request.QueryString["FileData"].ToString());
                            Session["AccessControl"] = "";
                            Response.Redirect("doc_mast.aspx?Source=FS&DocName=" + Request.QueryString["DocName"] + "&FileData=" + bytes, false);
                        }
                    }
                    else
                    {
                        Session["uid"] = "";
                        Session["pwd"] = "";
                        Session["URLDocUUID"] = null;
                    }
                }
            }
        }

        protected void cmdLogin_Click(object sender, EventArgs e)
        {
            try
            {
                LoginUser();
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

        protected void LoginUser()
        {
            try
            {
                AuthenticationUtils authUtil = new AuthenticationUtils();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                DataSet ds001 = new DataSet();
                DataSet ds02 = new DataSet();
                DataSet ds03 = new DataSet();
                ds01 = ObjClassStoreProc.UserInfoPassingUserID("admin");
                authUtil.startSession("admin", ds01.Tables[0].Rows[0][4].ToString());
                Session["AdmUserID"] = authUtil.UserName;
                Session["AdmTicket"] = authUtil.Ticket;
                // Admin Credentials end
                if (Request.QueryString["UC"] != null)
                {
                    if (Request.QueryString["UC"].ToString() == "6a800c19d0014966809c8838507")
                    {
                        authUtil.startSession(Session["uid"].ToString(), Session["pwd"].ToString());
                    }
                }
                else
                {
                    if (Request.QueryString["URLDocUUID"] != null)
                    {
                        Session["URLDocUUID"] = Request.QueryString["URLDocUUID"].ToString();
                        authUtil.startSession(txtEmailID.Text.Trim(), txtPwd.Text.Trim());
                    }
                        // for "guest1" user
                    else if (txtEmailID.Text.Trim() == "guest1")
                    {
                        // Fetch Guest Password Start
                        ds001 = ObjClassStoreProc.UserInfoPassingUserID("guest1");
                        if (ds001.Tables[0].Rows.Count > 0)
                        {
                            Session["pwd"] = ds001.Tables[0].Rows[0][4].ToString();
                        }
                        // Fetch Guest Password End
                        authUtil.startSession(txtEmailID.Text.Trim(), Session["pwd"].ToString());
                    }
                        // for normal user except "guest1"
                    else
                    {
                        ds01 = ObjClassStoreProc.UserInfoPassingEmailID(txtEmailID.Text.Trim());
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            if (ds01.Tables[0].Rows[0][4].ToString() == txtPwd.Text.Trim())
                            {
                                ds03 = ObjClassStoreProc.CheckCompStat(txtEmailID.Text.Trim());
                                if (ds03.Tables[0].Rows[0][12].ToString() == "Active") // Need to check the Company's Status
                                {
                                    if (ds01.Tables[0].Rows[0][7].ToString() == "A") // Checking for Active Users
                                    {
                                        if (ds01.Tables[0].Rows[0][11].ToString() == "N") // Checking for Normal Users
                                        {
                                            ds02 = ObjClassStoreProc.AssignedRoleID(txtEmailID.Text.Trim());
                                            if (ds02.Tables[0].Rows.Count > 0)
                                            {

                                            }
                                            else
                                            {
                                                throw new Exception("Your Role is not assigned. Please contact with Administrator!!");
                                            }
                                        }
                                        authUtil.startSession(ds01.Tables[0].Rows[0][0].ToString(), txtPwd.Text.Trim());
                                    }
                                    else
                                    {
                                        throw new Exception("This User is not activated yet!!");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Your Account is " + ds03.Tables[0].Rows[0][12].ToString() + ". Please contact with Administrator!!");
                                }
                            }
                            else
                            {
                                throw new Exception("Wrong Email ID & Password Combination!!");
                            }
                        }
                        else
                        {
                            throw new Exception("Wrong Email ID & Password Combination!!");
                        }
                    }
                }

                if (authUtil.IsSessionValid)
                {
                    Session["UserID"] = authUtil.UserName;
                    Session["Ticket"] = authUtil.Ticket;
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.UserInfoPassingUserID(Session["UserID"].ToString());
                    Session["CanChangePwd"] = ds01.Tables[0].Rows[0][9].ToString();
                    Session["UserFullName"] = ds01.Tables[0].Rows[0][1].ToString() + " " + ds01.Tables[0].Rows[0][2].ToString();
                    Session["UserType"] = ds01.Tables[0].Rows[0][11].ToString();
                    Session["CompCode"] = ds01.Tables[0].Rows[0][10].ToString();
                    Session["PwdStat"] = ds01.Tables[0].Rows[0][8].ToString();
                    Session["UserEmail"] = ds01.Tables[0].Rows[0][3].ToString();
                    // For Opening from outside start
                    if (Session["URLDocUUID"] != null)
                    {
                        if (Session["URLDocUUID"].ToString() != "")
                        {
                            ds01.Reset();
                            ds01 = ObjClassStoreProc.DocDetails(Session["URLDocUUID"].ToString(), Session["CompCode"].ToString());
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                Session["VSDocName"] = ds01.Tables[0].Rows[0][1].ToString();
                                Session["SelDocUUID"] = ds01.Tables[0].Rows[0][4].ToString();
                                Session["uid"] = null;
                                Session["pwd"] = null;
                                ViewDoc(Session["SelDocUUID"].ToString());
                            }
                            else
                            {
                                if (Session["PwdStat"].ToString() == "Changed")
                                {
                                    Session["AccessControl"] = "";
                                    if (Session["UserType"].ToString() == "S")
                                    {
                                        Response.Redirect("CompWiseStatistics.aspx", false);
                                    }
                                    else
                                    {
                                        Response.Redirect("userhome.aspx", false);
                                    }
                                }
                                else
                                {
                                    Session["AccessControl"] = "";
                                    Response.Redirect("reset_pwd.aspx", false);
                                }
                            }
                        }
                    }
                    // For Opening from outside end
                    else
                    {
                        if (Session["CanChangePwd"].ToString() == "Y")
                        {
                            if (Session["PwdStat"].ToString() == "Changed")
                            {
                                Session["AccessControl"] = "";
                                if (Request.QueryString["Source"] != null)
                                {
                                    if (Request.QueryString["Source"] == "FS") // Folder Sniffer
                                    {
                                        byte[] bytes;
                                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                                        bytes = encoding.GetBytes(Request.QueryString["FileData"].ToString());
                                        Response.Redirect("doc_mast.aspx?Source=FS&DocName=" + Request.QueryString["DocName"] + "&FileData=" + bytes, false);
                                    }
                                    else
                                    {
                                        if (Session["UserType"].ToString() == "S")
                                        {
                                            Response.Redirect("CompWiseStatistics.aspx", false);
                                        }
                                        else
                                        {
                                            Response.Redirect("userhome.aspx", false);
                                        }
                                    }
                                }
                                else
                                {
                                    if (Session["UserType"].ToString() == "S")
                                    {
                                        Response.Redirect("CompWiseStatistics.aspx", false);
                                    }
                                    else
                                    {
                                        Response.Redirect("userhome.aspx", false);
                                    }
                                }
                            }
                            else
                            {
                                Session["AccessControl"] = "";
                                Response.Redirect("reset_pwd.aspx", false);
                            }
                        }
                        else
                        {
                            Session["AccessControl"] = "";
                            Response.Redirect("home.aspx", false);
                        }
                    }
                }
                else
                {
                    throw new Exception("Alfresco Login Error");
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText("C:\\LogFolder\\LogFile.txt"))
                {
                    Log("Error: " + ex.Message, w);
                    w.Close();
                }
                throw new Exception(ex.Message);
            }
        }

        protected void ViewDoc(string DocUUID)
        {
            try
            {
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                
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
                newSaveFileName = ObjFetchOnlyNameORExtension.FetchOnlyDocName(Session["VSDocName"].ToString()) + "_" + Session["UserID"].ToString() + "." + ObjFetchOnlyNameORExtension.FetchOnlyDocExt(Session["VSDocName"].ToString());
                //if (Session["VSDocName"].ToString().LastIndexOf(".pdf") == -1)
                //{
                //    newSaveFileName = Session["VSDocName"].ToString() + "_" + Session["UserID"].ToString() + ".pdf";
                //}
                //else
                //{
                //    newSaveFileName = Session["VSDocName"].ToString().Substring(0, Session["VSDocName"].ToString().Length - 4) + "_" + Session["UserID"].ToString() + ".pdf";
                //}
                string file_name = Server.MapPath("TempDownload") + "\\" + newSaveFileName;
                SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                //Download end
                Session["hfPageControl"] = "F";
                Session["AccessControl"] = "Outside";
                Response.Redirect("FormFillup.aspx?docname=" + newSaveFileName + "&DocUUID=" + DocUUID, false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private void MessageBox(string msg)
        {
            msg.Replace("'","`");
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

    }
}