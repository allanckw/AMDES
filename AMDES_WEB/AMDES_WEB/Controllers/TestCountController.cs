using System.Collections.Generic;
using System.Xml.Linq;
using AMDES_KBS.Entity;
using System.IO;


namespace AMDES_KBS.Controllers
{
    public class TestCountController
    {
        public static void addCount(WebApplicationContext app)
        {
            string fileName = app.FolderPath + @"/count.CONF";
            int count = 0;

            if (!File.Exists(fileName))
            {
                StreamWriter file = new StreamWriter(fileName);
                file.WriteLine("0");
                file.Close();
            }
            else
            {
                StreamReader reader = new StreamReader(fileName);
                count = int.Parse(reader.ReadToEnd());
                reader.Close();

                count += 1;

                StreamWriter writer = new StreamWriter(fileName, false);
                writer.WriteLine(count);
                writer.Close();

            }
        }

        public static int readCount(WebApplicationContext app)
        {
            string fileName = app.FolderPath + @"/count.CONF";
            int count = 0;

            if (File.Exists(fileName))
            {
                StreamReader reader = new StreamReader(fileName);
                count = int.Parse(reader.ReadToEnd());
                reader.Close();
                return count;
            }
            else
            {
                addCount(app);
                return 1;
            }
            
        }
    }
}