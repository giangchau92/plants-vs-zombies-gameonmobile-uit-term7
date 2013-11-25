using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.GameMessages;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlantVsZombie.GameCore;
using PlantVsZombie.GameObjects;
using PlantVsZombie.GameComponents.Components;
using PlantVsZombie.GameObjects.Implements;

namespace PlantVsZombie.GameComponents.Behaviors.Zombie
{
    enum eNormalZombieState
    {
        RUNNING, EATING
    }
    public class Z_NormalLogicBehavior : BaseLogicBehavior
    {
        private eNormalZombieState ZombieState { get; set; } // Chỉ dùng cục bộ 

        public override void Update(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            // 
            ZombieState = eNormalZombieState.RUNNING;
            // Die
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("Z_NormalLogicBehavior: Expect Logic Component");
            if (logicCOm.Health < 0)
            {
                PlantVsZombie.GameCore.PZObjectManager.Instance.RemoveObject(this.Owner.Owner.ObjectId);
            }
            base.Update(message, gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("Z_NormalLogicBehavior: message is not CollisionDetectedMsg");

            
            // If no collision
            if (message.TargetCollision == null)
            {
                changeRunBehavior(gameTime);

                ZombieState = eNormalZombieState.RUNNING;
                return;
            }

            BasePlant plant = message.TargetCollision as BasePlant;

            // Collision detected;
            // Send message change behavior
            if (plant != null)
            {
                changeEatBehavivor(gameTime);

                ZombieState = eNormalZombieState.EATING;

                LogicComponent logicCOm = plant.GetComponent(typeof(LogicComponent)) as LogicComponent;
                if (logicCOm == null)
                    throw new Exception("Z_NormalLogicBehavior: Expect Target Logic Component");
                logicCOm.Health--;
            }
            else
            {
                if (ZombieState == eNormalZombieState.RUNNING)
                    changeRunBehavior(gameTime);
            }

            base.OnCollison(msg, gameTime);
        }

        private void changeEatBehavivor(GameTime gameTime)
        {
            MoveBehaviorChangeMsg moveMsg = new MoveBehaviorChangeMsg(MessageType.CHANGE_MOVE_BEHAVIOR, this);
            moveMsg.MoveBehaviorType = Components.eMoveBehaviorType.STANDING;
            moveMsg.DestinationObjectId = this.Owner.Owner.ObjectId;

            RenderBehaviorChangeMsg renderMsg = new RenderBehaviorChangeMsg(MessageType.CHANGE_RENDER_BEHAVIOR, this);
            renderMsg.RenderBehaviorType = GameComponents.Components.eMoveRenderBehaviorType.ZO_NORMAL_EATING;
            renderMsg.DestinationObjectId = this.Owner.Owner.ObjectId;

            PZObjectManager.Instance.SendMessage(moveMsg, gameTime);
            PZObjectManager.Instance.SendMessage(renderMsg, gameTime);
        }

        private void changeRunBehavior(GameTime gameTime)
        {
            MoveBehaviorChangeMsg moveMsg1 = new MoveBehaviorChangeMsg(MessageType.CHANGE_MOVE_BEHAVIOR, this);
            moveMsg1.MoveBehaviorType = Components.eMoveBehaviorType.RUNNING;
            moveMsg1.DestinationObjectId = this.Owner.Owner.ObjectId;

            RenderBehaviorChangeMsg renderMsg1 = new RenderBehaviorChangeMsg(MessageType.CHANGE_RENDER_BEHAVIOR, this);
            renderMsg1.RenderBehaviorType = GameComponents.Components.eMoveRenderBehaviorType.ZO_NORMAL_RUNNING;
            renderMsg1.DestinationObjectId = this.Owner.Owner.ObjectId;

            PZObjectManager.Instance.SendMessage(moveMsg1, gameTime);
            PZObjectManager.Instance.SendMessage(renderMsg1, gameTime);
        }

        
    }
}
