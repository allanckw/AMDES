using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS
{
    class TestCollection : System.Collections.ObjectModel.Collection<Test>
    {
        Random r = new Random();
        public TestCollection()
        {
            for (int i = 0; i < 30; i++)
            {
                int Y = r.Next(0, 100);
                int N = r.Next(0, 100);
                Add(new Test { TestName = "Test hahahahahahahahahahahahah" + (i+1), Yes = Y });
            }
        }

    }
}
