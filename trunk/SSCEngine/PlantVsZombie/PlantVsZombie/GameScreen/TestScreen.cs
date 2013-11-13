using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.ScreenManagement;
using Microsoft.Xna.Framework;
using SSCEngine.Utils.GameObject.Component;
using PlantVsZombie.GameComponents;
using PlantVsZombie.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using Microsoft.Xna.Framework.Input.Touch;
using PlantVsZombie.GameCore;
using PlantVsZombie.GameComponents.GameMessages;

namespace PlantVsZombie.GameScreen
{
    public class TestScreen : BaseGameScreen
    {
        PZObjectManager objectManager = PZObjectManager.Instance;

        public TestScreen(IGameScreenManager screenManager)
            : base(screenManager)
        {
            
            objectManager.AddObject(new NormalPlant());
        }

        public override void Update(GameTime gameTime)
        {
            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_UPDATE, this);
            updateMessage.DestinationObjectId = 0; // For all object

            objectManager.SendMessage(updateMessage, gameTime);

            TouchCollection touches = TouchPanel.GetState();
            if (touches.Count != 0 && objectManager.GetObjects().Count < 2)
            {
                //MoveBehaviorChangeMsg moveMsg = new MoveBehaviorChangeMsg(MessageType.CHANGE_MOVE_BEHAVIOR, this);
                //moveMsg.MoveBehaviorType = GameComponents.Components.eMoveBehaviorType.NORMAL_RUNNING;
                

                //RenderBehaviorChangeMsg renderMsg = new RenderBehaviorChangeMsg(MessageType.CHANGE_RENDER_BEHAVIOR, this);
                //renderMsg.RenderBehaviorType = GameComponents.Components.eMoveRenderBehaviorType.ZO_NORMAL_RUNNING;

                //objectManager.SendMessage(moveMsg, gameTime);
                //objectManager.SendMessage(renderMsg, gameTime);
                objectManager.AddObject(new NormalZombie());
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            spriteBatch.Begin();

            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_DRAW, this);
            updateMessage.DestinationObjectId = 0;
            objectManager.SendMessage(updateMessage, gameTime);

            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
