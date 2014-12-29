using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class EngineFile
    {
        public static string dataPath = @"Data\Engine.xml";

        string path;

        public string FileName
        {
            get { return path; }
            set { path = value; }
        }

        public EngineFile(string path)
        {
            this.path = path;
        }

        public EngineFile()
        {
           
        }
    }
}
