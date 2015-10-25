using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMDES_KBS.Entity;
using AMDES_KBS.Controllers;
using System.Threading;
using System;

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
                if (Session["Result"] != null)
                {
                    bool hasResults = bool.Parse(Session["Result"].ToString());
                    if (hasResults)
                        ViewState["isEnabled"] = false;
                    else
                        ViewState["isEnabled"] = true;
                }
                else
                    ViewState["isEnabled"] = true;
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

        public Dictionary<int, SectionPage> dicSectionPage
        {
            set
            {
                if (value == null)
                {
                    Dictionary<int, SectionPage> dic = new Dictionary<int, SectionPage>();
                    Session["dicSectionPage"] = dic;
                }
                else
                {
                    Session["dicSectionPage"] = value;
                }
            }

            get
            {
                Dictionary<int, SectionPage> dic = (Dictionary<int, SectionPage>)Session["dicSectionPage"];

                if (dic == null)
                    dic = new Dictionary<int, SectionPage>();

                Session["dicSectionPage"] = dic;
                return dic;
            }
        }

        private int CurrentSectionIndex
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
                    section = QuestionController.getGroupByID(value, CLIPSCtrl.ApplicationContext);
                    loadQuestions();
                }
            }
        }

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

        //http://www.vbforums.com/showthread.php?649132-Preventing-an-asp-CheckBox-from-losing-it-s-checked-value
        protected void Page_Init(object sender, EventArgs e)
        {
            if (this.Enabled)
                loadEnabledControls();

            if (!this.Enabled && this.sectionID == 0)
                loadReadOnlyControls();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            displayScore();

        }

        private void loadQuestions()
        {
            Control c = GetPostBackControl(Page);

            lblHeader.Text = section.Description.Replace("~~", " <br />");
            lblSection.Text = section.Header;

            this.phRegister.Controls.Clear();
            int ControlID = 0;

            SectionPage sPage;
            bool exist = dicSectionPage.TryGetValue(sectionID, out sPage);

            if (!exist)
            {
                sPage = new SectionPage(section.GroupID);

                List<QuestionsUC> pageList = new List<QuestionsUC>();

                for (int i = 0; i < section.Questions.Count; i++)
                {
                    Question q = section.Questions[i];
                    QuestionsUC qnCtrl = (QuestionsUC)LoadControl(@"~/CustomControls\QuestionsUC.ascx");
                    qnCtrl.Qn = q;

                    qnCtrl.QuestionNo = i + 1;

                    qnCtrl.ID = "qnCtrl" + q.ID;
                    qnCtrl.isEnabled = this.Enabled;
                    qnCtrl.SectionID = section.GroupID;

                    this.phRegister.Controls.Add(qnCtrl);
                    ControlID += 1;

                    if (i == 0 && q.hasImage)
                    {
                        pageList = new List<QuestionsUC>();
                        pageList.Add(qnCtrl);
                    }
                    else if (i > 0 && q.hasImage) //if image on this qn, form a new page
                    {
                        sPage.addPage(pageList);
                        pageList = new List<QuestionsUC>();
                        pageList.Add(qnCtrl);
                    }
                    else if (i > 0 && section.Questions[i - 1].hasImage) //if image on prev qn form a new page
                    {
                        sPage.addPage(pageList);
                        pageList = new List<QuestionsUC>();
                        pageList.Add(qnCtrl);
                    }
                    else
                        pageList.Add(qnCtrl);

                    if (i == section.Questions.Count - 1)
                        sPage.addPage(pageList);
                }
            }

            refreshSectionPage(section.GroupID, sPage);

            loadQuestionControls(section.GroupID);

        }

        private void refreshSectionPage(int grpID, SectionPage sPage)
        {
            if (dicSectionPage.Keys.Contains(grpID))
                dicSectionPage.Remove(grpID);

            dicSectionPage.Add(grpID, sPage);
        }

        private void loadQuestionControls(int sectionID)
        {
            this.phRegister.Controls.Clear();

            SectionPage sPage;
            dicSectionPage.TryGetValue(sectionID, out sPage);

            List<QuestionsUC> pageList;
            sPage.Questions.TryGetValue(sPage.CurrentPage, out pageList);

            foreach (QuestionsUC qnCtrl in pageList)
            {
                this.phRegister.Controls.Add(RefreshPanel(qnCtrl));
            }
        }

        private QuestionsUC RefreshPanel(QuestionsUC oldCtrl)
        {
            QuestionsUC newCtrl = (QuestionsUC)LoadControl(@"~/CustomControls\QuestionsUC.ascx");
            newCtrl.Qn = oldCtrl.Qn;

            //newCtrl.isYes = oldCtrl.isYes;
            newCtrl.isEnabled = oldCtrl.isEnabled;

            newCtrl.QuestionNo = oldCtrl.QuestionNo;
            newCtrl.ID = oldCtrl.ID;
            newCtrl.SectionID = oldCtrl.SectionID;

            return newCtrl;
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

        private void loadReadOnlyControls()
        {

            hist = (AMDES_KBS.Entity.History)Session["History"];

            if (this.SectionID == 0)
                this.SectionID = this.hist.getHistory().Keys.ElementAt(CurrentSectionIndex);

            SectionPage sPage;
            dicSectionPage.TryGetValue(sectionID, out sPage);

            //bug here
            if (CurrentSectionIndex == 0 && sPage.isFirstPage)
            {
                btnPrevious.Visible = false;
            }
            else
            {
                btnPrevious.Visible = true;
            }
            loadHistory();

        }

        private void loadHistory()
        {
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
                            if (quc.Qn.isNegation)
                                quc.isYes = !h.Answer;
                            else
                                quc.isYes = h.Answer;
                        }

                    }
                }
                displayScore();
            }
        }

        private void loadEnabledControls()
        {
            try
            {
                this.SectionID = CLIPSCtrl.getCurrentQnGroupID();

                SectionPage sPage;
                dicSectionPage.TryGetValue(sectionID, out sPage);

                if (this.SectionID == FirstQuestionController.readFirstQuestion(CLIPSCtrl.ApplicationContext).GrpID
                    && sPage.isFirstPage)
                {
                    btnPrevious.Visible = false; //if 1st section, 1st page previous button removed
                }

                if (CLIPSCtrl.getCurrentQnGroupID() == -1)
                {
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
                                    if (quc.Qn.isNegation)
                                        quc.isYes = !h.Answer;
                                    else
                                        quc.isYes = h.Answer;
                                }

                            }
                        }
                    }
                    displayScore();

                    Session["Result"] = false;
                }
            }
            catch (Exception ex)
            {
                Alert.Show(this.SectionID.ToString() + Environment.NewLine + ex.Message);

                //Response.Redirect("~/PatientStart.aspx");
            }
        }

        private int computeSectionScore()
        {
            SectionPage sPage;
            dicSectionPage.TryGetValue(sectionID, out sPage);

            int count = sPage.getSectionScore() + computePageScore();

            sPage.setPageScore(sPage.CurrentPage, 0);
            return count;
        }


        private int computePageScore()
        {
            SectionPage sPage;
            dicSectionPage.TryGetValue(sectionID, out sPage);

            int count = 0;

            foreach (Control c in phRegister.Controls)
            {
                if (c is QuestionsUC)
                {
                    QuestionsUC quc = (QuestionsUC)c;
                    bool ans = quc.isYes;
                    count += quc.Score;
                }
            }


            return count;
        }

        private void displayScore()
        {
            if (section.getQuestionTypeENUM() == QuestionType.COUNT)
            {
                QuestionCountGroup qcg = (QuestionCountGroup)section;
                lbl1.Visible = lblMax.Visible = lblScore.Visible = true;

                lblScore.Text = "0";
                lblMax.Text = " / " + qcg.MaximumScore.ToString();
                int count = computeSectionScore();
                lblScore.Text = count.ToString();
            }
            else
            {
                lbl1.Visible = lblMax.Visible = lblScore.Visible = false;
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            SectionPage sPage;
            dicSectionPage.TryGetValue(sectionID, out sPage);

            if (this.Enabled)
            {
                CLIPSWebController clp = CLIPSCtrl;

                foreach (Control c in phRegister.Controls)
                {
                    if (c is QuestionsUC)
                    {
                        QuestionsUC quc = (QuestionsUC)c;
                        clp.assertQuestion(section.GroupID, quc.QID, quc.isYes, quc.Qn.isNegation);
                    }
                }

                sPage.setPageScore(sPage.CurrentPage, computePageScore());

                if (!sPage.isMultiPage)
                {   //if it is not a multipage simply assert next section
                    clp.assertNextSection();

                }
                else if (sPage.isMultiPage && sPage.CurrentPage == sPage.LastPage)
                {   //Assert next section if current page = last page if it is a multipage
                    clp.assertNextSection();
                }
                else
                {
                    sPage.navigateNextPage();
                }

                clp.saveCurrentNavex();
                CLIPSCtrl = clp;

                refreshSectionPage(section.GroupID, sPage);
                Response.Redirect("~/Questionnaire.aspx");
            }
            else
            {
                CurrentSectionIndex += 1;

                if (CurrentSectionIndex < this.hist.getHistory().Keys.Count)
                {
                    this.SectionID = this.hist.getHistory().Keys.ElementAt(CurrentSectionIndex);
                    loadReadOnlyControls();
                }
                else
                {
                    CurrentSectionIndex = 0;

                    if (sPage.isMultiPage && sPage.CurrentPage != sPage.LastPage)
                        sPage.navigate(1);
                    else
                    {
                        CLIPSCtrl.getResultingDiagnosis();
                        Session["Result"] = true;
                        Response.Redirect("~/Results.aspx");
                    }
                }
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            SectionPage sPage;
            dicSectionPage.TryGetValue(sectionID, out sPage);

            if (this.Enabled)
            {
                CLIPSWebController clp = CLIPSCtrl;

                foreach (Control c in phRegister.Controls)
                {
                    if (c is QuestionsUC) //reset on previous
                    {
                        QuestionsUC quc = (QuestionsUC)c;
                        clp.assertQuestion(section.GroupID, quc.QID, quc.DefaultAnswer, quc.Qn.isNegation);
                    }
                }


                sPage.setPageScore(sPage.CurrentPage, 0);

                if (!sPage.isMultiPage)
                {   //if it is not a multipage simply assert prev section
                    clp.assertPrevSection();
                }
                else if (sPage.isMultiPage && sPage.isFirstPage)
                {   //Assert prev section if current page = 1st page if it is a multipage
                    //TODO: Test This Scenario, in both Desktop and web app
                    clp.assertPrevSection();
                }
                else
                {
                    sPage.navigatePreviousPage();
                }

                clp.saveCurrentNavex();

                CLIPSCtrl = clp;

                Response.Redirect("~/Questionnaire.aspx");
            }
            else
            {
                if (CurrentSectionIndex > 0)
                {
                    CurrentSectionIndex -= 1;
                }
                if (sPage.isMultiPage)
                    sPage.navigatePreviousPage();

                this.SectionID = this.hist.getHistory().Keys.ElementAt(CurrentSectionIndex);
                loadReadOnlyControls();
            }
        }

    }
}

