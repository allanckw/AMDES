using System;
using System.Collections.Generic;
using AMDES_KBS.Controllers;

//http://stackoverflow.com/questions/8417225/parse-xml-using-linq-to-xml-to-class-objects

namespace AMDES_KBS.Entity
{
    public class Patient : IComparable<Patient>
    {
        //public static string dataPath = @"Data\Patients.xml";
        public static string dataPath = @"\Patients.xml";
            //System.Web.HttpContext.Current.Server.MapPath(@"Data\Add\Patients.xml");

        private Assessor doctor;

        private DateTime dAssessment, dob;

        Dictionary<string, double> attributes;

        private string nric = "", firstname = "", lastname = "";

        public Patient(Assessor doc, DateTime dob)
        {
            this.doctor = doc;
            if (dob <= DateTime.Now.Date)
            {
                this.dob = dob;
            }
            else
            {
                throw new InvalidOperationException("Age cannot be from the future!!");
            }
            dAssessment = DateTime.Today;
            attributes = new Dictionary<string, double>();
            //status = PatientStatus.DRAFT;
        }

        public Patient()
        {
            dAssessment = DateTime.Today;
            attributes = new Dictionary<string, double>();
            //status = PatientStatus.DRAFT;
        }

        public Patient(Assessor doc, string nric, string first, string last, DateTime dob)
            : base()
        {
            this.doctor = doc;
            this.nric = nric;
            this.firstname = first;
            this.lastname = last;
            if (dob <= DateTime.Now.Date)
            {
                this.dob = dob;
            }
            else
            {
                throw new InvalidOperationException("Age cannot be from the future!!");
            }

            dAssessment = DateTime.Today;
            attributes = new Dictionary<string, double>();
            //status = PatientStatus.DRAFT;
        }

        public DateTime DOB
        {
            get { return dob; }
            set
            {
                if (dob <= DateTime.Now.Date)
                {
                    this.dob = value;
                }
                else
                {
                    throw new InvalidOperationException("Age cannot be from the future!!");
                }
            }
        }

        public string Last_Name
        {
            get { return lastname; }
            set { lastname = value; }
        }

        public string First_Name
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public string NRIC
        {
            get { return nric.ToUpper(); }
            set { nric = value.ToUpper(); }
        }

        public Assessor Doctor
        {
            get { return this.doctor; }
            set { this.doctor = value; }
        }
      
        public DateTime AssessmentDate
        {
            get { return dAssessment; }
            set { this.dAssessment = value; }
        }

        public int getAge()
        {
            int age = DateTime.Today.Year - DOB.Year;

            if (DOB > DateTime.Today.AddYears(-age))
                age--;

            return age;
        }

        public int CompareTo(Patient p)
        {
            return (p.nric.CompareTo(this.nric));
        }

        public History getLatestHistory()
        {
            List<History> hList = HistoryController.getHistoryByID(this.nric);
            if (hList.Count > 0)
                return hList[hList.Count - 1];
            else
                return null;
        }

        public List<History> getAllPatientHistory()
        {
            return HistoryController.getHistoryByID(this.nric);
        }

        public void createAttribute(string attrName, double value)
        {
            if (attributes.ContainsKey(attrName))
            {
                attributes[attrName] = value;
            }
            else
            {
                attributes.Add(attrName, value);
            }
        }

        public double retrieveAttribute(string attrName)
        {
           double h;
            if (attributes.TryGetValue(attrName, out h))
            {
                return attributes[attrName];
            }
            return -1;
        }

        public Dictionary<string, double> getAttributes()
        {
            return this.attributes;
        }

    }
}
