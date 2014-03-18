using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//http://stackoverflow.com/questions/8417225/parse-xml-using-linq-to-xml-to-class-objects

namespace AMDES_KBS.Entity
{
    public enum PatientStatus
    {
        COMPLETED,
        DRAFT
    };

    public class Patient : IComparable<Patient>
    {
        public static string dataPath = @"Data\Patients.xml";

        private Assessor doctor;

        private DateTime dAssessment, dob;

        private List<Test> testsList;

        private List<Symptom> sympsList; 
        //TODO: Store chain of QuestionGroupID to determine next form of action?
        

        private PatientStatus status;

        private string nric, firstname, lastname;

        public DateTime DOB
        {
            get { return dob; }
            set { dob = value; }
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

        public Patient()
        {
            testsList = new List<Test>();
            sympsList = new List<Symptom>();
            dAssessment = DateTime.Now;
            status = PatientStatus.DRAFT;
        }

        public Patient(Assessor doc, string nric, string first, string last, DateTime dob)
            : base()
        {
            this.doctor = doc;
            this.nric = nric;
            this.firstname = first;
            this.lastname = last;
            this.dob = dob;

            testsList = new List<Test>();
            sympsList = new List<Symptom>();
            dAssessment = DateTime.Now;
            status = PatientStatus.DRAFT;
        }

        public Assessor Doctor
        {
            get { return this.doctor; }
            set { this.doctor = value; }
        }

        public PatientStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public  DateTime AssessmentDate
        {
            get { return dAssessment; }
            set { this.dAssessment = value; }
        }

        public List<Test> TestsList
        {
            get { return this.testsList; }
        }

        public void addTest(Test t)
        {
            if (t != null)
                testsList.Add(t);
        }

        public Test getTestAt(int i)
        {
            if (testsList.Count > 0 && i >= 0)
                return this.testsList.ElementAt(i);
            else
                return null;
        }

        public void removeTestAt(int i)
        {
            if (testsList.Count > 0 && i >= 0)
                this.testsList.RemoveAt(i);
        }

        public  List<Symptom> SymptomsList
        {
            get { return this.sympsList; }
        }

        public void addSymptom(Symptom s)
        {
            if (s != null)
                this.sympsList.Add(s);
        }

        public Symptom getSymptomAt(int i)
        {
            if (sympsList.Count > 0 && i >= 0)
                return this.sympsList.ElementAt(i);
            else
                return null;
        }

        public void removeSymptomAt(int i)
        {
            if (sympsList.Count > 0 && i >= 0)
                this.sympsList.RemoveAt(i);
        }

        public int getAge()
        {
            int age = DateTime.Today.Year - DOB.Year;

            if (DOB > DateTime.Today.AddYears(-age)) 
                age--;

            return age;
        }

        public void setCompleted()
        {
            this.status = PatientStatus.COMPLETED;
        }

        public int CompareTo(Patient p)
        {
            return (p.nric.CompareTo(this.nric));
        }
    }
}
