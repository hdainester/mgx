using Microsoft.Xna.Framework.Graphics;

namespace Chaotx.Mgx.Control {
    using Layout;
    using Microsoft.Xna.Framework;

    public class ImageItem : Item {
        public Texture2D Image {get; set;}

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
            int width = (int)ScaledSize.X;
            int height = (int)ScaledSize.Y;
            int x = (int)(X - (width - Width)/2);
            int y = (int)(Y - (height - Height)/2);

            Rectangle destination = new Rectangle(x, y, width, height);
            spriteBatch.Draw(Image, destination, Color*Alpha);
        }
    }
}