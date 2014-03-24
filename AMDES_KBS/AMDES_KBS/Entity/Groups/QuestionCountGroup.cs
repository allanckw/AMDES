using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class QuestionCountGroup : QuestionGroup
    {
        private int maxQuestions = 10; //max no. of question in group

        private int threshold = 7; // < threshold returns false in clips

        public QuestionCountGroup(string header, int groupID)
            : base()
        {
            this.header = header;
            this.groupID = groupID;
        }

        public QuestionCountGroup()
        {
            qns = new List<Question>();
            this.qType = QuestionType.COUNT;
        }

        public int Threshold
        {
            get { return threshold; }
            set { 
                if (this.MaxQuestions >= value )
                {
                    threshold = value;
                }else{
                    throw new InvalidOperationException("Threshold cannot exceed maximum no. of questions!!!");
                }
            }
            
        }

        public int MaxQuestions
        {
            get { return maxQuestions; }
            set { maxQuestions = value; }
        }

        public override void addQuestion(string q, string sym = "")
        {
            Question qn = new Question(q, sym);

            if (qns.Count == maxQuestions)
            {
                if (q != null)
                {
                    qns.Add(qn);
                }
            }
            else
            {
                throw new InvalidOperationException("No. of questions in group exceeded maximum number of questions");
            }
        }


    }
}
