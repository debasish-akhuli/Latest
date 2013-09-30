using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace DMS.BAL
{
    public class validation_bal
    {
        /// <summary>
        /// This Method is used to check the selected task is already assigned for the current stage or not
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="wfid"></param>
        /// <param name="stepno"></param>
        /// <param name="taskid"></param>
        /// <param name="actiontype"></param>
        /// <returns></returns>
        public bool isDupTask(DataTable dt, Int64 wfid, Int32 stepno, string taskid,string actiontype)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt64(dt.Rows[i]["wf_id"]) == wfid && Convert.ToInt32(dt.Rows[i]["step_no"]) == stepno && dt.Rows[i]["task_id"].ToString() == taskid && dt.Rows[i]["acttype_id"].ToString() == actiontype)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isAlreadyThere(DataTable dt, Int64 wfid, Int32 stepno, string taskid)
        {
            if (taskid == "APPROVE")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt64(dt.Rows[i]["wf_id"]) == wfid && Convert.ToInt32(dt.Rows[i]["step_no"]) == stepno && dt.Rows[i]["task_id"].ToString() == "REVIEW")
                    {
                        return true;
                    }
                }
            }
            else if (taskid == "REVIEW")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt64(dt.Rows[i]["wf_id"]) == wfid && Convert.ToInt32(dt.Rows[i]["step_no"]) == stepno && dt.Rows[i]["task_id"].ToString() == "APPROVE")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsName(string name)
        {
            string MatchNamePattern = @"^[a-zA-Z''-'\s]{1,40}$";
            if (name != null) return Regex.IsMatch(name, MatchNamePattern);
            else return false;
        }

        public bool IsEmail(string email)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

    }
}