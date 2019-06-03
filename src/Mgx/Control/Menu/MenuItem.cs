using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Layout;
using System;

namespace Chaotx.Mgx.Controls.Menus {
    public class MenuItem : Control {
        private static readonly float FocusLayer = 0.99f;
        
        [Ordered, ContentSerializer(Optional=true)]
        public override float HGrow {
            get => base.HGrow;
            set {
                if(TextItem != null) TextItem.HGrow = value == 0 ? 0 : 1;
                if(ImageItem != null) ImageItem.HGrow = value == 0 ? 0 : 1;
                HPane.HGrow = VPane.HGrow = value == 0 ? 0 : 1;
                base.HGrow = value;
            }
        }

        [Ordered, ContentSerializer(Optional=true)]
        public override float VGrow {
            get => base.VGrow;
            set {
                if(TextItem != null) TextItem.VGrow = value == 0 ? 0 : 1;
                if(ImageItem != null) ImageItem.VGrow = value == 0 ? 0 : 1;
                HPane.VGrow = VPane.VGrow = value == 0 ? 0 : 1;
                base.VGrow = value;
            }
        }

        [Ordered, ContentSerializer(Optional=true)]
        public Orientation Orientation {
            get {return orientation;}
            set {SetProperty(ref orientation, value);}
        }

        [Ordered, ContentSerializer(Optional=true)]
        public TextItem TextItem {
            get => text;
            protected set {
                HPane.Remove(text);
                VPane.Remove(text);
                text = value;
                AlignItems();
            }
        }

        [Ordered, ContentSerializer(Optional=true)]
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
        public Menu Menu {get; internal set;}

        protected HPane HPane {get; set;}
        protected VPane VPane {get; set;}
        
        private TextItem text;
        private ImageItem image;
        private float focusFade;
        private float layerBackup;
        private float extraScale = 0.1f;
        private Orientation orientation;

        public MenuItem() : this("", null, null, 0, 0) {} // for content serializer
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

            HPane.HAlign = HAlignment.Center;
            VPane.HAlign = HAlignment.Center;
            HPane.VAlign = VAlignment.Center;
            VPane.VAlign = VAlignment.Center;

            Orientation = Orientation.Vertical;
            HAlign = HAlignment.Center;
            VAlign = VAlignment.Center;
            _Add(HPane);
            _Add(VPane);
        }

        public override void Load(ContentManager content) {
            if(TextItem != null) TextItem.Load(content);
            if(ImageItem != null) ImageItem.Load(content);
            base.Load(content);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            UpdateFocusedEffect(gameTime);
        }

        protected override void OnFocusGain() {
            layerBackup = InternalLayer;
            Layer = FocusLayer;
            base.OnFocusGain();
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

            if(!IsFocused && focusFade == 0)
                Layer = layerBackup;
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);
            
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
    }
}