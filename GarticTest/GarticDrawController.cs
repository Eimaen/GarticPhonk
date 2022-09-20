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
        public int TurnNumber { get; set; }

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
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 1, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { point.X, point.Y } });
            var packetEnd = new GarticDrawMessage(DrawMessageType.End, TurnNumber, new object[] { 1, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { point.X, point.Y } });
            Socket.SendString(packetStart.Serialize());
            Socket.SendString(packetEnd.Serialize());
        }

        public void ErasePoint(Color color, Point point, int size = 2, float opacity = 1f)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 2, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { point.X, point.Y } });
            var packetEnd = new GarticDrawMessage(DrawMessageType.End, TurnNumber, new object[] { 2, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { point.X, point.Y } });
            Socket.SendString(packetStart.Serialize());
            Socket.SendString(packetEnd.Serialize());
        }

        public void DrawFilledRectangle(Color color, Rectangle rectangle, float opacity = 1f)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 6, objectCount, new object[] { Hex(color), 2, opacity.ToString().Replace(',', '.') }, new object[] { rectangle.X, rectangle.Y }, new object[] { rectangle.Right, rectangle.Bottom } });
            Socket.SendString(packetStart.Serialize());
        }

        public void DrawRectangle(Color color, Rectangle rectangle, float opacity = 1f)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 4, objectCount, new object[] { Hex(color), 2, opacity.ToString().Replace(',', '.') }, new object[] { rectangle.X, rectangle.Y }, new object[] { rectangle.Right, rectangle.Bottom } });
            Socket.SendString(packetStart.Serialize());
        }

        public void DrawFilledEllipse(Color color, Rectangle rectangle, float opacity = 1f)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 7, objectCount, new object[] { Hex(color), 2, opacity.ToString().Replace(',', '.') }, new object[] { rectangle.X, rectangle.Y }, new object[] { rectangle.Right, rectangle.Bottom } });
            Socket.SendString(packetStart.Serialize());
        }

        public void DrawEllipse(Color color, Rectangle rectangle, int strokeSize = 2, float opacity = 1f)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 5, objectCount, new object[] { Hex(color), strokeSize, opacity.ToString().Replace(',', '.') }, new object[] { rectangle.X, rectangle.Y }, new object[] { rectangle.Right, rectangle.Bottom } });
            Socket.SendString(packetStart.Serialize());
        }

        public void DrawLine(Color color, Point start, Point destination, int size = 2, float opacity = 1f)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 3, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { start.X, start.Y }, new object[] { destination.X, destination.Y } });
            Socket.SendString(packetStart.Serialize());
        }

        [Obsolete("I don't know how it works so it's still WIP, don't use!", true)]
        public void Fill(Color color, Point point, float opacity = 1f)
        {
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 8, objectCount, new object[] { Hex(color), opacity.ToString().Replace(',', '.') } });
            Socket.SendString(packetStart.Serialize());
        }

        public void DrawMultiplePoints(Color color, Point[] points, int size = 2, float opacity = 1f)
        {
            if (points.Length == 0) return;
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 1, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { points[0].X, points[0].Y } });
            var packetEnd = new GarticDrawMessage(DrawMessageType.End, TurnNumber, new object[] { 1, objectCount, new List<object> { Hex(color), size, opacity.ToString().Replace(',', '.') } }.Concat(points.Select(point => new object[] { point.X, point.Y })));
            Socket.SendString(packetStart.Serialize());
            Socket.SendString(packetEnd.Serialize());
        }
        public void EraseMultiplePoints(Color color, Point[] points, int size = 2, float opacity = 1f)
        {
            if (points.Length == 0) return;
            objectCount++;
            var packetStart = new GarticDrawMessage(DrawMessageType.Begin, TurnNumber, new object[] { 2, objectCount, new object[] { Hex(color), size, opacity.ToString().Replace(',', '.') }, new object[] { points[0].X, points[0].Y } });
            var packetEnd = new GarticDrawMessage(DrawMessageType.End, TurnNumber, new object[] { 2, objectCount, new List<object> { Hex(color), size, opacity.ToString().Replace(',', '.') } }.Concat(points.Select(point => new object[] { point.X, point.Y })));
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
