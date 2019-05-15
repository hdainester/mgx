using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Chaotx.Mgx.Controls;
using System;
using Chaotx.Mgx.Assets;

namespace Chaotx.Mgx.Layout {
    public abstract class LayoutPane : Container {
        [ContentSerializer(FlattenContent = true, CollectionItemName = "ComponentAsset")]
        private List<Asset<Component>> ComponentAssets {get; set;}

        public LayoutPane() : this(new Component[0]) {}
        public LayoutPane(params Component[] children) {
            ComponentAssets = new List<Asset<Component>>();

            foreach(Component child in children)
                Add(child);
        }

        public override void Load(ContentManager content) {
            ComponentAssets.ForEach(asset => {
                asset.Load(content);
                Add(asset.Object);
            });

            base.Load(content);
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