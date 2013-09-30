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

namespace DMS.Actions
{
    public class RejectEmail
    {
        public void RejectMail(string WFLogID, int StepNo, string CompCode)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                DataSet ds001 = new DataSet();
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataSet ds03 = new DataSet();
                DataSet ds003 = new DataSet();

                string SenderMail = "";
                string SenderName = "";
                string SmtpHost = "";
                Int32 SmtpPort = 0;
                string CredenUsername = "";
                string CredenPwd = "";
                string WFID = "";
                string MailTo = "";
                string MailSub = "";
                string MailMsg = "";
                string MailFrom = "";
                string AssignedBy = "";
                string MailDocName = "";
                string WFLName = "";


                /// Is there any previous stage or not Start
                cmd = new SqlCommand("select * from wf_log_dtl where wf_log_id='" + WFLogID + "' and CompCode='" + CompCode + "' and step_no<=" + StepNo, con);
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
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

                    /// Select WF_ID from wf_log_mast
                    cmd = new SqlCommand("select a.wf_id,b.wf_name,c.doc_name from wf_log_mast a, wf_mast b,doc_mast c where a.wf_id=b.wf_id and a.doc_id=c.doc_id and a.wf_log_id='" + WFLogID + "'", con);
                    SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                    adapter02.Fill(ds02);
                    if (ds02.Tables[0].Rows.Count > 0)
                    {
                        WFID = ds02.Tables[0].Rows[0][0].ToString();
                        MailSub = ds02.Tables[0].Rows[0][2].ToString() + " Rejected";
                        MailDocName = ds02.Tables[0].Rows[0][2].ToString();
                        WFLName = ds02.Tables[0].Rows[0][1].ToString();
                    }

                    // Fetch who has rejected and his/her mail id
                    cmd = new SqlCommand("select a.role_id,b.user_id,c.email from wf_dtl a,user_role b,user_mast c where a.role_id=b.role_id and b.user_id=c.user_id and a.wf_id='" + WFID + "' and a.step_no='" + StepNo + "'", con);
                    SqlDataAdapter adapter04 = new SqlDataAdapter(cmd);
                    ds02.Reset();
                    adapter04.Fill(ds02);
                    if (ds02.Tables[0].Rows.Count > 0)
                    {
                        AssignedBy = ds02.Tables[0].Rows[0][1].ToString();
                        MailFrom = ds02.Tables[0].Rows[0][2].ToString();
                    }

                    MailMsg = MailDocName + " has been rejected by " + AssignedBy + " in Stage " + StepNo + " of " + WFLName + ".<br/><br/>Thank you.<br/>myDOCbase Systems Administrator";

                    /// Loop for all the previous stages
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        cmd = new SqlCommand("select a.role_id,b.user_id,c.email from wf_dtl a,user_role b,user_mast c where a.role_id=b.role_id and b.user_id=c.user_id and a.wf_id='" + WFID + "' and a.step_no='" + ds01.Tables[0].Rows[i][1].ToString() + "'", con);
                        SqlDataAdapter adapter03 = new SqlDataAdapter(cmd);
                        ds03.Reset();
                        adapter03.Fill(ds03);
                        if (ds03.Tables[0].Rows.Count > 0)
                        {
                            MailTo = ds03.Tables[0].Rows[0][2].ToString();
                            if (MailTo == "init@init.com")
                            {
                                cmd = new SqlCommand("select email from user_mast where user_id in(select started_by from wf_log_mast where wf_log_id='" + WFLogID + "')", con);
                                SqlDataAdapter adapter003 = new SqlDataAdapter(cmd);
                                ds003.Reset();
                                adapter003.Fill(ds003);
                                MailTo = ds003.Tables[0].Rows[0][0].ToString();
                            }
                            mailing Obj_Mail = new mailing();
                            if (Obj_Mail.SendEmail("", MailFrom, MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                            {

                            }
                        }
                    }
                }
                /// Is there any previous stage or not End

                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText("C:\\LogFolder\\LogFile.txt"))
                {
                    Log("Error in RejectEmail.RejectMail for CompCode: " + CompCode + "; Error: " + ex.Message, w);
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