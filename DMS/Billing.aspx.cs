using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS
{
    public partial class Billing : System.Web.UI.Page
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
                        if (Session["UserType"].ToString() == "A") // Super Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopStat();
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
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

        protected void PopStat()
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                SqlDataAdapter adapter01;
                DataSet ds01 = new DataSet();

                cmd = new SqlCommand("select CompCode,CompName,TotalSpace,UsedSpace,AvailableSpace,MaxNoOfUsers,SpaceRate,UserRate,TotalRate,CreationDate from ServerConfig where CompCode='" + Session["CompCode"].ToString() + "'", con);
                adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    lblTotalSpace.Text = Math.Round((Convert.ToDouble(ds01.Tables[0].Rows[0][2].ToString()) / 1024) / 1024, 2) + " GB / " + Math.Round(Convert.ToDouble(ds01.Tables[0].Rows[0][2].ToString()), 2) + " KB";
                    lblUsedSpace.Text = Math.Round((Convert.ToDouble(ds01.Tables[0].Rows[0][3].ToString()) / 1024) / 1024, 2) + " GB / " + Math.Round(Convert.ToDouble(ds01.Tables[0].Rows[0][3].ToString()), 2) + " KB";
                    lblAvailableSpace.Text = Math.Round((Convert.ToDouble(ds01.Tables[0].Rows[0][4].ToString()) / 1024) / 1024, 2) + " GB / " + Math.Round(Convert.ToDouble(ds01.Tables[0].Rows[0][4].ToString()), 2) + " KB";
                    lblMaxUsers.Text = ds01.Tables[0].Rows[0][5].ToString();
                    cmd = new SqlCommand("select count(*) as Tot from user_mast where CompCode='" + Session["CompCode"].ToString() + "' and user_stat='A'", con);
                    int ExistingUsers = Convert.ToInt32(cmd.ExecuteScalar());
                    lblExistingUsers.Text = ExistingUsers.ToString();
                    lblSpaceRate.Text = "$" + ds01.Tables[0].Rows[0][6].ToString() + " per Month";
                    lblUserRate.Text = "$" + ds01.Tables[0].Rows[0][7].ToString() + " per User per Month";
                    lblTotalRate.Text = "$" + ds01.Tables[0].Rows[0][8].ToString() + " per Month";
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }


    }
}