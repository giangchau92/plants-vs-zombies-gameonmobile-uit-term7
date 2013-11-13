using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using PlantVsZombie.GameComponents.Components;

namespace PlantVsZombie.GameComponents.Behaviors.Plant
{
    public class PL_NormalStandRenderBehavior : BaseBehavior
    {
        public Texture2D texture = null;

        public PL_NormalStandRenderBehavior()
        {
            texture = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("plant_stand");
        }

        public override void Update(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            MoveComponent moveCom = Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;

            if (moveCom == null)
                throw new Exception("NormalStandRenderBehaviorP: Move Components not exist!");
            spriteBatch.Draw(texture, moveCom.Position, Color.White);

            base.Update(gameTime);
        }
    }
}
