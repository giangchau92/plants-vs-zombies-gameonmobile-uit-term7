using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using Microsoft.Xna.Framework;

namespace PlantVsZombie.GameComponents.Components
{
    public class RenderComponent : IComponent<MessageType>
    {
        public Texture2D currentTexture { get; set; }

        public RenderComponent()
        {
            currentTexture = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("zombie_stand");
            int a;
            a = 1;
        }

        public IEntity<MessageType> Owner
        {
            get;
            set;
        }

        public void OnMessage(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (message.MessageType)
            {
                case MessageType.FRAME_DRAW:
                    draw();
                    break;
                default:
                    break;
            }
        }

        private void draw()
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            MoveComponent moveCom = Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
            if (moveCom == null)
                throw new Exception("Render Com: Move Components not exist!");
            spriteBatch.Draw(currentTexture, moveCom.Position, Color.White);
        }
    }
}
