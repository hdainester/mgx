using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

namespace Mgx.Control {
    using Layout;
    using View;
    using Menu;

    public abstract class Control : Container {
        private bool isFocused;
        public bool IsFocused {
            get {return isFocused
                && Parent != null
                && Parent.ParentView != null
                && Parent.ParentView.State == ViewState.Open;
            }
            protected set {isFocused = value;}
        }

        private bool isDisabled;
        public bool IsDisabled {
            get {return isDisabled;}
            set {
                if(value != isDisabled) {
                    isDisabled = value;
                    if(isDisabled) OnDisabled();
                    else OnEnabled();
                }
            }
        }

        public event EventHandler Action;
        public event EventHandler Enabled;
        public event EventHandler Disabled;
        public event EventHandler FocusGain;
        public event EventHandler FocusLoss;
        public event KeyEventHandler KeyPressed;
        public event KeyEventHandler KeyReleased;

        public virtual void HandleInput() {
            if(!IsDisabled) {
                HandleMouse();
                HandleTouch();

                // TODO
                // if(IsSelected) {
                    HandleGamepad();
                    HandleKeyboard();
                // }
            }
        }

        private MouseState prevMouse;
        protected virtual void HandleMouse() {
            MouseState mouse = Mouse.GetState();
            float mx = mouse.Position.X;
            float my = mouse.Position.Y;

            if(mx > X && mx - X < Width && my > Y && my - Y < Height) {
                if(!IsFocused) {
                    IsFocused = true;
                    OnFocusGain();
                }

                if(prevMouse != null) {
                    if(mouse.LeftButton == ButtonState.Released
                    && prevMouse.LeftButton == ButtonState.Pressed) {
                        // OnMousePressed(mouse.LeftButton);
                        OnAction();
                    }
                }
            } else {
                if(IsFocused) {
                    IsFocused = false;
                    OnFocusLoss();
                }
            }

            prevMouse = mouse;
        }

        private Dictionary<Keys, bool> keyMap = new Dictionary<Keys, bool>();
        protected virtual void HandleKeyboard() {
            KeyboardState keyboard = Keyboard.GetState();
            Dictionary<Keys, bool> newKeyMap = new Dictionary<Keys, bool>();

            keyboard.GetPressedKeys().ToList().ForEach(key => {
                newKeyMap.Add(key, true);
                if(!keyMap.ContainsKey(key))
                    OnKeyPressed(key);
            });

            keyMap.Keys.ToList().ForEach(key => {
                if(!newKeyMap.ContainsKey(key))
                    OnKeyReleased(key);
            });

            keyMap = newKeyMap;
        }

        protected virtual void HandleGamepad() {
            // TODO
        }

        protected virtual void HandleTouch() {
            // TODO
        }

        protected virtual void OnAction() {
            EventHandler handler = Action;
            if(handler != null) handler(this, null);
        }

        protected virtual void OnEnabled() {
            EventHandler handler = Enabled;
            if(handler != null) handler(this, null);
        }

        protected virtual void OnDisabled() {
            EventHandler handler = Disabled;
            if(handler != null) handler(this, null);
        }

        protected virtual void OnFocusGain() {
            EventHandler handler = FocusGain;
            if(handler != null) handler(this, null);
        }

        protected virtual void OnFocusLoss() {
            EventHandler handler = FocusLoss;
            if(handler != null) handler(this, null);
        }

        protected virtual void OnKeyPressed(Keys key) {
            KeyEventHandler handler = KeyPressed;
            if(handler != null) handler(this, new KeyEventArgs(key));
        }

        protected virtual void OnKeyReleased(Keys key) {
            KeyEventHandler handler = KeyReleased;
            if(handler != null) handler(this, new KeyEventArgs(key));
        }

        protected override void AlignChildren() {
            base.AlignChildren();
            _DefaultAlign();
        }
    }
}