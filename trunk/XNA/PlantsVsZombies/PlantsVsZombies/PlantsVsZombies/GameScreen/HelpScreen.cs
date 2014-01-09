using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    class HelpScreenFactory : IGameScreenFactory
    {
        private IGameScreenManager screenMan;
        private IGestureManager gesMan;

        public HelpScreenFactory(IGameScreenManager sMan, IGestureManager gMan)
        {
            this.screenMan = sMan;
            this.gesMan = gMan;
        }

        public IGameScreen CreateGameScreen()
        {
            var Help = new HelpScreen(screenMan, gesMan);
            Help.Initialize();

            return Help;
        }
    }
    class HelpScreen : BaseGameScreen
    {
        private const string BUTTON_TOUCHED_OK_SOUNDNAME = "Sounds/ButtonClick";

        // Background of MainMenu
        protected List<Texture2D> backgrounds;
        protected Texture2D currentTexture;
        private Sound buttonTouchedSound;
        // List of button
        protected Button back;
        protected Button next;
        // current page
        protected int currentPage;

        private SpriteBatch spriteBatch;

        private IGestureManager gm;
        private UIControlManager uiManager;

        public HelpScreen(IGameScreenManager sm, IGestureManager gm)
            : base(sm, "Help")
        {
            this.gm = gm;
        }

        public override void Initialize()
        {
            backgrounds = new List<Texture2D>();
            SetSceneComponents();
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(this.currentTexture, Vector2.Zero, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Button event do
        protected void SetSceneComponents()
        {
            this.spriteBatch = SCSServices.Instance.SpriteBatch;

            this.backgrounds.Add(SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\HelpScene_1"));
            this.backgrounds.Add(SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\HelpScene_2"));
            
            this.buttonTouchedSound = SCSServices.Instance.ResourceManager.GetResource<Sound>(BUTTON_TOUCHED_OK_SOUNDNAME);

            this.currentTexture = backgrounds[currentPage];
            
            this.uiManager = new UIControlManager(this.Game, DefaultGestureHandlingFactory.Instance);
            gm.AddDispatcher(this.uiManager);
            this.Components.Add(this.uiManager);

            this.next = new Button(this.Game, spriteBatch,
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\Next"),
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\NextOver"));
            this.next.FitSizeByImage();
            this.next.Canvas.Bound.Position = new Vector2(710f, 450f);
            this.next.OnTouched += this.next_Clicked;
            this.uiManager.Add(next);

            this.back = new Button(this.Game,  spriteBatch,
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\Back"),
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\BackOver"));
            this.back.FitSizeByImage();
            this.back.Canvas.Bound.Position = new Vector2(0f, 450f);
            this.back.OnTouched += this.back_Clicked;
            this.uiManager.Add(back);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Manager.RemoveCurrent();
            }
        }

        protected void back_Clicked(Button button)
        {
            if (currentPage > 0)
                currentPage--;
            else
                currentPage = this.backgrounds.Count - 1;

            ChangeBackground();
            SCSServices.Instance.AudioManager.PlaySound(this.buttonTouchedSound, false, true);
        }

        protected void next_Clicked(Button button)
        {
            if (currentPage < backgrounds.Count - 1)
                currentPage++;
            else
                currentPage = 0;

            ChangeBackground();
            SCSServices.Instance.AudioManager.PlaySound(this.buttonTouchedSound, false, true);
        }

        protected void ChangeBackground()
        {
            this.currentTexture = backgrounds[currentPage];
        }

        protected override void OnStateChanged()
        {
            switch (this.State)
            {
                case GameScreenState.Activated:
                    this.uiManager.Enabled = true;
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
