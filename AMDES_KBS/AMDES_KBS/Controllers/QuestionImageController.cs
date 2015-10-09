using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ADD.Controllers
{
    public class QuestionImageController
    {
        public static string appStartUpPath = AppDomain.CurrentDomain.BaseDirectory;

        public void processImage(string clientFilePath, int maxWidth, int maxheight, string saveFilePath)
        {
            string sPath = saveFilePath + ".jpg";
            if (System.IO.File.Exists(sPath))
            {
                System.IO.File.Delete(sPath);
            }


            System.Drawing.Image selectedImage = System.Drawing.Image.FromFile(clientFilePath);
            Bitmap tmpPhoto = default(Bitmap);
            int newWidth = 0;
            int newHeight = 0;

            if (selectedImage.Width > maxWidth || selectedImage.Height > maxheight)
            {
                double widthRatio = Convert.ToDouble(selectedImage.Width) / maxWidth;
                double heightRatio = Convert.ToDouble(selectedImage.Height) / maxheight;
                double ratio = Math.Max(widthRatio, heightRatio);
                newWidth = Convert.ToInt32((selectedImage.Width / ratio));
                newHeight = Convert.ToInt32((selectedImage.Height / ratio));
            }
            else
            {
                newWidth = selectedImage.Width;
                newHeight = selectedImage.Height;
            }

            tmpPhoto = new Bitmap(selectedImage, newWidth, newHeight);
            Graphics g = Graphics.FromImage(tmpPhoto);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(selectedImage, 0, 0, newWidth, newHeight);

            tmpPhoto.Save(sPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            tmpPhoto.Dispose();
            selectedImage.Dispose();


        }

    }
}
