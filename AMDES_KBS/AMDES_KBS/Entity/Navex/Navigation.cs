using System;
using System.Collections.Generic;
using System.Linq;

namespace AMDES_KBS.Entity
{
    public class Navigation : IComparable<Navigation>
    {
        public static string dataPath = @"data\Navex.xml";

        private string navID = "-1";
        private int destGrpID = -1;
        private List<int> diagnosis;

        public string NavID
        {
            get { return navID; }
            set { navID = value; }
        }

        public int DestGrpID
        {
            get { return destGrpID; }
            set
            {
                if (diagnosis.Count > 0)
                {
                    throw new InvalidOperationException("Cannot set destination group id as it is already conclusive");
                }
                else
                {
                    destGrpID = value;
                }
            }
        }

        public List<int> DiagnosesID
        {
            get { return diagnosis; }
            set { diagnosis = value; }
        }

        private List<NaviChildCritAttribute> childCriteriaAttr;

        public List<NaviChildCritAttribute> ChildCriteriaAttributes
        {
            get { return childCriteriaAttr; }
        }

        private List<NaviChildCriteriaQuestion> childCriteriaQn;

        public List<NaviChildCriteriaQuestion> ChildCriteriaQuestion
        {
            get { return childCriteriaQn; }
            set
            {
                if (value != null)
                    childCriteriaQn = value;
            }
        }

        public Navigation()
        {
            childCriteriaAttr = new List<NaviChildCritAttribute>();
            childCriteriaQn = new List<NaviChildCriteriaQuestion>();
            DiagnosesID = new List<int>();
        }

        public void addNavCriteriaQuestion(int grpID, bool ans)
        {
            childCriteriaQn.Add(new NaviChildCriteriaQuestion(this.NavID, grpID, ans));
        }

        public void addNavCriteriaAttribute(string attrName, string attrVal, bool attrAns, NaviChildCritAttribute.AttributeCmpType type)
        {
            childCriteriaAttr.Add(new NaviChildCritAttribute(this.NavID, attrName, attrVal, attrAns, type));
        }

        public void addNavCriteriaAttribute(NaviChildCritAttribute a)
        {
            if (a != null)
                childCriteriaAttr.Add(a);
        }

        public void addNavCriteriaQuestion(NaviChildCriteriaQuestion q)
        {
            if (q != null)
            {
                var x = childCriteriaQn.Where(s => s.CriteriaGrpID == q.CriteriaGrpID).SingleOrDefault();

                if (x == null)
                    childCriteriaQn.Add(q);
                else
                    throw new InvalidOperationException("Circular Reference detected, please do not add the same Question Group!");
            }
        }

        public void removeCriteriaQuestion(int grpID)
        {
            for (int i = 0; i < childCriteriaQn.Count; i++)
            {
                NaviChildCriteriaQuestion q = childCriteriaQn[i];
                if (q.CriteriaGrpID == grpID)
                {
                    childCriteriaQn.Remove(q);
                    break;
                }
            }
        }

        public void removeCriteriaAttribute(string attrName)
        {
            for (int i = 0; i < childCriteriaAttr.Count; i++)
            {
                NaviChildCritAttribute a = childCriteriaAttr[i];
                if (a.AttributeName.ToUpper().CompareTo(attrName.ToUpper()) == 0)
                {
                    childCriteriaAttr.Remove(a);
                    break;
                }
            }
        }

        public void clearCriteriaQuestions()
        {
            childCriteriaQn.Clear();
        }

        public void clearCriteriaAttributes()
        {
            childCriteriaAttr.Clear();
        }

        public void addDiagnosisID(int diagID)
        {
            if (destGrpID == -1)
                diagnosis.Add(diagID);
            else
                throw new InvalidOperationException("Cannot enter diagnosis id when a destination ID is set");
        }

        public void removeDiagnosisID(int diagID)
        {
            for (int i = 0; i < diagnosis.Count; i++)
            {
                if (diagnosis[i] == diagID)
                {
                    diagnosis.Remove(diagID);
                    break;
                }
            }
        }

        public bool isConclusive()
        {
            return this.diagnosis.Count > 0;
        }

        public int CompareTo(Navigation n)
        {
            return (this.ChildCriteriaQuestion.Count + this.ChildCriteriaAttributes.Count)
                    - (n.ChildCriteriaQuestion.Count + n.ChildCriteriaAttributes.Count);
        }
    }

}
