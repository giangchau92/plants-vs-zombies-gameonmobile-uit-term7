using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Component;
using Test.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test.Component.Implements
{
    public class PlantDrawComponent : IDrawComponent
    {
        private Plant _plant = null;
        private Texture2D textureStanding;
        private Texture2D textureShooting;

        public PlantDrawComponent()
        {
            textureStanding = SCSServices.GetInstance().Content.Load<Texture2D>("plant_stand");
            textureShooting = SCSServices.GetInstance().Content.Load<Texture2D>("plant_shoot");
        }

        public void Update(Objects.GameObject owner, GameTime gameTime)
        {
            if (owner as Plant == null)
                throw new Exception("ower is not Plant");

            _plant = owner as Plant;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_plant == null)
                throw new NullReferenceException();

            if (_plant.State == PlantState.STAND)
                spriteBatch.Draw(textureStanding, _plant.Position, Color.White);
            else if (_plant.State == PlantState.SHOOT)
                spriteBatch.Draw(textureShooting, _plant.Position, Color.White);
        }
    }
}
