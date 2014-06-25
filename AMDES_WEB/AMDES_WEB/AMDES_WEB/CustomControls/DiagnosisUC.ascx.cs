﻿using System;
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
        public Diagnosis Recommendation
        {
            set
            {
                lblHeader.Text = value.Header;
                lblComment.Text = value.Comment.Replace("~~", "<br>");

                if (value.hasResourceLink() == true)
                {
                    hypLink.Visible = true;
                    hypLink.NavigateUrl = value.Link;
                    hypLink.Text = value.LinkDesc;
                }

                addSymptons(value);

            }
        }


        private void addSymptons(Diagnosis diag)
        {
            if (diag.RetrieveSym)
            {
                foreach (int groupId in diag.RetrievalIDList)
                {
                    List<String> SymptomsListOfQuestionGroup = getSymptomsFromQuestionGroup(groupId);
                    foreach (String symptoms in SymptomsListOfQuestionGroup)
                    {
                        Label lblSymptons = new Label();
                        
                        lblSymptons.Text = "<br> &emsp;&emsp;" + App.bulletForm() + symptoms;
                    }
                }
            }

        }

        private List<String> getSymptomsFromQuestionGroup(int groupID)
        {
            List<String> SymptomsList = new List<String>();

            QuestionGroup qg = QuestionController.getGroupByID(groupID);
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}