using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.ScreenManagement;
using Microsoft.Xna.Framework;
using SCSEngine.Utils.GameObject.Component;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using Microsoft.Xna.Framework.Input.Touch;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.GameComponents.GameMessages;
using System.Diagnostics;
using SCSEngine.Sprite;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameCore.Level;
using PlantsVsZombies.GameCore;
using SCSEngine.Control;
using PlantsVsZombies.GrowSystem;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.GestureHandling.Implements.Detectors;
using PlantsVsZombies.Orientations;
using SCSEngine.Serialization.XmlSerialization;

namespace PlantsVsZombies.GameScreen
{
    public class TestScreen : BaseGameScreen, IGestureTarget<FreeTap>
    {
        PZObjectManager objectManager = PZObjectManager.Instance;
        PZBoard gameBoard;
        Level level;

        bool isPlant = true;

        // UI
        private Vector2 pos;
        private UIControlManager uiControlManager;
        private PvZGrowList growList;
        private PvZGrowSystem growSystem;

        public TestScreen(IGameScreenManager screenManager)
            : base(screenManager)
        {
            //objectManager.AddObject(new NormalPlant());
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Hold;

            

            gameBoard = new PZBoard(9, 5, objectManager);
            gameBoard.Board = new int[,] {
                {1, 0, 0, 0, 0, 0, 3, 0, 0},
                {1, 4, 0, 0, 0, 0, 3, 0, 0},
                {1, 3, 0, 0, 0, 0, 3, 0, 0},
                {1, 0, 0, 0, 0, 0, 3, 0, 0}
            };
            // Gen object

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 9; j++)
                {
                    int type = gameBoard.Board[i, j];
                    if (type == 1)
                    {
                        Vector2 pos = gameBoard.GetPositonAt(i, j);
                        objectManager.AddObject(PZObjectFactory.Instance.createPlant(gameBoard.GetPositonAt(i, j)));
                    }
                    else if (type == 2)
                    {
                        objectManager.AddObject(PZObjectFactory.Instance.createIcePlant(gameBoard.GetPositonAt(i, j)));
                    }
                    else if (type == 3)
                    {
                        objectManager.AddObject(PZObjectFactory.Instance.createStonePlant(gameBoard.GetPositonAt(i, j)));
                    }
                    else if (type == 4)
                    {
                        objectManager.AddObject(PZObjectFactory.Instance.createSunflowerPlant(gameBoard.GetPositonAt(i, j)));
                    }
                }

            level = PZLevelManager.Instance.GetLevel(0);
            //
            IGestureManager gm = DefaultGestureHandlingFactory.Instance.CreateManager(this.Game, //DefaultGestureHandlingFactory.Instance.CreateTouchController());
                new OrientedTouchController(DefaultGestureHandlingFactory.Instance.CreateTouchController(), GameOrientation.Instance));
            gm.AddDetector<FreeTap>(new FreeTapDetector());
            this.Components.Add(gm);
            IGestureDispatcher dp = DefaultGestureHandlingFactory.Instance.CreateDispatcher();
            dp.AddTarget<FreeTap>(this);
            gm.AddDispatcher(dp);

            this.uiControlManager = new UIControlManager(this.Game, DefaultGestureHandlingFactory.Instance);
            gm.AddDispatcher(this.uiControlManager);
            this.Components.Add(this.uiControlManager);
            //
            this.growList = new PvZGrowList(this.Game, 60, 10, this.uiControlManager, new PvZHardCurrency(100000));
            this.growList.Canvas.Bound.Position = new Vector2(0, 380);
            this.growList.Canvas.Bound.Size = new Vector2(280, 100);
            this.growList.Canvas.Content.Height = 80;
            this.growList.Canvas.Content.Position = new Vector2(100, 10);
            this.growList.Background = SCSServices.Instance.ResourceManager.GetResource<ISprite>("BuyPlant");
            this.uiControlManager.Add(this.growList);

            this.growSystem = new PvZGrowSystem(this.Game, new DoNothingGameGrow(gameBoard));
            this.growSystem.Deserialize(XmlSerialization.Instance.Deserialize(new FileStream(@"Xml\PlantGrowButtons.xml", FileMode.Open, FileAccess.Read)));
            this.growList.AddGrowButton(growSystem.Buttons["Single Pea"].CreateButton(this.Game));
        }

        public override void Update(GameTime gameTime)
        {
            

            //
            level.Update(gameBoard, gameTime);

            // Update game
            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_UPDATE, this);
            updateMessage.DestinationObjectId = 0; // For all object

            objectManager.SendMessage(updateMessage, gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            spriteBatch.Begin();

            spriteBatch.DrawString(SCSServices.Instance.DebugFont, string.Format("Touch pos: {0} - {1}", pos.X, pos.Y), new Vector2(100, 100), Color.White);

            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_DRAW, this);
            updateMessage.DestinationObjectId = 0; // For every object
            objectManager.SendMessage(updateMessage, gameTime);
            
            base.Draw(gameTime);
            spriteBatch.End();
        }



        public bool ReceivedGesture(FreeTap gEvent)
        {
            pos = gEvent.Current;
            return true;
        }

        public bool IsHandleGesture(FreeTap gEvent)
        {
            return true;
        }

        public uint Priority
        {
            get { return 0; }
        }

        public bool IsGestureCompleted
        {
            get { return false; }
        }
    }
}
