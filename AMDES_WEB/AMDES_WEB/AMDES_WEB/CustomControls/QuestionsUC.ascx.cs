using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMDES_KBS.Entity;

namespace AMDES_WEB.CustomControls
{
    public partial class QuestionsUC : System.Web.UI.UserControl
    {
        private int qid;
        private int score;
        private Question question;
        private bool answer;

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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void setAnswer(bool answer)
        {
            this.answer = answer;

            if (question.isNegation && this.answer) //negation and answer = yes : 0
                score = 0;
            else if (question.isNegation && !this.answer) //negation and answer = no :+
                score = question.Score;
            else if (!question.isNegation && this.answer) //no negation and answer = yes :+
                score = question.Score;
            else if (!question.isNegation && !this.answer) //no negation and answer = no : 0
                score = 0;
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
                chkAns.Checked = value;
                setAnswer(chkAns.Checked);
            }
        }

        public bool isEnabled
        {
            get
            { return chkAns.Enabled; }
            set
            { chkAns.Enabled = value; }
        }
    }
}