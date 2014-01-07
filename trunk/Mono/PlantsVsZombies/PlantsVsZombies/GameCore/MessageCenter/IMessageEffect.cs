using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
