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
using System.Configuration;
using System.Collections;

using Alfresco;
using Alfresco.RepositoryWebService;

namespace DMS.Actions
{
    public class PreambleCopy
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

        public string PreCopy(string WFLogID, int StepNo, string TaskID, string CompCode, string AdminUserID, string AdminLoginTicket, string UserID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds01 = new DataSet();
                DataSet ds04 = new DataSet();
                DataSet ds0001 = new DataSet();
                string WFID = "";
                Int64 DocID = 0;
                string DocName = "";
                string SourceUUID = "";
                string DestinationUUID = "";
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                con.Open();

                cmd = new SqlCommand("select a.wf_id,a.doc_id,b.doc_name,b.uuid from wf_log_mast a,doc_mast b where a.doc_id=b.doc_id and a.wf_log_id='" + WFLogID + "'", con);
                ds01.Reset();
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    WFID = ds01.Tables[0].Rows[0][0].ToString();
                    SourceUUID = ds01.Tables[0].Rows[0][3].ToString();
                }

                cmd = new SqlCommand("select ActualDocUUID from WFDocVersion where NewDocUUID='" + SourceUUID + "'", con);
                ds01.Reset();
                SqlDataAdapter adapterD01 = new SqlDataAdapter(cmd);
                adapterD01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    cmd = new SqlCommand("select top 1 a.WFLogID,a.StepNo,a.ActualDocUUID,a.NewDocUUID,b.doc_id,b.doc_name from WFDocVersion a,doc_mast b where a.NewDocUUID=b.uuid and a.WFLogID='" + WFLogID + "' and a.NewDocUUID!='' and a.StepNo<='" + StepNo + "' order by a.StepNo desc", con);
                    ds01.Reset();
                    SqlDataAdapter adapterD02 = new SqlDataAdapter(cmd);
                    adapterD02.Fill(ds01);
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        SourceUUID = ds01.Tables[0].Rows[0][3].ToString();
                        DocID = Convert.ToInt64(ds01.Tables[0].Rows[0][4].ToString());
                        DocName = ds01.Tables[0].Rows[0][5].ToString();
                    }
                }

                cmd = new SqlCommand("select copy_to_uuid from wf_task where wf_id='" + WFID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "'", con);
                ds01.Reset();
                SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                adapter02.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    DestinationUUID = ds01.Tables[0].Rows[0][0].ToString();
                }

                // Check the document is already there or not
                cmd = new SqlCommand("select * from doc_mast where doc_name='" + DocName + "' and fld_uuid='" + DestinationUUID + "'", con);
                ds04.Reset();
                SqlDataAdapter adapter04 = new SqlDataAdapter(cmd);
                adapter04.Fill(ds04);
                if (ds04.Tables[0].Rows.Count > 0)
                {
                    Utility.CloseConnection(con);
                    return "Copy Failure";
                }
                else
                {
                    #region Start Alfresco Part
                    this.spacesStore = new Alfresco.RepositoryWebService.Store();
                    this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                    this.spacesStore.address = "SpacesStore";

                    Alfresco.RepositoryWebService.Reference referenceForNode = new Alfresco.RepositoryWebService.Reference();
                    referenceForNode.store = this.spacesStore;
                    referenceForNode.uuid = SourceUUID; // Selected Doc's UUID

                    Alfresco.RepositoryWebService.Reference[] obj_new = new Alfresco.RepositoryWebService.Reference[] { referenceForNode };
                    Alfresco.RepositoryWebService.Predicate sourcePredicate = new Alfresco.RepositoryWebService.Predicate();
                    sourcePredicate.Items = obj_new;

                    Alfresco.RepositoryWebService.Reference referenceForTargetSpace = new Alfresco.RepositoryWebService.Reference();
                    referenceForTargetSpace.store = this.spacesStore;
                    referenceForTargetSpace.uuid = DestinationUUID; // Selected Location's (Folder's) UUID

                    //reference for the target space
                    Alfresco.RepositoryWebService.ParentReference targetSpace = new Alfresco.RepositoryWebService.ParentReference();
                    targetSpace.store = this.spacesStore;
                    targetSpace.uuid = referenceForTargetSpace.uuid;
                    targetSpace.associationType = Constants.ASSOC_CONTAINS;
                    targetSpace.childName = DocName; // Selected Doc's Name

                    //copy content
                    CMLCopy copy = new CMLCopy();
                    copy.where = sourcePredicate;
                    copy.to = targetSpace;

                    CML cmlCopy = new CML();
                    cmlCopy.copy = new CMLCopy[] { copy };

                    WebServiceFactory wsF = new WebServiceFactory();
                    wsF.UserName = AdminUserID;
                    wsF.Ticket = AdminLoginTicket;
                    wsF.getRepositoryService().update(cmlCopy);
                    #endregion End Alfresco Part

                    SearchNode ObjSearchNode = new SearchNode();
                    string CopiedDocUUID = ObjSearchNode.ExistNode(DestinationUUID, targetSpace.childName, AdminUserID, AdminLoginTicket);

                    #region .Net & SQL Server Coding Start
                    ds0001 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt32(DocID), CompCode);
                    if (ds0001.Tables[0].Rows.Count > 0)
                    {
                        UserRights RightsObj = new UserRights();
                        string result = ObjClassStoreProc.InsertDocMast(ds0001.Tables[0].Rows[0][1].ToString(), ds0001.Tables[0].Rows[0][1].ToString(), DestinationUUID, ds0001.Tables[0].Rows[0][2].ToString(), ds0001.Tables[0].Rows[0][3].ToString(), UserID, DateTime.Now, ds0001.Tables[0].Rows[0][8].ToString(), ds0001.Tables[0].Rows[0][9].ToString(), ds0001.Tables[0].Rows[0][10].ToString(), ds0001.Tables[0].Rows[0][11].ToString(), ds0001.Tables[0].Rows[0][12].ToString(), ds0001.Tables[0].Rows[0][13].ToString(), ds0001.Tables[0].Rows[0][14].ToString(), ds0001.Tables[0].Rows[0][15].ToString(), ds0001.Tables[0].Rows[0][16].ToString(), ds0001.Tables[0].Rows[0][17].ToString(), "", CopiedDocUUID, targetSpace.uuid + "/" + ds0001.Tables[0].Rows[0][1].ToString().Replace(" ", "%20"), "CP", CompCode, Convert.ToDouble(ds0001.Tables[0].Rows[0][24].ToString()));
                        if (Convert.ToInt32(result) == -1)
                        {

                        }
                        else
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            cmd = new SqlCommand("update ServerConfig set UsedSpace=UsedSpace+'" + Convert.ToDouble(ds0001.Tables[0].Rows[0][24].ToString()) + "' where CompCode='" + CompCode + "'", con);
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
                                string CompName = "";
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
                            con.Close();
                            DataSet dsPerm = new DataSet();
                            dsPerm.Reset();
                            dsPerm = RightsObj.FetchPermission(DestinationUUID, CompCode);
                            if (dsPerm.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                                {
                                    RightsObj.InsertPermissionSingleData(CopiedDocUUID, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), CompCode);
                                }
                            }
                        }
                    }
                    #endregion .Net & SQL Server Coding End

                    Utility.CloseConnection(con);
                    return "Copy Successful";
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText("C:\\LogFolder\\LogFile.txt"))
                {
                    Log("Error in PreambleCopy.PreCopy for CompCode: " + CompCode + "; Error: " + ex.Message, w);
                    w.Close();
                }
                return "Copy Failure";
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