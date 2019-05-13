using Chaotx.Mgx.Layout;
using Microsoft.Xna.Framework;

namespace Chaotx.Mgx.Views {
    public class ViewPane : StackPane {
        internal ViewPane() {}

        public void SetPosition(Vector2 position) {
            Position = position;
        }

        public void SetSize(Vector2 size) {
            Size = size;
        }
    }
}