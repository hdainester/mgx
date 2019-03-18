using Microsoft.Xna.Framework.Graphics;

namespace Mgx.Control {
    using Layout;
    using Microsoft.Xna.Framework;

    public class ImageItem : Component {
        public Texture2D Image {get; protected set;}

        public ImageItem(Texture2D image) {
            Image = image;
        }

        public override void Update(GameTime gameTime) {
            // nothing here
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Rectangle destination = new Rectangle((int)X, (int)Y, (int)Parent.Width, (int)Parent.Height);
            // TODO place Color property somewhere
            spriteBatch.Draw(Image, destination, Color);
        }
    }
}