using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using System;

namespace Chaotx.Mgx.Layout {
    public enum TravelMode {
        Linear, Logarithmic, Exponential
    }

    public enum GenericPosition {
        None, Left, TopLeft, Top, TopRight,
        Right, BottomRight, Bottom, BottomLeft
    }

    public enum SlidingPaneState {
        SlidingIn, SlidedIn, SlidingOut, SlidedOut
    }

    public class SlidingPane : StackPane {
        [Ordered, ContentSerializer(Optional=true)]
        internal SlidingPaneState InitialState {
            get => initialState;
            set => initialState = nextState = value;
        }

        [Ordered, ContentSerializer(Optional=true)]
        public GenericPosition GenericStart {
            get => genericStart;
            set {
                if(!value.Equals(genericStart)) {
                    startRelative = false;
                    SetProperty(ref genericStart, value);
                }
            }
        }

        [Ordered, ContentSerializer(Optional=true)]
        public GenericPosition GenericTarget {
            get => genericTarget;
            set {
                if(!value.Equals(genericTarget)) {
                    targetRelative = false;
                    SetProperty(ref genericTarget, value);
                }
            }
        }

        [Ordered, ContentSerializer(Optional=true)]
        public Vector2 RelativeStart {
            get => relativeStart;
            set {
                if(!value.Equals(relativeStart)) {
                    startRelative = true;
                    SetProperty(ref relativeStart, value);
                }
            }
        }

        [Ordered, ContentSerializer(Optional=true)]
        public Vector2 RelativeTarget {
            get => relativeTarget;
            set {
                if(!value.Equals(relativeTarget)) {
                    targetRelative = true;
                    SetProperty(ref relativeTarget, value);
                }
            }
        }

        [Ordered, ContentSerializer(Optional=true)]
        public bool SuppressUpdates {get; set;}

        [Ordered, ContentSerializer(Optional=true)]
        public int TravelTime {get; internal set;}

        [Ordered, ContentSerializer(Optional=true)]
        public TravelMode TravelMode {get; set;}

        [Ordered, ContentSerializer(Optional=true)]
        public float TravelBase {get; set;} = 5f;

        [Ordered, ContentSerializer(Optional=true)]
        public float TravelExponent {get; set;} = 5f;

        [Ordered, ContentSerializerIgnore]
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

        [ContentSerializerIgnore]
        internal override bool InitialAligned
            => Parent.InitialAligned && positionValidated;

        public event EventHandler SlidedIn;
        public event EventHandler SlidedOut;

        private int timeTraveled;
        private bool positionValidated;
        private bool slidin, locked, newState;
        private bool startRelative, targetRelative;
        private SlidingPaneState nextState;
        private SlidingPaneState initialState;
        private GenericPosition genericStart;
        private GenericPosition genericTarget;
        private Vector2 relativeStart;
        private Vector2 relativeTarget;

        public SlidingPane() : this(null, 2000, 0) {}
        public SlidingPane(LayoutPane origin = null, int time = 2000,
        Vector2? start = null, Vector2? target = null,
        SlidingPaneState initialState = SlidingPaneState.SlidingIn)
        : this(origin, time, 0, 0, initialState) {
            if(start.HasValue) RelativeStart = start.Value;
            if(target.HasValue) RelativeTarget = target.Value;
        }

        public SlidingPane(LayoutPane origin = null, int time = 2000,
        GenericPosition start = 0, GenericPosition target = 0,
        SlidingPaneState initialState = SlidingPaneState.SlidingIn) {
            locked = initialState != SlidingPaneState.SlidedIn;
            State = SlidingPaneState.SlidedOut;
            InitialState = initialState;
            GenericStart = start;
            GenericTarget = target;
            nextState = State;
            TravelTime = time;
            Origin = origin;
            newState = true;
        }

        public override void Update(GameTime gameTime) {
            if(!SuppressUpdates || State == SlidingPaneState.SlidedIn)
                base.Update(gameTime);

            if(newState) {
                newState = slidin = false;
                State = nextState;
            }

            if(!positionValidated) {
                if(!base.InitialAligned
                || !EvaluatePositions())
                    return;

                SetPosition(
                    State == SlidingPaneState.SlidingIn ||
                    State == SlidingPaneState.SlidedOut
                    ? StartPosition : TargetPosition);
            }

            if(State == SlidingPaneState.SlidingIn
            || State == SlidingPaneState.SlidingOut)
                if(!slidin) slidin = locked = EvaluatePositions();
                else Slide(gameTime);
        }

        public void SlideIn() {SlideIn(0, 0);}
        public void SlideIn(Vector2? start = null, Vector2? target = null) {
            if(start.HasValue)  RelativeStart = start.Value;
            if(target.HasValue) RelativeTarget = target.Value;
            nextState = SlidingPaneState.SlidingIn;
            timeTraveled = 0;
            newState = true;
            slidin = false;
        }

        public void SlideIn(GenericPosition start = 0, GenericPosition target = 0) {
            if(start != 0) GenericStart = start;
            if(target != 0) GenericTarget = target;
            nextState = SlidingPaneState.SlidingIn;
            timeTraveled = 0;
            newState = true;
            slidin = false;
        }

        public void SlideOut() {SlideOut(0, 0);}
        public void SlideOut(Vector2? start = null, Vector2? target = null) {
            if(start.HasValue) RelativeStart = start.Value;
            if(target.HasValue) RelativeTarget = target.Value;
            nextState = SlidingPaneState.SlidingOut;
            timeTraveled = 0;
            newState = true;
            slidin = false;
        }

        public void SlideOut(GenericPosition start = 0, GenericPosition target = 0) {
            if(start != 0) GenericStart = start;
            if(target != 0) GenericTarget = target;
            nextState = SlidingPaneState.SlidingOut;
            timeTraveled = 0;
            newState = true;
            slidin = false;
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
            Vector2 start, target;
            if(State == SlidingPaneState.SlidingIn) {
                start = StartPosition;
                target = TargetPosition;
            } else {
                start = TargetPosition;
                target = StartPosition;
            }

            timeTraveled += gameTime.ElapsedGameTime.Milliseconds;
            var f = Math.Min(1, timeTraveled/(float)TravelTime);
            var a = TravelMode == TravelMode.Logarithmic
                ? (float)Math.Max(0, Math.Log(f*TravelBase, TravelBase))
                : TravelMode == TravelMode.Exponential
                ? (float)Math.Pow(f, TravelExponent) : f;

            SetPosition(start + (target-start)*a);
            if(Position.Equals(target)) Finish();
        }

        private bool EvaluatePositions() {
            if(ParentView == null || !base.InitialAligned
            || ParentView.ViewPane.AlignmentPending)
                return false;

            ForceAlign();
            
            StartPosition =
                startRelative ? Position + RelativeStart
                : GenericToVector2(GenericStart);

            TargetPosition =
                targetRelative ? Position + RelativeTarget
                : GenericToVector2(GenericTarget);

            if(State == SlidingPaneState.SlidingIn
            || State == SlidingPaneState.SlidedOut)
                SetPosition(StartPosition);
            else
            if(State == SlidingPaneState.SlidingOut
            || State == SlidingPaneState.SlidedIn)
                SetPosition(TargetPosition);

            positionValidated = true;
            return true;
        }

        private void SetPosition(Vector2 value) {
            var old = locked;
            locked = false;
            Position = value;
            locked = old;
        }

        private void ForceAlign() {
            var old = locked;
            locked = false;
            Parent.AlignChildren();
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
                locked = false;
                OnSlidedIn();
            } else
            if(State == SlidingPaneState.SlidingOut) {
                State = SlidingPaneState.SlidedOut;
                OnSlidedOut();
            }

            slidin = false;
        }
    }
}