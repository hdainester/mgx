using Microsoft.Xna.Framework.Graphics;

namespace Mgx.Control {
    using Layout;
    using Microsoft.Xna.Framework;

    public class ImageItem : Item {
        public Texture2D Image {get; protected set;}

        public ImageItem(Texture2D image) : this(image, image.Width, image.Height) {}
        public ImageItem(Texture2D image, int width, int height) {
            Image = image;
            Width = width;
            Height = height;
        }

        public override void Update(GameTime gameTime) {
            // nothing here
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Rectangle destination = new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
            spriteBatch.Draw(Image, destination, Color*Alpha);
        }
    }
}