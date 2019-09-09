using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wind.Graphics;
using Rg = Rhino.Geometry;

namespace Hoopoe
{
    public class Shape
    {

        #region members

        protected bool isCompound = true;

        protected List<Geometry> geometries = new List<Geometry>();
        protected Graphic graphic = Graphics.StrokeBlack;

        #endregion

        #region constructors

        public Shape()
        {

        }

        public Shape(Rg.Curve curve)
        {
            if (curve.IsClosed) { graphic = Graphics.FillBlack; } else { graphic = Graphics.StrokeBlack; }
            this.Curves = new List<Rg.Curve>() { curve };
        }

        public Shape(Rg.Curve curve, Graphic graphic)
        {
            this.graphic = graphic;
            this.Curves = new List<Rg.Curve>() { curve };
        }

        public Shape(List<Rg.Curve> curves)
        {
            graphic = Graphics.StrokeBlack;
            this.Curves = curves;
        }

        public Shape(List<Rg.Curve> curves, Graphic graphic)
        {
            this.graphic = graphic;
            this.Curves = curves;
        }

        #endregion

        #region properties

        public virtual List<Geometry> Geometries
        {
            get { return geometries; }
        }

        public virtual List<Rg.Curve> Curves
        {
            get
            {
                List<Rg.Curve> curves = new List<Rg.Curve>();
                foreach (Geometry geometry in geometries) { curves.Add(geometry.Curve); }
                return curves;
            }
            set
            {
                List<Geometry> geos = new List<Geometry>();
                if (value.Count > 1)
                {
                    isCompound = true;
                    foreach (Rg.Curve crv in value)
                    {
                        geos.Add(new Geometry(crv));
                    }
                }
                else
                {
                    isCompound = false;
                    geos.Add(new Geometry(value[0]));
                }
                geometries = geos;
            }
        }

        public virtual Graphic Graphic
        {
            get { return graphic; }
            set { graphic = value; }
        }

        public virtual bool IsCompound
        {
            get { return isCompound; }
        }

        #endregion

        #region methods
        


        #endregion

        #region overrides

        public override string ToString()
        {
            string status = "Empty Shape";
            if (geometries.Count > 0) { if (isCompound) { status = "Compound Shape"; } else { status = geometries[0].CurveType.ToString() + " Shape"; } }
            return status;
        }

        #endregion

    }
}
