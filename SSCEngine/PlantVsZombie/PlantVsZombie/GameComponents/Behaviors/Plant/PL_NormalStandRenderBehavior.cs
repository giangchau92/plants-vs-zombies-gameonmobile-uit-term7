using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using PlantVsZombie.GameComponents.Components;
using SSCEngine.Utils.GameObject.Component;
using SCSEngine.Sprite;
using SCSEngine.Services.Sprite;

namespace PlantVsZombie.GameComponents.Behaviors.Plant
{
    public class PL_NormalStandRenderBehavior : BaseBehavior
    {
        public Texture2D texture = null;
        public ISprite sprite = null;

        public PL_NormalStandRenderBehavior()
        {
            texture = SCSServices.Instance.ResourceManager.GetResource<Texture2D>("plant_stand");
            sprite = SCSServices.Instance.ResourceManager.GetResource<ISprite>("DoublePea");
            sprite.Play();
        }

        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {
            //SpriteBatch spriteBatch = SCSServices.Instance.SpriteBatch;
            MoveComponent moveCom = Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;

            if (moveCom == null)
                throw new Exception("NormalStandRenderBehaviorP: Move Components not exist!");
            //spriteBatch.Draw(texture, moveCom.Position, Color.White);
            sprite.TimeStep(gameTime);
            SCSServices.Instance.SpritePlayer.Draw(sprite, moveCom.Position, Color.White);

            base.Update(message, gameTime);
        }
    }
}
