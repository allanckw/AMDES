using System;
using System.Collections.Generic;
using System.Linq;

namespace AMDES_KBS.Entity
{
    public class Navigation : IComparable<Navigation>, IEquatable<Navigation>
    {
        public static string dataPath = System.Web.HttpContext.Current.Server.MapPath(@"Data\Add\Navex.xml");

        private string navID = "-1";
        private int destGrpID = -1;
        private List<int> diagnosis;
        private List<NaviChildCritAttribute> childCriteriaAttr;
        private List<NaviChildCriteriaQuestion> childCriteriaQn;

        public Navigation()
        {
            childCriteriaAttr = new List<NaviChildCritAttribute>();
            childCriteriaQn = new List<NaviChildCriteriaQuestion>();
            DiagnosesID = new List<int>();
        }


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

        public List<NaviChildCritAttribute> ChildCriteriaAttributes
        {
            get { return childCriteriaAttr; }
        }

        public List<NaviChildCriteriaQuestion> ChildCriteriaQuestion
        {
            get { return childCriteriaQn; }
            set
            {
                if (value != null)
                    childCriteriaQn = value;
            }
        }

        public void addNavCriteriaQuestion(int grpID, bool ans)
        {
            childCriteriaQn.Add(new NaviChildCriteriaQuestion(this.NavID, grpID, ans));
        }

        public void addNavCriteriaAttribute(string attrName, string attrVal, bool attrAns, AttributeCmpType type)
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
                    throw new InvalidOperationException("Circular Reference detected, please do not add the same section!");
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
            if (this.Equals(n))
                return 0;
            else
                return -1;


        }

        public bool Equals(Navigation n)
        {
            bool dupe = true;

            //if they dun have the same no. of attribute they are not the same
            if (this.ChildCriteriaAttributes.Count != n.ChildCriteriaAttributes.Count)
            {
                return false;
            }
            else if (this.ChildCriteriaQuestion.Count != n.ChildCriteriaQuestion.Count)
            {
                return false; //if they dun have the same criteria questions, they are not the same
            }
            else if (this.childCriteriaAttr.Count == n.ChildCriteriaAttributes.Count &&
                     this.ChildCriteriaQuestion.Count == n.ChildCriteriaQuestion.Count)
            {

                for (int i = 0; i < this.ChildCriteriaQuestion.Count; i++)
                {
                    if (!this.ChildCriteriaQuestion.Contains(n.ChildCriteriaQuestion[i]))
                    {
                        dupe = false;
                        break;
                    }
                }

                if (!dupe)
                {
                    for (int i = 0; i < ChildCriteriaAttributes.Count; i++)
                    {
                        if (!this.childCriteriaAttr.Contains(n.ChildCriteriaAttributes[i]))
                        {
                            dupe = false;
                            break;
                        }
                    }
                }
                //dupe = true = same
                if (dupe && this.DestGrpID != n.DestGrpID)
                {
                    dupe = false;
                }
                else if (dupe && this.DestGrpID == n.DestGrpID)
                {
                    if (this.DestGrpID == -1) //check diagnoses
                    {
                        List<int> myDiag = this.DiagnosesID;
                        List<int> oDiag = n.DiagnosesID;

                        if (myDiag.Count != oDiag.Count)
                        {
                            dupe = false;
                        }
                        else
                        {
                            for (int i = 0; i < oDiag.Count; i++)
                            {
                                if (!myDiag.Contains(oDiag[i]))
                                {
                                    dupe = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return dupe;
        }

        public class CriteriaSortingComparer : IComparer<Navigation>
        {

            public int Compare(Navigation n1, Navigation n2)
            {
                if (n1 != null && n2 != null)
                {
                    return (n1.ChildCriteriaQuestion.Count + n1.ChildCriteriaAttributes.Count)
                                 - (n2.ChildCriteriaQuestion.Count + n2.ChildCriteriaAttributes.Count);
                }
                else
                {
                    if (n1 == null)
                    {
                        return n2.ChildCriteriaQuestion.Count + n2.ChildCriteriaAttributes.Count;
                    }
                    else
                    {
                        return n1.ChildCriteriaQuestion.Count + n1.ChildCriteriaAttributes.Count;
                    }
                }
            }

        }
    }

}
