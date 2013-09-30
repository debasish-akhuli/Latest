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
    public partial class admin_home : System.Web.UI.Page
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
                            lblUser.Text = Session["UserFullName"].ToString();
                            /// Populate the started workflow List GridView
                            PopStartedWF();
                        }
                        else
                        {
                            Response.Redirect("logout.aspx", true);
                        }
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", true);
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
        /// Populate the GridView to display the Started by the Logged in User Workflow list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopStartedWF()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.PopStartedWF();
                gvStartedWF.DataSource = ds01;
                gvStartedWF.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvStartedWF_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvStartedWF.SelectedRow;
                Label lbWFLogID = (Label)row.FindControl("lbWFLogID");
                string lbWFLogID1 = lbWFLogID.Text.ToString();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds001 = new DataSet();
                ds001 = ObjClassStoreProc.gvStartedWF(lbWFLogID1);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    MsgNodet.Text = "";

                    DataTable dt1 = new DataTable();

                    dt1.Columns.Add("Stage", typeof(int));
                    dt1.Columns.Add("Task", typeof(string));
                    dt1.Columns.Add("Assign Date", typeof(DateTime));
                    dt1.Columns.Add("Due Date", typeof(DateTime));
                    dt1.Columns.Add("Actual Completed Date", typeof(DateTime));

                    var x = (from r in ds001.Tables[0].AsEnumerable()
                             select r["step_no"]).Distinct().ToList();
                    bool flag = true;
                    for (int i = 0; i < x.Count; i++)
                    {
                        DataRow[] r1 = ds001.Tables[0].Select("step_no=" + x[i]);

                        foreach (DataRow dr in r1)
                        {
                            DataRow r = dt1.NewRow();
                            if (flag)
                            {
                                r["Stage"] = dr["step_no"];
                                r["Task"] = dr["task_name"];
                                r["Assign Date"] = dr["assign_dt"];
                                r["Due Date"] = dr["due_dt"];
                                r["Actual Completed Date"] = dr["task_done_dt"];
                                flag = false;
                            }
                            else
                            {
                                r["Task"] = dr["task_name"];
                                r["Assign Date"] = dr["assign_dt"];
                                r["Due Date"] = dr["due_dt"];
                                r["Actual Completed Date"] = dr["task_done_dt"];
                            }
                            dt1.Rows.Add(r);
                        }
                        flag = true;
                    }
                    gv.DataSource = dt1;
                    gv.DataBind();
                    dt1.Clear();
                }
                else
                {
                    gv.DataSource = null;
                    gv.DataBind();
                    MsgNodet.Text = "No Details Found for this Workflow!";
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvStartedWF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStartedWF.PageIndex = e.NewPageIndex;
            PopStartedWF();
        }

    }
}