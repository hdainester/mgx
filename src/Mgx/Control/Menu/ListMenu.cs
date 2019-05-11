using Microsoft.Xna.Framework.Input;
using Chaotx.Mgx.Layout;

using System.Linq;
using System;

namespace Chaotx.Mgx.Controls.Menus {

    public class ListMenu : Menu {
        private Orientation itemsOrientation;
        public Orientation ItemsOrientation {
            get {return itemsOrientation;}
            set {SetProperty(ref itemsOrientation, value);}
        }

        public ListMenu(params MenuItem[] items)
        : base(items) {}

        protected override void AlignChildren() {
            if(ItemsOrientation == Orientation.Horizontal)
                _OrientHorizontal();

            if(ItemsOrientation == Orientation.Vertical)
                _OrientVertical();

            if(ItemsOrientation == Orientation.RHorizontal)
                _OrientHorizontal(true);

            if(ItemsOrientation == Orientation.RVertical)
                _OrientVertical(true);

            base.AlignChildren();
        }

        private void _OrientHorizontal(bool reverse = false) {
            for(int i = 0; i < Items.Count; ++i)
                hPane.Add(Items[reverse ? Items.Count-1-i : i]);
        }

        private void _OrientVertical(bool reverse = false) {
            for(int i = 0; i < Items.Count; ++i)
                vPane.Add(Items[reverse ? Items.Count-1-i : i]);
        }

        protected override void OnKeyPressed(Keys key) {
            base.OnKeyPressed(key);

            if(ItemsOrientation == Orientation.Horizontal) {
                if(key == Keys.Left) {
                    while(Selected > 0)
                        if(!Items[--Selected].IsDisabled) break;
                }

                if(key == Keys.Right) {
                    while(Selected < Items.Count-1)
                        if(!Items[++Selected].IsDisabled) break;
                }

                if(key == Keys.Left || key  == Keys.Right)
                    if(Items[Selected].IsDisabled)
                        _SetFocus(Items[Selected], false);
            } else

            if(ItemsOrientation == Orientation.RHorizontal) {
                if(key == Keys.Right) Selected = Math.Max(0, Selected-1);
                if(key == Keys.Left) Selected = Math.Min(Items.Count-1, Selected+1);
            } else

            if(ItemsOrientation == Orientation.Vertical) {
                if(key == Keys.Up) Selected = Math.Max(0, Selected-1);
                if(key == Keys.Down) Selected = Math.Min(Items.Count-1, Selected+1);
            } else

            if(ItemsOrientation == Orientation.RVertical) {
                if(key == Keys.Down) Selected = Math.Max(0, Selected-1);
                if(key == Keys.Up) Selected = Math.Min(Items.Count-1, Selected+1);
            }
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);

            // TODO temp solution
            if(propertyName.Equals("HGrow"))
                hPane.HGrow = vPane.HGrow = HGrow;

            if(propertyName.Equals("VGrow"))
                hPane.VGrow = vPane.VGrow = VGrow;

            if(propertyName.Equals("ItemsOrientation"))
                AlignChildren();
        }
    }
}