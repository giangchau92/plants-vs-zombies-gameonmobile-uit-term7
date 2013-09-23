using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Test.Component.Implements;


namespace Test.Objects
{
    class NormalBullet : GameObject
    {
        public NormalBullet()
            :base(new NormalBulletLogicsComponent(), new NormalBulletDrawComponent())
        {
            Frame = new Rectangle(0, 0, 10, 10);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
