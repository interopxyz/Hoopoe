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
            return "M " + input.StartPoint.ToSVG() + " A " + input.Radius + "," + input.Radius + " " + input.AngleDegrees + " " + Convert.ToInt32(input.Angle > Math.PI) + "," +  Convert.ToInt32(!(Rg.Vector3d.VectorAngle(input.Plane.ZAxis, Rg.Vector3d.ZAxis)>0)) + " " + input.EndPoint.ToSVG() + " ";
        }

        public static string ToSVG(this Rg.Circle input)
        {
            Rg.Vector3d axisX = input.Plane.XAxis;
            axisX.Unitize();
            axisX = axisX * input.Radius;

            Rg.Vector3d axisY = input.Plane.YAxis;
            axisY.Unitize();
            axisY = axisY * input.Radius;

            Rg.Point3d A = input.Plane.Origin + axisX;
            Rg.Point3d B = input.Plane.Origin + axisY;

            axisX.Reverse();
            axisY.Reverse();

            Rg.Point3d C = input.Plane.Origin + axisX;
            Rg.Point3d D = input.Plane.Origin + axisY;

            double radians = Rg.Vector3d.VectorAngle(input.Plane.YAxis, Rg.Vector3d.YAxis, Rg.Plane.WorldXY);
            double degrees = 180.0 - (radians / Math.PI) * 180.0;
            int flip = Convert.ToInt32(!(Rg.Vector3d.VectorAngle(input.Plane.ZAxis, Rg.Vector3d.ZAxis) > 1));

            return "M " + A.ToSVG()
                + " A " + input.Radius + ", " + input.Radius + " " + degrees + " 0," + flip + " " + B.ToSVG()
                + " A " + input.Radius + ", " + input.Radius + " " + degrees + " 0," + flip + " " + C.ToSVG()
                + " A " + input.Radius + ", " + input.Radius + " " + degrees + " 0," + flip + " " + D.ToSVG()
                + " A " + input.Radius + ", " + input.Radius + " " + degrees + " 0," + flip + " " + A.ToSVG();
        }

        public static string ToSVG(this Rg.Ellipse input)
        {
            Rg.Vector3d axisX = input.Plane.XAxis;
            axisX.Unitize();
            axisX = axisX * input.Radius1;

            Rg.Vector3d axisY = input.Plane.YAxis;
            axisY.Unitize();
            axisY = axisY * input.Radius2;

            Rg.Point3d A = input.Plane.Origin+axisX;
            Rg.Point3d B = input.Plane.Origin + axisY;

            axisX.Reverse();
            axisY.Reverse();

            Rg.Point3d C = input.Plane.Origin + axisX;
            Rg.Point3d D = input.Plane.Origin + axisY;

            double radians = Rg.Vector3d.VectorAngle(input.Plane.YAxis, Rg.Vector3d.YAxis, Rg.Plane.WorldXY);
            double degrees = 180.0-(radians / Math.PI) * 180.0;
            int flip = Convert.ToInt32(!(Rg.Vector3d.VectorAngle(input.Plane.ZAxis, Rg.Vector3d.ZAxis) > 1));

            return "M " + A.ToSVG() 
                + " A " + input.Radius1 + ", " + input.Radius2 + " " + degrees + " 0,"+ flip + " " + B.ToSVG()
                + " A " + input.Radius1 + ", " + input.Radius2 + " " + degrees + " 0,"+ flip + " " + C.ToSVG()
                + " A " + input.Radius1 + ", " + input.Radius2 + " " + degrees + " 0,"+ flip + " " + D.ToSVG()
                + " A " + input.Radius1 + ", " + input.Radius2 + " " + degrees + " 0,"+ flip + " " + A.ToSVG();
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