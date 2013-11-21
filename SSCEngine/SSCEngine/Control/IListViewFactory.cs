using SSCEngine.Control.Clipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.Control
{
    public interface IListViewFactory
    {
        IListViewArrange CreateArranger();
        RectangleClipping CreateClipper();
        IListViewGestureHandler CreateGesturer();
    }

    public abstract class BaseListViewFactory : IListViewFactory
    {
        public virtual RectangleClipping CreateClipper()
        {
            return new RectangleClipping();
        }

        public abstract IListViewArrange CreateArranger();

        public abstract IListViewGestureHandler CreateGesturer();
    }

    public class VerticalListViewFactory : BaseListViewFactory
    {
        private float cellWidth, cellPad, decel;

        public VerticalListViewFactory(float cellWidth, float cellPadding, float decelRatio)
        {
            this.cellWidth = cellWidth;
            this.cellPad = cellPadding;
            this.decel = decelRatio;
        }

        public override IListViewArrange CreateArranger()
        {
            return new VerticalListViewArrange(cellWidth, cellPad);
        }

        public override IListViewGestureHandler CreateGesturer()
        {
            return new VerticalListViewGestureHandler(decel);
        }
    }

    public class HorizontalListViewFactory : BaseListViewFactory
    {
        private float cellWidth, cellPad, decel, maxBounces;

        public HorizontalListViewFactory(float cellWidth, float cellPadding, float decelRatio, float maxBounces)
        {
            this.cellWidth = cellWidth;
            this.cellPad = cellPadding;
            this.decel = decelRatio;
            this.maxBounces = maxBounces;
        }

        public override IListViewArrange CreateArranger()
        {
            return new HorizontalListViewArrange(cellWidth, cellPad);
        }

        public override IListViewGestureHandler CreateGesturer()
        {
            return new HorizontalListViewGestureHandler(decel, maxBounces);
        }
    }
}
