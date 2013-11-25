using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.ScreenManagement;
using Microsoft.Xna.Framework;
using SCSEngine.Utils.GameObject.Component;
using PlantVsZombies.GameComponents;
using PlantVsZombies.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using Microsoft.Xna.Framework.Input.Touch;
using PlantVsZombies.GameCore;
using PlantVsZombies.GameComponents.GameMessages;
using System.Diagnostics;
using SCSEngine.Sprite;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using PlantVsZombies.GameComponents.Components;

namespace PlantVsZombies.GameScreen
{
    public class TestScreen : BaseGameScreen
    {
        PZObjectManager objectManager = PZObjectManager.Instance;
        PZBoard gameBoard;

        bool isPlant = true;

        public TestScreen(IGameScreenManager screenManager)
            : base(screenManager)
        {
            //objectManager.AddObject(new NormalPlant());
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Hold;

            // Init game data
            initSpriteBank();

            gameBoard = new PZBoard(9, 5);
            gameBoard.Board = new int[,]{
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0}
            };
            // Gen object

            //for (int i = 0; i < 5; i++)
            //    for (int j = 0; j < 9; j++)
            //    {
            //        int type = gameBoard.Board[i, j];
            //        if (type == 1)
            //        {
            //            objectManager.AddObject(PZObjectFactory.Instance.createPlant(gameBoard.GetPositonAt(i, j)));
            //        }
            //        else if (type == 2)
            //        {
            //            objectManager.AddObject(PZObjectFactory.Instance.createIcePlant(gameBoard.GetPositonAt(i, j)));
            //        }
            //    }

            ObjectEntity obj = GameObjectCenter.Instance.CreateObject("xml_stand_zombie");
            (obj.GetComponent(typeof(MoveComponent)) as MoveComponent).Position = new Vector2(200, 200);
            int a;
            a = 1;
           
        }

        public override void Update(GameTime gameTime)
        {

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap)
                {
                    //objectManager.AddObject(new NormalZombie());
                    objectManager.AddObject(PZObjectFactory.Instance.createZombie(gameBoard.GetPositionAtPoint(gesture.Position)));
                }
                else if (gesture.GestureType == GestureType.Hold)
                {
                    if (isPlant)
                        objectManager.AddObject(PZObjectFactory.Instance.createPlant(gameBoard.GetPositionAtPoint(gesture.Position)));
                    else
                        objectManager.AddObject(PZObjectFactory.Instance.createIcePlant(gameBoard.GetPositionAtPoint(gesture.Position)));
                    isPlant = !isPlant;
                }
            }

            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_UPDATE, this);
            updateMessage.DestinationObjectId = 0; // For all object

            objectManager.SendMessage(updateMessage, gameTime);

            

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

            SpriteFramesBank.Instance.Add("Plants/DoublePea/DoublePea", FramesGenerator.Generate(100, 55, 1024, 40));
            SpriteFramesBank.Instance.Add("Plants/IcePea/IcePea", FramesGenerator.Generate(118, 63, 1024, 33));
            SpriteFramesBank.Instance.Add("Zombies/Nameless/Walk", FramesGenerator.Generate(73, 100, 1024, 16));
            SpriteFramesBank.Instance.Add("Zombies/Nameless/Attack", FramesGenerator.Generate(89, 101, 1024, 16));
            SpriteFramesBank.Instance.Add("Bullets/B_Pea", FramesGenerator.Generate(29, 22, 29, 1));
            //100, 55, 10, 40
        }
    }
}
