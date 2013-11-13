using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.Utils.GameObject.Component
{
    public interface IBehavior<T>
    {
        void Update(GameTime gameTime);
        IComponent<T> Owner { get; set; }
    }
}
