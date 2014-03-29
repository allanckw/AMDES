using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class Rules
    {
        public static string dataPath = @"data\Rules.xml";
        public static string defaultRulesPath = @"data\DefRules.xml";

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

        private List<int> diagnosis;

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
