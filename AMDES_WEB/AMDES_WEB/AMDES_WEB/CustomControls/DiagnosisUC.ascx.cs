using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;

namespace AMDES_WEB.CustomControls
{
    public partial class DiagnosisUC : System.Web.UI.UserControl
    {

        public CLIPSWebController CLIPSCtrl
        {
            set
            {
                Session["clp"] = value;
            }
            get
            {
                return (CLIPSWebController)Session["clp"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Diagnosis Recommendation
        {
            set
            {
                lblHeader.Text = value.Header.Replace("\n", "<br>");
                lblComment.Text = value.Comment.Replace("\n", "<br>").Replace("~~", "<br>") ;

                var uri = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");

                if (value.hasResourceLink())
                {
                    hypLink.Visible = true;
                    hypLink.NavigateUrl = value.Link.Replace("http://~", uri);
                    
                    hypLink.Text = value.LinkDesc;
                }

                addSymptoms(value);

            }
        }


        private void addSymptoms(Diagnosis diag)
        {
            if (diag.RetrieveSym)
            {
                foreach (int groupId in diag.RetrievalIDList)
                {
                    List<String> SymptomsListOfQuestionGroup = getSymptomsFromQuestionGroup(groupId);
                    foreach (String symptoms in SymptomsListOfQuestionGroup)
                    {
                        Label lblSymptons = new Label();

                        lblSymptons.Text = "<br>" + App.bulletForm() + symptoms;
                    }
                }
            }

        }

        private List<String> getSymptomsFromQuestionGroup(int groupID)
        {
            List<String> SymptomsList = new List<String>();

            QuestionGroup qg = QuestionController.getGroupByID(groupID, CLIPSCtrl.ApplicationContext);
            if (qg.Symptom.Trim() != "")
            {
                SymptomsList.Add(qg.Symptom.Trim());
            }

            foreach (Question q in qg.Questions)
            {
                String symptom = q.Symptom.Trim();
                if (symptom.Length > 0)
                {
                    if (!SymptomsList.Contains(symptom))
                    {
                        SymptomsList.Add(symptom);
                    }
                }
            }

            return SymptomsList;
        }


    }
}