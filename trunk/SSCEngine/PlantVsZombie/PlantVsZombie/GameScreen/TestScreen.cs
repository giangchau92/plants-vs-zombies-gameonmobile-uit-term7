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
using SCSEngine.Sprite;

namespace PlantVsZombie.GameScreen
{
    public class TestScreen : BaseGameScreen
    {
        PZObjectManager objectManager = PZObjectManager.Instance;
        PZBoard gameBoard;

        public TestScreen(IGameScreenManager screenManager)
            : base(screenManager)
        {
            //objectManager.AddObject(new NormalPlant());
            TouchPanel.EnabledGestures = GestureType.Tap;

            // Init game data
            initSpriteBank();

            gameBoard = new PZBoard(9, 5);
            gameBoard.Board = new int[,]{
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {1, 0, 0, 0, 0, 0, 0, 0, 0},
                {1, 0, 0, 0, 0, 0, 0, 0, 0},
                {1, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0}
            };
            // Gen object

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 9; j++)
                {
                    int type = gameBoard.Board[i, j];
                    if (type == 1)
                    {
                        objectManager.AddObject(PZObjectFactory.Instance.createPlant(gameBoard.GetPositonAt(i, j)));
                    }
                }
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
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            spriteBatch.Begin();

            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_DRAW, this);
            updateMessage.DestinationObjectId = 0; // For every object
            objectManager.SendMessage(updateMessage, gameTime);

            base.Draw(gameTime);
            spriteBatch.End();
        }

        private void initSpriteBank()
        {
            if (SpriteFramesBank.Instance.Contains("DoublePea"))
                return;

            SpriteFramesBank.Instance.Add("DoublePea", FramesGenerator.Generate(100, 55, 1024, 40));
            //100, 55, 10, 40
        }
    }
}
