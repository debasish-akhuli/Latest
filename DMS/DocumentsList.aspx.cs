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
    public partial class DocumentsList : System.Web.UI.Page
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
                        if (Session["UserType"].ToString() == "A") // Admin
                        {
                            cmdView.Attributes.Add("onClick", "javascript: return SetDtFields();");
                            lblUser.Text = Session["UserFullName"].ToString();
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

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        protected void cmdExport2Excel_Click(object sender, EventArgs e)
        {
            try
            {
                string DtSelectionRange = "";
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                SqlDataAdapter adapter01;
                DataSet ds01 = new DataSet();

                if (OptFull.Checked == true)
                {
                    cmd = new SqlCommand("select a.doc_name as DocumentName,(b.f_name + ' ' + b.l_name + ' (' + b.email + ')') as UploadedBy,a.upld_dt as UploadedOn,a.CompCode as CompanyCode,a.DocSizeInKB as SizeInKB from doc_mast a,user_mast b where a.CompCode=b.CompCode and a.upld_by=b.user_id and a.CompCode='" + Session["CompCode"].ToString() + "' order by a.upld_dt", con);
                }
                else if (OptTimeSpan.Checked == true)
                {
                    if (hfFromDt.Value != "" && hfToDt.Value != "")
                    {
                        DtSelectionRange = "a.upld_dt between '" + Convert.ToDateTime(hfFromDt.Value) + "' and '" + Convert.ToDateTime(hfToDt.Value) + "'";
                    }
                    else
                    {
                        throw new Exception("Please select the From-Date and To-Date !!");
                    }
                    cmd = new SqlCommand("select a.doc_name as DocumentName,(b.f_name + ' ' + b.l_name + ' (' + b.email + ')') as UploadedBy,a.upld_dt as UploadedOn,a.CompCode as CompanyCode,a.DocSizeInKB as SizeInKB from doc_mast a,user_mast b where a.CompCode=b.CompCode and a.upld_by=b.user_id and a.CompCode='" + Session["CompCode"].ToString() + "' and " + DtSelectionRange + " order by a.upld_dt", con);
                }
                
                adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    DataSetToExcel.Convert(ds01, Session["CompCode"].ToString() + "_" + DateTime.Now.ToShortDateString());
                }
                else
                {
                    throw new Exception("There is no record to export !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        
        protected void cmdView_Click(object sender, EventArgs e)
        {
            try
            {
                string DtSelectionRange = "";
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                SqlDataAdapter adapter01;
                DataSet ds01 = new DataSet();
                string TotDesign = "";

                if (OptFull.Checked == true)
                {
                    lblDtSelection.Text = "";
                    cmd = new SqlCommand("select a.doc_id,a.doc_name,a.uuid,a.fld_uuid,a.upld_by,(b.f_name + ' ' + b.l_name + ' (' + b.email + ')') as FullName,a.upld_dt,a.CompCode,a.DocUpldType,a.DocSizeInKB from doc_mast a,user_mast b where a.CompCode=b.CompCode and a.upld_by=b.user_id and a.CompCode='" + Session["CompCode"].ToString() + "' order by a.upld_dt", con);
                }
                else if (OptTimeSpan.Checked == true)
                {
                    lblDtSelection.Text = "Records are displaying from " + hfFromDt.Value + " to " + hfToDt.Value;
                    if (hfFromDt.Value != "" && hfToDt.Value != "")
                    {
                        DtSelectionRange = "a.upld_dt between '" + Convert.ToDateTime(hfFromDt.Value) + "' and '" + Convert.ToDateTime(hfToDt.Value) + "'";
                    }
                    else
                    {
                        throw new Exception("Please select the From-Date and To-Date !!");
                    }
                    cmd = new SqlCommand("select a.doc_id,a.doc_name,a.uuid,a.fld_uuid,a.upld_by,(b.f_name + ' ' + b.l_name + ' (' + b.email + ')') as FullName,a.upld_dt,a.CompCode,a.DocUpldType,a.DocSizeInKB from doc_mast a,user_mast b where a.CompCode=b.CompCode and a.upld_by=b.user_id and a.CompCode='" + Session["CompCode"].ToString() + "' and " + DtSelectionRange + " order by a.upld_dt", con);
                }
                
                adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    TotDesign = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" width=\"98%\" style=\"margin:0 auto;\"><tr><td style=\"padding:2px 5px 2px 5px; font-weight:bold;\"><div>Sl No</div></td><td style=\"padding:2px 5px 2px 5px; font-weight:bold;\"><div>Document Name</div></td><td style=\"padding:2px 5px 2px 5px; font-weight:bold;\"><div>Uploaded By</div></td><td style=\"padding:2px 5px 2px 5px; font-weight:bold;\"><div>Uploaded On</div></td><td style=\"padding:2px 5px 2px 5px; font-weight:bold;\"><div>Size (KB)</div></td></tr>";
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        TotDesign += "<tr>";
                        TotDesign += "<td style=\"padding:2px 5px 2px 5px;\">" + (i+1).ToString() + "</td>";
                        TotDesign += "<td style=\"padding:2px 5px 2px 5px;\">" + ds01.Tables[0].Rows[i][1].ToString() + "</td>";
                        TotDesign += "<td style=\"padding:2px 5px 2px 5px;\">" + ds01.Tables[0].Rows[i][5].ToString() + "</td>";
                        TotDesign += "<td style=\"padding:2px 5px 2px 5px;\">" + ds01.Tables[0].Rows[i][6].ToString() + "</td>";
                        TotDesign += "<td style=\"padding:2px 5px 2px 5px;\">" + ds01.Tables[0].Rows[i][9].ToString() + "</td>";
                        TotDesign += "</tr>";
                    }
                }
                else
                {
                    TotDesign = "";
                    lblDtSelection.Text = "";
                    divDocList.InnerHtml = TotDesign;
                    throw new Exception("There is no record to display !!");
                }
                if (TotDesign == "")
                {

                }
                else
                {
                    TotDesign += "</table>";
                }
                divDocList.InnerHtml = TotDesign;
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void OptFull_CheckedChanged(object sender, EventArgs e)
        {
            hfFromDt.Value = "";
            hfToDt.Value = "";
            divTimeSpan.Visible = false;
        }

        protected void OptTimeSpan_CheckedChanged(object sender, EventArgs e)
        {
            divTimeSpan.Visible = true;
        }

    }
}