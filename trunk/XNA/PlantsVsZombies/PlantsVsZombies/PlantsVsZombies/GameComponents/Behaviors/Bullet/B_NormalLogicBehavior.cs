using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SCSEngine.Utils.GameObject.Component;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameObjects;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameCore;
using SCSEngine.Serialization;
using SCSEngine.Audio;
using SCSEngine.Services;

namespace PlantsVsZombies.GameComponents.Behaviors.Bullet
{
    public class B_NormalLogicBehavior : BaseLogicBehavior
    {
        private static TimeSpan _lastTimeSound;
        private static TimeSpan _timeDelaySound = TimeSpan.FromSeconds(1);
        private bool justCollect = false;
        private Sound _sound;

        private float _damage = 0;

        public B_NormalLogicBehavior()
            : base()
        {
            _sound = SCSServices.Instance.ResourceManager.GetResource<Sound>("Sounds/IceBulletCollide");
        }

        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {

            base.Update(message, gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {

            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("B_NormalLogicBehavior: message is not CollisionDetectedMsg");

            ObjectEntity zom = message.TargetCollision as ObjectEntity;
            // Send message change to 
            if (zom != null && zom.ObjectType == eObjectType.ZOMBIE)
            {
                LogicComponent logicCOm = zom.GetComponent(typeof(LogicComponent)) as LogicComponent;
                if (logicCOm == null)
                    throw new Exception("B_NormalLogicBehavior: Expect Target Logic Component");
                logicCOm.Health -= _damage;
                PZObjectManager.Instance.RemoveObject(Owner.Owner.ObjectId);
                justCollect = true;
            }

            // Sound
            if (justCollect)
            {
                if (gameTime.TotalGameTime - _lastTimeSound > _timeDelaySound)
                {
                    SCSServices.Instance.AudioManager.PlaySound(_sound, false, true);
                    _lastTimeSound = gameTime.TotalGameTime;
                }
            }
            justCollect = false;
            //

            base.OnCollison(msg, gameTime);
        }

        public override IBehavior<MessageType> Clone()
        {
            B_NormalLogicBehavior clone = new B_NormalLogicBehavior();
            clone._damage = _damage;
            return clone;
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            // CODE HERE
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("PL_NormalLogicBehavior: Expect Logic Component");
            _damage = deserializer.DeserializeInteger("Damage");
        }
    }
}
