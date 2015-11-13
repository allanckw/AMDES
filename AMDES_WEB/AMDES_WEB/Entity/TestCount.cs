using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AMDES_KBS.Entity
{
    public class TestCount
    {
        public static string dataPath = @"\Counter.xml";

        private string name;
        private int count;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }
}