﻿
namespace AMDES_KBS.Entity
{
    public class Question
    {
        public Question()
        {
        }


        public Question(string qn, string sym = "")
        {
            this.name = qn;
            this.symptom = sym;
        }


        private int id;
        private string name;

        private string symptom;
        //if the answer to this group is true, what should it assert about the patient???
        //e.g. Amensia, Apraxia, etc.. Clips need to know what to assert?
        //when i press YES, i need to send clips this thing, optional can be blank

        public string Symptom
        {
            get { return symptom; }
            set { symptom = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int ID
        {
            get { return id; }
            set
            {

                id = value;
            }
        }

    }
}
