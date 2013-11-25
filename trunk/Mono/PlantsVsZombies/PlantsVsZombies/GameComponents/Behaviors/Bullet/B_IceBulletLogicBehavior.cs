using Microsoft.Xna.Framework;
using PlantVsZombies.GameComponents.Behaviors.Implements;
using PlantVsZombies.GameComponents.Components;
using PlantVsZombies.GameComponents.Effect.Implements;
using PlantVsZombies.GameComponents.GameMessages;
using PlantVsZombies.GameCore;
using PlantVsZombies.GameObjects;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombies.GameComponents.Behaviors.Bullet
{
    class B_IceBulletLogicBehavior : BaseLogicBehavior
    {

        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {

            base.Update(message, gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("B_IceBulletLogicBehavior: message is not CollisionDetectedMsg");

            BaseZombie zom = message.TargetCollision as BaseZombie;
            // Send message change to 
            if (zom != null)
            {
                LogicComponent logicCOm = zom.GetComponent(typeof(LogicComponent)) as LogicComponent;
                if (logicCOm == null)
                    throw new Exception("B_IceBulletLogicBehavior: Expect Target Logic Component");
                logicCOm.Health -= 10;

                SlowMoveEffect slowEff = new SlowMoveEffect();
                slowEff.TimeDurring = TimeSpan.FromSeconds(2);

                logicCOm.LogicBehavior.AddEffect(slowEff);

                PZObjectManager.Instance.RemoveObject(Owner.Owner.ObjectId);
            }

            base.OnCollison(msg, gameTime);
        }
    }
}
