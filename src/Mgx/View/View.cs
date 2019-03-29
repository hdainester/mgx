using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Mgx.View {
    using Layout;

    public abstract class View {

        protected class ViewContainer : StackPane {
            public ViewContainer(View view) {
                ParentView = view;
            }

            public void SetPosition(Vector2 position) {
                Position = position;
            }

            public void SetSize(Vector2 size) {
                Size = size;
            }
        }

        private ViewControl manager;
        public ViewControl Manager {
            get {return manager;}
            set {
                if(value != manager) {
                    if(manager != null)
                        manager.Remove(this);

                    manager = value;
                    if(value != null)
                        value.Add(this);
                }
            }
        }

        public ContentManager Content {get; protected set;}
        public GraphicsDevice Graphics {get; protected set;}
        public ViewState State {get; protected set;}
        protected ViewContainer MainContainer {get;}

        public View(ContentManager content, GraphicsDevice graphics) : this(content, graphics, null) {}
        public View(ContentManager content, GraphicsDevice graphics, ViewControl manager) {
            MainContainer = new ViewContainer(this);
            Graphics = graphics;
            Content = content;
            Manager = manager;
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