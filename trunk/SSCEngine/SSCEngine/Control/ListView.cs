using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SSCEngine.Control.Clipping;
using SSCEngine.GestureHandling;
using SSCEngine.GestureHandling.Implements.Events;
using SSCEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IListViewItem = SSCEngine.Control.IUIControl;

namespace SSCEngine.Control
{
    public class ListView : BaseUIControl, IGestureTarget<FreeDrag>
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
            this.Gesturer.UpdateOffset(this.Field);

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
                oldItemFrame.Bound.Alter(item.Canvas.Bound);
                oldItemFrame.Content.Alter(item.Canvas.Content);
                item.Canvas.Bound.Position += translate;
                Debug.WriteLine(string.Format("Item pos:{0} and drawPos:{1}", oldItemFrame.Bound.Position, item.Canvas.Bound.Position));
                item.Draw(gameTime);
                item.Canvas.Bound.Alter(oldItemFrame.Bound);
                item.Canvas.Content.Alter(oldItemFrame.Content);
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

        public bool ReceivedGesture(FreeDrag gEvent)
        {
            switch (gEvent.Touch.SystemTouch.State)
            {
                case TouchLocationState.Released:
                    this.Gesturer.Release(gEvent.Touch.Positions.Delta);
                    return false;
                case TouchLocationState.Moved:
                    this.Gesturer.Move(gEvent.Touch.Positions.Delta);
                    Debug.WriteLine(string.Format("Current Drag Position: {0}", gEvent.Touch.Positions.Current));
                    break;
            }

            return true;
        }

        public bool IsHandleGesture(FreeDrag gEvent)
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
            dispatcher.AddTarget<FreeDrag>(this);
        }

        public override void LeaveGestures(IGestureDispatcher dispatcher)
        {
            this.IsGestureCompleted = true;
        }
    }
}
