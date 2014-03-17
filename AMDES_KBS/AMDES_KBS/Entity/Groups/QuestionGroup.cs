using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        protected int groupID;
        protected string header;
        protected QuestionType qType;
        protected List<Question> qns;
        private string desc;
        private string symptom;

        protected QuestionGroup nextTrueLink, nextFalseLink, prevLink; //the next group that will make the decision

        public static string dataPath = @"data\Questions.xml";

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
        }

        public QuestionGroup()
        {
            qns = new List<Question>();
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

        public QuestionGroup NextTrueLink
        {
            get { return nextTrueLink; }
            set
            {
                if (this.GroupID == value.GroupID)
                {
                    throw new InvalidOperationException("Cannot point to urself!");
                }
                else
                {
                    nextTrueLink = value;
                }
            }
        }

        public QuestionGroup NextFalseLink
        {
            get { return nextFalseLink; }
            set
            {
                if (this.GroupID == value.GroupID)
                {
                    throw new InvalidOperationException("Cannot point to urself!");
                }
                else
                {
                    nextFalseLink = value;
                }
            }
        }
    }
}
