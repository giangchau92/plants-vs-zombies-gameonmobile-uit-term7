using Microsoft.Xna.Framework;
using PlantVsZombies.GameComponents.Components;
using PlantVsZombies.GameComponents.GameMessages;
using PlantVsZombies.GameCore;
using SCSEngine.Services;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantVsZombies.GameComponents.Behaviors.Plant
{
    public class P_IcePlantLogicBehavior : BaseLogicBehavior
    {
        eNormalPlantState PlantState { get; set; }
        TimeSpan currentTimeShoot = TimeSpan.Zero;
        TimeSpan shootTime = new TimeSpan(0, 0, 0, 0, 500);
        Vector2 shootPoint = new Vector2(90, 45);

        public override void Update(IMessage<MessageType> msg, GameTime gameTime)
        {
            PlantState = eNormalPlantState.STANDING;
            // Shoot
            IDictionary<ulong, ObjectEntity> objs = new Dictionary<ulong, ObjectEntity>(PZObjectManager.Instance.GetObjects());
            foreach (var item in objs)
            {
                if (item.Value == this.Owner)
                    continue;
                MoveComponent obj1 = this.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
                MoveComponent obj2 = item.Value.GetComponent(typeof(MoveComponent)) as MoveComponent;

                if (obj2.Position.Y == obj1.Position.Y && (obj1.Position.X < obj2.Position.X) && obj2.Position.X < SCSServices.Instance.Game.GraphicsDevice.Viewport.Width
                    && (obj2.Owner as ObjectEntity).ObjectType == eObjectType.ZOMBIE)
                {
                    // Change to shoot
                    //RenderBehaviorChangeMsg renderMsg = new RenderBehaviorChangeMsg(MessageType.CHANGE_RENDER_BEHAVIOR, this);
                    //renderMsg.RenderBehaviorType = GameComponents.Components.eMoveRenderBehaviorType.PL_SHOOTING;
                    //renderMsg.DestinationObjectId = this.Owner.Owner.ObjectId;

                    //PZObjectManager.Instance.SendMessage(renderMsg, gameTime);
                    PlantState = eNormalPlantState.SHOOTING;
                }
            }
            if (PlantState == eNormalPlantState.SHOOTING)
            {
                if (currentTimeShoot > shootTime)
                {

                    Vector2 pos = (this.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent).Position;
                    ObjectEntity bullet = PZObjectFactory.Instance.createIceBullet(new Vector2(pos.X + shootPoint.X, pos.Y - shootPoint.Y));
                    //bullet.SetPosition();
                    PZObjectManager.Instance.AddObject(bullet);
                    currentTimeShoot = TimeSpan.Zero;
                }
                else
                    currentTimeShoot += gameTime.ElapsedGameTime;
            }

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
            return new P_IcePlantLogicBehavior();
        }
    }
}
