using System;
using Avalonia.Media;
using RefactorMe.Common;

namespace RefactorMe
{
    public static class Drawer
    {
        private static float _currentX;
        private static float _currentY;
        private static IGraphics _graphics;

        public static void Initialize(IGraphics graphics)
        {
            _graphics = graphics;
            _graphics.Clear(Colors.Black);
        }

        public static void SetPosition(float x, float y)
        {
            _currentX = x;
            _currentY = y;
        }

        public static void DrawLine(Pen pen, double length, double angle)
        {
            var newX = (float)(_currentX + length * Math.Cos(angle));
            var newY = (float)(_currentY + length * Math.Sin(angle));
            _graphics.DrawLine(pen, _currentX, _currentY, newX, newY);
            _currentX = newX;
            _currentY = newY;
        }

        public static void Move(double length, double angle)
        {
            _currentX = (float)(_currentX + length * Math.Cos(angle));
            _currentY = (float)(_currentY + length * Math.Sin(angle));
        }
    }
    
    public static class ImpossibleSquare
    {
        private const double MainSegmentRatio = 0.375;
        private const double OffsetRatio = 0.04;
        private const double Sqrt2 = 1.41421356237;
        private const double Pi = Math.PI;
        private const double HalfPi = Math.PI / 2;
        private const double QuarterPi = Math.PI / 4;
        private const double ThreeQuartersPi = 3 * Math.PI / 4;
        private const int SegmentCount = 4;

        public static void Draw(int width, int height, double rotationAngle, IGraphics graphics)
        {
            Drawer.Initialize(graphics);
            var size = CalculateOptimalSize(width, height);
            
            var startPosition = CalculateStartPosition(width, height, size);
            Drawer.SetPosition(startPosition.X, startPosition.Y);

            DrawAllSegments(size);
        }

        private static int CalculateOptimalSize(int width, int height)
        {
            return Math.Min(width, height);
        }

        private static (float X, float Y) CalculateStartPosition(int width, int height, int size)
        {
            var diagonalLength = CalculateDiagonalLength(size);
            var startAngle = CalculateStartAngle();
            
            var x = (float)(diagonalLength * Math.Cos(startAngle)) + width / 2f;
            var y = (float)(diagonalLength * Math.Sin(startAngle)) + height / 2f;
            
            return (x, y);
        }

        private static double CalculateDiagonalLength(int size)
        {
            var mainSegment = size * MainSegmentRatio;
            var offset = size * OffsetRatio;
            return Math.Sqrt(2) * (mainSegment + offset) / 2;
        }

        private static double CalculateStartAngle()
        {
            return QuarterPi + Pi;
        }

        private static void DrawAllSegments(int size)
        {
            for (int segmentIndex = 0; segmentIndex < SegmentCount; segmentIndex++)
            {
                DrawSegment(size, segmentIndex);
            }
        }

        private static void DrawSegment(int size, int segmentIndex)
        {
            var baseAngle = CalculateBaseAngle(segmentIndex);
            var drawingTool = CreateDrawingTool();
            var dimensions = CalculateSegmentDimensions(size);

            DrawMainLines(drawingTool, dimensions, baseAngle);
            PerformPositionAdjustment(dimensions, baseAngle);
        }

        private static double CalculateBaseAngle(int segmentIndex)
        {
            return segmentIndex * -HalfPi;
        }

        private static Pen CreateDrawingTool()
        {
            return new Pen(Brushes.Yellow);
        }

        private static SegmentDimensions CalculateSegmentDimensions(int size)
        {
            var mainLength = size * MainSegmentRatio;
            var offsetLength = size * OffsetRatio;
            var diagonalOffset = offsetLength * Sqrt2;

            return new SegmentDimensions(mainLength, offsetLength, diagonalOffset);
        }

        private static void DrawMainLines(Pen pen, SegmentDimensions dimensions, double baseAngle)
        {
            Drawer.DrawLine(pen, dimensions.MainLength, baseAngle);
            Drawer.DrawLine(pen, dimensions.DiagonalOffset, baseAngle + QuarterPi);
            Drawer.DrawLine(pen, dimensions.MainLength, baseAngle + Pi);
            Drawer.DrawLine(pen, dimensions.MainLength - dimensions.OffsetLength, baseAngle + HalfPi);
        }

        private static void PerformPositionAdjustment(SegmentDimensions dimensions, double baseAngle)
        {
            Drawer.Move(dimensions.OffsetLength, baseAngle - Pi);
            Drawer.Move(dimensions.DiagonalOffset, baseAngle + ThreeQuartersPi);
        }

        private readonly struct SegmentDimensions
        {
            public double MainLength { get; }
            public double OffsetLength { get; }
            public double DiagonalOffset { get; }

            public SegmentDimensions(double mainLength, double offsetLength, double diagonalOffset)
            {
                MainLength = mainLength;
                OffsetLength = offsetLength;
                DiagonalOffset = diagonalOffset;
            }
        }
    }
}
