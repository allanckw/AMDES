using System;
using System.Collections.Generic;

namespace AMDES_KBS.Entity
{
    public class Diagnosis
    {

        public static string dataPath = @"Data\Diagnoses.xml";

        private int rid;
        private string comment;
        private string link;
        private string header;

        private bool retrieveSym;
        private List<int> retrievalIDList;

        public Diagnosis()
        {
            retrievalIDList = new List<int>();
        }

        public Diagnosis(int rid, string comment, string header, bool retrieveSym = false)
        {
            this.rid = rid;
            this.comment = comment;
            this.header = header;
            this.retrieveSym = retrieveSym;
            retrievalIDList = new List<int>();
        }

        public bool RetrieveSym
        {
            get { return retrieveSym; }
            set { retrieveSym = value; }
        }

        public List<int> RetrievalIDList
        {
            get { return retrievalIDList; }
            set
            {
                if (value.Count == 0)
                {
                }
                else if (!retrieveSym)
                {
                    throw new Exception("Cannot Add Retrieval List when it is not a Symptom Retrieval Rule");
                }
                else if (value.Count > 0)
                    this.retrievalIDList = value;
            }
        }

        public void addRetrievalID(int i)
        {
            if (!this.retrievalIDList.Contains(i))
            {
                this.retrievalIDList.Add(i);
            }

        }



        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        public int RID
        {
            get { return rid; }
            set { rid = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public string Link
        {
            get
            {
                if (this.link.Length > 0 && this.link.StartsWith("http://"))
                    return link;
                else if (link.Length > 0 && !this.link.StartsWith("http://"))
                    return "http://" + this.link;
                else
                    return "";
            }
            set { this.link = value; }
        }


    }
}
