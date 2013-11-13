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
        FRAME_DRAW,

        CHANGE_MOVE_BEHAVIOR,
        CHANGE_RENDER_BEHAVIOR,

        COLLISON_DETECT
    }
    public class GameMessage : IMessage<MessageType>
    {
        MessageType _messageType;
        MessageType IMessage<MessageType>.MessageType
        {
            get { return _messageType; }
        }

        public GameMessage(MessageType message, object sender)
        {
            this._messageType = message;
            DestinationObjectId = 0;
        }

        public bool Handled
        {
            get;
            set;
        }

        public ulong DestinationObjectId
        {
            get;
            set;
        }


        public object Sender
        {
            get;
            set;
        }
    }
}
