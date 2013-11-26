using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using DMS.UTILITY;
using QuickPDFDLL0813;
using System.Net;
using System.Configuration;
using DMS.Actions;
using DynamicFormula;

namespace DMS
{
    public partial class eFormOpening : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Header.DataBind();
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
                            if (Session["CanChangePwd"].ToString() == "N")
                            {
                                menuGenHome.Visible = false;
                                menuGenSystem.Visible = false;
                            }
                            else
                            {
                                menuGenHome.Visible = true;
                                menuGenSystem.Visible = true;
                            }
                        }
                        lblUser.Text = Session["UserFullName"].ToString();
                        PopulateDropdown();
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                }
                PopulateFormDetails(Request.QueryString["DocID"].ToString());
                PopulateDesign();
                ddDocType.SelectedValue = Session["ddDocTypeSel"].ToString();
                DocTypeSettings();
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
                //....Department 
                ds01.Reset();
                ds01 = ObjClassStoreProc.DeptAllBasedOnCompCode(Session["CompCode"].ToString());
                ddDept.DataSource = ds01;
                ddDept.DataTextField = "dept_name";
                ddDept.DataValueField = "dept_id";
                ddDept.DataBind();

                //....Doc Type
                ds01.Reset();
                ds01 = ObjClassStoreProc.eFormDocTypeCompBased(Session["CompCode"].ToString());
                ddDocType.DataSource = ds01;
                ddDocType.DataTextField = "doc_type_name";
                ddDocType.DataValueField = "doc_type_id";
                ddDocType.DataBind();

                // Fetch the Location
                FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
                DocTypeSettings();
                DeptSettings();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void FetchLocation(string DeptID, string DocTypeID)
        {
            try
            {
                divUpldLoc.Visible = true;

                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.FetchUploadedLocation(DeptID, DocTypeID, Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    hfUUID.Value = ds01.Tables[0].Rows[0][0].ToString();
                    lblLocation.Text = ds01.Tables[0].Rows[0][3].ToString() + " >> " + ds01.Tables[0].Rows[0][2].ToString() + " >> " + ds01.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    lblLocation.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void DocTypeSettings()
        {
            try
            {
                FetchWFL(ddDept.SelectedValue, ddDocType.SelectedValue);
                FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void DeptSettings()
        {
            try
            {
                FetchWFL(ddDept.SelectedValue, ddDocType.SelectedValue);
                FetchLocation(ddDept.SelectedValue, ddDocType.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void FetchWFL(string DeptID, string DocTypeID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.FetchUploadedLocation(DeptID, DocTypeID, Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    hfWFLID.Value = ds01.Tables[0].Rows[0][4].ToString();
                    lblWFL.Text = ds01.Tables[0].Rows[0][5].ToString();
                }
                else
                {
                    lblWFL.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void cmdViewWFL_Click(object sender, EventArgs e)
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
                string lbWfid1 = hfWFLID.Value;
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01 = ObjClassStoreProc.DisplayWFDtl(lbWfid1, Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    MsgNodet.Text = "";
                    Lblwf.Text = "Workflow Name : ";
                    Lbldept.Text = "Department Name : ";
                    Lbldoc.Text = "Document Type : ";
                    LblFld.Text = "Folder Name : ";
                    lblWfname.Text = ds01.Tables[0].Rows[0]["wf_name"].ToString();
                    lblWfdept.Text = ds01.Tables[0].Rows[0]["dept_name"].ToString();
                    lblWfDoctype.Text = ds01.Tables[0].Rows[0]["doc_type_name"].ToString();
                    lblWfFld.Text = ds01.Tables[0].Rows[0]["fld_name"].ToString();

                    DataTable dt1 = new DataTable();

                    dt1.Columns.Add("Stage", typeof(int));
                    dt1.Columns.Add("Role", typeof(string));

                    var x = (from r in ds01.Tables[0].AsEnumerable()
                             select r["step_no"]).Distinct().ToList();
                    bool flag = true;
                    for (int i = 0; i < x.Count; i++)
                    {
                        DataRow[] r1 = ds01.Tables[0].Select("step_no=" + x[i]);

                        foreach (DataRow dr in r1)
                        {
                            DataRow r = dt1.NewRow();
                            if (flag)
                            {
                                r["Stage"] = dr["step_no"];
                                r["Role"] = dr["role_name"];
                                flag = false;
                                dt1.Rows.Add(r);
                            }
                            else
                            {

                            }
                        }
                        flag = true;
                    }
                    gv.DataSource = dt1;
                    gv.DataBind();
                    dt1.Clear();
                }
                else
                {
                    Lblwf.Text = "";
                    Lbldept.Text = "";
                    Lbldoc.Text = "";
                    LblFld.Text = "";
                    lblWfname.Text = "";
                    lblWfdept.Text = "";
                    lblWfDoctype.Text = "";
                    lblWfFld.Text = "";
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

        protected void ddDocType_SelectedIndexChanged(object sender, EventArgs e)
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
                DocTypeSettings();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDept_SelectedIndexChanged(object sender, EventArgs e)
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
                DeptSettings();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        public DataTable CreateDT4Fields()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SerialNo", typeof(Int32));
            dt.Columns.Add("LabelID", typeof(string));
            dt.Columns.Add("ControlID", typeof(string));
            dt.Columns.Add("ControlType", typeof(string));
            dt.Columns.Add("DataType", typeof(string));
            dt.Columns.Add("MaxLength", typeof(Int32));
            dt.Columns.Add("MinVal", typeof(Int32));
            dt.Columns.Add("MaxVal", typeof(Int32));
            dt.Columns.Add("Formula", typeof(string));
            dt.Columns.Add("LabelDesc", typeof(string));
            dt.Columns.Add("Controls4Formula", typeof(string));
            return dt;
        }

        public DataRow AddNewInDTFields(DataRow r, int SerialNo, string LabelID, string ControlID, string ControlType, string DataType, int MaxLength, int MinVal, int MaxVal, string Formula, string LabelDesc, string Controls4Formula)
        {
            r["SerialNo"] = SerialNo;
            r["LabelID"] = LabelID;
            r["ControlID"] = ControlID;
            r["ControlType"] = ControlType;
            r["DataType"] = DataType;
            r["MaxLength"] = MaxLength;
            r["MinVal"] = MinVal;
            r["MaxVal"] = MaxVal;
            r["Formula"] = Formula;
            r["LabelDesc"] = LabelDesc;
            r["Controls4Formula"] = Controls4Formula;
            return r;
        }

        protected void PopulateDesign()
        {
            try
            {
                if (hfControls != null)
                {
                    ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                    DataSet ds01 = new DataSet();
                    DataTable dtFields = null;
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt32(Request.QueryString["DocID"].ToString()), Session["CompCode"].ToString());
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        string mDocUUID = ds01.Tables[0].Rows[0][4].ToString();
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.DocMetaValueDetails(mDocUUID);
                    }

                    dtFields = (DataTable)Session["dtFields"];
                    for (int i = 0; i < dtFields.Rows.Count; i++)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc = new TableCell();
                        TableCell tc1 = new TableCell();

                        Label lbl = new Label();
                        lbl.ID = dtFields.Rows[i]["LabelID"].ToString();
                        lbl.Text = dtFields.Rows[i]["LabelDesc"].ToString();

                        tr = new TableRow();
                        tc = new TableCell();
                        tc.Controls.Add(lbl);

                        if (dtFields.Rows[i]["ControlType"].ToString() == "T")
                        {
                            tc1 = new TableCell();

                            TextBox tb = new TextBox();
                            tb.ID = dtFields.Rows[i]["ControlID"].ToString();
                            tb.Width = 200;
                            if (dtFields.Rows[i]["DataType"].ToString() == "Text")
                            {
                                tb.MaxLength = Convert.ToInt32(dtFields.Rows[i]["MaxLength"].ToString());
                            }
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                tb.Text = ds01.Tables[0].Rows[0][i + 4].ToString();
                            }
                            tc1.Controls.Add(tb);
                        }
                        else if (dtFields.Rows[i]["ControlType"].ToString() == "D")
                        {
                            tc1 = new TableCell();
                            DropDownList dpl = new DropDownList();
                            dpl.ID = dtFields.Rows[i]["ControlID"].ToString();

                            // Finding the corresponding SessionID
                            string totStr = dtFields.Rows[i]["ControlID"].ToString();
                            int totLength = totStr.Length;
                            int DDSessionID = Convert.ToInt16(totStr.Substring(totStr.LastIndexOf('_') + 1, (totLength - totStr.LastIndexOf('_') - 1)));

                            string[] DDItems = Session[(DDSessionID + 1).ToString()].ToString().Split('~');
                            for (int j = 0; j < DDItems.Length; j++)
                            {
                                dpl.Items.Add(DDItems[j]);
                            }
                            if (ds01.Tables[0].Rows.Count > 0)
                            {
                                dpl.SelectedItem.Text = ds01.Tables[0].Rows[0][i + 4].ToString();
                            }
                            //if (hfButtonStatus.Value == "")
                            //{
                            //    ds01.Reset();
                            //    ds01 = ObjClassStoreProc.DropdownItems(Request.QueryString["FormID"].ToString(), dtFields.Rows[i]["ControlID"].ToString());
                            //    if (ds01.Tables[0].Rows.Count > 0)
                            //    {
                            //        for (int j = 0; j < ds01.Tables[0].Rows.Count; j++)
                            //        {
                            //            dpl.Items.Add(ds01.Tables[0].Rows[j][4].ToString());
                            //        }
                            //    }
                            //}
                            //else
                            //{

                            //}
                            tc1.Controls.Add(dpl);
                        }
                        tr.Cells.Add(tc);
                        tr.Cells.Add(tc1);
                        tblMain.Rows.Add(tr);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void PopulateFormDetails(string DocID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataTable dtFields = null;
                string DocUUID="";
                ds01.Reset();
                ds01 = ObjClassStoreProc.DocDetailsSelectPassingDocID(Convert.ToInt16(DocID), Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    //txtDocName.Text = ds01.Tables[0].Rows[0][1].ToString();
                    Session["ddDocTypeSel"] = ds01.Tables[0].Rows[0][2].ToString();
                    DocUUID = ds01.Tables[0].Rows[0][4].ToString();
                    ds02.Reset();
                    ds02 = ObjClassStoreProc.DocMetaValueDetails(DocUUID);
                    if (ds02.Tables[0].Rows.Count > 0)
                    {
                        Session["eFormTemplateUUID"] = ds02.Tables[0].Rows[0][1].ToString();
                    }
                    else
                    {
                        Session["eFormTemplateUUID"] = DocUUID;
                    }
                    ds01.Reset();
                    ds01 = ObjClassStoreProc.ControlDetails(Session["eFormTemplateUUID"].ToString());
                    if (ds01.Tables[0].Rows.Count > 0)
                    {
                        dtFields = CreateDT4Fields();
                        for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                        {
                            DataRow r = dtFields.NewRow();
                            dtFields.Rows.Add(AddNewInDTFields(r, Convert.ToInt32(ds01.Tables[0].Rows[i][1].ToString()), ds01.Tables[0].Rows[i][2].ToString(), ds01.Tables[0].Rows[i][3].ToString(), ds01.Tables[0].Rows[i][4].ToString(), ds01.Tables[0].Rows[i][6].ToString(), Convert.ToInt32(ds01.Tables[0].Rows[i][7].ToString()), Convert.ToInt32(ds01.Tables[0].Rows[i][8].ToString()), Convert.ToInt32(ds01.Tables[0].Rows[i][9].ToString()), ds01.Tables[0].Rows[i][10].ToString(), ds01.Tables[0].Rows[i][5].ToString(), ds01.Tables[0].Rows[i][11].ToString()));
                            if (ds01.Tables[0].Rows[i][4].ToString() == "D")
                            {
                                string totStr = ds01.Tables[0].Rows[i][3].ToString();
                                int totLength = totStr.Length;
                                int DDSessionID = Convert.ToInt16(totStr.Substring(totStr.LastIndexOf('_') + 1, (totLength - totStr.LastIndexOf('_') - 1)));

                                ds02.Reset();
                                ds02 = ObjClassStoreProc.DropdownItems(Session["eFormTemplateUUID"].ToString(), dtFields.Rows[i]["ControlID"].ToString());
                                if (ds02.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds02.Tables[0].Rows.Count; j++)
                                    {
                                        if (Session[(DDSessionID + 1).ToString()] != null)
                                        {
                                            Session[(DDSessionID + 1).ToString()] += "~" + ds02.Tables[0].Rows[j][4].ToString();
                                        }
                                        else
                                        {
                                            Session[(DDSessionID + 1).ToString()] += ds02.Tables[0].Rows[j][4].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No Control found in this Form !!");
                    }
                }
                else
                {
                    throw new Exception("No record found !!");
                }

                Session["dtFields"] = dtFields;
                //gvFields.DataSource = dtFields;
                //gvFields.DataBind();
                tblMain.Controls.Clear();

                for (int i = 0; i < dtFields.Rows.Count; i++)
                {
                    if (dtFields.Rows[i]["ControlType"].ToString() == "T")
                    {
                        hfControls.Value += "T";
                    }
                    else if (dtFields.Rows[i]["ControlType"].ToString() == "D")
                    {
                        hfControls.Value += "D";
                    }
                }

                ////Populate ddPosition DropDown
                //ddPosition.Items.Clear();
                //dtFields = (DataTable)Session["dtFields"];
                //ddPosition.Items.Add("At the Beginning");
                //for (int i = 0; i < dtFields.Rows.Count; i++)
                //{
                //    ddPosition.Items.Add("After " + dtFields.Rows[i]["LabelDesc"].ToString());
                //}
                //ddPosition.SelectedIndex = dtFields.Rows.Count;

                ////Populate ddLabelDesc Dropdown
                //ddLabelDesc.Items.Clear();
                //ddLabelDesc.DataSource = dtFields;
                //ddLabelDesc.DataTextField = "LabelDesc";
                //ddLabelDesc.DataValueField = "ControlID";
                //ddLabelDesc.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataTable dtFields = null;
                dtFields = (DataTable)Session["dtFields"];

                double result1;
                bool IsDouble = false;
                string TotStr = "";
                for (int i = 0; i < dtFields.Rows.Count; i++)
                {
                    if (dtFields.Rows[i]["ControlType"].ToString() == "T")
                    {
                        if (dtFields.Rows[i]["DataType"].ToString() == "Numeric")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[i]["ControlID"].ToString()); //(TextBox)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                            if (tb != null)
                            {
                                TotStr += tb.ID + "=" + tb.Text + "~";
                                IsDouble = double.TryParse(tb.Text.Trim(), out result1);
                                if (IsDouble == false)
                                {
                                    throw new Exception("Please enter numeric value in " + dtFields.Rows[i]["LabelDesc"].ToString() + " Field !!");
                                }
                                else
                                {
                                    if (Convert.ToDouble(tb.Text.Trim()) < Convert.ToDouble(dtFields.Rows[i]["MinVal"].ToString()))
                                    {
                                        throw new Exception("Please enter proper value in " + dtFields.Rows[i]["LabelDesc"].ToString() + " Field !!");
                                    }
                                    else if (Convert.ToDouble(tb.Text.Trim()) > Convert.ToDouble(dtFields.Rows[i]["MaxVal"].ToString()))
                                    {
                                        throw new Exception("Please enter proper value in " + dtFields.Rows[i]["LabelDesc"].ToString() + " Field !!");
                                    }
                                }
                            }
                        }
                        else if (dtFields.Rows[i]["DataType"].ToString() == "Formula")
                        {
                            TextBox tb = (TextBox)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                            if (tb != null)
                            {
                                #region Calculating the Formula
                                string Controls4Formula = dtFields.Rows[i]["Controls4Formula"].ToString();
                                string Formula = dtFields.Rows[i]["Formula"].ToString();
                                string[] Controls = Controls4Formula.ToString().Split(',');
                                for (int j = 0; j < Controls.Length; j++)
                                {
                                    if (Controls[j].ToString() != "")
                                    {
                                        TextBox tb1 = (TextBox)tblMain.FindControl(Controls[j].ToString());
                                        Formula = Formula.Replace(Controls[j].ToString(), tb1.Text.Trim());
                                    }
                                }
                                Formula f = new Formula(Formula);
                                tb.Text = f.evaluate(new System.Collections.Hashtable()).ToString();
                                #endregion
                                TotStr += tb.ID + "=" + tb.Text + "~";
                            }
                        }
                        else
                        {
                            TextBox tb = (TextBox)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                            if (tb != null)
                            {
                                TotStr += tb.ID + "=" + tb.Text + "~";
                            }
                        }
                    }
                    else if (dtFields.Rows[i]["ControlType"].ToString() == "D")
                    {
                        DropDownList ddL = (DropDownList)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                        if (ddL != null)
                        {
                            TotStr += ddL.ID + "=" + ddL.Text + "~";
                        }
                    }
                }
                //throw new Exception(TotStr);
                
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                #region Update Metadata Fields
                con.Open();
                DataSet ds0003 = new DataSet();
                cmd = new SqlCommand("select * from doc_type_mast where doc_type_id='" + ddDocType.SelectedValue + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                SqlDataAdapter adapter0003 = new SqlDataAdapter(cmd);
                adapter0003.Fill(ds0003);
                if (ds0003.Tables[0].Rows.Count > 0)
                {
                    // Here 12 to 21 are the field nos in the database
                    // For Tag1
                    if (ds0003.Tables[0].Rows[0][12].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][12].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][12].ToString()) - 1]["ControlID"].ToString());
                            txtTag1.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][12].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][12].ToString()) - 1]["ControlID"].ToString());
                            txtTag1.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag1.Text = "";
                    }

                    // For Tag2
                    if (ds0003.Tables[0].Rows[0][13].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][13].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][13].ToString()) - 1]["ControlID"].ToString());
                            txtTag2.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][13].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][13].ToString()) - 1]["ControlID"].ToString());
                            txtTag2.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag2.Text = "";
                    }

                    // For Tag3
                    if (ds0003.Tables[0].Rows[0][14].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][14].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][14].ToString()) - 1]["ControlID"].ToString());
                            txtTag3.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][14].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][14].ToString()) - 1]["ControlID"].ToString());
                            txtTag3.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag3.Text = "";
                    }

                    // For Tag4
                    if (ds0003.Tables[0].Rows[0][15].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][15].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][15].ToString()) - 1]["ControlID"].ToString());
                            txtTag4.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][15].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][15].ToString()) - 1]["ControlID"].ToString());
                            txtTag4.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag4.Text = "";
                    }

                    // For Tag5
                    if (ds0003.Tables[0].Rows[0][16].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][16].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][16].ToString()) - 1]["ControlID"].ToString());
                            txtTag5.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][16].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][16].ToString()) - 1]["ControlID"].ToString());
                            txtTag5.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag5.Text = "";
                    }

                    // For Tag6
                    if (ds0003.Tables[0].Rows[0][17].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][17].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][17].ToString()) - 1]["ControlID"].ToString());
                            txtTag6.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][17].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][17].ToString()) - 1]["ControlID"].ToString());
                            txtTag6.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag6.Text = "";
                    }

                    // For Tag7
                    if (ds0003.Tables[0].Rows[0][18].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][18].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][18].ToString()) - 1]["ControlID"].ToString());
                            txtTag7.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][18].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][18].ToString()) - 1]["ControlID"].ToString());
                            txtTag7.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag7.Text = "";
                    }

                    // For Tag8
                    if (ds0003.Tables[0].Rows[0][19].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][19].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][19].ToString()) - 1]["ControlID"].ToString());
                            txtTag8.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][19].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][19].ToString()) - 1]["ControlID"].ToString());
                            txtTag8.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag8.Text = "";
                    }

                    // For Tag9
                    if (ds0003.Tables[0].Rows[0][20].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][20].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][20].ToString()) - 1]["ControlID"].ToString());
                            txtTag9.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][20].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][20].ToString()) - 1]["ControlID"].ToString());
                            txtTag9.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag9.Text = "";
                    }

                    // For Tag10
                    if (ds0003.Tables[0].Rows[0][21].ToString() != "0")
                    {
                        if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][21].ToString()) - 1]["ControlType"].ToString() == "T")
                        {
                            TextBox tb = (TextBox)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][21].ToString()) - 1]["ControlID"].ToString());
                            txtTag10.Text = tb.Text;
                        }
                        else if (dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][21].ToString()) - 1]["ControlType"].ToString() == "D")
                        {
                            DropDownList ddL = (DropDownList)tblMain.FindControl(dtFields.Rows[Convert.ToInt16(ds0003.Tables[0].Rows[0][21].ToString()) - 1]["ControlID"].ToString());
                            txtTag10.Text = ddL.Text;
                        }
                    }
                    else
                    {
                        txtTag10.Text = "";
                    }

                }
                con.Close();
                #endregion

                #region Saving the values
                string TotalStr = TotStr;
                int StartPos = 0;
                int EndPos = 0;
                string NthVal = "";
                string DBFldName = "";
                string[] TermValues;
                TermValues = new string[dtFields.Rows.Count];

                double FileSize = 0;
                string result = "";
                string AutoGenID = Guid.NewGuid().ToString();

                result = ObjClassStoreProc.ExistDoc(txtDocName.Text.Trim(), hfUUID.Value, Session["CompCode"].ToString());
                if (Convert.ToInt32(result) == -1)
                {
                    throw new Exception("This Form is already exists in this Folder !!");
                }
                else
                {
                    result = ObjClassStoreProc.InsertDocMast(txtDocName.Text.Trim(), txtDocDesc.Text.Trim(), hfUUID.Value, ddDocType.SelectedValue, ddDept.SelectedValue, Session["UserID"].ToString(), DateTime.Now, txtTag1.Text.Trim(), txtTag2.Text.Trim(), txtTag3.Text.Trim(), txtTag4.Text.Trim(), txtTag5.Text.Trim(), txtTag6.Text.Trim(), txtTag7.Text.Trim(), txtTag8.Text.Trim(), txtTag9.Text.Trim(), txtTag10.Text.Trim(),"", AutoGenID, "", "N", Session["CompCode"].ToString(), FileSize);
                    if (Convert.ToInt32(result) == -1)
                    {
                        throw new Exception("This Form is already exists in this Folder !!");
                    }
                    else
                    {
                        Session["DocId"] = result;
                        #region User Rights Set
                        UserRights RightsObj = new UserRights();
                        DataSet dsPerm = new DataSet();
                        dsPerm.Reset();
                        dsPerm = RightsObj.FetchPermission(hfUUID.Value, Session["CompCode"].ToString());
                        if (dsPerm.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                            {
                                RightsObj.InsertPermissionSingleData(AutoGenID, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                            }
                        }
                        #endregion
                    }
                }

                result = ObjClassStoreProc.DocMetaValueInsert(AutoGenID, Session["eFormTemplateUUID"].ToString(), Session["CompCode"].ToString());
                con.Open();
                for (int i = 0; i < TotalStr.Length; i++)
                {
                    StartPos = TotalStr.IndexOf('=');
                    EndPos = TotalStr.IndexOf('~');
                    NthVal = TotalStr.Substring(StartPos + 1, EndPos - StartPos - 1);
                    TermValues[i] = NthVal;
                    TotalStr = TotalStr.Substring(EndPos + 1, TotalStr.Length - EndPos - 1);

                    DBFldName = "Tag" + (i + 1).ToString();
                    cmd = new SqlCommand("update DocMetaValue set " + DBFldName + "='" + TermValues[i].ToString() + "' where uuid='" + AutoGenID + "'", con);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                //Utility.CloseConnection(con);
                #endregion

                #region Start Default Workflow
                result = ObjClassStoreProc.DefinedWF(ddDocType.SelectedValue, ddDept.SelectedValue, Session["CompCode"].ToString());
                if (Convert.ToInt32(result) == -111)
                {
                    Redirect2Home("Document uploaded successfully!", "home.aspx");
                }
                else
                {
                    Session["WFId"] = result;
                    result = ObjClassStoreProc.StartDefaultWF(ddDocType.SelectedValue, ddDept.SelectedValue, DateTime.Now, Convert.ToInt16(Session["DocId"]), Convert.ToInt16(Session["WFId"]), Session["UserID"].ToString(), Session["CompCode"].ToString());
                    if (result != "")
                    {
                        Session["WFLogId"] = result;
                        DataSet ds01 = new DataSet();
                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectWFDtls(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                            {
                                result = ObjClassStoreProc.StartDefaultWFLogDtl(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), DateTime.Now, ds01.Tables[0].Rows[i]["duration"].ToString(), DateTime.Now, Session["CompCode"].ToString());
                                #region Insert the Log for versioning of the file Start
                                result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), Session["eFormTemplateUUID"].ToString(), AutoGenID, Session["CompCode"].ToString());
                                cmd = new SqlCommand("update WFDocVersion set NewDocUUID='" + AutoGenID + "' where WFLogID='" + Session["WFLogId"].ToString() + "' and StepNo='1'", con);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + AutoGenID + "' where WFLogID='" + Session["WFLogId"].ToString() + "' and StepNo>'1'", con);
                                cmd.ExecuteNonQuery();

                                DataSet dsV01 = new DataSet();
                                cmd = new SqlCommand("select * from WFDoc where WFLogID='" + Session["WFLogId"].ToString() + "' and DocUUID='" + AutoGenID + "'", con);
                                SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                adapterV01.Fill(dsV01);
                                if (dsV01.Tables[0].Rows.Count > 0)
                                {

                                }
                                else
                                {
                                    cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + Session["WFLogId"].ToString() + "','" + AutoGenID + "','" + Session["CompCode"].ToString() + "')", con);
                                    cmd.ExecuteNonQuery();
                                }
                                con.Close();

                                //if (Request.QueryString["NewFile"] != null)
                                //{
                                //    result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), Session["TemplateUUID"].ToString(), "", Session["CompCode"].ToString());
                                //    result = ObjClassStoreProc.WFDocVersionUpdate(Session["WFLogId"].ToString(), 1, Session["TemplateUUID"].ToString(), Session["TemplateUUID"].ToString(), Session["CompCode"].ToString());
                                //    cmd = new SqlCommand("update WFDocVersion set ActualDocUUID='" + Session["TemplateUUID"].ToString() + "' where WFLogID='" + Session["WFLogId"].ToString() + "' and StepNo>'1'", con);
                                //    cmd.ExecuteNonQuery();

                                //    DataSet dsV01 = new DataSet();
                                //    cmd = new SqlCommand("select * from WFDoc where WFLogID='" + Session["WFLogId"].ToString() + "' and DocUUID='" + newContentNode.uuid + "'", con);
                                //    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                //    adapterV01.Fill(dsV01);
                                //    if (dsV01.Tables[0].Rows.Count > 0)
                                //    {

                                //    }
                                //    else
                                //    {
                                //        cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + Session["WFLogId"].ToString() + "','" + newContentNode.uuid + "','" + Session["CompCode"].ToString() + "')", con);
                                //        cmd.ExecuteNonQuery();
                                //    }
                                //}
                                //else
                                //{
                                //    result = ObjClassStoreProc.WFDocVersionInsert(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), newContentNode.uuid, "", Session["CompCode"].ToString());
                                //    result = ObjClassStoreProc.WFDocVersionUpdate(Session["WFLogId"].ToString(), 1, newContentNode.uuid, newContentNode.uuid, Session["CompCode"].ToString());
                                //    DataSet dsV01 = new DataSet();
                                //    cmd = new SqlCommand("select * from WFDoc where WFLogID='" + Session["WFLogId"].ToString() + "' and DocUUID='" + newContentNode.uuid + "'", con);
                                //    SqlDataAdapter adapterV01 = new SqlDataAdapter(cmd);
                                //    adapterV01.Fill(dsV01);
                                //    if (dsV01.Tables[0].Rows.Count > 0)
                                //    {

                                //    }
                                //    else
                                //    {
                                //        cmd = new SqlCommand("insert into WFDoc(WFLogID,DocUUID,CompCode) values('" + Session["WFLogId"].ToString() + "','" + newContentNode.uuid + "','" + Session["CompCode"].ToString() + "')", con);
                                //        cmd.ExecuteNonQuery();
                                //    }
                                //}
                                #endregion
                            }
                        }


                        ds01.Reset();
                        ds01 = ObjClassStoreProc.SelectWFTasks(Convert.ToInt16(Session["WFId"]), Session["CompCode"].ToString());
                        if (ds01.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                            {
                                result = ObjClassStoreProc.StartDefaultWFLogTask(Session["WFLogId"].ToString(), Convert.ToInt16(ds01.Tables[0].Rows[i]["step_no"]), ds01.Tables[0].Rows[i]["task_id"].ToString(), ds01.Tables[0].Rows[i]["amble_mails"].ToString(), ds01.Tables[0].Rows[i]["amble_msg"].ToString(), ds01.Tables[0].Rows[i]["amble_attach"].ToString(), ds01.Tables[0].Rows[i]["AppendDocUUID"].ToString(), ds01.Tables[0].Rows[i]["amble_url"].ToString(), ds01.Tables[0].Rows[i]["AmbleSub"].ToString(), Session["CompCode"].ToString());
                            }
                        }
                        #region Update the Initiator Start
                        DataSet dsI01 = new DataSet();
                        con.Open();
                        cmd = new SqlCommand("select email from user_mast where user_id='" + Session["UserID"].ToString() + "'", con);
                        SqlDataAdapter adapterI01 = new SqlDataAdapter(cmd);
                        adapterI01.Fill(dsI01);
                        con.Close();
                        if (dsI01.Tables[0].Rows.Count > 0)
                        {
                            if (Session["AccessControl"].ToString() == "Outside")
                            {
                                result = ObjClassStoreProc.WFLogTaskUpdate(1, Session["WFLogId"].ToString(), "guest@guest.com", Session["InitiatorEmailID"].ToString(), DateTime.Now, "", "", 0, Session["CompCode"].ToString());
                            }
                            else
                            {
                                result = ObjClassStoreProc.WFLogTaskUpdate(1, Session["WFLogId"].ToString(), "init@init.com", dsI01.Tables[0].Rows[0][0].ToString(), DateTime.Now, "", "", 0, Session["CompCode"].ToString());
                            }
                        }
                        #endregion
                        CheckAction(Session["WFLogId"].ToString(), 1, DateTime.Now, "", "");
                        Session["WFTask"] = null;
                        RedirectMessageBox("Document uploaded successfully & the default workflow with tasks has been assigned!", "userhome.aspx");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.Message == "UNKNOWN token")
                {
                    MessageBox("Incorrect syntax in the formula !!");
                }
                else
                {
                    MessageBox(ex.Message);
                }
            }
        }

        protected void CheckAction(string WFLogID, int StepNo, DateTime TaskDoneDate, string TaskID, string Comments)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result = "";
                DataSet ds003 = new DataSet();
                DataSet dsT001 = new DataSet();

                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds002 = new DataSet();
                DataSet ds004 = new DataSet();
                DataSet ds005 = new DataSet();
                DataSet ds006 = new DataSet();
                con.Open();
                if (Session["AccessControl"].ToString() != "Outside")
                {
                    Session["InitiatorEmailID"] = "";
                }

                ds01.Reset();
                ds01 = ObjClassStoreProc.WFLogTaskSelect(1, WFLogID, Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        #region For Preamble Email Start
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PREEMAIL")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), "", Session["CompCode"].ToString());
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleEmail ObjPreambleEmail = new PreambleEmail();
                                ObjPreambleEmail.SendPreMail(WFLogID, StepNo, "PREEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                            }
                        }
                        #endregion
                        #region For Preamble Copy Start
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PRECOPY")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), "", Session["CompCode"].ToString());
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleCopy ObjPreambleCopy = new PreambleCopy();
                                string PreCPOutput = ObjPreambleCopy.PreCopy(WFLogID, StepNo, "PRECOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                CheckAction(WFLogID, StepNo, DateTime.Now, "PRECOPY", PreCPOutput);
                            }
                        }
                        #endregion
                        #region For Preamble Conditional Email Start
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PRECOND")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), "", Session["CompCode"].ToString());
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleConditionalEmail ObjPreambleConditionalEmail = new PreambleConditionalEmail();
                                ObjPreambleConditionalEmail.SendPreCondMail(WFLogID, StepNo, "PRECOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                            }
                        }
                        #endregion
                        #region For Preamble Append Start
                        if (ds01.Tables[0].Rows[i][2].ToString() == "PREAPPEND")
                        {
                            result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Preamble", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                            //Log Update
                            result = ObjClassStoreProc.WFLogUpdate(1, WFLogID, TaskDoneDate.ToString(), "Not Required", ds01.Tables[0].Rows[i][2].ToString(), Convert.ToInt16(StepNo), "", Session["CompCode"].ToString());
                            // Preamble Append
                            if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                            {

                            }
                            else
                            {
                                PreambleAppend ObjPreambleAppend = new PreambleAppend();
                                Session["RWFOldFile"] = ObjPreambleAppend.PreAppend(WFLogID, StepNo, "PREAPPEND", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                            }
                        }
                        #endregion

                        /// Check is there any Interactive actions or not start
                        ds003 = new DataSet();
                        ds003.Reset();
                        ds003 = ObjClassStoreProc.WFLogTaskSelect(2, WFLogID, Convert.ToInt16(StepNo), Session["CompCode"].ToString());
                        if (ds003.Tables[0].Rows.Count > 0)
                        {
                            if (TaskID == "")
                            {

                            }
                            else
                            {
                                // For APPROVE Task
                                if (TaskID == "APPROVE")
                                {
                                    #region
                                    dsT001.Reset();
                                    dsT001 = ObjClassStoreProc.WFLogTaskSelect(3, WFLogID, StepNo, Session["CompCode"].ToString());
                                    if (dsT001.Tables[0].Rows.Count > 0)
                                    {
                                        result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, Comments, TaskID, StepNo, Session["CompCode"].ToString());
                                        result = ObjClassStoreProc.WFLogTaskUpdate(2, WFLogID, "", "", TaskDoneDate, "Not Required", "REJECT", StepNo, Session["CompCode"].ToString());
                                        //Log Update
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, TaskDoneDate.ToString(), Comments, TaskID, StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, "Not Required", "Not Required", "REJECT", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                    }
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    return;
                                    #endregion
                                }
                                // For REJECT Task
                                else if (TaskID == "REJECT")
                                {
                                    #region
                                    // Need to Mail to the previous stages' users
                                    if (Session["WFTask"] != null && Session["WFTask"].ToString() == "REJECT")
                                    {

                                    }
                                    else
                                    {
                                        RejectEmail ObjRejectEmail = new RejectEmail();
                                        ObjRejectEmail.RejectMail(WFLogID, StepNo, Session["CompCode"].ToString());

                                        // Insert into Rejected List
                                        cmd = new SqlCommand("insert into RejectedList(wf_log_id,step_no,user_id,rejected_dt,comments,CompCode) values('" + WFLogID + "','" + StepNo + "','" + Session["UserID"].ToString() + "','" + TaskDoneDate + "','" + Comments + "','" + Session["CompCode"].ToString() + "')", con);
                                        cmd.ExecuteNonQuery();
                                        // Update wf_log_task & wf_log_dtl for previous stage start
                                        // Current Stage
                                        result = ObjClassStoreProc.WFLogTaskUpdate(3, WFLogID, "", "", DateTime.Now, null, "", StepNo, Session["CompCode"].ToString());
                                        cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Not Started' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        result = ObjClassStoreProc.WFLogUpdate(2, WFLogID, TaskDoneDate.ToString(), Comments, "REJECT", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());
                                        result = ObjClassStoreProc.WFLogUpdate(3, WFLogID, "Not Required", "Not Required", "", StepNo, Session["UserID"].ToString(), Session["CompCode"].ToString());

                                        // Pervious Stage
                                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        DataSet ds114 = new DataSet();
                                        SqlDataAdapter adapter114 = new SqlDataAdapter(cmd);
                                        ds114.Reset();
                                        adapter114.Fill(ds114);
                                        Session["WFTask"] = "REJECT";
                                        if (ds114.Tables[0].Rows.Count > 0)
                                        {
                                            cmd = new SqlCommand("update wf_log_task set task_done_dt=NULL,comments=NULL where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            cmd = new SqlCommand("update wf_log_dtl set comments=NULL,wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + (Convert.ToInt32(StepNo) - 1).ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            cmd.ExecuteNonQuery();
                                            //Log Update
                                            int MaxStep = 0;
                                            int StartStep = 0;
                                            DataSet dsMaxStep = new DataSet();
                                            cmd = new SqlCommand("select max(StepNo) from WFLog where WFLogID='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                            SqlDataAdapter adapterMaxStep = new SqlDataAdapter(cmd);
                                            dsMaxStep.Reset();
                                            adapterMaxStep.Fill(dsMaxStep);
                                            MaxStep = Convert.ToInt32(dsMaxStep.Tables[0].Rows[0][0].ToString());
                                            StartStep = Convert.ToInt32(StepNo) - 1;

                                            for (int kk = StartStep; kk <= MaxStep; kk++)
                                            {
                                                DataSet dsPrev = new DataSet();
                                                cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + kk + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                                SqlDataAdapter adapterPrev = new SqlDataAdapter(cmd);
                                                dsPrev.Reset();
                                                adapterPrev.Fill(dsPrev);
                                                if (kk <= StepNo) // for previous stages
                                                {
                                                    for (int jj = 0; jj < dsPrev.Tables[0].Rows.Count; jj++)
                                                    {
                                                        cmd = new SqlCommand("insert into WFLog(WFLogID,StepNo,UserID,TaskID,TaskDoneDate,Comments,CompCode) values('" + WFLogID + "', '" + kk + "','','" + dsPrev.Tables[0].Rows[jj][2].ToString() + "','Waiting','Waiting','" + Session["CompCode"].ToString() + "')", con);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                else // for later stages
                                                {
                                                    cmd = new SqlCommand("delete from WFLog where WFLogID='" + WFLogID + "' and StepNo='" + kk + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                                    cmd.ExecuteNonQuery();
                                                    for (int jj = 0; jj < dsPrev.Tables[0].Rows.Count; jj++)
                                                    {
                                                        cmd = new SqlCommand("insert into WFLog(WFLogID,StepNo,UserID,TaskID,TaskDoneDate,Comments,CompCode) values('" + WFLogID + "', '" + kk + "','','" + dsPrev.Tables[0].Rows[jj][2].ToString() + "','Waiting','Waiting','" + Session["CompCode"].ToString() + "')", con);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            CheckAction(WFLogID, StepNo - 1, DateTime.Now, "", "");
                                            return;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    // Update wf_log_task & wf_log_dtl for previous stage end
                                    #endregion
                                }
                                else if (TaskID == "REVIEW")
                                {
                                    #region
                                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and task_done_dt is null and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    DataSet dsT002 = new DataSet();
                                    SqlDataAdapter adapterT002 = new SqlDataAdapter(cmd);
                                    dsT002.Reset();
                                    adapterT002.Fill(dsT002);
                                    if (dsT002.Tables[0].Rows.Count > 0)
                                    {
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='" + Comments + "' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + TaskID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Not Required' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='REJECT' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        //Log Update
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='" + Comments + "',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + TaskID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = new SqlCommand("update WFLog set TaskDoneDate='Not Required',Comments='Not Required',UserID='" + Session["UserID"].ToString() + "' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='REJECT' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                                    return;
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null and (task_id='PRECOPY' or task_id='PREEMAIL' or task_id='PRECOND' or task_id='PREAPPEND')", con);
                            ds004 = new DataSet();
                            SqlDataAdapter adapter004 = new SqlDataAdapter(cmd);
                            ds004.Reset();
                            adapter004.Fill(ds004);
                            if (ds004.Tables[0].Rows.Count > 0)
                            {

                            }
                            else
                            {
                                #region For Postamble Mail Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTEMAIL")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleEmail ObjPostambleEmail = new PostambleEmail();
                                    ObjPostambleEmail.SendPostMail(WFLogID, StepNo, "POSTEMAIL", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Copy Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTCOPY")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Copy Action
                                    PostambleCopy ObjPostambleCopy = new PostambleCopy();
                                    string PostCPOutput = ObjPostambleCopy.PostCopy(WFLogID, StepNo, "POSTCOPY", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                    CheckAction(WFLogID, StepNo, DateTime.Now, "POSTCOPY", PostCPOutput);
                                }
                                #endregion
                                #region For Postamble Conditional Mail Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTCOND")
                                {
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    // Send Mail
                                    PostambleConditionalEmail ObjPostambleConditionalEmail = new PostambleConditionalEmail();
                                    ObjPostambleConditionalEmail.SendPostCondMail(WFLogID, StepNo, "POSTCOND", Session["CompCode"].ToString(), Session["AccessControl"].ToString(), Session["InitiatorEmailID"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString());
                                }
                                #endregion
                                #region For Postamble Append Start
                                if (ds01.Tables[0].Rows[i][2].ToString() == "POSTAPPEND")
                                {
                                    // Update status in database
                                    cmd = new SqlCommand("update wf_log_task set task_done_dt='" + TaskDoneDate + "',comments='Postamble' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and task_id='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    //Log Update
                                    cmd = new SqlCommand("update WFLog set TaskDoneDate='" + TaskDoneDate + "',Comments='Not Required' where TaskDoneDate='Waiting' and WFLogID='" + WFLogID + "' and StepNo='" + StepNo + "' and TaskID='" + ds01.Tables[0].Rows[i][2].ToString() + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();

                                    PostambleAppend ObjPostambleAppend = new PostambleAppend();
                                    Session["RWFOldFile"] = ObjPostambleAppend.PostAppend(WFLogID, StepNo, "POSTAPPEND", Session["CompCode"].ToString(), Session["AdmUserID"].ToString(), Session["AdmTicket"].ToString(), Session["UserID"].ToString());
                                }
                                #endregion
                            }
                        }
                        /// Check is there any Interactive actions or not end
                    }
                    // Update Status start
                    cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null", con);
                    ds005 = new DataSet();
                    SqlDataAdapter adapter005 = new SqlDataAdapter(cmd);
                    adapter005.Fill(ds005);
                    if (ds005.Tables[0].Rows.Count > 0)
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Ongoing' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                        // Call this method again
                        DataSet ds100 = new DataSet();
                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null  and (task_id!='PRECOPY' and task_id!='PREEMAIL' and task_id!='POSTCOPY' and task_id!='POSTEMAIL' and task_id!='PRECOND' and task_id!='POSTCOND' and task_id!='PREAPPEND' and task_id!='POSTAPPEND')", con);
                        SqlDataAdapter adapter100 = new SqlDataAdapter(cmd);
                        adapter100.Fill(ds100);
                        if (ds100.Tables[0].Rows.Count > 0)
                        {

                        }
                        else
                        {
                            CheckAction(WFLogID, StepNo, DateTime.Now, "", "");
                            return;
                        }
                    }
                    else
                    {
                        cmd = new SqlCommand("update wf_log_dtl set wf_stat='Completed' where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("select * from wf_log_task where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "' and task_done_dt is null", con);
                        ds006 = new DataSet();
                        SqlDataAdapter adapter006 = new SqlDataAdapter(cmd);
                        adapter006.Fill(ds006);
                        if (ds006.Tables[0].Rows.Count > 0)
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Ongoing' where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new SqlCommand("update wf_log_mast set wf_prog_stat='Completed',actual_completed_dt='" + TaskDoneDate + "' where wf_log_id='" + WFLogID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // Update Status end

                    // Check the stage is completed or not
                    cmd = new SqlCommand("select wf_stat from wf_log_dtl where wf_log_id='" + WFLogID + "' and step_no='" + StepNo + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    ds002 = new DataSet();
                    SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                    adapter002.Fill(ds002);
                    if (ds002.Tables[0].Rows.Count > 0)
                    {
                        if (ds002.Tables[0].Rows[0][0].ToString() == "Completed")
                        {
                            // Call this method again
                            CheckAction(WFLogID, StepNo + 1, DateTime.Now, "", "");
                            return;
                        }
                    }
                }
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                //MessageBox(ex.Message);
            }
        }

        private void Redirect2Home(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"home.aspx\"</script>";

            Page.Controls.Add(lbl);
        }

        private void RedirectMessageBox(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"userhome.aspx\"</script>";

            Page.Controls.Add(lbl);
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.Flush();
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("eFormEditing.aspx?DocID=" + Request.QueryString["DocID"].ToString(),false);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

    }
}