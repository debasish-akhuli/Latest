using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS
{
    public class mailing
    {
        /// <summary>
        /// This Method is used to select the Mail Setup Details from Database using <MailSettings> Store Procedure
        /// </summary>
        /// <returns></returns>
        public DataSet MailSettings()
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            cmd = new SqlCommand("MailSettings", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            Utility.CloseConnection(con);
            return ds;
        }

        /// <summary>
        /// This Method is used to send mail
        /// </summary>
        /// <param name="attachedFile"></param>
        /// <param name="fromName"></param>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="ccEmail"></param>
        /// <param name="bccEmail"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="SenderMail"></param>
        /// <param name="SenderName"></param>
        /// <param name="SmtpHost"></param>
        /// <param name="SmtpPort"></param>
        /// <param name="CredenUsername"></param>
        /// <param name="CredenPwd"></param>
        /// <returns></returns>
        public bool SendEmail(string attachedFile, string fromName, string fromEmail, string toEmail, string ccEmail, string bccEmail, string subject, string content, string SenderMail, string SenderName, string SmtpHost, int SmtpPort, string CredenUsername, string CredenPwd)
        {
            try
            {
                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();
                MailAddress fromAddress = new MailAddress(fromEmail, fromName);
                MailAddress senderAddress = new MailAddress(SenderMail, SenderName);
                mailMsg.To.Add(toEmail);
                if (ccEmail != "")
                    mailMsg.CC.Add(ccEmail);

                if (bccEmail != "")
                    mailMsg.Bcc.Add(bccEmail);

                mailMsg.Sender = senderAddress;
                mailMsg.From = fromAddress;
                mailMsg.Subject = subject;
                mailMsg.Body = content;
                mailMsg.IsBodyHtml = true;

                SmtpClient emailClient = new SmtpClient(SmtpHost, SmtpPort);
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(CredenUsername, CredenPwd);
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = SMTPUserInfo;

                if (attachedFile != "")
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(attachedFile);
                    mailMsg.Attachments.Add(attachment);
                }
                emailClient.Send(mailMsg);
                return true;
            }
            catch
            {
                throw;
            }
        }

    }
}