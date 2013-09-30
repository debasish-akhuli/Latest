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
    public partial class grp_doc : System.Web.UI.Page
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
                        if (Session["UserID"].ToString() == "admin")
                        {
                            divMenuAdmin.Visible = true;
                            divMenuGen.Visible = false;
                        }
                        else
                        {
                            divMenuAdmin.Visible = false;
                            divMenuGen.Visible = true;
                        }
                        Page.Header.DataBind();

                        /// To populate the dropdown
                        PopulateDropdown();
                        PopSearchedList();
                        lblUserName.Text = Session["UserFullName"].ToString();
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
        /// To populate the dropdowns
        /// </summary>
        protected void PopulateDropdown()
        {
            try
            {
                DBClass DBObj = new DBClass();
                //....Group
                DataSet ds4 = new DataSet();
                ds4 = DBObj.DDGroup();
                ddGroup.DataSource = ds4;
                ddGroup.DataTextField = "grp_name";
                ddGroup.DataValueField = "grp_id";
                ddGroup.DataBind();
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
        /// Populate the GridView to display the searched list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopSearchedList()
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                string Str = "";

                Str = "select a.doc_id,a.doc_name,b.doc_type_id,b.doc_type_name,c.fld_name,a.download_path,d.drw_name,e.cab_name from doc_mast a,doc_type_mast b,folder_mast c,drawer_mast d,cabinet_mast e where a.doc_type_id=b.doc_type_id and a.fld_uuid=c.fld_uuid and c.drw_uuid=d.drw_uuid and d.cab_uuid=e.cab_uuid and a.doc_id in(select doc_id from doc_grp where grp_id='" + ddGroup.SelectedValue + "')";
                cmd = new SqlCommand(Str, con);
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                gvSearchedList.DataSource = ds;
                gvSearchedList.DataBind();
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Call the Search Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopSearchedList();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvSearchedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvSearchedList.SelectedRow;
                Label lbDownloadPath = (Label)row.FindControl("lbDownloadPath");
                HyperLink cmdDownload = (HyperLink)row.FindControl("cmdDownload");
                cmdDownload.NavigateUrl = lbDownloadPath.Text;
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PopSearchedList();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

    }
}