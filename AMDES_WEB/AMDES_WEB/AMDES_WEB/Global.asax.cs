using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Timers;
using System.IO;
using AMDES_KBS.Controllers;

namespace AMDES_WEB
{
    public class Global : System.Web.HttpApplication
    {
        private void scheduler()
        {
            Timer t = new Timer();


            t.Interval = 5;

            t.AutoReset = true;

            t.Enabled = true;

            t.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data\");
            string lastSent = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data\lastSent.scd");
            if (File.Exists(lastSent))
            {
                StreamReader sr = new StreamReader(lastSent);
                string d = sr.ReadToEnd();
                sr.Close();

                if (d.Length > 0)
                {
                    DateTime current = DateTime.Now;
                    DateTime saved = new DateTime(long.Parse(d));
                    TimeSpan ts = current - saved;
                    if (ts.Hours > 1)
                    {
                        pruneOldLogs(filepath);
                    }

                }
                else
                {
                    StreamWriter sw = new StreamWriter(lastSent, false);
                    sw.WriteLine(DateTime.Now.Ticks);
                    sw.Close();
                    pruneOldLogs(filepath);
                    
                }
            }
            else
            {
                StreamWriter sw = new StreamWriter(lastSent, false);
                sw.WriteLine(DateTime.Now.Ticks);
                sw.Close();
                pruneOldLogs(filepath);
            }
        }

        private void pruneOldLogs(string fp)
        {
            string[] apps = Directory.GetDirectories(fp);
            foreach (string app in apps)
            {
                string dir = app + @"\Logs";
                DirectoryInfo d = new DirectoryInfo(dir);

                foreach (FileInfo log in d.GetFiles("*.log"))
                {
                    string timeStamp = log.Name.Split('_')[1].Replace(".log", "");

                    TimeSpan ts = DateTime.Now - log.LastWriteTime;

                    if (ts.Hours > 1)
                    {
                        File.Delete(log.FullName);
                        PatientController.deletePatient(log.Name.Replace(".log", ""));
                    }
                }
            }
        }


        protected void Application_Start(object sender, EventArgs e)
        {
            scheduler();
            timer_Elapsed(this, null);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}