using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using Microsoft.Xna.Framework;
using PlantVsZombies.GameComponents.GameMessages;
using PlantVsZombies.GameComponents.Behaviors.Implements;
using SCSEngine.Serialization.XNASerializationHelper;
using SCSEngine.Sprite;

namespace PlantVsZombies.GameComponents.Components
{
    public enum eMoveRenderBehaviorType
    {
        ZO_NORMAL_RUNNING,
        ZO_NORMAL_EATING,

        STANDING,
        PL_SHOOTING,

        B_FLYING
    }

    public class RenderComponent : IComponent<MessageType>
    {
        // Behavior
        public IBehavior<MessageType> currentBehavior = null;
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
                    draw(message, gameTime);
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

        private void draw(IMessage<MessageType> message, GameTime gameTime)
        {
            currentBehavior.Update(message, gameTime);
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
            {
                if (currentBehavior != null)
                    currentBehavior.UnLoad();
                currentBehavior = supportBehaviors[typeBehavior];
                currentBehavior.OnLoad();
            }
        }

        public void Serialize(SCSEngine.Serialization.ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(SCSEngine.Serialization.IDeserializer deserializer)
        {
            var behaviorDesers = deserializer.DeserializeAll("Behavior");

            foreach (var behaviorDeser in behaviorDesers)
            {
                string type = behaviorDeser.DeserializeString("Type");
                eMoveRenderBehaviorType renderTypr = eMoveRenderBehaviorType.ZO_NORMAL_RUNNING;

                if (type == "xml_render_eat")
                    renderTypr = eMoveRenderBehaviorType.ZO_NORMAL_EATING;
                else if (type == "xml_render_run")
                    renderTypr = eMoveRenderBehaviorType.ZO_NORMAL_RUNNING;

                string resourceName = behaviorDeser.DeserializeString("Behavior");
                Vector2 bound = XNASerialization.Instance.DeserializeVector2(behaviorDeser, "Bound");

                RenderBehavior renderBehavior = new RenderBehavior();
                renderBehavior.Sprite = SCSServices.Instance.ResourceManager.GetResource<ISprite>(resourceName);
                renderBehavior.SpriteBound = new Rectangle(0, 0, (int)bound.X, (int)bound.Y);
                this.AddBehavior(renderTypr, renderBehavior);
            }
        }

        IComponent<MessageType> IComponent<MessageType>.Clone()
        {
            RenderComponent renCom = RenderComponentFactory.CreateComponent();
            foreach (var behavior in this.supportBehaviors)
            {
                renCom.AddBehavior(behavior.Key, behavior.Value.Clone());
            }
            renCom.currentBehavior = this.currentBehavior.Clone();

            return renCom;
        }
    }
}
