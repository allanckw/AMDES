using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    //Adaptive in the sense that the qn can change, can be customized
    /*
     * A->B->C2->C3->C4_C13->Ans <== Where do i define that i finally found the ans? From where?
     * 
     * not sure where is the final screen going to come from, missing information
        cant be i pluck from thin air the final screen, need another class call DiagnosisGroup?
    
     * <diagnosisgroup> 
     *      <diagnosisid> R1 </diagnosisid>
     *      <text> blah blah blah </text>
     *      <Symptoms>
     *          <Symptom>| Amensia </Symptom> // | = or
     *          <Symptom>^ Apraxia </Symptom> // ^ = and
     *      </Symptoms>   
     */
    class QuestionController
    {

        private static int groupIDCounter = 1;

        private static void createDataFile()
        {
            //create xml document from scratch
            if (!File.Exists(QuestionGroup.dataPath))
            {
                XDocument document = new XDocument(

                    new XDeclaration("1.0", "utf-8", "yes"),

                    new XComment("AMDES Questions xml"),
                        new XElement("QuestionGroups")
                );

                document.Save(QuestionGroup.dataPath);
            }
        }

        public static int getNextGroupID()
        {   //Call this function whenever you click a new QuestionGroup !!! 
            //Linkage Determine here and set using constructor
            groupIDCounter++;
            return groupIDCounter - 1;
        }

        public static void updateQuestionGroup(QuestionGroup p)
        {
            createDataFile();

            if (QuestionController.getGroupByID(p.GroupID) == null)
            {
                QuestionController.addQuestionGroup(p); //if id is not present, just add
            }
            else
            {
                QuestionController.deleteQuestionGroup(p.GroupID); //delete and add
                QuestionController.addQuestionGroup(p);
            }
        }

        private static void addQuestionGroup(QuestionGroup p)
        {
            XDocument document = XDocument.Load(QuestionGroup.dataPath);

            //Not sure if i need to set NextGroupLink from outside so i can load the information to clips on load
            //(i.e. the information for : Y -> A, N -> B, < 7 -> A, > 7 -> B
            XElement newGRP = new XElement("QuestionGroup", new XAttribute("id", p.GroupID),
                                new XElement("Header", p.Header), //what it should show on the tab header 
                                new XElement("Description", p.Description),
                //description of the Question QuestionGroup, for example in Decision Point D, need to tell user that he need to give the user a memory phrase
                                new XElement("QnType", p.getQuestionType()), //type, AND / OR / COUNT
                                new XElement("Symptom", p.Symptom),
                                new XElement("Negation", p.isNegation),
                //What does it assert about the patient if this is true? (i need this value in patient));
                                new XElement("Questions") //questions to ask

                                );


            if (p.Questions.Count > 0)
            {
                for (int j = 0; j < p.Questions.Count; j++)
                {
                    Question q = p.Questions.ElementAt(j);
                    newGRP.Element("Questions").Add(
                                        new XElement("Question", new XAttribute("QID", (j + 1)),
                                        new XElement("Name", q.Name),
                                        new XElement("Symptom", q.Symptom),
                                        new XElement("Negation", q.isNegation),
                                        new XElement("ImageURL", q.ImagePath),
                                        new XElement("Score", q.Score)
                                        )
                                    );
                    //What does it assert about the patient if this is true? (i need this value in patient));
                }
            }

            if (p.getQuestionTypeENUM() == QuestionType.COUNT)
            {   //for Count QuestionGroup, Max no. of questions (Default 10) Threshold.
                QuestionCountGroup qc = (QuestionCountGroup)p;
                newGRP.Add(new XElement("MaxQuestions", qc.MaxQuestions),
                           new XElement("Threshold", qc.Threshold) //Determine T / F based on score (<= Threshold) => false
                          );
            }

            document.Element("QuestionGroups").Add(newGRP);
            document.Save(QuestionGroup.dataPath);
        }

        public static List<QuestionGroup> getAllQuestionGroup() //call this on form onload in settings
        {
            List<QuestionGroup> pList = new List<QuestionGroup>();

            if (File.Exists(QuestionGroup.dataPath))
            {
                XDocument document = XDocument.Load(QuestionGroup.dataPath);

                var qgrps = (from pa in document.Descendants("QuestionGroup")
                             select pa).ToList();

                foreach (var x in qgrps)
                {
                    QuestionGroup grp = readQnGrpData(x);
                    pList.Add(grp);
                    if (groupIDCounter <= grp.GroupID)
                    {
                        groupIDCounter = grp.GroupID + 1;
                    }
                }

                return pList.OrderBy(x => x.Header).ToList<QuestionGroup>();
            }
            else
            {
                return pList; // return empty list
            }
        }

        private static QuestionGroup readQnGrpData(XElement x)
        {

            if (x != null)
            {
                QuestionGroup p = new QuestionGroup();

                p.setQuestionType(int.Parse(x.Element("QnType").Value));

                if (p.getQuestionTypeENUM() == QuestionType.COUNT)
                {
                    return readQnCountGroupData(x);
                }
                else
                {
                    p.GroupID = int.Parse(x.Attribute("id").Value);
                    p.Header = x.Element("Header").Value;
                    p.Description = x.Element("Description").Value;
                    p.Symptom = x.Element("Symptom").Value;
                    
                    if (x.Element("Negation") != null)
                        p.isNegation = bool.Parse(x.Element("Negation").Value);
                    else
                        p.isNegation = false;
                    

                    var qns = (from pa in x.Descendants("Questions").Descendants("Question")
                               select pa).ToList();

                    foreach (var q in qns)
                    {
                        p.Questions.Add(readQuestion(q));
                    }

                    return p;
                }

            }
            else
            {
                return null;
            }
        }

        private static QuestionCountGroup readQnCountGroupData(XElement x)
        {
            if (x != null)
            {
                QuestionCountGroup p = new QuestionCountGroup();

                p.setQuestionType(int.Parse(x.Element("QnType").Value));
                p.GroupID = int.Parse(x.Attribute("id").Value);
                p.Header = x.Element("Header").Value;
                p.Description = x.Element("Description").Value;
                p.Symptom = x.Element("Symptom").Value;

                if (x.Element("Negation") != null)
                    p.isNegation = bool.Parse(x.Element("Negation").Value);
                else
                    p.isNegation = false;
                               
                var qns = (from pa in x.Descendants("Questions").Descendants("Question")
                           select pa).ToList();

                foreach (var q in qns)
                {
                    p.Questions.Add(readQuestion(q));
                }

                p.MaxQuestions = int.Parse(x.Element("MaxQuestions").Value);
                p.Threshold = int.Parse(x.Element("Threshold").Value);

                return p;
            }
            else
            {
                return null;
            }
        }

        private static Question readQuestion(XElement x)
        {
            if (x != null)
            {
                Question q = new Question();
                q.ID = int.Parse(x.Attribute("QID").Value);
                q.Name = x.Element("Name").Value;
                q.Symptom = x.Element("Symptom").Value;

                if (x.Element("Score") != null)
                    q.Score = int.Parse(x.Element("Score").Value);
                else
                    q.Score = 1;

                if (x.Element("ImageURL") != null)
                    q.ImagePath = x.Element("ImageURL").Value;
                else
                    q.ImagePath = "";

                if (x.Element("Negation") != null)
                    q.isNegation = bool.Parse(x.Element("Negation").Value);
                else
                    q.isNegation = false;
                
                return q;
            }
            else
            {
                return null;
            }
        }

        public static QuestionGroup getGroupByID(int id)
        {
            XDocument document = XDocument.Load(QuestionGroup.dataPath);

            try
            {
                var grp = (from pa in document.Descendants("QuestionGroup")
                           where int.Parse(pa.Attribute("id").Value) == id
                           select pa).SingleOrDefault();

                return readQnGrpData(grp);

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static void deleteQuestionGroup(int id)
        {
            XDocument document = XDocument.Load(QuestionGroup.dataPath);

            if (QuestionController.getGroupByID(id) != null)
            {
                (from pa in document.Descendants("QuestionGroup")
                 where int.Parse(pa.Attribute("id").Value) == id
                 select pa).SingleOrDefault().Remove();

                document.Save(QuestionGroup.dataPath);
            }
        }
    }
}
