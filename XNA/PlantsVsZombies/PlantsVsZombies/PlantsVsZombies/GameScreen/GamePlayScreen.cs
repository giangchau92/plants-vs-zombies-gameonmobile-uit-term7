using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.GameCore.Level;
using PlantsVsZombies.GameCore.MessageCenter;
using PlantsVsZombies.GrowSystem;
using SCSEngine.Audio;
using SCSEngine.Control;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Detectors;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.Mathematics;
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
        private readonly string[] backgroundSongNames = { "Sounds/Horns", "Sounds/Patriot", "Sounds/Vampires", "Sounds/Wind stalker" };
        private readonly string[] backgroundSoundEffectNames = { "Sounds/Groan", "Sounds/LowFly" };

        private enum PlayState
        {
            START,
            SELECT_LEVEL,
            RUNNING
        }

        PZObjectManager objectManager = PZObjectManager.Instance;
        PZBoard gameBoard;
        Level level;
        PvZSunSystem _sunSystem;
        IGestureManager gm;
        PlayBackground playBacground;
        MessageCenter _messageCenter;

        // UI
        private UIControlManager uiControlManager;
        private IGestureDispatcher dispatcher;
        private PvZGrowSystem growSystem;
        private IPvZGameGrow gameGrow;

        // Sounds
        private double songTimer = 0;

        private PlayState state = PlayState.START;

        public GamePlayScreen(IGameScreenManager screenManager, IGestureManager gm, PvZGrowSystem growSys)
            : base(screenManager, "PlayScreen")
        {
            gameBoard = new PZBoard(9, 4, objectManager);
            gameBoard.Board = new int[,] {
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            this.gm = gm;
            level = PZLevelManager.Instance.GetLevel();
            level.OnBeginWave += level_OnBeginWave;
            this.playBacground = new PlayBackground(this.Game, SCSServices.Instance.ResourceManager.GetResource<Texture2D>(level.Background));
            this.playBacground.Initialize();
            this.playBacground.OnAnimatingCompleted += this.OnBackgroundAnimatingCompleted;
            this.playBacground.StartAnimate();

            _messageCenter = new MessageCenter(this.Game);

            this.growSystem = growSys;
        }

        public override void Initialize()
        {
            this.PlaySong();

            base.Initialize();
        }

        private void level_OnBeginWave(int currentWave, bool isFinalWave)
        {
            if (isFinalWave)
                _messageCenter.PushMessage("FINAL WAVE!");
        }

        private void OnBackgroundAnimatingCompleted(PlayBackground background)
        {
            this.state = PlayState.SELECT_LEVEL;
            this.InitGamePlay();
        }

        private void InitGamePlay()
        {
            PZObjectManager.Instance.RemoveAllObject();
            
            this.dispatcher = DefaultGestureHandlingFactory.Instance.CreateDispatcher();
            gm.AddDispatcher(this.dispatcher);

            this.uiControlManager = new UIControlManager(this.Game, DefaultGestureHandlingFactory.Instance);
            gm.AddDispatcher(this.uiControlManager);
            this.Components.Add(this.uiControlManager);

            //
            // Sun system
            _sunSystem = new PvZSunSystem(this.Game, 50, this.dispatcher);
            SCSServices.Instance.Game.Services.RemoveService(typeof(PvZSunSystem));
            SCSServices.Instance.Game.Services.AddService(typeof(PvZSunSystem), _sunSystem);
            this.Components.Add(_sunSystem);

            this.gameGrow = new PvZGameGrow(this.gameBoard);

            var chooseSys = new PvZChooseSystem(this.Game, this.growSystem.ButtonFactoryBank, this.uiControlManager, _sunSystem, level.NumberOfPlants);
            chooseSys.Initialize();
            chooseSys.OnCameOut += this.OnChooseSystemCompleted;
            this.Components.Add(chooseSys);
            chooseSys.ComeIn();

        }

        private void OnChooseSystemCompleted(PvZChooseSystem chooseSys)
        {
            var growList = chooseSys.MakeGrowList(this.gameGrow);
            this.uiControlManager.Add(growList);
            chooseSys.RemoveAll();

            PvZGrowButton shovel = this.growSystem.ButtonFactoryBank["Shovel"].CreateButton(this.Game);
            shovel.Canvas.Bound.Position = new Vector2(480, growList.Canvas.Bound.Top);
            shovel.Canvas.Bound.Size = new Vector2(52, 56);

            state = PlayState.RUNNING;
            _messageCenter.PushMessage(level.Name);
        }

        public void PlaySong()
        {
            Song backgroundSong = SCSServices.Instance.ResourceManager.GetResource<Song>(backgroundSongNames[GRandom.RandomInt(backgroundSongNames.Length)]);
            songTimer = backgroundSong.Duration.TotalMilliseconds;
            SCSServices.Instance.AudioManager.PlaySong(backgroundSong, false, true);
        }

        public void PlaySoundEffects()
        {
            Sound backgroundSE = SCSServices.Instance.ResourceManager.GetResource<Sound>(backgroundSoundEffectNames[GRandom.RandomInt(backgroundSoundEffectNames.Length)]);
            SCSServices.Instance.AudioManager.PlaySound(backgroundSE, false, true);
        }

        public override void Update(GameTime gameTime)
        {
            _messageCenter.Update(gameTime);
            this.playBacground.Update(gameTime);

            if (this.state == PlayState.RUNNING)
            {
                level.Update(gameBoard, gameTime);
//
                if (isWin())
                {
                    Sound winSound = SCSServices.Instance.ResourceManager.GetResource<Sound>("Sounds/WinGame");
                    SCSServices.Instance.AudioManager.PlaySound(winSound, false, true);
                    PZLevelManager.Instance.UnlockLevel();
                    var winGame = (MessageGameScreen)this.Manager.Bank.GetNewScreen("WinGame");
                    winGame.OnScreenCompleted += this.TryToNextLevel;
                    this.Manager.AddExclusive(winGame);
                }

                if (isLose())
                {
                    Sound loseSound = SCSServices.Instance.ResourceManager.GetResource<Sound>("Sounds/LoseGame");
                    SCSServices.Instance.AudioManager.PlaySound(loseSound, false, true);
                    Sound backgroundSE = SCSServices.Instance.ResourceManager.GetResource<Sound>(backgroundSoundEffectNames[GRandom.RandomInt(backgroundSoundEffectNames.Length)]);
                    SCSServices.Instance.AudioManager.PlaySound(backgroundSE, false, true);
                    PZLevelManager.Instance.ResetGame();
                    var loseGame = (MessageGameScreen)this.Manager.Bank.GetNewScreen("LoseGame");
                    loseGame.OnScreenCompleted += this.OnGameLost;
                    this.Manager.AddExclusive(loseGame);
                }

                // Update game
                IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_UPDATE, this);
                updateMessage.DestinationObjectId = 0; // For all object

                objectManager.SendMessage(updateMessage, gameTime);
            }

            songTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (songTimer <= 0.0)
            {
                this.PlaySong();
            }

            if (GRandom.RandomLogic(0.001))
            {
                this.PlaySoundEffects();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (this.state == PlayState.RUNNING)
                {
                    this.ShowConfirmDialog();
                }
                else
                {
                    this.Manager.RemoveCurrent();
                }
            }

            base.Update(gameTime);
        }

        private void OnGameLost(MessageGameScreen background)
        {
            this.Manager.AddExclusive(this.Manager.Bank.GetScreen("MainMenu"));
        }

        private void TryToNextLevel(MessageGameScreen background)
        {
            if (PZLevelManager.Instance.IsLevelAvailable)
            {
                this.Manager.AddExclusive(this.Manager.Bank.GetNewScreen("PlayScreen"));
            }
            else
            {
                PZLevelManager.Instance.ResetGame();
                this.Manager.AddExclusive(this.Manager.Bank.GetScreen("MainMenu"));
            }
        }

        private void ShowConfirmDialog()
        {
            this.Manager.AddPopup(this.Manager.Bank.GetScreen("Option"));
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
            _messageCenter.Draw(gameTime);
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

        protected override void OnStateChanged()
        {
            switch (this.State)
            {
                case GameScreenState.Activated:
                    if (this.dispatcher != null)
                    {
                        this.dispatcher.Enabled = true;
                    }
                    if (this.uiControlManager != null)
                    {
                        this.uiControlManager.Enabled = true;
                    }
                    this.PlaySong();
                    break;
                case GameScreenState.Deactivated:
                case GameScreenState.Paused:
                    if (this.dispatcher != null)
                    {
                        this.dispatcher.Enabled = false;
                    }
                    if (this.uiControlManager != null)
                    {
                        this.uiControlManager.Enabled = false;
                    }
                    SCSServices.Instance.AudioManager.StopSong();
                    break;
            }

            base.OnStateChanged();
        }
    }
}
