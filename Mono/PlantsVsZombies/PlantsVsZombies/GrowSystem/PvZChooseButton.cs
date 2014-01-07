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
using System.Threading.Tasks;

namespace PlantsVsZombies.GrowSystem
{
    public delegate void ChooseButtonEventHandler(PvZChooseButton button);

    public class PvZChooseButton : BaseUIControl, IGestureTarget<Tap>
    {
        public bool IsDisabled { get; set; }
        public String Name { get; set; }

        private SpritePlayer spritePlayer;
        public ISprite Background { get; set; }

        public event ChooseButtonEventHandler OnTap;

        public PvZChooseButton(Game game)
            : base(game)
        {
            this.IsDisabled = false;
            this.spritePlayer = SCSServices.Instance.SpritePlayer;
        }

        public override void RegisterGestures(SCSEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
            dispatcher.AddTarget<Tap>(this);
        }

        public override void LeaveGestures(SCSEngine.GestureHandling.IGestureDispatcher dispatcher)
        {
            dispatcher.RemoveTarget<Tap>(this);
        }

        public bool ReceivedGesture(Tap gEvent)
        {
            if (this.OnTap != null)
            {
                this.OnTap(this);
            }

            return true;
        }

        public bool IsHandleGesture(Tap gEvent)
        {
            return !this.IsDisabled && this.Canvas.Bound.Contains(gEvent.Touch.Positions.Current);
        }

        public uint Priority
        {
            get { return 0; }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.Background != null)
            {
                Color color = (this.IsDisabled) ? Color.Gray : Color.White;
                this.spritePlayer.Draw(this.Background, this.Canvas.Bound.Rectangle, color);
            }

            base.Draw(gameTime);
        }

        public void KillMe()
        {
            this.IsUICompleted = true;
        }
    }
}
