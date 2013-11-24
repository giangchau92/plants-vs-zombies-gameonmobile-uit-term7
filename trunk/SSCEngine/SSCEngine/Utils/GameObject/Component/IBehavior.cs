using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.Utils.GameObject.Component
{
    public interface IBehavior<T>
    {
        void OnLoad();
        void UnLoad();
        void Update(IMessage<T> message, GameTime gameTime);
        IComponent<T> Owner { get; set; }
    }
}
