using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public enum TestStatus
    {
        PENDING_RESULT,
        NORMAL,
        ABNORMAL
    };

    public class Test
    {
        private string testName;
        private DateTime? dateOrder;
        private DateTime? reportDate;
        private TestStatus status;
        Assessor doctor;
        
        public Test(Assessor a, string testName)
        {
            this.doctor = a;
            this.dateOrder = DateTime.Now;
            this.testName = testName;
            this.reportDate = DateTime.Now;
            this.status = TestStatus.PENDING_RESULT;
        }

        public Test()
        {
            this.dateOrder = DateTime.Now;
            this.reportDate = DateTime.Now;
            this.status = TestStatus.PENDING_RESULT;
        }

        public DateTime? OrderedDate
        {
            get { return this.dateOrder; }
            set { this.dateOrder = value; }
        }

        public DateTime? ReportDate
        {
            get
            {
                if (this.reportDate == this.dateOrder)
                    return null;
                else
                    return this.reportDate;
            }
            set { this.reportDate = value; }
        }

        public TestStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public Assessor Doctor
        {
            get { return this.doctor; }
            set { this.doctor = value; }
        }

        public string TestName
        {
            get { return this.testName; }
            set { this.testName = value; }
        }

        public TestStatus  getQuestionTypeENUM()
        {
            return status;
        }

        public int getStatus()
        {
            return (int)status;
        }

        public void setStatus(int x)
        {
            if (typeof(TestStatus).IsEnumDefined(x))
            {
                status = (TestStatus)x;
            }
            else
            {
                throw new InvalidOperationException("Invalid Type!");
            }
        }
    }
}
