using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using System.Threading.Tasks;

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

        public MainMenuScreen(IGameScreenManager screenManager, IGestureManager gMan)
            : base(screenManager)
        {
            this.gestureManager = gMan;
        }

        public override void Initialize()
        {
            SetSceneComponents();

            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
            this.Manager.AddExclusive(this.Manager.Bank.GetScreen("PlayScreen"));
        }

        private void option_Clicked(Button button)
        {
        }

        private void help_Clicked(Button button)
        {
        }

        private void quit_Clicked(Button button)
        {
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
    }
}
