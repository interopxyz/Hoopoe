using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg = Rhino.Geometry;


namespace Hoopoe
{
    public class Geometry
    {

        #region members

        public enum CurveTypes { None, Spline, Polyline, Rectangle, Line, Arc, Circle, Ellipse }
        protected CurveTypes curveType = CurveTypes.None;
        protected Rg.Curve curve = null;

        #endregion

        #region constructors

        public Geometry()
        {

        }

        public Geometry(Rg.Curve curve)
        {
            SetCurveType(curve);
            this.curve = curve;
        }

        public Geometry(Rg.Circle circle)
        {
            curveType = CurveTypes.Circle;
            curve = circle.ToNurbsCurve();
        }

        public Geometry(Rg.Arc arc)
        {
            curveType = CurveTypes.Arc;
            curve = arc.ToNurbsCurve();
        }

        public Geometry(Rg.Ellipse ellipse)
        {
            curveType = CurveTypes.Ellipse;
            curve = ellipse.ToNurbsCurve();
        }

        public Geometry(Rg.Line line)
        {
            curveType = CurveTypes.Line;
            curve = line.ToNurbsCurve();
        }

        public Geometry(Rg.Rectangle3d rectangle3D)
        {
            curveType = CurveTypes.Rectangle;
            curve = rectangle3D.ToNurbsCurve();
        }

        public Geometry(Rg.Polyline polyline)
        {
            curveType = CurveTypes.Polyline;
            curve = polyline.ToNurbsCurve();
        }

        #endregion

        #region properties

        public Rg.Curve Curve
        {
            get { return curve; }
            set { SetCurveType(value); }
        }

        public CurveTypes CurveType
        {
            get { return curveType; }
        }

        #endregion

        #region methods

        private void SetCurveType(Rg.Curve curve)
        {
            Rg.Circle R = new Rg.Circle();
            Rg.Arc A = new Rg.Arc();
            Rg.Ellipse S = new Rg.Ellipse();
            Rg.Polyline P = new Rg.Polyline();

            if (curve.TryGetCircle(out R))
            {
                curveType = CurveTypes.Circle;
            }
            else if (curve.TryGetArc(out A))
            {
                curveType = CurveTypes.Arc;
            }
            else if (curve.TryGetEllipse(out S))
            {
                curveType = CurveTypes.Ellipse;
            }
            else if (curve.IsLinear())
            {
                curveType = CurveTypes.Line;
            }
            else if (curve.TryGetPolyline(out P))
            {
                curveType = CurveTypes.Polyline;
            }
            else
            {
                curveType = CurveTypes.Spline;
            }

        }

        #endregion

    }
}
