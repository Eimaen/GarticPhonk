using GarticTest.Model;
using GarticTest.Model.Messages.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarticTest
{
    internal class GarticDrawController
    {
        public GarticSocket Socket;

        private int objectCount = 0;

        public GarticDrawController(GarticSocket socket)
        {
            Socket = socket;
        }

        private static string Hex(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public void DrawPoint(Color color, Point point, int size = 2, float opacity = 1f)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, new object[] { 1, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { point.X, point.Y } });
            var packetEnd = new GarticDrawMessage(DrawMessageType.End, new object[] { 1, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { point.X, point.Y } });
            Socket.SendString(packetStart.Serialize());
            Socket.SendString(packetEnd.Serialize());
        }

        public void DrawFilledRectangle(Color color, Rectangle rectangle)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, new object[] { 6, objectCount, new object[] { Hex(color), 2, 1 }, new object[] { rectangle.X, rectangle.Y }, new object[] { rectangle.Right, rectangle.Bottom } });
            Socket.SendString(packetStart.Serialize());
        }

        public void DrawMultiplePoints(Color color, Point[] points, int size = 2, float opacity = 1f)
        {
            if (points.Length == 0) return;
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, new object[] { 1, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { points[0].X, points[0].Y } });
            var packetEnd = new GarticDrawMessage(DrawMessageType.End, new object[] { 1, objectCount, new List<object> { Hex(color), size, opacity.ToString().Replace(',', '.') } }.Concat(points.Select(point => new object[] { point.X, point.Y })));
            Console.WriteLine(packetEnd.Serialize());
            Socket.SendString(packetStart.Serialize());
            Socket.SendString(packetEnd.Serialize());
        }

        public void DrawImage(string filename, int pixelSize = 1)
        {
            Bitmap image = Image.FromFile(filename).ResizeImage(760, 425);

            Color mostUsedColor = image.GetMostUsedColor();
            if (mostUsedColor != Color.White)
                DrawFilledRectangle(mostUsedColor, new Rectangle(0, 0, 760, 425));

            for (int x = 0; x < image.Width; x += pixelSize)
                for (int y = 0; y < image.Height; y += pixelSize)
                {
                    Color color = image.GetPixel(x, y);
                    if (color != mostUsedColor && color != Color.Transparent)
                        DrawFilledRectangle(color, new Rectangle(x, y, pixelSize, pixelSize));
                }
        }
    }
}
