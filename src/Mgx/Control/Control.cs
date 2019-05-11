using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

using Chaotx.Mgx.Control.Menu;
using Chaotx.Mgx.Layout;
using Chaotx.Mgx.View;

namespace Chaotx.Mgx.Control {
    public abstract class Control : Container {
        private bool isFocused;
        public bool IsFocused {
            get {return !IsDisabled
                && isFocused
                && Parent != null
                && Parent.ParentView != null
                && Parent.ParentView.State != ViewState.Closed
                && Parent.ParentView.State != ViewState.Hidden;
            }
            protected set {
                if(value != isFocused) {
                    SetProperty(ref isFocused, value);
                    if(isFocused)
                        OnFocusGain();
                    else OnFocusLoss();
                }
            }
        }

        private bool isDisabled;
        public bool IsDisabled {
            get {return isDisabled;}
            set {
                if(value != isDisabled) {
                    SetProperty(ref isDisabled, value);
                    if(isDisabled) OnDisabled();
                    else OnEnabled();
                }
            }
        }

        public event EventHandler Action;
        public event EventHandler Cancel;
        public event EventHandler Enabled;
        public event EventHandler Disabled;
        public event EventHandler FocusGain;
        public event EventHandler FocusLoss;
        public event KeyEventHandler KeyPressed;
        public event KeyEventHandler KeyReleased;
        public event ButtonEventHandler ButtonPressed;
        public event ButtonEventHandler ButtonReleased;
        
        public override void Update(GameTime gameTime) {
            if(!IsDisabled
            && ParentView != null
            && !ParentView.InputDisabled
            && ParentView.State == ViewState.Open)
                HandleInput();

            base.Update(gameTime);
        }

        protected virtual void HandleInput() {
            if(ParentView != null) foreach(var key in ParentView.InputArgs.Keys) HandleKey(key);
            if(ParentView != null) foreach(var button in ParentView.InputArgs.Buttons) HandleButton(button);
            if(ParentView != null) foreach(var mouse in ParentView.InputArgs.MouseStates) HandleMouse(mouse);
            if(ParentView != null) foreach(var touch in ParentView.InputArgs.TouchLocations) HandleTouch(touch);
            if(ParentView != null) foreach(var gesture in ParentView.InputArgs.GestureSamples) HandleGesture(gesture);
        }

        protected virtual void HandleKey(Keys key) {
            if(!IsFocused) return;
            if(Keyboard.GetState().IsKeyDown(key)) {
                if(key == Keys.Enter) OnAction();
                if(key == Keys.Escape) OnCancel();
                OnKeyPressed(key);
            } else OnKeyReleased(key);
        }

        protected virtual void HandleButton(Buttons button) {
            if(!IsFocused) return;
            if(GamePad.GetState(0).IsButtonDown(button)) {
                if(button == Buttons.A) OnAction();
                if(button == Buttons.Back) OnCancel();
                OnButtonPressed(button);
            } else OnButtonReleased(button);
        }

        private bool mouseLeftDown;
        protected virtual void HandleMouse(MouseState mouse) {
            if(ContainsPoint(mouse.Position)) {
                IsFocused = true;

                if(mouse.LeftButton == ButtonState.Pressed) {
                    if(!mouseLeftDown) {
                        mouseLeftDown = true;
                        OnAction();
                    }
                } else mouseLeftDown = false;
            } else IsFocused = false;
        }

        protected virtual void HandleTouch(TouchLocation touch) {
            // TODO test
            if(touch.State == TouchLocationState.Pressed
            && ContainsPoint(touch.Position))
                OnAction();
        }

        protected virtual void HandleGesture(GestureSample gesture) {
            if(gesture.GestureType == GestureType.Tap
            && ContainsPoint(gesture.Position))
                OnAction();
        }

        protected virtual void OnAction() {
            EventHandler handler = Action;
            if(handler != null) handler(this, null);
        }

        protected virtual void OnCancel() {
            EventHandler handler = Cancel;
            if(handler != null) handler(this, null);
        }

        protected virtual void OnEnabled() {
            EventHandler handler = Enabled;
            if(handler != null) handler(this, null);
        }

        protected virtual void OnDisabled() {
            EventHandler handler = Disabled;
            if(handler != null) handler(this, null);
            if(isFocused) OnFocusLoss();
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

        protected virtual void OnButtonPressed(Buttons button) {
            ButtonEventHandler handler = ButtonPressed;
            if(handler != null) handler(this, new ButtonEventArgs(button));
        }

        protected virtual void OnButtonReleased(Buttons button) {
            ButtonEventHandler handler = ButtonReleased;
            if(handler != null) handler(this, new ButtonEventArgs(button));
        }

        protected override void AlignChildren() {
            base.AlignChildren();
            _DefaultAlign();
        }

        protected static void _SetFocus(Control c, bool focus) {
            c.IsFocused = focus;
        }
    }
}