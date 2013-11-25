using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents;
using PlantVsZombies.GameComponents.Components;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombies.GameComponents.Behaviors.Implements
{
    public class MoveBehavior : BaseBehavior
    {
        public Vector2 Velocity { get; set; }
        public Vector2 VelocityAdd { get; set; }

        public MoveBehavior()
            : base()
        {
            VelocityAdd = Velocity = Vector2.Zero;
        }

        public override void Update(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            MoveComponent moveCom = this.Owner as MoveComponent;
            if (moveCom == null)
                throw new Exception("MormalRunBehavior: Owner must be MoveComponent!");

            moveCom.Velocity = Velocity + VelocityAdd;
            VelocityAdd = Vector2.Zero;
            moveCom.UpdatePosition(gameTime);
            base.Update(message, gameTime);
        }

        public static MoveBehavior CreateBehavior()
        {
            MoveBehavior result = new MoveBehavior();
            return result;
        }


        public override IBehavior<MessageType> Clone()
        {
            MoveBehavior moveBehavior = new MoveBehavior();
            moveBehavior.Velocity = this.Velocity;
            moveBehavior.VelocityAdd = this.VelocityAdd;
            return moveBehavior;
        }
    }
}
