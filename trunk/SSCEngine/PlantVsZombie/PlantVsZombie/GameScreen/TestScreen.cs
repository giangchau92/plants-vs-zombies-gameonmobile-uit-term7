using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement.Implement;
using SCSEngine.ScreenManagement;
using Microsoft.Xna.Framework;
using SSCEngine.Utils.GameObject.Component;
using PlantVsZombie.GameComponents;
using PlantVsZombie.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;

namespace PlantVsZombie.GameScreen
{
    public class TestScreen : BaseGameScreen
    {
        List<ObjectEntity> listObject = new List<ObjectEntity>();

        public TestScreen(IGameScreenManager screenManager)
            : base(screenManager)
        {
            listObject.Add(new NormalZombie());
        }

        public override void Update(GameTime gameTime)
        {
            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_UPDATE);
            foreach (var obj in listObject)
            {
                obj.OnMessage(updateMessage, gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            spriteBatch.Begin();

            IMessage<MessageType> updateMessage = new GameMessage(MessageType.FRAME_DRAW);
            foreach (var obj in listObject)
            {
                obj.OnMessage(updateMessage, gameTime);
            }
            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
