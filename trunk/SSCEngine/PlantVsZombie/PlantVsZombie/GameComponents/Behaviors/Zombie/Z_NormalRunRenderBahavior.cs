using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using PlantVsZombie.GameComponents.Components;
using SSCEngine.Utils.GameObject.Component;

namespace PlantVsZombie.GameComponents.Behaviors.Zombie
{
    public class Z_NormalRunRenderBahavior : BaseBehavior
    {
        Texture2D texture = null;
        public Z_NormalRunRenderBahavior()
        {
            texture = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("zombie_run");
        }

        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            MoveComponent moveCom = Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;

            if (moveCom == null)
                throw new Exception("NormalRunRenderBahavior: Move Components not exist!");
            spriteBatch.Draw(texture, moveCom.Position, Color.White);

            base.Update(message, gameTime);
        }
    }
}
