using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Controls {
    public class ImageItem : Item {
        [Ordered, ContentSerializer(Optional=true)]
        public bool KeepAspectRatio {
            get => keepAspectRatio;
            set => SetProperty(ref keepAspectRatio, value);
        }

        [Ordered, ContentSerializer(Optional=true)]
        public float AspectRatio {
            get => aspectRatio;
            set => SetProperty(ref aspectRatio, value);
        }

        [Ordered, ContentSerializer(Optional=true, ElementName="Image")]
        private string imageRef {get => _imageRef; set => SetProperty(ref _imageRef, value);}

        [ContentSerializerIgnore]
        public override Vector2 Size {
            get => base.Size;
            internal set {
                if(KeepAspectRatio && AspectRatio != 0
                && value.X != 0 && value.Y != 0) {
                    float wr = value.X/AspectRatio;
                    value = wr < value.Y
                        ? new Vector2(value.X, wr)
                        : new Vector2(value.Y*AspectRatio, value.Y);
                }

                base.Size = value;
            }
        }

        [ContentSerializerIgnore]
        public Texture2D Image {get; set;}

        private float aspectRatio;
        private bool keepAspectRatio;
        private string _imageRef = "";

        public ImageItem() {} // for content serializer
        public ImageItem(Texture2D image, int width = -1, int height = -1) {
            if(image != null) {
                if(width < 0) width = image.Width;
                if(height < 0) height = image.Height;
                AspectRatio = Width/(float)Height;
            }

            Image = image;
            Width = width;
            Height = height;
        }

        public override void Load(ContentManager content) {
            if(Image == null) {
                Image = content.Load<Texture2D>(_imageRef);
                if(Width <= 0) Width = Image.Width;
                if(Height <= 0) Height = Image.Height;
                AspectRatio = Width/(float)Height;
            }
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