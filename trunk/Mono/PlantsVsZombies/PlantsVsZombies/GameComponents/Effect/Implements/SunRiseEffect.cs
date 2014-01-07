using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Behaviors;
using PlantsVsZombies.GameComponents.Behaviors.Implements;
using PlantsVsZombies.GameComponents.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameComponents.Effect.Implements
{
    public class SunRiseEffect : IEffect
    {
        TimeSpan curentTime = TimeSpan.Zero;
        public TimeSpan TimeDurring { get; set; }

        Vector2 v0 = new Vector2(0, -100);
        Vector2 a = new Vector2(0, 500);

        public SunRiseEffect()
        {
            TimeDurring = TimeSpan.FromSeconds(0.7f);
        }

        public void Update(GameTime gameTime)
        {
            if (curentTime >= TimeDurring)
            {
                // Remove itseft
                MoveComponent moveCOm = this.Owner.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
                if (moveCOm == null)
                    throw new Exception("SlowMoveEffect: Expect Target Move Component");

                MoveBehavior moveBehavior = (moveCOm.GetCurrentBehavior() as MoveBehavior);
                moveBehavior.Velocity = Vector2.Zero;

                this.Owner.RemoveEffect(this);
            }
            else
            {
                // Take effect
                MoveComponent moveCOm = this.Owner.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
                if (moveCOm == null)
                    throw new Exception("SlowMoveEffect: Expect Target Move Component");

                Vector2 v = v0 + new Vector2(a.X * (float)curentTime.TotalSeconds, a.Y * (float)curentTime.TotalSeconds);

                MoveBehavior moveBehavior = (moveCOm.GetCurrentBehavior() as MoveBehavior);
                moveBehavior.Velocity = v;
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
