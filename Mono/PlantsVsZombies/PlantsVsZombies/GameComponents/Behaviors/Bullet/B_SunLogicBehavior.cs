using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameComponents.Effect;
using PlantsVsZombies.GameComponents.Effect.Implements;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameCore;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameComponents.Behaviors.Bullet
{
    public class B_SunLogicBehavior : BaseLogicBehavior
    {
        private enum eSunState
        {
            JUMP, STAND
        }

        private eSunState SunState { get; set; }

        public B_SunLogicBehavior()
            : base()
        {
            SunState = eSunState.JUMP;
            IEffect effect = new SunRiseEffect();
            AddEffect(effect);
        }

        public override void Update(IMessage<MessageType> msg, GameTime gameTime)
        {
            if (listEffect.Count == 0)
                SunState = eSunState.STAND;



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
            var clone = new B_SunLogicBehavior();
            return clone;
        }
    }
}
