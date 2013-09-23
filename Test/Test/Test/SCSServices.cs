using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Test
{
    public class SCSServices
    {
        private static SCSServices _instance = null;
        private SCSServices()
        {
        }

        public static SCSServices GetInstance()
        {
            if (_instance == null)
                _instance = new SCSServices();
            return _instance;
        }

        public void PreLoad(ContentManager content, Game1 game, SpriteFont font)
        {
            Content = content;
            Game = game;
            DebugFont = font;
        }

        public ContentManager Content { get; private set; }

        public Game1 Game { get; private set; }

        public SpriteFont DebugFont { get; private set; }
    }
}
