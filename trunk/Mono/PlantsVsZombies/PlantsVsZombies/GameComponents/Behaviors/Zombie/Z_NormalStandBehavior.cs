using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlantsVsZombies.GameComponents.Behaviors;
using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Components;
using SCSEngine.Utils.GameObject.Component;

namespace PlantsVsZombies.GameComponents.Behaviors.Zombie
{
    public class Z_NormalStandBehavior : BaseBehavior
    {
        private Vector2 vel = new Vector2(0, 0);
        public override void Update(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            MoveComponent moveCom = this.Owner as MoveComponent;
            if (moveCom == null)
                throw new Exception("MormalRunBehavior: Owner must be MoveComponent!");

            moveCom.Velocity = vel;
            //moveCom.UpdatePosition(gameTime); // no need to recalculator
            base.Update(message, gameTime);
        }
    }
}
