using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using Alfresco;
using Alfresco.AdministrationWebService;
using Alfresco.RepositoryWebService;
using DMS.UTILITY;

namespace DMS
{
    public partial class reset_pwd : System.Web.UI.Page
    {
        private AdministrationService administrationService;

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
                        if (Session["CanChangePwd"].ToString() == "N")
                        {
                            Response.Redirect("logout.aspx", false);
                        }
                        else
                        {
                            if (Session["UserType"].ToString() == "S") // Super Admin
                            {
                                divMenuSuperAdmin.Visible = true;
                                divMenuAdmin.Visible = false;
                                divMenuNormal.Visible = false;
                            }
                            else if (Session["UserType"].ToString() == "A") // Admin
                            {
                                divMenuSuperAdmin.Visible = false;
                                divMenuAdmin.Visible = true;
                                divMenuNormal.Visible = false;
                            }
                            else
                            {
                                divMenuSuperAdmin.Visible = false;
                                divMenuAdmin.Visible = false;
                                divMenuNormal.Visible = true;
                            }
                            menuGenHome.Visible = true;
                            menuGenSystem.Visible = true;
                        }
                        ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                        DataSet ds01 = new DataSet();
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.UserInfoPassingUserID(Session["UserID"].ToString());
                        //SqlConnection con = Utility.GetConnection();
                        //SqlCommand cmd = null;
                        //con.Open();
                        //DataSet ds0001 = new DataSet();
                        //cmd = new SqlCommand("select user_pwd,PwdStat from user_mast where user_id='" + Session["UserID"].ToString() + "'", con);
                        //SqlDataAdapter adapter0001 = new SqlDataAdapter(cmd);
                        //adapter0001.Fill(ds0001);
                        //Utility.CloseConnection(con);
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            Session["USerPwd"] = ds01.Tables[0].Rows[0][4].ToString();
                            Session["PwdStatus"] = ds01.Tables[0].Rows[0][8].ToString();
                        }
                        lblUser.Text = Session["UserFullName"].ToString();
                        txtUserID.Text = ds01.Tables[0].Rows[0][3].ToString();
                        // If it is for the first time or not start
                        if (Session["PwdStatus"].ToString() == "Changed")
                        {
                            cmdSkipNow.Visible = false;
                            if (Session["UserType"].ToString() == "S") // Super Admin
                            {
                                divMenuSuperAdmin.Visible = true;
                                divMenuAdmin.Visible = false;
                                divMenuNormal.Visible = false;
                            }
                            else if (Session["UserType"].ToString() == "A") // Admin
                            {
                                divMenuSuperAdmin.Visible = false;
                                divMenuAdmin.Visible = true;
                                divMenuNormal.Visible = false;
                            }
                            else
                            {
                                divMenuSuperAdmin.Visible = false;
                                divMenuAdmin.Visible = false;
                                divMenuNormal.Visible = true;
                            }
                        }
                        else
                        {
                            cmdSkipNow.Visible = true;
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        //  If it is for the first time or not End
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
        /// The following function is used to Change Password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Reset_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtOPwd.Text.Trim() != Session["USerPwd"].ToString()) || (txtOPwd.Text.Trim() == "")) //Session["Ticket"]
                {
                    throw new Exception("Invalid old password");
                }
                else if ((txtCPwd.Text.Trim() != txtNPwd.Text.Trim()) || (txtNPwd.Text.Trim() == "") || (txtCPwd.Text.Trim() == ""))
                {
                    throw new Exception("Password mismatch");
                }
                else
                {
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {

                    }
                    else
                    {
                        Response.Redirect("SessionExpired.aspx", false);
                    }



                    /// Alfresco Part Start
                    WebServiceFactory wsFA = new WebServiceFactory();
                    wsFA.UserName = Session["AdmUserID"].ToString();
                    wsFA.Ticket = Session["AdmTicket"].ToString();
                    this.administrationService = wsFA.getAdministrationService();
                    this.administrationService.changePassword(Session["UserID"].ToString(), Session["USerPwd"].ToString(), txtCPwd.Text.Trim());
                    /// Alfresco Part End

                    /// .Net & SQL Server Part Start
                    user_mast_bal OBJ_UserBAL = new user_mast_bal();
                    /// Pass the <UserID><UserPwd> values to <user_mast_bal>

                    OBJ_UserBAL.UserID = Session["UserID"].ToString();
                    OBJ_UserBAL.UserPwd = txtCPwd.Text.Trim();
                    OBJ_UserBAL.PwdStat = "Changed";

                    string result = OBJ_UserBAL.UpdtUserMast();

                    if (Convert.ToInt64(result) > 0)
                    {
                        //throw new Exception("Password reset successfully");
                        Redirect2Dashboard("Password changed successfully", "userhome.aspx");
                    }
                    else
                    {
                        throw new Exception("Password not updated");
                    }
                    /// .Net & SQL Server Part End
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
                //Response.Redirect("userhome.aspx",false);
            }
        }

        private void Redirect2Dashboard(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"userhome.aspx\"</script>";

            Page.Controls.Add(lbl);
        }

        protected void cmdSkipNow_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("userhome.aspx",false);
            }
            catch (Exception ex)
            {

            }
        }

    }
}