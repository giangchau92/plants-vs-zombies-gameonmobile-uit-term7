using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Test.Objects;

namespace Test.Component
{
    public interface IDrawComponent
    {
        void Update(GameObject owner, GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
