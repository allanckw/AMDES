using System;
using System.Collections.Generic;
using AMDES_KBS.Controllers;

namespace AMDES_KBS.Entity
{
    public enum QuestionType
    {
        OR,
        AND,
        COUNT
    };

    public class QuestionGroup
    {
        public static string dataPath = @"\Questions.xml";

        protected int groupID;
        protected string header;
        protected QuestionType qType;
        protected List<Question> qns;
        private string desc;
        private string symptom;
        protected Navigation nextTrueLink, nextFalseLink; //the next group that will make the decision

        
        //20150930 - Add Negative Scoring
        private bool negativeScoring;
        //20150930 - Add Negative Scoring
        public bool isNegation
        {
            get { return negativeScoring; }
            set { negativeScoring = value; }
        }
        
        public string Symptom
        {
            get { return symptom; }
            set { symptom = value; }
        }
        //if the answer to this group is true, what should it assert about the patient???
        //e.g. Amensia, Apraxia, etc.. Clips need to know what to assert?
        //when the group eval to true, i need to send clips this thing, optional can be blank

        public QuestionGroup(string header, QuestionType t, int groupID)
            : base()
        {
            this.header = header;
            this.groupID = groupID;
            if (t == QuestionType.COUNT)//must not attempt to set count here
            {
                throw new InvalidOperationException("Invalid Question Type");
            }

            //this.isNegation = false;
        }

        public QuestionGroup()
        {
            qns = new List<Question>();
            //this.isNegation = false;
        }

        public List<Question> Questions { get { return qns; } }

        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        public QuestionType getQuestionTypeENUM()
        {
            return qType;
        }

        public int getQuestionType()
        {
            return (int)qType;
        }

        public void setQuestionType(int x)
        {
            if (typeof(QuestionType).IsEnumDefined(x))
            {
                qType = (QuestionType)x;
            }
            else
            {
                throw new InvalidOperationException("Invalid Type!");
            }
        }

        public string Header
        {
            get { return header; }
            set { this.header = value; }
        }

        public virtual void addQuestion(string q, string sym = "")
        {
            Question qn = new Question( q, sym);

            if (q != null)
            {
                qns.Add(qn);
            }
        }

        public void removeQuestionAt(int i)
        {
            if (i < qns.Count)
            {
                qns.RemoveAt(i);

            }
        }

        public void clearQuestions()
        {
            qns.Clear();
        }

        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }

        public bool Equals(QuestionGroup qg)
        {
            return this.GroupID == qg.GroupID;
        }

    }
}
