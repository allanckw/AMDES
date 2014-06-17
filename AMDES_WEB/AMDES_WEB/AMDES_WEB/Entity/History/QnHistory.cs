
namespace AMDES_KBS.Entity
{
    public class QnHistory
    {
        private int qid;
        private bool ans;

        public QnHistory(int qid, bool ans)
        {
            this.qid = qid;
            this.ans = ans;
        }

        public int QuestionID
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
