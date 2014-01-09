using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
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
        string debugString;

        public PvZGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);//166666 333333

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            this.IsFixedTimeStep = false;
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

                IGestureManager gestureManager = DefaultGestureHandlingFactory.Instance.CreateManager(this, DefaultGestureHandlingFactory.Instance.CreateTouchController());
                gestureManager.AddDetector<FreeTap>(new FreeTapDetector());
                gestureManager.AddDetector<Tap>(new TapDetector());
                this.Components.Add(gestureManager);

                screenManager = new PZScreenManager(this, gestureManager);
                screenManager.AddExclusive(screenManager.Bank.GetNewScreen("MainMenu"));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Load content error {0}", e);
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
                debugString = string.Format("Eslaped: {0}", gameTime.ElapsedGameTime.TotalMilliseconds);

                base.Update(gameTime);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Update error {0}", e);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            try
            {
                // TODO: Add your drawing code here
                screenManager.Draw(gameTime);

                this.spriteBatch.Begin();
                base.Draw(gameTime);
                spriteBatch.DrawString(SCSServices.Instance.DebugFont, string.Format("Debug: {0}", debugString), Vector2.Zero, Color.White);
                this.spriteBatch.End();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Draw error {0}", e);
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

            //Plants & Bullets
            SpriteFramesBank.Instance.Add("Images/Plants/Cherry", FramesGenerator.Generate(126, 86, 1024, 24));
            SpriteFramesBank.Instance.Add("Images/Bullets/B_Cherry", FramesGenerator.Generate(74, 80, 1024, 17));

            SpriteFramesBank.Instance.Add("Images/Plants/Chilly", FramesGenerator.Generate(78, 75, 1024, 24));
            SpriteFramesBank.Instance.Add("Images/Bullets/B_Chili", FramesGenerator.Generate(31, 44, 1024, 22));

            SpriteFramesBank.Instance.Add("Images/Plants/DoublePea", FramesGenerator.Generate(100, 1024, 10, 40));

            SpriteFramesBank.Instance.Add("Images/Plants/IcePea", FramesGenerator.Generate(118, 63, 1024, 33));
            SpriteFramesBank.Instance.Add("Images/Bullets/B_IcePea", FramesGenerator.Generate(29, 1024, 1, 1));

            SpriteFramesBank.Instance.Add("Images/Plants/Pea", FramesGenerator.Generate(92, 62, 1024, 33));
            SpriteFramesBank.Instance.Add("Images/Bullets/B_Pea", FramesGenerator.Generate(29, 1024, 1, 1));

            SpriteFramesBank.Instance.Add("Images/Plants/FreeMushroom", FramesGenerator.Generate(44, 34, 1024, 35));
            SpriteFramesBank.Instance.Add("Images/Bullets/B_FreeMush", FramesGenerator.Generate(26, 11, 1024, 1));

            SpriteFramesBank.Instance.Add("Images/Plants/SeaMushroom", FramesGenerator.Generate(58, 51, 1024, 48));
            SpriteFramesBank.Instance.Add("Images/Bullets/B_WaterMush", FramesGenerator.Generate(31, 24, 1024, 1));

            SpriteFramesBank.Instance.Add("Images/Plants/Mine", FramesGenerator.Generate(83, 47, 1024, 17));
            SpriteFramesBank.Instance.Add("Images/Plants/MineGrow", FramesGenerator.Generate(113, 1024, 9, 30));

            SpriteFramesBank.Instance.Add("Images/Plants/Stone", FramesGenerator.Generate(101, 101, 1024, 38));

            SpriteFramesBank.Instance.Add("Images/Plants/Sun", FramesGenerator.Generate(50, 50, 1024, 15));

            SpriteFramesBank.Instance.Add("Images/Plants/Sunflower", FramesGenerator.Generate(84, 60, 1024, 48));

            SpriteFramesBank.Instance.Add("Images/Plants/SunMushroom", FramesGenerator.Generate(46, 38, 1024, 26));
            SpriteFramesBank.Instance.Add("Images/Plants/SunMushroomGreater", FramesGenerator.Generate(60, 1024, 17, 26));

            SpriteFramesBank.Instance.Add("Bullets/Sun", FramesGenerator.Generate(50, 50, 750, 15));
            SpriteFramesBank.Instance.Add("Bullets/B_Pea", FramesGenerator.Generate(29, 22, 29, 1));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/BarrowWight/Attack", FramesGenerator.Generate(73, 101, 1024, 15));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/BarrowWight/Death", FramesGenerator.Generate(103, 105, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/BarrowWight/Walk", FramesGenerator.Generate(64, 81, 1024, 17));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/Skeleton/Attack", FramesGenerator.Generate(129, 102, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/Skeleton/Death", FramesGenerator.Generate(156, 134, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/Skeleton/Walk", FramesGenerator.Generate(62, 82, 1024, 16));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/Tattered/Attack", FramesGenerator.Generate(90, 105, 1024, 15));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/Tattered/Death", FramesGenerator.Generate(103, 114, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/Tattered/Walk", FramesGenerator.Generate(64, 95, 1024, 16));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/GiantSkeletonSword/Attack", FramesGenerator.Generate(131, 125, 1024, 15));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/GiantSkeletonSword/Death", FramesGenerator.Generate(139, 146, 1024, 21));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/GiantSkeletonSword/Walk", FramesGenerator.Generate(112, 99, 1024, 19));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/BoneGolem/Attack", FramesGenerator.Generate(275, 273, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/BoneGolem/Death", FramesGenerator.Generate(121, 134, 1024, 18));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/BoneGolem/Walk", FramesGenerator.Generate(153, 149, 1024, 23));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/GiantSkeletonBlade/Attack", FramesGenerator.Generate(202, 191, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/GiantSkeletonBlade/Death", FramesGenerator.Generate(169, 193, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Skeletons/GiantSkeletonBlade/Walk", FramesGenerator.Generate(159, 141, 1024, 16));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Spider/Attack", FramesGenerator.Generate(65, 50, 1024, 11));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Spider/Death", FramesGenerator.Generate(43, 44, 1024, 11));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Spider/Walk", FramesGenerator.Generate(51, 43, 1024, 12));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Giant/Attack", FramesGenerator.Generate(141, 104, 1024, 11));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Giant/Death", FramesGenerator.Generate(89, 93, 1024, 11));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Giant/Walk", FramesGenerator.Generate(107, 86, 1024, 12));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Remorhaz/Attack", FramesGenerator.Generate(236, 209, 1024, 15));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Remorhaz/Death", FramesGenerator.Generate(190, 190, 1024, 21));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Remorhaz/Walk", FramesGenerator.Generate(190, 190, 1024, 28));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Drider/Attack", FramesGenerator.Generate(126, 115, 1024, 14));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Drider/Death", FramesGenerator.Generate(163, 134, 1024, 29));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Spiders/Drider/Walk", FramesGenerator.Generate(186, 156, 1024, 18));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Drowded/Attack", FramesGenerator.Generate(86, 78, 1024, 15));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Drowded/Death", FramesGenerator.Generate(97, 91, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Drowded/Walk", FramesGenerator.Generate(83, 87, 1024, 20));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Ghoul/Attack", FramesGenerator.Generate(107, 1024, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Ghoul/Death", FramesGenerator.Generate(199, 136, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Ghoul/Walk", FramesGenerator.Generate(81, 91, 1024, 16));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Histachii/Attack", FramesGenerator.Generate(106, 91, 1024, 15));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Histachii/Death", FramesGenerator.Generate(87, 113, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Histachii/Walk", FramesGenerator.Generate(53, 92, 1024, 12));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Lemure/Attack", FramesGenerator.Generate(133, 108, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Lemure/Death", FramesGenerator.Generate(97, 107, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Lemure/Walk", FramesGenerator.Generate(109, 87, 1024, 16));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Mummy/Attack", FramesGenerator.Generate(103, 92, 1024, 15));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Mummy/Death", FramesGenerator.Generate(79, 105, 12, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Mummy/Walk", FramesGenerator.Generate(53, 88, 1024, 20));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Nameless/Attack", FramesGenerator.Generate(89, 101, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Nameless/Death", FramesGenerator.Generate(155, 130, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Nameless/Walk", FramesGenerator.Generate(73, 100, 1024, 16));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Nupperibo/Attack", FramesGenerator.Generate(81, 82, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Nupperibo/Death", FramesGenerator.Generate(80, 83, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Nupperibo/Walk", FramesGenerator.Generate(69, 69, 1024, 16));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Ravel/Attack", FramesGenerator.Generate(115, 84, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Ravel/Death", FramesGenerator.Generate(105, 96, 1024, 16));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Ravel/Walk", FramesGenerator.Generate(51, 74, 1024, 16));

            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Vampire/Attack", FramesGenerator.Generate(65, 76, 1024, 13));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Vampire/Death", FramesGenerator.Generate(89, 92, 1024, 21));
            SpriteFramesBank.Instance.Add(@"Images/Zombies/Zombies/Vampire/Walk", FramesGenerator.Generate(81, 79, 1024, 11));
            //100, 55, 10, 40
        }
    }
}