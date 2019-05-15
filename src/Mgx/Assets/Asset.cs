using Microsoft.Xna.Framework.Content;
using Chaotx.Mgx.Layout;

using System.Collections;
using System.Reflection;
using System.Linq;

namespace Chaotx.Mgx.Assets {
    public interface IAsset {}
    public class Asset<T> : IAsset where T : Component { // TODO ILoadable instead of Component
        [ContentSerializer(Optional = true)]
        public string Template {get; internal set;}

        [ContentSerializer(Optional = true, ElementName = "Properties")]
        public T Object {get => obj; protected set => obj = value;}
        private T obj;

        public virtual void Load(ContentManager content) {
            if(Template != null) {
                T tem = content.Load<T>(Template);
                obj = obj != null ? ApplyTemplate(tem, obj) : tem;
            }
        }

        private static T ApplyTemplate(T template, T obj) {
            obj.GetType().GetTypeInfo().GetRuntimeProperties().ToList().ForEach(property => {
                object val_obj = null;

                if(obj.WasPropertySet(property.Name)
                || (val_obj = property.GetValue(obj)) != null
                && (val_obj is IAsset || val_obj is IList)) {
                    if(val_obj == null) val_obj = property.GetValue(obj);
                    IList list_tem = property.GetValue(template) as IList;

                    if(list_tem != null && !list_tem.IsReadOnly) {
                        IList list_obj = val_obj as IList;

                        foreach(var val in list_obj)
                            list_tem.Add(val);
                    } else if(val_obj != null)
                        property.SetValue(template, val_obj);
                }
            });

            return template;
        }
    }
}