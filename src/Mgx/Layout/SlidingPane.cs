using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;

namespace Chaotx.Mgx.Layout {
    public enum GenericPosition {
        Center, Left, TopLeft, Top, TopRight,
        Right, BottomRight, Bottom, BottomLeft
    }

    public class SlidingPane : StackPane {
        public Vector2 StartPosition {get; internal set;}
        public Vector2 TargetPosition {get; internal set;}

        public int TravelTime {get; internal set;}
        public float Acceleration {get; internal set;}

        public override Vector2 Position {
            get => base.Position;
            internal set {
                if(!locked)
                    base.Position = value;
            }
        }

        private int timeTraveled;
        private bool active, moving, locked;
        private bool autoStart, autoTarget;
        private Vector2 currentPosition;
        private GenericPosition genericStart;

        public SlidingPane(GenericPosition start, int time = 2000)
        : this(Vector2.Zero, time) {
            autoStart = true;
            genericStart = start;
        }

        public SlidingPane(Vector2 start, int time = 2000)
        : this(start, Vector2.Zero, time) {
            autoTarget = true;
        }

        public SlidingPane(Vector2 start, Vector2 target, int time = 2000) {
            currentPosition = StartPosition = start;
            TargetPosition = target;
            active = locked = true;
            TravelTime = time;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if(active) Move(gameTime);
        }

        protected void Move(GameTime gameTime) {
            if(autoStart) {
                if(!InitialAligned) return;
                autoStart = false;
                StartPosition = currentPosition =
                    genericStart == GenericPosition.Left ? new Vector2(X - Width, Y)
                    : genericStart == GenericPosition.TopLeft ? new Vector2(X - Width, Y - Height)
                    : genericStart == GenericPosition.Top ? new Vector2(X, Y - Height)
                    : genericStart == GenericPosition.TopRight ? new Vector2(X + Width, Y - Height)
                    : genericStart == GenericPosition.Right ? new Vector2(X + Width, Y)
                    : genericStart == GenericPosition.BottomRight ? new Vector2(X + Width, Y + Height)
                    : genericStart == GenericPosition.Bottom ? new Vector2(X, Y + Height)
                    : genericStart == GenericPosition.BottomLeft ? new Vector2(X - Width, Y + Height)
                    : Position;
            }

            if(autoTarget) {
                if(!InitialAligned) return;
                TargetPosition = Position;
                autoTarget = false;
            }

            if(currentPosition.Equals(TargetPosition)) {
                active = moving = locked = false;
                ParentView.ViewPane.AlignChildren();
                return;
            }

            if(!moving) {
                if(!Position.Equals(currentPosition)) {
                    locked = false;
                    Position = currentPosition;
                    locked = true;
                }

                moving = !ParentView.ViewPane.AlignmentPending;
                return;
            }

            // TODO (De-)acceleration
            timeTraveled += gameTime.ElapsedGameTime.Milliseconds;
            var f = Math.Min(1, timeTraveled/(float)TravelTime);
            var v = TargetPosition - StartPosition;
            
            locked = false;
            currentPosition = StartPosition + v*f;
            Position = currentPosition;//(currentPosition += v*f);
            locked = true;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if(moving || !active)
                base.Draw(spriteBatch);
        }
    }
}