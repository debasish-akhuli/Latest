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
    public partial class CompWiseStatistics : System.Web.UI.Page
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
                            PopulateGridView();
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

        //protected void gvDispRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    if (Session["UserID"] != null && Session["Ticket"] != null)
        //    {

        //    }
        //    else
        //    {
        //        Response.Redirect("SessionExpired.aspx", false);
        //    }
        //    gvDispRec.PageIndex = e.NewPageIndex;
        //    PopDailyStat(Session["strCompCode"].ToString());
        //}

        protected void gvDispRecTot_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["UserID"] != null && Session["Ticket"] != null)
            {

            }
            else
            {
                Response.Redirect("SessionExpired.aspx", false);
            }
            gvDispRecTot.PageIndex = e.NewPageIndex;
            PopulateGridView();
        }

        protected void PopulateGridView()
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                SqlDataAdapter adapter01;
                DataSet ds01 = new DataSet();
                DataTable dt03 = null;

                cmd = new SqlCommand("select CompCode,CompName,ROUND(TotalSpace,2) as TotalSpace,ROUND(UsedSpace,2) as UsedSpace,ROUND(AvailableSpace, 2) as AvailableSpace,MaxNoOfUsers,SpaceRate,UserRate,TotalRate,CreationDate from ServerConfig where CompCode not in('00000000') order by CompName", con);
                adapter01 = new SqlDataAdapter(cmd);
                ds01.Reset();
                adapter01.Fill(ds01);
                dt03 = CreateDT();
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        DataRow r3 = dt03.NewRow();
                        cmd = new SqlCommand("select count(*) as Tot from user_mast where CompCode='" + ds01.Tables[0].Rows[i][0].ToString() + "' and user_stat='A'", con);
                        int ExistingUsers = Convert.ToInt32(cmd.ExecuteScalar());

                        dt03.Rows.Add(AddNewRow(r3, ds01.Tables[0].Rows[i][0].ToString(), ds01.Tables[0].Rows[i][1].ToString(), Convert.ToDouble(ds01.Tables[0].Rows[i][2].ToString()), Convert.ToDouble(ds01.Tables[0].Rows[i][3].ToString()), Convert.ToDouble(ds01.Tables[0].Rows[i][4].ToString()), Convert.ToInt16(ds01.Tables[0].Rows[i][5].ToString()), Convert.ToInt16(ExistingUsers), Convert.ToDouble(ds01.Tables[0].Rows[i][6].ToString()), Convert.ToDouble(ds01.Tables[0].Rows[i][7].ToString()), Convert.ToDouble(ds01.Tables[0].Rows[i][8].ToString())));
                    }
                }
                gvDispRecTot.DataSource = dt03;
                gvDispRecTot.DataBind();

                //DataSet ds02 = new DataSet();
                //DataSet dsCompList = new DataSet();
                //string BillDesc = "";

                //cmd = new SqlCommand("select * from DailyUserDocStat where MONTH(ProcessDate)='" + ddMonth.SelectedValue + "' and YEAR(ProcessDate)='" + ddYear.SelectedValue + "'", con);
                //adapter01 = new SqlDataAdapter(cmd);
                //ds01.Reset();
                //adapter01.Fill(ds01);
                //if (ds01.Tables[0].Rows.Count > 0)
                //{

                //}
                //else
                //{
                //    gvDispRec.DataSource = null;
                //    gvDispRec.DataBind();
                //    throw new Exception("Record is not updated for this month & year.");
                //}

                //// Month wise
                ////cmd = new SqlCommand("select a.CompCode,a.CompName,a.BillTypeID,b.BillTypeDesc,b.MaxNoOfUsers,b.MaxNoOfDocs,b.OldDocRate,b.NewDocRate,b.RWDRate,b.RWRate,b.RRate,b.FlatUserRate,b.FlatDocRate,b.BlockRate from ServerConfig a,BillTypeMast b where a.BillTypeID=b.BillTypeID and a.CompCode not in('00000000') order by a.CompName", con);
                //cmd = new SqlCommand("select a.CompCode,a.CompName,a.BillTypeID,b.BillTypeDesc,a.MaxNoOfUsers,a.MaxNoOfDocs,a.OldDocRate,a.NewDocRate,a.RWDRate,a.RWRate,a.RRate,a.FlatUserRate,a.FlatDocRate,a.BlockRate,b.ExtraDocRate from ServerConfig a,BillTypeMast b where a.BillTypeID=b.BillTypeID and a.CompCode not in('00000000') order by a.CompName", con);
                //adapter01 = new SqlDataAdapter(cmd);
                //dsCompList.Reset();
                //adapter01.Fill(dsCompList);
                //DataTable dt03 = null;
                //dt03 = CreateDT();
                //if (dsCompList.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < dsCompList.Tables[0].Rows.Count; i++)
                //    {
                //        if (dsCompList.Tables[0].Rows[i][2].ToString() == "B01")
                //        {
                //            BillDesc = "Scheme: " + dsCompList.Tables[0].Rows[i][3].ToString() + "\n\nOld Doc Rate: " + dsCompList.Tables[0].Rows[i][6].ToString() + "\n\nNew Doc Rate: " + dsCompList.Tables[0].Rows[i][7].ToString();
                //        }
                //        else if (dsCompList.Tables[0].Rows[i][2].ToString() == "B02")
                //        {
                //            BillDesc = "Scheme: " + dsCompList.Tables[0].Rows[i][3].ToString() + "\n\nUser Rate(Read, Write & Delete Permission): " + dsCompList.Tables[0].Rows[i][8].ToString() + "\n\nUser Rate(Read & Write Permission): " + dsCompList.Tables[0].Rows[i][9].ToString() + "\n\nUser Rate(Only Read Permission): " + dsCompList.Tables[0].Rows[i][10].ToString();
                //        }
                //        else if (dsCompList.Tables[0].Rows[i][2].ToString() == "B03")
                //        {
                //            BillDesc = "Scheme: " + dsCompList.Tables[0].Rows[i][3].ToString() + "\n\nMax # of Users: " + dsCompList.Tables[0].Rows[i][4].ToString() + "\n\nMax # of Docs: " + dsCompList.Tables[0].Rows[i][5].ToString() + "\n\nFlat User Rate: " + dsCompList.Tables[0].Rows[i][11].ToString() + "\n\nExtra Doc Rate: " + dsCompList.Tables[0].Rows[i][14].ToString();
                //        }
                //        else if (dsCompList.Tables[0].Rows[i][2].ToString() == "B04")
                //        {
                //            BillDesc = "Scheme: " + dsCompList.Tables[0].Rows[i][3].ToString() + "\n\nMax # of Docs: " + dsCompList.Tables[0].Rows[i][5].ToString() + "\n\nFlat Doc Rate: " + dsCompList.Tables[0].Rows[i][12].ToString() + "\n\nExtra Doc Rate: " + dsCompList.Tables[0].Rows[i][14].ToString();
                //        }
                //        else if (dsCompList.Tables[0].Rows[i][2].ToString() == "B05")
                //        {
                //            BillDesc = "Scheme: " + dsCompList.Tables[0].Rows[i][3].ToString() + "\n\nMax # of Docs per User: " + dsCompList.Tables[0].Rows[i][5].ToString() + "\n\nFlat User Rate: " + dsCompList.Tables[0].Rows[i][11].ToString() + "\n\nExtra Doc Rate: " + dsCompList.Tables[0].Rows[i][14].ToString();
                //        }
                //        else if (dsCompList.Tables[0].Rows[i][2].ToString() == "B06")
                //        {
                //            BillDesc = "Scheme: " + dsCompList.Tables[0].Rows[i][3].ToString() + "\n\nBlock Rate: " + dsCompList.Tables[0].Rows[i][13].ToString();
                //        }
                //        cmd = new SqlCommand("select NoOfOldDocs as TotOldDocs from DailyUserDocStat where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and MONTH(ProcessDate)='" + ddMonth.SelectedValue + "' and YEAR(ProcessDate)='" + ddYear.SelectedValue + "'", con);
                //        adapter01 = new SqlDataAdapter(cmd);
                //        ds02.Reset();
                //        adapter01.Fill(ds02);

                //        cmd = new SqlCommand("select max(NoOfUsers) as TotUser from DailyUserDocStat where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and MONTH(ProcessDate)='" + ddMonth.SelectedValue + "' and YEAR(ProcessDate)='" + ddYear.SelectedValue + "'", con);
                //        int TotNumber = Convert.ToInt32(cmd.ExecuteScalar());

                //        cmd = new SqlCommand("select max(NoOfNewDocs) as TotNewDocs from DailyUserDocStat where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and MONTH(ProcessDate)='" + ddMonth.SelectedValue + "' and YEAR(ProcessDate)='" + ddYear.SelectedValue + "'", con);
                //        int TotNewDocs = Convert.ToInt32(cmd.ExecuteScalar());

                //        DataRow r3 = dt03.NewRow();
                //        cmd = new SqlCommand("select a.CompCode,sum(a.NoOfUsers) as TotUser,SUM(a.NoOfNewDocs) as TotNewDocs,SUM(a.NoOfSuperUser) as TotRWD, SUM(a.NoOfAdmin) as TotRW, SUM(a.NoOfReader) as TotR,CAST(ROUND(SUM(a.BillAmt), 2) AS DECIMAL(9,2)) as TotAmt from DailyUserDocStat a,ServerConfig b where a.CompCode=b.CompCode and a.CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and MONTH(a.ProcessDate)='" + ddMonth.SelectedValue + "' and YEAR(a.ProcessDate)='" + ddYear.SelectedValue + "' group by a.CompCode,MONTH(a.ProcessDate),YEAR(a.ProcessDate)", con);
                //        adapter01 = new SqlDataAdapter(cmd);
                //        ds01.Reset();
                //        adapter01.Fill(ds01);
                //        if (ds01.Tables[0].Rows.Count > 0)
                //        {
                //            //Adding row in Data Table
                //            dt03.Rows.Add(AddNewRow(r3, dsCompList.Tables[0].Rows[i][0].ToString(), dsCompList.Tables[0].Rows[i][1].ToString(), dsCompList.Tables[0].Rows[i][2].ToString(), BillDesc, Convert.ToInt16(TotNumber), Convert.ToInt16(ds02.Tables[0].Rows[0][0].ToString()), Convert.ToInt16(TotNewDocs), Convert.ToInt16(ds01.Tables[0].Rows[0][3].ToString()), Convert.ToInt16(ds01.Tables[0].Rows[0][4].ToString()), Convert.ToInt16(ds01.Tables[0].Rows[0][5].ToString()), Convert.ToDouble(ds01.Tables[0].Rows[0][6].ToString())));
                //        }
                //    }
                //}
                //gvDispRecTot.DataSource = dt03;
                //gvDispRecTot.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }

        public DataTable CreateDT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CompCode", typeof(string));
            dt.Columns.Add("CompName", typeof(string));
            dt.Columns.Add("TotalSpace", typeof(float));
            dt.Columns.Add("UsedSpace", typeof(float));
            dt.Columns.Add("AvailableSpace", typeof(float));
            dt.Columns.Add("MaxNoOfUsers", typeof(Int64));
            dt.Columns.Add("ExistingUsers", typeof(Int64));
            dt.Columns.Add("SpaceRate", typeof(float));
            dt.Columns.Add("UserRate", typeof(float));
            dt.Columns.Add("TotalRate", typeof(float));
            return dt;
        }

        public DataRow AddNewRow(DataRow r3, string CompCode, string CompName, double TotalSpace, double UsedSpace, double AvailableSpace, int MaxNoOfUsers, int ExistingUsers, double SpaceRate, double UserRate, double TotalRate)
        {
            r3["CompCode"] = CompCode;
            r3["CompName"] = CompName;
            r3["TotalSpace"] = TotalSpace;
            r3["UsedSpace"] = UsedSpace;
            r3["AvailableSpace"] = AvailableSpace;
            r3["MaxNoOfUsers"] = MaxNoOfUsers;
            r3["ExistingUsers"] = ExistingUsers;
            r3["SpaceRate"] = SpaceRate;
            r3["UserRate"] = UserRate;
            r3["TotalRate"] = TotalRate;
            return r3;
        }

        //protected void PopDailyStat(string CompCode)
        //{
        //    try
        //    {
        //        SqlConnection con = Utility.GetConnection();
        //        SqlCommand cmd = null;
        //        con.Open();
        //        SqlDataAdapter adapter01;
        //        DataSet ds01 = new DataSet();
        //        // Daily
        //        //cmd = new SqlCommand("select a.CompCode,CONVERT(VARCHAR(10), a.ProcessDate, 120) as ProcessDate,a.NoOfUsers,a.NoOfOldDocs,a.NoOfNewDocs,a.NoOfSuperUser,a.NoOfAdmin,a.NoOfReader,CAST(ROUND(a.BillAmt, 2) AS DECIMAL(9,2)) as BillAmt,b.CompName from CompanyDailyStat a,ServerConfig b where a.CompCode=b.CompCode and MONTH(a.ProcessDate)='" + ddMonth.SelectedValue + "' and YEAR(a.ProcessDate)='" + ddYear.SelectedValue + "' and a.CompCode='" + CompCode + "' order by a.ProcessDate desc,b.CompName", con);
        //        cmd = new SqlCommand("select a.CompCode,CONVERT(VARCHAR(10), a.ProcessDate, 120) as ProcessDate,a.NoOfUsers,a.NoOfOldDocs,a.NoOfNewDocs,a.NoOfSuperUser,a.NoOfAdmin,a.NoOfReader,CAST(ROUND(a.BillAmt, 2) AS DECIMAL(9,2)) as BillAmt,b.CompName from DailyUserDocStat a,ServerConfig b where a.CompCode=b.CompCode and MONTH(a.ProcessDate)='" + ddMonth.SelectedValue + "' and YEAR(a.ProcessDate)='" + ddYear.SelectedValue + "' and a.CompCode='" + CompCode + "' order by a.ProcessDate desc,b.CompName", con);
        //        adapter01 = new SqlDataAdapter(cmd);
        //        ds01.Reset();
        //        adapter01.Fill(ds01);
        //        gvDispRec.DataSource = ds01;
        //        gvDispRec.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox(ex.Message);
        //    }
        //}

        protected void gvDispRecTot_SelectedIndexChanged(object sender, EventArgs e)
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
                Session["strCompCode"] = gvDispRecTot.SelectedValue.ToString(); //gvDispRecTot.SelectedRow.Cells[0].Text;
                //PopDailyStat(Session["strCompCode"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
            }
        }
        
        //protected void cmdProcessData_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Session["UserID"] != null && Session["Ticket"] != null)
        //        {

        //        }
        //        else
        //        {
        //            Response.Redirect("SessionExpired.aspx", false);
        //        }
        //        SqlConnection con = Utility.GetConnection();
        //        SqlCommand cmd = null;
        //        con.Open();
        //        SqlDataAdapter adapter01;

        //        ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
        //        DataSet dsCompList = new DataSet();
        //        DataSet ds01 = new DataSet();
        //        DataSet ds02 = new DataSet();
        //        double OldDocRate = 0;
        //        double NewDocRate = 0;
        //        double RWDRate = 0;
        //        double RWRate = 0;
        //        double RRate = 0;
        //        double FlatUserRate = 0;
        //        double FlatDocRate = 0;
        //        double BlockRate = 0;
        //        double MaxNoOfUsers = 0;
        //        double MaxNoOfDocs = 0;
        //        double PerExtraDocRate = 0;

        //        int TotalNoOfDays = System.DateTime.DaysInMonth(Convert.ToInt32(ddYear.SelectedValue), Convert.ToInt32(ddMonth.SelectedValue));

        //        cmd = new SqlCommand("update DailyUserDocStat set BillAmt=0 where month(ProcessDate)='" + ddMonth.SelectedValue + "' and year(ProcessDate)='" + ddYear.SelectedValue + "'", con);
        //        cmd.ExecuteNonQuery();

        //        cmd = new SqlCommand("select * from DailyUserDocStat where month(ProcessDate)='" + ddMonth.SelectedValue + "' and year(ProcessDate)='" + ddYear.SelectedValue + "' order by ProcessDate", con);
        //        adapter01 = new SqlDataAdapter(cmd);
        //        dsCompList.Reset();
        //        adapter01.Fill(dsCompList);
        //        if (dsCompList.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dsCompList.Tables[0].Rows.Count; i++)
        //            {
        //                cmd = new SqlCommand("select count(*) as Tot from PermissionLog where NodeType='Cabinet' and Permission='D' and CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and UpdatedOn>='" + dsCompList.Tables[0].Rows[i][1].ToString() + "' and UpdatedOn<'" + Convert.ToDateTime(dsCompList.Tables[0].Rows[i][1].ToString()).AddDays(1) + "' and UserID in(select USER_ID from user_mast where UserType='N' and CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "')", con);
        //                int TotSuperAdmin = Convert.ToInt32(cmd.ExecuteScalar());
        //                cmd = new SqlCommand("select count(*) as Tot from PermissionLog where NodeType='Cabinet' and Permission='M' and CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and UpdatedOn>='" + dsCompList.Tables[0].Rows[i][1].ToString() + "' and UpdatedOn<'" + Convert.ToDateTime(dsCompList.Tables[0].Rows[i][1].ToString()).AddDays(1) + "' and UserID in(select USER_ID from user_mast where UserType='N' and CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "')", con);
        //                int TotAdmin = Convert.ToInt32(cmd.ExecuteScalar());
        //                cmd = new SqlCommand("select count(*) as Tot from PermissionLog where NodeType='Cabinet' and Permission='V' and CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and UpdatedOn>='" + dsCompList.Tables[0].Rows[i][1].ToString() + "' and UpdatedOn<'" + Convert.ToDateTime(dsCompList.Tables[0].Rows[i][1].ToString()).AddDays(1) + "' and UserID in(select USER_ID from user_mast where UserType='N' and CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "')", con);
        //                int TotReader = Convert.ToInt32(cmd.ExecuteScalar());
        //                cmd = new SqlCommand("update DailyUserDocStat set NoOfSuperUser='" + TotSuperAdmin + "',NoOfAdmin='" + TotAdmin + "',NoOfReader='" + TotReader + "' where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        cmd = new SqlCommand("select * from DailyUserDocStat where month(ProcessDate)='" + ddMonth.SelectedValue + "' and year(ProcessDate)='" + ddYear.SelectedValue + "' order by ProcessDate", con);
        //        adapter01 = new SqlDataAdapter(cmd);
        //        dsCompList.Reset();
        //        adapter01.Fill(dsCompList);
        //        if (dsCompList.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dsCompList.Tables[0].Rows.Count; i++)
        //            {
        //                cmd = new SqlCommand("select BillTypeID,OldDocRate,NewDocRate,RWDRate,RWRate,RRate,FlatUserRate,FlatDocRate,BlockRate,MaxNoOfUsers,MaxNoOfDocs from ServerConfig where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "'", con);
        //                adapter01 = new SqlDataAdapter(cmd);
        //                ds01.Reset();
        //                adapter01.Fill(ds01);
        //                if (ds01.Tables[0].Rows.Count > 0)
        //                {
        //                    OldDocRate = Convert.ToDouble(ds01.Tables[0].Rows[0][1].ToString()) / TotalNoOfDays;
        //                    NewDocRate = Convert.ToDouble(ds01.Tables[0].Rows[0][2].ToString()) / TotalNoOfDays;
        //                    RWDRate = Convert.ToDouble(ds01.Tables[0].Rows[0][3].ToString()) / TotalNoOfDays;
        //                    RWRate = Convert.ToDouble(ds01.Tables[0].Rows[0][4].ToString()) / TotalNoOfDays;
        //                    RRate = Convert.ToDouble(ds01.Tables[0].Rows[0][5].ToString()) / TotalNoOfDays;
        //                    FlatUserRate = Convert.ToDouble(ds01.Tables[0].Rows[0][6].ToString()) / TotalNoOfDays;
        //                    FlatDocRate = Convert.ToDouble(ds01.Tables[0].Rows[0][7].ToString()) / TotalNoOfDays;
        //                    BlockRate = Convert.ToDouble(ds01.Tables[0].Rows[0][8].ToString()) / TotalNoOfDays;
        //                    MaxNoOfUsers=Convert.ToDouble(ds01.Tables[0].Rows[0][9].ToString());
        //                    MaxNoOfDocs = Convert.ToDouble(ds01.Tables[0].Rows[0][10].ToString());
        //                    PerExtraDocRate = Convert.ToDouble(2) / TotalNoOfDays;
        //                }
        //                if (ds01.Tables[0].Rows[0][0].ToString() == "B01")
        //                {
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+( " + NewDocRate * Convert.ToDouble(dsCompList.Tables[0].Rows[i][4].ToString()) + ")), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+( " + OldDocRate * Convert.ToDouble(dsCompList.Tables[0].Rows[i][3].ToString()) + ")), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                }
        //                else if (ds01.Tables[0].Rows[0][0].ToString() == "B02")
        //                {
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+( " + RWDRate * Convert.ToDouble(dsCompList.Tables[0].Rows[i][5].ToString()) + ")), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+( " + RWRate * Convert.ToDouble(dsCompList.Tables[0].Rows[i][6].ToString()) + ")), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+( " + RRate * Convert.ToDouble(dsCompList.Tables[0].Rows[i][7].ToString()) + ")), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                }
        //                else if (ds01.Tables[0].Rows[0][0].ToString() == "B03")
        //                {
        //                    cmd = new SqlCommand("select SUM(NoOfOldDocs + NoOfNewDocs) from DailyUserDocStat where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate>='" + dsCompList.Tables[0].Rows[i][1].ToString() + "' and ProcessDate<'" + Convert.ToDateTime(dsCompList.Tables[0].Rows[i][1].ToString()).AddDays(1) + "'", con);
        //                    double TotNoOfDocs = Convert.ToDouble(Convert.ToInt32(cmd.ExecuteScalar()));
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+ " + FlatUserRate + "), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                    if (TotNoOfDocs > MaxNoOfDocs)
        //                    {
        //                        cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+ " + ((TotNoOfDocs - MaxNoOfDocs)*PerExtraDocRate) + "), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                        cmd.ExecuteNonQuery();
        //                    }
        //                }
        //                //else if (ds01.Tables[0].Rows[0][0].ToString() == "B04")
        //                //{
        //                //    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+ " + FlatUserRate + "), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                //    cmd.ExecuteNonQuery();
        //                //}
        //                else if (ds01.Tables[0].Rows[0][0].ToString() == "B04")
        //                {
        //                    cmd = new SqlCommand("select SUM(NoOfOldDocs + NoOfNewDocs) from DailyUserDocStat where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate>='" + dsCompList.Tables[0].Rows[i][1].ToString() + "' and ProcessDate<'" + Convert.ToDateTime(dsCompList.Tables[0].Rows[i][1].ToString()).AddDays(1) + "'", con);
        //                    double TotNoOfDocs = Convert.ToDouble(Convert.ToInt32(cmd.ExecuteScalar()));
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+ " + FlatDocRate + "), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                    if (TotNoOfDocs > MaxNoOfDocs)
        //                    {
        //                        cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+ " + ((TotNoOfDocs - MaxNoOfDocs) * PerExtraDocRate) + "), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                        cmd.ExecuteNonQuery();
        //                    }
        //                }
        //                else if (ds01.Tables[0].Rows[0][0].ToString() == "B05")
        //                {
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+( " + FlatUserRate * Convert.ToDouble(dsCompList.Tables[0].Rows[i][2].ToString()) + ")), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+( " + Convert.ToDouble(dsCompList.Tables[0].Rows[i][9].ToString()) * PerExtraDocRate + ")), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                }
        //                else if (ds01.Tables[0].Rows[0][0].ToString() == "B06")
        //                {
        //                    cmd = new SqlCommand("select SUM(NoOfOldDocs + NoOfNewDocs) from DailyUserDocStat where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate>='" + dsCompList.Tables[0].Rows[i][1].ToString() + "' and ProcessDate<'" + Convert.ToDateTime(dsCompList.Tables[0].Rows[i][1].ToString()).AddDays(1) + "'", con);
        //                    double TotNoOfDocs =Convert.ToDouble(Convert.ToInt32(cmd.ExecuteScalar()));
        //                    int BlockOfDocs = Convert.ToInt32(Math.Ceiling(TotNoOfDocs/5000));
        //                    cmd = new SqlCommand("update DailyUserDocStat set BillAmt=CAST(ROUND((BillAmt+ " + BlockRate * BlockOfDocs + "), 2) AS DECIMAL(9,2)) where CompCode='" + dsCompList.Tables[0].Rows[i][0].ToString() + "' and ProcessDate='" + dsCompList.Tables[0].Rows[i][1].ToString() + "'", con);
        //                    cmd.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //        PopulateGridView();
        //        throw new Exception("Data Processing has been completed successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox(ex.Message);
        //    }
        //}

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }


    }
}