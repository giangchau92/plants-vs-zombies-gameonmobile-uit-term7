using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameCore.MessageCenter
{
    interface IMessageEffect
    {
        //MessageCenter Owner { get; set; }
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void Reset(string message);
    }
}
