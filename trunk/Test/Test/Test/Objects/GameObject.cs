using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Test.Component;

namespace Test.Objects
{
    public class GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Rectangle Bound { get; set; }
        public Rectangle Frame { get; set; }
        public bool Alive { get; set; }

        private ILogicsComponent _logicsComponent;
        private IDrawComponent _drawComponent;

        public GameObject(ILogicsComponent logicsCom, IDrawComponent drawCom)
        {
            _logicsComponent = logicsCom;
            _drawComponent = drawCom;
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Bound = Rectangle.Empty;
            Frame = Rectangle.Empty;
            Alive = true;
        }

        public GameObject(ILogicsComponent logicsCom, IDrawComponent drawCom, Vector2 pos, Vector2 vel)
        {
            _logicsComponent = logicsCom;
            _drawComponent = drawCom;
            Position = pos;
            Velocity = vel;
            Bound = Rectangle.Empty;
            Frame = Rectangle.Empty;
            Alive = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!Alive)
                return;
            _logicsComponent.Update(this, gameTime);
            _drawComponent.Update(this, gameTime);

            // Update Bound
            Bound = new Rectangle((int)Position.X, (int)Position.Y,
                Frame.Width, Frame.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
                _drawComponent.Draw(spriteBatch);
        }
    }
}
