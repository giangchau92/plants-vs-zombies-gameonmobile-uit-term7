using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.ServiceManagement;
using SCSEngine.Services.Audio;
using SCSEngine.Services.Input;
using SCSEngine.Services.Sprite;
using SCSEngine.ResourceManagement;
using SCSEngine.GestureHandling;

namespace SCSEngine.Services
{
    public class SCSServices
    {
        private static SCSServices _instance = null;

        public static SCSServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SCSServices();
                return _instance;
            }
        }

        public SCSServices()
        {
        //    this.Game = game;
        //    this.SpriteBatch = sprBatch;
        //    this.SpritePlayer = new SpritePlayer(this.SpriteBatch);
        //    this.AudioManager = new AudioManager(this.Game);
        //    this.InputHandle = new InputHandle();
        }

        public Game Game { get; set; }

        public IGestureManager GestureManager { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public SpritePlayer SpritePlayer { get; set; }

        public AudioManager AudioManager { get; set; }

        public InputHandle InputHandle { get; set; }

        public IResourceManager ResourceManager { get; set; }

        public SpriteFont DebugFont { get; set; }

        public GameTime GameTime { get; set; }
    }
}
