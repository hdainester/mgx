using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

using Chaotx.Mgx.Layout;
using System.Text.RegularExpressions;

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
        private static readonly string IdSep = "|";

        public bool InputDisabled {get; set;}
        public ViewPane ViewPane {get; protected set;}
        public LayoutPane RootPane {
            get => rootPane;
            protected set {
                rootPane = value;
                ViewPane.Clear();
   
                if(value != null)
                    ViewPane.Add(rootPane);
            }
        }

        public ViewManager Manager {
            get => manager;
            internal set {
                manager = value;
                ViewPane.Load(Content);
                AlignViewPane();

                if(!rootHistory.Contains(rootPane)) {
                    rootHistory.Add(rootPane);
                    items = new Dictionary<string, object>();
                    ScanForItems(ViewPane, items);
                    Init();
                }
            }
        }

        public InputArgs InputArgs {get; protected set;}
        public ContentManager Content => Manager.Content; // temp fix
        public GraphicsDevice Graphics => Manager.Graphics.GraphicsDevice; // temp fix
        public ViewState State {get; protected set;}

        private Dictionary<string, object> items;
        private HashSet<LayoutPane> rootHistory;

        private HashSet<Keys> pressedKeys;
        private HashSet<Buttons> pressedButtons;
        private MouseState prevMouseState;
        private LayoutPane rootPane;
        private ViewManager manager;

        public View() : this(null) {}
        public View(LayoutPane rootPane) {
            State = ViewState.Closed;
            pressedKeys = new HashSet<Keys>();
            pressedButtons = new HashSet<Buttons>();
            rootHistory = new HashSet<LayoutPane>();
            InputArgs = new InputArgs();
            ViewPane = new ViewPane();
            ViewPane.ParentView = this;
            RootPane = rootPane;
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
                
            ViewPane.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            ViewPane.Draw(spriteBatch);
        }

        private string itemIds = "";
        public T GetItem<T> (string id) where T : Component {
            string regex1 = "\\" + IdSep + @"([\w]+\.)*" + id + @"(\.[\w]+)*" + "\\" + IdSep;
            string regex2 = "\\" + IdSep + @"([\w]+\.)*" + id + "\\" + IdSep;
            string regex3 = "\\" + IdSep + id + @"([\w]+\.)*" + "\\" + IdSep;

            var matches = Regex.Matches(itemIds, regex1);
            if(matches.Count == 1) return items[matches[0].Value.Substring(
                IdSep.Length, matches[0].Value.Length-IdSep.Length-1)] as T;

            matches = Regex.Matches(itemIds, regex2);
            if(matches.Count == 1) return items[matches[0].Value.Substring(
                IdSep.Length, matches[0].Value.Length-IdSep.Length-1)] as T;

            matches = Regex.Matches(itemIds, regex3);
            if(matches.Count == 1) return items[matches[0].Value.Substring(
                IdSep.Length, matches[0].Value.Length-IdSep.Length-1)] as T;

            throw new ArgumentException(
                string.Format("no such item with id \"{0}\"", id));
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
        protected void AlignViewPane() {
            ViewPane.HGrow = ViewPane.VGrow = 1;
            ViewPane.SetPosition(new Vector2(0, 0));
            ViewPane.SetSize(new Vector2(
                Graphics.Viewport.Width,
                Graphics.Viewport.Height));
        }

        private void ScanForItems(Container pane, Dictionary<string, object> buf) {
            pane.Children.ToList().ForEach(child => {
                if(child.Id != null) {
                    if(child.Id.Contains(IdSep)) throw new FormatException(string.Format(
                        "separator symbol \"{0}\" not allowed in Ids at: {1}", IdSep, child.Id));

                    buf.Add(child.Id, child);
                    itemIds += IdSep + child.Id + IdSep;
                }

                Container container = child as Container;
                if(container != null) ScanForItems(container, buf);
            });
        }

        // Is called after the first assignment of a new RootPane
        protected virtual void Init() {}
    }
}