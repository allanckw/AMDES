using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class QnHistory
    {
        private string qid;
        private bool ans;

        public QnHistory(string qid, bool ans)
        {
            this.qid = qid;
            this.ans = ans;
        }

        public string QuestionID
        {
            get { return qid; }
            set { qid = value; }
        }

        public bool Answer
        {
            get { return ans; }
            set { ans = value; }
        }

    }
}
