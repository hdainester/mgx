using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Controls {
    public abstract class Item : Component {
        [ContentSerializer(Optional = true, ElementName = "Size")]
        private Vector2 _Size {get => Size; set => Size = value;}

        [ContentSerializerIgnore]
        public override Vector2 Size {
            get => IsSizeScaled ? scaledSize : base.Size;
            protected set {
                base.Size = value;
                scaledSize = value*Scale;
            }
        }

        [ContentSerializer(Optional = true)]
        public float Scale {
            get {return scale;}
            set {
                SetProperty(ref scale, value);
                scaledSize = base.Size*Scale;
            }
        }

        [ContentSerializer(Optional=true)]
        public Color Color {
            get {return color;}
            set {SetProperty(ref color, value);}
        }

        [ContentSerializer(Optional = true)]
        public bool IsSizeScaled {get; set;}

        [ContentSerializerIgnore]
        public Vector2 ScaledSize => scaledSize;

        private Vector2 scaledSize;
        private float scale = 1f;
        private Color color = Color.White;
    }
}