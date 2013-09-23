using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Test.Objects;

namespace Test
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<GameObject> GameWorld { get; set; }

        MouseState mouseStateCurrent;
        MouseState mouseStatePre;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            GameWorld = new List<GameObject>();
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

            SpriteFont font = Content.Load<SpriteFont>("DebugFont");

            SCSServices.GetInstance().PreLoad(Content, this, font);
            // TODO: use this.Content to load your game content here
            
            // Add plant
            Plant plant = new Plant();
            plant.Position = new Vector2(50, 100);
            GameWorld.Add(plant);

            mouseStatePre = mouseStateCurrent = Mouse.GetState();
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

            mouseStatePre = mouseStateCurrent;
            mouseStateCurrent = Mouse.GetState();

            if (mouseStateCurrent.LeftButton == ButtonState.Released
                && mouseStatePre.LeftButton == ButtonState.Pressed)
            {
                // AddZomnie
                Zombie zombie = new Zombie();
                zombie.Position = new Vector2(600, 100);
                GameWorld.Add(zombie);
            }

            // TODO: Add your update logic here
            for (int i = 0; i < GameWorld.Count; i++ )
            {
                GameObject item = GameWorld[i];
                if (item.Alive)
                    item.Update(gameTime);
                else
                {
                    GameWorld.Remove(item);
                    i--;
                }
            }



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
            spriteBatch.Begin();
            foreach (var item in GameWorld)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.DrawString(SCSServices.GetInstance().DebugFont, GameWorld.Count.ToString(), new Vector2(100, 200), Color.White );
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
