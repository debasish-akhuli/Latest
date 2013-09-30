using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using System.Text;
using QuickPDFDLL0813;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using DMS.BAL;
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using System.Configuration;

namespace DMS
{
    public partial class DocSaving : System.Web.UI.Page
    {
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;
        private RepositoryService repoServiceA;
        PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDF64DLL0813.dll");
        //PDFLibrary QP = new PDFLibrary("C:\\Program Files (x86)\\Quick PDF Library\\DLL\\QuickPDFDLL0813.dll");

        public RepositoryService RepoService
        {
            set { repoService = value; }
        }
        public RepositoryService RepoServiceA
        {
            set { repoServiceA = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            // Set the session variables blank which are used to set the previous selected path start
            Session["SelectedCabUUID"] = "";
            Session["SelectedDrwUUID"] = "";
            Session["SelectedFldUUID"] = "";
            Session["SelectedDocID"] = "";
            // Set the session variables blank which are used to set the previous selected path end
            HttpRequest pdfRequest = Request;
            HttpResponse pdfResponse = Response;
            var istream = Request.InputStream;
            FdfReader fdf = new FdfReader(istream);
            string LicenseKey = "";
            string ServerIPAddress = "";
            // Fetch ServerConfig Details Start
            ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
            DataSet ds01 = new DataSet();
            ds01 = ObjClassStoreProc.SelectServerConfig(Session["CompCode"].ToString());
            if (ds01.Tables[0].Rows.Count > 0)
            {
                LicenseKey = ds01.Tables[0].Rows[0][0].ToString();
                ServerIPAddress = ds01.Tables[0].Rows[0][1].ToString();
            }
            // Fetch ServerConfig Details End


            // Save the updated form
            string NewFile = ""; //Guid.NewGuid() + ".pdf";
            string CurrTimeStamp = "";
            string TempDocName = "";
            string DocTypeID = "";
            string UserID = "";
            string TempDocStat = "";
            int Result;
            int FieldCountAcroForms;
            int TotalPages;
            SqlConnection con = Utility.GetConnection();

            if (Session["hfPageControl"].ToString() == "F") // Fresh Doc, has not started Workflow yet
            {
                Result = QP.UnlockKey(LicenseKey);
                if (Result == 1)
                {
                    // Load the PDF form from the folder
                    QP.LoadFromFile(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString(), "");
                    // Count the number of form fields in the loaded document
                    FieldCountAcroForms = QP.FormFieldCount();
                    // Count the number of pages in the document
                    TotalPages = QP.PageCount();
                    for (int f = 1; f <= fdf.Fields.Count - 1; f++)
                    {
                        QP.SetFormFieldValue(f, fdf.GetFieldValue(f.ToString()));
                    }
                    // Sign Fields Update
                    DataSet ds = null;
                    ds = (DataSet)Session["dsSignFlds"];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ////////if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) > 0)
                        ////////{
                        ////////    QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()), Session["UserFullName"].ToString());
                        ////////}
                        ////////if (Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString()) > 0)
                        ////////{
                        ////////    QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString()), DateTime.Now.ToString());
                        ////////}
                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()), "");
                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()), "");
                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()), "");
                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString()), "");
                        //if (Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()) > 0)
                        //{
                        //    QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()), Session["UserFullName"].ToString());
                        //}
                        //if (Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()) > 0)
                        //{
                        //    QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()), DateTime.Now.ToString());
                        //}
                        //if (Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()) > 0)
                        //{
                        //    QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()), Session["UserFullName"].ToString());
                        //}
                        //if (Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString()) > 0)
                        //{
                        //    QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString()), DateTime.Now.ToString());
                        //}
                        for (int kk = 1; kk < ds.Tables[0].Rows.Count; kk++)
                        {
                            if (ds.Tables[0].Rows[kk][1].ToString() != "" && ds.Tables[0].Rows[kk][2].ToString() != "")
                            {
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[kk][1].ToString()), "");
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[kk][2].ToString()), "");
                            }
                        }
                    }

                    if (Session["OpenDocName"].ToString().LastIndexOf(".pdf") == -1)
                    {
                        CurrTimeStamp = GetTimestamp(DateTime.Now);
                        NewFile = Session["OpenDocName"].ToString() + "_" + CurrTimeStamp + ".pdf";
                    }
                    else
                    {
                        CurrTimeStamp = GetTimestamp(DateTime.Now);
                        NewFile = Session["OpenDocName"].ToString().Substring(0, Session["OpenDocName"].ToString().Length - 4) + "_" + CurrTimeStamp + ".pdf";
                    }
                    QP.SaveToFile(Server.MapPath("TempDownload") + "\\" + NewFile);
                    // Delete the Template File
                    File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());

                    // Now save the doc user wise into the database
                    TempDocName = NewFile;
                    UserID = Session["UserID"].ToString();
                    TempDocStat = "Not Uploaded";
                    DocTypeID = Session["TempDocTypeID"].ToString();

                    SqlCommand cmd = null;
                    con.Open();
                    cmd = new SqlCommand("insert into TempDocSaving(TempDocName,UserID,DocTypeID,TempDocStat,CreationDate,TemplateUUID,CompCode) values('" + TempDocName + "','" + UserID + "','" + DocTypeID + "','" + TempDocStat + "','" + DateTime.Now + "','" + Session["SelDocUUID"].ToString() + "','" + Session["CompCode"].ToString() + "')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Utility.CloseConnection(con);
                }
                else
                {
                    MessageBox("Invalid Quick PDF License Key");
                }
            }
            else if (Session["hfPageControl"].ToString() == "FE") // Exist Doc, has not started Workflow yet
            {
                Result = QP.UnlockKey(LicenseKey);
                if (Result == 1)
                {
                    // Load the PDF form from the folder
                    QP.LoadFromFile(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString(), "");
                    // Count the number of form fields in the loaded document
                    FieldCountAcroForms = QP.FormFieldCount();
                    // Count the number of pages in the document
                    TotalPages = QP.PageCount();
                    for (int f = 1; f <= fdf.Fields.Count - 1; f++)
                    {
                        QP.SetFormFieldValue(f, fdf.GetFieldValue(f.ToString()));
                    }
                    SqlCommand cmd = null;
                    con.Open();
                    DataSet ds = FetchSignFlds4mTempDoc(Session["OpenDocName"].ToString());
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()), "");
                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()), "");
                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()), "");
                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString()), "");
                    }

                    NewFile = Session["OpenDocName"].ToString();
                    QP.SaveToFile(Server.MapPath("TempDownload") + "\\" + NewFile);

                    // Now save the doc user wise into the database
                    TempDocName = NewFile;
                    UserID = Session["UserID"].ToString();
                    cmd = new SqlCommand("update TempDocSaving set CreationDate='" + DateTime.Now + "' where TempDocName='" + TempDocName + "' and UserID='" + UserID + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Utility.CloseConnection(con);
                }
                else
                {
                    MessageBox("Invalid Quick PDF License Key");
                }
            }
        }

        private DataSet FetchSignFlds4mTempDoc(string TempDocName)
        {
            SqlConnection con = Utility.GetConnection();
            SqlCommand cmd = null;
            con.Open();
            DataSet ds001 = new DataSet();
            DataSet ds002 = new DataSet();
            ds001.Reset();
            cmd = new SqlCommand("select TemplateUUID,DocTypeID from TempDocSaving where TempDocName='" + TempDocName + "'", con);
            SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
            adapter001.Fill(ds001);
            if (ds001.Tables[0].Rows.Count > 0)
            {
                cmd = new SqlCommand("select SignFieldNo1,SignDateFieldNo1,SignFieldNo2,SignDateFieldNo2,SignFieldNo3,SignDateFieldNo3 from doc_type_mast where doc_type_id='" + ds001.Tables[0].Rows[0][1].ToString() + "'", con);
                SqlDataAdapter adapter002 = new SqlDataAdapter(cmd);
                adapter002.Fill(ds002);
            }
            Utility.CloseConnection(con);
            return ds002;
        }

        private String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

    }
}