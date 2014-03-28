using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class FirstQuestion
    {
        public static string dataPath = @"data\FirstQn.xml";

        private int grpID;

        public int GrpID
        {
            get { return grpID; }
            set { grpID = value; }
        }
        private int nextGrpID;

        public int NextGrpID
        {
            get { return nextGrpID; }
            set { nextGrpID = value; }
        }


    }
}
