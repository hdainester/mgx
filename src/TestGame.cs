using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

namespace Test {
    public class TestGame : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // mgx default fonts example
        private Dictionary<string, SpriteFont> mgxFonts;
        private Dictionary<string, Color> fontColors;
        private Dictionary<string, Vector2> textPositions;


        public TestGame() {
            mgxFonts = new Dictionary<string, SpriteFont>();
            fontColors = new Dictionary<string, Color>();
            textPositions = new Dictionary<string, Vector2>();

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // mgx default fonts example
            mgxFonts.Add("Header Font", Content.Load<SpriteFont>("mgx/fonts/header"));
            mgxFonts.Add("Header (italic) Font", Content.Load<SpriteFont>("mgx/fonts/header_italic"));
            mgxFonts.Add("Header (bold) Font", Content.Load<SpriteFont>("mgx/fonts/header_bold"));
            mgxFonts.Add("Content Font", Content.Load<SpriteFont>("mgx/fonts/content"));
            mgxFonts.Add("Content (italic) Font", Content.Load<SpriteFont>("mgx/fonts/content_italic"));
            mgxFonts.Add("Content (bold) Font", Content.Load<SpriteFont>("mgx/fonts/content_bold"));
            mgxFonts.Add("Footer Font", Content.Load<SpriteFont>("mgx/fonts/footer"));
            mgxFonts.Add("Footer (italic) Font", Content.Load<SpriteFont>("mgx/fonts/footer_italic"));
            mgxFonts.Add("Footer (bold) Font", Content.Load<SpriteFont>("mgx/fonts/footer_bold"));
        }

        private int prevTime = -1;
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int days = gameTime.TotalGameTime.Days;
            int hours = gameTime.TotalGameTime.Hours;
            int minutes = gameTime.TotalGameTime.Minutes;
            int seconds = gameTime.TotalGameTime.Seconds;

            if(prevTime != seconds) {
                Console.WriteLine("Passed Time: {0:00}:{1:00}:{2:00}:{3:00}",
                    days, hours, minutes, seconds);

                prevTime = seconds;
            }

            // basic example for update logic of mgx
            // fonts example (also handling input)
            UpdateFonts();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // mgx default fonts example
            Vector2 position;
            SpriteFont font;
            Color color;

            spriteBatch.Begin();
            mgxFonts.Keys.ToList().ForEach(key => {
                font = mgxFonts[key];
                color = fontColors[key];
                position = textPositions[key];
                spriteBatch.DrawString(font, key, position, color);
            });
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // example of handling input for mgx
        // font test on mouseover
        protected void HandleInput(string fontKey, Vector2 pos, Vector2 size) {
            MouseState ms = Mouse.GetState();

            if(ms.Position.X >= pos.X && ms.Position.Y >= pos.Y
            && ms.Position.X < pos.X + size.X && ms.Position.Y < pos.Y + size.Y)
                fontColors[fontKey] = Color.Yellow;
            else fontColors[fontKey] = Color.White;
        }

        // helper to update text positions for
        // mgx font test
        protected void UpdateFonts() {
            int textHeight = 0;
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;
            Vector2 size, position = Vector2.Zero;
            SpriteFont font;

            mgxFonts.Keys.ToList().ForEach(key =>
                textHeight += (int)mgxFonts[key].MeasureString(key).Y);

            position.Y = screenHeight/2 - textHeight/2;
            mgxFonts.Keys.ToList().ForEach(key => {
                font = mgxFonts[key];
                size = font.MeasureString(key);
                position.X = screenWidth/2 - (int)size.X/2;
                textPositions[key] = position;
                HandleInput(key, position, size);
                position.Y += size.Y;
            });
        }
    }
}