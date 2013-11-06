using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;

namespace PlantVsZombie.GameComponents.Components
{
    public class PhysicComponent : IComponent<MessageType>
    {
        private Rectangle _bound;
        public Rectangle Bound
        {
            get { return _bound; }
            set
            {
                _bound = value;
                UpdateFrame();
            }
        }

        private Rectangle _frame;
        public Rectangle Frame { get { return _frame; } }

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

        public PhysicComponent()
        {
            _frame = Rectangle.Empty;
            Bound = Rectangle.Empty;
        }

        public void OnMessage(IMessage<MessageType> message, GameTime gameTime)
        {
            switch (message.MessageType)
            {
                case MessageType.FRAME_UPDATE:
                    UpdateFrame();
                    break;
                default:
                    break;
            }
        }

        private void UpdateFrame()
        {
            if (_owner == null)
            {
                _frame = Bound;
                return;
            }
            MoveComponent moveCom = Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
            if (moveCom == null)
            {
                _frame = Bound;
                return;
            }
            _frame = new Rectangle((int)moveCom.Position.X, (int)moveCom.Position.Y, Bound.Width, Bound.Height);
        }
    }
}
