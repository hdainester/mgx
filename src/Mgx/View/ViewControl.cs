using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace Mgx.View {
    public class ViewControl {
        private List<View> views = new List<View>();

        public void Add(View view) {
            views.Add(view);
        }

        public void Remove(View view) {
            views.Remove(view);
        }

        public void Update(GameTime gameTime) {
            views.ForEach(view => {
                view.Update(gameTime);
            });
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            views.ForEach(view => view.Draw(spriteBatch));
            spriteBatch.End();
        }
    }
}