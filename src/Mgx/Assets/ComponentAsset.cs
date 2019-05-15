using Chaotx.Mgx.Layout;
using Microsoft.Xna.Framework.Content;

namespace Chaotx.Mgx.Assets {
    public class ComponentAsset<T> : Asset where T : Component {
        [ContentSerializer(Optional = true, ElementName = "Properties")]
        public T Object {get => obj; protected set => obj = value;}
        private T obj;

        [ContentSerializerIgnore]
        public override object RawObject {get => Object; protected set => Object = value as T;}

        public override void Load(ContentManager content) {
            if(Template != null) {
                T tem = content.Load<T>(Template);
                obj = obj != null ? ApplyTemplate(tem, obj) : tem;
            }
        }
    }
}