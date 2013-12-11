using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using PlantsVsZombies.GameComponents.Components;
using SCSEngine.Utils.GameObject.Component;

namespace PlantsVsZombies.GameComponents.Behaviors.Plant
{
    public class PL_NormalShootRenderBehavior : BaseBehavior
    {
        public Texture2D texture = null;

        public PL_NormalShootRenderBehavior()
        {
            texture = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("plant_shoot");
        }

        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            MoveComponent moveCom = Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;

            if (moveCom == null)
                throw new Exception("NormalShootRenderBehavior: Move Components not exist!");
            spriteBatch.Draw(texture, moveCom.Position, Color.White);

            base.Update(message, gameTime);
        }
    }
}
