using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameCore;
using SCSEngine.Serialization;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameComponents.Behaviors.Plant
{
    public class P_StoneLogicBehavior : BaseLogicBehavior
    {
        eNormalPlantState PlantState { get; set; }

        public P_StoneLogicBehavior()
            :base()
        {
        }


        public override void Update(IMessage<MessageType> msg, GameTime gameTime)
        {
            PlantState = eNormalPlantState.STANDING;
            
            
            // Die
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("PL_NormalLogicBehavior: Expect Logic Component");
            if (logicCOm.Health < 0)
            {
                PZObjectManager.Instance.RemoveObject(this.Owner.Owner.ObjectId);
            }
            base.Update(msg, gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("PL_NormalLogicBehavior: message is not CollisionDetectedMsg");

            base.OnCollison(msg, gameTime);
        }

        public override IBehavior<MessageType> Clone()
        {
            var clone = new P_StoneLogicBehavior();
            return clone;
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            // CODE HERE
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("PL_NormalLogicBehavior: Expect Logic Component");
            logicCOm.Health = deserializer.DeserializeInteger("Health");
        }
    }
}
