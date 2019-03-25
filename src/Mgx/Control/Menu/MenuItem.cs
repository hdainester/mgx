using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System;

namespace Mgx.Control.Menu {
    using Layout;

    public class MenuItem : StackPane, IControlable {
        private Orientation orientation;
        public Orientation Orientation {
            get {return orientation;}
            set {SetProperty(ref orientation, value);}
        }

        public TextItem Text {get; protected set;}
        public ImageItem Image {get; protected set;}

        public bool IsFocused {get; protected set;}

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

        private HPane hPane;
        private VPane vPane;

        public MenuItem(Texture2D image) : this(null, null, image) {}
        public MenuItem(Texture2D image, int imageWidth, int imageHeight) : this(null, null, image, imageWidth, imageHeight) {}
        public MenuItem(string text, SpriteFont font) : this(text, font, null, 0, 0) {}
        public MenuItem(string text, SpriteFont font, Texture2D image) : this (text, font, image, image.Width, image.Height) {}
        public MenuItem(string text, SpriteFont font, Texture2D image, int imageWidth, int imageHeight) {
            vPane = new VPane();
            hPane = new HPane();
            Add(hPane);
            Add(vPane);

            if(image != null) {
                Image = new ImageItem(image, imageWidth, imageHeight);
                Image.HAlign = HAlignment.Center;
                Image.VAlign = VAlignment.Center;
                hPane.Add(Image);
            }

            if(text != null) {
                Text = new TextItem(font, text);
                Text.HAlign = HAlignment.Center;
                Text.VAlign = VAlignment.Center;
                hPane.Add(Text);
            }

            Orientation = Orientation.Vertical;
            HAlign = HAlignment.Center;
            VAlign = VAlignment.Center;
            hPane.HAlign = vPane.HAlign = HAlignment.Center;
            hPane.VAlign = vPane.VAlign = VAlignment.Center;
        }

        protected override void OnPropertyChanged(string propertyName) {
            base.OnPropertyChanged(propertyName);

            if(propertyName.Equals("Orientation") && Text != null && Image != null) {
                if(Orientation == Orientation.Horizontal) {
                    hPane.Add(Image);
                    hPane.Add(Text);
                }

                if(Orientation == Orientation.RHorizontal) {
                    hPane.Add(Text);
                    hPane.Add(Image);
                }

                if(Orientation == Orientation.Vertical) {
                    vPane.Add(Image);
                    vPane.Add(Text);
                }

                if(Orientation == Orientation.RVertical) {
                    vPane.Add(Text);
                    vPane.Add(Image);
                }
            }
        }

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

            if(Text != null) {
                Text.Color = Color.White;
                Text.Alpha = 1;
            }

            if(Image != null) {
                Image.Color = Color.White;
                Image.Alpha = 1;
            }
        }

        protected virtual void OnDisabled() {
            EventHandler handler = Disabled;
            if(handler != null) handler(this, null);

            if(Text != null) {
                Text.Color = Color.Gray;
                Text.Alpha = 0.75f;
            }

            if(Image != null) {
                Image.Color = Color.Gray;
                Image.Alpha = 0.75f;
            }
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
    }
}