using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Mgx.Layout {
    using Control;

    public abstract class Container : Component {
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

        public void Add(Component child) {
            if(!children.Contains(child)) {
                if(child.Parent != null)
                    child.Parent.Remove(child);
                    
                _SetParent(child, this);
                children.Add(child);
                Control control = child as Control;
                Container container = child as Container;
                if(control != null) controls.Add(control);
                if(container != null) containers.Add(container);
                child.PropertyChanged += ChildPropertyChangedHandler;
                alignmentPending = true;
            }
        }

        public void Remove(Component child) {
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

        public override void Update(GameTime gameTime) {
            if(alignmentPending) {
                alignmentPending = false;
                AlignChildren();
            }

            controls.ForEach(c => c.HandleInput());
            children.ForEach(c => c.Update(gameTime));
        }

        public override void Draw(SpriteBatch spriteBatch) {
            children.ForEach(c => c.Draw(spriteBatch));
        }

        private bool alignmentPending;
        protected virtual void ChildPropertyChangedHandler(
        object sender, PropertyChangedEventArgs args) {
            if(args.PropertyName.Equals("HAlign")
            || args.PropertyName.Equals("VAlign")
            || args.PropertyName.Equals("HGrow")
            || args.PropertyName.Equals("VGrow")
            || args.PropertyName.Equals("Size")
            || args.PropertyName.Equals("Position")) {
                alignmentPending = true;
                Container container = sender as Container;
                if(container != null) container.alignmentPending = true;
            }
        }

        protected virtual void AlignChildren() {}
    }
}