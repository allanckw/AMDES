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
    public partial class QuestionsUC : System.Web.UI.UserControl
    {
        private int qid;
        private int score;
        private Question question;
        private bool answer;
        private int sectionID;

        public int SectionID
        {
            get { return sectionID; }
            set { sectionID = value; }
        }

        public Question Qn
        {
            get { return question; }
            set
            {
                this.question = value;
                this.QuestionText = value.Name;
                this.qid = value.ID;

                this.score = 0;

                if (value.isNegation)
                    this.isYes = true;
                else
                    this.isYes = false;

            }
        }

        public bool DefaultAnswer
        {
            get 
            {
                if (this.question.isNegation)
                    return true;
                else
                    return false;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            loadImage();
        }

        private void loadImage()
        {
            if (Qn.ImagePath.Length > 0)
            {
                var uri = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");
                imgQn.ImageUrl = uri + "/AppData/" + CLIPSCtrl.ApplicationContext.FolderName + "/img/" + Qn.ImagePath;
                imgQn.Visible = true;
            }
            else
            {
                imgQn.Visible = false;
            }
        }

        private void setAnswer(bool answer)
        {
            this.answer = answer;


            if (question.isNegation && this.answer) //negation and answer = yes : 0
            {
                score = 0;
                chkAns.Checked = true;
            }
            else if (question.isNegation && !this.answer) //negation and answer = no :+
            {
                score = question.Score;
                chkAns.Checked = false;
            }
            else if (!question.isNegation && this.answer) //no negation and answer = yes :+
            {
                score = question.Score;
                chkAns.Checked = true;
            }
            else if (!question.isNegation && !this.answer) //no negation and answer = no : 0
            {
                score = 0;
                chkAns.Checked = false;
            }
        }

        public int QuestionNo
        {
            get { return int.Parse(lblQnID.Text.Trim()); }
            set
            {
                lblQnID.Text = value.ToString();
            }
        }

        public string QuestionText
        {
            get { return lblQn.Text.Trim(); }
            set
            {
                lblQn.Text = value.Replace("~~", "<br />");//.Replace("   ", "&emsp;");
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }

        public int QID
        {
            get
            {
                return qid;
            }
            set
            {
                qid = value;
            }
        }


        public bool isYes
        {
            get
            {
                setAnswer(chkAns.Checked);
                return chkAns.Checked;
            }
            set
            {
                setAnswer(value);
            }
        }

        public bool isEnabled
        {
            get { return chkAns.Enabled; }

            set { chkAns.Enabled = value; }
        }

        protected void chkAns_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}