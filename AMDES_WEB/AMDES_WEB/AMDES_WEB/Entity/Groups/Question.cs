
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

        public Question(string qn, string sym = "", int score = 1)
        {
            this.name = qn;
            this.symptom = sym;
            this.score = score;
        }


        private int id;
        private string name;
        private string symptom;
        private int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

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
