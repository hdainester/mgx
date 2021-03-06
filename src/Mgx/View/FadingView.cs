using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Views {
    public class FadingView : View {
        public int FadeInTime {get; set;} = 1000;
        public int FadeOutTime {get; set;} = 1000;

        public FadingView() : this(null) {}
        public FadingView(LayoutPane rootPane) : base(rootPane) {}

        public override void Show() {
            State = ViewState.Opening;
            fadeTime = 0;
        }

        public override void Hide() {
            State = ViewState.Hiding;
            fadeTime = 0;
        }

        public override void Close() {
            State = ViewState.Closing;
            fadeTime = 0;
        }

        private int fadeTime;
        public override void Update(GameTime gameTime) {
            if(State == ViewState.Opening
            || State == ViewState.Hiding
            || State == ViewState.Closing) {
                fadeTime += gameTime.ElapsedGameTime.Milliseconds;

                if(State == ViewState.Opening) {
                    if(fadeTime < FadeInTime)
                        ViewPane.Alpha = fadeTime/(float)FadeInTime;
                    else {
                        ViewPane.Alpha = 1f;
                        State = ViewState.Open;
                    }
                } else {
                    if(fadeTime < FadeOutTime)
                        ViewPane.Alpha = 1f - fadeTime/(float)FadeInTime;
                    else {
                        ViewPane.Alpha = 0f;
                        State = State == ViewState.Hiding
                            ? ViewState.Hidden
                            : ViewState.Closed;
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}