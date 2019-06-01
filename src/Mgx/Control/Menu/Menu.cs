using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System;

using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Controls.Menus {
    public abstract class Menu : Control {
        [Ordered, ContentSerializer(FlattenContent = true, CollectionItemName = "MenuItem")]
        private List<MenuItem> _Items {
            get => items;
            set => value.ForEach(AddItem);
        }

        [ContentSerializerIgnore]
        public ReadOnlyCollection<MenuItem> Items {
            get {return items.AsReadOnly();}}

        [ContentSerializerIgnore]
        public int Selected {
            get {return selected;}
            set {SetProperty(ref selected, value);}
        }

        protected HPane HPane {get; set;}
        protected VPane VPane {get; set;}

        private List<MenuItem> items;
        private MenuItem item;
        private int selected;
        
        public Menu(params MenuItem[] items) {
            _Add(HPane = new HPane());
            _Add(VPane = new VPane());
            HPane.HAlign = HAlignment.Center;
            VPane.HAlign = HAlignment.Center;
            HPane.VAlign = VAlignment.Center;
            VPane.VAlign = VAlignment.Center;
            this.items = new List<MenuItem>();
            foreach(var item in items) AddItem(item);
        }

        public override void Load(ContentManager content) {
            Items.ToList().ForEach(item => item.Load(content));
            base.Load(content);
        }

        public void AddItem(MenuItem item) {
            if(!items.Contains(item)) {
                item.FocusGain += OnItemFocusGain;
                item.FocusLoss += OnItemFocusLoss;
                items.Add(item);
                AlignChildren();
                item.Menu = this;
            }
        }

        public void RemoveItem(MenuItem item) {
            if(items.Remove(item)) {
                if(item == this.item) {
                    item.IsFocused = false;
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
            // nothing here (yet)
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);

            if(propertyName == "Selected") {
                if(Selected < Items.Count) {
                    if(item != null)
                        item.IsFocused = false;

                    item = Items[Selected];
                    item.IsFocused = true;
                }
            }
        }
    }
}