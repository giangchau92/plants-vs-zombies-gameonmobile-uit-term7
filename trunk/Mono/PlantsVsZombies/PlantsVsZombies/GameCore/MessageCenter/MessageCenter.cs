using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GameCore.MessageCenter
{
    public enum MessageState
    {
        SHOWING, VISIBLE, HIDING, NOTHING
    }
    public class MessageCenter
    {
        private Game _game;
        private SpriteFont _messageFont;
        private List<String> _listMessage = new List<string>();

        private MessageState _messageState;

        public MessageCenter(Game game)
        {
            _game = game;
            _messageState = MessageState.NOTHING;
        }

        public void ShowMessage(String message)
        {
            _listMessage.Add(message);
        }
        public void Update(GameTime gameTime)
        {
            if (_listMessage.Count == 0)
                return;

        }

        public void Draw(GameTime gameTime)
        {
            
        }
    }
}
