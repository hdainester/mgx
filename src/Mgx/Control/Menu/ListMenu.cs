using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Mgx.Control.Menu {
    using Layout;

    public class ListMenu : Menu {
        public int SelectedIndex {get; protected set;}

        private Orientation itemsOrientation;
        public Orientation ItemsOrientation {
            get {return itemsOrientation;}
            set {SetProperty(ref itemsOrientation, value);}
        }

        private HPane hPane;
        private VPane vPane;

        public ListMenu(params MenuItem[] items) : base(items) {
            hPane = new HPane();
            vPane = new VPane();
            hPane.HAlign = vPane.HAlign = HAlignment.Center;
            hPane.VAlign = vPane.VAlign = VAlignment.Center;
            // hPane.HGrow = hPane.VGrow = 1;
            // vPane.HGrow = vPane.VGrow = 1;

            _Add(hPane);
            _Add(vPane);
            AlignItems();
        }

        protected override void AlignItems() {
            if(ItemsOrientation == Orientation.Horizontal)
                _OrientHorizontal();

            if(ItemsOrientation == Orientation.Vertical)
                _OrientVertical();

            if(ItemsOrientation == Orientation.RHorizontal)
                _OrientHorizontal(true);

            if(ItemsOrientation == Orientation.RVertical)
                _OrientVertical(true);
        }

        private void _OrientHorizontal(bool reverse = false) {
            for(int i = 0; i < Items.Count; ++i)
                hPane.Add(Items[reverse ? Items.Count-1-i : i]);
        }

        private void _OrientVertical(bool reverse = false) {
            for(int i = 0; i < Items.Count; ++i)
                vPane.Add(Items[reverse ? Items.Count-1-i : i]);
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);
            if(propertyName.Equals("ItemsOrientation"))
                AlignItems();
        }
    }
}