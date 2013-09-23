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
    public class ZombieDrawComponent : IDrawComponent
    {
        private Zombie _owner = null;
        private Texture2D textureRunning;
        private Texture2D textureStanding;
        private Texture2D textureDying;

        public ZombieDrawComponent()
        {
            textureStanding = SCSServices.GetInstance().Content.Load<Texture2D>("zombie_stand");
            textureRunning = SCSServices.GetInstance().Content.Load<Texture2D>("zombie_run");
            textureDying = SCSServices.GetInstance().Content.Load<Texture2D>("zombie_die");
        }

        public void Update(GameObject owner, GameTime gameTime)
        {
            if (owner as Zombie == null)
                throw new Exception("ower is not Zombie");

            _owner = owner as Zombie;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_owner == null)
                throw new NullReferenceException();

            if (_owner.State == ZombieState.RUNNING)
                spriteBatch.Draw(textureRunning, _owner.Position, Color.White);
            else if (_owner.State == ZombieState.DYING)
                spriteBatch.Draw(textureDying, _owner.Position, Color.White);
            else if (_owner.State == ZombieState.STANDING)
                spriteBatch.Draw(textureStanding, _owner.Position, Color.White);
        }
    }
}
