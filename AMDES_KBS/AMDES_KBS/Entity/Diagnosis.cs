using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class Diagnosis
    {

        public static string dataPath = @"Data\Diagnoses.xml";

        private int rid;
        private string comment;
        private string link;
        private string header;

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
