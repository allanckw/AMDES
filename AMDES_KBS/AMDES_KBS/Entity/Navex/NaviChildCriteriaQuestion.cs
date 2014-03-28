using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class NaviChildCriteriaQuestion
    {
        private string navid;

        public string Navid
        {
            get { return navid; }
            set { navid = value; }
        }
        private int criteriaGrpID;

        public int CriteriaGrpID
        {
            get { return criteriaGrpID; }
            set { criteriaGrpID = value; }
        }
        private bool ans;

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
    }
}
