using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Mgx.Control.Menu {
    public abstract class Menu : MenuItem {
        private List<MenuItem> items;
        public ReadOnlyCollection<MenuItem> Items {
            get {return items.AsReadOnly();}
            protected set {items = new List<MenuItem>(value);}
        }

        public Menu(params MenuItem[] items) : base(null, null, null, 0, 0) {
            this.items = new List<MenuItem>(items);
        }

        public void AddItem(MenuItem item) {
            if(!items.Contains(item)) {
                items.Add(item);
                AlignItems();
            }
        }

        public void RemoveItem(MenuItem item) {
            if(items.Remove(item))
                AlignItems();
        }

        protected abstract void AlignItems();
    }
}