using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class Question
    {
        public Question()
        {
        }

        public Question(int id, string qn)
        {
            this.id = id;
            this.name = qn;
        }

        private string name;
        private int id;
      
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
       
    }
}
