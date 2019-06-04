using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Chaotx.Mgx.Layout {
    public enum FadingPaneState {
        FadingIn, FadedIn, FadingOut, FadedOut
    }

    public class FadingPane : StackPane {
        [Ordered, ContentSerializer(Optional=true)]
        public int FadeInTime {get; set;} = 1000;

        [Ordered, ContentSerializer(Optional=true)]
        public int FadeOutTime {get; set;} = 1000;

        [Ordered, ContentSerializer(Optional=true)]
        public int LifeSpan {get; set;}

        [Ordered, ContentSerializer(Optional=true)]
        public bool SuppressUpdates {get; set;}

        [Ordered, ContentSerializer(Optional=true, ElementName="InitialState")]
        public FadingPaneState State {get; protected set;}

        public event EventHandler FadedIn;
        public event EventHandler FadedOut;

        private int fadeIn, fadeOut, lived;

        public FadingPane() : this(-1) {}
        public FadingPane(int lifeSpan, FadingPaneState initialState = 0) {
            LifeSpan = lifeSpan;
            State = initialState;
        }

        public override void Update(GameTime gameTime) {
            if(State == FadingPaneState.FadedIn
            && SuppressUpdates || !SuppressUpdates)
                base.Update(gameTime);

            int et = gameTime.ElapsedGameTime.Milliseconds;
            if(State == FadingPaneState.FadedIn) {
                if(Alpha != 1) Alpha = 1;
                if(LifeSpan >= 0 && (lived += et) >= LifeSpan)
                    FadeOut();
            }

            if(State == FadingPaneState.FadedOut)
                if(Alpha != 0) Alpha = 0;

            if(State == FadingPaneState.FadingIn) {
                if(fadeIn < FadeInTime) {
                    fadeIn += et;
                    Alpha = Math.Min(1, fadeIn/(float)FadeInTime);

                    if(Alpha == 1) {
                        State = FadingPaneState.FadedIn;
                        lived = 0;
                        OnFadedIn();
                    }
                }
            }

            if(State == FadingPaneState.FadingOut) {
                if(fadeOut < FadeOutTime) {
                    fadeOut += et;
                    Alpha = Math.Max(0, 1 - fadeOut/(float)FadeOutTime);

                    if(Alpha == 0) {
                        State = FadingPaneState.FadedOut;
                        OnFadedOut();
                    }
                }
            }
        }

        public void FadeIn(int time = -1) {
            if(time >= 0) FadeInTime = time;
            State = FadingPaneState.FadingIn;
            fadeIn = 0;
        }

        public void FadeOut(int time = -1) {
            if(time >= 0) FadeOutTime = time;
            State = FadingPaneState.FadingOut;
            fadeOut = 0;
        }

        protected virtual void OnFadedIn(EventArgs args = null) {
            var handler = FadedIn;
            if(handler != null) handler(this, args);
        }

        protected virtual void OnFadedOut(EventArgs args = null) {
            var handler = FadedOut;
            if(handler != null) handler(this, args);
        }
    }
}