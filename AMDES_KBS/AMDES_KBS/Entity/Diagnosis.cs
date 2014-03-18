using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class Diagnosis
    {
        private string rid;
        private string comment;

        public string RID
        {
            get { return rid; }
            set { rid = value; }
        }
       
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }


    }
}
