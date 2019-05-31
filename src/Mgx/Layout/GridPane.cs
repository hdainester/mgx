using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

namespace Chaotx.Mgx.Layout {
    public class GridPane : StackPane {
        [Ordered, ContentSerializer(Optional=true)]
        public HAlignment CellsHAlign {get; set;}

        [Ordered, ContentSerializer(Optional=true)]
        public VAlignment CellsVAlign {get; set;}

        [Ordered, ContentSerializer(Optional=true)]
        public bool CompressCells {get; set;}

        [Ordered, ContentSerializer(FlattenContent=true, CollectionItemName="Row")]
        private List<Row> Rows {
            get => _rows; // TODO (not required but exception in ParseAttributes if collection is empty or null)
            set {
                int x, y = 0;
                _rows = value; // TODO temp fix (see above)
                value.ForEach(row => {
                    x = 0;
                    row.Components.ForEach(c =>
                        Set(x++, y, c));

                    var gridRow = rows.Children[y++] as GridRow;
                    gridRow.HAlign = row.HAlign;
                    gridRow.VAlign = row.VAlign;
                });
            }
        }

        [ContentSerializerIgnore]
        public int GridWidth {get; protected set;}

        [ContentSerializerIgnore]
        public int GridHeight {get; protected set;}
        
        private Dictionary<Point, GridCell> cells;
        private GridRows rows;
        private List<Row> _rows;

        public GridPane() {
            _rows = new List<Row>();
            cells = new Dictionary<Point, GridCell>();
            CellsHAlign = HAlignment.Center;
            CellsVAlign = VAlignment.Center;
            rows = new GridRows();
        }

        public void Set(int x, int y, Component child) {
            ExpandTo(x, y);
            cells[new Point(x, y)].Content = child;
        }

        public Component Get(int x, int y) {
            return cells[new Point(x, y)].Content;
        }

        public override void Add(Component child) {
            Set(GridWidth, GridHeight, child);
        }

        public override void Remove(Component child) {
            throw new System.NotImplementedException(
                "cannot remove from GridPane. Use Set(x, y, null) instead.");
        }

        public override void Load(ContentManager content) {
            // fixes StackOverflowException thrown by
            // ContentProcessor when added in ctor
            // TODO temp solution (load may not be called)
            // -> need to find cause of stackoverflow
            if(Children.Count == 0) _Add(rows);
            base.Load(content);
        }

        protected override void AlignChildren() {
            // TODO temp solution (in case load was not called)
            if(Children.Count == 0) _Add(rows);
            _AlignCells();
            base.AlignChildren();
        }

        private void _AlignCells() {
            float cellWidth = 0, cellHeight = 0;
            if(HGrow > 0) cellWidth = Width/GridWidth;
            if(VGrow > 0) cellHeight = Height/GridHeight;
            if(HGrow == 0 || VGrow == 0) {
                cells.Values.ToList().ForEach(cell => {
                    if(HGrow == 0 && cell.Content != null
                    && cell.Content.Width > cellWidth)
                        cellWidth = cell.Content.Width;

                    if(VGrow == 0 && cell.Content != null
                    && cell.Content.Height > cellHeight)
                        cellHeight = cell.Content.Height;
                });

                cells.Values.ToList().ForEach(cell => {
                    cell.Width = cellWidth;
                    cell.Height = cellHeight;
                });
            }

            cells.Values.ToList().ForEach(cell => {
                cell.HAlign = CellsHAlign;
                cell.VAlign = CellsVAlign;
            });
        }

        private void ExpandTo(int x, int y) {
            for(int _x, _y = 0; _y <= y; ++_y) {
                if(_y >= rows.Children.Count)
                    rows.Add(new GridRow(rows));

                var row = rows.Children[_y] as GridRow;
                for(_x = 0; _x <= x; ++_x) {
                    if(_x >= row.Children.Count) {
                        var cell = new GridCell(row, null);
                        cells.Add(new Point(_x, _y), cell);
                        row.Add(cell);
                    }
                }
            }

            GridHeight = rows.Children.Count;
            GridWidth = rows.Children.Count > 0
                ? (rows.Children[0] as HPane).Children.Count : 0;
        }
    }
}