using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Data
{
    public enum TestStatus
    {
        NORMAL,
        ABNORMAL,
        PENDING_RESULT
    };
    
    class Test
    {
        private string testName;
        private DateTime dateOrder;
        private DateTime? reportDate;

        private TestStatus status;

        Assessor doctor;


        public readonly DateTime OrderedDate
        {
            get { return this.dateOrder; }
        }

        public DateTime? ReportDate
        {
            get { 
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
    }
}
