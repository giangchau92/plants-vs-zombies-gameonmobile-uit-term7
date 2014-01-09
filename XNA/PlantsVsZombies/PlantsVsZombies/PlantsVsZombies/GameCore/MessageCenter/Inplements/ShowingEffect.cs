using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameCore.MessageCenter.Inplements
{
    class ShowingEffect : IMessageEffect
    {
        public Vector2 Positon { get; set; }
        public float Scale { get; set; }
        public int alpha { get; set; }
        public string _content;
        private MessageCenter _owner;
        private float ratio = 1;
        private float _percent = 0; // 0 -> 1

        public SpriteFont Font { get; set; }

        public ShowingEffect(MessageCenter ower)
        {
            _owner = ower;
            Font = SCSServices.Instance.DebugFont;
        }
        

        public void Update(GameTime gameTime)
        {
            if (_owner.State == EffectState.SHOWING)
            {
                _percent += (float)(ratio * gameTime.ElapsedGameTime.TotalSeconds);
                alpha = (int)(1 * _percent);
                Scale = (float)(1 * _percent);
                Debug.WriteLine("ABC: " + _percent.ToString());
                if (_percent >= 1)
                    _owner.State = EffectState.SHOWED;
            }
            
        }

        public void Draw(GameTime gameTime)
        {
            if (_owner.State == EffectState.SHOWING || _owner.State == EffectState.SHOWED)
            {
                SCSServices.Instance.SpriteBatch.DrawString(Font, _content, Positon, Color.Red, 0, new Vector2(0.5f, 0.5f), Scale, SpriteEffects.None, 0);
                Debug.WriteLine(Scale.ToString());
            }
        }

        public void Reset(string message)
        {
            _content = message;
            Scale = 0;
            alpha = 0;
            _percent = 0;
            // Calc position string
            Vector2 size = Font.MeasureString(_content);
            Viewport view = SCSServices.Instance.Game.GraphicsDevice.Viewport;

            Positon = new Vector2(view.Width / 2 - size.X / 2, view.Height / 2 - size.Y / 2);
        }
    }
}
