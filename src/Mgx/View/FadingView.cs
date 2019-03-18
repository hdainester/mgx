using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Mgx.View {
    using Layout;

    public class FadingView : View {
        public FadingView(ContentManager content, GraphicsDevice graphics)
        : base(content, graphics) {}

        public override void Show() {
            State = ViewState.Open;
            // throw new System.NotImplementedException();
        }

        public override void Hide() {
            State = ViewState.Hidden;
            // throw new System.NotImplementedException();
        }

        public override void Close() {
            State = ViewState.Closed;
            // throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime) {
            MainContainer.Update(gameTime);
            // throw new System.NotImplementedException();
        }
    }
}