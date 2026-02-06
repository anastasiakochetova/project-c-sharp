using System.Collections.Generic;
using Avalonia.Media;
using Geometry;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        private static readonly Dictionary<Segment, Color> colors =
            new Dictionary<Segment, Color>();

        public static void SetColor(this Segment segment, Color color)
        {
            colors[segment] = color;
        }

        public static Color GetColor(this Segment segment)
        {
            if (colors.TryGetValue(segment, out var stored))
                return stored;

            return Colors.Black;
        }
    }
}
