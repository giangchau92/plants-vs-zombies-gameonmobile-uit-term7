using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Components;
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
using SCSEngine.Sprite;
using SCSEngine.Utils.GameObject.Component;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PlantsVsZombies.GameScreen
{
    public class GamePlayScreen : BaseGameScreen
    {
        private enum PlayState
        {
            START,
            RUNNING
        }

        PZObjectManager objectManager = PZObjectManager.Instance;
        PZBoard gameBoard;
        Level level;
        PvZSunSystem _sunSystem;
        IGestureManager gm;
        PlayBackground playBacground;

        // UI
        private UIControlManager uiControlManager;
        private PvZGrowSystem growSystem;

        private PlayState state = PlayState.START;

        public GamePlayScreen(IGameScreenManager screenManager, IGestureManager gm)
            : base(screenManager)
        {
            gameBoard = new PZBoard(9, 4, objectManager);
            gameBoard.Board = new int[,] {
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            this.gm = gm;

            this.playBacground = new PlayBackground(this.Game, SCSServices.Instance.ResourceManager.GetResource<Texture2D>(@"Images\Controls\Background_Forest"));
            this.playBacground.Initialize();
            this.playBacground.OnAnimatingCompleted += this.OnBackgroundAnimatingCompleted;
            this.playBacground.StartAnimate();
        }

        private void OnBackgroundAnimatingCompleted(PlayBackground background)
        {
            this.state = PlayState.RUNNING;
            this.InitGamePlay();
        }

        private void InitGamePlay()
        {
            level = PZLevelManager.Instance.GetLevel(0);
            
            IGestureDispatcher dp = DefaultGestureHandlingFactory.Instance.CreateDispatcher();
            gm.AddDispatcher(dp);

            this.uiControlManager = new UIControlManager(this.Game, DefaultGestureHandlingFactory.Instance);
            gm.AddDispatcher(this.uiControlManager);
            this.Components.Add(this.uiControlManager);

            //
            // Sun system
            _sunSystem = new PvZSunSystem(this.Game, 50, dp);
            SCSServices.Instance.Game.Services.RemoveService(typeof(PvZSunSystem));
            SCSServices.Instance.Game.Services.AddService(typeof(PvZSunSystem), _sunSystem);
            this.Components.Add(_sunSystem);

            this.growSystem = new PvZGrowSystem(this.Game, new PvZGameGrow(gameBoard));
            this.growSystem.Deserialize(XmlSerialization.Instance.Deserialize(new FileStream(@"Xml\PlantGrowButtons.xml", FileMode.Open, FileAccess.Read)));
            var chooseSys = new PvZChooseSystem(this.Game, this.growSystem.ButtonFactoryBank, this.uiControlManager, _sunSystem);
            chooseSys.Initialize();
            chooseSys.OnCameOut += this.OnChooseSystemCompleted;
            this.Components.Add(chooseSys);
            chooseSys.ComeIn();
        }

        private void OnChooseSystemCompleted(PvZChooseSystem chooseSys)
        {
            var growList = chooseSys.MakeGrowList();
            this.uiControlManager.Add(growList);

            chooseSys.RemoveAll();
        }

        public override void Update(GameTime gameTime)
        {
            this.playBacground.Update(gameTime);

            if (this.state == PlayState.RUNNING)
            {
                level.Update(gameBoard, gameTime);
//
            level.Update(gameBoard, gameTime);
            if (isWin())
            {
                Debug.WriteLine("WIN CMNR!");
            }

            if (isLose())
            {
                Debug.WriteLine("LOSE CMNR!");
            }

                // Update game
                IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_UPDATE, this);
                updateMessage.DestinationObjectId = 0; // For all object

                objectManager.SendMessage(updateMessage, gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            spriteBatch.Begin();
            this.playBacground.Draw(gameTime);
            if (this.state == PlayState.RUNNING)
            {
                IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_DRAW, this);
                updateMessage.DestinationObjectId = 0; // For every object
                objectManager.SendMessage(updateMessage, gameTime);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }


        private bool isWin()
        {
            if (level.LevelState == LevelState.END)
            {
                IDictionary<ulong, ObjectEntity> list = objectManager.GetObjects();
                foreach (var item in list)
                {
                    if (item.Value.ObjectType == eObjectType.ZOMBIE)
                        return false;
                }
                return true;
            }
            return false;
        }

        private bool isLose()
        {
            IDictionary<ulong, ObjectEntity> list = objectManager.GetObjects();
            foreach (var item in list)
            {
                if (item.Value.ObjectType == eObjectType.ZOMBIE)
                {
                    PhysicComponent moveCOm = item.Value.GetComponent(typeof(PhysicComponent)) as PhysicComponent;
                    if (moveCOm.Frame.Right < 0)
                        return true;
                }
            }
            return false;
        }
    }
}
