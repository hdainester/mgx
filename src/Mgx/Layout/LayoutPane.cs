using Chaotx.Mgx.Controls;

namespace Chaotx.Mgx.Layout {
    public abstract class LayoutPane : Container {
        public LayoutPane() : this(new Component[0]) {}
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

        public void Clear() {
            for(int i = Children.Count-1; i >= 0; --i)
                Remove(Children[i]);
        }
    }
}