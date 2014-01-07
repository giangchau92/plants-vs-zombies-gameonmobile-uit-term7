using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.GameCore.Level;
using PlantsVsZombies.GrowSystem;
using PlantsVsZombies.Orientations;
using SCSEngine.Control;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Detectors;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.ScreenManagement;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.Serialization.XmlSerialization;
using SCSEngine.Services;
using SCSEngine.Utils.GameObject.Component;
using System.IO;

namespace PlantsVsZombies.GameScreen
{
    public class TestScreen : BaseGameScreen, IGestureTarget<FreeTap>
    {
        PZObjectManager objectManager = PZObjectManager.Instance;
        PZBoard gameBoard;
        Level level;

        // UI
        private Vector2 pos;
        private UIControlManager uiControlManager;
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
                        objectManager.AddObject(PZObjectFactory.Instance.createPlant("xml_Plant_DoublePea", gameBoard.GetPositonAt(i, j)));
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
            gm.AddDetector<Tap>(new TapDetector());
            this.Components.Add(gm);
            IGestureDispatcher dp = DefaultGestureHandlingFactory.Instance.CreateDispatcher();
            dp.AddTarget<FreeTap>(this);
            gm.AddDispatcher(dp);

            this.uiControlManager = new UIControlManager(this.Game, DefaultGestureHandlingFactory.Instance);
            gm.AddDispatcher(this.uiControlManager);
            this.Components.Add(this.uiControlManager);

            this.growSystem = new PvZGrowSystem(this.Game, new PvZGameGrow(gameBoard));
            this.growSystem.Deserialize(XmlSerialization.Instance.Deserialize(new FileStream(@"Xml\PlantGrowButtons.xml", FileMode.Open, FileAccess.Read)));
            var chooseSys = new PvZChooseSystem(this.Game, this.growSystem.ButtonFactoryBank, this.uiControlManager);
            chooseSys.Initialize();
            chooseSys.OnCameOut += this.OnChooseSystemCompleted;
            chooseSys.ComeIn();
            this.Components.Add(chooseSys);
        }

        private void OnChooseSystemCompleted(PvZChooseSystem chooseSys)
        {
            var growList = chooseSys.MakeGrowList();
            this.uiControlManager.Add(growList);

            chooseSys.RemoveAll();
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
