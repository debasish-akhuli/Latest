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
using Alfresco.RepositoryWebService;
using DMS.UTILITY;

namespace DMS
{
    public partial class ClientManagement : System.Web.UI.Page
    {
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;

        public RepositoryService RepoService
        {
            set { repoService = value; }
        }

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
                            PopulateDropdown();
                            PopulateFields(ddCompany.SelectedValue);
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

        protected void CtrlChanged(Object sender, EventArgs e)
        {
            try
            {
                SetRates(ddSpace.SelectedValue, txtMaxNoOfUsers.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDropdown()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //....Company
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCompAll();
                ddCompany.DataSource = ds01;
                ddCompany.DataTextField = "CompName";
                ddCompany.DataValueField = "CompCode";
                ddCompany.DataBind();
                //....Bill Type
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectBillTypeAll();
                ddSpace.DataSource = ds01;
                ddSpace.DataTextField = "ItemDesc";
                ddSpace.DataValueField = "ItemCode";
                ddSpace.DataBind();
                SetRates(ddSpace.SelectedValue, txtMaxNoOfUsers.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void SetRates(string ItemCode, string UserNo)
        {
            try
            {
                if (txtMaxNoOfUsers.Text.Trim() == "")
                {
                    throw new Exception("Please put a valid number !!");
                }
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                SqlDataAdapter adapter01;
                SqlDataAdapter adapter02;
                DataSet ds01 = new DataSet();
                cmd = new SqlCommand("select ItemDesc,CAST(ROUND(ItemRate, 2) AS DECIMAL(9,2)),SpaceInKB from BillTypeMast where ItemCode='" + ItemCode + "'", con);
                adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    lblSpace.Text = ds01.Tables[0].Rows[0][2].ToString();
                    lblSpaceRate.Text = ds01.Tables[0].Rows[0][1].ToString();
                    lblSpaceRate1.Text = "$" + ds01.Tables[0].Rows[0][1].ToString() + " per month";
                }
                cmd = new SqlCommand("select CAST(ROUND(ItemRate, 2) AS DECIMAL(9,2)) from BillTypeMast where ItemCode='000'", con);
                adapter02 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter02.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    lblPerUserRate.Text = ds01.Tables[0].Rows[0][0].ToString();
                    lblPerUserRate1.Text = "$" + ds01.Tables[0].Rows[0][0].ToString() + " per month";
                }
                lblTotalRate.Text = (Convert.ToDouble(lblSpaceRate.Text) + Convert.ToDouble(lblPerUserRate.Text) * Convert.ToDouble(UserNo)).ToString();
                lblTotalRate1.Text = "$" + (Convert.ToDouble(lblSpaceRate.Text) + Convert.ToDouble(lblPerUserRate.Text) * Convert.ToDouble(UserNo)).ToString() + " per month";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        /// The following function is used to update the existing record in the Database's  <QuickPDFLicenseKey><ServerIP> fields of <ServerConfig> table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
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
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                con.Open();
                #region Check total no of users which are already in the system
                cmd = new SqlCommand("select count(*) as Tot from user_mast where CompCode='" + ddCompany.SelectedValue + "' and user_stat='A'", con);
                int TotUser = Convert.ToInt32(cmd.ExecuteScalar());
                if (TotUser <= Convert.ToInt32(txtMaxNoOfUsers.Text.ToString()))
                {

                }
                else
                {
                    throw new Exception("You need to inactivate some users first !!");
                }

                cmd = new SqlCommand("select UsedSpace as UsedSpace from ServerConfig where CompCode='" + ddCompany.SelectedValue + "'", con);
                double UsedSpace = Convert.ToDouble(cmd.ExecuteScalar());
                if (UsedSpace <= Convert.ToDouble(lblSpace.Text))
                {

                }
                else
                {
                    throw new Exception("This client has already occupied more space than your selected one !!");
                }
                #endregion
                double TotalRate = Convert.ToDouble(lblSpaceRate.Text.Trim()) + (Convert.ToDouble(lblPerUserRate.Text.Trim()) * Convert.ToDouble(txtMaxNoOfUsers.Text.Trim()));
                cmd = new SqlCommand("update ServerConfig set CompName='" + txtCompName.Text.Trim() + "',ContactPersonName='" + txtContactPersonName.Text.Trim() + "',PhoneNo='" + txtContactNo.Text.Trim() + "',Status='" + ddStatus.SelectedValue + "',TotalSpace='" + lblSpace.Text + "',MaxNoOfUsers='" + txtMaxNoOfUsers.Text.Trim() + "',SpaceRate='" + lblSpaceRate.Text.Trim() + "',TotalRate='" + TotalRate + "' where CompCode='" + ddCompany.SelectedValue + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("update ServerConfig set AvailableSpace=TotalSpace-UsedSpace where CompCode='" + ddCompany.SelectedValue + "'", con);
                cmd.ExecuteNonQuery();
                Utility.CloseConnection(con);
                PopulateFields(ddCompany.SelectedValue);
                throw new Exception("Data has been updated !!");
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            PopulateFields(ddCompany.SelectedValue);
        }

        protected void ddSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetRates(ddSpace.SelectedValue, txtMaxNoOfUsers.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateFields(string CompCode)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectServerConfig(CompCode);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    txtCompCode.Text = ds01.Tables[0].Rows[0][6].ToString();
                    txtCompName.Text = ds01.Tables[0].Rows[0][3].ToString();
                    txtContactPersonName.Text = ds01.Tables[0].Rows[0][7].ToString();
                    txtEmailID.Text = ds01.Tables[0].Rows[0][8].ToString();
                    txtContactNo.Text = ds01.Tables[0].Rows[0][9].ToString();
                    ddStatus.SelectedValue = ds01.Tables[0].Rows[0][12].ToString();
                    txtMaxNoOfUsers.Text = ds01.Tables[0].Rows[0][16].ToString();
                    // For Space
                    SqlConnection con = Utility.GetConnection();
                    SqlCommand cmd = null;
                    con.Open();
                    DataSet ds_01 = new DataSet();
                    cmd = new SqlCommand("select * from BillTypeMast where SpaceInKB='" + ds01.Tables[0].Rows[0][13].ToString() + "'", con);
                    SqlDataAdapter adapter_01 = new SqlDataAdapter(cmd);
                    adapter_01.Fill(ds_01);
                    if (ds_01.Tables[0].Rows.Count > 0)
                    {
                        ddSpace.SelectedValue = ds_01.Tables[0].Rows[0][0].ToString();
                    }
                    Utility.CloseConnection(con);
                    SetRates(ddSpace.SelectedValue, txtMaxNoOfUsers.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

    }
}