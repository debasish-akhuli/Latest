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
using System.Net;

namespace DMS
{
    public partial class MsgDisp : System.Web.UI.Page
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
            string file3 = Server.MapPath("TempDownload") + "\\log1.txt";
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
                    Page.Header.DataBind();

                    HttpRequest pdfRequest = Request;
                    HttpResponse pdfResponse = Response;
                    var istream = Request.InputStream;
                    ///Check the page content
                    //Request.SaveAs(Server.MapPath("~/Test.txt"), true);

                    ///PDF stream/content itself submited back to that page
                    FdfReader fdf = new FdfReader(istream);
                    // Now save the Rawdata into database
                    #region Fresh Template, has not started Workflow yet
                    if (Session["hfPageControl"].ToString() == "F")
                    {
                        hfPageControl.Value = "F";
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

                        int Result = QP.UnlockKey(LicenseKey);
                        if (Result == 1)
                        {
                            // Load the PDF form from the folder
                            QP.LoadFromFile(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString(), "");
                            // Count the number of form fields in the loaded document
                            int FieldCountAcroForms = QP.FormFieldCount();
                            if (Session["SelDocUUID"] != null)
                            {
                                SqlConnection con = Utility.GetConnection();
                                SqlCommand cmd = null;
                                DataSet ds001 = new DataSet();
                                con.Open();
                                cmd = new SqlCommand("select dept_id from doc_mast where uuid='" + Session["SelDocUUID"].ToString() + "'", con);
                                SqlDataAdapter adapter001 = new SqlDataAdapter(cmd);
                                adapter001.Fill(ds001);
                                if (ds001.Tables[0].Rows[0][0].ToString() == "NA")
                                {
                                    for (int i = FieldCountAcroForms; i > FieldCountAcroForms - 2; i--)
                                    {
                                        QP.DeleteFormField(i);
                                    }
                                }
                                con.Close();
                                Utility.CloseConnection(con);
                            }


                            // Count the number of pages in the document
                            int TotalPages = QP.PageCount();
                            for (int f = 1; f <= fdf.Fields.Count - 1; f++)
                            {
                                QP.SetFormFieldValue(f, fdf.GetFieldValue(f.ToString()));
                            }
                            // Sign Fields Update
                            DataSet ds = null;
                            ds = (DataSet)Session["dsSignFlds"];
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) > 0)
                                {
                                    if (Session["AccessControl"].ToString() == "Outside")
                                    {
                                        Session["InitiatorEmailID"] = fdf.GetFieldValue("1");
                                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()), fdf.GetFieldValue("2"));
                                    }
                                    else
                                    {
                                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()), Session["UserFullName"].ToString());
                                    }
                                }
                                if (Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString()) > 0)
                                {
                                    QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString()), DateTime.Now.ToString());
                                }
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()), "");
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()), "");
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()), "");
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString()), "");
                                //if (Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()) > 0)
                                //{
                                //    if (Session["AccessControl"].ToString() == "Outside")
                                //    {
                                //        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()), fdf.GetFieldValue("2"));
                                //    }
                                //    else
                                //    {
                                //        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()), Session["UserFullName"].ToString());
                                //    }
                                //}
                                //if (Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()) > 0)
                                //{
                                //    QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()), DateTime.Now.ToString());
                                //}
                                //if (Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()) > 0)
                                //{
                                //    if (Session["AccessControl"].ToString() == "Outside")
                                //    {
                                //        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()), fdf.GetFieldValue("2"));
                                //    }
                                //    else
                                //    {
                                //        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()), Session["UserFullName"].ToString());
                                //    }
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

                            // Save the updated form
                            string NewFile = ""; //Guid.NewGuid() + ".pdf";
                            string CurrTimeStamp = "";
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
                            //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                            Session["NewFile"] = NewFile;
                            //Session["TemplateUUID"] = Session["SelDocUUID"];
                            hfDocGuID.Value = Session["NewFile"].ToString();
                            //Response.Redirect("doc_mast.aspx?NewFile=" + NewFile + "&TemplateUUID=" + Session["SelDocUUID"].ToString(), false);
                            //if (Session["SelDocUUID"] != null)
                            //{
                            //    // Entered form uploading into Alfresco and then start the default assigned workflow
                            //    UploadNewFile(NewFile, Session["SelDocUUID"].ToString());
                            //    // Delete the file as the work has been done
                            //    //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["H2DDoc1"].ToString());
                            //    //File.Delete(Server.MapPath("TempDownload") + "\\" + NewFile);
                            //}
                        }
                        else
                        {
                            MessageBox("Invalid Quick PDF License Key");
                        }
                    }
                    #endregion
                    #region Exist Doc, has not started Workflow yet
                    else if (Session["hfPageControl"].ToString() == "FE")
                    {
                        hfPageControl.Value = "FE";
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

                        int Result = QP.UnlockKey(LicenseKey);
                        if (Result == 1)
                        {
                            // Load the PDF form from the folder
                            QP.LoadFromFile(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString(), "");
                            // Count the number of form fields in the loaded document
                            int FieldCountAcroForms = QP.FormFieldCount();


                            for (int i = FieldCountAcroForms; i > FieldCountAcroForms - 2; i--)
                            {
                                QP.DeleteFormField(i);
                            }



                            // Count the number of pages in the document
                            int TotalPages = QP.PageCount();
                            for (int f = 1; f <= fdf.Fields.Count - 1; f++)
                            {
                                QP.SetFormFieldValue(f, fdf.GetFieldValue(f.ToString()));
                            }
                            DataSet ds = FetchSignFlds4mTempDoc(Session["OpenDocName"].ToString());
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()), "");
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()), "");
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()), "");
                                QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString()), "");
                            }

                            // Save the updated form
                            string NewFile = Session["OpenDocName"].ToString(); //Guid.NewGuid() + ".pdf";                            
                            QP.SaveToFile(Server.MapPath("TempDownload") + "\\" + NewFile);
                            // Delete the Template File
                            //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                            Session["NewFile"] = NewFile;
                            //Session["TemplateUUID"] = Session["SelDocUUID"];
                            hfDocGuID.Value = Session["NewFile"].ToString();
                            //Response.Redirect("doc_mast.aspx?NewFile=" + NewFile + "&TemplateUUID=" + Session["SelDocUUID"].ToString(), false);
                            //if (Session["SelDocUUID"] != null)
                            //{
                            //    // Entered form uploading into Alfresco and then start the default assigned workflow
                            //    UploadNewFile(NewFile, Session["SelDocUUID"].ToString());
                            //    // Delete the file as the work has been done
                            //    //File.Delete(Server.MapPath("TempDownload") + "\\" + Session["H2DDoc1"].ToString());
                            //    //File.Delete(Server.MapPath("TempDownload") + "\\" + NewFile);
                            //}
                        }
                        else
                        {
                            MessageBox("Invalid Quick PDF License Key");
                        }
                    }
                    #endregion
                    #region Running Workflow
                    else if (Session["hfPageControl"].ToString() == "R")
                    {
                        hfPageControl.Value = "R";
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

                        int Result = QP.UnlockKey(LicenseKey);
                        if (Result == 1)
                        {
                            // Load a PDF form from the folder
                            QP.LoadFromFile(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString(), "");
                            // Count the number of form fields in the loaded document
                            int FieldCountAcroForms = QP.FormFieldCount();
                            // Count the number of pages in the document
                            int TotalPages = QP.PageCount();
                            //int jj = 1;
                            for (int f = 1; f <= fdf.Fields.Count - 1; f++)
                            {
                                if (fdf.GetFieldValue(f.ToString()) != null && fdf.GetFieldValue(f.ToString()) != "" && fdf.GetFieldValue(f.ToString()) != "Off")
                                {
                                    QP.SetFormFieldValue(f, fdf.GetFieldValue(f.ToString()));
                                }

                                //#region for signature & date fields
                                //if (fdf.GetFieldValue(f.ToString()) != null && fdf.GetFieldValue(f.ToString()) != "")
                                //{
                                //    QP.SetFormFieldValue(f, fdf.GetFieldValue(f.ToString()));
                                //    jj++;
                                //}
                                //else
                                //{
                                //    // Sign Fields Update
                                //    DataSet ds = null;
                                //    ds = (DataSet)Session["dsSignFlds"];
                                //    if (ds.Tables[0].Rows.Count > 0)
                                //    {
                                //        if (Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString()) > 0 && Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()) > 0)
                                //        {
                                //            if (f == Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString()))
                                //            {
                                //                QP.SetFormFieldValue(jj, Session["UserFullName"].ToString());
                                //                jj++;
                                //            }
                                //            if (f == Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()))
                                //            {
                                //                QP.SetFormFieldValue(jj, DateTime.Now.ToString());
                                //                jj++;
                                //            }
                                //        }
                                //    }
                                //}
                                //#endregion
                            }
                            DataSet ds = null;
                            ds = (DataSet)Session["dsSignFlds"];
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (Session["StageNo"].ToString() == "2")
                                {
                                    if (Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()) > 0)
                                    {
                                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString()), Session["UserFullName"].ToString());
                                    }
                                    if (Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()) > 0)
                                    {
                                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString()), DateTime.Now.ToString());
                                    }
                                }
                                else if (Session["StageNo"].ToString() == "3")
                                {
                                    if (Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()) > 0)
                                    {
                                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString()), Session["UserFullName"].ToString());
                                    }
                                    if (Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString()) > 0)
                                    {
                                        QP.SetFormFieldValue(Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString()), DateTime.Now.ToString());
                                    }
                                }
                            }

                            QP.SaveToFile(Server.MapPath("TempDownload") + "\\" + Session["OpenDocName"].ToString());
                            hfDocGuID.Value = Session["OpenDocName"].ToString();
                        }
                        else
                        {
                            MessageBox("Invalid Quick PDF License Key");
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
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

        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();

            return ImageData; //return the byte data
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

    }
}