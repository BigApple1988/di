using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public static class RectangleExtensions
    {
        public static IEnumerable<RectangleF> GetNeighbour(this RectangleF rectangle, SizeF size)
        {
            yield return new RectangleF(
                rectangle.X - size.Width,
                rectangle.Y + rectangle.Height,
                size.Width, size.Height);
            yield return new RectangleF(
                rectangle.X - size.Width, 
                rectangle.Y,
                size.Width,size.Height);
            yield return new RectangleF(
                rectangle.X - size.Width, 
                rectangle.Y - size.Height,
                size.Width, size.Height);
            yield return new RectangleF(
                rectangle.X + rectangle.Width, 
                rectangle.Y, 
                size.Width, size.Height);
            yield return new RectangleF(
                rectangle.X + rectangle.Width, 
                rectangle.Y + rectangle.Height,
                size.Width, size.Height);
            yield return new RectangleF(
                rectangle.X + rectangle.Width, 
                rectangle.Y - size.Height, 
                size.Width, size.Height);
            yield return new RectangleF(
                rectangle.X, 
                rectangle.Y + rectangle.Height, 
                size.Width, size.Height);
            yield return new RectangleF(
                rectangle.X, 
                rectangle.Y - size.Height, 
                size.Width, size.Height);
        }

    }
}