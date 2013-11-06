using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;

namespace PlantVsZombie.GameComponents.Components
{
    public class LogicComponent : IComponent<MessageType>
    {
        public LogicComponent()
        {
        }

        public IEntity<MessageType> Owner
        {
            get;
            set;
        }

        public void OnMessage(IMessage<MessageType> message, GameTime gameTime)
        {
            switch (message.MessageType)
            {
                case MessageType.FRAME_UPDATE:
                    break;
                default:
                    break;
            }
        }
    }
}
