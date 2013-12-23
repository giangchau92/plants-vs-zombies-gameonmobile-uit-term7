using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Components;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.Behaviors.Implements
{
    public class RenderBehavior : BaseBehavior
    {
        private ISprite _sprite = null;
        public ISprite Sprite
        {
            get{ return _sprite; }
            set
            {
                _sprite = value;
                if (_sprite != null)
                    _sprite.Play();
            }
        }

        public Rectangle SpriteBound { get; set; }

        public RenderBehavior()
            : base()
        {
            Sprite = null;
            SpriteBound = Rectangle.Empty;
        }

        public override void Update(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpritePlayer spritePlayer = SCSServices.Instance.SpritePlayer;
            PhysicComponent moveCom = Owner.Owner.GetComponent(typeof(PhysicComponent)) as PhysicComponent;

            if (moveCom == null)
                throw new Exception("Z_ZombieRenderBehavior: Move Components not exist!");

            Sprite.TimeStep(gameTime);
            spritePlayer.Draw(Sprite, new Vector2(moveCom.Frame.X, moveCom.Frame.Y), Color.White);

            base.Update(message, gameTime);
        }

        public override void OnLoad()
        {
            PhysicComponent phyCom = this.Owner.Owner.GetComponent(typeof(PhysicComponent)) as PhysicComponent;
            if (phyCom == null)
                throw new Exception("Z_ZombieRenderBehavior: Physic Component not exist!");
            phyCom.Bound = SpriteBound;
            if (Sprite == null)
                throw new Exception("Z_ZombieRenderBehavior: Sprite null exception!");
            Sprite.Play();
            base.OnLoad();
        }

        public override void UnLoad()
        {
            base.UnLoad();
        }

        public static RenderBehavior CreateBehavior()
        {
            RenderBehavior result = new RenderBehavior();
            return result;
        }

        public override IBehavior<MessageType> Clone()
        {
            RenderBehavior renderBehavior = new RenderBehavior();
            renderBehavior.Sprite = this.Sprite;
            renderBehavior.SpriteBound = this.SpriteBound;
            return renderBehavior;
        }
    }
}
