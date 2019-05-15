using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

using Chaotx.Mgx.Assets;

namespace Chaotx.Mgx.Controls.Menus {
    public abstract class Menu : MenuItem {
        [ContentSerializer(FlattenContent = true, CollectionItemName = "MenuItemAsset")]
        private List<ComponentAsset<MenuItem>> MenuItemAssets {get; set;}

        [ContentSerializerIgnore]
        public ReadOnlyCollection<MenuItem> Items {
            get {return items.AsReadOnly();}
            protected set {items = new List<MenuItem>(value);}
        }

        [ContentSerializerIgnore]
        public int Selected {
            get {return selected;}
            set {SetProperty(ref selected, value);}
        }

        private List<MenuItem> items;
        private MenuItem item;
        private int selected;

        public Menu(params MenuItem[] items) : base(null, null, null, 0, 0) {
            this.items = new List<MenuItem>();
            foreach(var item in items) AddItem(item);
        }

        public override void Load(ContentManager content) {
            MenuItemAssets.ForEach(asset => {
                asset.Load(content);
                AddItem(asset.Object);
            });

            base.Load(content);
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
            // nothing here (yet)
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