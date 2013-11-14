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
    public class Z_NormalEatingRenderBehavior : BaseBehavior
    {
        private Texture2D texture = null;

        public Z_NormalEatingRenderBehavior()
            : base()
        {
            texture = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("zombie_die");
        }

        public override void Update(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            MoveComponent moveCom = Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;

            if (moveCom == null)
                throw new Exception("Z_NormalEatingRenderBehavior: Move Components not exist!");
            spriteBatch.Draw(texture, moveCom.Position, Color.White);

            base.Update(message, gameTime);
        }
    }
}
