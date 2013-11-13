using SSCEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlantVsZombie.GameComponents.Behaviors
{
    public class BaseLogicBehavior : BaseBehavior
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public virtual void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
        }
    }
}
