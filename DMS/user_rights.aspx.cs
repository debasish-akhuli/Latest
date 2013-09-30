using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS
{
    public partial class user_rights : System.Web.UI.Page
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
                            PopCompany();
                            PopulateRole();
                            populateCabinet();
                            divCompany.Visible = true;
                            divMenuSuperAdmin.Visible = true;
                            divMenuAdmin.Visible = false;
                            divMenuNormal.Visible = false;
                        }
                        else if (Session["UserType"].ToString() == "A") // Admin
                        {
                            lblUser.Text = Session["UserFullName"].ToString();
                            PopCompany();
                            PopulateRole();
                            populateCabinet();
                            divCompany.Visible = false;
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

        protected void ddCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            PopulateRole();
            populateCabinet();
        }

        protected void PopCompany()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                //....Company
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectCompActiveAll();
                ddCompany.DataSource = ds01;
                ddCompany.DataTextField = "CompName";
                ddCompany.DataValueField = "CompCode";
                ddCompany.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void PopulateRole()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.RoleBasedOnCompCode(ddCompany.SelectedValue);
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.RoleBasedOnCompCode(Session["CompCode"].ToString());
                }
                //DBClass DBObj = new DBClass();
                //DataSet ds2 = new DataSet();
                //ds2 = DBObj.DDRole();
                gvRole.DataSource = ds01;
                gvRole.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void chkRole(Object sender, EventArgs e)
        {
            try
            {
                RoleChecked();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void RoleChecked()
        {
            try
            {
                int count = 0;
                string chkID = "";
                foreach (GridViewRow row in gvRole.Rows)
                {
                    CheckBox checkbox = (CheckBox)row.FindControl("CheckBox2");

                    if (checkbox.Checked)
                    {
                        Label lbCabID = (Label)row.FindControl("lbAutoIDRole");
                        chkID = lbCabID.Text;
                        count += 1;
                    }
                }
                if (count == 1)
                {
                    PopulateGridView(chkID);
                }
                else
                {
                    gvDispCab.DataSource = null;
                    gvDispCab.DataBind();
                    gvDispDoc.DataSource = null;
                    gvDispDoc.DataBind();
                    gvDispDrw.DataSource = null;
                    gvDispDrw.DataBind();
                    gvDispFld.DataSource = null;
                    gvDispFld.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void populateCabinet()
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                if (Session["UserType"].ToString() == "S") // Super Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(ddCompany.SelectedValue, Session["UserID"].ToString());
                }
                else if (Session["UserType"].ToString() == "A") // Admin
                {
                    ds01 = ObjClassStoreProc.SelectCabinetAll(Session["CompCode"].ToString(), Session["UserID"].ToString());
                }
                gvCab.DataSource = ds01;
                gvCab.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void ChkAll_Role(object sender, EventArgs e)
        {
            CheckBox chk2;
            foreach (GridViewRow rowItem in gvRole.Rows)
            {
                chk2 = (CheckBox)(rowItem.Cells[0].FindControl("CheckBox2"));
                chk2.Checked = ((CheckBox)sender).Checked;
            }
            gvDispCab.DataSource = null;
            gvDispCab.DataBind();
            gvDispDoc.DataSource = null;
            gvDispDoc.DataBind();
            gvDispDrw.DataSource = null;
            gvDispDrw.DataBind();
            gvDispFld.DataSource = null;
            gvDispFld.DataBind();           
        }

        protected void chkSelectAllCab_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            CheckBox chk;
            foreach (GridViewRow rowItem in gvCab.Rows)
            {
                chk = (CheckBox)(rowItem.Cells[0].FindControl("cabRows"));
                chk.Checked = ((CheckBox)sender).Checked;
            }
            ViewState["cab"] = null;
            ViewState["cab_uuid"] = null;
            int count = 0;
            string selected = "";
            string selected_uuid = "";
            foreach (GridViewRow row in gvCab.Rows)
            {
                CheckBox checkbox = (CheckBox)row.FindControl("cabRows");

                if (checkbox.Checked)
                {
                    string ID = gvCab.DataKeys[row.RowIndex].Value.ToString();
                    count += 1;

                    selected += selected == "" ? ID.ToString() : "," + ID.ToString();
                    selected_uuid += selected_uuid == "" ? gvCab.DataKeys[row.RowIndex][0].ToString() : "," + gvCab.DataKeys[row.RowIndex][0].ToString();
                }
            }
            if (count == 1)
            {
                populateDrawer(selected);
                ViewState["cab"] = selected;
                ViewState["cab_uuid"] = selected_uuid;
            }
            else
            {
                ViewState["cab"] = selected;
                ViewState["cab_uuid"] = selected_uuid;
                gvDrawer.DataSource = null;
                gvDrawer.DataBind();
                gvFolder.DataSource = null;
                gvFolder.DataBind();
                gvDoc.DataSource = null;
                gvDoc.DataBind();
                string[] sites = selected.ToString().Split(',');
                foreach (string s in sites)
                {

                }
            }
        }
        protected void chkSelectAllDrw_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            CheckBox chk;
            foreach (GridViewRow rowItem in gvDrawer.Rows)
            {
                chk = (CheckBox)(rowItem.Cells[0].FindControl("drwRows"));
                chk.Checked = ((CheckBox)sender).Checked;
            }
            ViewState["drw"] = null;
            ViewState["drw_uuid"] = null;
            int countD = 0;
            string selectedD = "";
            string selectedD_uuid = "";
            foreach (GridViewRow row in gvDrawer.Rows)
            {
                CheckBox checkboxD = (CheckBox)row.FindControl("drwRows");

                if (checkboxD.Checked)
                {
                    string ID1 = gvDrawer.DataKeys[row.RowIndex].Value.ToString();
                    countD += 1;

                    selectedD += selectedD == "" ? ID1.ToString() : "," + ID1.ToString();
                    selectedD_uuid += selectedD_uuid == "" ? gvDrawer.DataKeys[row.RowIndex][0].ToString() : "," + gvDrawer.DataKeys[row.RowIndex][0].ToString();
                }
            }
            if (countD == 1)
            {
                populateFolder(selectedD);
                ViewState["drw"] = selectedD;
                ViewState["drw_uuid"] = selectedD_uuid;
            }
            else
            {
                ViewState["drw"] = selectedD;
                ViewState["drw_uuid"] = selectedD_uuid;
                gvFolder.DataSource = null;
                gvFolder.DataBind();
                gvDoc.DataSource = null;
                gvDoc.DataBind();
                string[] sites1 = selectedD.ToString().Split(',');
                foreach (string s1 in sites1)
                {

                }
            }
        }
        protected void chkSelectAllFld_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            CheckBox chk;
            foreach (GridViewRow rowItem in gvFolder.Rows)
            {
                chk = (CheckBox)(rowItem.Cells[0].FindControl("fldRows"));
                chk.Checked = ((CheckBox)sender).Checked;
            }
            ViewState["fld"] = null;
            ViewState["fld_uuid"] = null;
            int countF = 0;
            string selectedF = "";
            string selectedF_uuid = "";
            foreach (GridViewRow row in gvFolder.Rows)
            {
                CheckBox checkboxF = (CheckBox)row.FindControl("fldRows");

                if (checkboxF.Checked)
                {
                    string ID2 = gvFolder.DataKeys[row.RowIndex].Value.ToString();
                    countF += 1;
                    selectedF += selectedF == "" ? ID2.ToString() : "," + ID2.ToString();
                    selectedF_uuid += selectedF_uuid == "" ? gvFolder.DataKeys[row.RowIndex][0].ToString() : "," + gvFolder.DataKeys[row.RowIndex][0].ToString();
                }
            }
            if (countF == 1)
            {
                populateDoc(selectedF);
                ViewState["fld"] = selectedF;
                ViewState["fld_uuid"] = selectedF_uuid;
            }
            else
            {
                ViewState["fld"] = selectedF;
                ViewState["fld_uuid"] = selectedF_uuid;
                gvDoc.DataSource = null;
                gvDoc.DataBind();
                string[] sites2 = selectedF.ToString().Split(',');
                foreach (string s2 in sites2)
                {

                }
            }
        }
        protected void chkSelectAllDoc_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            CheckBox chk;
            foreach (GridViewRow rowItem in gvDoc.Rows)
            {
                chk = (CheckBox)(rowItem.Cells[0].FindControl("docRows"));
                chk.Checked = ((CheckBox)sender).Checked;
            }
        }
        protected void Check_Clicked_Cab(Object sender, EventArgs e)
        {
            ViewState["cab"] = null;
            ViewState["cab_uuid"] = null;
            int count = 0;
            string selected = "";
            string selected_uuid = "";
            foreach (GridViewRow row in gvCab.Rows)
            {
                CheckBox checkbox = (CheckBox)row.FindControl("cabRows");

                if (checkbox.Checked)
                {
                    string ID = gvCab.DataKeys[row.RowIndex].Value.ToString();
                    count += 1;

                    selected += selected == "" ? ID.ToString() : ","+ID.ToString();
                    selected_uuid += selected_uuid == "" ? gvCab.DataKeys[row.RowIndex][0].ToString() : "," + gvCab.DataKeys[row.RowIndex][0].ToString();
                }
            }
            if (count == 1)
            {
                populateDrawer(selected);
                ViewState["cab"] = selected;
                ViewState["cab_uuid"] = selected_uuid;
            }
            else
            {
                ViewState["cab"] = selected;
                ViewState["cab_uuid"] = selected_uuid;
                gvDrawer.DataSource = null;
                gvDrawer.DataBind();
                gvFolder.DataSource = null;
                gvFolder.DataBind();
                gvDoc.DataSource = null;
                gvDoc.DataBind();
                string[] sites = selected.ToString().Split(',');
                foreach (string s in sites)
                {
                    
                }
            }
        }

        protected void populateDrawer(string selected)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.DrawerAllSelectCabinetBased(selected, Session["UserID"].ToString());
                gvDrawer.DataSource = ds01;
                gvDrawer.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void Check_Clicked_Drw(Object sender, EventArgs e)
        {
            ViewState["drw"] = null;
            ViewState["drw_uuid"] = null;
            int countD = 0;
            string selectedD = "";
            string selectedD_uuid = "";
            foreach (GridViewRow row in gvDrawer.Rows)
            {
                CheckBox checkboxD = (CheckBox)row.FindControl("drwRows");

                if (checkboxD.Checked)
                {
                    string ID1 = gvDrawer.DataKeys[row.RowIndex].Value.ToString();
                    countD += 1;

                    selectedD += selectedD == "" ? ID1.ToString() : "," + ID1.ToString();
                    selectedD_uuid += selectedD_uuid == "" ? gvDrawer.DataKeys[row.RowIndex][0].ToString() : "," + gvDrawer.DataKeys[row.RowIndex][0].ToString();
                }
            }
            if (countD == 1)
            {
                populateFolder(selectedD);
                ViewState["drw"] = selectedD;
                ViewState["drw_uuid"] = selectedD_uuid;
            }
            else
            {
                ViewState["drw"] = selectedD;
                ViewState["drw_uuid"] = selectedD_uuid;
                gvFolder.DataSource = null;
                gvFolder.DataBind();
                gvDoc.DataSource = null;
                gvDoc.DataBind();
                string[] sites1 = selectedD.ToString().Split(',');
                foreach (string s1 in sites1)
                {

                }
            }
        }

        protected void populateFolder(string selectedD)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.FolderAllSelectDrawerBased(selectedD, Session["UserID"].ToString());
                gvFolder.DataSource = ds01;
                gvFolder.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        protected void Check_Clicked_Fld(Object sender, EventArgs e)
        {
            ViewState["fld"] = null;
            ViewState["fld_uuid"] = null;
            int countF = 0;
            string selectedF = "";
            string selectedF_uuid = "";
            foreach (GridViewRow row in gvFolder.Rows)
            {
                CheckBox checkboxF = (CheckBox)row.FindControl("fldRows");

                if (checkboxF.Checked)
                {
                    string ID2 = gvFolder.DataKeys[row.RowIndex].Value.ToString();
                    countF += 1;
                    selectedF += selectedF == "" ? ID2.ToString() : "," + ID2.ToString();
                    selectedF_uuid += selectedF_uuid == "" ? gvFolder.DataKeys[row.RowIndex][0].ToString() : "," + gvFolder.DataKeys[row.RowIndex][0].ToString();
                }
            }
            if (countF == 1)
            {
                populateDoc(selectedF);
                ViewState["fld"] = selectedF;
                ViewState["fld_uuid"] = selectedF_uuid;
            }
            else
            {
                ViewState["fld"] = selectedF;
                ViewState["fld_uuid"] = selectedF_uuid;
                gvDoc.DataSource = null;
                gvDoc.DataBind();
                string[] sites2 = selectedF.ToString().Split(',');
                foreach (string s2 in sites2)
                {

                }
            }
        }

        protected void populateDoc(string selectedF)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                ds01.Reset();
                ds01 = ObjClassStoreProc.SelectDocsAllBasedOnFolder(selectedF, Session["UserID"].ToString());
                //user_rights_dal URObj = new user_rights_dal();
                //DataSet ds1 = new DataSet();
                //ds1 = URObj.GVDoc(selectedF);
                gvDoc.DataSource = ds01;
                gvDoc.DataBind();
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
        /// The following function is used to insert a record in the Database's  <user_id> & <role_id> fields of <user_role> table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAllow_Click(object sender, EventArgs e)
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
                string mPermission="X";
                if (OptNo.Checked == true)
                {
                    mPermission="X";
                }
                else if(OptView.Checked==true)
                {
                    mPermission="V";
                }
                else if(OptModify.Checked==true)
                {
                    mPermission="M";
                }
                else if(OptDelete.Checked==true)
                {
                    mPermission="D";
                }
                ViewState["doc"] = null;
                ViewState["doc_uuid"] = null;
                int countF = 0;
                string selectedDet = "";
                string selectedDet_uuid = "";
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                string mUserID = "";
                /// Fetch the user_id start
                string Roleids = "";
                foreach (GridViewRow row in gvRole.Rows)
                {
                    CheckBox checkboxDc = (CheckBox)row.FindControl("CheckBox2");

                    if (checkboxDc.Checked)
                    {
                        string ID3 = gvRole.DataKeys[row.RowIndex].Value.ToString();

                        //countF += 1;
                        Roleids += Roleids == "" ? "'" + ID3.ToString() + "'" : "," + "'" + ID3.ToString() + "'";
                        //selectedDet_uuid += selectedDet_uuid == "" ? gvDoc.DataKeys[row.RowIndex][0].ToString() : "," + gvDoc.DataKeys[row.RowIndex][0].ToString();
                    }
                }
                string querys = "select user_id from user_role where CompCode='" + Session["CompCode"].ToString() + "' and role_id IN(" + Roleids + ")";
                cmd = new SqlCommand(querys, con);
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        mUserID += mUserID == "" ? "'" + ds.Tables[0].Rows[i]["user_id"].ToString() + "'" : " OR UserID=" + "'" + ds.Tables[0].Rows[i]["user_id"].ToString() + "'";
                    }
                }
                /// Fetch the user_id end

                

                foreach (GridViewRow row in gvDoc.Rows)
                {
                    CheckBox checkboxDc = (CheckBox)row.FindControl("docRows");

                    if (checkboxDc.Checked)
                    {
                        string ID3 = gvDoc.DataKeys[row.RowIndex].Value.ToString();

                        countF += 1;
                        selectedDet += selectedDet == "" ? ID3.ToString() : "," + ID3.ToString();
                        selectedDet_uuid += selectedDet_uuid == "" ? gvDoc.DataKeys[row.RowIndex][0].ToString() : "," + gvDoc.DataKeys[row.RowIndex][0].ToString();
                    }
                }
                if (countF >= 1)
                {
                    ViewState["doc"] = selectedDet;
                    ViewState["doc_uuid"] = selectedDet_uuid;
                }

                if (ViewState["doc"] != null)
                {
                    char[] ch = { ',' };
                    string[] str = ViewState["doc"].ToString().Split(ch);
                    char[] ch1 = { ',' };
                    string[] str1 = ViewState["doc_uuid"].ToString().Split(ch1);
                    for (var i = 0; i < str.Length; i++)
                    {
                        cmd = new SqlCommand("update UserRights set Permission='" + mPermission + "' where NodeUUID='" + str[i].ToString() + "' and NodeType='Document' and UserID=" + mUserID + " and CompCode='" + Session["CompCode"].ToString() + "'", con);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    if (ViewState["fld"] != null)
                    {
                        char[] ch = { ',' };
                        string[] str = ViewState["fld"].ToString().Split(ch);
                        char[] ch1 = { ',' };
                        string[] str1 = ViewState["fld_uuid"].ToString().Split(ch1);
                        for (var i = 0; i < str.Length; i++)
                        {
                            cmd = new SqlCommand("update UserRights set Permission='" + mPermission + "' where NodeUUID='" + str[i].ToString() + "' and NodeType='Folder' and UserID=" + mUserID + " and CompCode='" + Session["CompCode"].ToString() + "'", con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (ViewState["drw"] != null)
                        {
                            char[] ch = { ',' };
                            string[] str = ViewState["drw"].ToString().Split(ch);
                            char[] ch1 = { ',' };
                            string[] str1 = ViewState["drw_uuid"].ToString().Split(ch1);
                            for (var i = 0; i < str.Length; i++)
                            {
                                cmd = new SqlCommand("update UserRights set Permission='" + mPermission + "' where NodeUUID='" + str[i].ToString() + "' and NodeType='Drawer' and UserID=" + mUserID + " and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            if (ViewState["cab"] != null)
                            {
                                char[] ch = { ',' };
                                string[] str = ViewState["cab"].ToString().Split(ch);
                                char[] ch1 = { ',' };
                                string[] str1 = ViewState["cab_uuid"].ToString().Split(ch1);
                                for (var i = 0; i < str.Length; i++)
                                {
                                    cmd = new SqlCommand("update UserRights set Permission='" + mPermission + "' where NodeUUID='" + str[i].ToString() + "' and NodeType='Cabinet' and UserID=" + mUserID + " and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    cmd.ExecuteNonQuery();
                                    #region PermissionLog
                                    //SqlConnection con = Utility.GetConnection();
                                    //SqlCommand cmd = null;
                                    //con.Open();
                                    //cmd = new SqlCommand("update PermissionLog set Permission='" + mPermission + "',UpdatedOn='" + DateTime.Now + "' where NodeUUID='" + str[i].ToString() + "' and NodeType='Cabinet' and UserID=" + mUserID + " and CompCode='" + Session["CompCode"].ToString() + "'", con);
                                    //cmd.ExecuteNonQuery();
                                    //Utility.CloseConnection(con);
                                    #endregion
                                }
                            }
                        }                        
                    }
                }
                Utility.CloseConnection(con);
                ViewState["cab"] = null;
                ViewState["drw"] = null;
                ViewState["fld"] = null;
                ViewState["doc"] = null;
                populateCabinet();
                gvDrawer.DataSource = null;
                gvDrawer.DataBind();
                gvFolder.DataSource = null;
                gvFolder.DataBind();
                gvDoc.DataSource = null;
                gvDoc.DataBind();
                RoleChecked();
                //MessageBox("Cabinet :" + ViewState["cab"].ToString() + "Drawer : " + ViewState["drw"].ToString() + "Folder :" + ViewState["fld"].ToString() + "Docs :" + selectedDet);
                MessageBox("Permission Updated Successfully.");
                
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Populate the GridView to display the entered records with Store Procedure Name:<GV_UserRights>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateGridView(string chkID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                string mUserID = "";
                /// Fetch the user_id start
                cmd = new SqlCommand("select user_id from user_role where role_id='" + chkID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                DataSet ds001 = new DataSet();
                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                adapter001.Fill(ds001);
                if (ds001.Tables[0].Rows.Count > 0)
                {
                    mUserID = ds001.Tables[0].Rows[0][0].ToString();
                }
                /// Fetch the user_id end
                
                /// Cabinet
                //cmd = new SqlCommand("select b.NodeUUID,a.cab_name from cabinet_mast a,UserRights b where a.cab_uuid=b.NodeUUID and b.Permission!='X' and b.UserID='" + mUserID + "' order by a.cab_name", con);
                cmd = new SqlCommand("select b.NodeUUID,a.cab_name,b.Permission from cabinet_mast a,UserRights b where a.cab_uuid=b.NodeUUID and b.UserID='" + mUserID + "' order by a.cab_name", con);
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                gvDispCab.DataSource = ds;
                gvDispCab.DataBind();

                /// Drawer
                cmd = new SqlCommand("select b.NodeUUID,a.drw_name,b.Permission from drawer_mast a,UserRights b where a.drw_uuid=b.NodeUUID and b.UserID='" + mUserID + "' and a.cab_uuid not in(select NodeUUID from UserRights where Permission='X' and UserID='" + mUserID + "' and NodeType='Cabinet') order by a.drw_name", con);
                DataSet ds1 = new DataSet();
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);
                adapter1.Fill(ds1);
                gvDispDrw.DataSource = ds1;
                gvDispDrw.DataBind();

                /// Folder
                cmd = new SqlCommand("select b.NodeUUID,a.fld_name,b.Permission from folder_mast a,UserRights b where a.fld_uuid=b.NodeUUID and b.UserID='" + mUserID + "' and a.fld_uuid not in(select NodeUUID from UserRights where Permission='X' and UserID='" + mUserID + "' and NodeType='Drawer') order by a.fld_name", con);
                DataSet ds2 = new DataSet();
                SqlDataAdapter adapter2 = new SqlDataAdapter(cmd);
                adapter2.Fill(ds2);
                gvDispFld.DataSource = ds2;
                gvDispFld.DataBind();

                /// Document
                cmd = new SqlCommand("select b.NodeUUID,a.doc_name,b.Permission from doc_mast a,UserRights b where a.uuid=b.NodeUUID and b.UserID='" + mUserID + "' and a.fld_uuid not in(select NodeUUID from UserRights where Permission='X' and UserID='" + mUserID + "' and NodeType='Folder') order by a.doc_name", con);
                DataSet ds3 = new DataSet();
                SqlDataAdapter adapter3 = new SqlDataAdapter(cmd);
                adapter3.Fill(ds3);
                gvDispDoc.DataSource = ds3;
                gvDispDoc.DataBind();
                Utility.CloseConnection(con);
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispCab_RowEditing(object sender, GridViewEditEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To update a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispCab_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To cancel after clicking on edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispCab_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Row updating event in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispCab_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// To delete a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispCab_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                con.Open();

                int count = 0;
                string chkID = "";
                foreach (GridViewRow row1 in gvRole.Rows)
                {
                    CheckBox checkbox = (CheckBox)row1.FindControl("CheckBox2");

                    if (checkbox.Checked)
                    {
                        Label lbRoleID = (Label)row1.FindControl("lbAutoIDRole");
                        chkID = lbRoleID.Text;
                        count += 1;
                    }
                }
                if (count == 1)
                {
                    string mUserID = "";
                    /// Fetch the user_id start
                    cmd = new SqlCommand("select user_id from user_role where role_id='" + chkID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    DataSet ds001 = new DataSet();
                    SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                    adapter001.Fill(ds001);
                    if (ds001.Tables[0].Rows.Count > 0)
                    {
                        mUserID = ds001.Tables[0].Rows[0][0].ToString();
                    }
                    /// Fetch the user_id end

                    GridViewRow row = (GridViewRow)gvDispCab.Rows[e.RowIndex];
                    Label lbCabID = (Label)row.FindControl("lbCabID");

                    user_rights_dal OBJ_RightsDAL = new user_rights_dal();

                    cmd = new SqlCommand("update UserRights set Permission='X' where NodeUUID='" + lbCabID.Text.Trim() + "' and UserID='" + mUserID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();

                    gvDispCab.EditIndex = -1;

                    PopulateGridView(chkID);
                    Utility.CloseConnection(con);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDrw_RowEditing(object sender, GridViewEditEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To update a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDrw_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To cancel after clicking on edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDrw_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Row updating event in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDrw_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// To delete a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDrw_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                con.Open();

                int count = 0;
                string chkID = "";
                foreach (GridViewRow row1 in gvRole.Rows)
                {
                    CheckBox checkbox = (CheckBox)row1.FindControl("CheckBox2");

                    if (checkbox.Checked)
                    {
                        Label lbRoleID = (Label)row1.FindControl("lbAutoIDRole");
                        chkID = lbRoleID.Text;
                        count += 1;
                    }
                }
                if (count == 1)
                {

                    string mUserID = "";
                    /// Fetch the user_id start
                    cmd = new SqlCommand("select user_id from user_role where role_id='" + chkID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    DataSet ds001 = new DataSet();
                    SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                    adapter001.Fill(ds001);
                    if (ds001.Tables[0].Rows.Count > 0)
                    {
                        mUserID = ds001.Tables[0].Rows[0][0].ToString();
                    }
                    /// Fetch the user_id end

                    GridViewRow row = (GridViewRow)gvDispDrw.Rows[e.RowIndex];
                    Label lbDrwID = (Label)row.FindControl("lbDrwID");

                    cmd = new SqlCommand("update UserRights set Permission='X' where NodeUUID='" + lbDrwID.Text.Trim() + "' and UserID='" + mUserID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();

                    gvDispCab.EditIndex = -1;
                    PopulateGridView(chkID);
                    Utility.CloseConnection(con);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispFld_RowEditing(object sender, GridViewEditEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To update a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispFld_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To cancel after clicking on edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispFld_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Row updating event in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispFld_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// To delete a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispFld_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                con.Open();

                int count = 0;
                string chkID = "";
                foreach (GridViewRow row1 in gvRole.Rows)
                {
                    CheckBox checkbox = (CheckBox)row1.FindControl("CheckBox2");

                    if (checkbox.Checked)
                    {
                        Label lbRoleID = (Label)row1.FindControl("lbAutoIDRole");
                        chkID = lbRoleID.Text;
                        count += 1;
                    }
                }
                if (count == 1)
                {
                    string mUserID = "";
                    /// Fetch the user_id start
                    cmd = new SqlCommand("select user_id from user_role where role_id='" + chkID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    DataSet ds001 = new DataSet();
                    SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                    adapter001.Fill(ds001);
                    if (ds001.Tables[0].Rows.Count > 0)
                    {
                        mUserID = ds001.Tables[0].Rows[0][0].ToString();
                    }
                    /// Fetch the user_id end

                    GridViewRow row = (GridViewRow)gvDispFld.Rows[e.RowIndex];
                    Label lbFldID = (Label)row.FindControl("lbFldID");

                    cmd = new SqlCommand("update UserRights set Permission='X' where NodeUUID='" + lbFldID.Text.Trim() + "' and UserID='" + mUserID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();

                    gvDispCab.EditIndex = -1;
                    PopulateGridView(chkID);
                    Utility.CloseConnection(con);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDoc_RowEditing(object sender, GridViewEditEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To update a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDoc_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// To cancel after clicking on edit a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDoc_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Row updating event in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// To delete a record in the gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispDoc_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                con.Open();

                int count = 0;
                string chkID = "";
                foreach (GridViewRow row1 in gvRole.Rows)
                {
                    CheckBox checkbox = (CheckBox)row1.FindControl("CheckBox2");

                    if (checkbox.Checked)
                    {
                        Label lbRoleID = (Label)row1.FindControl("lbAutoIDRole");
                        chkID = lbRoleID.Text;
                        count += 1;
                    }
                }
                if (count == 1)
                {
                    string mUserID = "";
                    /// Fetch the user_id start
                    cmd = new SqlCommand("select user_id from user_role where role_id='" + chkID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    DataSet ds001 = new DataSet();
                    SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                    adapter001.Fill(ds001);
                    if (ds001.Tables[0].Rows.Count > 0)
                    {
                        mUserID = ds001.Tables[0].Rows[0][0].ToString();
                    }
                    /// Fetch the user_id end

                    GridViewRow row = (GridViewRow)gvDispDoc.Rows[e.RowIndex];
                    Label lbDocID = (Label)row.FindControl("lbDocID");

                    cmd = new SqlCommand("update UserRights set Permission='X' where NodeUUID='" + lbDocID.Text.Trim() + "' and UserID='" + mUserID + "' and CompCode='" + Session["CompCode"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();

                    gvDispCab.EditIndex = -1;
                    PopulateGridView(chkID);
                    Utility.CloseConnection(con);
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }


    }
}