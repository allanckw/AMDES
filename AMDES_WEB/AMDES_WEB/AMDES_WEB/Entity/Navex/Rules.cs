using System.Collections.Generic;
using AMDES_KBS.Controllers;

namespace AMDES_KBS.Entity
{
    public class Rules
    {
        public static string dataPath = System.Web.HttpContext.Current.Server.MapPath(@"Data\Add\Rules.xml");

        public static string defaultRulesPath =System.Web.HttpContext.Current.Server.MapPath(@"Data\Add\DefRules.xml");

        private List<Navigation> navList;

        public List<Navigation> Navigations
        {
            get { return navList; }
        }

        private string description = "N.A";

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int ruleID;

        public int RuleID
        {
            get { return ruleID; }
            set { ruleID = value; }
        }

        public List<int> diagnosis;

        public List<Diagnosis> DiagnosisList
        {
            get {
                List<Diagnosis> diaList = new List<Diagnosis>();
                foreach (int diaID in diagnosis)
                {
                    diaList.Add(DiagnosisController.getDiagnosisByID(diaID));
                }
                return diaList; 
            }
        }

        public Rules(int ruleID)
        {
            this.ruleID = ruleID;
        }

        public Rules()
        {
            navList = new List<Navigation>();
            diagnosis = new List<int>();
        }

        public void addDiagnosisID(int diagID)
        {
            if (diagID != -1)
                diagnosis.Add(diagID);
        }

        public void removeDiagnosisID(int diagID)
        {
            int index = -1;
            for (int i = 0; i < diagnosis.Count; i++)
            {
                if (diagnosis[i] == diagID)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                diagnosis.RemoveAt(index);
            }
        }

        public void insertNavigation(Navigation nav)
        {
            if (nav != null)
            {
                navList.Add(nav);
            }
        }

        public void removeNavigation(Navigation nav)
        {
            int index = -1;
            for (int i = 0; i < navList.Count; i++)
            {
                if (navList[i].NavID == nav.NavID)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                navList.RemoveAt(index);
            }

        }

        public void removeNavigationAt(int i)
        {
            if (i >= 0 && i < navList.Count)
            {
                navList.RemoveAt(i);
            }
        }
    }
}
