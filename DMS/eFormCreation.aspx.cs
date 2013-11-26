using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using DMS.UTILITY;

namespace DMS
{
    public partial class eFormCreation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ddCabinet.Attributes.Add("onChange", "javascript: ButtonStatus('C');");
                ddDrawer.Attributes.Add("onChange", "javascript: ButtonStatus('C');");
                ddFolder.Attributes.Add("onChange", "javascript: ButtonStatus('C');");
                GenDesign();
                if (!IsPostBack)
                {
                    hfButtonStatus.Value = "";
                    hfDropdownValues.Value = "";
                    hfControls4Formula.Value = "";
                    Session["dtFields"] = null;

                    // Set the session variables blank which are used to set the previous selected path start
                    Session["SelectedCabUUID"] = "";
                    Session["SelectedDrwUUID"] = "";
                    Session["SelectedFldUUID"] = "";
                    Session["SelectedDocID"] = "";
                    // Set the session variables blank which are used to set the previous selected path end
                    if (Session["UserID"] != null && Session["Ticket"] != null)
                    {
                        PopulateDropdown();
                        PopulateCabinet();
                        if (Session["UserType"].ToString() == "S") // Super Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            divMenuSuperAdmin.Visible = false;
                            divMenuAdmin.Visible = true;
                            divMenuNormal.Visible = false;
                        }
                        else
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
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

        protected void cmdInsert_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void GenDesign()
        {
            try
            {
                if (hfControls != null)
                {
                    DataTable dtFields = null;
                    if (hfButtonStatus.Value == "G")
                    {
                        string DataType = ddDataType.SelectedItem.Text;
                        int MaxLength = 0;
                        int MinVal = 0;
                        int MaxVal = 0;
                        string Formula = "";
                        if (ddControlType.SelectedValue == "T")
                        {
                            if (DataType == "Text")
                            {
                                if (txtMaxLength.Text.Trim() == "")
                                {
                                    MaxLength = 0;
                                }
                                else
                                {
                                    MaxLength = Convert.ToInt32(txtMaxLength.Text);
                                }
                                MinVal = 0;
                                MaxVal = 0;
                                Formula = "";
                                hfControls4Formula.Value = "";
                            }
                            else if (DataType == "Numeric")
                            {
                                MaxLength = 0;
                                if (txtMinVal.Text.Trim() == "")
                                {
                                    MinVal = 0;
                                }
                                else
                                {
                                    MinVal = Convert.ToInt32(txtMinVal.Text);
                                }
                                if (txtMaxVal.Text.Trim() == "")
                                {
                                    MaxVal = 0;
                                }
                                else
                                {
                                    MaxVal = Convert.ToInt32(txtMaxVal.Text);
                                }
                                Formula = "";
                                hfControls4Formula.Value = "";
                            }
                            else if (DataType == "Date")
                            {
                                MaxLength = 0;
                                MinVal = 0;
                                MaxVal = 0;
                                Formula = "";
                                hfControls4Formula.Value = "";
                            }
                            else if (DataType == "Formula")
                            {
                                MaxLength = 0;
                                MinVal = 0;
                                MaxVal = 0;
                                Formula = txtFormula.Text.Trim();
                            }
                        }
                        else if (ddControlType.SelectedValue == "D")
                        {
                            DataType = "Text";
                            MaxLength = 0;
                            MinVal = 0;
                            MaxVal = 0;
                            Formula = "";
                            hfControls4Formula.Value = "";
                        }

                        #region check the control is already added or not
                        if (Session["dtFields"] != null)
                        {
                            dtFields = (DataTable)Session["dtFields"];
                            for (int i = 0; i < dtFields.Rows.Count; i++)
                            {
                                if (txtLabelText.Text.Trim() == dtFields.Rows[i]["LabelDesc"].ToString())
                                {
                                    throw new Exception("This Field is already created!!");
                                }
                            }
                        }
                        #endregion

                        GenDT4Fields(ddPosition.SelectedIndex, ddControlType.SelectedValue, DataType, MaxLength, MinVal, MaxVal, Formula, txtLabelText.Text.Trim(), hfControls4Formula.Value);

                        //Populate ddPosition DropDown
                        ddPosition.Items.Clear();
                        dtFields = (DataTable)Session["dtFields"];
                        ddPosition.Items.Add("At the Beginning");
                        for (int i = 0; i < dtFields.Rows.Count; i++)
                        {
                            ddPosition.Items.Add("After " + dtFields.Rows[i]["LabelDesc"].ToString());
                        }
                        ddPosition.SelectedIndex = dtFields.Rows.Count;

                        //Populate ddLabelDesc Dropdown
                        ddLabelDesc.Items.Clear();
                        ddLabelDesc.DataSource = dtFields;
                        ddLabelDesc.DataTextField = "LabelDesc";
                        ddLabelDesc.DataValueField = "ControlID";
                        ddLabelDesc.DataBind();

                        hfControls.Value += ddControlType.SelectedValue;
                        if (ddControlType.SelectedValue == "D")
                        {
                            Session[hfControls.Value.Length.ToString()] = hfDropdownValues.Value;
                            hfDropdownValues.Value = "";
                        }

                        #region Reset the Controls
                        txtLabelText.Text = "";
                        //Populate ddControlType Dropdown
                        ddControlType.Items.Clear();
                        System.Web.UI.WebControls.ListItem[] items = new System.Web.UI.WebControls.ListItem[2];
                        items[0] = new System.Web.UI.WebControls.ListItem("Text", "T");
                        items[1] = new System.Web.UI.WebControls.ListItem("Dropdown", "D");
                        ddControlType.Items.AddRange(items);
                        ddControlType.DataBind();

                        //Populate ddDataType Dropdown
                        ddDataType.Items.Clear();
                        ddDataType.Items.Add("Text");
                        ddDataType.Items.Add("Numeric");
                        ddDataType.Items.Add("Date");
                        ddDataType.Items.Add("Formula");

                        txtMaxLength.Text = "0";
                        txtMinVal.Text = "0";
                        txtMaxVal.Text = "0";
                        txtFormula.Text = "";
                        hfControls4Formula.Value = "";
                        #endregion
                    }

                    PopulateDesign();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateDesign()
        {
            try
            {
                if (hfControls != null)
                {
                    DataTable dtFields = null;
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

        protected string GenControlID(string ControlType, string ControlName, int ControlIndex)
        {
            string ControlID = ControlName + ControlIndex.ToString();
            DataTable dtFields = null;
            if (Session["dtFields"] != null)
            {
                dtFields = (DataTable)Session["dtFields"];
            }
            else
            {
                dtFields = CreateDT4Fields();
            }
            for (int i = 0; i < dtFields.Rows.Count; i++)
            {
                if (ControlType == "L")
                {
                    if (dtFields.Rows[i]["LabelID"].ToString() == ControlID)
                    {
                        ControlID = GenControlID(ControlType, ControlName, ControlIndex + 1);
                    }
                }
                else
                {
                    if (dtFields.Rows[i]["ControlID"].ToString() == ControlID)
                    {
                        ControlID = GenControlID(ControlType, ControlName, ControlIndex + 1);
                    }
                }
            }
            return ControlID;
        }

        protected void GenDT4Fields(int PositionNo, string ControlType, string DataType, int MaxLength, int MinVal, int MaxVal, string Formula, string LabelDesc, string Controls4Formula)
        {
            try
            {
                int SerialNo = 1;
                DataTable dtFields = null;
                DataTable dtFieldsTEMP = null;
                if (Session["dtFields"] != null)
                {
                    dtFields = (DataTable)Session["dtFields"];
                    SerialNo = dtFields.Rows.Count + 1;
                }
                else
                {
                    dtFields = CreateDT4Fields();
                }

                string LabelID = "";
                string ControlID = "";
                //LabelID = "lblName_" + hfControls.Value.Length.ToString();
                LabelID = GenControlID("L", "lblName_", hfControls.Value.Length);
                if (ControlType == "T")
                {
                    //ControlID = "txtName_" + hfControls.Value.Length.ToString();
                    ControlID = GenControlID("T", "txtName_", hfControls.Value.Length);
                }
                else if (ControlType == "D")
                {
                    //ControlID = "ddList_" + hfControls.Value.Length.ToString();
                    ControlID = GenControlID("D", "ddList_", hfControls.Value.Length);
                }
                DataRow r = dtFields.NewRow();
                if (PositionNo == dtFields.Rows.Count)
                {
                    dtFields.Rows.Add(AddNewInDTFields(r, SerialNo, LabelID, ControlID, ControlType, DataType, MaxLength, MinVal, MaxVal, Formula, LabelDesc, Controls4Formula));
                }
                else
                {
                    #region Moving to TEMP DT
                    for (int i = PositionNo + 1; i <= dtFields.Rows.Count; i++)
                    {
                        if (dtFieldsTEMP == null || dtFieldsTEMP.Rows.Count == 0)
                        {
                            dtFieldsTEMP = CreateDT4Fields();
                        }
                        DataRow rTEMP = dtFieldsTEMP.NewRow();
                        dtFieldsTEMP.Rows.Add(AddNewInDTFields(rTEMP, Convert.ToInt32(dtFields.Rows[i - 1]["SerialNo"].ToString()) + 1, dtFields.Rows[i - 1]["LabelID"].ToString(), dtFields.Rows[i - 1]["ControlID"].ToString(), dtFields.Rows[i - 1]["ControlType"].ToString(), dtFields.Rows[i - 1]["DataType"].ToString(), Convert.ToInt32(dtFields.Rows[i - 1]["MaxLength"].ToString()), Convert.ToInt32(dtFields.Rows[i - 1]["MinVal"].ToString()), Convert.ToInt32(dtFields.Rows[i - 1]["MaxVal"].ToString()), dtFields.Rows[i - 1]["Formula"].ToString(), dtFields.Rows[i - 1]["LabelDesc"].ToString(), dtFields.Rows[i - 1]["Controls4Formula"].ToString()));
                    }
                    #endregion
                    #region Deleting after moving to TEMP DT
                    for (int i = dtFields.Rows.Count - 1; i >= PositionNo; i--)
                    {
                        DataRow dr = dtFields.Rows[i];
                        if (dr["SerialNo"].ToString() == (i + 1).ToString())
                            dr.Delete();
                    }
                    #endregion
                    dtFields.Rows.Add(AddNewInDTFields(r, PositionNo + 1, LabelID, ControlID, ControlType, DataType, MaxLength, MinVal, MaxVal, Formula, LabelDesc, Controls4Formula));
                }
                Session["dtFields"] = dtFields;
                if (dtFieldsTEMP != null)
                {
                    dtFields.Merge(dtFieldsTEMP);
                }
                gvFields.DataSource = dtFields;
                gvFields.DataBind();
                dtFieldsTEMP = null;
            }
            catch (Exception ex)
            {

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

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddDocType.SelectedValue == "")
                {
                    throw new Exception("Please select a Document Type !!");
                }
                else if (ddFolder.SelectedValue == "")
                {
                    throw new Exception("Please select a Folder !!");
                }
                else if (txtFormName.Text.Trim() == "")
                {
                    throw new Exception("Please enter a Name to save the eForm !!");
                }
                string AutoGenID = Guid.NewGuid().ToString();
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = "";
                DataTable dtFields = null;
                double FileSize = 0;

                result = ObjClassStoreProc.ExistDoc(txtFormName.Text.Trim(), ddFolder.SelectedValue, Session["CompCode"].ToString());
                if (Convert.ToInt32(result) == -1)
                {
                    throw new Exception("This Form is already exists in this Folder !!");
                }
                else
                {
                    result = ObjClassStoreProc.InsertDocMast(txtFormName.Text.Trim(), txtFormName.Text.Trim(), ddFolder.SelectedValue, ddDocType.SelectedValue, "NA", Session["UserID"].ToString(), DateTime.Now, "", "", "", "", "", "", "", "", "", "", "", AutoGenID, "", "E", Session["CompCode"].ToString(), FileSize);
                    if (Convert.ToInt32(result) == -1)
                    {
                        throw new Exception("This Form is already exists in this Folder !!");
                    }
                    else
                    {
                        UserRights RightsObj = new UserRights();
                        DataSet dsPerm = new DataSet();
                        dsPerm.Reset();
                        dsPerm = RightsObj.FetchPermission(ddFolder.SelectedValue, Session["CompCode"].ToString());
                        if (dsPerm.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsPerm.Tables[0].Rows.Count; i++)
                            {
                                RightsObj.InsertPermissionSingleData(AutoGenID, "Document", dsPerm.Tables[0].Rows[i][0].ToString(), dsPerm.Tables[0].Rows[i][1].ToString(), Session["CompCode"].ToString());
                            }
                        }

                        dtFields = (DataTable)Session["dtFields"];
                        for (int i = 0; i < dtFields.Rows.Count; i++)
                        {
                            #region Saving ControlMaster
                            result = ObjClassStoreProc.InsertControlMaster(AutoGenID, Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString()), dtFields.Rows[i]["LabelID"].ToString(), dtFields.Rows[i]["ControlID"].ToString(), dtFields.Rows[i]["ControlType"].ToString(), dtFields.Rows[i]["LabelDesc"].ToString(), dtFields.Rows[i]["DataType"].ToString(), Convert.ToInt32(dtFields.Rows[i]["MaxLength"].ToString()), Convert.ToInt32(dtFields.Rows[i]["MinVal"].ToString()), Convert.ToInt32(dtFields.Rows[i]["MaxVal"].ToString()), dtFields.Rows[i]["Formula"].ToString(), dtFields.Rows[i]["Controls4Formula"].ToString());
                            #endregion
                            if (dtFields.Rows[i]["ControlType"].ToString() == "D")
                            {
                                #region Saving DropdownItems
                                DropDownList ddL1 = (DropDownList)tblMain.Rows[Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString())].Cells[0].FindControl(dtFields.Rows[i]["ControlID"].ToString());
                                if (ddL1 != null)
                                {
                                    for (int j = 0; j < ddL1.Items.Count; j++)
                                    {
                                        result = ObjClassStoreProc.InsertDropdownItems(AutoGenID, Convert.ToInt32(dtFields.Rows[i]["SerialNo"].ToString()), dtFields.Rows[i]["ControlID"].ToString(), j, ddL1.Items[j].ToString());
                                    }
                                }
                                #endregion
                            }
                        }
                        hfButtonStatus.Value = "";
                        hfDropdownValues.Value = "";
                        hfControls4Formula.Value = "";
                        Session["dtFields"] = null;
                        Redirect2HomePage("The eForm has been created successfully !!", "home.aspx");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void gvFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        protected void gvFields_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)gvFields.Rows[e.RowIndex];
                Label lbSerialNo = (Label)row.FindControl("lbSerialNo");

                DataTable dtFields = null;
                DataTable dtFieldsTEMP = null;
                dtFields = (DataTable)Session["dtFields"];
                for (int i = 0; i < dtFields.Rows.Count; i++)
                {
                    DataRow dr = dtFields.Rows[i];
                    if (dr["SerialNo"].ToString() == lbSerialNo.Text.ToString())
                    {
                        #region hfControls Deletion
                        string ControlID = dr["ControlID"].ToString();
                        int ControlIDLength = ControlID.Length;
                        int IndexOfControlID = Convert.ToInt16(ControlID.Substring(ControlID.LastIndexOf('_') + 1, (ControlIDLength - ControlID.LastIndexOf('_') - 1)));
                        string OldhfControls = hfControls.Value;
                        hfControls.Value = OldhfControls.Substring(0, IndexOfControlID) + OldhfControls.Substring(IndexOfControlID + 1, OldhfControls.Length - IndexOfControlID - 1);
                        #endregion

                        dr.Delete();

                        //Populate ddPosition DropDown
                        ddPosition.Items.Clear();
                        //dtFields = (DataTable)Session["dtFields"];
                        ddPosition.Items.Add("At the Beginning");
                        for (int j = 0; j < dtFields.Rows.Count; j++)
                        {
                            ddPosition.Items.Add("After " + dtFields.Rows[j]["LabelDesc"].ToString());
                        }
                        ddPosition.SelectedIndex = dtFields.Rows.Count;

                        //Populate ddLabelDesc Dropdown
                        ddLabelDesc.Items.Clear();
                        ddLabelDesc.DataSource = dtFields;
                        ddLabelDesc.DataTextField = "LabelDesc";
                        ddLabelDesc.DataValueField = "ControlID";
                        ddLabelDesc.DataBind();

                        #region Moving to TEMP DT
                        for (int j = Convert.ToInt32(lbSerialNo.Text.ToString()); j <= dtFields.Rows.Count; j++)
                        {
                            if (dtFieldsTEMP == null || dtFieldsTEMP.Rows.Count == 0)
                            {
                                dtFieldsTEMP = CreateDT4Fields();
                            }
                            DataRow rTEMP = dtFieldsTEMP.NewRow();
                            dtFieldsTEMP.Rows.Add(AddNewInDTFields(rTEMP, Convert.ToInt32(dtFields.Rows[j - 1]["SerialNo"].ToString()) - 1, dtFields.Rows[j - 1]["LabelID"].ToString(), dtFields.Rows[j - 1]["ControlID"].ToString(), dtFields.Rows[j - 1]["ControlType"].ToString(), dtFields.Rows[j - 1]["DataType"].ToString(), Convert.ToInt32(dtFields.Rows[j - 1]["MaxLength"].ToString()), Convert.ToInt32(dtFields.Rows[j - 1]["MinVal"].ToString()), Convert.ToInt32(dtFields.Rows[j - 1]["MaxVal"].ToString()), dtFields.Rows[j - 1]["Formula"].ToString(), dtFields.Rows[j - 1]["LabelDesc"].ToString(), dtFields.Rows[j - 1]["Controls4Formula"].ToString()));
                        }
                        #endregion
                        #region Deleting after moving to TEMP DT
                        for (int j = dtFields.Rows.Count - 1; j >= Convert.ToInt32(lbSerialNo.Text.ToString()) - 1; j--)
                        {
                            DataRow dr1 = dtFields.Rows[j];
                            dr1.Delete();
                        }
                        #endregion
                    }
                }
                Session["dtFields"] = dtFields;
                if (dtFieldsTEMP != null)
                {
                    dtFields.Merge(dtFieldsTEMP);
                }
                gvFields.DataSource = dtFields;
                gvFields.DataBind();
                dtFieldsTEMP = null;
                tblMain.Controls.Clear();
                PopulateDesign();
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
                //....Doc Type
                ds01.Reset();
                ds01 = ObjClassStoreProc.eFormDocTypeCompBased(Session["CompCode"].ToString());
                ddDocType.DataSource = ds01;
                ddDocType.DataTextField = "doc_type_name";
                ddDocType.DataValueField = "doc_type_id";
                ddDocType.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateCabinet()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.CabinetPermissionM(Session["CompCode"].ToString(), Session["UserID"].ToString());
                ddCabinet.DataSource = ds01;
                ddCabinet.DataTextField = "cab_name";
                ddCabinet.DataValueField = "cab_uuid";
                ddCabinet.DataBind();
                PopulateDrawer(ddCabinet.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateDrawer(string SelCabID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerPermissionM(SelCabID, Session["UserID"].ToString());
                ddDrawer.DataSource = ds01;
                ddDrawer.DataTextField = "drw_name";
                ddDrawer.DataValueField = "drw_uuid";
                ddDrawer.DataBind();
                PopulateFolder(ddDrawer.SelectedValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PopulateFolder(string SelDrawerUUID)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderPermissionM(SelDrawerUUID, Session["UserID"].ToString());
                ddFolder.DataSource = ds01;
                ddFolder.DataTextField = "fld_name";
                ddFolder.DataValueField = "fld_uuid";
                ddFolder.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ddCabinet_SelectedIndexChanged(object sender, EventArgs e)
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
                PopulateDrawer(ddCabinet.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ddDrawer_SelectedIndexChanged(object sender, EventArgs e)
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
                PopulateFolder(ddDrawer.SelectedValue);
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

        private void Redirect2HomePage(string msg, string msg2)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "');" + Environment.NewLine + "window.location=\"home.aspx\"</script>";

            Page.Controls.Add(lbl);
        }
    }
}