using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMDES_WEB.CustomControls
{
    public class SectionPage
    {
        private Dictionary<int, List<QuestionsUC>> questions; //key = page, value = uc
        private int sectionID;
        private bool multiPage;
        private int lastPage;

        private Dictionary<int, int> pageScore; //key = page, value = score

        private int currentPage = 1;


        public Dictionary<int, int> PageScore
        {
            get { return pageScore; }
            set { pageScore = value; }
        }

        public bool isMultiPage
        {
            get { return multiPage; }
            set { multiPage = value; }
        }

        public Dictionary<int, List<QuestionsUC>> Questions
        {
            get { return questions; }
        }

        public int SectionID
        {
            get { return sectionID; }
            set { sectionID = value; }
        }

        public SectionPage(int sectionID)
        {
            this.sectionID = sectionID;
            this.questions = new Dictionary<int, List<QuestionsUC>>();
            this.pageScore = new Dictionary<int, int>();
            this.lastPage = 0;
        }

        public void addPage(List<QuestionsUC> pageQN)
        {
            this.lastPage++;
            this.Questions.Add(this.lastPage, pageQN);

            if (lastPage > 1)
                this.isMultiPage = true;

            setPageScore(this.lastPage, 0);
        }

        public int CurrentPage
        {
            get { return this.currentPage; }
        }

        public int FirstPage
        {
            get { return 1; }
        }

        public int NextPage
        {
            get
            {
                if (this.currentPage < this.lastPage)
                    return currentPage + 1;
                else
                    return this.lastPage;
            }
        }

        public int LastPage
        {
            get { return this.lastPage; }
        }

        public void navigateNextPage()
        {
            if (this.currentPage < this.lastPage)
                this.currentPage++;
        }

        public void navigate(int page)
        {
            if (page < this.lastPage && this.currentPage > 0)
                this.currentPage = page;
        }

        public void navigatePreviousPage()
        {
            if (this.currentPage > 1)
                this.currentPage--;
        }

        public bool isFirstPage
        {
            get { return this.CurrentPage == this.FirstPage; }
        }

        public void setPageScore(int page, int score)
        {
            if (pageScore.Keys.Contains(page))
                pageScore.Remove(page);

            pageScore.Add(page, score);
        }

        public int getPageScore(int page)
        {
            int score;
            pageScore.TryGetValue(page, out score);

            return score;
        }

        public int getSectionScore()
        {
            int total = 0;
            for (int i = 1; i <= lastPage; i++)
                total += getPageScore(i);

            return total;
        }
    }
}