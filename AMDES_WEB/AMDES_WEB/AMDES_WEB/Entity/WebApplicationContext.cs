using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class WebApplicationContext
    {
        private string desc;
        private string name;
        private string folderPath;
        private string folderName = "";

        private bool isSelected;
        private bool isConfiguredCorrectly;

        public bool IsConfiguredCorrectly
        {
            get { return isConfiguredCorrectly; }
            set { isConfiguredCorrectly = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        public string FolderName
        {
            get
            {
                if (folderName.Length == 0)
                {
                    folderName = folderPath.Substring(folderPath.LastIndexOf(@"\"), folderPath.Length - folderPath.LastIndexOf(@"\"));
                    folderName = folderName.Replace(@"\", "");
                }
                return folderName;
            }

        }


        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }


    }
}
