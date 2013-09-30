using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Alfresco;

namespace DMS
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticationUtils authUtil = new AuthenticationUtils();
            authUtil.endSession();
            Session.Clear();
            Response.Redirect("Default.aspx",true);
        }
    }
}