using Rg = Rhino.Geometry;
using Sw = System.Windows;
using Sm = System.Windows.Media;
using Sh = System.Windows.Shapes;
using System.Linq;
using Wind;
using Wg = Wind.Graphics;

namespace Hoopoe
{
    public static class DrawingToMedia
    {
        #region To Geometry Visual
        
        public static Sm.DrawingVisual ToVisualDrawing(this Shape input)
        {
            Sm.DrawingVisual drawingVisual = new Sm.DrawingVisual();
            Sm.DrawingContext drawingContext = drawingVisual.RenderOpen();
            Sm.GeometryGroup drawing = new Sm.GeometryGroup();

            if (input.IsCompound)
            {
                foreach (Geometry geo in input.Geometries)
                {
                    Sm.Geometry geometry = geo.ToGeometry();
                    drawing.Children.Add(geometry);
                }
            }
            else
            {
                Sm.Geometry geometry = input.ToGeometry();
                drawing.Children.Add(geometry);
            }

            drawingContext.DrawGeometry(input.Graphic.Fill.ToMediaBrush(), input.Graphic.Stroke.ToMediaPen(), drawing);
            drawingContext.Close();

            if (input.Graphic.Effects.HasBlurEffect) drawingVisual.Effect = input.Graphic.Effects.Blur.ToMediaEffect();
            if (input.Graphic.Effects.HasShadowEffect) drawingVisual.Effect = input.Graphic.Effects.Shadow.ToMediaEffect();

            return drawingVisual;
        }

        public static Sm.DrawingVisual ToGeometryVisual(this Drawing input)
        {
            Sm.DrawingVisual drawings = new Sm.DrawingVisual();

            double scale = input.GetScale();
            Rg.Curve curve = input.Frame.ToNurbsCurve();

            double x0 = input.Frame.Center.X - input.Width / scale / 2;
            double x1 = input.Frame.Center.X + input.Width / scale / 2;

            double y0 = input.Frame.Center.Y - input.Height / scale / 2;
            double y1 = input.Frame.Center.Y + input.Height / scale / 2;

            Rg.Rectangle3d rect = new Rg.Rectangle3d(Rg.Plane.WorldXY, new Rg.Point3d(x0, y0, 0), new Rg.Point3d(x1, y1, 0));

            drawings.Children.Add(new Shape(rect.ToNurbsCurve(), new Wg.Graphic(Wg.Strokes.Transparent,new Wg.Fill(input.Background))).ToVisualDrawing());

            foreach (Shape shape in input.Shapes)
            {
                drawings.Children.Add(shape.ToVisualDrawing());
            }

            Sm.TransformGroup xform = new Sm.TransformGroup();

            double shiftW = (input.Width / scale / 2 - input.Frame.Center.X);
            double shiftH = -(input.Height / scale / 2 + input.Frame.Center.Y);

            xform.Children.Add(new Sm.TranslateTransform(shiftW, shiftH));
            xform.Children.Add(new Sm.ScaleTransform(scale, (-1)*scale));

            drawings.Transform = xform;

            return drawings;
        }

        #endregion


        #region To Geometry Drawing

        public static Sm.Drawing ToGeometryDrawing(this Shape input)
        {
            Sm.GeometryDrawing geometryDrawing = new Sm.GeometryDrawing();
            Sm.GeometryGroup drawing = new Sm.GeometryGroup();

            if (input.IsCompound)
            {
                foreach(Geometry geo in input.Geometries)
                {
                    Sm.Geometry geometry = geo.ToGeometry();
                    drawing.Children.Add(geometry);
                }
            }
            else
            {
                Sm.Geometry geometry = input.ToGeometry();
                drawing.Children.Add(geometry);
            }
            
            geometryDrawing.Geometry = drawing;
            geometryDrawing.Pen = input.Graphic.Stroke.ToMediaPen();
            geometryDrawing.Brush = input.Graphic.Fill.ToMediaBrush();

            Sm.DrawingGroup drawingGroup = new Sm.DrawingGroup();
            drawingGroup.Children.Add(geometryDrawing);
            
            return drawingGroup;
        }

        public static Sm.DrawingGroup ToGeometryGroup(this Drawing input)
        {
            Sm.DrawingGroup drawings = new Sm.DrawingGroup();

            double scale = input.GetScale();
            Rg.Curve curve = input.Frame.ToNurbsCurve();

            double x0 = input.Frame.Center.X - input.Width / scale / 2;
            double x1 = input.Frame.Center.X + input.Width / scale / 2;

            double y0 = input.Frame.Center.Y - input.Height / scale / 2;
            double y1 = input.Frame.Center.Y + input.Height / scale / 2;

            Rg.Rectangle3d rect = new Rg.Rectangle3d(Rg.Plane.WorldXY, new Rg.Point3d(x0, y0, 0), new Rg.Point3d(x1, y1, 0));

            drawings.Children.Add(new Shape(rect.ToNurbsCurve(), new Wg.Graphic(Wg.Strokes.Transparent, new Wg.Fill(input.Background))).ToGeometryDrawing());

            foreach (Shape shape in input.Shapes)
            {
                drawings.Children.Add(shape.ToGeometryDrawing());
            }

            drawings.ClipGeometry = input.Frame.ToPolyline().ToGeometry();

            Sm.TransformGroup xform = new Sm.TransformGroup();

            xform.Children.Add(new Sm.TranslateTransform(input.Frame.Center.X - input.Width / 2, input.Frame.Center.Y - input.Height / 2));
            xform.Children.Add(new Sm.ScaleTransform(1, -1));

            drawings.Transform = xform;
            
            return drawings;
        }

        public static Sm.Geometry ToGeometry(this Shape input)
        {
            Sm.Geometry geometry = null;
            geometry = input.Geometries[0].ToGeometry();
            return geometry;
        }

        public static Sm.Geometry ToGeometry(this Geometry input)
        {
            Sm.Geometry geometry = null;
            switch (input.CurveType)
            {
                case Geometry.CurveTypes.Arc:
                    Rg.Arc arc = new Rg.Arc();
                    input.Curve.TryGetArc(out arc);
                    geometry = arc.ToGeometry();
                    break;
                case Geometry.CurveTypes.Circle:
                    Rg.Circle circle = new Rg.Circle();
                    input.Curve.TryGetCircle(out circle);
                    geometry = circle.ToGeometry();
                    break;
                case Geometry.CurveTypes.Ellipse:
                    Rg.Ellipse ellipse = new Rg.Ellipse();
                    input.Curve.TryGetEllipse(out ellipse);
                    geometry = ellipse.ToGeometry();
                    break;
                case Geometry.CurveTypes.Line:
                    Rg.Line line = new Rg.Line(input.Curve.PointAtStart, input.Curve.PointAtEnd);
                    geometry = line.ToGeometry();
                    break;
                case Geometry.CurveTypes.Polyline:
                    Rg.Polyline polyline = new Rg.Polyline();
                    input.Curve.TryGetPolyline(out polyline);
                    geometry = polyline.ToGeometry();
                    break;
                case Geometry.CurveTypes.Rectangle:
                    Rg.Polyline pline = new Rg.Polyline();
                    input.Curve.TryGetPolyline(out pline);
                    Rg.Rectangle3d rectangle = new Rg.Rectangle3d(Rg.Plane.WorldXY, pline[0], pline[2]);
                    geometry = rectangle.ToGeometry();
                    break;
                default:
                    geometry = input.Curve.ToGeometry();
                    break;
            }
            return geometry;
        }

        #endregion

    }

}
