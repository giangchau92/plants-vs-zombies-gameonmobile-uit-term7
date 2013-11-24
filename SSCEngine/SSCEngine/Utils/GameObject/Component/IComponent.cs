using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SCSEngine.Serialization;


namespace SCSEngine.Utils.GameObject.Component
{
    public interface IComponent<T> : ISerializable
    {
        IEntity<T> Owner { get; set; }
        void OnMessage(IMessage<T> message, GameTime gameTime);
    }
}
