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
using QuickPDFDLL0813;
using System.Configuration;
using System.Collections;

using Alfresco;
using Alfresco.ContentWebService;

namespace DMS.Actions
{
    public class PreambleEmail
    {
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");
        
        public void SendPreMail(string WFLogID, int StepNo, string TaskID, string CompCode, string AccessControl, string InitiatorEmailID, string AdminUserID, string AdminLoginTicket)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet dsPreEmail01 = new DataSet();
                DataSet ds01 = new DataSet();
                DataSet ds03 = new DataSet();
                DataSet ds04 = new DataSet();
                DataSet ds05 = new DataSet();
                DataSet ds06 = new DataSet();
                DataSet ds07 = new DataSet();
                DataSet ds001 = new DataSet();
                DataSet dsA01 = new DataSet();
                DataSet dsD01 = new DataSet();
                DataSet dsDocDtls = new DataSet();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                
                string MailTo = "";
                string MailSub = "";
                string MailMsg = "";
                string MailFrom = "";
                string Duration = "";
                string AssignedBy = "";
                string AssignedTo = "";
                string WFName = "";
                string SenderMail = "";
                string SenderName = "";
                string SmtpHost = "";
                Int32 SmtpPort = 0;
                string CredenUsername = "";
                string CredenPwd = "";
                string DocExtension = "";
                string EmailAttach = "No";
                string EmailURL = "No";

                #region Fetch the Mail Settings from Database Start
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
                #endregion

                string AttachmentFileUUID = "";
                string AttachFileName = "";
                string TempAttachFileURL = "";
                string PermAttachFileURL = "";
                string DomainName = "";
                string CompName = "";
                string HotlineNumber = "";
                string HotlineEmail = "";
                cmd = new SqlCommand("select amble_attach,amble_url from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "' and CompCode='" + CompCode + "'", con);
                SqlDataAdapter adapterA01 = new SqlDataAdapter(cmd);
                adapterA01.Fill(dsA01);

                #region Fetch ServerConfig Details Start
                dsPreEmail01.Reset();
                dsPreEmail01 = ObjClassStoreProc.SelectServerConfig(CompCode);
                if (dsPreEmail01.Tables[0].Rows.Count > 0)
                {
                    DomainName = dsPreEmail01.Tables[0].Rows[0][2].ToString();
                    CompName = dsPreEmail01.Tables[0].Rows[0][3].ToString();
                    HotlineNumber = dsPreEmail01.Tables[0].Rows[0][4].ToString();
                    HotlineEmail = dsPreEmail01.Tables[0].Rows[0][5].ToString();
                }
                #endregion

                #region For Attachment=Yes
                if (dsA01.Tables[0].Rows[0][0].ToString() == "Yes")
                {
                    EmailAttach = "Yes";
                    cmd = new SqlCommand("select top 1 * from WFDocVersion where WFLogID='" + WFLogID + "' and NewDocUUID!='' and StepNo<='" + StepNo + "' and CompCode='" + CompCode + "' order by StepNo desc", con);
                    
                    SqlDataAdapter adapterD01 = new SqlDataAdapter(cmd);
                    adapterD01.Fill(dsD01);
                    if (dsD01.Tables[0].Rows.Count > 0)
                    {
                        AttachmentFileUUID = dsD01.Tables[0].Rows[0][3].ToString();
                    }

                    // Initialise the reference to the spaces store
                    Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                    spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                    spacesStore.address = "SpacesStore";

                    Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                    referenceForNode.store = spacesStore;
                    referenceForNode.uuid = AttachmentFileUUID;//Doc UUID

                    Alfresco.ContentWebService.Reference[] obj_new = new Alfresco.ContentWebService.Reference[] { referenceForNode };
                    Alfresco.ContentWebService.Predicate sourcePredicate = new Alfresco.ContentWebService.Predicate();
                    sourcePredicate.Items = obj_new;

                    WebServiceFactory wsFA = new WebServiceFactory();
                    wsFA.UserName = AdminUserID;
                    wsFA.Ticket = AdminLoginTicket;

                    Alfresco.ContentWebService.Content[] readResult = wsFA.getContentService().read(sourcePredicate, Constants.PROP_CONTENT);

                    String ticketURL = "?ticket=" + wsFA.Ticket;
                    String downloadURL = readResult[0].url + ticketURL;
                    Uri address = new Uri(downloadURL);
                    string url = downloadURL;
                    dsDocDtls.Reset();
                    dsDocDtls = ObjClassStoreProc.DocDetails(AttachmentFileUUID);

                    if (dsDocDtls.Tables[0].Rows.Count > 0)
                    {
                        DocExtension = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(dsDocDtls.Tables[0].Rows[0][1].ToString());
                    }

                    string file_name = HttpContext.Current.Server.MapPath("eMailDocs\\") + AttachmentFileUUID + "_1." + DocExtension;
                    SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                    ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DocTypeDetailsPassingDocUUID(AttachmentFileUUID);
                    if (ds01.Tables[0].Rows[0][29].ToString() != "Other")
                    {
                        if (DocExtension == "pdf")
                        {
                            QP.LoadFromFile(file_name, "");
                            int TotalFormFields = QP.FormFieldCount();
                            for (int i = TotalFormFields; i > TotalFormFields - 1; i--)
                            {
                                QP.DeleteFormField(i);
                            }
                            QP.SaveToFile(file_name);
                        }
                    }
                    AttachFileName = file_name;
                }
                #endregion

                #region For TempURL=Yes
                if (dsA01.Tables[0].Rows[0][1].ToString() == "Yes")
                {
                    EmailURL = "Yes";
                    cmd = new SqlCommand("select top 1 * from WFDocVersion where WFLogID='" + WFLogID + "' and NewDocUUID!='' and StepNo<='" + StepNo + "' and CompCode='" + CompCode + "' order by StepNo desc", con);
                    SqlDataAdapter adapterD01 = new SqlDataAdapter(cmd);
                    adapterD01.Fill(dsD01);
                    if (dsD01.Tables[0].Rows.Count > 0)
                    {
                        AttachmentFileUUID = dsD01.Tables[0].Rows[0][3].ToString();
                    }

                    // Initialise the reference to the spaces store
                    Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                    spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                    spacesStore.address = "SpacesStore";

                    Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                    referenceForNode.store = spacesStore;
                    referenceForNode.uuid = AttachmentFileUUID;//Doc UUID

                    Alfresco.ContentWebService.Reference[] obj_new = new Alfresco.ContentWebService.Reference[] { referenceForNode };
                    Alfresco.ContentWebService.Predicate sourcePredicate = new Alfresco.ContentWebService.Predicate();
                    sourcePredicate.Items = obj_new;

                    WebServiceFactory wsFA = new WebServiceFactory();
                    wsFA.UserName = AdminUserID;
                    wsFA.Ticket = AdminLoginTicket;

                    Alfresco.ContentWebService.Content[] readResult = wsFA.getContentService().read(sourcePredicate, Constants.PROP_CONTENT);

                    String ticketURL = "?ticket=" + wsFA.Ticket;
                    String downloadURL = readResult[0].url + ticketURL;
                    Uri address = new Uri(downloadURL);
                    string url = downloadURL;
                    dsDocDtls.Reset();
                    dsDocDtls = ObjClassStoreProc.DocDetails(AttachmentFileUUID);

                    if (dsDocDtls.Tables[0].Rows.Count > 0)
                    {
                        DocExtension = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(dsDocDtls.Tables[0].Rows[0][1].ToString());
                    }

                    string file_name = HttpContext.Current.Server.MapPath("TempFolderURL\\") + AttachmentFileUUID + "_X1." + DocExtension;
                    SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                    ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DocTypeDetailsPassingDocUUID(AttachmentFileUUID);
                    if (ds01.Tables[0].Rows[0][29].ToString() != "Other")
                    {
                        if (DocExtension == "pdf")
                        {
                            QP.LoadFromFile(file_name, "");
                            int TotalFormFields = QP.FormFieldCount();
                            for (int i = TotalFormFields; i > TotalFormFields - 1; i--)
                            {
                                QP.DeleteFormField(i);
                            }
                            QP.SaveToFile(file_name);
                        }
                    }
                    TempAttachFileURL = "http://" + DomainName + "/TempFolderURL//" + AttachmentFileUUID + "_X1." + DocExtension;
                    PermAttachFileURL = "http://" + DomainName + "/Default.aspx?URLDocUUID=" + AttachmentFileUUID;
                }
                #endregion

                cmd = new SqlCommand("select wf_id,doc_id,started_by from wf_log_mast where wf_log_id='" + WFLogID + "' and CompCode='" + CompCode + "'", con);
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    AssignedBy = ds01.Tables[0].Rows[0][2].ToString();
                    cmd = new SqlCommand("select wf_name from wf_mast where wf_id='" + ds01.Tables[0].Rows[0][0].ToString() + "' and CompCode='" + CompCode + "'", con);
                    SqlDataAdapter adapter06 = new SqlDataAdapter(cmd);
                    adapter06.Fill(ds06);
                    if (ds06.Tables[0].Rows.Count > 0)
                    {
                        WFName = ds06.Tables[0].Rows[0][0].ToString();
                    }

                    cmd = new SqlCommand("select amble_mails,amble_msg,AmbleSub from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "' and CompCode='" + CompCode + "'", con);
                    SqlDataAdapter adapter07 = new SqlDataAdapter(cmd);
                    adapter07.Fill(ds07);
                    if (ds07.Tables[0].Rows.Count > 0)
                    {
                        MailTo = ds07.Tables[0].Rows[0][0].ToString();
                        if (EmailAttach == "Yes")
                        {
                            MailMsg = "<br/><br/>Please find the attachment for your review.";
                        }
                        if (EmailURL == "Yes")
                        {
                            MailMsg += "<br/><br/>Please click on one of the URLs below to view the document:<br/><br/>1) Without logging in to the system:<br/>" + TempAttachFileURL + "<br/>(Link expires after 1 year due to security reasons.)<br/><br/>2) Through the myDOCbase system (requires login):<br/>" + PermAttachFileURL;
                        }
                        MailMsg = ds07.Tables[0].Rows[0][1].ToString() + MailMsg + "<br/><br/>In the event of a problem, kindly dial our Customer Service Hotline (" + HotlineNumber + ") or email the problem to: " + HotlineEmail + ".<br/><br/>Thank you.<br/>myDOCbase Systems Administrator<br/>" + CompName;
                        MailSub = ds07.Tables[0].Rows[0][2].ToString();
                    }

                    if (AccessControl == "Outside")
                    {
                        MailFrom = InitiatorEmailID;
                    }
                    else
                    {
                        cmd = new SqlCommand("select email from user_mast where user_id='" + AssignedBy + "'", con);
                        SqlDataAdapter adapter05 = new SqlDataAdapter(cmd);
                        adapter05.Fill(ds05);
                        if (ds05.Tables[0].Rows.Count > 0)
                        {
                            MailFrom = ds05.Tables[0].Rows[0][0].ToString();
                        }
                    }

                    cmd = new SqlCommand("select role_id,duration from wf_dtl where wf_id='" + ds01.Tables[0].Rows[0][0].ToString() + "' and step_no='" + StepNo + "' and CompCode='" + CompCode + "'", con);
                    SqlDataAdapter adapter03 = new SqlDataAdapter(cmd);
                    adapter03.Fill(ds03);
                    if (ds03.Tables[0].Rows.Count > 0)
                    {
                        Duration = ds03.Tables[0].Rows[0][1].ToString();
                        cmd = new SqlCommand("select user_id,email from user_mast where user_id in(select user_id from user_role where role_id='" + ds03.Tables[0].Rows[0][0].ToString() + "')", con);
                        SqlDataAdapter adapter04 = new SqlDataAdapter(cmd);
                        adapter04.Fill(ds04);
                        if (ds04.Tables[0].Rows.Count > 0)
                        {
                            AssignedTo = ds04.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    mailing Obj_Mail = new mailing();
                    if (Obj_Mail.SendEmail(AttachFileName, MailFrom, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText("C:\\LogFolder\\LogFile.txt"))
                {
                    Log("Error in PreambleEmail.SendPreMail for CompCode: " + CompCode + "; Error: " + ex.Message, w);
                    w.Close();
                }
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