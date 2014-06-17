using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    public class DefaultBehaviorController
    {
        private static int rulesRIDCounter = 1;


        private static void createDataFile()
        {
            //create xml document from scratch
            if (!File.Exists(Rules.defaultRulesPath))
            {
                XDocument document = new XDocument(

                    new XDeclaration("1.0", "utf-8", "yes"),

                    new XComment("AMDES Rules xml"),
                        new XElement("Navigations")
                );

                document.Save(Rules.defaultRulesPath);
            }
        }

        private static int getNextRuleRID()
        { 
            rulesRIDCounter++;
            return rulesRIDCounter - 1;
        }

        public static List<Navigation> getAllDefaultBehavior() //call this on form onload in settings
        {
            List<Navigation> rList = new List<Navigation>();

            if (File.Exists(Rules.defaultRulesPath))
            {
                XDocument document = XDocument.Load(Rules.defaultRulesPath);

                var rules = (from pa in document.Descendants("Navex")
                             select pa).ToList();

                foreach (var x in rules)
                {
                    Navigation rule = readNavigation(x);
                    rList.Add(rule);
                    if (rulesRIDCounter <= int.Parse(rule.NavID))
                    {
                        rulesRIDCounter = int.Parse(rule.NavID) + 1;
                    }
                }

                return rList;
            }
            else
            {
                return rList; // return empty list
            }
        }

        private static void addNavigation(Navigation q)
        {
            XDocument document = XDocument.Load(Rules.defaultRulesPath);

            //Not sure if i need to set NextGroupLink from outsRIDe so i can load the information to clips on load
            //(i.e. the information for : Y -> A, N -> B, < 7 -> A, > 7 -> B
           
            string navid = getNextRuleRID().ToString();
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
                                new XElement("CompareType", ca.getCompareType())//,
                    //new XElement("Answer", ca.Ans)
                                );

                navex.Element("NavigationChildAttributes").Add(cca);
            }

            for (int t = 0; t < q.DiagnosesID.Count; t++)
            {
                int diagID = q.DiagnosesID.ElementAt(t);
                XElement diag = new XElement("DiagnosisID", diagID);
                navex.Element("Diagnoses").Add(diag);
            }
            document.Element("Navigations").Add(navex);
            document.Save(Rules.defaultRulesPath);
        }

        public static void updateRules(Navigation n)
        {
            createDataFile();
            
            if (getDefaultBehaviorByID(int.Parse(n.NavID)) == null)
            {
                addNavigation(n); //if RID is not present, just add
            }
            else
            {
                deleteDefaultBehavior(int.Parse(n.NavID)); //delete and add
                addNavigation(n);
            }

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

        public static Navigation getDefaultBehaviorByID(int NavID)
        {
            XDocument document = XDocument.Load(Rules.defaultRulesPath);

            try
            {
                var grp = (from pa in document.Descendants("Navex")
                           where int.Parse(pa.Attribute("NavID").Value) == NavID
                           select pa).SingleOrDefault();

                return readNavigation(grp);

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static void deleteDefaultBehavior(int NavID)
        {
            XDocument document = XDocument.Load(Rules.defaultRulesPath);

            if (getDefaultBehaviorByID(NavID) != null)
            {
                (from pa in document.Descendants("Navex")
                 where int.Parse(pa.Attribute("NavID").Value) == NavID
                 select pa).SingleOrDefault().Remove();

                document.Save(Rules.defaultRulesPath);
            }
        }
    }
}
