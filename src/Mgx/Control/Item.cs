using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Controls {
    public abstract class Item : Component {
        [ContentSerializer(Optional = true, ElementName = "Size")]
        private Vector2 _Size {get => Size; set => Size = value;}

        [ContentSerializer(Optional = true)]
        public float Scale {
            get {return scale;}
            set {
                SetProperty(ref scale, value);
                scaledSize = base.Size*Scale;
            }
        }

        [ContentSerializer(Optional = true, ElementName = "Color")]
        private Vector3 _Color {
            get => _color;
            set => Color = Color.FromNonPremultiplied(
                (int)value.X, (int)value.Y, (int)(_color = value).Z, 255);
        }

        [ContentSerializer(Optional = true)]
        public bool IsSizeScaled {get; set;}

        [ContentSerializerIgnore]
        public override Vector2 Size {
            get => IsSizeScaled ? scaledSize : base.Size;
            protected set {
                base.Size = value;
                scaledSize = value*Scale;
            }
        }

        [ContentSerializerIgnore]
        public Color Color {
            get {return color;}
            set {SetProperty(ref color, value);}
        }

        [ContentSerializerIgnore]
        public Vector2 ScaledSize => scaledSize;

        private Vector2 scaledSize;
        private float scale = 1f;
        private Color color = Color.White;
        private Vector3 _color = new Vector3(255, 255, 255);
    }
}