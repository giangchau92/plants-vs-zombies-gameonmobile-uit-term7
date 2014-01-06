using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameCore.MessageCenter
{
    interface IStringEffect
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
