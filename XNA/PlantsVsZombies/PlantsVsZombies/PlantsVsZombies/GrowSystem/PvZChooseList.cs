using Microsoft.Xna.Framework;
using SCSEngine.Control;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GrowSystem
{
    public delegate void ChooseListEventHandler(PvZChooseList list, PvZChooseButton button);

    public class PvZChooseList : BaseUIControl
    {
        private UIControlManager uiManager;
        private int maxButtons, nBufferedButton;

        public int MaxButtons
        {
            get { return maxButtons; }
            set { maxButtons = value; }
        }

        public int NumberOfBufferedButtons
        {
            get { return nBufferedButton; }
            set { nBufferedButton = value; }
        }

        private SpritePlayer spritePlayer;
        public ISprite Background { get; set; }

        private List<PvZChooseButton> chooseButtons = new List<PvZChooseButton>();

        public event ChooseListEventHandler OnButtonMoveCompleted;

        public List<PvZChooseButton> ChooseButtons
        {
            get { return chooseButtons; }
        }

        private float elemWidth, elemHeight, elemPad; 

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

        public PvZChooseList(Game game, float elemWidth, float elemHeight, float elemPadding, UIControlManager uiManager)
            : base(game)
        {
            this.elemWidth = elemWidth;
            this.elemHeight = elemHeight;
            this.spritePlayer = SCSServices.Instance.SpritePlayer;
            this.uiManager = uiManager;
            this.maxButtons = int.MaxValue;
            this.nBufferedButton = 0;
        }

        public void AddChooseButton(PvZChooseButton chButton)
        {
            this.ArrageButton(chButton, this.chooseButtons.Count);

            this.chooseButtons.Add(chButton);
            this.uiManager.AddTarget<Tap>(chButton);
        }

        public void RemoveChooseButton(PvZChooseButton chButton)
        {
            if (this.chooseButtons.Contains(chButton))
            {
                chButton.KillMe();
            }
        }

        public PvZChooseButton GetButton(String name)
        {
            foreach (var chButton in this.chooseButtons)
            {
                if (chButton.Name == name)
                {
                    return chButton;
                }
            }

            return null;
        }

        public void ReArrange()
        {
            for (int i = 0; i < this.chooseButtons.Count; ++i)
            {
                this.ArrageButton(this.chooseButtons[i], i);
            }
        }

        private void ArrageButton(PvZChooseButton grButton, int i)
        {
            int nCols = (int) (this.Canvas.Content.Width / (this.elemWidth + this.elemPad));
            int j = i / nCols;
            i %= nCols;

            grButton.Canvas.Bound.Position.X = this.Canvas.Bound.Position.X + this.Canvas.Content.Position.X + (this.elemWidth + this.ElemPad) * i;
            grButton.Canvas.Bound.Position.Y = this.Canvas.Bound.Position.Y + this.Canvas.Content.Position.Y + (this.elemHeight + this.ElemPad) * j;
            grButton.Canvas.Bound.Size.X = this.elemWidth;
            grButton.Canvas.Bound.Size.Y = this.elemHeight;
        }

        public override void RegisterGestures(IGestureDispatcher dispatcher)
        {
        }

        public override void LeaveGestures(IGestureDispatcher dispatcher)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in this.chooseButtons)
            {
                button.Update(gameTime);
            }

            bool isHasChanges = false;
            for (int i = 0; i < this.chooseButtons.Count;)
            {
                if (this.chooseButtons[i].IsUICompleted)
                {
                    this.uiManager.RemoveTarget<Tap>(chooseButtons[i]);
                    this.chooseButtons.RemoveAt(i);
                    isHasChanges = true;
                }
                else
                {
                    ++i;
                }
            }

            if (isHasChanges)
            {
                this.ReArrange();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.spritePlayer != null && this.Background != null)
            {
                this.spritePlayer.Draw(this.Background, this.Canvas.Bound.Rectangle, Color.White);
            }

            foreach (var button in this.chooseButtons)
            {
                button.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public RectangleF MakeDestination()
        {
            if (this.nBufferedButton < this.maxButtons)
            {
                int i = this.nBufferedButton;
                // if i < max
                int nCols = (int)(this.Canvas.Content.Width / (this.elemWidth + this.elemPad));
                int j = i / nCols;
                i %= nCols;
                RectangleF destRect;

                destRect.X = this.Canvas.Bound.Position.X + this.Canvas.Content.Position.X + (this.elemWidth + this.ElemPad) * i;
                destRect.Y = this.Canvas.Bound.Position.Y + this.Canvas.Content.Position.Y + (this.elemHeight + this.ElemPad) * j;
                destRect.Width = this.elemWidth;
                destRect.Height = this.elemHeight;

                return destRect;
            }

            return RectangleF.Empty;
        }

        public void OnPvZChooseButtonMoveComplete(PvZChooseButton chooseButton)
        {
            if (this.OnButtonMoveCompleted != null)
            {
                this.OnButtonMoveCompleted(this, chooseButton);
            }
        }


        public UIControlManager UIManager
        {
            get { return this.uiManager; }
        }

        public void RemoveAll()
        {
            foreach (var button in this.chooseButtons)
            {
                this.uiManager.RemoveTarget<Tap>(button);
            }
        }
    }
}
