using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hoopoe
{
    public static class DrawingToImages
    {

        #region To Geometry Drawing

        public static Bitmap ToBitmap(this System.Windows.Media.Drawing drawing, double width, double height)
        {
            return drawing.ToBitmap(width,height,96, new PngBitmapEncoder());
        }

        public static Bitmap ToBitmap(this System.Windows.Media.Drawing drawing, double width, double height, int dpi, BitmapEncoder encoder)
        {
            System.Windows.Controls.Image drawingImage = new System.Windows.Controls.Image { Source = new DrawingImage(drawing) };
            
            drawingImage.Arrange(new Rect(0, 0, width, height));
            
            var bitmap = new RenderTargetBitmap((int)(width/96*dpi), (int)(height/96*dpi), dpi, dpi, PixelFormats.Pbgra32);
            
            bitmap.Render(drawingImage);

            MemoryStream stream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);

            return new Bitmap(stream);
        }

        #endregion

        #region To Visual Drawing

        public static Bitmap ToBitmap(this System.Windows.Media.DrawingVisual drawing, double width, double height)
        {
            return drawing.ToBitmap(width, height, 96, new PngBitmapEncoder());
        }

        public static Bitmap ToBitmap(this System.Windows.Media.DrawingVisual drawing, double width, double height, int dpi, BitmapEncoder encoder)
        {

            var bitmap = new RenderTargetBitmap((int)(width / 96 * dpi), (int)(height / 96 * dpi), dpi, dpi, PixelFormats.Pbgra32);

            bitmap.Render(drawing);

            MemoryStream stream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);

            return new Bitmap(stream);
        }

        #endregion

    }
}
