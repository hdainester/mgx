using Chaotx.Mgx.Layout;
using Microsoft.Xna.Framework;

namespace Chaotx.Mgx.View {
    public class ViewContainer : StackPane {
        internal ViewContainer(View view) {
            ParentView = view;
        }

        public void SetPosition(Vector2 position) {
            Position = position;
        }

        public void SetSize(Vector2 size) {
            Size = size;
        }
    }
}