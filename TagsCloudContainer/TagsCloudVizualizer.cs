using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;

namespace TagsCloudContainer
{
    public class TagsCloudVizualizer
    {
        private ITagsParser parser;
        private readonly IImageFIleGenerator generator;
        private PointF center = new PointF(5000,5000);
        public TagsCloudVizualizer(ITagsParser parser)
        {
            this.parser = parser;
            Rectangles = new Dictionary<PointF, RectangleF>();
        }

        public void GenerateCloud()
        {
            var tags = parser.ParseTags().OrderByDescending(tuple => tuple.Item2).Take(500);
            var bmp = new Bitmap(10000, 10000);
            var drawing = Graphics.FromImage(bmp);
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.Clear(Color.White);
            Brush textBrush = new SolidBrush(Color.Black);
            foreach (var tuple in tags)
            {
                var font = new Font("Arial",tuple.Item2*10);
                var textSize = drawing.MeasureString(tuple.Item1,
                    font);
                drawing.DrawString(tuple.Item1,font,textBrush,PutNextRectangle(textSize));
            }
            drawing.Save();
            bmp.Save("abc.png",ImageFormat.Png);
        }
        private RectangleF firstRectangle;
        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            RectangleF rectangle;
            PointF point;
            if (Rectangles.Count == 0)
            {
                point = new PointF(center.X - rectangleSize.Width / 2, center.Y + rectangleSize.Height / 2);

                rectangle = new RectangleF(point, rectangleSize);
                firstRectangle = rectangle;
            }
            else
            {
                var newRect = GetNewRectangle(rectangleSize);
                point = new PointF(newRect.Value.X, newRect.Value.Y);
                rectangle = newRect.Value;
            }
            Rectangles.Add(point, rectangle);
            return rectangle;

        }
        private RectangleF? GetNewRectangle(SizeF rectangleSize)
        {
            var orderedPoints = GetOrderedPoints();
            foreach (var orderedPoint in orderedPoints)
            {
                foreach (var rectangle in Rectangles[orderedPoint].GetNeighbour(rectangleSize))
                {
                    var intersections = Rectangles.Values.FirstOrDefault(rect => rectangle.IntersectsWith(rect));
                    if (intersections == new RectangleF())
                    {
                        return rectangle;
                    }
                }
            }
            return null;
        }
        private IEnumerable<PointF> GetOrderedPoints()
        {
            return Rectangles.Keys.OrderBy(p => p.DistanceFrom(center));
        }
        private Dictionary<PointF, RectangleF> Rectangles;



    }
}