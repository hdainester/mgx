using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Chaotx.Mgx.Control {
    using Layout;

    public class TextItem : Item {
        private SpriteFont font;
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

        private string text = "";
        public string Text {
            get {return text;}
            set {
                if(value != text && value != null) {
                    text = value;
                    Size = Font.MeasureString(text);
                }
            }
        }

        public TextItem(SpriteFont font)
        : this(font, "") {}

        public TextItem(SpriteFont font, string text) {
            Font = font;
            Text = text;
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