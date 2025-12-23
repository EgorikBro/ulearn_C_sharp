using System.Collections.Generic;
using Avalonia.Media;
using Geometry;

namespace GeometryPainting;

public static class SegmentExtensions
{
    private static Dictionary<Segment, Color> ColorsDict = new Dictionary<Segment, Color>();
	
	public static Color GetColor(this Segment segment)
    {
        if (ColorsDict.ContainsKey(segment))
        {
            return ColorsDict[segment];
        }
        return Colors.Black;
    }

    public static void SetColor(this Segment segment, Color color)
    {
        if (ColorsDict.ContainsKey(segment))
            ColorsDict[segment] = color;
        else 
            ColorsDict.Add(segment, color);
    }
}
