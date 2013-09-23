using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Component;
using Test.Objects;
using Microsoft.Xna.Framework;

namespace Test.Component.Implements
{
    public class PlantLogicsComponent : ILogicsComponent
    {
        TimeSpan shootDelayTime;

        public PlantLogicsComponent()
        {
            shootDelayTime = TimeSpan.Zero;
        }

        public void Update(GameObject owner, GameTime gameTime)
        {
            if (owner as Plant == null)
                throw new Exception("owner is not Plant");

            Plant plant = owner as Plant;
            List<GameObject> gameObjects = SCSServices.GetInstance().Game.GameWorld;
            bool isShoot = false;
            foreach (var item in gameObjects)
            {
                if (item == plant)
                    continue;
                if (item.Position.Y == plant.Position.Y && (plant.Position.X < item.Position.X))
                {
                    if (item as Zombie != null)
                    {
                        plant.State = PlantState.SHOOT;
                        isShoot = true;
                    }
                }
            }
            if (!isShoot)
                plant.State = PlantState.STAND;

            // Shoot
            if (plant.State == PlantState.SHOOT)
            {
                shootDelayTime += gameTime.ElapsedGameTime;

                if (shootDelayTime.TotalSeconds >= 1)
                {
                    NormalBullet bullet = new NormalBullet();
                    bullet.Position = new Vector2(plant.Position.X + 25, plant.Position.Y + 50);
                    bullet.Velocity = new Vector2(100, 0);
                    SCSServices.GetInstance().Game.GameWorld.Add(bullet);

                    shootDelayTime = TimeSpan.Zero;
                }
            }
        }
    }
}
