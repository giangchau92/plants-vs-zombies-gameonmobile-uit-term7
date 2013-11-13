using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.GameMessages;

namespace PlantVsZombie.GameComponents.Components
{
    public enum eMoveRenderBehaviorType
    {
        ZO_NORMAL_RUNNING,
        ZO_NORMAL_EATING,
        STANDING,

        PL_SHOOTING
    }

    public class RenderComponent : IComponent<MessageType>
    {
        // Behavior
        IBehavior<MessageType> currentBehavior = null;
        Dictionary<eMoveRenderBehaviorType, IBehavior<MessageType>> supportBehaviors = new Dictionary<eMoveRenderBehaviorType, IBehavior<MessageType>>();

        public RenderComponent()
        {
        }

        public IEntity<MessageType> Owner
        {
            get;
            set;
        }

        public void OnMessage(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (message.MessageType)
            {
                case MessageType.FRAME_DRAW:
                    draw(gameTime);
                    break;
                case MessageType.CHANGE_RENDER_BEHAVIOR:
                    RenderBehaviorChangeMsg msg = message as RenderBehaviorChangeMsg;
                    if (msg == null)
                        throw new Exception("RenderComponent: Message must be MoveBehaviorChangeMsg!");
                    ChangeBehavior(msg.RenderBehaviorType);
                    break;
                default:
                    break;
            }
        }

        private void draw(GameTime gameTime)
        {
            currentBehavior.Update(gameTime);
            // Draw health
            LogicComponent logicCOm = this.Owner.GetComponent(typeof(LogicComponent)) as LogicComponent;
            MoveComponent moveCOm = this.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
            if (logicCOm == null)
                throw new Exception("RenderComponent: Expect Logic component");
            if (moveCOm == null)
                throw new Exception("RenderComponent: Expect Move component");

            SCSServices.Instance.SpriteBatch.DrawString(SCSServices.Instance.DebugFont, logicCOm.Health.ToString(), moveCOm.Position, Color.Red);
        }

        public void AddBehavior(eMoveRenderBehaviorType type, IBehavior<MessageType> behavior)
        {
            behavior.Owner = this;
            this.supportBehaviors.Add(type, behavior);
            currentBehavior = behavior;
        }

        public void ChangeBehavior(eMoveRenderBehaviorType typeBehavior)
        {
            if (supportBehaviors.ContainsKey(typeBehavior))
                currentBehavior = supportBehaviors[typeBehavior];
        }
    }
}
