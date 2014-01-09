using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlantsVsZombies.GameCore.MessageCenter.Inplements;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameCore.MessageCenter
{
    public enum EffectState
    {
        READY, SHOWING, SHOWED, HIDING, HIDED
    }
    public class MessageCenter
    {
        private Game _game;
        private SpriteFont _messageFont;
        private List<String> _listMessage = new List<string>();
        private TimeSpan _currentTime;
        private TimeSpan _timeDelay = TimeSpan.FromSeconds(1);

        private EffectState _state;
        public EffectState State
        {
            get { return _state; }
            set { _state = value; }
        }

        private IMessageEffect _showingEffect;
        private IMessageEffect _hidingEffect;


        public MessageCenter(Game game)
        {
            _game = game;
            _state = EffectState.READY;
            _showingEffect = new ShowingEffect(this);
            _hidingEffect = new HidingEffect(this);
        }

        public void PushMessage(String message)
        {
            _listMessage.Add(message);
        }
        public void Update(GameTime gameTime)
        {
            if (_listMessage.Count == 0)
            {
                State = EffectState.READY;
                return;
            }
            if (State == EffectState.READY)
            {
                _showingEffect.Reset(_listMessage[0]);
                _currentTime = TimeSpan.Zero;
                State = EffectState.SHOWING;
            }
            else if (State == EffectState.SHOWED)
            {
                if (_currentTime > _timeDelay)
                {
                    _hidingEffect.Reset(_listMessage[0]);
                    State = EffectState.HIDING;
                } else
                    _currentTime += gameTime.ElapsedGameTime;
            } else if (State == EffectState.HIDED)
            {
                _listMessage.RemoveAt(0);
                State = EffectState.READY;
            } else if (State == EffectState.SHOWING)
            {
                _showingEffect.Update(gameTime);
            } else if (State == EffectState.HIDING)
            {
                _hidingEffect.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (State == EffectState.SHOWING)
            {
                _showingEffect.Draw(gameTime);
            } else if (State == EffectState.HIDING)
            {
                _hidingEffect.Draw(gameTime);
            }
        }
    }
}
