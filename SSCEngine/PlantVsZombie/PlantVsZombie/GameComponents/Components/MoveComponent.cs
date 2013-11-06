using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;

namespace PlantVsZombie.GameComponents.Components
{
    public class MoveComponent : IComponent<MessageType>
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        public MoveComponent()
        {
        }

        public void OnMessage(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (message.MessageType)
            {
                case MessageType.FRAME_UPDATE:
                    Position = new Vector2(Position.X + 100 * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
                    break;
                default:
                    break;
            }
        }

        private IEntity<MessageType> _owner = null;
        public IEntity<MessageType> Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }
    }
}
