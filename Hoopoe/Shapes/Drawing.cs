using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wind.Graphics;
using Rh = Rhino.Geometry;

namespace Hoopoe
{
    public class Drawing
    {

        #region members

        public List<Shape> Shapes = new List<Shape>();

        protected Rh.Rectangle3d frame = new Rh.Rectangle3d();
        protected bool manualFrame = false;

        protected double width = 0;
        protected bool manualWidth = false;
        protected double height = 0;
        protected bool manualHeight = false;

        public Color Background = Colors.Transparent;

        #endregion

        #region constructors

        public Drawing()
        {

        }

        public Drawing(List<Shape> shapes)
        {
            this.Shapes = shapes;
            this.frame = GetBoundary();
        }

        public Drawing(List<Shape> shapes, Rh.Rectangle3d frame)
        {
            this.Shapes = shapes;
            this.frame = frame;
        }

        #endregion

        #region properties

        public virtual bool IsFrameOverriden
        {
            get { return manualFrame; }
            set
            {
                manualFrame = value;
            }
        }

        public virtual Rh.Rectangle3d Frame
        {
            get
            {
                if (manualFrame) { return frame; } else { return GetBoundary(); }
            }
            set
            {
                frame = value;
                manualFrame = true;
            }
        }

        public virtual double Width
        {
            get
            {
                if (manualWidth) { return width; } else { return Frame.Width; }
            }
            set
            {
                width = Math.Abs(value);
                manualWidth = true;
            }
        }

        public virtual double Height
        {
            get
            {
                if (manualHeight) { return height; } else { return Frame.Height; }
            }
            set
            {
                height = Math.Abs(value);
                manualHeight = true;
            }
        }

        #endregion

        #region methods

        public void WidthReset()
        {
            manualWidth = true;
        }

        public void HeightReset()
        {
            manualHeight = true;
        }

        public Rh.Rectangle3d GetBoundary()
        {
            List<Rh.Point3d> points = new List<Rh.Point3d>();
            foreach (Shape shape in this.Shapes)
            {
                foreach (Rh.Curve curve in shape.Curves)
                {
                    Rh.BoundingBox box = curve.GetBoundingBox(true);
                    points.Add(box.Min);
                    points.Add(box.Max);
                }
            }
            Rh.BoundingBox bbox = new Rh.BoundingBox(points);

            return new Rh.Rectangle3d(Rh.Plane.WorldXY, new Rh.Point3d(bbox.Min.X, bbox.Min.Y, 0), new Rh.Point3d(bbox.Max.X, bbox.Max.Y, 0));
        }

        public double GetScale()
        {
            double scale = 1.0;
            double scaleW = this.Width / this.Frame.Width;
            double scaleH = this.Height / this.Frame.Height;
            if (scaleW > scaleH) { scale = scaleH; } else { scale = scaleW; }
            return scale;
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Drawing (Shp:"+ Shapes.Count + " W:" + this.Width + " H:" + this.Height + ")";
        }

        #endregion

    }
}
