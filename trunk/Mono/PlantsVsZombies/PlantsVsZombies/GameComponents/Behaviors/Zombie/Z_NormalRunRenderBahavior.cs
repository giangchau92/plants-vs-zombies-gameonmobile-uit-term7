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

namespace PlantVsZombie.GameComponents.Behaviors.Zombie
{
    public class Z_NormalRunRenderBahavior : BaseBehavior
    {
        ISprite sprite = null;
        public Rectangle SpriteBound { get; set; }

        public Z_NormalRunRenderBahavior()
        {
            sprite = SCSServices.Instance.ResourceManager.GetResource<ISprite>("Zombies/Nameless/Walk");
            SpriteBound = new Rectangle(0, 0, 73, 100);
            sprite.Play();
        }

        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {
            SpritePlayer spritePlayer = SCSServices.Instance.SpritePlayer;
            PhysicComponent moveCom = Owner.Owner.GetComponent(typeof(PhysicComponent)) as PhysicComponent;

            if (moveCom == null)
                throw new Exception("NormalRunRenderBahavior: Move Components not exist!");

            sprite.TimeStep(gameTime);
            spritePlayer.Draw(sprite, new Vector2(moveCom.Frame.X, moveCom.Frame.Y), Color.White);

            base.Update(message, gameTime);
        }

        public override void OnLoad()
        {
            PhysicComponent phyCom = this.Owner.Owner.GetComponent(typeof(PhysicComponent)) as PhysicComponent;
            phyCom.Bound = SpriteBound;
            base.OnLoad();
        }
    }
}
