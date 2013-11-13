using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SSCEngine.Utils.GameObject.Component;
using PlantVsZombie.GameComponents.GameMessages;
using PlantVsZombie.GameComponents.Components;
using PlantVsZombie.GameCore;

namespace PlantVsZombie.GameComponents.Behaviors.Plant
{
    public class PL_NormalLogicBehavior : BaseLogicBehavior
    {
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Shoot
            IDictionary<ulong, ObjectEntity> objs = PZObjectManager.Instance.GetObjects();
            foreach (var item in objs)
            {
                if (item.Value == this.Owner)
                    continue;
                MoveComponent obj1 = this.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
                MoveComponent obj2 = item.Value.GetComponent(typeof(MoveComponent)) as MoveComponent;

                if (obj2.Position.Y == obj1.Position.Y && (obj1.Position.X < obj2.Position.X))
                {
                    // Change to shoot

                    RenderBehaviorChangeMsg renderMsg = new RenderBehaviorChangeMsg(MessageType.CHANGE_RENDER_BEHAVIOR, this);
                    renderMsg.RenderBehaviorType = GameComponents.Components.eMoveRenderBehaviorType.PL_SHOOTING;
                    renderMsg.DestinationObjectId = this.Owner.Owner.ObjectId;

                    //PZObjectManager.Instance.SendMessage(moveMsg, gameTime);
                    PZObjectManager.Instance.SendMessage(renderMsg, gameTime);
                }
            }
            // Die
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("PL_NormalLogicBehavior: Expect Logic Component");
            if (logicCOm.Health < 0)
            {
                PlantVsZombie.GameCore.PZObjectManager.Instance.RemoveObject(this.Owner.Owner.ObjectId);
            }
            base.Update(gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("PL_NormalLogicBehavior: message is not CollisionDetectedMsg");

            int a;
            a = 1;

            base.OnCollison(msg, gameTime);
        }
    }
}
