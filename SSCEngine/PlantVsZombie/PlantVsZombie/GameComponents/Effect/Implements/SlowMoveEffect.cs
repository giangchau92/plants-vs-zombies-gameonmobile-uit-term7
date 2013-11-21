using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlantVsZombie.GameComponents.Behaviors;
using PlantVsZombie.GameComponents.Components;
using PlantVsZombie.GameComponents.Behaviors.Implements;

namespace PlantVsZombie.GameComponents.Effect.Implements
{
    public class SlowMoveEffect : IEffect
    {
        TimeSpan curentTime = TimeSpan.Zero;
        public TimeSpan TimeDurring { get; set; }

        Vector2 slowVelocity = new Vector2(50, 0);

        public SlowMoveEffect()
        {
            TimeDurring = TimeSpan.Zero;
        }

        public void Update(GameTime gameTime)
        {
            if (curentTime >= TimeDurring)
            {
                // Remove itseft
                this.Owner.RemoveEffect(this);
            }
            else
            {
                // Take effect
                MoveComponent moveCOm = this.Owner.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
                if (moveCOm == null)
                    throw new Exception("SlowMoveEffect: Expect Target Move Component");

                MoveBehavior moveBehavior = (moveCOm.GetCurrentBehavior() as MoveBehavior);
                if (Math.Abs(moveBehavior.Velocity.X) - slowVelocity.X > 0)
                {
                    if (moveBehavior.Velocity.X < 0)
                        moveBehavior.VelocityAdd = new Vector2(slowVelocity.X, -slowVelocity.Y);
                    else
                        moveBehavior.VelocityAdd = new Vector2(-slowVelocity.X, -slowVelocity.Y);
                }
                curentTime += gameTime.ElapsedGameTime;
            }
        }


        private BaseLogicBehavior _owner = null;
        public BaseLogicBehavior Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }
    }
}
