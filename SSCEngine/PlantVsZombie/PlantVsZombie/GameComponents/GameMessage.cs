using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;

namespace PlantVsZombie.GameComponents
{
    public enum MessageType
    {
        FRAME_UPDATE,
        FRAME_DRAW
    }
    public class GameMessage : IMessage<MessageType>
    {
        MessageType _messageType;
        MessageType IMessage<MessageType>.MessageType
        {
            get { return _messageType; }
        }

        public GameMessage(MessageType message)
        {
            this._messageType = message;
        }

        
    }
}
