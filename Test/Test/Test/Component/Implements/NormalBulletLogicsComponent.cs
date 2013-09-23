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
    public class NormalBulletLogicsComponent : ILogicsComponent
    {
        public void Update(Objects.GameObject owner, GameTime gameTime)
        {
            if (owner as NormalBullet == null)
                throw new Exception("owner is not Bullet");

            NormalBullet bullet = owner as NormalBullet;
            owner.Position += new Vector2((float)(owner.Velocity.X * gameTime.ElapsedGameTime.TotalSeconds),
                (float)(owner.Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds));

            // Check with zombie
            List<GameObject> gameObjects = SCSServices.GetInstance().Game.GameWorld;
            foreach (var item in gameObjects)
            {
                if (item == bullet)
                    continue;

                if (item.Bound.Intersects(bullet.Bound) && (item as Zombie) != null)
                {
                    (item as Zombie).Heath -= 10;
                    bullet.Alive = false;
                }
            }
        }
    }
}
