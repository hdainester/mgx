using Microsoft.Xna.Framework.Content;
using Chaotx.Mgx.Layout;

using System.Collections;
using System.Reflection;
using System.Linq;

namespace Chaotx.Mgx.Assets {
    public abstract class Asset {
        [Ordered, ContentSerializer(Optional = true)]
        public string Template {get; internal set;}

        [ContentSerializerIgnore]
        public abstract object RawObject {get; protected set;}

        public abstract void Load(ContentManager content);

        internal static T ApplyTemplate<T>(T template, T obj) where T : IReflective {
            obj.GetType().GetTypeInfo().GetRuntimeProperties().ToList().ForEach(property => {
                object val_obj = null;
                Asset ass_obj = null;

                if(obj.IsDeclared(property.Name)
                || (val_obj = property.GetValue(obj)) != null
                && (val_obj is Asset || val_obj is IList)) {
                    if(val_obj == null) val_obj = property.GetValue(obj);
                    IList list_tem = property.GetValue(template) as IList;

                    if(list_tem != null && !list_tem.IsReadOnly) {
                        IList list_obj = val_obj as IList;

                        foreach(var val in list_obj)
                            list_tem.Add(val);
                    } else if((ass_obj = val_obj as Asset) != null) {
                        Asset ass_tem = property.GetValue(template) as Asset;

                        if(ass_tem != null) ApplyTemplate(
                            ass_tem.RawObject as IReflective,
                            ass_obj.RawObject as IReflective);
                        else ass_tem = ass_obj;
                    } else if(val_obj != null)
                        property.SetValue(template, val_obj);
                }
            });

            return template;
        }
    }
}