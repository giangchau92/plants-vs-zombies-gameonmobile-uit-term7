using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Behaviors;
using PlantsVsZombies.GameComponents.Behaviors.Zombie;
using PlantsVsZombies.GameComponents.Behaviors.Plant;
using PlantsVsZombies.GameComponents.Behaviors.Bullet;

namespace PlantsVsZombies.GameComponents.Components
{
    public class LogicComponent : IComponent<MessageType>
    {
        public float Health { get; set; }

        private BaseLogicBehavior _logicBehavior = null;
        public BaseLogicBehavior LogicBehavior
        {
            get { return _logicBehavior;}
            set
            {
                _logicBehavior = value;
                _logicBehavior.Owner = this;
            }
        }

        public LogicComponent()
        {
            Health = 100;
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
                    if (LogicBehavior != null)
                        LogicBehavior.Update(message, gameTime);
                    break;
                case MessageType.COLLISON_DETECT:
                    if (LogicBehavior != null)
                        LogicBehavior.OnCollison(message, gameTime);
                    break;
                default:
                    break;
            }
        }

        public void Serialize(SCSEngine.Serialization.ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(SCSEngine.Serialization.IDeserializer deserializer)
        {
            var behDeser = deserializer.SubDeserializer("Behavior");
            string behaviorType = behDeser.DeserializeString("Type");

            if (behaviorType == "xml_NormalZombie")
                this.LogicBehavior = new Z_NormalLogicBehavior();
            else if (behaviorType == "xml_NormalPlant")
                this.LogicBehavior = new P_NormalLogicBehavior();
            else if (behaviorType == "xml_IcePlant")
                this.LogicBehavior = new P_IcePlantLogicBehavior();
            else if (behaviorType == "xml_StonePlant")
                this.LogicBehavior = new P_StoneLogicBehavior();
            else if (behaviorType == "xml_SunflowerPlant")
                this.LogicBehavior = new P_SunFlowerLogicBehavior();
            else if (behaviorType == "xml_NormalBullet")
                this.LogicBehavior = new B_NormalLogicBehavior();
            else if (behaviorType == "xml_IceBullet")
                this.LogicBehavior = new B_IceBulletLogicBehavior();
            else if (behaviorType == "xml_Sun")
                this.LogicBehavior = new B_SunLogicBehavior();
            

            this.LogicBehavior.Deserialize(behDeser);
        }

        IComponent<MessageType> IComponent<MessageType>.Clone()
        {
            LogicComponent logicCom = LogicComponentFactory.CreateComponent();
            logicCom.Health = this.Health;
            logicCom.LogicBehavior = this.LogicBehavior.Clone() as BaseLogicBehavior;
            return logicCom;
        }


        public void OnComplete()
        {
            
        }
    }
}
