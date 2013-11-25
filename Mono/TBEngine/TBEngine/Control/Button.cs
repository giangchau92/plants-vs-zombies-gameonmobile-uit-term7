using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SCSEngine.Control
{
    public delegate void ButtonEventHandler(Button button);

    public class Button : BaseUIControl, IGestureTarget<FreeTap>
    {
        #region Draw
        public Texture2D NormalImage { get; set; }
        public Texture2D HoldImage { get; set; }

        public bool IsOverlay = false;
        #endregion

        #region ButtonDrawState
        public enum ButtonState { Normal, Hold };
        public ButtonState State { get; private set; }
        #endregion

        #region Events
        event ButtonEventHandler OnPressed;
        event ButtonEventHandler OnTouched; //Released
        event ButtonEventHandler OnHold;

        protected void uiOnPressed()
        {
            if (this.OnPressed != null)
            {
                this.OnPressed(this);
            }
        }
        protected void uiOnTouched()
        {
            if (this.OnTouched != null)
            {
                this.OnTouched(this);
            }
        }
        protected void uiOnHold()
        {
            if (this.OnHold != null)
            {
                this.OnHold(this);
            }
        }
        protected void uiOnOutSide()
        {
        }
        #endregion

        public override void RegisterGestures(IGestureDispatcher dispatcher)
        {
            dispatcher.AddTarget<FreeTap>(this);
        }

        public override void LeaveGestures(IGestureDispatcher dispatcher)
        {
            this.IsGestureCompleted = true;
        }

        public virtual bool ReceivedGesture(FreeTap gEvent)
        {
            if (this.Canvas.Bound.Contains(gEvent.Current) && (gEvent.Touch.SystemTouch.State != TouchLocationState.Released))
            {
                if (this.State == ButtonState.Normal)
                {
                    this.uiOnPressed();
                }
                else
                {
                    this.uiOnHold();
                }

                this.State = ButtonState.Hold;
            }
            else
            {
                this.State = ButtonState.Normal;
                this.uiOnOutSide();
            }

            if (gEvent.Touch.SystemTouch.State == TouchLocationState.Released)
            {
                this.uiOnTouched();
                return false;
            }

            return true;
        }

        public virtual bool IsHandleGesture(FreeTap gEvent)
        {
            return ((gEvent.Touch.SystemTouch.State == TouchLocationState.Pressed) && this.Canvas.Bound.Contains(gEvent.Current));
        }

        public uint Priority
        {
            get;
            set;
        }

        public bool IsGestureCompleted { get; private set; }

        private SpriteBatch sprBatch;

        public override void Draw(GameTime gameTime)
        {
            if (null == sprBatch)
                return;

            sprBatch.Begin();
            if ((this.NormalImage != null) && (this.IsOverlay || this.State == ButtonState.Normal))
            {
                sprBatch.Draw(this.NormalImage, this.Canvas.Bound.Rectangle, this.Canvas.Content.Rectangle, Color.White);
            }

            if ((this.HoldImage != null) && (this.State == ButtonState.Hold))
            {
                sprBatch.Draw(this.HoldImage, this.Canvas.Bound.Rectangle, this.Canvas.Content.Rectangle, Color.White);
            }
            sprBatch.End();

            base.Draw(gameTime);
        }

        public void FitSizeByImage()
        {
            Texture2D img = (this.NormalImage != null) ? (this.NormalImage) : (this.HoldImage != null) ? (this.HoldImage) : (null);
            if (img == null)
                return;

            this.Canvas.Content.Position = Vector2.Zero;
            this.Canvas.Content.Size = new Vector2(img.Width, img.Height);
            
            this.Canvas.Bound.Size = this.Canvas.Content.Size;
        }


        public Button(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.sprBatch = spriteBatch;
            this.Priority = 0;
        }

        public Button(Game game, SpriteBatch spriteBatch,Texture2D normal, Texture2D hold)
            : base(game)
        {
            this.sprBatch = spriteBatch;
            this.NormalImage = normal;
            this.HoldImage = hold;
            this.Priority = 0;

            this.FitSizeByImage();
        }
    }
}
