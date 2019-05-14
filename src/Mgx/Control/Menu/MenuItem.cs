using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Assets;
using Chaotx.Mgx.Layout;

using System;

namespace Chaotx.Mgx.Controls.Menus {
    public class MenuItem : Control {
        [ContentSerializer(Optional = true)]
        public Orientation Orientation {
            get {return orientation;}
            set {SetProperty(ref orientation, value);}
        }

        [ContentSerializer(Optional = true, ElementName = "TextItem")]
        private Asset<TextItem> TextAsset {get; set;}

        [ContentSerializer(Optional = true, ElementName = "ImageItem")]
        private Asset<ImageItem> ImageAsset {get; set;}

        [ContentSerializerIgnore]
        public TextItem TextItem {
            get => text;
            protected set {
                HPane.Remove(text);
                VPane.Remove(text);
                text = value;
                AlignItems();
            }
        }

        [ContentSerializerIgnore]
        public ImageItem ImageItem {
            get => image;
            protected set {
                HPane.Remove(image);
                VPane.Remove(image);
                image = value;
                AlignItems();
            }
        }

        [ContentSerializerIgnore] // TODO
        public Menu Menu {get; protected set;}

        protected HPane HPane {get; set;}
        protected VPane VPane {get; set;}
        
        private TextItem text;
        private ImageItem image;
        private float focusFade;
        private float extraScale = 0.1f;
        private Orientation orientation;

        private MenuItem() : this("", null, null, 0, 0) {} // for content serializer
        public MenuItem(Texture2D image) : this("", null, image) {}
        public MenuItem(Texture2D image, int imageWidth, int imageHeight) : this("", null, image, imageWidth, imageHeight) {}
        public MenuItem(string text, SpriteFont font) : this(text, font, null, 0, 0) {}
        public MenuItem(string text, SpriteFont font, Texture2D image) : this (text, font, image, image.Width, image.Height) {}
        public MenuItem(string text, SpriteFont font, Texture2D image, int imageWidth, int imageHeight) {
            VPane = new VPane();
            HPane = new HPane();
            
            if(image != null) {
                ImageItem = new ImageItem(image, imageWidth, imageHeight);
                ImageItem.HAlign = HAlignment.Center;
                ImageItem.VAlign = VAlignment.Center;
                HPane.Add(ImageItem);
            }

            if(font != null) {
                TextItem = new TextItem(font, text);
                TextItem.HAlign = HAlignment.Center;
                TextItem.VAlign = VAlignment.Center;
                HPane.Add(TextItem);
            }

            HPane.HAlign = VPane.HAlign = HAlignment.Center;
            HPane.VAlign = VPane.VAlign = VAlignment.Center;

            Orientation = Orientation.Vertical;
            HAlign = HAlignment.Center;
            VAlign = VAlignment.Center;
        }

        public override void Load(ContentManager content) {
            // They may not be added in the ctor or
            // duplicate entries will occur in the
            // Children collection when loaded with
            // the content pipeline
            _Add(HPane);
            _Add(VPane);
            
            if(TextAsset != null) {
                TextAsset.Load(content);
                TextItem = TextAsset.Object;
            }

            if(ImageAsset != null) {
                ImageAsset.Load(content);
                ImageItem = ImageAsset.Object;
            }

            base.Load(content);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            UpdateFocusedEffect(gameTime);
        }

        protected override void OnAction() {
            if(!IsDisabled && (Menu == null || !Menu.IsDisabled))
                base.OnAction();
        }

        protected override void OnCancel() {
            if(!IsDisabled && (Menu == null || !Menu.IsDisabled))
                base.OnCancel();
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
                if(TextItem != null) TextItem.Scale = scale;
                if(ImageItem != null) ImageItem.Scale = scale;
            }
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);

            if(propertyName.Equals("HGrow"))
                HPane.HGrow = VPane.HGrow = HGrow;

            if(propertyName.Equals("VGrow"))
                HPane.VGrow = VPane.VGrow = VGrow;

            if(propertyName.Equals("Orientation"))
                AlignItems();
        }

        protected virtual void AlignItems() {
            if(TextItem != null) {
                HPane.Remove(TextItem);
                VPane.Remove(TextItem);
            }

            if(ImageItem != null) {
                HPane.Remove(ImageItem);
                VPane.Remove(ImageItem);
            }

            if(Orientation == Orientation.Horizontal) {
                if(ImageItem != null) HPane.Add(ImageItem);
                if(TextItem != null) HPane.Add(TextItem);
            }

            if(Orientation == Orientation.RHorizontal) {
                if(TextItem != null) HPane.Add(TextItem);
                if(ImageItem != null) HPane.Add(ImageItem);
            }

            if(Orientation == Orientation.Vertical) {
                if(ImageItem != null) VPane.Add(ImageItem);
                if(TextItem != null) VPane.Add(TextItem);
            }

            if(Orientation == Orientation.RVertical) {
                if(TextItem != null) VPane.Add(TextItem);
                if(ImageItem != null) VPane.Add(ImageItem);
            }
        }

        protected static void _SetMenu(MenuItem item, Menu menu) {
            item.Menu = menu;
        }
    }
}