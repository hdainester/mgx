using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Mgx.View {
    public class ViewControl {
        private LinkedList<View> views = new LinkedList<View>();
        public ReadOnlyCollection<View> Views {
            get {return views.ToList().AsReadOnly();}
            set {views = new LinkedList<View>(value);}
        }

        public void Add(View view) {
            views.AddFirst(view);
        }

        public void Remove(View view) {
            views.Remove(view);
        }

        public void Update(GameTime gameTime) {
            LinkedListNode<View> node = views.First;
            LinkedListNode<View> prevNode;

            for(View view; node != null; node = node.Next) {
                view = node.Value;

                if(view.State == ViewState.Closed) {
                    if((prevNode = node.Next) != null
                    && prevNode.Value.State == ViewState.Hidden)
                        prevNode.Value.Show();
                        
                    views.Remove(node);
                } else {
                    view.Update(gameTime);
                    if(view.State == ViewState.Greedy)
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            foreach(View view in views) {
                view.Draw(spriteBatch);
                if(view.State == ViewState.Greedy)
                    break;
            }
            spriteBatch.End();
        }
    }
}