using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wd = Aviary.Wind;
using Wg = Aviary.Wind.Graphics;

namespace Aviary.Hoopoe
{
    public static class GraphicsToSVG
    {

        #region graphics

        public static string ToSVGclass(this Wg.Graphic input)
        {
            string output = input.Stroke.ToSVG() + Environment.NewLine;
            output += input.Fill.ToSVG() + Environment.NewLine;

            return output;
        }

        public static string ToSVG(this Wg.Graphic input)
        {
            string output = input.Stroke.ToSVG() + Environment.NewLine;

            switch (input.Fill.FillType)
            {
                case Wg.Fill.FillTypes.LinearGradient:
                    output += ((Wg.GradientLinear)input.Fill).ToSVG();
                    break;
                case Wg.Fill.FillTypes.RadialGradient:
                    output += ((Wg.GradientRadial)input.Fill).ToSVG();
                    break;
                default:
                    output += input.Fill.ToSVG();
                    break;
            }

            return output;
        }

        public static string ToSVG(this Wg.Stroke input)
        {
            string output = "<style>" + Environment.NewLine;
            output += ".cls-" + input.ID + "{" + Environment.NewLine;
            output += "stroke:" + input.Color.ToSVG() + ";" + Environment.NewLine;
            output += "stroke-opacity:" + input.Color.A / 255.0 + ";" + Environment.NewLine;
            output += "stroke-width:" + input.Weight + ";" + Environment.NewLine;
            output += "stroke-linecap:" + input.Cap.ToSVG() + ";" + Environment.NewLine;
            output += "stroke-linejoin:" + input.Corner.ToSVG() + ";" + Environment.NewLine;
            output += "stroke-miterlimit:" + input.MiterLimit + ";";
            if (input.HasPattern) output += Environment.NewLine + "stroke-dasharray:" + input.Pattern + ";" + Environment.NewLine;
            output += "}";
            output += "</style>";
            return output;
        }


        public static string ToSVG(this Wg.Fill input)
        {
            string output = "<style>" + Environment.NewLine;
            output += ".cls-" + input.ID + "{" + Environment.NewLine;
            output += "fill:" + input.Background.ToSVG() + ";" + Environment.NewLine;
            output += "fill-opacity:" + input.Background.A / 255.0 + ";" + Environment.NewLine;
            output += "}";
            output += "</style>";
            return output;
        }

        public static string ToSVG(this Wg.GradientLinear input)
        {
            double radians = input.Angle / 180 * Math.PI;
            double XA = (50 + Math.Sin(radians) * 50);
            double YA = (50 + Math.Cos(radians) * 50);
            double XB = (50 + Math.Sin(radians + Math.PI) * 50);
            double YB = (50 + Math.Cos(radians + Math.PI) * 50);

            string output = "<linearGradient id=\"" + input.ID + "\" ";
            output += "x1=\"" + XA + "%\" y1=\"" + YA + "%\" ";
            output += "x2=\"" + XB + "%\" y2=\"" + YB + "%\" ";
            output += "gradientUnits=\"objectBoundingBox\" >" + Environment.NewLine;
            for (int i = 0; i < input.Colors.Count; i++)
            {
                output += "<stop offset=\"" + (input.Stops[i] * 100.0) + "%\" style=\"stop-color:" + input.Colors[i].ToSVG() + "; stop-opacity:" + (input.Colors[i].A / 255.0) + "\" />" + Environment.NewLine;
            }
            output += "</linearGradient>";

            return output;
        }

        public static string ToSVG(this Wg.GradientRadial input)
        {

            string output = "<radialGradient id=\"" + input.ID + "\" ";
            output += "cx=\"" + input.Center.X * 100 + "%\" cy=\"" + input.Center.Y * 100 + "%\" r=\"" + input.RadiusX * 100 + "%\" ";
            output += "fx =\"" + input.Focus.X * 100 + "%\" fy=\"" + input.Focus.Y * 100 + "%\" ";
            output += "gradientUnits=\"objectBoundingBox\" >" + Environment.NewLine;
            for (int i = 0; i < input.Colors.Count; i++)
            {
                output += "<stop offset=\"" + (input.Stops[i] * 100.0) + "%\" style=\"stop-color:" + input.Colors[i].ToSVG() + "; stop-opacity:" + (input.Colors[i].A / 255.0) + "\" />" + Environment.NewLine;
            }
            output += "</radialGradient>";

            return output;
        }

        #endregion

        #region effects

        public static string ToSVG(this Wg.Effects input)
        {
            string output = "<filter id=\"" + input.ID + "\" x=\"-25%\" width=\"150%\" y =\"-25%\" height=\"150%\" >" + Environment.NewLine;
            if (input.HasBlurEffect) output += input.Blur.ToSVG() + Environment.NewLine;
            if (input.HasShadowEffect) output += input.Shadow.ToSVG() + Environment.NewLine;
            output += "</filter>" + Environment.NewLine;
            return output;
        }

        public static string ToSVG(this Wg.BlurEffect input)
        {
            return "<feGaussianBlur result=\"blurOut\" in=\"SourceGraphic\" stdDeviation=\"" + input.Radius + "\" />";
        }

        public static string ToSVG(this Wg.ShadowEffect input)
        {
            double radians = (input.Angle + 180) / 180 * Math.PI;
            string output = "<feDropShadow dx=\"" + Math.Round(input.Distance * Math.Sin(radians), 4) + "\" dy=\"" + Math.Round(input.Distance * Math.Cos(radians), 4) + "\" stdDeviation=\"" + input.Radius + "\" flood-color=\"" + input.Color.ToSVG() + "\" flood-opacity=\"" + Math.Round(input.Color.A / 255.0, 4) + "\" />" + Environment.NewLine;
            return output;
        }

        #endregion

        #region primatives

        public static string ToSVG(this Wg.Color input)
        {
            return "rgb(" + input.R + "," + input.G + "," + input.B + ")";
        }

        public static string ToSVG(this Wg.Stroke.StrokeCorners input)
        {
            string output = "round";
            switch (input)
            {
                case Wg.Stroke.StrokeCorners.Miter:
                    output = "miter";
                    break;
                case Wg.Stroke.StrokeCorners.Bevel:
                    output = "bevel";
                    break;
            }
            return output;
        }

        public static string ToSVG(this Wg.Stroke.StrokeCaps input)
        {
            string output = "round";
            switch (input)
            {
                case Wg.Stroke.StrokeCaps.Flat:
                    output = "butt";
                    break;
                case Wg.Stroke.StrokeCaps.Square:
                    output = "square";
                    break;
            }
            return output;
        }

        #endregion

    }
}
