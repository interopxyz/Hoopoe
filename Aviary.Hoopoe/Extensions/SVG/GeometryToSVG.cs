using System;
using System.Linq;

using Rg = Rhino.Geometry;

using Wg = Aviary.Wind.Graphics;

namespace Aviary.Hoopoe
{
    public static class GeometryToSVG
    {


        public static string ToSVG(this Rg.Point3d input, int decimals = 4)
        {
            return Math.Round(input.X, decimals) + "," + Math.Round(input.Y, decimals) + " ";
        }

        public static string ToSVG(this Rg.Line input)
        {
            return "M " + input.From.ToSVG() + input.To.ToSVG() + " ";
        }

        public static string ToSVG(this Rg.Rectangle3d input)
        {
            return "M " + input.Corner(0).ToSVG() + input.Corner(1).ToSVG() + input.Corner(2).ToSVG() + input.Corner(3).ToSVG() + input.Corner(0).ToSVG() + " ";
        }

        public static string ToSVG(this Rg.Arc input)
        {
            return "M " + input.StartPoint.ToSVG() + " A " + input.Radius + "," + input.Radius + " " + input.AngleDegrees + Convert.ToInt32(input.Angle > Math.PI) + "," + Convert.ToInt32(input.AngleDomain.T0 < input.AngleDomain.T1) + " " + input.EndPoint.ToSVG() + " ";
        }

        public static string ToSVG(this Rg.Circle input)
        {
            Rg.Curve curve = input.ToNurbsCurve();
            return "M " + curve.PointAtStart.ToSVG() + "a " + input.Radius + ", " + input.Radius + " 0 1,0 " + input.Radius * (-2) + ", 0" + " a " + input.Radius + ", " + input.Radius + " 0 1,0 " + input.Radius * (2) + ", 0 ";
        }

        public static string ToSVG(this Rg.Ellipse input)
        {
            Rg.Curve curve = input.ToNurbsCurve();
            Rg.Point3d start = curve.PointAtStart;
            Rg.Point3d mid = curve.PointAtNormalizedLength(0.5);
            double radians = Rg.Vector3d.VectorAngle(Rg.Vector3d.XAxis, (curve.PointAtStart-input.Plane.Origin),Rg.Plane.WorldXY);
            double degrees = radians / Math.PI * 180;

            return "M " + start.ToSVG() 
                + " A " + input.Radius1 + ", " + input.Radius2 + " " + degrees + " 1,0 " + mid.ToSVG()
                + " A " + input.Radius1 + ", " + input.Radius2 + " " + degrees + " 1,0 " + start.ToSVG()
                + " ";
        }

        public static string ToSVG(this Rg.Polyline input)
        {
            Rg.Curve curve = input.ToNurbsCurve();
            string output = "M " + input[0].ToSVG();
            for (int i = 1; i < input.Count; i++)
            {
                output += input[i].ToSVG();
            }
            output += " ";
            return output;
        }

        public static string ToSVG(this Rg.Curve input)
        {
            Rg.NurbsCurve nurbs = input.ToNurbsCurve();
            nurbs.MakePiecewiseBezier(true);
            Rhino.Geometry.BezierCurve[] bezier = Rg.BezierCurve.CreateCubicBeziers(nurbs, 0, 0);

            string output = "M " + input.PointAtStart.ToSVG();
            for (int i = 0; i < bezier.Count(); i++)
            {
                output += " C " + bezier[i].GetControlVertex3d(1).ToSVG() + bezier[i].GetControlVertex3d(2).ToSVG() + bezier[i].GetControlVertex3d(3).ToSVG();
            }
            output += " ";
            return output;
        }
    }
}