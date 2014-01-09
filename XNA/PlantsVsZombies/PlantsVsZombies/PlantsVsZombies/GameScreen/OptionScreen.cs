using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SCSEngine.Audio;
using SCSEngine.Control;
using SCSEngine.GestureHandling;
using SCSEngine.ScreenManagement;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameScreen
{
    class OptionScreenFactory : IGameScreenFactory
    {
        private IGameScreenManager screenMan;
        private IGestureManager gesMan;

        public OptionScreenFactory(IGameScreenManager sMan, IGestureManager gMan)
        {
            this.screenMan = sMan;
            this.gesMan = gMan;
        }

        public IGameScreen CreateGameScreen()
        {
            var option = new OptionScreen(screenMan, gesMan);
            option.Initialize();

            return option;
        }
    }

    public class OptionScreen : BaseGameScreen
    {
        private const string BUTTON_TOUCHED_OK_SOUNDNAME = "Sounds/ButtonClick";
        private readonly Vector2 BACKGROUND_POSITION = new Vector2(130f, 35f);

        // Background of MainMenu
        protected Texture2D background;
        // List of button
        protected Button Yes;
        protected Button No;
        private Sound buttonTouchedSound;
        
        private SpriteBatch spriteBatch;
        private UIControlManager uiManager;

        private IGestureManager gm;

        public OptionScreen(IGameScreenManager sm, IGestureManager gm)
            : base(sm, "Option")
        {
            this.gm = gm;
        }

        public override void Initialize()
        {
            SetSceneComponents();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Manager.RemoveCurrent();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            this.spriteBatch.Draw(this.background, BACKGROUND_POSITION, Color.White);

            base.Draw(gameTime);
            spriteBatch.End();
        }

        // Button event do
        protected void SetSceneComponents()
        {
            this.spriteBatch = SCSServices.Instance.SpriteBatch;
            this.background = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\NewGame");
            this.buttonTouchedSound = SCSServices.Instance.ResourceManager.GetResource<Sound>(BUTTON_TOUCHED_OK_SOUNDNAME);

            this.uiManager = new UIControlManager(this.Game, DefaultGestureHandlingFactory.Instance);
            gm.AddDispatcher(this.uiManager);
            this.Components.Add(this.uiManager);

            this.Yes = new Button(this.Game, spriteBatch,
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\Yes"),
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\YesOver"));
            this.Yes.FitSizeByImage();
            this.Yes.Canvas.Bound.Position = new Vector2(130f, 310f);
            this.Yes.OnTouched += this.Yes_Clicked;
            this.uiManager.Add(Yes);

            this.No = new Button(this.Game, spriteBatch,
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\No"),
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Images\\Controls\\NoOver"));
            this.No.FitSizeByImage();
            this.No.Canvas.Bound.Position = new Vector2(440f, 310f);
            this.No.OnTouched += this.No_Clicked;
            uiManager.Add(No);
        }

        void No_Clicked(Button button)
        {
            SCSServices.Instance.AudioManager.PlaySound(this.buttonTouchedSound, false, true);
            this.Manager.RemoveScreen(this.Manager.Bank.GetScreen("PlayScreen"));
            this.Manager.AddExclusive(this.Manager.Bank.GetScreen("MainMenu"));
        }

        void Yes_Clicked(Button button)
        {
            SCSServices.Instance.AudioManager.PlaySound(this.buttonTouchedSound, false, true);
            this.Manager.RemoveCurrent();
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
