using System.Collections.Generic;
using System.Xml.Linq;
using AMDES_KBS.Entity;
using System.IO;

namespace AMDES_KBS.Controllers
{
    public class EnginePathController
    {
        public static void writeEngineFileName(EngineFile a)
        {
            //create xml document from scratch
            
            XDocument document = new XDocument(

                new XDeclaration("1.0", "utf-8", "yes"),

                new XComment("AMDES ENGINE FILE xml"),
                    new XElement("Engine",
                        new XElement("Name", a.FileName)
                        //,new XElement("Location", a.Name)
                        )
            );

            //save constructed document
            document.Save(EngineFile.dataPath);

        }

        public static EngineFile readEngineFileName()
        {
            if (File.Exists(EngineFile.dataPath))
            {
                XElement xelement = XElement.Load(EngineFile.dataPath);
                IEnumerable<XElement> assessors = xelement.Elements();

                EngineFile a = new EngineFile();

                a.FileName = xelement.Element("Name").Value.ToString();


                return a;
            }
            else
            {
                return null;
            }
        }

        public static EngineFile readEngineFileName(XElement x)
        {
            if (x != null)
            {
                EngineFile a = new EngineFile(x.Element("Name").Value);

                return a;
            }
            else
            {
                return null;
            }
        }
    }
}
