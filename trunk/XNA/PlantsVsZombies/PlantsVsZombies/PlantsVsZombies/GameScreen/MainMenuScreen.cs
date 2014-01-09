using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SCSEngine.Audio;
using SCSEngine.Control;
using SCSEngine.GestureHandling;
using SCSEngine.ScreenManagement;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.Services;
using SCSEngine.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameScreen
{
    class MainMenuScreenFactory : IGameScreenFactory
    {
        private IGameScreenManager screenMan;
        private IGestureManager gesMan;

        public MainMenuScreenFactory(IGameScreenManager sMan, IGestureManager gMan)
        {
            this.screenMan = sMan;
            this.gesMan = gMan;
        }

        public IGameScreen CreateGameScreen()
        {
            var menu = new MainMenuScreen(screenMan, gesMan);
            menu.Initialize();

            return menu;
        }
    }


    class MainMenuScreen : BaseGameScreen
    {
        private const string BUTTON_TOUCHED_OK_SOUNDNAME = "Sounds/ButtonClick";
        private const string BUTTON_TOUCHED_FAIL_SOUNDNAME = "Sounds/FailChoose";
        private const string BACKGROUND_MUSICNAME = "Sounds/Wind stalker";
        private const string EXIT_SOUNDNAME = "Sounds/goodbye";
        private const string PLAYGAME_TOUCHED_SOUNDNAME = "Sounds/ButtonMainMenu";

        // Background of MainMenu
        protected Texture2D background;
        // List of button
        protected Button start;
        protected Button option;
        protected Button help;
        protected Button quit;

        private SpriteBatch spriteBatch;
        private IGestureManager gestureManager;
        private UIControlManager uiManager;

        private Song backgroundMusic;
        private Sound buttonOKSound, buttonFailSound;
        private Sound exitGameSound, playGameSound;



        public MainMenuScreen(IGameScreenManager screenManager, IGestureManager gMan)
            : base(screenManager, "MainMenu")
        {
            this.gestureManager = gMan;
        }

        public override void Initialize()
        {
            SetSceneComponents();
            this.backgroundMusic = SCSServices.Instance.ResourceManager.GetResource<Song>(BACKGROUND_MUSICNAME);
            this.buttonOKSound = SCSServices.Instance.ResourceManager.GetResource<Sound>(BUTTON_TOUCHED_OK_SOUNDNAME);
            this.buttonFailSound = SCSServices.Instance.ResourceManager.GetResource<Sound>(BUTTON_TOUCHED_FAIL_SOUNDNAME);
            this.exitGameSound = SCSServices.Instance.ResourceManager.GetResource<Sound>(EXIT_SOUNDNAME);
            this.playGameSound = SCSServices.Instance.ResourceManager.GetResource<Sound>(PLAYGAME_TOUCHED_SOUNDNAME);

            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Manager.AddPopup(this.Manager.Bank.GetScreen("Exit"));
            }
        }

        // Button event do
        protected void SetSceneComponents()
        {
            this.uiManager = new UIControlManager(this.Game, DefaultGestureHandlingFactory.Instance);
            this.gestureManager.AddDispatcher(this.uiManager);
            this.Components.Add(this.uiManager);

            this.spriteBatch = SCSServices.Instance.SpriteBatch;
            this.background = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MainMenu");

            start = new Button(this.Game, this.spriteBatch, SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MStartGame"),
                        SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MStartGameOver"));
            start.FitSizeByImage();
            start.Canvas.Bound.Position = new Vector2(60f, 380f);
            start.OnTouched += this.start_Clicked;
            this.uiManager.Add(start);

            option = new Button(this.Game, this.spriteBatch, SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MOption"),
                        SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MOptionOver"));
            option.FitSizeByImage();
            option.Canvas.Bound.Position = new Vector2(300f, 370f);
            option.OnTouched += this.option_Clicked;
            this.uiManager.Add(option);

            help = new Button(this.Game, this.spriteBatch, SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MHelp"),
                        SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MHelpOver"));
            help.FitSizeByImage();
            help.Canvas.Bound.Position = new Vector2(435f, 415f);
            help.OnTouched += this.help_Clicked;
            this.uiManager.Add(help);

            quit = new Button(this.Game, this.spriteBatch, SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MQuitGame"),
                        SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\MQuitGameOver"));
            quit.FitSizeByImage();
            quit.Canvas.Bound.Position = new Vector2(680f, 450f);
            quit.OnTouched += this.quit_Clicked;
            this.uiManager.Add(quit);
        }

        private void start_Clicked(Button button)
        {
            SCSServices.Instance.AudioManager.PlaySound(this.playGameSound, false, true);
            this.Manager.AddExclusive(this.Manager.Bank.GetNewScreen("PlayScreen"));
        }

        private void option_Clicked(Button button)
        {
            SCSServices.Instance.AudioManager.PlaySound(this.buttonFailSound, false, true);
        }

        private void help_Clicked(Button button)
        {
            SCSServices.Instance.AudioManager.PlaySound(this.buttonFailSound, false, true);
        }

        private void quit_Clicked(Button button)
        {
            SCSServices.Instance.AudioManager.PlaySound(this.exitGameSound, false, true);
                this.Manager.AddPopup(this.Manager.Bank.GetScreen("Exit"));
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            if (this.spriteBatch != null && this.background != null)
            {
                this.spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
            }

            base.Draw(gameTime);
            this.spriteBatch.End();
        }

        protected override void OnStateChanged()
        {
            switch (this.State)
            {
                case GameScreenState.Activated:
                    this.uiManager.Enabled = true;
                    SCSServices.Instance.AudioManager.PlaySong(this.backgroundMusic, false, true);
                    break;
                case GameScreenState.Deactivated:
                case GameScreenState.Paused:
                    this.uiManager.Enabled = false;
                    SCSServices.Instance.AudioManager.StopSong();
                    break;
            }

            base.OnStateChanged();
        }
    }
}
