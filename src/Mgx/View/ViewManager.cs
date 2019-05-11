using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Chaotx.Mgx.Views {
    public class ViewManager {
        public ContentManager Content {get;}
        public GraphicsDeviceManager Graphics {get;}

        private LinkedList<View> views = new LinkedList<View>();
        public ReadOnlyCollection<View> Views {
            get {return views.ToList().AsReadOnly();}
            set {views = new LinkedList<View>(value);}
        }

        public ViewManager(ContentManager content, GraphicsDeviceManager graphics) {
            Content = content;
            Graphics = graphics;
        }

        public void Add(View view, bool greedy = true) {
            if(view.Manager != null) 
                view.Manager.Remove(view);

            view.Manager = this;
            views.AddFirst(view);
            view.Show();

            if(greedy && views.Count > 1) {
                View next = views.First.Next.Value;
                if(next.State == ViewState.Opening
                || next.State == ViewState.Open)
                    next.Suspend();
            }
        }

        public void Remove(View view) {
            if(view.Manager == this) {
                views.Remove(view);
                view.Manager = null;
            }
        }

        public void Update(GameTime gameTime) {
            LinkedListNode<View> node = views.First;
            LinkedListNode<View> prevNode;

            for(View view; node != null; node = node.Next) {
                view = node.Value;

                if(view.State == ViewState.Closed) {
                    if(node == views.First
                    && (prevNode = node.Next) != null
                    && prevNode.Value.State == ViewState.Hidden)
                        prevNode.Value.Show();
                        
                    views.Remove(node);
                } else if(view.State != ViewState.Suspended)
                    view.Update(gameTime);
                else if(node == views.First)
                    view.Resume();
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            foreach(View view in views.Reverse())
                view.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}