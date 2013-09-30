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
using Alfresco.RepositoryWebService;

namespace DMS.Actions
{
    public class PreambleAppend
    {
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");
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

        public string PreAppend(string WFLogID, int StepNo, string TaskID, string CompCode, string AdminUserID, string AdminLoginTicket,string UserID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet dsPreAppend01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataSet ds03 = new DataSet();
                DataSet ds0001 = new DataSet();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string BeforeAppendDocUUID = "";
                string ActualDocUUID = "";
                string AppendDocUUID = "";
                string LicenseKey = "";
                string ServerIPAddress = "";
                string DomainName = "";
                string CompName = "";
                string HotlineNumber = "";
                string HotlineEmail = "";
                con.Open();

                #region Fetch ServerConfig Details Start
                dsPreAppend01.Reset();
                dsPreAppend01 = ObjClassStoreProc.SelectServerConfig(CompCode);
                if (dsPreAppend01.Tables[0].Rows.Count > 0)
                {
                    LicenseKey = dsPreAppend01.Tables[0].Rows[0][0].ToString();
                    ServerIPAddress = dsPreAppend01.Tables[0].Rows[0][1].ToString();
                    DomainName = dsPreAppend01.Tables[0].Rows[0][2].ToString();
                    CompName = dsPreAppend01.Tables[0].Rows[0][3].ToString();
                    HotlineNumber = dsPreAppend01.Tables[0].Rows[0][4].ToString();
                    HotlineEmail = dsPreAppend01.Tables[0].Rows[0][5].ToString();
                }
                #endregion

                cmd = new SqlCommand("select top 1 * from WFDocVersion where WFLogID='" + WFLogID + "' and NewDocUUID!='' and StepNo<='" + StepNo + "' order by StepNo desc", con);
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                dsPreAppend01.Reset();
                adapter01.Fill(dsPreAppend01);
                if (dsPreAppend01.Tables[0].Rows.Count > 0)
                {
                    BeforeAppendDocUUID = dsPreAppend01.Tables[0].Rows[0][3].ToString();
                    ActualDocUUID = dsPreAppend01.Tables[0].Rows[0][2].ToString();
                }
                string Doc1 = DwnldFile(BeforeAppendDocUUID,AdminUserID,AdminLoginTicket);
                string RWFOldFile = Doc1;

                cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                adapter02.Fill(ds02);
                if (ds02.Tables[0].Rows.Count > 0)
                {
                    AppendDocUUID = ds02.Tables[0].Rows[0][8].ToString();
                }
                string Doc2 = DwnldFile(AppendDocUUID,AdminUserID,AdminLoginTicket);
                RWFOldFile = Doc2;
                string OutputFile = HttpContext.Current.Server.MapPath("TempDownload") + "\\";
                QP.LoadFromFile(OutputFile + Doc1, "");
                int TotalFormFields = QP.FormFieldCount();
                for (int i = TotalFormFields; i > TotalFormFields - 1; i--)
                {
                    QP.DeleteFormField(i);
                }
                QP.SaveToFile(OutputFile + Doc1);
                int Result = QP.UnlockKey(LicenseKey);
                if (Result == 1)
                {
                    QP.LoadFromFile(OutputFile + Doc1, "");
                    int PrimaryDoc = QP.SelectedDocument();

                    QP.LoadFromFile(OutputFile + Doc2, "");
                    int SecondaryDoc = QP.SelectedDocument();

                    QP.SelectDocument(PrimaryDoc);
                    QP.MergeDocument(SecondaryDoc);
                    string fileName = GenNewDocName(Doc1);
                    QP.SaveToFile(OutputFile + fileName);

                    double FileSize = 0;

                    ds03 = ObjClassStoreProc.DocDetails(BeforeAppendDocUUID);
                    ds0001 = ObjClassStoreProc.DocDetails(AppendDocUUID);
                    FileSize = Convert.ToDouble(ds03.Tables[0].Rows[0][24].ToString()) + Convert.ToDouble(ds0001.Tables[0].Rows[0][24].ToString());

                    #region Upload into Alfresco
                    Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                    spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    spacesStore.address = "SpacesStore";

                    Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
                    parentReference.store = spacesStore;
                    parentReference.uuid = ds03.Tables[0].Rows[0][5].ToString(); // Folder's uuid

                    parentReference.associationType = Constants.ASSOC_CONTAINS;
                    parentReference.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, fileName);

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

                    CMLCreate create = new CMLCreate();
                    create.parent = parentReference;
                    create.id = "1";
                    create.type = Constants.TYPE_CONTENT;
                    create.property = properties;

                    WebServiceFactory wsF = new WebServiceFactory();
                    wsF.UserName = AdminUserID;
                    wsF.Ticket = AdminLoginTicket;
                    this.repoServiceA = wsF.getRepositoryService();

                    CML cml = new CML();
                    cml.create = new CMLCreate[] { create };
                    UpdateResult[] updateResult = repoServiceA.update(cml);

                    Alfresco.RepositoryWebService.Reference rwsRef = updateResult[0].destination;
                    Alfresco.ContentWebService.Reference newContentNode = new Alfresco.ContentWebService.Reference();
                    newContentNode.path = rwsRef.path;
                    newContentNode.uuid = rwsRef.uuid;
                    Alfresco.ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
                    cwsStore.address = "SpacesStore";
                    spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    newContentNode.store = cwsStore;

                    byte[] bytes;
                    bytes = StreamFile(HttpContext.Current.Server.MapPath("TempDownload") + "\\" + fileName);

                    Alfresco.ContentWebService.ContentFormat contentFormat = new Alfresco.ContentWebService.ContentFormat();
                    contentFormat.mimetype = "application/pdf";

                    wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);

                    File.Delete(HttpContext.Current.Server.MapPath("TempDownload") + "\\" + fileName);
                    #endregion

                    string result = ObjClassStoreProc.InsertDocMast(fileName, fileName, ds03.Tables[0].Rows[0][5].ToString(), ds03.Tables[0].Rows[0][2].ToString(), ds03.Tables[0].Rows[0][3].ToString(), UserID, DateTime.Now, ds03.Tables[0].Rows[0][8].ToString(), ds03.Tables[0].Rows[0][9].ToString(), ds03.Tables[0].Rows[0][10].ToString(), ds03.Tables[0].Rows[0][11].ToString(), ds03.Tables[0].Rows[0][12].ToString(), ds03.Tables[0].Rows[0][13].ToString(), ds03.Tables[0].Rows[0][14].ToString(), ds03.Tables[0].Rows[0][15].ToString(), ds03.Tables[0].Rows[0][16].ToString(), ds03.Tables[0].Rows[0][17].ToString(), "", newContentNode.uuid, newContentNode.uuid + "/" + fileName.Replace(" ", "%20"), "AP", CompCode, FileSize);

                    cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + FileSize + "' where CompCode='" + CompCode + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace - UsedSpace where CompCode='" + CompCode + "'", con);
                    cmd.ExecuteNonQuery();
                    #region Validation of space if it is exceeding or not
                    SqlDataAdapter adpAvl01;
                    DataSet dsAvl01 = new DataSet();
                    double AvailableSpace = 0;

                    cmd = new SqlCommand("select TotalSpace,UsedSpace,AvailableSpace from ServerConfig where CompCode='" + CompCode + "'", con);
                    adpAvl01 = new SqlDataAdapter(cmd);
                    dsAvl01.Reset();
                    adpAvl01.Fill(dsAvl01);
                    AvailableSpace = Convert.ToDouble(dsAvl01.Tables[0].Rows[0][2].ToString());
                    if (AvailableSpace < 0)
                    {
                        DataSet ds01 = new DataSet();
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
                        ds01 = ObjClassStoreProc.SelectServerConfig(CompCode);
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
                            MailMsg = "Hello,<br/>" + CompName + " [" + CompCode + "] has exceeded the using of storage space.<br/><br/>Thanks,<br/>System Administrator.";
                            if (Obj_Mail.SendEmail("", "admin", MailFrom, MailFrom, "", MailFrom, MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                            {

                            }
                        }
                    }
                    #endregion

                    UserRights RightsObj = new UserRights();
                    DataSet dsPerm = new DataSet();
                    dsPerm.Reset();
                    dsPerm = RightsObj.FetchPermission(ds03.Tables[0].Rows[0][5].ToString(), CompCode);
                    if (dsPerm.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                        {
                            RightsObj.InsertPermissionSingleData(newContentNode.uuid, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), CompCode);
                        }
                    }
                    cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + newContentNode.uuid + "' where WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + newContentNode.uuid + "' where WFLogID='" + WFLogID + "' and StepNo>'" + StepNo + "'", con);
                    cmd.ExecuteNonQuery();
                    #region Invisible the Older versions start
                    DataSet dsV01 = new DataSet();
                    cmd = new SqlCommand("select * from WFDoc where WFLogID='" + WFLogID + "' and DocUUID='" + newContentNode.uuid + "'", con);
                    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                    adapterV01.Fill(dsV01);
                    if (dsV01.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + WFLogID + "','" + newContentNode.uuid + "','" + CompCode + "')", con);
                        cmd.ExecuteNonQuery();
                        DataSet dsV02 = new DataSet();
                        cmd = new SqlCommand("select doc_id from doc_mast where uuid ='" + newContentNode.uuid + "'", con);
                        SqlDataAdapter adapterV02 = new SqlDataAdapter(cmd);
                        adapterV02.Fill(dsV02);
                        cmd = new SqlCommand("update wf_log_mast set doc_id='" + dsV02.Tables[0].Rows[0][0].ToString() + "' where wf_log_id='" + WFLogID + "'", con);
                        cmd.ExecuteNonQuery();
                        DataSet dsV001 = new DataSet();
                        cmd = new SqlCommand("select * from WFDoc where WFLogID='" + WFLogID + "' and DocUUID!='" + newContentNode.uuid + "' order by AutoID desc", con);
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
                    #endregion
                }
                else
                {
                    throw new Exception("Invalid Quick PDF license key");
                }
                return RWFOldFile;
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText("C:\\LogFolder\\LogFile.txt"))
                {
                    Log("Error in PreambleAppend.PreAppend for CompCode: " + CompCode + "; Error: " + ex.Message, w);
                    w.Close();
                }
                return "";
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.Flush();
        }

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] ImageData = new byte[fs.Length];
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return ImageData; //return the byte data
        }

        private string DwnldFile(string DocUUID, string AdminUserID, string AdminLoginTicket)
        {
            string DocExtension = "";
            DataSet dsDocDtls = new DataSet();
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
            wsFA.UserName = AdminUserID;
            wsFA.Ticket = AdminLoginTicket;
            Alfresco.ContentWebService.Content[] readResult = wsFA.getContentService().read(sourcePredicate, Constants.PROP_CONTENT);

            String ticketURL = "?ticket=" + wsFA.Ticket;
            String downloadURL = readResult[0].url + ticketURL;
            Uri address = new Uri(downloadURL);

            string url = downloadURL;
            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
            dsDocDtls.Reset();
            dsDocDtls = ObjClassStoreProc.DocDetails(DocUUID);

            if (dsDocDtls.Tables[0].Rows.Count > 0)
            {
                DocExtension = ObjFetchOnlyNameORExtension.FetchOnlyDocExt(dsDocDtls.Tables[0].Rows[0][1].ToString());
            }
            string newSaveFileName = GenNewDocName(dsDocDtls.Tables[0].Rows[0][1].ToString());
            //Session["RWFOldFile"] = newSaveFileName;
            string file_name = HttpContext.Current.Server.MapPath("TempDownload") + "\\" + newSaveFileName;
            SaveFileFromURL ObjSaveFileFromURL = new SaveFileFromURL();
            ObjSaveFileFromURL.SaveFile4mURL(file_name, url);
            return newSaveFileName;          
        }

        public string GenNewDocName(string DocName)
        {
            try
            {
                FetchOnlyNameORExtension ObjFetchOnlyNameORExtension = new FetchOnlyNameORExtension();
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

    }
}