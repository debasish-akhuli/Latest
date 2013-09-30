using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;

namespace DMS
{
    public partial class mail_setup : System.Web.UI.Page
    {
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
                            PopulateFields();
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            Response.Redirect("logout.aspx", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                }
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

        /// <summary>
        /// The following function is used to update the existing record in the Database's  <sender_mail><sender_name><smtp_host><smtp_port><creden_username><creden_pwd> fields of <mail_setup> table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddMaster_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] != null && Session["Ticket"] != null)
                {

                }
                else
                {
                    Response.Redirect("SessionExpired.aspx", false);
                }
                mail_setup_bal OBJ_MailSetupBAL = new mail_setup_bal();
                /// Pass the values to <mail_setup_bal>

                OBJ_MailSetupBAL.SenderMail = txtSenderMail.Text.Trim();
                OBJ_MailSetupBAL.SenderName = txtSenderName.Text.Trim();
                OBJ_MailSetupBAL.SMTPHost = txtSMTPHost.Text.Trim();
                OBJ_MailSetupBAL.SMTPPort = Convert.ToInt64(txtSMTPPort.Text.Trim());
                OBJ_MailSetupBAL.CredenUName = txtCredenUName.Text.Trim();
                OBJ_MailSetupBAL.CredenPwd = txtCredenPwd.Text.Trim();

                string result = OBJ_MailSetupBAL.UpdateMailSetup();

                if (Convert.ToInt64(result) > 0)
                {
                    throw new Exception("Record updated successfully");
                }
                else
                {
                    throw new Exception("Record not updated");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To Display the existing record
        /// </summary>
        protected void PopulateFields()
        {
            try
            {
                DBClass DBObj = new DBClass();
                DataSet ds1 = new DataSet();
                ds1 = DBObj.MailSetup();
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    txtSenderMail.Text = ds1.Tables[0].Rows[0][0].ToString();
                    txtSenderName.Text = ds1.Tables[0].Rows[0][1].ToString();
                    txtSMTPHost.Text = ds1.Tables[0].Rows[0][2].ToString();
                    txtSMTPPort.Text = ds1.Tables[0].Rows[0][3].ToString();
                    txtCredenUName.Text = ds1.Tables[0].Rows[0][4].ToString();
                    txtCredenPwd.Text = ds1.Tables[0].Rows[0][5].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }


    }
}