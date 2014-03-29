using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using AMDES_KBS.Entity;

namespace AMDES_KBS.Controllers
{
    public class FirstQuestionController
    {
        public static void writeFirstQuestion(FirstQuestion f)
        {
            //create xml document from scratch


            XElement fq = new XElement("FirstQuestion");
            XElement initGrp = new XElement("InitialGrpID", f.GrpID);
            //XElement nextGrp = new XElement("DestinationGrpID", f.NextGrpID);

            fq.Add(initGrp);
            //fq.Add(nextGrp);
            fq.Save(FirstQuestion.dataPath);
        }

        public static FirstQuestion readFirstQuestion()
        {
            if (File.Exists(FirstQuestion.dataPath))
            {
                XDocument document = XDocument.Load(FirstQuestion.dataPath);
                FirstQuestion fq = new FirstQuestion();

                fq.GrpID = int.Parse(document.Descendants("InitialGrpID").ElementAt(0).Value);
                //fq.NextGrpID = int.Parse(document.Descendants("DestinationGrpID").ElementAt(0).Value);
                return fq;
            }
            else
            {
                return null; //when null, prompt user to add first question
            }

        }

    }
}
