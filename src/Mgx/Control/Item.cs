using Microsoft.Xna.Framework;
using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Controls {
    public abstract class Item : Component {
        public bool LayoutWithTrueSize {get; set;}

        private float scale = 1f;
        public float Scale {
            get {return scale;}
            set {
                SetProperty(ref scale, value);
                scaledSize = base.Size*Scale;
            }
        }

        private Vector2 scaledSize;
        public Vector2 ScaledSize => scaledSize;

        public override Vector2 Size {
            get => LayoutWithTrueSize ? scaledSize : base.Size;
            protected set {
                base.Size = value;
                scaledSize = value*Scale;
            }
        }
    }
}