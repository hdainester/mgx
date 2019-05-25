namespace Chaotx.Mgx.Layout {
    // TODO
    public class GridCell : Container {
        public Component Content {get;}

        public GridCell(Component content) {
            Content = content;
        }
    }

    // TODO
    public class GridPane : LayoutPane {
        public int GridWidth {get; protected set;}
        public int GridHeight {get; protected set;}

        private VPane rows = new VPane();

        public void Set(int x, int y, Component child) {
            // (rows.Containers[y] as HPane).
        }

        public Component Get(int x, int y) {
            return null;
        }
    }
}