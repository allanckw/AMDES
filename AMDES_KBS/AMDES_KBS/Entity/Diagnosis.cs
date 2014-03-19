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

            get { return this.link; }
            set {this.link = value;}
        }


    }
}
