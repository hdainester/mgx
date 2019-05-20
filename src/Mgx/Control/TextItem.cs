using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Controls {
    public class TextItem : Item {
        [Ordered, ContentSerializer(Optional = true)]
        public string Text {
            get {return text;}
            set {
                if(value != text && value != null) {
                    text = value;

                    if(Font != null)
                        Size = Font.MeasureString(text);
                }
            }
        }

        [Ordered, ContentSerializer(Optional = true, ElementName = "Font")]
        private string _fontRef;

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
                Font = content.Load<SpriteFont>(_fontRef);
        }

        public override void Update(GameTime gameTime) {
            // nothing here
        }

        public override void Draw(SpriteBatch spriteBatch) {
            int x = (int)(X - (ScaledSize.X - Size.X)/2);
            int y = (int)(Y - (ScaledSize.Y - Size.Y)/2);

            spriteBatch.DrawString(Font, Text, new Vector2(x, y), Color*Alpha,
                0f, Vector2.Zero, Scale, SpriteEffects.None, 1f);
        }
    }
}