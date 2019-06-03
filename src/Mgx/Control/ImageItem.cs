using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Controls {
    public class ImageItem : Item {
        [Ordered, ContentSerializer(Optional=true, ElementName="Image")]
        internal string ImageRef {get; set;}
        
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

        public ImageItem() {} // for content serializer
        public ImageItem(Texture2D image, int width = -1, int height = -1) {
            if(image != null) {
                if(width < 0) width = image.Width;
                if(height < 0) height = image.Height;
                AspectRatio = width/(float)height;
            }

            Image = image;
            Width = width;
            Height = height;
        }

        public override void Load(ContentManager content) {
            if(Image == null) {
                Image = content.Load<Texture2D>(ImageRef);
                if(Width <= 0) Width = Image.Width;
                if(Height <= 0) Height = Image.Height;
                AspectRatio = Width/(float)Height;
            }
        }

        public override void Update(GameTime gameTime) {
            // nothing here
        }

        public override void Draw(SpriteBatch spriteBatch) {
            float width = ScaledSize.X;
            float height = ScaledSize.Y;
            float x = X - (width - Width)/2;
            float y = Y - (height - Height)/2;
            float rotation = 0f; // TODO

            Vector2 origin = Vector2.Zero; // TODO
            Rectangle? source = null; // TODO
            Rectangle destination = new Rectangle(
                (int)(x+0.5f), (int)(y+0.5f),
                (int)(width+0.5f), (int)(height+0.5f));

            spriteBatch.Draw(Image, destination, source, Color*Alpha,
                rotation, origin, SpriteEffects.None, Layer);
        }
    }
}