
namespace AMDES_KBS.Entity
{
    public class Question
    {
        public Question()
        {
        }

        public Question(string qn, string sym = "", int score = 1, bool negation = false)
        {
            this.name = qn;
            this.symptom = sym;
            this.score = score;
            this.isNegation = negation;
        }

        //20150930 - Add Negative Scoring
        private bool negativeScoring;
        //20150930 - Add Negative Scoring
        public bool isNegation
        {
            get { return negativeScoring; }
            set { negativeScoring = value; }
        }

        private int id;
        private string name;
        private string symptom;
        private int score;

        //20150930 - Add Image Path
        private string imgPath;

        public string ImagePath
        {
            get { return imgPath; }
            set { imgPath = value; }
        }


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
