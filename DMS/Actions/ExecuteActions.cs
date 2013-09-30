using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DMS.UTILITY;
using System.Net;
using System.IO;
using System.Configuration;
using System.Collections;

namespace DMS.Actions
{
    public class ExecuteActions
    {
        public void CheckAction(string WFLogID, int StepNo, DateTime TaskDoneDate, string TaskID, string Comments, string AccessControl, string InitiatorEmailID, string CompCode, string AdminUserID, string AdminLoginTicket)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText("C:\\LogFolder\\LogFile.txt"))
                {
                    Log("Error in ExecuteActions.CheckAction for CompCode: " + CompCode + "; Error: " + ex.Message, w);
                    w.Close();
                }
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.Flush();
        }
    }
}