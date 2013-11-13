using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.GameMessages;

namespace PlantVsZombie.GameComponents.Components
{
    public enum eMoveBehaviorType
    {
        NORMAL_RUNNING,
        STANDING
    }
    public class MoveComponent : IComponent<MessageType>
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        // Behavior
        IBehavior<MessageType> currentBehavior = null;
        Dictionary<eMoveBehaviorType, IBehavior<MessageType>> supportBehaviors = new Dictionary<eMoveBehaviorType, IBehavior<MessageType>>();

        public MoveComponent()
        {
        }


        public void UpdatePosition(GameTime gameTime)
        {
            Position = new Vector2(Position.X + Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds,
                Position.Y + Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void OnMessage(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (message.MessageType)
            {
                case MessageType.FRAME_UPDATE:
                    currentBehavior.Update(gameTime);
                    break;
                case MessageType.CHANGE_MOVE_BEHAVIOR:
                    MoveBehaviorChangeMsg msg = message as MoveBehaviorChangeMsg;
                    if (msg == null)
                        throw new Exception("RenderComponent: Message must be MoveBehaviorChangeMsg!");
                    ChangeBehavior(msg.MoveBehaviorType);
                    break;
                default:
                    break;
            }
        }

        public void AddBehavior(eMoveBehaviorType type, IBehavior<MessageType> behavior)
        {
            behavior.Owner = this;
            this.supportBehaviors.Add(type, behavior);
            currentBehavior = behavior;
        }

        public void ChangeBehavior(eMoveBehaviorType typeBehavior)
        {
            if (supportBehaviors.ContainsKey(typeBehavior))
                currentBehavior = supportBehaviors[typeBehavior];
        }

        public IEntity<MessageType> Owner
        {
            get;
            set;
        }
    }
}
