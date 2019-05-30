using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace Chaotx.Mgx.Layout {
    internal class GridRows : VPane {
        public override float HGrow {get => Parent.HGrow > 0 ? 1 : 0;}
        public override float VGrow {get => Parent.VGrow > 0 ? 1 : 0;}
    }

    internal class GridRow : HPane {
        public GridRows Rows {get;}
        public override float HGrow {get => Rows.HGrow;}
        public override float VGrow {get => Rows.VGrow;}
        public GridRow(GridRows rows) {
            HAlign = HAlignment.Center;
            VAlign = VAlignment.Center;
            Rows = rows;
        }
    }

    internal class GridCell : StackPane {
        public GridRow Row {get;}
        public new float Width {get; set;}
        public new float Height {get; set;}
        public override float HGrow {get => Row.HGrow;}
        public override float VGrow {get => Row.VGrow;}
        public override Vector2 Size {get {
            if((Row.Rows.Parent as GridPane).CompressCells)
                return base.Size;

            float w = HGrow > 0 ? base.Size.X : Width;
            float h = VGrow > 0 ? base.Size.Y : Height;
            return new Vector2(w, h);
        }}

        public Component Content {
            get => content;
            set {
                if(content != null)
                    _Remove(content);

                if(value != null)
                    _Add(value);

                content = value;
            }
        }

        private Component content;
        public GridCell(GridRow row, Component content = null) {
            Row = row;
            Content = content;
        }
    }

    internal class Row {
        [Ordered, ContentSerializer(Optional=true)]
        public HAlignment HAlign {get; set;}

        [Ordered, ContentSerializer(Optional=true)]
        public VAlignment VAlign {get; set;}

        [Ordered, ContentSerializer(FlattenContent=true, CollectionItemName="Component")]
        public List<Component> Components {get; set;}
    }
}