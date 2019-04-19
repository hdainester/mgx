using Microsoft.Xna.Framework;

namespace Chaotx.Mgx.Control {
    using Layout;

    public abstract class Item : Component {
        private float scale = 1f;
        public float Scale {
            get {return scale;}
            set {
                SetProperty(ref scale, value);
                scaledSize = base.Size*Scale;
            }
        }

        private Vector2 scaledSize;
        public override Vector2 Size {
            get {return scaledSize;}
            protected set {
                base.Size = value;
                scaledSize = value*Scale;
            }
        }
    }
}