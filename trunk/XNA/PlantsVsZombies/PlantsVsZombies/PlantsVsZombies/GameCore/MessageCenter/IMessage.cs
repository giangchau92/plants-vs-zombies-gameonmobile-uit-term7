using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameCore.MessageCenter
{
    interface IMessage
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
