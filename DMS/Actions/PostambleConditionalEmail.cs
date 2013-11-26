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
using iTextSharp.text;
using iTextSharp.text.pdf;

using Alfresco;
using Alfresco.ContentWebService;

namespace DMS.Actions
{
    public class PostambleConditionalEmail
    {
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");

        public void SendPostCondMail(string WFLogID, int StepNo, string TaskID, string CompCode, string AccessControl, string InitiatorEmailID, string AdminUserID, string AdminLoginTicket)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet dsPostCondEmail01 = new DataSet();
                DataSet ds01 = new DataSet();
                DataSet ds05 = new DataSet();
                DataSet ds08 = new DataSet();
                DataSet ds09 = new DataSet();
                DataSet ds10 = new DataSet();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
                string FormType = "";

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
                dsPostCondEmail01.Reset();
                dsPostCondEmail01 = ObjClassStoreProc.SelectServerConfig(CompCode);
                if (dsPostCondEmail01.Tables[0].Rows.Count > 0)
                {
                    DomainName = dsPostCondEmail01.Tables[0].Rows[0][2].ToString();
                    CompName = dsPostCondEmail01.Tables[0].Rows[0][3].ToString();
                    HotlineNumber = dsPostCondEmail01.Tables[0].Rows[0][4].ToString();
                    HotlineEmail = dsPostCondEmail01.Tables[0].Rows[0][5].ToString();
                }
                #endregion

                #region Attached File in the workflow
                cmd = new SqlCommand("select * from WFDocVersion where WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and CompCode='" + CompCode + "'", con);
                ds01.Reset();
                SqlDataAdapter adapterD01 = new SqlDataAdapter(cmd);
                adapterD01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    AttachmentFileUUID = ds01.Tables[0].Rows[0][2].ToString();
                }
                #endregion

                #region Check for eForm or other type
                string ExistingProcess = "";
                ds01.Reset();
                ds01 = ObjClassStoreProc.DocDetails(AttachmentFileUUID, CompCode);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    FormType = ds01.Tables[0].Rows[0][27].ToString();
                    if (FormType == "eForm")
                    {
                        DocExtension = ds01.Tables[0].Rows[0][1].ToString().Substring(ds01.Tables[0].Rows[0][1].ToString().Length - 4, 4);
                        if (DocExtension == ".pdf")
                        {
                            ExistingProcess = "Y";
                        }
                        else
                        {
                            ExistingProcess = "N";
                        }
                    }
                    else
                    {
                        ExistingProcess = "Y";
                    }
                }
                #endregion

                if (ExistingProcess == "Y")
                {
                    #region For Attachment=Yes
                    if (dsA01.Tables[0].Rows[0][0].ToString() == "Yes")
                    {
                        EmailAttach = "Yes";
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
                        dsDocDtls = ObjClassStoreProc.DocDetails(AttachmentFileUUID, CompCode);
                        if (dsDocDtls.Tables[0].Rows.Count > 0)
                        {
                            DocExtension = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(dsDocDtls.Tables[0].Rows[0][1].ToString());
                        }
                        string file_name = HttpContext.Current.Server.MapPath("eMailDocs\\") + AttachmentFileUUID + "_4." + DocExtension;
                        SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                        ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.DocTypeDetailsPassingDocUUID(AttachmentFileUUID);
                        if (ds01.Tables[0].Rows[0][29].ToString() != "Non Editable")
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
                        dsDocDtls = ObjClassStoreProc.DocDetails(AttachmentFileUUID, CompCode);
                        if (dsDocDtls.Tables[0].Rows.Count > 0)
                        {
                            DocExtension = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(dsDocDtls.Tables[0].Rows[0][1].ToString());
                        }
                        string file_name = HttpContext.Current.Server.MapPath("TempFolderURL\\") + AttachmentFileUUID + "_X4." + DocExtension;
                        SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
                        ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.DocTypeDetailsPassingDocUUID(AttachmentFileUUID);
                        if (ds01.Tables[0].Rows[0][29].ToString() == "Editable")
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
                        TempAttachFileURL = "http://" + DomainName + "/TempFolderURL//" + AttachmentFileUUID + "_X4." + DocExtension;
                        PermAttachFileURL = "http://" + DomainName + "/Default.aspx?URLDocUUID=" + AttachmentFileUUID;
                    }
                    #endregion
                }
                else if (ExistingProcess == "N")
                {
                    #region For Attachment=Yes
                    if (dsA01.Tables[0].Rows[0][0].ToString() == "Yes")
                    {
                        EmailAttach = "Yes";
                        #region PDF Creation
                        Font font8 = FontFactory.GetFont("ARIAL", 10);
                        DataTable dt;
                        dt = new DataTable();
                        dt = CreateDT4PDF(AttachmentFileUUID, CompCode);

                        string FileName = ds01.Tables[0].Rows[0][1].ToString().Replace(" ", "_") + "_" + GetTimestamp(DateTime.Now) + ".pdf";
                        string TotStr = ds01.Tables[0].Rows[0][1].ToString();
                        Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 42, 35);
                        PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath("eMailDocs") + "\\" + FileName, FileMode.Create));
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
                        #endregion
                        AttachFileName = HttpContext.Current.Server.MapPath("eMailDocs") + "\\" + FileName;
                    }
                    #endregion

                    #region For TempURL=Yes
                    if (dsA01.Tables[0].Rows[0][1].ToString() == "Yes")
                    {
                        EmailURL = "Yes";
                        #region PDF Creation
                        Font font8 = FontFactory.GetFont("ARIAL", 10);
                        DataTable dt;
                        dt = new DataTable();
                        dt = CreateDT4PDF(AttachmentFileUUID, CompCode);

                        string FileName = ds01.Tables[0].Rows[0][1].ToString().Replace(" ", "_") + "_" + GetTimestamp(DateTime.Now) + ".pdf";
                        string TotStr = ds01.Tables[0].Rows[0][1].ToString();
                        Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 42, 35);
                        PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath("TempFolderURL") + "\\" + FileName, FileMode.Create));
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
                        #endregion
                        string file_name = HttpContext.Current.Server.MapPath("TempFolderURL") + "\\" + FileName.Replace(" ", "_");
                        TempAttachFileURL = "http://" + DomainName + "/TempFolderURL//" + FileName.Replace(" ", "_");
                        //PermAttachFileURL = "http://" + DomainName + "/Default.aspx?URLDocUUID=" + AttachmentFileUUID;
                    }
                    #endregion
                }

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
                                if (FormType == "eForm")
                                {
                                    MailMsg += "<br/><br/>Please click on the URL below to view the document:<br/><br/>Without logging in to the system:<br/>" + TempAttachFileURL + "<br/>(Link expires after 1 year due to security reasons.)";
                                }
                                else
                                {
                                    MailMsg += "<br/><br/>Please click on one of the URLs below to view the document:<br/><br/>1) Without logging in to the system:<br/>" + TempAttachFileURL + "<br/>(Link expires after 1 year due to security reasons.)<br/><br/>2) Through the myDOCbase system (requires login):<br/>" + PermAttachFileURL;
                                }
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
                    Log("Error in PostambleConditionalEmail.SendPostCondMail for CompCode: " + CompCode + "; Error: " + ex.Message, w);
                    w.Close();
                }
            }
        }

        protected DataTable CreateDT4PDF(string DocUUID, string CompCode)
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
                ds01 = ObjClassStoreProc.DocDetails(TemplateUUID, CompCode);
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

        private String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
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

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.Flush();
        }

    }
}