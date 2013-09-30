using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DMS.UTILITY;
using System.Data;
using System.Net.Mail;
using System.IO;
using System.Configuration;

namespace DMS
{
    public partial class PasswordRecovery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the session variables blank which are used to set the previous selected path start
            Session["SelectedCabUUID"] = "";
            Session["SelectedDrwUUID"] = "";
            Session["SelectedFldUUID"] = "";
            Session["SelectedDocID"] = "";
            // Set the session variables blank which are used to set the previous selected path end
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.Flush();
        }

        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //SqlConnection con = Utility.GetConnection();
                //SqlCommand cmd = null;
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
                DataSet ds01 = new DataSet();
                DataSet ds001 = new DataSet();

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
                
                //con.Open();
                //cmd = new SqlCommand("select email,user_pwd from user_mast where email='" + txtMail.Text.Trim() + "'", con);
                //SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                //adapter01.Fill(ds01);
                //Utility.CloseConnection(con);
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                ds01.Reset();
                ds01 = ObjClassStoreProc.UserInfoPassingEmailID(txtMail.Text.Trim());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    MailFrom=ds01.Tables[0].Rows[0][3].ToString();
                    MailTo = ds01.Tables[0].Rows[0][3].ToString();
                    MailSub = "Password Recovery";
                    MailMsg = "Hi,<br/><br/>" + "Your Login Password is: " + ds01.Tables[0].Rows[0][4].ToString() + ".<br/><br/><br/>Regards,<br/>Admin.";

                    mailing Obj_Mail = new mailing();
                    if (Obj_Mail.SendEmail("", "admin", MailFrom, MailTo, "", "", MailSub, MailMsg, SenderMail, SenderName, SmtpHost, SmtpPort, CredenUsername, CredenPwd))
                    {
                        MessageBox("Your Password has been Sent in Your Email ID. Please Check Your Email and then Login.");
                    }
                    else
                    {
                        MessageBox("Invalid Email ID. Please Try Again.");
                    }
                }
                else
                {
                    MessageBox("Invalid Email ID. Please Try Again.");
                }
                txtMail.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private void MessageBox(string msg)
        {
            msg.Replace("'", "`");
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

    }
}