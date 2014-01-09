using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameComponents.Behaviors.Implements;
using SCSEngine.Serialization.XNASerializationHelper;
using SCSEngine.Sprite;
using PlantsVsZombies.GameCore;

namespace PlantsVsZombies.GameComponents.Components
{
    public enum eMoveRenderBehaviorType
    {
        ZO_NORMAL_RUNNING,
        ZO_NORMAL_EATING,
        ZO_NORMAL_DEATH,

        STANDING,
        PL_SHOOTING,
        PL_EXPLOSION,

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

            SCSServices.Instance.SpriteBatch.DrawString(SCSServices.Instance.DebugFont, ((int)logicCOm.Health).ToString(), moveCOm.Position, Color.Red);
        }

        public IBehavior<MessageType> GetCurrentBehavior()
        {
            return this.currentBehavior;
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
                else if (type == "xml_render_stand")
                    renderTypr = eMoveRenderBehaviorType.STANDING;
                else if (type == "xml_render_death")
                    renderTypr = eMoveRenderBehaviorType.ZO_NORMAL_DEATH;
                else if (type == "xml_render_explosion")
                    renderTypr = eMoveRenderBehaviorType.PL_EXPLOSION;

                string resourceName = behaviorDeser.DeserializeString("ResourceName");
                double timeFrame = behaviorDeser.DeserializeDouble("TimeFrame");
                Vector2 footPos;
                try
                {
                    footPos = XNASerialization.Instance.DeserializeVector2(behaviorDeser, "FootPosition");
                }
                catch (Exception)
                {
                    footPos = new Vector2(PZBoard.CELL_WIDTH, PZBoard.CELL_HEIGHT);
                }
                RenderBehavior renderBehavior = new RenderBehavior();
                renderBehavior.Sprite = SCSServices.Instance.ResourceManager.GetResource<ISprite>(resourceName);
                renderBehavior.Sprite.TimeDelay = TimeSpan.FromSeconds(timeFrame);
                renderBehavior.FootPositon = footPos;
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
            //renCom.currentBehavior = this.currentBehavior.Clone();

            return renCom;
        }


        public void OnComplete()
        {
        }
    }
}
