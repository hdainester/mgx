using Microsoft.Xna.Framework.Content;
using Chaotx.Mgx.Layout;

using System.Reflection;
using System.Linq;
using System.Collections;

namespace Chaotx.Mgx.Assets {
    public class Asset<T> where T : Component, new() { // TODO ILoadable instead of Component
        [ContentSerializer(Optional = true)]
        public string Template {get; internal set;}

        [ContentSerializer(Optional = true, ElementName = "Properties")]
        public T Object {get => obj; protected set => obj = value;}
        private T obj;

        public virtual void Load(ContentManager content) {
            if(obj != null)
                obj.Load(content);

            if(Template != null) {
                T tem = content.Load<T>(Template);
                tem.Load(content);

                if(obj != null)
                    obj = ApplyTemplate(tem, obj);
                else obj = tem;
            }
        }

        private static T ApplyTemplate(T template, T obj) {
            T def = new T();

            obj.GetType().GetTypeInfo().GetProperties()
                .Where(property => property.CanRead && property.CanWrite)
                .ToList().ForEach(property => {
                    object val_obj = property.GetValue(obj);
                    object val_def = property.GetValue(def);

                    IList list_tem = property.GetValue(template) as IList;
                    IList list_obj = null;

                    if(list_tem != null) {
                        list_obj = val_obj as IList;

                        foreach(var val in list_obj)
                            list_tem.Add(val);
                    } else if(val_obj != null && !val_obj.Equals(val_def))
                        property.SetValue(template, property.GetValue(obj));

                    // Alternative: Override Collections
                    // if(val_obj != null && !val_obj.Equals(val_def))
                    //     property.SetValue(template, property.GetValue(obj));
                });

            return template;
        }
    }
}