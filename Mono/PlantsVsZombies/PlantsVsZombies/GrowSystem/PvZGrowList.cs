using Microsoft.Xna.Framework;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SCSEngine.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GrowSystem
{
    public class PvZGrowList : BaseUIControl
    {
        private SpritePlayer spritePlayer;
        public ISprite Background { get; set; }

        private List<PvZGrowButton> growButtons = new List<PvZGrowButton>();
        public List<PvZGrowButton> GrowButtons
        {
            get { return growButtons; }
        }

        private float elemWidth, elemPad; 

        public float ElemPad
        {
            get { return elemPad; }
            set
            {
                if (value != this.elemPad)
                {
                    this.elemPad = value;
                    this.ReArrange();
                }
            }
        }

        public float ElementWidth
        {
            get { return elemWidth; }
            set
            {
                if (value != elemWidth)
                {
                    elemWidth = value;
                    this.ReArrange();
                }
            }
        }

        public PvZGrowList(Game game, float elemWidth, float elemPadding)
            : base(game)
        {
            this.ElementWidth = elemWidth;
            this.spritePlayer = ((SCSServices)game.Services.GetService(typeof(SCSServices))).SpritePlayer;
        }

        public void AddGrowButton(PvZGrowButton grButton)
        {
            this.ArrageButton(grButton, this.growButtons.Count);
            grButton.OnTouchLeave += this.OnGrowButtonTouchLeaved;

            this.growButtons.Add(grButton);
        }

        private void ReArrange()
        {
            for (int i = 0; i < this.growButtons.Count; ++i)
            {
                this.ArrageButton(this.growButtons[i], i);
            }
        }

        private void ArrageButton(PvZGrowButton grButton, int i)
        {
            grButton.Canvas.Bound.Position.X = this.Canvas.Bound.Position.X + this.Canvas.Content.Position.X + (this.elemWidth + this.ElemPad) * i;
            grButton.Canvas.Bound.Position.Y = this.Canvas.Bound.Position.Y + this.Canvas.Content.Position.Y;
            grButton.Canvas.Bound.Size.X = this.elemWidth;
            grButton.Canvas.Bound.Size.Y = this.Canvas.Content.Size.Y;
        }

        public override void RegisterGestures(SCSEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
        }

        public override void LeaveGestures(SCSEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var grButton in this.growButtons)
            {
                if (!grButton.Enabled && true /*button is avaiable*/)
                {
                    grButton.Enabled = true;
                }
                else if (grButton.Enabled && false /*button unavaiable*/)
                {
                    grButton.Enabled = false;
                }

                grButton.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.spritePlayer != null && this.Background != null)
            {
                this.spritePlayer.Draw(this.Background, this.Canvas.Bound.Rectangle, Color.White);
            }

            foreach (var grButton in this.growButtons)
            {
                if (this.Canvas.Bound.Contains(grButton.Canvas.Bound))
                {
                    grButton.Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }

        private void OnGrowButtonTouchLeaved(PvZGrowButton button, SCSEngine.GestureHandling.Implements.Events.FreeTap leaveGesture)
        {
            if (!this.Canvas.Bound.Contains(leaveGesture.Current))
            {
                //create plant-shadow
                // add p-s to ui manager (g-dispatcher)
            }
        }
    }
}