using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace Chaotx.Mgx.Control.Menu {
    public abstract class Menu : MenuItem {
        private List<MenuItem> items;
        public ReadOnlyCollection<MenuItem> Items {
            get {return items.AsReadOnly();}
            protected set {items = new List<MenuItem>(value);}
        }

        private MenuItem item;
        private int selected;
        public int Selected {
            get {return selected;}
            set {SetProperty(ref selected, value);}
        }

        public Menu(params MenuItem[] items) : base(null, null, null, 0, 0) {
            this.items = new List<MenuItem>();
            foreach(var item in items) AddItem(item);
        }

        public void AddItem(MenuItem item) {
            if(!items.Contains(item)) {
                item.FocusGain += OnItemFocusGain;
                item.FocusLoss += OnItemFocusLoss;
                items.Add(item);
                AlignChildren();
                _SetMenu(item, this);
            }
        }

        public void RemoveItem(MenuItem item) {
            if(items.Remove(item)) {
                if(item == this.item) {
                    _SetFocus(item, false);
                    this.item = null;
                }

                AlignChildren();
            }
        }

        private void OnItemFocusLoss(object item, EventArgs args) {
            if(item == this.item)
                this.item = null;
        }

        private void OnItemFocusGain(object item, EventArgs args) {
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);

            if(propertyName == "Selected") {
                if(Selected < Items.Count) {
                    if(item != null)
                        _SetFocus(item, false);

                    item = Items[Selected];
                    _SetFocus(item, true);
                }
            }
        }
    }
}