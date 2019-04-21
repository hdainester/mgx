using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Chaotx.Mgx.View {
    using Layout;

    public class FadingView : View {
        public int FadeInTime {get; set;} = 1000;
        public int FadeOutTime {get; set;} = 1000;

        public FadingView(ContentManager content, GraphicsDevice graphics)
        : base(content, graphics) {}

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
                        MainContainer.Alpha = fadeTime/(float)FadeInTime;
                    else {
                        MainContainer.Alpha = 1f;
                        State = ViewState.Open;
                    }
                } else {
                    if(fadeTime < FadeOutTime)
                        MainContainer.Alpha = 1f - fadeTime/(float)FadeInTime;
                    else {
                        MainContainer.Alpha = 0f;
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