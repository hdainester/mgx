using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Runtime.CompilerServices;
using System.ComponentModel;
using System;

namespace Mgx.Layout {
    public abstract class Component : INotifyPropertyChanged {
        public int HGrow {get; set;}
        public int VGrow {get; set;}
        public HAlignment HAlign {get; set;}
        public VAlignment VAlign {get; set;}
        public Container Parent {get; protected set;}
        public Color Color {get; set;} = Color.White;
        // public float Alpha {get; set;}

        public event PropertyChangedEventHandler PropertyChanged;

        private Vector2 position;
        public Vector2 Position {
            get {return position;}
            protected set {
                if(value != position)
                    SetProperty(ref position, value);
            }
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
            protected set {
                if(value != size)
                    SetProperty(ref size, value);
            }
        }

        public float Width {
            get {return Size.X;}
            protected set {Size = new Vector2(value, Size.Y);}
        }

        public float Height {
            get {return Size.Y;}
            protected set {Size = new Vector2(Size.X, value);}
        }

        public virtual void Load(ContentManager content) {}
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string name = "") {
            field = value;
            OnPropertyChanged(name);
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