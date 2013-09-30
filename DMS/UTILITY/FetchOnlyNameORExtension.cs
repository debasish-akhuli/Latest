using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS.UTILITY
{
    public class FetchOnlyNameORExtension
    {
        public string FetchOnlyDocName(string DocName)
        {
            try
            {
                int TotLength = DocName.Length;
                int LastDotPos = DocName.LastIndexOf('.') + 1;
                string WithoutExtDocName = DocName.Substring(0, LastDotPos - 1);
                return WithoutExtDocName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string FetchOnlyDocExt(string DocName)
        {
            try
            {
                int TotLength = DocName.Length;
                int LastDotPos = DocName.LastIndexOf('.') + 1;
                string WithoutExtDocName = DocName.Substring(0, LastDotPos - 1);
                int WithoutExtDocLength = WithoutExtDocName.Length;
                int ExtLength = TotLength - WithoutExtDocLength;
                string Extension = DocName.Substring(LastDotPos, ExtLength - 1);
                return Extension;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}