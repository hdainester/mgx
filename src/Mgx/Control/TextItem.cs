using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Controls {
    public class TextItem : Item {
        [Ordered, ContentSerializer(Optional = true, ElementName = "Font")]
        internal string FontRef {get; set;}

        [Ordered, ContentSerializer(Optional = true)]
        public string Text {
            get {return text;}
            set {
                if(value != text && value != null) {
                    SetProperty(ref text, value);

                    if(Font != null)
                        Size = Font.MeasureString(text);
                }
            }
        }

        [ContentSerializerIgnore]
        public SpriteFont Font {
            get {return font;}
            set {
                if(value == null)
                    throw new System.InvalidOperationException("font may not be null");

                if(value != font) {
                    font = value;
                    Size = Font.MeasureString(text);
                }
            }
        }

        private SpriteFont font;
        private string text = "";

        public TextItem() {} // for content serializer
        public TextItem(SpriteFont font)
        : this(font, "") {}

        public TextItem(SpriteFont font, string text) {
            Font = font;
            Text = text;
        }

        public override void Load(ContentManager content) {
            base.Load(content);

            if(Font == null)
                Font = content.Load<SpriteFont>(FontRef);
        }

        public override void Update(GameTime gameTime) {
            // nothing here
        }

        public override void Draw(SpriteBatch spriteBatch) {
            int x = (int)(X - (ScaledSize.X - Size.X)/2);
            int y = (int)(Y - (ScaledSize.Y - Size.Y)/2);
            float rotation = 0f; // TODO
            Vector2 origin = Vector2.Zero; // TODO

            spriteBatch.DrawString(Font, Text, new Vector2(x, y), Color*Alpha,
                rotation, origin, Scale, SpriteEffects.None, Layer);
        }
    }
}