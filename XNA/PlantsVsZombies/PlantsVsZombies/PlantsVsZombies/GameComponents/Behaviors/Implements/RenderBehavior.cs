using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameCore;
using SCSEngine.Services;
using SCSEngine.Services.Sprite;
using SCSEngine.Sprite;
using SCSEngine.Sprite.Implements;
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

        public Rectangle SpriteBound
        {
            get
            {
                return new Rectangle(0, 0, _sprite.CurrentFrame.Width, _sprite.CurrentFrame.Height);
            }
        }

        public Vector2 FootPositon { get; set; }

        public Color Color { get; set; }

        public RenderBehavior()
            : base()
        {
            Sprite = null;
            this.Color = new Color(255, 255, 255, 255);
        }

        public override void Update(IMessage<MessageType> message, Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpritePlayer spritePlayer = SCSServices.Instance.SpritePlayer;
            PhysicComponent moveCom = Owner.Owner.GetComponent(typeof(PhysicComponent)) as PhysicComponent;

            if (moveCom == null)
                throw new Exception("RenderBehavior: Move Components not exist!");


            Sprite.TimeStep(gameTime);
            Vector2 drawPos = new Vector2(moveCom.Frame.X, moveCom.Frame.Y + moveCom.Frame.Height - SpriteBound.Height + PZBoard.CELL_HEIGHT - 90);//FootPositon.Y - tam thoi de 90

            //spritePlayer.Draw(Sprite, new Vector2(moveCom.Frame.X, moveCom.Frame.Y), Color.White);
            spritePlayer.Draw(Sprite, drawPos, this.Color);

            base.Update(message, gameTime);
        }

        public override void OnLoad()
        {
            PhysicComponent phyCom = this.Owner.Owner.GetComponent(typeof(PhysicComponent)) as PhysicComponent;
            if (phyCom == null)
                throw new Exception("RenderBehavior: Physic Component not exist!");
            phyCom.Bound = SpriteBound;
            phyCom.Bound = new Rectangle(0, 0, PZBoard.CELL_WIDTH, PZBoard.CELL_HEIGHT);
            if (Sprite == null)
                throw new Exception("RenderBehavior: Sprite null exception!");
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
            renderBehavior.Sprite = new Sprite(Sprite.SpriteData);
            renderBehavior.Sprite.TimeDelay = Sprite.TimeDelay;
            renderBehavior.FootPositon = FootPositon;
            return renderBehavior;
        }
    }
}
