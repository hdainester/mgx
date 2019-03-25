using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Mgx.Layout {
    using Control;

    public abstract class Container : Component {
        private bool alignmentPending;
        private List<Component> children = new List<Component>();
        private List<Container> containers = new List<Container>();
        private List<IControlable> controls = new List<IControlable>();

        public ReadOnlyCollection<Component> Children {
            get {return children.AsReadOnly();}
            protected set {children = new List<Component>(value);}
        }

        public ReadOnlyCollection<Container> Containers {
            get {return containers.AsReadOnly();}
            protected set {containers = new List<Container>(value);}
        }

        public ReadOnlyCollection<IControlable> Controls {
            get {return controls.AsReadOnly();}
            protected set {controls = new List<IControlable>(value);}
        }

        public Container(params Component[] children) {
            foreach(Component child in children)
                Add(child);
        }

        public void Add(Component child) {
            if(child.Parent != null)
                child.Parent.Remove(child);
                
            _SetParent(child, this);
            children.Add(child);
            IControlable control = child as IControlable;
            Container container = child as Container;
            if(control != null) controls.Add(control);
            if(container != null) containers.Add(container);
            child.PropertyChanged += ChildPropertyChangedHandler;
            alignmentPending = true;
        }

        public void Remove(Component child) {
            if(children.Remove(child)) {
                _SetParent(child, null);
                IControlable control = child as IControlable;
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

        protected virtual void ChildPropertyChangedHandler(
        object sender, PropertyChangedEventArgs args) {
            if(args.PropertyName.Equals("HAlign")
            || args.PropertyName.Equals("VAlign")
            || args.PropertyName.Equals("HGrow")
            || args.PropertyName.Equals("VGrow")
            || args.PropertyName.Equals("Size")
            || args.PropertyName.Equals("Position")) {
                alignmentPending = true;
                // TODO may be unnecesary since sender should realign anyway
                Container container = sender as Container;
                if(container != null) container.alignmentPending = true;
            }
        }

        protected virtual void AlignChildren() {
            // TODO default alignment like StackPane
        }
    }
}