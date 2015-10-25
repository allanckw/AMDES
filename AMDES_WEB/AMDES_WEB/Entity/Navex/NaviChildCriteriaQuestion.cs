
using System;
namespace AMDES_KBS.Entity
{
    public class NaviChildCriteriaQuestion : IEquatable<NaviChildCriteriaQuestion>, IComparable<NaviChildCriteriaQuestion>
    {
        private string navid;

        private int criteriaGrpID;

        private bool ans;

        public string Navid
        {
            get { return navid; }
            set { navid = value; }
        }

        public int CriteriaGrpID
        {
            get { return criteriaGrpID; }
            set { criteriaGrpID = value; }
        }

        public bool Ans
        {
            get { return ans; }
            set { ans = value; }
        }

        public NaviChildCriteriaQuestion(string navID, int grpID, bool ans)
        {
            this.navid = navID;
            this.criteriaGrpID = grpID;
            this.ans = ans;
        }

        public NaviChildCriteriaQuestion()
        {
        }



        public bool Equals(NaviChildCriteriaQuestion ncq)
        {
            return (this.CriteriaGrpID == ncq.criteriaGrpID &&
                    this.Ans == ncq.Ans);
        }

        public int CompareTo(NaviChildCriteriaQuestion ncq)
        {
            if (this.Equals(ncq))
                return 0;
            else
                return -1;
        }
    }
}
