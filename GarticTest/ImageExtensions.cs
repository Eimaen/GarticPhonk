using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest
{
    public static class ImageExtensions
    {
        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap CanvasFit(this Image image, Size canvasSize)
        {
            var destImage = new Bitmap(canvasSize.Width, canvasSize.Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                double widthRatio = (double)canvasSize.Width / image.Width;
                double heightRatio = (double)canvasSize.Height / image.Height;
                double ratio = widthRatio < heightRatio ? widthRatio : heightRatio;

                int newWidth = (int)(image.Width * ratio);
                int newHeight = (int)(image.Height * ratio);

                int posX = (int)((canvasSize.Width - (image.Width * ratio)) / 2);
                int posY = (int)((canvasSize.Height - (image.Height * ratio)) / 2);

                graphics.Clear(Color.Transparent);
                graphics.DrawImage(image, posX, posY, newWidth, newHeight);
            }

            return destImage;
        }

        public static Color GetMostUsedColor(this Bitmap image)
        {
            Dictionary<int, int> dctColorIncidence = new Dictionary<int, int>();

            for (int row = 0; row < image.Size.Width; row++)
            {
                for (int col = 0; col < image.Size.Height; col++)
                {
                    Color color = image.GetPixel(row, col);

                    if (color == Color.Transparent)
                        continue;

                    int pixelColor = color.ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                        dctColorIncidence[pixelColor]++;
                    else
                        dctColorIncidence.Add(pixelColor, 1);
                }
            }

            return Color.FromArgb(dctColorIncidence.OrderByDescending(x => x.Value).First().Key);
        }
    }
}
