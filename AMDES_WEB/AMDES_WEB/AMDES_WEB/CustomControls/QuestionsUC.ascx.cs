using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMDES_WEB.CustomControls
{
    public partial class QuestionsUC : System.Web.UI.UserControl
    {

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

        private int qid;
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
            get { return chkAns.Checked; }
            set { chkAns.Checked = value; }
        }

        public bool isEnabled
        {
            get
            {
                return chkAns.Enabled;
            }
            set
            {
                chkAns.Enabled = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void chkAns_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}