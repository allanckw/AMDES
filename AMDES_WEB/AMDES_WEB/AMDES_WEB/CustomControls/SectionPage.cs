using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMDES_WEB.CustomControls
{
    public class SectionPage
    {
        private Dictionary<int, List<QuestionsUC>> questions;
        private int sectionID;
        private bool multiPage;
        private int lastPage;

        private int currentPage = 1;

        private int pageScore;
        public int PageScore
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
            this.lastPage = 0;
        }

        public void addPage(List<QuestionsUC> pageQN)
        {
            this.lastPage++;
            this.Questions.Add(this.lastPage, pageQN);

            if (lastPage > 1)
                this.isMultiPage = true;
        }

        public int getCurrentPage()
        {
            return this.currentPage;
        }

        public int getFirstPage()
        {
            return 1;
        }

        public int getNextPage()
        {
            if (this.currentPage < this.lastPage)
                return currentPage + 1;
            else
                return this.lastPage;
        }

        public int getLastPage()
        {
            return this.lastPage;
        }

        public void navigateNextPage()
        {
            if (this.currentPage < this.lastPage)
                this.currentPage++;
        }

        public void navigatePreviousPage()
        {
            if (this.currentPage > 1)
                this.currentPage--;
        }

        public bool isFirstPage
        {
            get { return this.getCurrentPage() == this.getCurrentPage(); }
        }

        public int getQuestionTotalScoreForPage(int page)
        {
            List<QuestionsUC> qnList;
            questions.TryGetValue(page, out qnList);
            int scoreToDeduct = 0;
            foreach (QuestionsUC qnc in qnList)
            {
               scoreToDeduct += qnc.Qn.Score;
            }

            return scoreToDeduct;
        }
    }
}