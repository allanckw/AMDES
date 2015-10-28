using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    //Applicable for web application only.
    public class WebApplicationContextController
    {
        public static WebApplicationContext setApplicationContext(string appName)
        {
            WebApplicationContext sApp = new WebApplicationContext();
            sApp.Name = appName;
            sApp.Description = App.readAppTitle(appName);
            sApp.IsSelected = true;
            sApp.IsConfiguredCorrectly = true;
            sApp.FolderPath = System.Web.HttpContext.Current.Server.MapPath(@"~/AppData/" + appName);
           
            return sApp;
        }

        private static void setEntitiesPath()
        {
        //    PatAttribute.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\PatAttributes.xml";
        //    QuestionGroup.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Questions.xml";
        //    History.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\History.xml";
        //    FirstQuestion.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\FirstQn.xml";
        //    Navigation.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Navex.xml";
        //    Rules.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Rules.xml";
        //    Rules.defaultRulesPath = CLIPSController.selectedAppContext.FolderPath + @"\DefRules.xml";
        //    Assessor.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Assessor.xml";
        //    Diagnosis.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Diagnoses.xml";
        //    Patient.dataPath = CLIPSController.selectedAppContext.FolderPath + @"\Patients.xml";
        //
        }
    }
}
