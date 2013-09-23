using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Test.Component;
using Test.Objects;

namespace Test.Component.Implements
{
    class ZombieLogicsComponent : ILogicsComponent
    {
        Vector2 velocityZombie = new Vector2(-50, 0);

        public ZombieLogicsComponent()
        {
        }

        public void Update(GameObject owner, GameTime gameTime)
        {
            if (owner as Zombie == null)
                throw new Exception("owner is not Zombie");

            Zombie zom = owner as Zombie;

            // Update velocity
            if (zom.State == ZombieState.STANDING || zom.State == ZombieState.DYING)
                owner.Velocity = Vector2.Zero;
            else if (zom.State == ZombieState.RUNNING)
                owner.Velocity = velocityZombie;

            owner.Position += new Vector2((float)(owner.Velocity.X * gameTime.ElapsedGameTime.TotalSeconds),
                (float)(owner.Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds));

            // Check with plant
            bool isEat = false;
            List<GameObject> gameObjects = SCSServices.GetInstance().Game.GameWorld;
            foreach (var item in gameObjects)
            {
                if (item == zom)
                    continue;

                if (item.Bound.Intersects(zom.Bound) && item as Plant != null)
                {
                    zom.State = ZombieState.STANDING;
                    (item as Plant).Heath--;
                    isEat = true;
                }
            }
            if (!isEat)
                zom.State = ZombieState.RUNNING;
        }
    }
}
