//using System;
//using System.Xml.Linq;
//using AMDES_KBS.Entity;

//namespace AMDES_KBS.Controllers
//{
//    class TestController
//    {
//        public static XElement writeTest(Test t, Assessor p)
//        {
//            XElement x = new XElement("Test",
//                            new XElement("TestName", t.TestName),
//                            new XElement("Status", t.getStatus()),
//                            new XElement("OrderedDate", t.OrderedDate.GetValueOrDefault().Ticks),
//                            new XElement("ReportDate", t.ReportDate.GetValueOrDefault().Ticks),
//                                    new XElement("Assessor",
//                                        new XElement("AssessorName", p.Name),
//                                        new XElement("AssessLocation", p.ClinicName)
//                                    )
//                            );

//            return x;
//        }

//        public static Test readPatientTest(XElement x)
//        {
//            if (x != null)
//            {
//                Test t = new Test();
//                t.TestName = x.Element("TestName").Value;
//                t.Doctor = AssessorController.readAssessor(x.Element("Assessor"));
//                t.OrderedDate = new DateTime(long.Parse(x.Element("OrderedDate").Value));
//                t.ReportDate = new DateTime(long.Parse(x.Element("ReportDate").Value));
//                t.setStatus(int.Parse(x.Element("Status").Value));

//                return t;
//            }
//            return null;
//        }


//    }
//}
