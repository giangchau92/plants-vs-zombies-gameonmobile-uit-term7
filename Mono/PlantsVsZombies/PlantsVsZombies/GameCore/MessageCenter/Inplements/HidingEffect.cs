using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameCore.MessageCenter.Inplements
{
    public class HidingEffect : IMessageEffect
    {
        public Vector2 Positon { get; set; }
        public float Scale { get; set; }
        public int alpha { get; set; }
        public string _content;
        private MessageCenter _owner;
        private float ratio = 1;
        private float _percent = 0; // 0 -> 1

        public SpriteFont Font { get; set; }

        public HidingEffect(MessageCenter ower)
        {
            _owner = ower;
            Font = SCSServices.Instance.DebugFont;
        }
        

        public void Update(GameTime gameTime)
        {
            if (_owner.State == EffectState.HIDING)
            {
                _percent += (float)(ratio * gameTime.ElapsedGameTime.TotalSeconds);
                alpha = (int)(1 * _percent);
                Scale = (int)(1 * _percent);

                if (_percent <= 0)
                    _owner.State = EffectState.HIDED;
            }
            
        }

        public void Draw(GameTime gameTime)
        {
            if (_owner.State == EffectState.SHOWING)
            {
                SCSServices.Instance.SpriteBatch.DrawString(Font, _content, Positon, new Color(Color.Red, alpha), 0, new Vector2(0.5f, 0.5f), Scale, SpriteEffects.None, 0);
            }
        }

        public void Reset(string message)
        {
            _content = message;
            Scale = 1;
            alpha = 1;
            _percent = 1;
            // Calc position string
            Vector2 size = Font.MeasureString(_content);
            Viewport view = SCSServices.Instance.Game.GraphicsDevice.Viewport;

            Positon = new Vector2(view.Height / 2 - size.X / 2, view.Width / 2 - size.Y / 2);

        }
    }
}
