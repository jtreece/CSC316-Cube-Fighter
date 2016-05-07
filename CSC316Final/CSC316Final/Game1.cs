using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CSC316Final
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model player1;
        Model player2;
        Model ground;
        Texture2D health_bar;
        SpriteFont font;
        Matrix world;
        Matrix view;
        Matrix projection;
        Vector3 player1_position;
        Vector3 player2_position;
        Vector3 ground_position;
        Vector3 camera_position;
        Vector3 center_screen;
        Vector3 left_stage_boundary;
        Vector3 right_stage_boundary;
        Vector3 move;
        Vector3 zoom;
        Vector3 start_distance;
        int player1_health;
        int player2_health;

        public Game1(): base()
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
            player1_position = new Vector3(-10, 0, 0);
            player2_position = new Vector3(10, 0, 0);
            player1_health = player2_health = 100;
            start_distance = player2_position - player1_position;
            ground_position = Vector3.Zero;
            camera_position = new Vector3(0, 10, 30);
            center_screen = (player1_position + player2_position) / 2;
            left_stage_boundary = new Vector3(-25, 0, 0);
            right_stage_boundary = new Vector3(25, 0, 0);
            move = Vector3.Right;
            zoom = Vector3.Zero;

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

            // TODO: use this.Content to load your game content here
            player1 = Content.Load<Model>("player1_model");
            player2 = Content.Load<Model>("player2_model");
            ground = Content.Load<Model>("ground_model");
            health_bar = Content.Load<Texture2D>("HealthBar");
            font = Content.Load<SpriteFont>("arial");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                player1_position -= move;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                player1_position += move;


            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                player2_position -= move;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                player2_position += move;

            if (player1_position.X - left_stage_boundary.X <= 0)
                player1_position = left_stage_boundary;
            if (player1_position.X - right_stage_boundary.X >= 0)
                player1_position = right_stage_boundary; 
            if (player2_position.X - left_stage_boundary.X <= 0)
                player2_position = left_stage_boundary;
            if (player2_position.X - right_stage_boundary.X >= 0)
                player2_position = right_stage_boundary;
            if (player1_position.Y <= 0)
                player1_position.Y = 0.0f;
            if (player2_position.Y <= 0)
                player2_position.Y = 0.0f;

            if (Vector3.Distance(player1_position, player2_position) >= 30)
            {
                zoom.Z += 0.5f*(float)gameTime.TotalGameTime.TotalMilliseconds / 1000;
            }
            if (Vector3.Distance(player1_position, player2_position) <= 30)
            {
                zoom.Z -= 0.5f*(float)gameTime.TotalGameTime.TotalMilliseconds / 1000;
            }
            if (zoom.Z >= 15)
                zoom.Z = 15;
            if (zoom.Z <= 0)
                zoom.Z = 0;
            
            center_screen = (player1_position + player2_position) / 2;
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
            world = Matrix.CreateTranslation(player1_position);
            view = Matrix.CreateLookAt(camera_position + zoom + center_screen, center_screen, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 1000.0f);
            player1.Draw(world, view, projection);

            world = Matrix.CreateTranslation(player2_position);
            player2.Draw(world, view, projection);

            world = Matrix.CreateTranslation(ground_position);
            ground.Draw(world, view, projection);

            spriteBatch.Begin();
            spriteBatch.Draw(health_bar, new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - 2 * health_bar.Width / 3 - 10, 10, 2 * health_bar.Width / 3, 44), new Rectangle(0, 70, health_bar.Width, 44), Color.Gray);
            spriteBatch.Draw(health_bar, new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - 2 * health_bar.Width / 3 - 10, 10, (int)(health_bar.Width * ((double)2 * player1_health / 300)), 44), new Rectangle(0, 70, health_bar.Width, 44), Color.Green);
            spriteBatch.Draw(health_bar, new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - 2 * health_bar.Width / 3 - 10, 10, 2 * health_bar.Width / 3, 44), new Rectangle(0, 0, health_bar.Width, 65), Color.White);
            spriteBatch.Draw(health_bar, new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 + 10, 10, 2 * health_bar.Width / 3, 44), new Rectangle(0, 70, health_bar.Width, 44), Color.Gray);
            spriteBatch.Draw(health_bar, new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 + 10, 10, (int)(health_bar.Width * ((double)2 * player1_health / 300)), 44), new Rectangle(0, 70, health_bar.Width, 44), Color.Green);
            spriteBatch.Draw(health_bar, new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 + 10, 10, 2 * health_bar.Width / 3, 44), new Rectangle(0, 0, health_bar.Width, 65), Color.White);
            spriteBatch.DrawString(font, "Player 1", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 2 * health_bar.Width / 3 - 10, 55), Color.White);
            spriteBatch.DrawString(font, "Player 2", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 + 2 * health_bar.Width / 3 - 45, 55), Color.White);
            spriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);
        }
    }
}
