namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;
    }

    public class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(Math.Pow(vector.X, 2) +  Math.Pow(vector.Y, 2));
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            var resultVector = new Vector();
            resultVector.X = vector1.X + vector2.X;
            resultVector.Y = vector1.Y + vector2.Y;
            return resultVector;
        }

        public static double GetLength(Segment segment)
        {
            var resultVector = new Vector();
            resultVector.X = Math.Abs(segment.End.X - segment.Begin.X);
            resultVector.Y = Math.Abs(segment.End.Y - segment.Begin.Y);
            return GetLength(resultVector);
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            var segment1 = new Segment();
            segment1.Begin = segment.Begin;
            segment1.End = vector;

            var segment2 = new Segment();
            segment2.Begin = vector;
            segment2.End = segment.End;

            return GetLength(segment1) + GetLength(segment2) == GetLength(segment);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;
    }
}
