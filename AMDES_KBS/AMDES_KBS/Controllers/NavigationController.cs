using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMDES_KBS.Entity;
using System.Xml.Linq;
using System.IO;

namespace AMDES_KBS.Controllers
{
    public class NavigationController
    {

      
        private static int rulesRIDCounter = 1;

        private static void createDataFile()
        {
            //create xml document from scratch
            if (!File.Exists(Rules.dataPath))
            {
                XDocument document = new XDocument(

                    new XDeclaration("1.0", "utf-8", "yes"),

                    new XComment("AMDES Rules xml"),
                        new XElement("Rules")
                );

                document.Save(Rules.dataPath);
            }
        }

        public static int getNextRuleRID()
        {   //Call this function whenever you click a new Rules !!! 
            //Linkage Determine here and set using constructor
            rulesRIDCounter++;
            return rulesRIDCounter - 1;
        }

        public static void updateRules(Rules p)
        {
            createDataFile();

            if (getRulesByID(p.RuleID) == null)
            {
                addRules(p); //if RID is not present, just add
            }
            else
            {
                deleteRules(p.RuleID); //delete and add
                addRules(p);
            }
        }

        private static void addRules(Rules p)
        {
            XDocument document = XDocument.Load(Rules.dataPath);

            //Not sure if i need to set NextGroupLink from outsRIDe so i can load the information to clips on load
            //(i.e. the information for : Y -> A, N -> B, < 7 -> A, > 7 -> B
            XElement newGRP = new XElement("Rule", new XAttribute("RID", p.RuleID),
                              new XElement("Description", p.Description),
                                //description of the Question Rules, for example in Decision Point D, need to tell user that he need to give the user a memory phrase
                                new XElement("Navigations"),
                                new XElement("Diagnoses")
                                );

            if (p.DiagnosisList.Count > 0)
            {
                for (int i = 0; i < p.diagnosis.Count; i++)
                {
                    int diagID = p.diagnosis[i];

                    XElement diag = new XElement("RDiagnosisID", diagID);

                    newGRP.Element("Diagnoses").Add(diag);
                }
            }

            if (p.Navigations.Count > 0)
            {
                for (int j = 0; j < p.Navigations.Count; j++)
                {
                    Navigation q = p.Navigations.ElementAt(j);
                    string navid = p.RuleID + "." + (j + 1).ToString();
                    XElement navex = new XElement("Navex", new XAttribute("NavID", navid),
                                           new XElement("Destination", q.DestGrpID),
                                           new XElement("NavigationChildCriterias"),
                                           new XElement("NavigationChildAttributes"),
                                           new XElement("Diagnoses")
                                           );


                    for (int n = 0; n < q.ChildCriteriaQuestion.Count; n++)
                    {
                        NaviChildCriteriaQuestion cq = q.ChildCriteriaQuestion.ElementAt(n);
                        XElement ccq = new XElement("ChildCriteriaQuestionGroup",
                                            new XElement("CriteriaGrpID", cq.CriteriaGrpID),
                                            new XElement("Answer", cq.Ans));

                        navex.Element("NavigationChildCriterias").Add(ccq);

                    }

                    for (int a = 0; a < q.ChildCriteriaAttributes.Count; a++)
                    {
                        NaviChildCritAttribute ca = q.ChildCriteriaAttributes.ElementAt(a);
                        XElement cca = new XElement("ChildCriteriaAttribute",
                                        new XElement("AttributeName", ca.AttributeName),
                                        new XElement("AttributeValue", ca.AttributeValue),
                                        new XElement("CompareType", ca.getCompareType()),
                                        //new XElement("Answer", ca.Ans),
                                        new XElement("SetOnGroupID", ca.GroupID)
                                        );

                        navex.Element("NavigationChildAttributes").Add(cca);

                    }

                    for (int t = 0; t < q.DiagnosesID.Count; t++)
                    {
                        int diagID = q.DiagnosesID.ElementAt(t);
                        XElement diag = new XElement("DiagnosisID", diagID);
                        navex.Element("Diagnoses").Add(diag);

                    }
                    newGRP.Element("Navigations").Add(navex);
                }
            }



            document.Element("Rules").Add(newGRP);
            document.Save(Rules.dataPath);
        }

        public static List<Rules> getAllRules() //call this on form onload in settings
        {
            List<Rules> rList = new List<Rules>();

            if (File.Exists(Rules.dataPath))
            {
                XDocument document = XDocument.Load(Rules.dataPath);

                var rules = (from pa in document.Descendants("Rule")
                             select pa).ToList();

                foreach (var x in rules)
                {
                    Rules rule = readRulesData(x);
                    rList.Add(rule);
                    if (rulesRIDCounter <= rule.RuleID)
                    {
                        rulesRIDCounter = rule.RuleID + 1;
                    }
                }

                return rList;
            }
            else
            {
                return rList; // return empty list
            }
        }

        private static Rules readRulesData(XElement x)
        {
            if (x != null)
            {
                Rules r = new Rules();
                r.RuleID = int.Parse(x.Attribute("RID").Value);
                r.Description = x.Element("Description").Value;

                var navex = (from pa in x.Descendants("Navigations").Descendants("Navex")
                             select pa).ToList();

                foreach (var n in navex)
                {
                    r.Navigations.Add(readNavigation(n));
                }

                var diag = (from pa in x.Descendants("Diagnoses").Descendants("RDiagnosisID")
                            select pa).ToList();


                foreach (var d in diag)
                {
                    r.addDiagnosisID(readDiagnosisID(d));
                }

                return r;

            }
            return null;
        }

        private static Navigation readNavigation(XElement x)
        {
            if (x != null)
            {
                Navigation n = new Navigation();
                n.NavID = x.Attribute("NavID").Value;
                n.DestGrpID = int.Parse(x.Element("Destination").Value);

                var cq = (from pa in x.Descendants("NavigationChildCriterias").Descendants("ChildCriteriaQuestionGroup")
                          select pa).ToList();

                foreach (var cq1 in cq)
                {
                    n.ChildCriteriaQuestion.Add(readChildCriteriaQuestion(cq1));
                }

                var ca = (from pa in x.Descendants("NavigationChildAttributes").Descendants("ChildCriteriaAttribute")
                          select pa).ToList();

                foreach (var ca1 in ca)
                {
                    n.ChildCriteriaAttributes.Add(readChildCriteriaAttributes(ca1));
                }

                var diag = (from pa in x.Descendants("Diagnoses").Descendants("DiagnosisID")
                            select pa).ToList();

                foreach (var d in diag)
                {
                    n.DiagnosesID.Add(readDiagnosisID(d));
                }

                return n;
            }

            return null;

        }

        private static NaviChildCriteriaQuestion readChildCriteriaQuestion(XElement x)
        {

            if (x != null)
            {
                NaviChildCriteriaQuestion p = new NaviChildCriteriaQuestion();

                p.CriteriaGrpID = int.Parse(x.Element("CriteriaGrpID").Value);
                p.Ans = bool.Parse(x.Element("Answer").Value);
                return p;

            }
            else
            {
                return null;
            }
        }

        private static NaviChildCritAttribute readChildCriteriaAttributes(XElement x)
        {
            if (x != null)
            {
                NaviChildCritAttribute p = new NaviChildCritAttribute();
                p.AttributeName = x.Element("AttributeName").Value;
                p.AttributeValue = x.Element("AttributeValue").Value;
                p.setRuleType(int.Parse(x.Element("CompareType").Value));
                //p.Ans = bool.Parse(x.Element("Answer").Value);
                p.GroupID = int.Parse(x.Element("SetOnGroupID").Value);
                return p;
            }
            else
            {
                return null;
            }
        }

        private static int readDiagnosisID(XElement x)
        {
            if (x != null)
            {
                return int.Parse(x.Value);
            }
            else
                return -1;
        }

        public static Rules getRulesByID(int RID)
        {
            XDocument document = XDocument.Load(Rules.dataPath);

            try
            {
                var grp = (from pa in document.Descendants("Rule")
                           where int.Parse(pa.Attribute("RID").Value) == RID
                           select pa).SingleOrDefault();

                return readRulesData(grp);

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static void deleteRules(int RID)
        {
            XDocument document = XDocument.Load(Rules.dataPath);

            if (getRulesByID(RID) != null)
            {
                (from pa in document.Descendants("Rule")
                 where int.Parse(pa.Attribute("RID").Value) == RID
                 select pa).SingleOrDefault().Remove();

                document.Save(Rules.dataPath);
            }
        }

    }
}
