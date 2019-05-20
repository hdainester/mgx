using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System;

namespace Chaotx.Mgx.Layout {
    public abstract class Component : INotifyPropertyChanged, IReflective {
        [Ordered, ContentSerializer(Optional = true)]
        public string Id {get; internal set;}

        [Ordered, ContentSerializer(Optional=true)]
        public float Alpha {
            get {return Parent == null ? alpha : alpha*Parent.Alpha;}
            set {SetProperty(ref alpha, value);}
        }

        [Ordered, ContentSerializer(Optional=true)]
        public float HGrow {
            get {return hGrow;}
            set {SetProperty(ref hGrow, value);}
        }

        [Ordered, ContentSerializer(Optional=true)]
        public float VGrow {
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
            protected set {SetProperty(ref position, value);}
        }

        [ContentSerializerIgnore]
        public float X {
            get {return Position.X;}
            protected set {Position = new Vector2(value, Position.Y);}
        }

        [ContentSerializerIgnore]
        public float Y {
            get {return Position.Y;}
            protected set {Position = new Vector2(Position.X, value);}
        }

        [ContentSerializerIgnore]
        public virtual Vector2 Size {
            get {return size;}
            protected set {SetProperty(ref size, value);}
        }

        [ContentSerializerIgnore]
        public float Width {
            get {return Size.X;}
            protected set {Size = new Vector2(value, Size.Y);}
        }

        [ContentSerializerIgnore]
        public float Height {
            get {return Size.Y;}
            protected set {Size = new Vector2(Size.X, value);}
        }

        [ContentSerializerIgnore]
        public Container Parent {
            get {return parent;}
            protected set {SetProperty(ref parent, value);}
        }

        private float alpha = 1f;
        private float hGrow;
        private float vGrow;
        private HAlignment hAlign;
        private VAlignment vAlign;
        private Vector2 position;
        private Vector2 size;
        private Container parent;

        // deprecated
        private HashSet<string> setProperties = new HashSet<string>();
        
        internal List<string> _DeclaredProperties
            {get; set;} = new List<string>();

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
            return x > X && x - X < Width && y > Y && y - Y < Height;
        }

        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string name = "") {
            if(field != null && value == null
            || !value.Equals(field)) {
                // TODO: this is a temporary solution to keep
                // track of what properties have been set so
                // they may override template properties within
                // an asset when loaded through content pipeline
                setProperties.Add(name);
                
                field = value;
                OnPropertyChanged(name);
            }
        }

        protected static void _SetPosition(Component c, Vector2 position) {
            c.Position = position;
        }

        protected static void _SetX(Component c, float x) {
            c.X = x;
        }

        protected static void _SetY(Component c, float y) {
            c.Y = y;
        }
        
        protected static void _SetSize(Component c, Vector2 size) {
            c.Size = size;
        }

        protected static void _SetWidth(Component c, float width) {
            c.Width = width;
        }

        protected static void _SetHeight(Component c, float height) {
            c.Height = height;
        }

        protected static void _SetParent(Component c, Container parent) {
            c.Parent = parent;
        }

        // Use this method to set the value of an properties
        // underlying private variable bypassing its set-method.
        // The propertyName is automatically converted to a
        // name matching the default naming conventions of
        // private attributes (e.g. MyProperty => myProperty).
        // Properties with no such underlying variable are
        // not supported.
        // deprecated
        public void RawSet(string propertyName, object value) {
            var name = char.ToLower(propertyName[0]) + propertyName.Substring(1);
            var field = GetField(name, GetType(), typeof(Component));

            if(field == default(FieldInfo))
                throw new Exception("No such property '" + name + "'");

            field.SetValue(this, value);
        }

        // deprecated
        public bool WasPropertySet(string propertyName) {
            return setProperties.Contains(propertyName);
        }

        // TODO complete IReflective
        internal static FieldInfo GetField(string fieldName, Type type, Type topType) {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            var field = Array.Find(fields, fi => fi.Name.Equals(fieldName));
            if(field != default(FieldInfo)) return field;
            if(type == topType) return null;
            return GetField(fieldName, type.BaseType, topType);
        }

        public bool IsDeclared(string propertyName) {
            return _DeclaredProperties.Contains(propertyName);
        }
    }
}