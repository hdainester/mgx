using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;

using Chaotx.Mgx.Controls;
using Chaotx.Mgx.Views;

namespace Chaotx.Mgx.Layout {
    public abstract class Container : Component {
        [ContentSerializerIgnore]
        public override Vector2 Position {
            get => base.Position;
            internal set {
                if(value.Equals(Position))
                    return;
                    
                var diff = value - Position;
                base.Position = value;

                Children.ToList().ForEach(child =>
                    child.Position += diff);
            }
        }

        [ContentSerializerIgnore]
        public ReadOnlyCollection<Component> Children {
            get => children.AsReadOnly();
            protected set => children = new List<Component>(value);
        }

        [ContentSerializerIgnore]
        public ReadOnlyCollection<Container> Containers {
            get {return containers.AsReadOnly();}
            protected set {containers = new List<Container>(value);}
        }

        [ContentSerializerIgnore]
        public ReadOnlyCollection<Control> Controls {
            get {return controls.AsReadOnly();}
            protected set {controls = new List<Control>(value);}
        }

        [ContentSerializerIgnore]
        public View ParentView {
            get {return Parent != null ? Parent.ParentView : parentView;}
            internal set {
                if(Parent != null) Parent.ParentView = value;
                else parentView = value;
            }
        }

        [ContentSerializerIgnore]
        internal bool AlignmentPending {
            get => alignmentPending;
            set => alignmentPending = value;
        }

        [ContentSerializerIgnore]
        internal virtual bool InitialAligned => initialAligned
            && (Parent == null || Parent.InitialAligned);

        private View parentView;
        private bool alignmentPending;
        private bool initialAligned;
        private List<Component> children = new List<Component>();
        private List<Container> containers = new List<Container>();
        private List<Control> controls = new List<Control>();
        
        public override void Load(ContentManager content) {
            base.Load(content);
            children.ForEach(child => child.Load(content));
        }

        public override void Update(GameTime gameTime) {
            if(AlignmentPending) {
                AlignmentPending = false;
                AlignChildren();
            }

            for(int i = children.Count-1; i >= 0; --i)
                children[i].Update(gameTime);

            if(!initialAligned) initialAligned =
                !AlignmentPending && !ParentView.ViewPane.AlignmentPending;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if(InitialAligned)
                children.ForEach(c => c.Draw(spriteBatch));
        }

        protected virtual void ChildPropertyChangedHandler(
        object sender, PropertyChangedEventArgs args) {
            if(args.PropertyName.Equals("HAlign")
            || args.PropertyName.Equals("VAlign")
            || args.PropertyName.Equals("HGrow")
            || args.PropertyName.Equals("VGrow")
            // || args.PropertyName.Equals("Scale") // deprecated (too busy)
            || args.PropertyName.Equals("Size")) {
            // || args.PropertyName.Equals("Position")) { // deprecated (Position update in property now)
                if(ParentView != null)
                    ParentView.ViewPane.AlignmentPending = true;
                else {
                    Container root = this;
                    while(root.Parent != null) root = root.Parent;
                    root.AlignmentPending = true;
                }
            }
        }

        internal virtual void AlignChildren() {
            Containers.OrderBy(c => c.HGrow + c.VGrow)
                .ToList().ForEach(c => {
                    c.AlignChildren();
                    c.AlignmentPending = false;
                });

            if((HGrow == 0 || VGrow == 0) && Children.Count > 0) {
                foreach(var child in Children)
                    if(HGrow == 0 && child.HGrow == 0
                    || VGrow == 0 && child.VGrow == 0)
                        return;

                throw new Exception("Malformed layout");
            }
        }

        internal void _Add(Component child) {
            if(child.Parent != null)
                child.Parent._Remove(child);

            child.Parent = this;
            children.Add(child);
            Control control = child as Control;
            Container container = child as Container;
            if(control != null) controls.Add(control);
            if(container != null) containers.Add(container);
            child.PropertyChanged += ChildPropertyChangedHandler;
            AlignmentPending = true;
        }

        internal void _Remove(Component child) {
            if(children.Remove(child)) {
                child.Parent = null;
                Control control = child as Control;
                Container container = child as Container;
                if(control != null) controls.Remove(control);
                if(container != null) containers.Remove(container);
                child.PropertyChanged -= ChildPropertyChangedHandler;
                AlignmentPending = true;
            }
        }

        /// Default alignment for children is for each child
        /// determined independent of the other children
        internal void _DefaultAlign() {
            float w = 0, h = 0;

            if(HGrow == 0 || VGrow == 0) {
                Children.ToList().ForEach(child => {
                    if(HGrow == 0 && child.HGrow == 0 && child.Width > w) w = child.Width;
                    if(VGrow == 0 && child.VGrow == 0 && child.Height > h) h = child.Height;
                });
                
                if(HGrow == 0) Width = w;
                if(VGrow == 0) Height = h;
            }

            Children.ToList().ForEach(child => {
                if(child.HGrow > 0) child.Width = Math.Min(1, child.HGrow)*Width;
                if(child.VGrow > 0) child.Height = Math.Min(1, child.VGrow)*Height;
                if(child.HAlign == HAlignment.Left) child.X = X;
                if(child.HAlign == HAlignment.Center) child.X = X + Width/2 - child.Width/2;
                if(child.HAlign == HAlignment.Right) child.X = X + Width - child.Width;
                if(child.VAlign == VAlignment.Top) child.Y = Y;
                if(child.VAlign == VAlignment.Center) child.Y = Y + Height/2 - child.Height/2;
                if(child.VAlign == VAlignment.Bottom) child.Y = Y + Height - child.Height;
            });
        }
    }
}