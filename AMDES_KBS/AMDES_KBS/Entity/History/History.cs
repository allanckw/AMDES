using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public enum PatientStatus
    {
        COMPLETED,
        DRAFT
    };

    public class History
    {
        private PatientStatus status;

        public static string dataPath = @"data\History.xml";

        private string patientID;

        private List<Symptom> sympsList;

        private List<Diagnosis> diagList;

        private DateTime assessmentDate;

        private DateTime? completedDate;

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

        public DateTime? CompletedDate
        {
            get { return completedDate; }
            set { completedDate = value; }
        }

        public History()
        {
            history = new Dictionary<int, List<QnHistory>>();
            sympsList = new List<Symptom>();
            diagList = new List<Diagnosis>();
            status = PatientStatus.DRAFT;
            completedDate = null;

        }

        public History(string patientID, DateTime assDate)
        {
            history = new Dictionary<int, List<QnHistory>>();
            this.patientID = patientID;
            assessmentDate = assDate;
            sympsList = new List<Symptom>();
            diagList = new List<Diagnosis>();
            status = PatientStatus.DRAFT;
            completedDate = null;

        }

        public void createNewHistory(int groupID)
        {
            List<QnHistory> qnHistory = new List<QnHistory>();
            history.Add(groupID, qnHistory);
        }

        public List<QnHistory> retrieveHistoryList(int groupID)
        {
            List<QnHistory> h = new List<QnHistory>();
            if (history.TryGetValue(groupID, out h))
            {
                return history[groupID].OrderBy(x => x.QuestionID).ToList(); 
            }
            return null;
        }

        private void addHistoryItem(int groupID, int qid, bool ans)
        {
            List<QnHistory> h;
            if (history.TryGetValue(groupID, out h))
            {
                history[groupID].Add(new QnHistory(qid, ans));
            }
        }

        public bool removeHistoryItem(int groupID, int  qid)
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

        public void updateHistoryItem(int groupID, int qid, bool ans)
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

        public PatientStatus getStatusEnum()
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

        public void setCompleted()
        {
            this.status = PatientStatus.COMPLETED;
            this.completedDate = DateTime.Now.Date;
        }
    }





}


