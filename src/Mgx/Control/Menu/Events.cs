using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace Chaotx.Mgx.Control.Menu {
    public class KeyEventArgs : EventArgs {
        public Keys Key {get; protected set;}

        public KeyEventArgs(Keys key) {
            Key = key;
        }
    }

    public delegate void KeyEventHandler(object sender, KeyEventArgs args);
    // public delegate void ButtonEventHandler(object sender, ButtonEventArgs args);
    // public delegate void MouseEventHandler(object sender, MouseEventArgs args);
    // public delegate void TouchEventHandler(object sender, TouchEventArgs args);
}