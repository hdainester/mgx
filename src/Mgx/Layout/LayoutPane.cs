using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Chaotx.Mgx.Controls;
using System.Linq;
using System;

namespace Chaotx.Mgx.Layout {
    public abstract class LayoutPane : Container {
        [Ordered, ContentSerializer(FlattenContent = true, CollectionItemName = "Component")]
        private List<Component> _Children {
            get => Children.ToList();
            set => value.ForEach(Add);
        }

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