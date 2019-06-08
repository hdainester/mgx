using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Chaotx.Mgx.Layout;

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
                } else if(view.State != ViewState.Suspended) {
                    if(view.ViewPane.AlignmentPending)
                        UpdateLayers(); // TODO test reliability and performance

                    view.Update(gameTime);
                } else if(node == views.First)
                    view.Resume();
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp);
            foreach(View view in views.Reverse())
                view.Draw(spriteBatch);
            spriteBatch.End();
        }

        // TODO this is not a very performant
        // solution but it does the job fo now
        protected void UpdateLayers() {
            int i = 0;

            Views.Reverse().ToList().ForEach(v =>
                i = UpdateLayers(v, i+1));
        }

        private int UpdateLayers(View view, int i = 0,
            Component comp = null, int count = 0)
        {
            if(comp == null) {
                comp = view.ViewPane;
                count = Views.Count;
                Views.ToList().ForEach(v => count += CountItems(v));
                if(count == 0) return 0;
            }
            
            comp.Layer = i/(float)count;
            var cont = comp as Container;
            
            if(cont != null) cont
                .Children.ToList()
                .ForEach(c => i = UpdateLayers(view, i+1, c, count));

            return i;
        }

        private int CountItems(View view, Component comp = null) {
            if(comp == null) comp = view.ViewPane;
            if(comp == null) return 0;

            var cont = comp as Container;
            int count = 1;

            if(cont != null) cont
                .Children.ToList()
                .ForEach(c => count += CountItems(view, c));

            return count;
        }
    }
} 