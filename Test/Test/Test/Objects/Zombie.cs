using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Component.Implements;
using Test.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test.Objects
{
    public enum ZombieState
    {
        STANDING, RUNNING, DYING
    }
    public class Zombie : GameObject
    {
        public ZombieState State { get; set; }

        public int Heath { get; set; }

        public Zombie()
            : base(new ZombieLogicsComponent(), new ZombieDrawComponent())
        {
            State = ZombieState.RUNNING;
            Frame = new Rectangle(0, 0, 50, 100);
            Heath = 100;
        }

        public override void Update(GameTime gameTime)
        {
            if (Heath <= 0)
                Alive = false;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(SCSServices.GetInstance().DebugFont, Heath.ToString(), new Vector2(Position.X, Position.Y - 30), Color.White);
        }
    }
}
