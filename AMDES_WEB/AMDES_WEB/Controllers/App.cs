using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace AMDES_KBS.Controllers
{
    public class App
    {
        public static string readAppTitle(string app)
        {
            string fileName = System.Web.HttpContext.Current.Server.MapPath(@"~/Data/" + app + @"\name.CONF");
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            else
            {
                return "";
            }

        }

        public static string readNoSymptomString(string app)
        {

            string fileName = System.Web.HttpContext.Current.Server.MapPath(@"~/Data/" + app + @"\NoSymptom.CONF");
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            else
            {
                return "";
            }
        }
        public static string bulletForm()
        {
            return "\u2713";
        }

        public static string slash()
        {
            return "\u002F";
        }
    }
}