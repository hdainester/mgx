using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;

namespace Chaotx.Mgx.Layout {
    using Control;
    using View;

    public abstract class Container : Component {
        private View parentView;
        public View ParentView {
            get {return Parent != null ? Parent.ParentView : parentView;}
            protected set {
                if(Parent != null) Parent.ParentView = value;
                else parentView = value;
            }
        }

        private bool alignmentPending;
        private List<Component> children = new List<Component>();
        private List<Container> containers = new List<Container>();
        private List<Control> controls = new List<Control>();

        public ReadOnlyCollection<Component> Children {
            get {return children.AsReadOnly();}
            protected set {children = new List<Component>(value);}
        }

        public ReadOnlyCollection<Container> Containers {
            get {return containers.AsReadOnly();}
            protected set {containers = new List<Container>(value);}
        }

        public ReadOnlyCollection<Control> Controls {
            get {return controls.AsReadOnly();}
            protected set {controls = new List<Control>(value);}
        }

        public override void Update(GameTime gameTime) {
            if(alignmentPending) {
                alignmentPending = false;
                AlignChildren();
            }

            if(ParentView != null
            && !ParentView.InputDisabled
            && ParentView.State == ViewState.Open)
                controls.ForEach(c => c.HandleInput());

            for(int i = children.Count-1; i >= 0; --i)
                children[i].Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if(ParentView.State != ViewState.Opening
            || ParentView.State == ViewState.Opening && !alignmentPending)
                children.ForEach(c => c.Draw(spriteBatch));
        }

        protected virtual void ChildPropertyChangedHandler(
        object sender, PropertyChangedEventArgs args) {
            if(args.PropertyName.Equals("HAlign")
            || args.PropertyName.Equals("VAlign")
            || args.PropertyName.Equals("HGrow")
            || args.PropertyName.Equals("VGrow")
            || args.PropertyName.Equals("Scale")
            || args.PropertyName.Equals("Size")
            || args.PropertyName.Equals("Position")) {
                Container root = this;
                while(root.Parent != null) root = root.Parent;
                root.alignmentPending = true;
            }
        }

        protected virtual void AlignChildren() {
            Containers.OrderBy(c => c.HGrow + c.VGrow)
                .ToList().ForEach(c => {
                    c.AlignChildren();
                    c.alignmentPending = false;
                });

            if((HGrow == 0 || VGrow == 0) && Children.Count > 0) {
                foreach(var child in Children)
                    if(HGrow == 0 && child.HGrow == 0
                    || VGrow == 0 && child.VGrow == 0)
                        return;

                throw new Exception("Malformed layout");
            }
        }

        protected void _Add(Component child) {
            if(child.Parent != null)
                child.Parent._Remove(child);
                
            _SetParent(child, this);
            children.Add(child);
            Control control = child as Control;
            Container container = child as Container;
            if(control != null) controls.Add(control);
            if(container != null) containers.Add(container);
            child.PropertyChanged += ChildPropertyChangedHandler;
            alignmentPending = true;
        }

        protected void _Remove(Component child) {
            if(children.Remove(child)) {
                _SetParent(child, null);
                Control control = child as Control;
                Container container = child as Container;
                if(control != null) controls.Remove(control);
                if(container != null) containers.Remove(container);
                child.PropertyChanged -= ChildPropertyChangedHandler;
                alignmentPending = true;
            }
        }

        /// Default alignment for children is for each child
        /// determined independent of the other children
        protected void _DefaultAlign() {
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
                if(child.HGrow > 0) _SetWidth(child, Math.Min(1, child.HGrow)*Width);
                if(child.VGrow > 0) _SetHeight(child, Math.Min(1, child.VGrow)*Height);
                if(child.HAlign == HAlignment.Left) _SetX(child, X);
                if(child.HAlign == HAlignment.Center) _SetX(child, X + Width/2 - child.Width/2);
                if(child.HAlign == HAlignment.Right) _SetX(child, X + Width - child.Width);
                if(child.VAlign == VAlignment.Top) _SetY(child, Y);
                if(child.VAlign == VAlignment.Center) _SetY(child, Y + Height/2 - child.Height/2);
                if(child.VAlign == VAlignment.Bottom) _SetY(child, Y + Height - child.Height);
            });
        }
    }
}