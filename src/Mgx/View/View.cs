using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Mgx.View {
    using Layout;

    public abstract class View {
        protected class ViewContainer : StackPane {
            public void SetPosition(Vector2 position) {
                Position = position;
            }

            public void SetSize(Vector2 size) {
                Size = size;
            }
        }

        public ContentManager Content {get; protected set;}
        public ViewState State {get; protected set;}
        public GraphicsDevice Graphics {get;}
        protected ViewContainer MainContainer {get;}

        public View(ContentManager content, GraphicsDevice graphics) {
            MainContainer = new ViewContainer();
            Graphics = graphics;
            Content = content;
            AlignMainContainer();
        }

        public abstract void Show();
        public abstract void Hide();
        public abstract void Close();
        public virtual void HandleInput() {}
        public virtual void Update(GameTime gameTime) {
            if(State == ViewState.Open)
                HandleInput();
                
            MainContainer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            MainContainer.Draw(spriteBatch);
        }

        // TODO (needs to be called after viewport changes)
        protected void AlignMainContainer() {
            MainContainer.HGrow = MainContainer.VGrow = 1;
            MainContainer.SetPosition(new Vector2(0, 0));
            MainContainer.SetSize(new Vector2(
                Graphics.Viewport.Width,
                Graphics.Viewport.Height));
        }
    }
}