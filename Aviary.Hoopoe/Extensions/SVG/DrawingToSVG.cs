using System;
using System.Collections.Generic;

using Rg = Rhino.Geometry;

using Wg = Aviary.Wind.Graphics;
using Hp = Aviary.Hoopoe;

namespace Aviary.Hoopoe
{
    public static class DrawingToSVG
    {
        #region To Geometry Drawing

        public static string ToSVG(this Hp.Drawing input)
        {
            double scale = input.GetScale();

            string drawing = "<svg width=\"" + input.Width + "\" height=\"" + input.Height + "\" shape-rendering=\"geometricPrecision\" xmlns=\"http://www.w3.org/2000/svg\" >" + Environment.NewLine;
            
            double shiftW = (input.Width / scale/2 - input.Frame.Center.X) ;
            double shiftH = -(input.Height / scale/2 + input.Frame.Center.Y);

            drawing += "<g class=\"Canvas\" id=\"Canvas\" transform=\"scale(" + scale + "," + (-1) * scale + ") translate(" + shiftW + "," + shiftH + ") \">" + Environment.NewLine;

            Dictionary<string,Wg.Effects> effects = new Dictionary<string, Wg.Effects>();
            Dictionary<string, Wg.Graphic> graphics = new Dictionary<string, Wg.Graphic>();

            foreach (Shape shape in input.Shapes)
            {
                drawing += shape.ToPath();
                if (!graphics.ContainsKey(shape.Graphic.ID)) graphics.Add(shape.Graphic.ID, shape.Graphic);

                if (shape.Graphic.Effects.HasEffects)
                {
                    if (!effects.ContainsKey(shape.Graphic.Effects.ID))
                    {
                        effects.Add(shape.Graphic.Effects.ID, shape.Graphic.Effects);
                    }
                }
            }

            drawing += "</g >" + Environment.NewLine;

            drawing += "<defs>" + Environment.NewLine;
            drawing += "<clipPath id=\"Frame\"> <rect x=\"0\" y=\"0\" width=\"" + input.Width + "\" height=\"" + input.Height + "\" /> </clipPath>" + Environment.NewLine;

            foreach (Wg.Effects effect in effects.Values)
            {
                drawing += effect.ToSVG();
            }

            foreach(Wg.Graphic graphic in graphics.Values)
            {
                drawing += graphic.ToSVG();
            }

            drawing += "</defs>" + Environment.NewLine;
            drawing += "</svg >";

            return drawing;
        }

        public static string ToPath(this Shape input)
        {

            string paths = "<path d= \"";

            foreach (Geometry geo in input.Geometries)
            {
                string path = geo.ToSVG();
                paths += (Environment.NewLine + path);
            }

            paths += "\"" + Environment.NewLine + "class=\"cls-" + input.Graphic.Stroke.ID;
            if (input.Graphic.Fill.FillType == Wg.Fill.FillTypes.Solid) { paths += " cls-" + input.Graphic.Fill.ID; }
            paths += "\"" + Environment.NewLine;
            if (input.Graphic.Effects.HasEffects) paths += "filter = \"url(#" + input.Graphic.Effects.ID + ")\"" + Environment.NewLine;
            if((input.Graphic.Fill.FillType == Wg.Fill.FillTypes.LinearGradient)||(input.Graphic.Fill.FillType == Wg.Fill.FillTypes.RadialGradient))
            {
                paths+="fill=\"url(#" + input.Graphic.Fill.ID + ")\"";
            }
            paths += " />" + Environment.NewLine;

            return paths;
        }

        public static string ToSVG(this Hp.Geometry input)
        {
            string path = string.Empty;
            switch (input.CurveType)
            {
                case Geometry.CurveTypes.Arc:
                    Rg.Arc arc = new Rg.Arc();
                    input.Curve.TryGetArc(out arc);
                    path = arc.ToSVG();
                    break;
                case Geometry.CurveTypes.Circle:
                    Rg.Circle circle = new Rg.Circle();
                    input.Curve.TryGetCircle(out circle);
                    path = circle.ToSVG();
                    break;
                case Geometry.CurveTypes.Ellipse:
                    Rg.Ellipse ellipse = new Rg.Ellipse();
                    input.Curve.TryGetEllipse(out ellipse);
                    path = ellipse.ToSVG();
                    break;
                case Geometry.CurveTypes.Line:
                    Rg.Line line = new Rg.Line(input.Curve.PointAtStart, input.Curve.PointAtEnd);
                    path = line.ToSVG();
                    break;
                case Geometry.CurveTypes.Polyline:
                    Rg.Polyline polyline = new Rg.Polyline();
                    input.Curve.TryGetPolyline(out polyline);
                    path = polyline.ToSVG();
                    break;
                case Geometry.CurveTypes.Rectangle:
                    Rg.Polyline pline = new Rg.Polyline();
                    input.Curve.TryGetPolyline(out pline);
                    path = pline.ToSVG();
                    break;
                default:
                    path = input.Curve.ToSVG();
                    break;
            }

            return path;
        }

        #endregion

    }
}
