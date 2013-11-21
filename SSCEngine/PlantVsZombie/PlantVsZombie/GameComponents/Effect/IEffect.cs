using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombie.GameComponents.Effect
{
    public interface IEffect
    {
        BaseLogicBehavior Owner { get; set; }
        void Update(GameTime gameTime);
    }
}
