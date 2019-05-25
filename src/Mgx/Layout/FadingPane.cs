using System;
using Microsoft.Xna.Framework;

namespace Chaotx.Mgx.Layout {
    public class FadingPane : StackPane {
        public int FadeInTime {get; set;} = 1000;
        public int FadeOutTime {get; set;} = 1000;
        public int LifeSpan {get; set;}
        public event EventHandler Death;

        private int fadeIn, fadeOut;
        private bool isDead;

        public FadingPane(int lifeSpan = -1) {
            LifeSpan = lifeSpan;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            int et = gameTime.ElapsedGameTime.Milliseconds;

            if(fadeIn < FadeInTime) {
                fadeIn += et;
                Alpha = Math.Min(1, fadeIn/(float)FadeInTime);
            }

            if(isDead) {
                if(fadeOut < FadeOutTime) {
                    fadeOut += et;
                    Alpha = Math.Max(0, 1 - fadeOut/(float)FadeOutTime);
                }

                if(Alpha == 0) {
                    Parent._Remove(this);
                    OnDeath(null);
                }
            }

            if(fadeIn >= FadeInTime && LifeSpan >= 0) {
                LifeSpan -= et;
                isDead = LifeSpan <= 0;
            }
        }

        protected virtual void OnDeath(EventArgs args) {
            var handler = Death;
            if(handler != null) handler(this, args);
        }
    }
}