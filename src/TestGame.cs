using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

using Mgx.View;

namespace Test {
    public class TestGame : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ViewControl viewControl;

        public TestGame() {
            viewControl = new ViewControl();            
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
            viewControl.Add(new MenuTestView(Content, GraphicsDevice));
        }

        private int prevTime = -1;
        protected override void Update(GameTime gameTime) {
            if(viewControl.Views.Count == 0)
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

            viewControl.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            viewControl.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}