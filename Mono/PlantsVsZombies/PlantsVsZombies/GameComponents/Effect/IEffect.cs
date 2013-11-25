using Microsoft.Xna.Framework;
using PlantVsZombies.GameComponents.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombies.GameComponents.Effect
{
    public interface IEffect
    {
        BaseLogicBehavior Owner { get; set; }
        void Update(GameTime gameTime);
    }
}
