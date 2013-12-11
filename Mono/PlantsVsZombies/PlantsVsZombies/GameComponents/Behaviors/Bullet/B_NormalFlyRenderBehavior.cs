using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlantsVsZombies.GameComponents.Components;
using SCSEngine.Services;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.Behaviors.Bullet
{
    public class B_NormalFlyRenderBehavior : BaseBehavior
    {
        public Texture2D texture = null;

        public B_NormalFlyRenderBehavior()
        {
            texture = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("bullet");
        }

        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            MoveComponent moveCom = Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;

            if (moveCom == null)
                throw new Exception("B_NormalFlyRenderBehavior: Move Components not exist!");
            spriteBatch.Draw(texture, moveCom.Position, Color.White);

            base.Update(message, gameTime);
        }
    }
}
