using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    public class ApplicationContextController
    {
        public static void GetAllApplications()
        {
            CLIPSController.AllAppContexts = new List<ApplicationContext>();

            string[] dirs = Directory.GetDirectories(Path.GetFullPath(@"Data\"));

            foreach (string s in dirs)
            {
                ApplicationContext app = new ApplicationContext();
                app.FolderPath = s;


                if (File.Exists(s + @"\NAME.CONF") && File.Exists(s + @"\WOT.CONF"))
                {
                    string[] files = Directory.GetFiles(s, "*.conf");
                    foreach (string f in files)
                    {
                        if (f.ToUpper().Contains("NAME.CONF"))
                            app.Name = readAppInfo(f);

                        if (f.ToUpper().Contains("WOT.CONF"))
                            app.Description = readAppInfo(f);
                    }
                    app.IsConfiguredCorrectly = true;
                }
                else
                {
                    app.Name = s.Substring(s.LastIndexOf(@"\"), s.Length - s.LastIndexOf(@"\")) + " NOT CONFIGURED ";
                    app.IsConfiguredCorrectly = false;
                }

                CLIPSController.AllAppContexts.Add(getApplicationContext(app));
               
            }
            CLIPSController.AllAppContexts = CLIPSController.AllAppContexts.OrderByDescending(a => a.IsSelected).ToList<ApplicationContext>();
        }

        public static ApplicationContext getApplicationContext(ApplicationContext app)
        {
            app.IsSelected = File.Exists(app.FolderPath + @"\X.CONF");

            if (app.IsSelected)
                CLIPSController.selectedAppContext = app;

            setEntitiesPath();

            return app;
        }

        public static void setApplicationContext(ApplicationContext sApp)
        {

            if (sApp.IsConfiguredCorrectly)
            {
                CLIPSController.selectedAppContext = null;
                sApp.IsSelected = true;
                foreach (ApplicationContext app in CLIPSController.AllAppContexts)
                {
                    if (File.Exists(app.FolderPath + @"\X.CONF"))
                        File.Delete(app.FolderPath + @"\X.CONF");

                    sApp.IsSelected = false;
                }

                FileStream fs = File.Create(sApp.FolderPath + @"\X.CONF");
                fs.Close();

                CLIPSController.selectedAppContext = sApp;
                setEntitiesPath();
                
            }
            else
            {
                throw new InvalidOperationException("You cannot set an application which is incorrectly configured as the application context!!");
            }
        }

        public static string readAppInfo(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);

            }
            else
            {
                return "";
            }

        }

        public static string getSelectedAppContext()
        {
            return "";
        }

        private static void setEntitiesPath()
        {
            PatAttribute.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\PatAttributes.xml";
            QuestionGroup.dataPath  = CLIPSController.selectedAppContext.FolderPath + @"\Questions.xml";
            History.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\History.xml";
            FirstQuestion.dataPath  = CLIPSController.selectedAppContext.FolderPath + @"\FirstQn.xml";
            Navigation.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Navex.xml";
            Rules.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Rules.xml";
            Rules.defaultRulesPath = CLIPSController.selectedAppContext.FolderPath + @"\DefRules.xml";
            Assessor.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Assessor.xml";
            Diagnosis.dataPath =  CLIPSController.selectedAppContext.FolderPath + @"\Diagnoses.xml";
            Patient.dataPath  = CLIPSController.selectedAppContext.FolderPath + @"\Patients.xml";

        }
    }
}
