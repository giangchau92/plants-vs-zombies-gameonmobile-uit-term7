using Microsoft.Xna.Framework;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SCSEngine.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Utils.Mathematics;
using PlantsVsZombies.GameCore;

namespace PlantsVsZombies.GrowSystem
{
    public class PvZGrowList : BaseUIControl
    {
        private UIControlManager uiManager;

        private SpritePlayer spritePlayer;
        public ISprite Background { get; set; }

        private List<PvZGrowButton> growButtons = new List<PvZGrowButton>();
        private IPvZGameCurrency currencySystem;

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

        public PvZGrowList(Game game, float elemWidth, float elemPadding, UIControlManager uiManager, IPvZGameCurrency currSys)
            : base(game)
        {
            this.ElementWidth = elemWidth;
            this.spritePlayer = ((SCSServices)game.Services.GetService(typeof(SCSServices))).SpritePlayer;
            this.uiManager = uiManager;
            this.currencySystem = currSys;
        }

        public void AddGrowButton(PvZGrowButton grButton)
        {
            this.ArrageButton(grButton, this.growButtons.Count);
            grButton.OnTouchLeave += this.OnGrowButtonTouchLeaved;

            this.growButtons.Add(grButton);
            this.uiManager.AddTarget<FreeTap>(grButton);
        }

        private void ReArrange()
        {
            for (int i = 0; i < this.growButtons.Count; ++i)
            {
                this.ArrageButton(this.growButtons[i], i);
            }

            this.Canvas.Content.Size.X = (elemWidth + elemPad) * this.growButtons.Count;
        }

        private void ArrageButton(PvZGrowButton grButton, int i)
        {
            grButton.Canvas.Bound.Position.X = this.Canvas.Bound.Position.X + this.Canvas.Content.Position.X + (this.elemWidth + this.ElemPad) * i;
            grButton.Canvas.Bound.Position.Y = this.Canvas.Bound.Position.Y + this.Canvas.Content.Position.Y;
            grButton.Canvas.Bound.Size.X = this.elemWidth;
            grButton.Canvas.Bound.Size.Y = this.Canvas.Content.Size.Y;
        }

        public override void RegisterGestures(IGestureDispatcher dispatcher)
        {
        }

        public override void LeaveGestures(IGestureDispatcher dispatcher)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var grButton in this.growButtons)
            {
                if (!grButton.Enabled && grButton.Price <= this.currencySystem.CurrentMoney)
                {
                    grButton.Enabled = true;
                }
                else if (grButton.Enabled && grButton.Price > this.currencySystem.CurrentMoney)
                {
                    grButton.Enabled = false;
                }

                grButton.Update(gameTime);
            }

            for (int i = 0; i < this.growButtons.Count; )
            {
                if (this.growButtons[i].IsUICompleted)
                {
                    this.uiManager.RemoveTarget<FreeTap>(growButtons[i]);
                    this.growButtons.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
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
            CRectangleF contentInBound = new CRectangleF(this.Canvas.Content);
            contentInBound.Position += this.Canvas.Bound.Position;
            if (!contentInBound.Contains(leaveGesture.Current))
            {
                //create plant-shadow
                var shadow = button.ShadowFactory.CreatePlantShadow();
                shadow.CreatorButton = button;
                shadow.Canvas.Bound.Position = leaveGesture.Current;
                shadow.Canvas.Bound.Size = new Vector2(shadow.PlanShadowImage.CurrentFrame.Width,
                    shadow.PlanShadowImage.CurrentFrame.Height);

                // add p-s to ui manager (g-dispatcher)
                this.uiManager.Add(shadow);
                this.uiManager.SetHandleTarget<FreeTap>(leaveGesture, shadow);
            }
        }
    }
}