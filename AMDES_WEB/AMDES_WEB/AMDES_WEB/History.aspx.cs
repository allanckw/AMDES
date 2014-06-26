using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

namespace AMDES_WEB
{
    public partial class History : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            bool result;
            bool.TryParse(Session["Result"].ToString(), out result);

            if (!result)
            {
                Response.Redirect("~/PatientStart.aspx");
            }
            

        }
    }
}