using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Audio;
using SCSEngine.ScreenManagement;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameScreen
{
    class IntroductionScreenFactory : IGameScreenFactory
    {
        private IGameScreenManager screenMan;

        public IntroductionScreenFactory(IGameScreenManager sMan)
        {
            this.screenMan = sMan;
        }

        public IGameScreen CreateGameScreen()
        {
            var Introduction = new IntroductionScreen(screenMan);
            Introduction.Initialize();

            return Introduction;
        }
    }
    class IntroductionScreen : BaseGameScreen
    {
        private const double FADE_TIME = 2000.0;
        private const string BACKGROUND_SOUNDNAME = @"Sounds\LowFly";

        private enum FadeState
        {
            FadeIn,
            FadeOut
        }

        protected Texture2D background;
        protected Color transparentIn = Color.White;
        private double alpha;
        private Sound backgroundSound;

        protected double timer;
        private FadeState state;

        private SpriteBatch spriteBatch;

        public IntroductionScreen(IGameScreenManager sm)
            : base(sm, "Introduction")
        {
        }

        public override void Initialize()
        {
            this.spriteBatch = SCSServices.Instance.SpriteBatch;

            background = SCSServices.Instance.ResourceManager.GetResource<Texture2D>(@"Images\Controls\Loading");
            alpha = 0;

            this.timer = FADE_TIME;
            this.state = FadeState.FadeIn;

            this.backgroundSound = SCSServices.Instance.ResourceManager.GetResource<Sound>(BACKGROUND_SOUNDNAME);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.state == FadeState.FadeIn)
            {
                double dA = (255.0 / FADE_TIME) * gameTime.ElapsedGameTime.TotalMilliseconds;
                alpha += dA;

                this.timer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this.timer <= 0)
                {
                    alpha = 255.0;
                    this.timer = FADE_TIME;
                    this.state = FadeState.FadeOut;
                }
            }
            else
            {
                double dA = (1.0 / FADE_TIME) * gameTime.ElapsedGameTime.TotalMilliseconds;
                alpha -= dA;

                this.timer -= gameTime.ElapsedGameTime.TotalMilliseconds;

                if (this.timer <= 0)
                {
                    alpha = 0.0;
                    this.timer = FADE_TIME;
                    this.state = FadeState.FadeIn;

                    this.ScreenCompleted();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            transparentIn.A = (byte)alpha;
            spriteBatch.Draw(this.background, Vector2.Zero, this.transparentIn);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ScreenCompleted()
        {
            this.Manager.AddExclusive(this.Manager.Bank.GetScreen("MainMenu"));
        }

        protected override void OnStateChanged()
        {
            switch (this.State)
            {
                case GameScreenState.Activated:
                    SCSServices.Instance.AudioManager.PlaySound(this.backgroundSound, false, true);
                    break;
                case GameScreenState.Deactivated:
                case GameScreenState.Paused:
                    break;
            }

            base.OnStateChanged();
        }
    }
}
