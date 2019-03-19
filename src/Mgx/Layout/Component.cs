using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Runtime.CompilerServices;
using System.ComponentModel;
using System;

namespace Mgx.Layout {
    public abstract class Component : INotifyPropertyChanged {
        private int hgrow;
        public int HGrow {
            get {return hgrow;}
            set {SetProperty(ref hgrow, value);}
        }

        private int vgrow;
        public int VGrow {
            get {return vgrow;}
            set {SetProperty(ref vgrow, value);}
        }

        private HAlignment halign;
        public HAlignment HAlign {
            get {return halign;}
            set {SetProperty(ref halign, value);}
        }

        private VAlignment valign;
        public VAlignment VAlign {
            get {return valign;}
            set {SetProperty(ref valign, value);}
        }

        private Container parent;
        public Container Parent {
            get {return parent;}
            protected set {SetProperty(ref parent, value);}
        }

        private Color color = Color.White;
        public Color Color {
            get {return color;}
            set {SetProperty(ref color, value);}
        }
        
        private float alpha = 1f;
        public float Alpha {
            get {return Parent == null ? alpha : alpha*Parent.Alpha;}
            set {alpha = value;}
        }

        private Vector2 position;
        public Vector2 Position {
            get {return position;}
            protected set {SetProperty(ref position, value);}
        }

        public float X {
            get {return Position.X;}
            protected set {Position = new Vector2(value, Position.Y);}
        }

        public float Y {
            get {return Position.Y;}
            protected set {Position = new Vector2(Position.X, value);}
        }

        private Vector2 size;
        public Vector2 Size {
            get {return size;}
            protected set {SetProperty(ref size, value);}
        }

        public float Width {
            get {return Size.X;}
            protected set {Size = new Vector2(value, Size.Y);}
        }

        public float Height {
            get {return Size.Y;}
            protected set {Size = new Vector2(Size.X, value);}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Load(ContentManager content) {}
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string name = "") {
            if(!value.Equals(field)) {
                field = value;
                OnPropertyChanged(name);
            }
        }

        protected static void _SetParent(Component c, Container parent) {
            c.Parent = parent;
        }

        protected static void _SetX(Component c, float x) {
            c.X = x;
        }

        protected static void _SetY(Component c, float y) {
            c.Y = y;
        }

        protected static void _SetPosition(Component c, Vector2 position) {
            c.Position = position;
        }

        protected static void _SetWidth(Component c, float width) {
            c.Width = width;
        }

        protected static void _SetHeight(Component c, float height) {
            c.Height = height;
        }

        protected static void _SetSize(Component c, Vector2 size) {
            c.Size = size;
        }
    }
}