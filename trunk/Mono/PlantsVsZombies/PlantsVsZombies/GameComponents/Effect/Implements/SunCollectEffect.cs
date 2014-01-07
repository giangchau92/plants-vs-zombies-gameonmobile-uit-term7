using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Behaviors;
using PlantsVsZombies.GameComponents.Behaviors.Implements;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GrowSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameComponents.Effect.Implements
{
    class SunCollectEffect : IEffect
    {
        double timeDurring = 0.75; // TIME DURRING (s)
        TimeSpan curentTime = TimeSpan.Zero;
        public TimeSpan TimeDurring { get; set; }

        Vector2 v0 = new Vector2(0, 50);
        Vector2 a = new Vector2(0, 0);

        public SunCollectEffect(Vector2 pos)
        {
            Vector2 distance = PvZHardCurrency.CollectionPoint - pos;
            
            TimeDurring = TimeSpan.FromSeconds(timeDurring);
            v0 = new Vector2(distance.X/(float)timeDurring, distance.Y/(float)timeDurring);
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
                Debug.WriteLine("Remove SunCollectEffect");
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
