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

namespace CS4300_Final_Project
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FinalProject : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice mDevice;
        Effect effect;
        Camera mCamera;

        Matrix viewMatrix;
        Matrix projectionMatrix;

        XWing mXWing = new XWing();
        Table mTable = new Table();
        Terrain mTerrain;

        public FinalProject()
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

            base.Initialize();
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Window.Title = "Perlin Noise ALL the Things!";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mDevice = graphics.GraphicsDevice;
            effect = Content.Load<Effect>("effects");

            // Load objects to draw
            mTerrain = new Terrain(Content);
            mXWing.load(Content, effect);
            mTable.load(Content, effect);

            Rectangle windowBounds = this.Window.ClientBounds;
            mCamera = new Camera(new Vector3(60, 30, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0), windowBounds.Width, windowBounds.Height, mDevice.Viewport.AspectRatio, mTerrain);
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
            float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            // Proccess the input from the user
            mCamera.processInput();

            // Update the view and projection matrices with the output of the camera class
            viewMatrix = mCamera.m_LookAtMatrix;
            projectionMatrix = mCamera.m_ProjectionMatrix;

            // Move the XWing!
            mXWing.move();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            mDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            mDevice.RasterizerState = rs;

            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xTexture"].SetValue(mTerrain.getTexture());

            effect.CurrentTechnique = effect.Techniques["Textured"];
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                mTerrain.draw(mDevice);
            }

            effect.CurrentTechnique = effect.Techniques["Colored"];
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                mXWing.draw(viewMatrix, projectionMatrix);
                mTable.draw(viewMatrix, projectionMatrix);
            }

            base.Draw(gameTime);
        }
    }
}
