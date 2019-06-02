using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System;

namespace Chaotx.Mgx.Layout {
    public enum GenericPosition {
        None, Left, TopLeft, Top, TopRight,
        Right, BottomRight, Bottom, BottomLeft
    }

    public enum SlidingPaneState {
        SlidingIn, SlidedIn, SlidingOut, SlidedOut
    }

    public class SlidingPane : StackPane {
        [Ordered, ContentSerializer(Optional=true)]
        public int TravelTime {get; internal set;}

        [Ordered, ContentSerializer(Optional=true)]
        public GenericPosition GenericStart {
            get => genericStart;
            set => genericStart = value;
        }

        [Ordered, ContentSerializer(Optional=true)]
        public Vector2 StartPosition {get; internal set;}

        [Ordered, ContentSerializerIgnore]
        public Vector2 TargetPosition {get; internal set;}

        [ContentSerializerIgnore]
        public SlidingPaneState State {get; protected set;}

        [ContentSerializerIgnore]
        public LayoutPane Origin {get; set;}

        [ContentSerializerIgnore]
        public override Vector2 Position {
            get => base.Position;
            internal set {
                if(!locked)
                    base.Position = value;
            }
        }

        public event EventHandler SlidedIn;
        public event EventHandler SlidedOut;

        private int timeTraveled;
        private bool slidin, locked;
        private SlidingPaneState nextState;
        private GenericPosition genericStart;
        private GenericPosition genericTarget;

        public SlidingPane(int time = 2000, LayoutPane origin = null,
        SlidingPaneState initialState = SlidingPaneState.SlidingIn)
        : this(0, time, origin, initialState) {}

        public SlidingPane(Vector2 start, int time = 2000, LayoutPane origin = null,
        SlidingPaneState initialState = SlidingPaneState.SlidingIn)
        : this(0, time, origin, initialState) {
            StartPosition = start;
        }

        public SlidingPane(GenericPosition start = 0, int time = 2000, LayoutPane origin = null,
        SlidingPaneState initialState = SlidingPaneState.SlidingIn) {
            GenericStart = start;
            State = initialState;
            nextState = State;
            TravelTime = time;
            Origin = origin;
        }

        public override void Update(GameTime gameTime) {
            if(State != SlidingPaneState.SlidedOut)
                base.Update(gameTime);

            if(State == SlidingPaneState.SlidedIn
            || State == SlidingPaneState.SlidedOut)
                State = nextState;

            if(State == SlidingPaneState.SlidingIn
            || State == SlidingPaneState.SlidingOut) {
                if(!slidin)
                    slidin = locked = EvaluatePositions();
                else Slide(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if(State != SlidingPaneState.SlidedOut)
                base.Draw(spriteBatch);
        }

        public void SlideIn(GenericPosition start, LayoutPane origin = null) {
            if(origin != null) Origin = origin;
            nextState = SlidingPaneState.SlidingIn;
            GenericStart = start;
            timeTraveled = 0;
        }

        public void SlideIn(LayoutPane origin = null) {
            SlideIn(GenericStart, origin);
        }

        public void SlideOut(GenericPosition target = 0) {
            nextState = SlidingPaneState.SlidingOut;
            StartPosition = Position;
            timeTraveled = 0;
            genericTarget = target == 0
                ? genericStart : target;
        }

        protected virtual void OnSlidedIn(EventArgs args = null) {
            var handler = SlidedIn;
            if(handler != null) handler(this, args);
        }

        protected virtual void OnSlidedOut(EventArgs args = null) {
            var handler = SlidedOut;
            if(handler != null) handler(this, args);
        }

        private void Slide(GameTime gameTime) {
            timeTraveled += gameTime.ElapsedGameTime.Milliseconds;
            var f = Math.Min(1, timeTraveled/(float)TravelTime);
            var a = (float)Math.Max(0, Math.Log10(f*10f));
            var v = TargetPosition - StartPosition;
            SetPosition(StartPosition + v*a);
            if(Position.Equals(TargetPosition))
                Finish();
        }

        private bool EvaluatePositions() {
            if(ParentView == null || !InitialAligned
            || ParentView.ViewPane.AlignmentPending)
                return false;

            if(State == SlidingPaneState.SlidingIn) {
                StartPosition = GenericToVector2(genericStart);
                var old = locked;
                locked = false;
                Parent.AlignChildren();
                locked = old;
                TargetPosition = Position;
            } else
            if(State == SlidingPaneState.SlidingOut) {
                StartPosition = Position;
                TargetPosition = GenericToVector2(genericTarget);
            }
            
            SetPosition(StartPosition);
            return true;
        }

        private void SetPosition(Vector2 value) {
            var old = locked;
            locked = false;
            Position = value;
            locked = old;
        }

        private Vector2 GenericToVector2(GenericPosition generic) {
            float x = X;
            float y = Y;
            float w = Width;
            float h = Height;

            if(Origin != null) {
                x = Origin.X;
                y = Origin.Y;
                w = Origin.Width;
                h = Origin.Height;
            } else
            if(Parent != null) {
                x = Parent.X;
                y = Parent.Y;
                w = Parent.Width;
                h = Parent.Height;
            } else
            if(ParentView != null) {
                x = ParentView.ViewPane.X;
                y = ParentView.ViewPane.Y;
                w = ParentView.ViewPane.Width;
                h = ParentView.ViewPane.Height;
            }

            return generic == GenericPosition.Left ? new Vector2(x - Width, y + h/2f - Height/2f)
                : generic == GenericPosition.TopLeft ? new Vector2(x - Width, y - Height)
                : generic == GenericPosition.Top ? new Vector2(x + w/2f - Width/2f, y - Height)
                : generic == GenericPosition.TopRight ? new Vector2(x + w, y - Height)
                : generic == GenericPosition.Right ? new Vector2(x + w, y + h/2f - Height/2f)
                : generic == GenericPosition.BottomRight ? new Vector2(x + w, y + h)
                : generic == GenericPosition.Bottom ? new Vector2(x + w/2f - Width/2f, y + h)
                : generic == GenericPosition.BottomLeft ? new Vector2(x - Width, y + h)
                : Position;
        }

        private void Finish() {
            if(State == SlidingPaneState.SlidingIn) {
                State = SlidingPaneState.SlidedIn;
                nextState = State;
                locked = false;
                OnSlidedIn();
            } else
            if(State == SlidingPaneState.SlidingOut) {
                State = SlidingPaneState.SlidedOut;
                nextState = State;
                OnSlidedOut();
            }

            slidin = false;
        }
    }
}