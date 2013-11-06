using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace SSCEngine.Utils.GameObject.Component
{
    public interface IComponent<T>
    {
        IEntity<T> Owner { get; set; }
        void OnMessage(IMessage<T> message, GameTime gameTime);
    }
}
