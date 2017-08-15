using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloudContainer;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Dictionary<PointF, RectangleF> Rectangles;
        private PointF center;
        private RectangleF firstRectangle;
        public Dictionary<PointF, RectangleF> GetCloud()
        {
            return Rectangles;
        }

        public CircularCloudLayouter(PointF center)
        {
            Rectangles = new Dictionary<PointF, RectangleF>();
            this.center = center;
        }

      private IEnumerable<PointF> GetOrderedPoints()
        {
            return Rectangles.Keys.OrderBy(p => p.DistanceFrom(center));
        }
        public void SaveBitmap(string fileName)
        {
            var minX = Rectangles.Values.Min(r => r.X);
            var minY = Rectangles.Values.Min(r => r.Y);
            var maxX = Rectangles.Values.Max(r => r.X+r.Width);
            var maxY = Rectangles.Values.Max(r => r.Y + r.Height);
            var bitmap = new Bitmap((int)maxX,(int)maxY);
            SolidBrush b = new SolidBrush(Color.White);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(b,0,0,maxX,maxY);
            var pen = new Pen(Color.Red,1);
            
            var random = new Random();
            foreach (var rectangle in Rectangles.Values)
            {
                g.DrawRectangle(pen,rectangle.X,rectangle.Y,rectangle.Width,rectangle.Height);
                if (rectangle == firstRectangle)
                {
                    g.FillRectangle(new SolidBrush(Color.BlueViolet),rectangle);
                }
                pen.Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }
            var cropArea = new RectangleF(minX,minY,maxX-minX,maxY-minY);
            bitmap = bitmap.Clone(cropArea, bitmap.PixelFormat);
            bitmap.Save($"{fileName}.bmp");
            
            
        }
        public RectangleF PutNextRectangle(SizeF rectangleSize)
        {
            RectangleF rectangle;
            PointF point;
            if (Rectangles.Count == 0)
            {
                point = new PointF(center.X-rectangleSize.Width/2,center.Y+rectangleSize.Height/2);
                
                rectangle = new RectangleF(point,rectangleSize);
                firstRectangle = rectangle;
            }
            else
            {
                var newRect = GetNewRectangle(rectangleSize);
                point = new PointF(newRect.Value.X, newRect.Value.Y);
                rectangle = newRect.Value;
            }
            Rectangles.Add(point,rectangle);
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

        
    }

  }