using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Control;
using SCSEngine.ScreenManagement;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace PlantsVsZombies.GameScreen
{
    public delegate void MessageGameScreenEventHandler(MessageGameScreen background);

    class MessageGameScreenFactory : IGameScreenFactory
    {
        private IGameScreenManager screenMan;
        private String backgroundName, screenName;

        public MessageGameScreenFactory(IGameScreenManager sMan, string backgroundName, string screenName)
        {
            this.screenMan = sMan;
            this.backgroundName = backgroundName;
            this.screenName = screenName;
        }

        public IGameScreen CreateGameScreen()
        {
            var msg = new MessageGameScreen(screenMan, screenName);
            msg.Background = SCSServices.Instance.ResourceManager.GetResource<Texture2D>(this.backgroundName);
            msg.Initialize();

            return msg;
        }
    }


    public class MessageGameScreen : BaseGameScreen
    {
        // Background of MainMenu
        private Texture2D background;
        public Texture2D Background
        {
            get { return background; }
            set { background = value; }
        }

        private SpriteBatch spriteBatch;
        private float delayDuration = 2f;
        private float delayTime;

        public event MessageGameScreenEventHandler OnScreenCompleted;

        public MessageGameScreen(IGameScreenManager screenManager, string name)
            : base(screenManager, name)
        {
            spriteBatch = SCSServices.Instance.SpriteBatch;
        }

        public override void Initialize()
        {
            this.delayTime = this.delayDuration;

            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            this.delayTime -= (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (this.delayTime < 0)
            {
                this.delayTime = delayDuration;
                if (this.OnScreenCompleted != null)
                {
                    this.OnScreenCompleted(this);
                }
            }

            base.Update(gameTime);
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
