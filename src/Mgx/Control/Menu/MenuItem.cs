using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;

namespace Chaotx.Mgx.Control.Menu {
    using Layout;

    public class MenuItem : Control {
        private Orientation orientation;
        public Orientation Orientation {
            get {return orientation;}
            set {SetProperty(ref orientation, value);}
        }

        public TextItem Text {get; protected set;}
        public ImageItem Image {get; protected set;}
        public Menu Menu {get; protected set;}

        public bool KeyBoardEnabled {get; set;} = true;
        public bool GamePadEnabled {get; set;} = true;
        public bool MouseEnabled {get; set;} = true;
        public bool TouchEnabled {get; set;} = true;

        private HPane hPane;
        private VPane vPane;
        
        private float focusFade;
        private float extraScale = 0.1f;

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

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            UpdateFocusedEffect(gameTime);
        }

        public override void HandleInput() {
            if(!IsDisabled && (Menu == null || !Menu.IsDisabled)) {
                if(KeyBoardEnabled && (Menu == null || Menu.KeyBoardEnabled)) HandleKeyboard();
                if(GamePadEnabled && (Menu == null || Menu.GamePadEnabled)) HandleGamepad();
                if(MouseEnabled && (Menu == null || Menu.MouseEnabled)) HandleMouse();
                if(TouchEnabled && (Menu == null || Menu.TouchEnabled)) HandleTouch();
            }
        }

        protected virtual void UpdateFocusedEffect(GameTime gameTime) {
            if(IsFocused || focusFade != 0) {
                float elapsedSecs = gameTime.ElapsedGameTime.Milliseconds/500f;
                double secs = gameTime.TotalGameTime.TotalSeconds;
                float s = (float)(Math.Sin(2*Math.PI*secs) + 1)/2*extraScale;

                if(IsFocused)
                    focusFade = Math.Min(1, focusFade += elapsedSecs);
                else if(focusFade != 0)
                    focusFade = Math.Max(0, focusFade -= elapsedSecs);

                // float scale = originalScale + s*focusFade;
                float scale = 1 + s*focusFade;
                if(Text != null) Text.Scale = scale;
                if(Image != null) Image.Scale = scale;
            }
        }

        // protected override void OnEnabled() {
        //     base.OnEnabled();

        //     if(Text != null) {
        //         Text.Color = Color.White;
        //         Text.Alpha = 1;
        //     }

        //     if(Image != null) {
        //         Image.Color = Color.White;
        //         Image.Alpha = 1;
        //     }
        // }

        // protected override void OnDisabled() {
        //     base.OnDisabled();

        //     if(Text != null) {
        //         Text.Color = Color.Gray;
        //         Text.Alpha = 0.75f;
        //     }

        //     if(Image != null) {
        //         Image.Color = Color.Gray;
        //         Image.Alpha = 0.75f;
        //     }
        // }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);

            // TODO temp solution
            if(propertyName.Equals("IsDisabled"))
                if(IsDisabled) IsFocused = false;

            if(propertyName.Equals("HGrow"))
                hPane.HGrow = vPane.HGrow = HGrow;

            if(propertyName.Equals("VGrow"))
                hPane.VGrow = vPane.VGrow = VGrow;

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

        protected static void _SetMenu(MenuItem item, Menu menu) {
            item.Menu = menu;
        }
    }
}