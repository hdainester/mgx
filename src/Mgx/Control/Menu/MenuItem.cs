using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Mgx.Control.Menu {
    using Layout;

    public class MenuItem : Control {
        private Orientation orientation;
        public Orientation Orientation {
            get {return orientation;}
            set {SetProperty(ref orientation, value);}
        }

        public TextItem Text {get; protected set;}
        public ImageItem Image {get; protected set;}

        private HPane hPane;
        private VPane vPane;

        public MenuItem(Texture2D image) : this(null, null, image) {}
        public MenuItem(Texture2D image, int imageWidth, int imageHeight) : this(null, null, image, imageWidth, imageHeight) {}
        public MenuItem(string text, SpriteFont font) : this(text, font, null, 0, 0) {}
        public MenuItem(string text, SpriteFont font, Texture2D image) : this (text, font, image, image.Width, image.Height) {}
        public MenuItem(string text, SpriteFont font, Texture2D image, int imageWidth, int imageHeight) {
            vPane = new VPane();
            hPane = new HPane();

            if(image != null) {
                Image = new ImageItem(image, imageWidth, imageHeight);
                Image.HAlign = HAlignment.Center;
                Image.VAlign = VAlignment.Center;
                hPane.Add(Image);
            }

            if(text != null) {
                Text = new TextItem(font, text);
                Text.HAlign = HAlignment.Center;
                Text.VAlign = VAlignment.Center;
                hPane.Add(Text);
            }

            
            hPane.HAlign = vPane.HAlign = HAlignment.Center;
            hPane.VAlign = vPane.VAlign = VAlignment.Center;

            Orientation = Orientation.Vertical;
            HAlign = HAlignment.Center;
            VAlign = VAlignment.Center;
            _Add(hPane);
            _Add(vPane);
        }

        protected override void OnEnabled() {
            base.OnEnabled();

            if(Text != null) {
                Text.Color = Color.White;
                Text.Alpha = 1;
            }

            if(Image != null) {
                Image.Color = Color.White;
                Image.Alpha = 1;
            }
        }

        protected override void OnDisabled() {
            base.OnDisabled();

            if(Text != null) {
                Text.Color = Color.Gray;
                Text.Alpha = 0.75f;
            }

            if(Image != null) {
                Image.Color = Color.Gray;
                Image.Alpha = 0.75f;
            }
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);

            if(propertyName.Equals("Orientation") && Text != null && Image != null) {
                if(Orientation == Orientation.Horizontal) {
                    hPane.Add(Image);
                    hPane.Add(Text);
                }

                if(Orientation == Orientation.RHorizontal) {
                    hPane.Add(Text);
                    hPane.Add(Image);
                }

                if(Orientation == Orientation.Vertical) {
                    vPane.Add(Image);
                    vPane.Add(Text);
                }

                if(Orientation == Orientation.RVertical) {
                    vPane.Add(Text);
                    vPane.Add(Image);
                }
            }
        }
    }
}