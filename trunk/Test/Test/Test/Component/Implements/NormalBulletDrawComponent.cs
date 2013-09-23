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
    public class NormalBulletDrawComponent : IDrawComponent
    {
        private NormalBullet _owner = null;
        private Texture2D textureBullet;

        public NormalBulletDrawComponent()
        {
            textureBullet = SCSServices.GetInstance().Content.Load<Texture2D>("bullet");
        }

        public void Update(GameObject owner, GameTime gameTime)
        {
            if (owner as NormalBullet == null)
                throw new Exception("ower is not Bullet");

            _owner = owner as NormalBullet;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_owner == null)
                throw new NullReferenceException();

            spriteBatch.Draw(textureBullet, _owner.Position, Color.White);
        }
    }
}
