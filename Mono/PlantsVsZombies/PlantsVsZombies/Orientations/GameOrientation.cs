using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.Orientations
{
    public class GameOrientation
    {
        private const int GameWidth = 800, GameHeight = 480;

        private GameOrientation()
        {
            this.drawPos = new Vector2(GameHeight, 0f);
            this.drawRot = MathHelper.PiOver2;
        }

        public static GameOrientation Instance { get; private set; }

        static GameOrientation()
        {
            Instance = new GameOrientation();
        }

        public void InitRenderTarget(GraphicsDevice gd)
        {
            this.landscapeRT = new RenderTarget2D(gd, GameWidth, GameHeight);
        }

        private RenderTarget2D landscapeRT;
        private Vector2 drawPos;
        private float drawRot;

        public void BeginDraw(SpriteBatch sprBatch)
        {
            sprBatch.GraphicsDevice.SetRenderTarget(this.landscapeRT);
        }

        public void EndDraw(SpriteBatch sprBatch)
        {
            sprBatch.GraphicsDevice.SetRenderTarget(null);

            sprBatch.Begin();
            sprBatch.Draw(this.landscapeRT, this.drawPos, null, Color.White, this.drawRot, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            sprBatch.End();
        }

        public void OnOrientationChanged(object sender, OrientationChangedEventArgs args)
        {
            switch (args.Orientation)
            {
                case PageOrientation.LandscapeLeft:
                    this.drawPos = new Vector2(GameHeight, 0f);
                    this.drawRot = MathHelper.PiOver2;
                    break;
                case PageOrientation.LandscapeRight:
                    this.drawPos = new Vector2(0f, GameWidth);
                    this.drawRot = -MathHelper.PiOver2;
                    break;
            }
        }
    }
}
