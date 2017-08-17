using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using Autofac.Features.Indexed;

namespace TagsCloudContainer
{
    public interface ITextLoaderMetadata
    {
        FileFormat Format { get; set; }
    }
    public class TagsCloudVizualizer
    {
        private ITagsParser parser;
        private PointF center = new PointF(5000,5000);
        private IIndex<FileFormat, ITextLoader> loaderIndex;
        public TagsCloudVizualizer(IIndex<FileFormat,ITextLoader> loader, ITagsParser parser)
        {
            this.parser = parser;
            loaderIndex = loader;
            rectangles = new Dictionary<PointF, RectangleF>();
        }

        private FileFormat GetFileFormatFromPath(string fileName)
        {
            var inputFileNameExtenstion = Path.GetExtension(fileName);
            switch (inputFileNameExtenstion?.ToLower())
            {
                case "txt":
                    return FileFormat.Txt;
                default:
                    return FileFormat.Unspecified;
            }
        }

        private ImageFormat GetImageFormatFromPath(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            switch (extension?.ToLower())
            {
                case "bmp":
                    return ImageFormat.Bmp;
                default:
                    return ImageFormat.Png;

            }
        }
        public void GenerateCloud(Color brushColor,Color backgroundColor, Font font,Size imageSize, string inputFileName,string outputFileName)
        {
            var loader = loaderIndex[GetFileFormatFromPath(inputFileName)];
            var imageFormat = GetImageFormatFromPath(outputFileName);
            var text = loader.LoadText(inputFileName);
            var tags = parser.ParseTags(text).OrderByDescending(tuple => tuple.Item2).Take(500);
            var bmp = new Bitmap(imageSize.Width,imageSize.Height);
            center = new PointF((float)imageSize.Width / 2, (float)imageSize.Height/2);
            var drawing = Graphics.FromImage(bmp);
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.Clear(backgroundColor);
            Brush textBrush = new SolidBrush(brushColor);
            foreach (var tuple in tags)
            {
                var font2 = new Font(font.FontFamily,tuple.Item2*10);
                var textSize = drawing.MeasureString(tuple.Item1,
                    font2);
                drawing.DrawString(tuple.Item1,font2,textBrush,PutNextRectangle(textSize));
            }
            var minX = rectangles.Values.Min(f => f.X);
            var minY = rectangles.Values.Min(f => f.Y);
            var maxX = rectangles.Values.Max(f => f.X+f.Width);
            var maxY = rectangles.Values.Max(f => f.Y + f.Height);
            drawing.Save();
            var cropArea = new RectangleF(minX, minY, maxX-minX, maxY-minY);
            bmp= bmp.Clone(cropArea, bmp.PixelFormat);
            bmp.Save(outputFileName,imageFormat);
        }
        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            RectangleF rectangle;
            PointF point;
            if (rectangles.Count == 0)
            {
                point = new PointF(center.X - rectangleSize.Width / 2, center.Y + rectangleSize.Height / 2);

                rectangle = new RectangleF(point, rectangleSize);
            }
            else
            {
                var newRect = GetNewRectangle(rectangleSize);
                point = new PointF(newRect.Value.X, newRect.Value.Y);
                rectangle = newRect.Value;
            }
            rectangles.Add(point, rectangle);
            return rectangle;

        }
        private RectangleF? GetNewRectangle(SizeF rectangleSize)
        {
            var orderedPoints = GetOrderedPoints();
            foreach (var orderedPoint in orderedPoints)
            {
                foreach (var rectangle in rectangles[orderedPoint].GetNeighbour(rectangleSize))
                {
                    var intersections = rectangles.Values.FirstOrDefault(rect => rectangle.IntersectsWith(rect));
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
            return rectangles.Keys.OrderBy(p => p.DistanceFrom(center));
        }
        private readonly Dictionary<PointF, RectangleF> rectangles;



    }
}