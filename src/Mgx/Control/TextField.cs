using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Layout;
using System;

namespace Chaotx.Mgx.Controls {
    public class TextField : Control {
        public delegate void TextInputEventHandler(object sender, TextInputEventArgs args);

        [Ordered, ContentSerializer(Optional = true)]
        public HAlignment TextAlignment {
            get {return textItem.HAlign;}
            set {textItem.HAlign = value;}
        }

        [ContentSerializerIgnore]
        public string Text {
            get {return textItem.Text;}
            set {textItem.Text = value;}
        }

        [ContentSerializerIgnore]
        public override Vector2 Size {
            get => base.Size;
            internal set => base.Size
                = new Vector2(value.X, font.MeasureString("I").Y);
        }

        public event TextInputEventHandler TextInput;

        private GameWindow window;
        private Texture2D backTexture;
        private ImageItem background;
        private TextItem textItem;
        private SpriteFont font;

        public TextField(GameWindow window, SpriteFont font, Texture2D backTexture) {
            this.backTexture = backTexture;
            this.window = window;
            this.font = font;
            Init();
        }

        private void Init() {
            if(textItem != null) _Remove(textItem);
            if(background != null) _Remove(background);

            background = new ImageItem(backTexture);
            background.HGrow = background.VGrow = 1;
            background.Color = Color.LightGray;
            background.Alpha = 0.75f;

            textItem = new TextItem(font);
            textItem.HAlign = HAlignment.Left;
            textItem.VAlign = VAlignment.Center;
            textItem.Color = Color.Black;

            window.TextInput -= TextInputHandler;
            window.TextInput += TextInputHandler;

            _Add(background);
            _Add(textItem);
        }

        private void TextInputHandler(object sender, TextInputEventArgs args) {
            if(!IsDisabled) {
                if(args.Key == Keys.Back && textItem.Text.Length > 0)
                    textItem.Text = textItem.Text.Remove(textItem.Text.Length-1);
                else if(font.Characters.Contains(args.Character))
                    textItem.Text += args.Character;

                OnTextInput(args);
            }
        }

        protected void OnTextInput(TextInputEventArgs args) {
            TextInputEventHandler handler = TextInput;
            if(handler != null) handler(this, args);
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);

            if(propertyName == "IsDisabled") {
                background.Alpha = IsDisabled ? 0.5f : 0.75f;
                textItem.Alpha = IsDisabled ? 0.75f : 1;
            }
        }
    }
}