using Chaotx.Mgx.Layout;
using Microsoft.Xna.Framework.Content;

namespace Chaotx.Mgx.Assets {
    public class ComponentAsset<T> : IAsset where T : Component {
        [Ordered, ContentSerializer(Optional = true, ElementName = "Properties")]
        public virtual T Object {get; protected set;}

        [ContentSerializerIgnore]
        public object RawObject => Object;
    }
}