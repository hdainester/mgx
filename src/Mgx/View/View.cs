using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

using Chaotx.Mgx.Layout;

namespace Chaotx.Mgx.Views {
    public class InputArgs {
        public HashSet<Keys> Keys {get;}
        public HashSet<Buttons> Buttons {get;}
        public HashSet<MouseState> MouseStates {get;}
        public HashSet<TouchLocation> TouchLocations {get;}
        public HashSet<GestureSample> GestureSamples {get;}

        public InputArgs() : this(
            Enumerable.Empty<Keys>(),
            Enumerable.Empty<Buttons>(),
            Enumerable.Empty<MouseState>(),
            Enumerable.Empty<TouchLocation>(),
            Enumerable.Empty<GestureSample>()) {}

        public InputArgs(
            IEnumerable<Keys> keys,
            IEnumerable<Buttons> buttons,
            IEnumerable<MouseState> mouseStates,
            IEnumerable<TouchLocation> touchLocations,
            IEnumerable<GestureSample> gestureSamples)
        {
            Keys = new HashSet<Keys>(keys);
            Buttons = new HashSet<Buttons>(buttons);
            MouseStates = new HashSet<MouseState>(mouseStates);
            TouchLocations = new HashSet<TouchLocation>(touchLocations);
            GestureSamples = new HashSet<GestureSample>(gestureSamples);
        }
    }

    public abstract class View {
        [ContentSerializer(Optional = true)]
        public bool InputDisabled {get; set;}

        [ContentSerializer(FlattenContent = true)]
        public ViewContainer MainContainer {
            get => mainContainer;
            protected set {
                mainContainer = value;
                mainContainer.ParentView = this;
            }
        }

        [ContentSerializerIgnore]
        public ViewManager Manager {
            get => manager;
            internal set {
                manager = value;
                AlignMainContainer();
            }
        }

        [ContentSerializerIgnore]
        public InputArgs InputArgs {get; protected set;}

        [ContentSerializerIgnore]
        public ContentManager Content {get; protected set;}

        [ContentSerializerIgnore]
        public GraphicsDevice Graphics => Manager.Graphics.GraphicsDevice; // temp fix

        [ContentSerializerIgnore]
        public ViewState State {get; protected set;}

        private HashSet<Keys> pressedKeys;
        private HashSet<Buttons> pressedButtons;
        private MouseState prevMouseState;
        private ViewContainer mainContainer;
        private ViewManager manager;

        public View() {
            State = ViewState.Closed;
            pressedKeys = new HashSet<Keys>();
            pressedButtons = new HashSet<Buttons>();
            InputArgs = new InputArgs();
            MainContainer = new ViewContainer();
        }

        // TODO make these virtual (like Suspend())
        public abstract void Show();
        public abstract void Hide();
        public abstract void Close();
        public virtual void Suspend() {State = ViewState.Suspended;}
        public virtual void Resume() {State = ViewState.Open;}

        public virtual void Update(GameTime gameTime) {
            if(!InputDisabled && State == ViewState.Open)
                HandleInput();
                
            MainContainer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            MainContainer.Draw(spriteBatch);
        }

        protected virtual void HandleInput() {
            HashSet<Keys> keys = new HashSet<Keys>();
            HashSet<Buttons> buttons = new HashSet<Buttons>();
            HashSet<MouseState> mouseStates = new HashSet<MouseState>();
            HashSet<TouchLocation> touchLocations = new HashSet<TouchLocation>();
            HashSet<GestureSample> gestureSamples = new HashSet<GestureSample>();

            List<Keys> pressed = new List<Keys>(Keyboard.GetState().GetPressedKeys());
            pressedKeys.Where(key => !pressed.Contains(key)).ToList().ForEach(key => keys.Add(key));
            pressedKeys.RemoveWhere(key => !pressed.Contains(key));
            foreach(var key in pressed) {
                if(!pressedKeys.Contains(key)) {
                    pressedKeys.Add(key);
                    keys.Add(key);
                }
            }

            GamePadState pad = GamePad.GetState(0);
            foreach(var button in (Buttons[])Enum.GetValues(typeof(Buttons))) {
                if(pad.IsButtonDown(button)) {
                    if(!pressedButtons.Contains(button)) {
                        pressedButtons.Add(button);
                        buttons.Add(button);
                    }
                } else if(pressedButtons.Remove(button))
                    buttons.Add(button);
            }

            MouseState mouseState = Mouse.GetState();
            if(mouseState != null && !mouseState.Equals(prevMouseState)) {
                prevMouseState = mouseState;
                mouseStates.Add(mouseState);
            }

            foreach(var touch in TouchPanel.GetState())
                touchLocations.Add(touch);

            while(TouchPanel.IsGestureAvailable)
                gestureSamples.Add(TouchPanel.ReadGesture());

            InputArgs = new InputArgs(
                keys, buttons, mouseStates,
                touchLocations, gestureSamples);
        }

        // TODO (needs to be called after viewport changes)
        protected void AlignMainContainer() {
            MainContainer.HGrow = MainContainer.VGrow = 1;
            MainContainer.SetPosition(new Vector2(0, 0));
            MainContainer.SetSize(new Vector2(
                Graphics.Viewport.Width,
                Graphics.Viewport.Height));
        }
    }
}