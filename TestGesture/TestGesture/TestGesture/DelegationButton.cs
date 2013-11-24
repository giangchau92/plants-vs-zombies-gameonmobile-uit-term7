using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SSCEngine.Control;
using SSCEngine.GestureHandling;
using SSCEngine.GestureHandling.Implements.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGesture
{
    public interface IDelegateButtonTargetFactory
    {
        IGestureTarget<FreeTap> CreateDelegateTarget();
    }

    public class DelegationButton : Button
    {
        private IDelegateButtonTargetFactory delegateFactory;
        private IGestureTarget<FreeTap> delegateTarget;

        private bool isDelegating;
        public bool IsDelegating
        {
            get { return this.isDelegating; }
            set { this.isDelegating = value; }
        }
        public DelegationButton(Game game, SpriteBatch sprBatch, IDelegateButtonTargetFactory delFactory)
            : base(game, sprBatch)
        {
            this.delegateFactory = delFactory;
        }

        public override bool ReceivedGesture(FreeTap gEvent)
        {
            if (IsDelegating)
            {
                return this.delegateTarget.ReceivedGesture(gEvent);
            }
            else
            {
                return base.ReceivedGesture(gEvent);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
