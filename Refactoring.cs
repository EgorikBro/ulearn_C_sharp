using System;
using Avalonia.Media;
using RefactorMe.Common;

namespace RefactorMe
{
    public class Drawer
    {
        private static float x;
        private static float y;
        private static IGraphics graphics;
        private static readonly Pen YellowPen = new Pen(Brushes.Yellow);

        public static void Initialize(IGraphics graphics)
        {
            Drawer.graphics = graphics;
            graphics.Clear(Colors.Black);
        }

        public static void SetPosition(float x, float y)
        {
            Drawer.x = x;
            Drawer.y = y;
        }

        public static void DrawLine(double length, double angle)
        {
            var newX = (float)(x + length * Math.Cos(angle));
            var newY = (float)(y + length * Math.Sin(angle));
            graphics.DrawLine(YellowPen, x, y, newX, newY);
            x = newX;
            y = newY;
        }

        public static void Move(double length, double angle)
        {
            x = (float)(x + length * Math.Cos(angle));
            y = (float)(y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        public static void Draw(int width, int height, double rotationAngle, IGraphics graphics)
        {
            Drawer.Initialize(graphics);
            var size = Math.Min(width, height);

            var mainLineLength = size * 0.375;
            var notchLength = size * 0.04;
            var notchDiagonalLength = notchLength * Math.Sqrt(2);

            var diagonalLength = Math.Sqrt(2) * (mainLineLength + notchLength) / 2;
            var centerX = (float)width / 2;
            var centerY = (float)height / 2;
            var startX = (float)(diagonalLength * Math.Cos(Math.PI + Math.PI/4)) + centerX;
            var startY = (float)(diagonalLength * Math.Sin(Math.PI + Math.PI/4)) + centerY;

            Drawer.SetPosition(startX, startY);

            var sideAngles = new[] { 0, -Math.PI/2, Math.PI, Math.PI/2 };
            foreach (var sideAngle in sideAngles)
                DrawSquareSide(mainLineLength, notchLength, notchDiagonalLength, sideAngle);
        }

        private static void DrawSquareSide(double mainLineLength, double notchLength, 
            double notchDiagonalLength, double sideAngle)
        {
            Drawer.DrawLine(mainLineLength, sideAngle);
            Drawer.DrawLine(notchDiagonalLength, sideAngle + Math.PI/4);
            Drawer.DrawLine(mainLineLength, sideAngle + Math.PI);
            Drawer.DrawLine(mainLineLength - notchLength, sideAngle + Math.PI/2);

            Drawer.Move(notchLength, sideAngle - Math.PI);
            Drawer.Move(notchDiagonalLength, sideAngle + 3*Math.PI/4);
        }
    }
}
