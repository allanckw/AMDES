using System;
using System.Collections.Generic;

namespace AMDES_KBS.Entity
{
    public class QuestionCountGroup : QuestionGroup
    {
        private int maxQuestions = 10; //max no. of question in group

        private int threshold = 8; // < threshold returns false in clips = 

        private int maxScore = -1;

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
            set { threshold = value; }
        }

        public int MaxQuestions
        {
            get { return maxQuestions; }
            set { maxQuestions = value; }
        }

        public int MaximumScore
        {
            get
            {
                if (this.maxScore == -1)
                {
                    this.maxScore = 0;
                    foreach(Question q in qns)
                    {
                        this.maxScore += q.Score;
                    }
                    return this.maxScore;
                }
                else
                {
                    return this.maxScore;
                }

            }
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
