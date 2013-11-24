using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SCSEngine.Control.Clipping;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IListViewItem = SCSEngine.Control.IUIControl;

namespace SCSEngine.Control
{
    public class ListView : BaseUIControl, IGestureTarget<FreeTap>
    {
        private List<IListViewItem> controls = new List<IListViewItem>();
        public IEnumerable<IListViewItem> Components
        {
            get { return controls; }
        }

        public Texture2D Background { get; set; }
        protected IListViewArrange Arranger { get; set; }
        protected RectangleClipping Clipper { get; set; }
        protected IListViewGestureHandler Gesturer { get; set; }
        public CRectangleF Field { get; set; }

        public ListView(Game game, IListViewFactory lf)
            : base(game)
        {
            Background = null;
            this.Arranger = lf.CreateArranger();
            this.Clipper = lf.CreateClipper();
            this.Gesturer = lf.CreateGesturer();
            this.Field = CRectangleF.Empty;
        }

        public override void Update(GameTime gameTime)
        {
            this.Gesturer.UpdateOffset(this.Field, this.Canvas.Content);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            CRectangleF tmpOffset = this.Clipper.OffsetClip(this.Canvas.Content, this.Field);
            Vector2 translate = this.Field.Position + this.Canvas.Bound.Position + this.Canvas.Content.Position;
            ICanvas oldItemFrame = new BaseCanvas();
            for (int i = 0; i < controls.Count; ++i)
            {
                var item = controls[i];
                item.Canvas.Bound.Position += translate;
                item.Draw(gameTime);
                item.Canvas.Bound.Position -= translate;
            }

            base.Draw(gameTime);
        }

        public void AddItem(IListViewItem item)
        {
            this.controls.Add(item);
            this.listViewOnItemChanged();
        }

        public IListViewItem GetItemAt(int index)
        {
            return this.controls[index];
        }

        public void RemoveItemAt(int index)
        {
            this.controls.RemoveAt(index);
            this.listViewOnItemChanged();
        }

        public void RemoveItem(IListViewItem item)
        {
            this.controls.Remove(item);
            this.listViewOnItemChanged();
        }

        private void listViewOnItemChanged()
        {
            this.Arranger.GetContentSize(ref this.Field.Size, this.controls.Count);
            for (int i = 0; i < this.controls.Count; ++i)
            {
                this.Arranger.Arrange(this.controls[i].Canvas, i);
            }
        }

        public bool ReceivedGesture(FreeTap gEvent)
        {
            switch (gEvent.Touch.SystemTouch.State)
            {
                case TouchLocationState.Pressed:
                    this.Gesturer.Move(Vector2.Zero);
                    break;
                case TouchLocationState.Released:
                    this.Gesturer.Release(gEvent.Touch.Positions.Delta);
                    return false;
                case TouchLocationState.Moved:
                    this.Gesturer.Move(gEvent.Touch.Positions.Delta);
                    break;
            }

            return true;
        }

        public bool IsHandleGesture(FreeTap gEvent)
        {
            return this.Canvas.Bound.Contains(gEvent.Touch.Positions.Begin);
        }

        public uint Priority
        {
            get { return 0; }
        }

        public bool IsGestureCompleted { get; private set; }

        public override void RegisterGestures(IGestureDispatcher dispatcher)
        {
            dispatcher.AddTarget<FreeTap>(this);
        }

        public override void LeaveGestures(IGestureDispatcher dispatcher)
        {
            this.IsGestureCompleted = true;
        }
    }
}
