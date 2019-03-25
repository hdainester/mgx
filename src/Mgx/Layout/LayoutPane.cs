namespace Mgx.Layout {
    using  Control;

    public abstract class LayoutPane : Container {
        public LayoutPane(params Component[] children) {
            foreach(Component child in children)
                Add(child);
        }

        public void Add(Component child) {
            _Add(child);
        }

        public void Remove(Component child) {
            _Remove(child);
        }
    }
}