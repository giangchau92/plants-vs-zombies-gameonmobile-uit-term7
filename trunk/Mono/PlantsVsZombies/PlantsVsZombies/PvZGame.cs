using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.Orientations;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.GameObjects;
using SCSEngine.GestureHandling;
using SCSEngine.GestureHandling.Implements.Detectors;
using SCSEngine.GestureHandling.Implements.Events;
using SCSEngine.ResourceManagement;
using SCSEngine.Services;
using SCSEngine.Sprite;
using System;
using System.Diagnostics;
using SCSEngine.Control;
using PlantsVsZombies.GrowSystem;
using SCSEngine.Serialization.XmlSerialization;
using System.IO;

namespace PlantsVsZombies
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PvZGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PZScreenManager screenManager;

        

        

        public PvZGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);//166666 333333

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            this.IsFixedTimeStep = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            try
            {
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);

                // TODO: use this.Content to load your game content here
                // Add Resource Manager
                this.initSpriteBank();
                IResourceManager resourceManager = new PZResourceManager(this.Content);
                this.Services.AddService(typeof(IResourceManager), resourceManager);
                //Load sprite data

                // Level manager
                PZLevelManager.Instance.initLevel();
                //SCSService
                SCSServices.Instance.Game = this;
                SCSServices.Instance.SpriteBatch = spriteBatch;
                SCSServices.Instance.SpritePlayer = new SCSEngine.Services.Sprite.SpritePlayer(spriteBatch);
                SCSServices.Instance.AudioManager = new SCSEngine.Services.Audio.AudioManager(this);
                SCSServices.Instance.ResourceManager = resourceManager;
                SCSServices.Instance.SpritePlayer = new SCSEngine.Services.Sprite.SpritePlayer(spriteBatch);

                SpriteFont font = Content.Load<SpriteFont>("DebugFont");
                SCSServices.Instance.DebugFont = font;

                this.Services.AddService(typeof(SCSServices), SCSServices.Instance);

                //Test
                GameObjectCenter.Instance.InitEnity();

                IGestureManager gestureManager = DefaultGestureHandlingFactory.Instance.CreateManager(this, //DefaultGestureHandlingFactory.Instance.CreateTouchController());
                    new OrientedTouchController(DefaultGestureHandlingFactory.Instance.CreateTouchController(), GameOrientation.Instance));
                gestureManager.AddDetector<FreeTap>(new FreeTapDetector());
                gestureManager.AddDetector<Tap>(new TapDetector());
                this.Components.Add(gestureManager);

                screenManager = new PZScreenManager(this, gestureManager);
                screenManager.AddExclusive(screenManager.Bank.GetNewScreen("MainMenu"));

                

                GameOrientation.Instance.InitRenderTarget(this.GraphicsDevice);
            }
            catch (Exception )
            {
                //Debug.WriteLine("Load content error {0}", e);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

            try
            {

                // TODO: Add your update logic here
                screenManager.Update(gameTime);
                ////Debug.WriteLine(string.Format("Eslaped: {0}", gameTime.ElapsedGameTime.TotalMilliseconds));

                base.Update(gameTime);

                this.CheatTouch();
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Update error {0}", e);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GameOrientation.Instance.BeginDraw(this.spriteBatch);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            try
            {
                // TODO: Add your drawing code here
                screenManager.Draw(gameTime);

                this.spriteBatch.Begin();
                base.Draw(gameTime);
                this.spriteBatch.End();

                GameOrientation.Instance.EndDraw(this.spriteBatch);
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Draw error {0}", e);
            }
        }

      

        bool _firstUpdate = true;
        private void CheatTouch()
        {
            if (_firstUpdate)
            {
                // Temp hack to fix gestures
                typeof(Microsoft.Xna.Framework.Input.Touch.TouchPanel)
                    .GetField("_touchScale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .SetValue(null, Vector2.One);

                _firstUpdate = false;
            }
        }

        private void initSpriteBank()
        {
            if (SpriteFramesBank.Instance.Contains("DoublePea"))
                return;

            SpriteFramesBank.Instance.Add("Plants/DoublePea/DoublePea", FramesGenerator.Generate(100, 55, 1024, 40));
            SpriteFramesBank.Instance.Add("Plants/IcePea/IcePea", FramesGenerator.Generate(118, 63, 1024, 33));
            SpriteFramesBank.Instance.Add("Plants/Stone/Stone", FramesGenerator.Generate(101, 101, 1024, 38));
            SpriteFramesBank.Instance.Add("Plants/Sunflower/Sunflower", FramesGenerator.Generate(84, 60, 1024, 40));
            SpriteFramesBank.Instance.Add("Bullets/Sun", FramesGenerator.Generate(50, 50, 750, 15));
            SpriteFramesBank.Instance.Add("Zombies/Nameless/Walk", FramesGenerator.Generate(73, 100, 1024, 16));
            SpriteFramesBank.Instance.Add("Zombies/Nameless/Attack", FramesGenerator.Generate(89, 101, 1024, 16));
            SpriteFramesBank.Instance.Add("Zombies/Nameless/Death", FramesGenerator.Generate(155, 130, 1024, 16));
            // Histachii
            SpriteFramesBank.Instance.Add("Zombies/Histachii/Walk", FramesGenerator.Generate(53, 92, 1024, 12));
            SpriteFramesBank.Instance.Add("Zombies/Histachii/Attack", FramesGenerator.Generate(106, 91, 1024, 15));
            SpriteFramesBank.Instance.Add("Zombies/Histachii/Death", FramesGenerator.Generate(87, 113, 1024, 16));
            SpriteFramesBank.Instance.Add("Bullets/B_Pea", FramesGenerator.Generate(29, 22, 29, 1));
            //100, 55, 10, 40
        }
    }
}