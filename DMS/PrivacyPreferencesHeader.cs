using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS
{
    public class PrivacyPreferencesHeader : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
        }

        public void Dispose()
        {

        }
    }

}