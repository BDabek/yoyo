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

namespace YoYo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        YoYo yoyo;
        Model yoyoModel,lina;
        Camera camera;
        bool isStarted;
        float strength;
        KeyboardState OldKeyState;
        Effect oldEffect;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            isStarted = false;
            strength = 10;
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
            camera = new Camera(this, new Vector3(-30f, 1f, -30f), Vector3.Zero, 5f);
            Components.Add(camera);
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
            yoyoModel = Content.Load<Model>("yoyo");
            lina = Content.Load<Model>("lina");
            yoyo = new YoYo(yoyoModel, 1);

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
            KeyboardState NewKeyState = Keyboard.GetState();
            // TODO: Add your update logic here
            if (NewKeyState.IsKeyDown(Keys.S) && OldKeyState.IsKeyDown(Keys.S))
            {
                isStarted = true;
                yoyo = new YoYo(yoyoModel, 1);
                yoyo.push(strength);
            }
            if (NewKeyState.IsKeyUp(Keys.R) && OldKeyState.IsKeyDown(Keys.R))
            {
                strength += 1;
                Console.WriteLine("Sila {0}", strength);
            }
            if (NewKeyState.IsKeyUp(Keys.T) && OldKeyState.IsKeyDown(Keys.T))
            {
                strength -= 1;
                Console.WriteLine("Sila {0}", strength);
            }
            if (NewKeyState.IsKeyUp(Keys.D) && OldKeyState.IsKeyDown(Keys.D))
            {
                yoyo.pull(strength);
            }
            if (isStarted)
            {
                yoyo.UpdatePosition(gameTime);

            }
            OldKeyState = NewKeyState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            yoyo.Draw(camera);

            base.Draw(gameTime);

        }
    }
}
