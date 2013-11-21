using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SCSEngine.Mathematics;
using SSCEngine.GestureHandling;
using SSCEngine.GestureHandling.Implements.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TestGesture
{
    public class DragObject : DrawableGameComponent, IGestureTarget<FreeTap>
    {
        private SpriteBatch sBatch;
        private Texture2D img;
        private Vector2 position = Vector2.Zero;
        private Vector2 touchPos;
        private Color color;

        public DragObject(Game game, SpriteBatch sprBatch, Texture2D image, Vector2 pos)
            : base(game)
        {
            this.sBatch = sprBatch;
            this.img = image;
            this.position = pos;
            color = GRandom.RandomSolidColor();
        }

        public override void Draw(GameTime gameTime)
        {
            this.sBatch.Begin();
            this.sBatch.Draw(this.img, position, null, color, 0f, new Vector2(this.img.Width / 2, this.img.Height / 2), 1f, SpriteEffects.None, 0f);
            this.sBatch.End();

            base.Draw(gameTime);
        }

        public bool ReceivedGesture(FreeTap gEvent)
        {
            if (gEvent.Touch.SystemTouch.State == TouchLocationState.Pressed)
            {
                touchPos = gEvent.Touch.Positions.Current - this.position;
            }
            else
            {
                this.position = gEvent.Touch.Positions.Current - touchPos;
            }

            if (gEvent.Touch.SystemTouch.State == TouchLocationState.Released)
            {
                color = GRandom.RandomSolidColor();
                return false;
            }

            return true;
        }

        public bool IsHandleGesture(FreeTap gEvent)
        {
            return (this.position.X <= gEvent.Touch.Positions.Current.X && this.position.Y <= gEvent.Touch.Positions.Current.Y &&
                this.position.X + this.img.Width >= gEvent.Touch.Positions.Current.X && this.position.Y + this.img.Height >= gEvent.Touch.Positions.Current.Y);
        }

        public uint Priority
        {
            get
            {
                return 0;
            }
        }
        public bool IsGestureCompleted
        {
            get { return false; }
        }
    }
}
