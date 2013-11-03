using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using SSCEngine.GestureHandling;
using SCSEngine.ResourceManagement;
using SCSEngine.Mathematics;
using SSCEngine.GestureHandling.Implements.Events;
using System.Diagnostics;
using SSCEngine.Control;

namespace TestGesture
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestGestureGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        IResourceManager resMan;

        // can co gesture manager de detect gesture
        IGestureManager gestureMan;
        // gesture dispatcher de phan phoi cac gesture event
        UIControlManager uiManager;

        public TestGestureGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333 / 2);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here:
            
            // KHOI TAO GESTURE MANAGER
            this.gestureMan = DefaultGestureHandlingFactory.Instance.CreateManager(this);
            // INIT CAC GESTURE DETECTOR CO BAN, NEU MUON THEM NUA THI CU ADD DETECTURE VAO MANAGER
            DefaultGestureHandlingFactory.Instance.InitDetectors(gestureMan);
            // ADD GESTURE MANAGER VAO COMPONENTS CUA GAME, MOI GAME THUONG CHI TAO MOT GESTURE MANAGER
            this.Components.Add(gestureMan);

            this.uiManager = new UIControlManager(this, DefaultGestureHandlingFactory.Instance);
            this.gestureMan.AddDispatcher(uiManager);

            this.Components.Add(uiManager);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.resMan = new SCSResourceManager(this.Content);
            // TODO: use this.Content to load your game content here

            PictureBox pb = new PictureBox(this, spriteBatch);
            pb.Image = resMan.GetResource<Texture2D>("img");
            pb.FitSizeByImage();

            ListView lv = new ListView(this, HorizontalListViewFactory.Instance);
            lv.AddItem(pb);
            lv.Canvas.Bound.Size.X = 1000;
            lv.Canvas.Bound.Size.Y = 100;
            lv.Canvas.Content.Size.X = 1000;
            lv.Canvas.Content.Size.Y = 100;
            this.uiManager.Add(lv);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

                base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
