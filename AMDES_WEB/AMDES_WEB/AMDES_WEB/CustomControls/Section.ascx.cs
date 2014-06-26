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
    public partial class Section : System.Web.UI.UserControl
    {

        private int sectionID;
        private QuestionGroup section;
        private AMDES_KBS.Entity.History hist;

        public bool Enabled
        {
            set
            {
                ViewState["isEnabled"] = value;
            }
            get
            {
                bool enabled;
                bool result = bool.TryParse(ViewState["isEnabled"].ToString(), out enabled);
                if (!result)
                {
                    ViewState["isEnabled"] = true;
                    return true;
                }
                else
                {
                    return enabled;
                }
            }
        }

        private int CurrentSection
        {
            set
            {
                Session["CurrSection"] = value;
            }
            get
            {
                return int.Parse(Session["CurrSection"].ToString());
            }
        }

        public int SectionID
        {
            get { return sectionID; }
            set
            {
                if (value > 0)
                {
                    sectionID = value;
                    section = QuestionController.getGroupByID(value);
                    loadQuestions();
                }
            }
        }

        public CLIPSController CLIPSCtrl
        {
            set
            {
                Session["clp"] = value;
            }
            get
            {
                return (CLIPSController)Session["clp"];
            }
        }

        private void loadQuestions()
        {
            Control c = GetPostBackControl(Page);

            lblHeader.Text = section.Description.Replace("~~", " <br />");
            lblSection.Text = section.Header;

            

            this.phRegister.Controls.Clear();
            int ControlID = 0;

            for (int i = 0; i < section.Questions.Count; i++)
            {
                Question q = section.Questions[i];
                QuestionsUC qnCtrl = (QuestionsUC)LoadControl(@"~/CustomControls\QuestionsUC.ascx");
                qnCtrl.QuestionNo = i + 1;
                qnCtrl.QuestionText = q.Name;
                qnCtrl.QID = q.ID;
                qnCtrl.ID = "qnCtrl" + q.ID;
                qnCtrl.isEnabled = this.Enabled;

                this.phRegister.Controls.Add(qnCtrl);
                ControlID += 1;
            }
        }

        public Control GetPostBackControl(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if ((ctrlname != null) & ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        //http://www.vbforums.com/showthread.php?649132-Preventing-an-asp-CheckBox-from-losing-it-s-checked-value
        protected void Page_Init(object sender, EventArgs e)
        {
            if (this.Enabled)
                loadEnabledControls();
            else
                if (this.sectionID == 0)
                    loadReadOnlyControls();

        }

        private void loadReadOnlyControls()
        {
            hist = (AMDES_KBS.Entity.History)Session["History"];

            if (CurrentSection == 0)
                btnPrevious.Visible = false;
            else
                btnPrevious.Visible = true;

            loadHistory();

        }

        private void loadHistory()
        {
            if (this.SectionID == 0)
                this.SectionID = this.hist.getHistory().Keys.ElementAt(CurrentSection);

            if (CLIPSCtrl.CurrentPatient.getLatestHistory() != null)
            {
                List<QnHistory> qnHistory = CLIPSCtrl.CurrentPatient.getLatestHistory().retrieveHistoryList(this.SectionID);
                if (qnHistory.Count > 0)
                {
                    foreach (QnHistory h in qnHistory)
                    {
                        Control c = phRegister.FindControl("qnCtrl" + h.QuestionID.ToString());
                        if (c is QuestionsUC)
                        {
                            QuestionsUC quc = (QuestionsUC)c;
                            quc.isYes = h.Answer;
                        }

                    }
                }
                computeScore();
            }
        }

        private void loadEnabledControls()
        {
            try
            {
                this.SectionID = CLIPSCtrl.getCurrentQnGroupID();

                if (this.SectionID == FirstQuestionController.readFirstQuestion().GrpID)
                    btnPrevious.Visible = false; //if 1st question, previous button removed

                if (CLIPSCtrl.getCurrentQnGroupID() == -1)
                {
                    this.SectionID = CLIPSCtrl.getCurrentQnGroupID();
                    CLIPSCtrl.getResultingDiagnosis();
                    Session["Result"] = true;
                    Response.Redirect("~/Results.aspx");
                }
                else
                {
                    if (CLIPSCtrl.CurrentPatient.getLatestHistory() != null)
                    {
                        List<QnHistory> qnHistory = CLIPSCtrl.CurrentPatient.getLatestHistory().retrieveHistoryList(this.sectionID);
                        if (qnHistory.Count > 0)
                        {
                            foreach (QnHistory h in qnHistory)
                            {
                                Control c = phRegister.FindControl("qnCtrl" + h.QuestionID.ToString());
                                if (c is QuestionsUC)
                                {
                                    QuestionsUC quc = (QuestionsUC)c;
                                    quc.isYes = h.Answer;
                                }

                            }
                        }
                    }
                    computeScore();

                    Session["Result"] = false;
                }
            }
            catch (Exception ex)
            {
                Alert.Show(this.SectionID.ToString() + Environment.NewLine + ex.Message);

                //Response.Redirect("~/PatientStart.aspx");
            }
        }

        private void computeScore()
        {
            if (section.getQuestionTypeENUM() == QuestionType.COUNT)
            {
                QuestionCountGroup qcg = (QuestionCountGroup)section;
                lbl1.Visible = lblMax.Visible = lblScore.Visible = true;
                lblScore.Text = "0";
                lblMax.Text = " / " + qcg.MaxQuestions.ToString();
            }
            else
            {
                lbl1.Visible = lblMax.Visible = lblScore.Visible = false;
            }

            if (lblScore.Visible)
            {
                int count = 0;
                foreach (Control c in phRegister.Controls)
                {
                    if (c is QuestionsUC)
                    {
                        QuestionsUC quc = (QuestionsUC)c;
                        if (quc.isYes)
                            count += 1;
                    }
                }
                lblScore.Text = count.ToString();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack && lblScore.Visible)
            {
                int count = 0;
                foreach (Control c in phRegister.Controls)
                {
                    if (c is QuestionsUC)
                    {
                        QuestionsUC quc = (QuestionsUC)c;
                        if (quc.isYes)
                            count += 1;
                    }
                }

                lblScore.Text = count.ToString();
            }

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                CLIPSController clp = CLIPSCtrl;

                foreach (Control c in phRegister.Controls)
                {
                    if (c is QuestionsUC)
                    {
                        QuestionsUC quc = (QuestionsUC)c;
                        if (quc.isYes)
                            clp.assertQuestion(section.GroupID, quc.QID, true);
                    }
                }

                clp.assertNextSection();
                clp.saveAssertLog();
                clp.saveCurrentNavex();

                CLIPSCtrl = clp;
                Response.Redirect("~/Questionnaire.aspx");
            }
            else
            {
                CurrentSection += 1;

                if (CurrentSection < this.hist.getHistory().Keys.Count)
                {
                    this.SectionID = this.hist.getHistory().Keys.ElementAt(CurrentSection);
                    loadReadOnlyControls();
                }
                else
                {
                    CurrentSection = 0;
                    CLIPSCtrl.getResultingDiagnosis();
                    Session["Result"] = true;
                    Response.Redirect("~/Results.aspx");
                }
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                CLIPSController clp = CLIPSCtrl;

                foreach (Control c in phRegister.Controls)
                {
                    if (c is QuestionsUC) //reset on previous
                    {
                        QuestionsUC quc = (QuestionsUC)c;
                        if (quc.isYes)
                            clp.assertQuestion(section.GroupID, quc.QID, false);
                    }
                }

                clp.assertPrevSection();
                clp.saveAssertLog();
                clp.saveCurrentNavex();

                CLIPSCtrl = clp;
                Response.Redirect("~/Questionnaire.aspx");
            }
            else
            {
                if (CurrentSection > 0)
                {
                    CurrentSection -= 1;
                    this.SectionID = this.hist.getHistory().Keys.ElementAt(CurrentSection);
                    loadReadOnlyControls();
                }
            }
        }


    }
}