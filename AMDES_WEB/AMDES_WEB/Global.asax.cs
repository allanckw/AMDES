using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.SessionState;
using System.Timers;
using System.IO;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;

namespace AMDES_WEB
{
    public class Global : System.Web.HttpApplication
    {
        private void scheduler()
        {
            Timer t = new Timer();
            t.Interval = 30 * 60 * 1000; //min * s * ms
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
                    if (ts.Minutes > 60)
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

                //foreach (FileInfo log in d.GetFiles("*.log"))
                //{
                //    string timeStamp = log.Name.Split('_')[1].Replace(".log", "");

                //    TimeSpan ts = DateTime.Now - log.LastWriteTime;

                //    if (ts.Minutes > 30)
                //    {
                //        File.Delete(log.FullName);
                //        PatientController.deletePatient(log.Name.Replace(".log", ""));
                //    }
                //}

                List<AMDES_KBS.Entity.History> patList = HistoryController.getAllHistory();
                foreach (AMDES_KBS.Entity.History p in patList)
                {
                    string createdTime = p.PatientID.Split('_')[1];
                    DateTime cd = new DateTime(long.Parse(createdTime));
                    TimeSpan ts = DateTime.Now - cd;
                    if (ts.Minutes > 60)
                    {
                        HistoryController.deletePatientNavigationHistory(p.PatientID);
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