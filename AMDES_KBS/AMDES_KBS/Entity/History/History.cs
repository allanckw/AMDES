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

        Dictionary<int, List<QnHistory>> history;

        public Dictionary<int, List<QnHistory>> getHistory()
        {
            return history;
        }

        public History(string patientID)
        {
            history = new Dictionary<int, List<QnHistory>>();
            this.patientID = patientID;
        }

        public string PatientID
        {
            get { return patientID; }
            set { patientID = value; }
        }


        public History()
        {
            history = new Dictionary<int, List<QnHistory>>();
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
    }





}


