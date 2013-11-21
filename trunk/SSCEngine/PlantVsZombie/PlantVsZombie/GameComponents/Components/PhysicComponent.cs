using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;
using PlantVsZombie.GameCore;
using PlantVsZombie.GameComponents.GameMessages;

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
                    CheckCollision(gameTime);
                    break;
                default:
                    break;
            }
        }

        public void UpdateFrame()
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
            _frame = new Rectangle((int)moveCom.Position.X, (int)moveCom.Position.Y - Bound.Height, Bound.Width, Bound.Height);
        }

        private void CheckCollision(GameTime gameTime)
        {
            IDictionary<ulong, ObjectEntity> objs = PZObjectManager.Instance.GetCopyObjects();

            bool isCollision = false;
            foreach (var item in objs)
            {
                if (item.Value == this.Owner)
                    continue;

                PhysicComponent otherPhys = item.Value.GetComponent(typeof(PhysicComponent)) as PhysicComponent;
                if (otherPhys == null)
                    throw new Exception("PhysicComponent: Target doesn't have Physic component");
                if (this.Frame.Intersects(otherPhys.Frame))
                {
                    CollisionDetectedMsg colMsg = new CollisionDetectedMsg(MessageType.COLLISON_DETECT, this.Owner);
                    colMsg.DestinationObjectId = this.Owner.ObjectId;
                    colMsg.TargetCollision = item.Value;

                    PZObjectManager.Instance.SendMessage(colMsg, gameTime);
                    isCollision = true;
                }
            }

            if (!isCollision)
            {
                CollisionDetectedMsg colMsg = new CollisionDetectedMsg(MessageType.COLLISON_DETECT, this.Owner);
                colMsg.DestinationObjectId = this.Owner.ObjectId;
                colMsg.TargetCollision = null;

                PZObjectManager.Instance.SendMessage(colMsg, gameTime);
            }
        }
    }
}
