using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMDES_KBS.Controllers;

//http://stackoverflow.com/questions/8417225/parse-xml-using-linq-to-xml-to-class-objects

namespace AMDES_KBS.Entity
{
    public enum PatientStatus
    {
        COMPLETED,
        DRAFT
    };

    public enum PatientEthnicGrp
    {
        CHINESE,
        MALAY,
        INDIAN,
        EURASIAN,
        OTHERS
    }

    public enum PatientGender
    {
        MALE,
        FEMALE
    }

    public class Patient : IComparable<Patient>
    {
        public static string dataPath = @"Data\Patients.xml";

        private Assessor doctor;

        private DateTime dAssessment, dob;

        private List<Test> testsList;

        private List<Symptom> sympsList; //immediate sym list of result

        private List<Diagnosis> diagList; //immediate diaglist of result

        private PatientStatus status;

        private string nric = "", firstname = "", lastname = "";

        private PatientGender gender = PatientGender.MALE;

        private PatientEthnicGrp ethnicGrp = PatientEthnicGrp.CHINESE;

        public PatientEthnicGrp EthnicGroup
        {
            get { return ethnicGrp; }
            set { ethnicGrp = value; }
        }


        public PatientGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }


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

        public Patient(Assessor doc, DateTime dob)
        {
            this.doctor = doc;
            this.DOB = dob;
            testsList = new List<Test>();
            sympsList = new List<Symptom>();
            diagList = new List<Diagnosis>();
             dAssessment = DateTime.Today;
            status = PatientStatus.DRAFT;
        }

        public Patient()
        {
            testsList = new List<Test>();
            sympsList = new List<Symptom>();
            diagList = new List<Diagnosis>();
            dAssessment = DateTime.Today;
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
            diagList = new List<Diagnosis>();
            dAssessment = DateTime.Today;
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

        public DateTime AssessmentDate
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

        public List<Symptom> SymptomsList
        {
            get { return this.sympsList; }
        }

        public void addSymptom(Symptom s)
        {
            var z = sympsList.Where(x => (x.SymptomName.ToUpper().CompareTo(s.SymptomName.ToUpper()) == 0)).SingleOrDefault();

            if (s != null && z == null)
            {
                this.sympsList.Add(s);
            }

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



        public List<Diagnosis> Diagnoses
        {
            get { return diagList; }
        }

        public void addDiagnosis(Diagnosis d)
        {
            var z = diagList.Where(x => (x.RID == d.RID)).SingleOrDefault();

            if (d != null && z == null)
            {
                this.diagList.Add(d);
            }

        }

        public Diagnosis getDiagnosisAt(int i)
        {
            if (sympsList.Count > 0 && i >= 0)
                return this.diagList.ElementAt(i);
            else
                return null;
        }

        public void removeDiagnosisAt(int i)
        {
            if (sympsList.Count > 0 && i >= 0)
                this.diagList.RemoveAt(i);
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

        public PatientStatus getQuestionTypeENUM()
        {
            return status;
        }

        public int getStatus()
        {
            return (int)status;
        }

        public void setStatus(int x)
        {
            if (typeof(PatientStatus).IsEnumDefined(x))
            {
                status = (PatientStatus)x;
            }
            else
            {
                throw new InvalidOperationException("Invalid Type!");
            }
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
    }
}
