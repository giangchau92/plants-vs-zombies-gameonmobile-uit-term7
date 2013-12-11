using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.Components;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombie.GameComponents.Behaviors.Bullet
{
    public class B_NormalFlyBehavior : BaseBehavior
    {
        private Vector2 vel = new Vector2(200, 0);
        public override void Update(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            MoveComponent moveCom = this.Owner as MoveComponent;
            if (moveCom == null)
                throw new Exception("MormalRunBehavior: Owner must be MoveComponent!");

            moveCom.Velocity = vel;
            moveCom.UpdatePosition(gameTime);
            base.Update(message, gameTime);
        }
    }
}
