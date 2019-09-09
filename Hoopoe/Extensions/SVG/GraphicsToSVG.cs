using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wd = Wind;
using Wg = Wind.Graphics;

namespace Hoopoe
{
    public static class GraphicsToSVG
    {

        #region graphics

        public static string ToSVGclass(this Wg.Graphic input)
        {
            string output = "<style>" + Environment.NewLine;
            output += ".cls-"+input.ID + "{" + Environment.NewLine;
            output += input.ToSVG() + Environment.NewLine;
            output += "</style>" + Environment.NewLine;
            return output;
        }

        public static string ToSVG(this Wg.Graphic input)
        {
            string output = input.Stroke.ToSVG() + Environment.NewLine;
            output += input.Fill.ToSVG();

            return output;
        }

        public static string ToSVG(this Wg.Stroke input)
        {
            string output = "stroke:" + input.Color.ToSVG()+";" + Environment.NewLine;
            output += "stroke-opacity:" + input.Color.A / 255.0 + ";" + Environment.NewLine;
            output += "stroke-width:" + input.Weight + ";" + Environment.NewLine;
            output += "stroke-linecap:" + input.Cap.ToSVG() + ";" + Environment.NewLine;
            output += "stroke-linejoin:" + input.Corner.ToSVG() + ";" + Environment.NewLine;
            output += "stroke-miterlimit:" + input.MiterLimit + ";";
            if (input.HasPattern) output += Environment.NewLine + "stroke-dasharray:" + input.Pattern + ";";
            return output;
        }


        public static string ToSVG(this Wg.Fill input)
        {
            string output = "fill:" + input.Background.ToSVG() +";"+ Environment.NewLine;
            output += "fill-opacity:" + input.Background.A / 255.0 + ";";
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
            string output = "<feDropShadow dx=\"" + Math.Round(input.Distance * Math.Sin(radians),4) + "\" dy=\"" + Math.Round(input.Distance * Math.Cos(radians),4) + "\" stdDeviation=\"" + input.Radius + "\" flood-color=\"" + input.Color.ToSVG()+"\" flood-opacity=\""+Math.Round(input.Color.A/255.0,4)+"\" />" + Environment.NewLine;
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
