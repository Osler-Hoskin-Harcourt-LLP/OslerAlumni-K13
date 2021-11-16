using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OslerAlumni.Mvc.Core.Extensions
{
    public static class BitMapExtensions
    {
        /// <summary>
        /// https://stackoverflow.com/questions/734930/how-to-crop-an-image-using-c
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Bitmap CropAtRect(this Bitmap bitmap, Rectangle rectangle)
        {
            Bitmap newImage = new Bitmap(rectangle.Width, rectangle.Height);

            newImage.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(bitmap, -rectangle.X, -rectangle.Y);
            }
            return newImage;
        }
    }
}
