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
        private string linkDesc;

        private List<CmpAttribute> attributes;

        public string LinkDesc
        {
            get { return linkDesc; }
            set { linkDesc = value; }
        }

        private string header;

        private bool retrieveSym;

        //res
        private bool isResource;

        public bool IsResource
        {
            get { return isResource; }
            set { isResource = value; }
        }

        private List<int> retrievalIDList;

        public Diagnosis()
        {
            retrievalIDList = new List<int>();
            attributes = new List<CmpAttribute>();
        }

        public Diagnosis(int rid, string comment, string header, bool retrieveSym = false)
        {
            this.rid = rid;
            this.comment = comment;
            this.header = header;
            this.retrieveSym = retrieveSym;
            retrievalIDList = new List<int>();

            attributes = new List<CmpAttribute>();
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

        public bool hasResourceLink()
        {
            return this.link.Length > 0;
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
                if (this.link != null)
                {
                    if (this.link.Length > 0 && this.link.StartsWith("http://"))
                        return link;
                    else if (link.Length > 0 && !this.link.StartsWith("http://"))
                        return "http://" + this.link;
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
            set { this.link = value; }
        }

        public void addAttribute(CmpAttribute value)
        {
            attributes.Add(value);
        }

        public CmpAttribute retrieveAttributeAt(int i)
        {
            if (i > 0 && i < attributes.Count)
                return attributes[i];
            else
                return null;
        }

        public void removeAttributeAt(int i)
        {
            if (i > 0 && i < attributes.Count)
                attributes.RemoveAt(i);
            
        }


        public List<CmpAttribute> getAttributes()
        {
            return this.attributes;
        }
    }
}
