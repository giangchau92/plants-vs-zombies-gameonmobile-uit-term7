using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Test.Objects;

namespace Test.Component
{
    public interface ILogicsComponent
    {
        void Update(GameObject owner, GameTime gameTime);
    }
}
