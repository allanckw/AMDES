using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class History
    {
        public static string dataPath = @"data\History.xml";

        private string patientID;

        private List<Symptom> sympsList;

        private List<Diagnosis> diagList;

        private DateTime assessmentDate;

        Dictionary<int, List<QnHistory>> history;
        
        public string PatientID
        {
            get { return patientID; }
            set { patientID = value; }
        }

        public Dictionary<int, List<QnHistory>> getHistory()
        {
            return history;
        }
        public DateTime AssessmentDate
        {
            get { return assessmentDate; }
            set { assessmentDate = value; }
        }

        public History()
        {
            history = new Dictionary<int, List<QnHistory>>();
        }

        public History(string patientID, DateTime assDate)
        {
            history = new Dictionary<int, List<QnHistory>>();
            this.patientID = patientID;
            assessmentDate = assDate;
        }

        public void createNewHistory(int groupID)
        {
            List<QnHistory> qnHistory = new List<QnHistory>();
            history.Add(groupID, qnHistory);
        }

        private void addHistoryItem(int groupID, string qid, bool ans)
        {
            List<QnHistory> h;
            if (history.TryGetValue(groupID, out h))
            {
                history[groupID].Add(new QnHistory(qid, ans));
            }
        }

        public bool removeHistoryItem(int groupID, string qid)
        {
            List<QnHistory> h;
            int id = -1;

            if (history.TryGetValue(groupID, out h))
            {
                for (int i = 0; i < h.Count(); i++)
                {
                    QnHistory qh = h[i];
                    if (h[i].QuestionID == qid)
                    {
                        id = i;
                        break;
                    }
                }
                if (id != -1)
                {
                    history[groupID].RemoveAt(id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void updateHistoryItem(int groupID, string qid, bool ans)
        {
            removeHistoryItem(groupID, qid);
            addHistoryItem(groupID, qid, ans);
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
    }





}


