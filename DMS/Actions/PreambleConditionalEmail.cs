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
    public class PreambleConditionalEmail
    {
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");

        public void SendPreCondMail(string WFLogID, int StepNo, string TaskID, string CompCode, string AccessControl, string InitiatorEmailID, string AdminUserID, string AdminLoginTicket)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet dsPreCondEmail01 = new DataSet();
                DataSet ds01 = new DataSet();
                DataSet ds05 = new DataSet();
                DataSet ds08 = new DataSet();
                DataSet ds09 = new DataSet();
                DataSet ds10 = new DataSet();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();

                string MailTo = "";
                string MailSub = "";
                string MailMsg = "";
                string MailFrom = "";
                string AssignedBy = "";
                string WFName = "";
                string SenderMail = "";
                string SenderName = "";
                string SmtpHost = "";
                Int32 SmtpPort = 0;
                string CredenUsername = "";
                string CredenPwd = "";
                string DocUUID = "";
                string DocName = "";
                string DocTypeID = "";
                string DocExtension = "";
                DataSet dsDocDtls = new DataSet();
                string EmailAttach = "No";
                string EmailURL = "No";

                #region Fetch the Mail Settings from Database Start
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
                #endregion

                string AttachmentFileUUID = "";
                string AttachFileName = "";
                string TempAttachFileURL = "";
                string PermAttachFileURL = "";
                string DomainName = "";
                string CompName = "";
                string HotlineNumber = "";
                string HotlineEmail = "";

                cmd = new SqlCommand("select amble_attach,amble_url from wf_cond where wf_id in(select wf_id from wf_log_mast where wf_log_id='" + WFLogID + "') and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                DataSet dsA01 = new DataSet();
                SqlDataAdapter adapterA01 = new SqlDataAdapter(cmd);
                adapterA01.Fill(dsA01);
                
                #region Fetch ServerConfig Details Start
                dsPreCondEmail01.Reset();
                dsPreCondEmail01 = ObjClassStoreProc.SelectServerConfig(CompCode);
                if (dsPreCondEmail01.Tables[0].Rows.Count > 0)
                {
                    DomainName = dsPreCondEmail01.Tables[0].Rows[0][2].ToString();
                    CompName = dsPreCondEmail01.Tables[0].Rows[0][3].ToString();
                    HotlineNumber = dsPreCondEmail01.Tables[0].Rows[0][4].ToString();
                    HotlineEmail = dsPreCondEmail01.Tables[0].Rows[0][5].ToString();
                }
                #endregion

                #region For Attachment=Yes
                if (dsA01.Tables[0].Rows[0][0].ToString() == "Yes")
                {
                    EmailAttach = "Yes";
                    cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and CompCode='" + CompCode + "'", con);
                    ds01.Reset();
                    SqlDataAdapter adapterD01 = new SqlDataAdapter(cmd);
                    adapterD01.Fill(ds01);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        AttachmentFileUUID = ds01.Tables[0].Rows[0][2].ToString();
                    }

                    // Initialise the reference to the spaces store
                    Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                    spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                    spacesStore.address = "SpacesStore";

                    Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                    referenceForNode.store = spacesStore;
                    referenceForNode.uuid = AttachmentFileUUID; //Doc UUID

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
                    string file_name = HttpContext.Current.Server.MapPath("eMailDocs\\") + AttachmentFileUUID + "_3." + DocExtension;
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
                    cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and CompCode='" + CompCode + "'", con);
                    ds01.Reset();
                    SqlDataAdapter adapterD01 = new SqlDataAdapter(cmd);
                    adapterD01.Fill(ds01);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        AttachmentFileUUID = ds01.Tables[0].Rows[0][2].ToString();
                    }

                    // Initialise the reference to the spaces store
                    Alfresco.ContentWebService.Store spacesStore = new Alfresco.ContentWebService.Store();
                    spacesStore.scheme = Alfresco.ContentWebService.StoreEnum.workspace;
                    spacesStore.address = "SpacesStore";

                    Alfresco.ContentWebService.Reference referenceForNode = new Alfresco.ContentWebService.Reference();
                    referenceForNode.store = spacesStore;
                    referenceForNode.uuid = AttachmentFileUUID; //Doc UUID

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
                    string file_name = HttpContext.Current.Server.MapPath("TempFolderURL\\") + AttachmentFileUUID + "_X3." + DocExtension;
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
                    TempAttachFileURL = "http://" + DomainName + "/TempFolderURL//" + AttachmentFileUUID + "_X3." + DocExtension;
                    PermAttachFileURL = "http://" + DomainName + "/Default.aspx?URLDocUUID=" + AttachmentFileUUID;
                }
                #endregion

                cmd = new SqlCommand("select a.wf_id,a.doc_id,a.started_by,b.doc_name,b.uuid from wf_log_mast a,doc_mast b where a.doc_id=b.doc_id and a.wf_log_id='" + WFLogID + "'", con);
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    DocName = ds01.Tables[0].Rows[0][3].ToString();
                    DocUUID = ds01.Tables[0].Rows[0][4].ToString();
                    AssignedBy = ds01.Tables[0].Rows[0][2].ToString();

                    if (AccessControl == "Outside")
                    {
                        MailFrom = InitiatorEmailID;
                    }
                    else
                    {
                        cmd = new SqlCommand("select email from user_mast where user_id='" + AssignedBy + "'", con);
                        SqlDataAdapter adapter05 = new SqlDataAdapter(cmd);
                        ds05.Reset();
                        adapter05.Fill(ds05);
                        if (ds05.Tables[0].Rows.Count > 0)
                        {
                            MailFrom = ds05.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    cmd = new SqlCommand("select wf_name,doc_type_id from wf_mast where wf_id='" + ds01.Tables[0].Rows[0][0].ToString() + "'", con);
                    SqlDataAdapter adapter06 = new SqlDataAdapter(cmd);
                    ds05.Reset();
                    adapter06.Fill(ds05);
                    if (ds05.Tables[0].Rows.Count > 0)
                    {
                        WFName = ds05.Tables[0].Rows[0][0].ToString();
                        DocTypeID = ds05.Tables[0].Rows[0][1].ToString();
                    }

                    // Checking for is there any condition or not start
                    cmd = new SqlCommand("select form_field_no,cond_op,cond_val,amble_mails,amble_msg,AmbleSub from wf_cond where wf_id='" + ds01.Tables[0].Rows[0][0].ToString() + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "' order by form_field_no", con);
                    SqlDataAdapter adapter08 = new SqlDataAdapter(cmd);
                    ds08.Reset();
                    adapter08.Fill(ds08);
                    if (ds08.Tables[0].Rows.Count > 0)
                    {
                        string Operator = "";
                        for (int g = 0; g < ds08.Tables[0].Rows.Count; g++)
                        {
                            MailTo = ds08.Tables[0].Rows[g][3].ToString();
                            if (EmailAttach == "Yes")
                            {
                                MailMsg = "<br/><br/>Please find the attachment for your review.";
                            }
                            if (EmailURL == "Yes")
                            {
                                MailMsg += "<br/><br/>Please click on one of the URLs below to view the document:<br/><br/>1) Without logging in to the system:<br/>" + TempAttachFileURL + "<br/>(Link expires after 1 year due to security reasons.)<br/><br/>2) Through the myDOCbase system (requires login):<br/>" + PermAttachFileURL;
                            }
                            MailMsg = ds08.Tables[0].Rows[g][4].ToString() + MailMsg + "<br/><br/>In the event of a problem, kindly dial our Customer Service Hotline (" + HotlineNumber + ") or email the problem to: " + HotlineEmail + ".<br/><br/>Thank you.<br/>myDOCbase Systems Administrator<br/>" + CompName;
                            MailSub = ds08.Tables[0].Rows[g][5].ToString();

                            // Fetch Start..
                            string ActualValue = "";
                            string ActualFldNo = "";
                            cmd = new SqlCommand("select tag1_fieldno,tag2_fieldno,tag3_fieldno,tag4_fieldno,tag5_fieldno,tag6_fieldno,tag7_fieldno,tag8_fieldno,tag9_fieldno,tag10_fieldno from doc_type_mast where doc_type_id='" + DocTypeID + "'", con);
                            SqlDataAdapter adapter09 = new SqlDataAdapter(cmd);
                            ds09.Reset();
                            adapter09.Fill(ds09);
                            if (ds09.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < 10; k++)
                                {
                                    if (ds08.Tables[0].Rows[g][0].ToString() == ds09.Tables[0].Rows[0][k].ToString())
                                    {
                                        ActualFldNo = "tag" + (Convert.ToInt32(k) + 1).ToString();
                                        cmd = new SqlCommand("select " + ActualFldNo + " from doc_mast where uuid='" + DocUUID + "'", con);
                                        SqlDataAdapter adapter10 = new SqlDataAdapter(cmd);
                                        ds10.Reset();
                                        adapter10.Fill(ds10);
                                        if (ds10.Tables[0].Rows.Count > 0)
                                        {
                                            ActualValue = ds10.Tables[0].Rows[0][0].ToString();
                                        }
                                    }
                                }
                            }
                            // Fetch End

                            Operator = ds08.Tables[0].Rows[g][1].ToString();
                            if (Operator == "=")
                            {
                                if (ActualValue == ds08.Tables[0].Rows[g][2].ToString())
                                {
                                    mailing Obj_Mail = new mailing();
                                    if (Obj_Mail.SendEmail(AttachFileName, MailFrom, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                                    {

                                    }
                                }
                            }
                            else if (Operator == ">")
                            {
                                int num;
                                bool res = int.TryParse(ActualValue, out num);
                                if (int.TryParse(ActualValue, out num) == false)
                                {
                                    if (0 > Convert.ToDouble(ds08.Tables[0].Rows[g][2].ToString()))
                                    {
                                        mailing Obj_Mail = new mailing();
                                        if (Obj_Mail.SendEmail(AttachFileName, MailFrom, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDouble(ActualValue) > Convert.ToDouble(ds08.Tables[0].Rows[g][2].ToString()))
                                    {
                                        mailing Obj_Mail = new mailing();
                                        if (Obj_Mail.SendEmail(AttachFileName, MailFrom, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                                        {

                                        }
                                    }
                                }
                            }
                            else if (Operator == "<")
                            {
                                int num;
                                bool res = int.TryParse(ActualValue, out num);
                                if (int.TryParse(ActualValue, out num) == false)
                                {
                                    if (0 < Convert.ToDouble(ds08.Tables[0].Rows[g][2].ToString()))
                                    {
                                        mailing Obj_Mail = new mailing();
                                        if (Obj_Mail.SendEmail(AttachFileName, MailFrom, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDouble(ActualValue) < Convert.ToDouble(ds08.Tables[0].Rows[g][2].ToString()))
                                    {
                                        mailing Obj_Mail = new mailing();
                                        if (Obj_Mail.SendEmail(AttachFileName, MailFrom, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                                        {

                                        }
                                    }
                                }
                            }
                            else if (Operator == "!=")
                            {
                                if (ActualValue != ds08.Tables[0].Rows[g][2].ToString())
                                {
                                    mailing Obj_Mail = new mailing();
                                    if (Obj_Mail.SendEmail(AttachFileName, MailFrom, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                                    {

                                    }
                                }
                            }
                        }
                    }
                    // Checking for is there any condition or not end
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText("C:\\LogFolder\\LogFile.txt"))
                {
                    Log("Error in PreambleConditionalEmail.SendPreCondMail for CompCode: " + CompCode + "; Error: " + ex.Message, w);
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