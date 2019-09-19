using Sw = System.Windows;
using Sm = System.Windows.Media;
using Sh = System.Windows.Shapes;
using System.Linq;

using Rg = Rhino.Geometry;

using Wg = Aviary.Wind.Graphics;

namespace Aviary.Hoopoe
{
    public static class GeometryToMedia
    {
        #region To Windows Point

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Point3d
        /// </summary>
        /// <param name="input">Rhinocommon Point3d</param>
        /// <returns>System Windows Point</returns>
        public static Sw.Point ToWindowsPoint(this Rg.Point3d input)
        {
            return new Sw.Point(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Point3f
        /// </summary>
        /// <param name="input">Rhinocommon Point3f</param>
        /// <returns>System Windows Point</returns>
        public static Sw.Point ToWindowsPoint(this Rg.Point3f input)
        {
            return new Sw.Point(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Point2d
        /// </summary>
        /// <param name="input">Rhinocommon Point2d</param>
        /// <returns>System Windows Point</returns>
        public static Sw.Point ToWindowsPoint(this Rg.Point2d input)
        {
            return new Sw.Point(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Point2f
        /// </summary>
        /// <param name="input">Rhinocommon Point2f</param>
        /// <returns>System Windows Point</returns>
        public static Sw.Point ToWindowsPoint(this Rg.Point2f input)
        {
            return new Sw.Point(input.X, input.Y);
        }

        #endregion

        #region To Windows Vector

        /// <summary>
        /// Returns a Windows Vector from a Rhino.Geometry.Vector3d
        /// </summary>
        /// <param name="input">Rhinocommon Vector3d</param>
        /// <returns>System Windows Vector</returns>
        public static Sw.Vector ToWindowsVector(this Rg.Vector3d input)
        {
            return new Sw.Vector(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Vector3f
        /// </summary>
        /// <param name="input">Rhinocommon Vector3f</param>
        /// <returns>System Windows Vector</returns>
        public static Sw.Vector ToWindowsVector(this Rg.Vector3f input)
        {
            return new Sw.Vector(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Vector2d
        /// </summary>
        /// <param name="input">Rhinocommon Vector2d</param>
        /// <returns>System Windows Vector</returns>
        public static Sw.Vector ToWindowsVector(this Rg.Vector2d input)
        {
            return new Sw.Vector(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Vector2f
        /// </summary>
        /// <param name="input">Rhinocommon Vector2f</param>
        /// <returns>System Windows Vector</returns>
        public static Sw.Vector ToWindowsVector(this Rg.Vector2f input)
        {
            return new Sw.Vector(input.X, input.Y);
        }

        #endregion

        #region To Windows Media Curve Geometry

        /// <summary>
        /// Returns a Windows Media Line from a Rhinocommon Line
        /// </summary>
        /// <param name="input">Rhinocommon Line</param>
        /// <returns>System Windows Media Line</returns>
        public static Sm.LineGeometry ToGeometry(this Rg.Line input)
        {
            return new Sm.LineGeometry(input.From.ToWindowsPoint(), input.To.ToWindowsPoint());
        }

        /// <summary>
        /// Returns a Windows Media Rect from a Rhinocommon Rectangle
        /// </summary>
        /// <param name="input">Rhinocommon Rectangle</param>
        /// <returns>System Windows Media Rect</returns>
        public static Sm.RectangleGeometry ToGeometry(this Rg.Rectangle3d input)
        {
            Sw.Point center = input.Center.ToWindowsPoint();
            double angle = Rg.Vector3d.VectorAngle(Rg.Vector3d.YAxis, input.Plane.YAxis, Rg.Plane.WorldXY);
            Sm.Transform xform = new Sm.RotateTransform(angle, center.X, center.Y);

            Sw.Rect rect = new Sw.Rect(center.X-input.Width/2,center.Y+input.Height/2,input.Width,input.Height);
            return new Sm.RectangleGeometry(rect,0,0,xform);
        }

        /// <summary>
        /// Returns a Windows Media Ellipse from a Rhinocommon Circle
        /// </summary>
        /// <param name="input">Rhinocommon Circle</param>
        /// <returns>System Windows Media Circle</returns>
        public static Sm.EllipseGeometry ToGeometry(this Rg.Circle input)
        {
            return new Sm.EllipseGeometry(input.Center.ToWindowsPoint(), input.Radius, input.Radius);
        }

        /// <summary>
        /// Returns a Windows Media Ellipse from a Rhinocommon Ellipse
        /// </summary>
        /// <param name="input">Rhinocommon Ellipse</param>
        /// <returns>System Windows Media Ellipse</returns>
        public static Sm.EllipseGeometry ToGeometry(this Rg.Ellipse input)
        {
            Sw.Point origin = input.Plane.Origin.ToWindowsPoint();
            double angle = Rg.Vector3d.VectorAngle(Rg.Vector3d.YAxis, input.Plane.YAxis, Rg.Plane.WorldXY);
            angle = angle / System.Math.PI * 180;
            Sm.Transform xform = new Sm.RotateTransform(angle, origin.X, origin.Y);

            return new Sm.EllipseGeometry(origin, input.Radius1, input.Radius2, xform);
        }

        /// <summary>
        /// Returns a Windows Media Path Geometry from a Rhinocommon Arc
        /// </summary>
        /// <param name="input">Rhinocommon Arc</param>
        /// <returns>System Windows Media Path Geometry</returns>
        public static Sm.PathGeometry ToGeometry(this Rg.Arc input)
        {
            Sm.ArcSegment arc = new Sm.ArcSegment();
            Sm.PathFigure figure = new Sm.PathFigure();
            Sm.PathGeometry geometry = new Sm.PathGeometry();
            Sm.PathFigureCollection figureCollection = new Sm.PathFigureCollection();
            Sm.PathSegmentCollection segmentCollection = new Sm.PathSegmentCollection();

            figure.StartPoint = input.StartPoint.ToWindowsPoint();

            arc.Point = input.EndPoint.ToWindowsPoint();
            arc.Size = new Sw.Size(input.Radius, input.Radius);
            if (input.AngleDomain.IsIncreasing) { arc.SweepDirection = Sm.SweepDirection.Clockwise; } else { arc.SweepDirection = Sm.SweepDirection.Counterclockwise; }

            segmentCollection.Add(arc);
            figure.Segments = segmentCollection;
            figureCollection.Add(figure);
            geometry.Figures = figureCollection;

            return geometry;
        }

        /// <summary>
        /// Returns a Windows Media Path Geometry from a Rhinocommon Polyline
        /// </summary>
        /// <param name="input">Rhinocommon Polyline</param>
        /// <returns>System Windows Media Path Geometry</returns>
        public static Sm.PathGeometry ToGeometry(this Rg.Polyline input)
        {
            Sm.PathFigure figure = new Sm.PathFigure();
            Sm.PathGeometry geometry = new Sm.PathGeometry();
            Sm.PathFigureCollection figureCollection = new Sm.PathFigureCollection();
            Sm.PathSegmentCollection segmentCollection = new Sm.PathSegmentCollection();

            figure.StartPoint = input[0].ToWindowsPoint();
            for (int i = 1; i < input.Count; i++)
            {
                Sm.LineSegment line = new Sm.LineSegment(input[i].ToWindowsPoint(), true);
                segmentCollection.Add(line);
            }

            figure.Segments = segmentCollection;
            figureCollection.Add(figure);
            geometry.Figures = figureCollection;

            return geometry;
        }

        /// <summary>
        /// Returns a Windows Media Bezier Spline Path Geometry from a Rhinocommon Curve
        /// </summary>
        /// <param name="input">Rhinocommon Curve</param>
        /// <returns>System Windows Media Bezier Curve Path Geometry </returns>
        public static Sm.PathGeometry ToGeometry(this Rg.Curve input)
        {
            Rg.NurbsCurve nurbsCurve = input.ToNurbsCurve();
            nurbsCurve.MakePiecewiseBezier(true);
            Rg.BezierCurve[] bezier = Rg.BezierCurve.CreateCubicBeziers(nurbsCurve,0,0);
            
            Sm.PathFigure figure = new Sm.PathFigure();
            Sm.PathGeometry geometry = new Sm.PathGeometry();
            Sm.PathFigureCollection figureCollection = new Sm.PathFigureCollection();
            Sm.PathSegmentCollection segmentCollection = new Sm.PathSegmentCollection();

            figure.StartPoint = bezier[0].GetControlVertex3d(0).ToWindowsPoint();
            for (int i = 0; i < bezier.Count(); i++)
            {
                Sm.BezierSegment segment = new Sm.BezierSegment(bezier[i].GetControlVertex3d(1).ToWindowsPoint(), bezier[i].GetControlVertex3d(2).ToWindowsPoint(), bezier[i].GetControlVertex3d(3).ToWindowsPoint(), true);
                segmentCollection.Add(segment);
            }

            figure.Segments = segmentCollection;
            figureCollection.Add(figure);
            geometry.Figures = figureCollection;

            return geometry;
        }

        #endregion

        #region To Windows Media Path

        /// <summary>
        /// Returns a Windows Media Line from a Rhinocommon Line
        /// </summary>
        /// <param name="input">Rhinocommon Vector3d</param>
        /// <returns>System Windows Vector</returns>
        public static Sh.Path ToPath(this Rg.Line input)
        {
            Sh.Path path = new Sh.Path();
            
            path.Data = input.ToGeometry();

            return path;
        }

        /// <summary>
        /// Returns a Windows Shapes Path from a Rhinocommon Circle
        /// </summary>
        /// <param name="input">Rhinocommon Circle</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Circle input)
        {
            Sh.Path path = new Sh.Path();

            path.Data = input.ToGeometry();

            return path;
        }

        /// <summary>
        /// Returns a Windows Shapes Path from a Rhinocommon Ellipse
        /// </summary>
        /// <param name="input">Rhinocommon Ellipse</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Ellipse input)
        {
            Sh.Path path = new Sh.Path();

            path.Data = input.ToGeometry();

            return path;
        }

        /// <summary>
        /// Returns a Windows Media Path from a Rhinocommon Arc
        /// </summary>
        /// <param name="input">Rhinocommon Arc</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Arc input)
        {
            Sh.Path path = new Sh.Path();

            path.Data = input.ToGeometry();
            
            return path;
        }

        /// <summary>
        /// Returns a Windows Shapes Path from a Rhinocommon Rectangle
        /// </summary>
        /// <param name="input">Rhinocommon Rectangle</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Rectangle3d input)
        {
            Sh.Path path = new Sh.Path();

            path.Data = input.ToGeometry();

            return path;
        }

        /// <summary>
        /// Returns a Windows Shapes Path from a Rhinocommon Polyline
        /// </summary>
        /// <param name="input">Rhinocommon Polyline</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Polyline input)
        {
            Sh.Path path = new Sh.Path();

            path.Data = input.ToGeometry();

            return path;
        }

        /// <summary>
        /// Returns a Windows Media Shapes Bezier Path from a Rhinocommon Curve
        /// </summary>
        /// <param name="input">Rhinocommon Curve</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Curve input)
        {
            Sh.Path path = new Sh.Path();

            path.Data = input.ToGeometry();

            return path;
        }

        #endregion

    }

}
