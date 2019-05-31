using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System;

namespace Chaotx.Mgx.Layout {
    public abstract class Component : INotifyPropertyChanged {
        [Ordered, ContentSerializer(Optional=true)] // TODO: Id is not set in Mgx pipeline with ContentSerializerIgnore
        public string Id {get; internal set;}

        [Ordered, ContentSerializer(Optional=true)]
        public float Alpha {
            get {return Parent == null ? alpha : alpha*Parent.Alpha;}
            set {SetProperty(ref alpha, value);}
        }

        [Ordered, ContentSerializer(Optional=true)]
        public virtual float HGrow {
            get {return hGrow;}
            set {SetProperty(ref hGrow, value);}
        }

        [Ordered, ContentSerializer(Optional=true)]
        public virtual float VGrow {
            get {return vGrow;}
            set {SetProperty(ref vGrow, value);}
        }

        [Ordered, ContentSerializer(Optional=true)]
        public HAlignment HAlign {
            get {return hAlign;}
            set {SetProperty(ref hAlign, value);}
        }

        [Ordered, ContentSerializer(Optional=true)]
        public VAlignment VAlign {
            get {return vAlign;}
            set {SetProperty(ref vAlign, value);}
        }

        [ContentSerializerIgnore]
        public virtual Vector2 Position {
            get {return position;}
            internal set {SetProperty(ref position, value);}
        }

        [ContentSerializerIgnore]
        public float X {
            get {return Position.X;}
            internal set {Position = new Vector2(value, Position.Y);}
        }

        [ContentSerializerIgnore]
        public float Y {
            get {return Position.Y;}
            internal set {Position = new Vector2(Position.X, value);}
        }

        [ContentSerializerIgnore]
        public virtual Vector2 Size {
            get {return size;}
            internal set {SetProperty(ref size, value);}
        }

        [ContentSerializerIgnore]
        public float Width {
            get {return Size.X;}
            internal set {Size = new Vector2(value, Size.Y);}
        }

        [ContentSerializerIgnore]
        public float Height {
            get {return Size.Y;}
            internal set {Size = new Vector2(Size.X, value);}
        }

        [ContentSerializerIgnore]
        public Container Parent {
            get {return parent;}
            internal set {SetProperty(ref parent, value);}
        }

        [ContentSerializerIgnore]
        public float Layer {
            get => layer + (Parent != null ? Parent.Layer : 0);
            internal set => SetProperty(ref layer, value);
        }

        private float alpha = 1f;
        private float layer;
        private float hGrow;
        private float vGrow;
        private HAlignment hAlign;
        private VAlignment vAlign;
        private Vector2 position;
        private Vector2 size;
        private Container parent;

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void Load(ContentManager content) {}
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public bool ContainsPoint(Point position) {
            return ContainsPoint(position.X, position.Y);
        }

        public bool ContainsPoint(Vector2 position) {
            return ContainsPoint(position.X, position.Y);
        }

        public bool ContainsPoint(float x, float y) {
            return x >= X && x - X < Width
                && y >= Y && y - Y < Height;
        }

        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string name = "") {
            if(field != null && value == null
            || !value.Equals(field)) {
                field = value;
                OnPropertyChanged(name);
            }
        }
    }
}