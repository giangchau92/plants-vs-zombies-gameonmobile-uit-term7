using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Utils.Adapters;
using SSCEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGesture
{
    public class PvZButtonDraw : IHighlightedButtonBrush
    {
        Texture2D img;
        SpriteBatch sprBatch;

        Vector2 imgCenter;

        public PvZButtonDraw(SpriteBatch spriteBatch, Texture2D normal)
        {
            this.img = normal;
            this.imgCenter = new Vector2(img.Width / 2, img.Height / 2);

            this.sprBatch = spriteBatch;
        }

        public void DrawNormal(HighlightedButton button, GameTime gameTime)
        {
            sprBatch.Begin();
            sprBatch.Draw(img, button.Canvas.Bound.Rectangle, Color.White);
            sprBatch.End();
        }

        public void DrawHighlight(HighlightedButton button, GameTime gameTime)
        {
            CRectangleF highCanvas = new CRectangleF(button.Canvas.Bound);
            highCanvas.Position -= highCanvas.Size * 0.25f;
            highCanvas.Size *= 1.5f;
            sprBatch.Begin();
            sprBatch.Draw(img, highCanvas.Rectangle, new Color(255, 255, 255, 100));
            sprBatch.End();
        }


        public void DrawCooldown(HighlightedButton button, GameTime gameTime, float cooldownPercent)
        {
            Color c = new Color(0.5f - cooldownPercent, 0.5f - cooldownPercent, 1f);

            sprBatch.Begin();
            sprBatch.Draw(img, button.Canvas.Bound.Rectangle, c);
            sprBatch.End();
        }
    }
}
