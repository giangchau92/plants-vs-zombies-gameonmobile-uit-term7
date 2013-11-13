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
using System.Diagnostics;

namespace PlantVsZombie.GameScreen
{
    public class TestScreen : BaseGameScreen
    {
        PZObjectManager objectManager = PZObjectManager.Instance;

        public TestScreen(IGameScreenManager screenManager)
            : base(screenManager)
        {
            
            objectManager.AddObject(new NormalPlant());
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public override void Update(GameTime gameTime)
        {
            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_UPDATE, this);
            updateMessage.DestinationObjectId = 0; // For all object

            objectManager.SendMessage(updateMessage, gameTime);

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap)
                {
                    objectManager.AddObject(new NormalZombie());
                }
            }

            base.Update(gameTime);
            Debug.WriteLine(string.Format("Eslaped: {0}", gameTime.ElapsedGameTime.TotalMilliseconds));

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
