using System.Linq;
using System;

namespace Mgx.Layout {
    public class StackPane : Container {
        public StackPane(params Component[] children) : base(children) {}

        protected override void AlignChildren() {
            float w = 0, h = 0;

            Children.ToList().ForEach(child => {
                if(HGrow == 0 && child.HGrow == 0 && child.Width > w) w = child.Width;
                if(VGrow == 0 && child.VGrow == 0 && child.Height > h) h = child.Height;
                if(child.HGrow > 0) _SetWidth(child, Math.Min(1, child.HGrow)*Width);
                if(child.VGrow > 0) _SetHeight(child, Math.Min(1, child.VGrow)*Height);
                if(child.HAlign == HAlignment.Left) _SetX(child, X);
                if(child.HAlign == HAlignment.Center) _SetX(child, X + Width/2 - child.Width/2);
                if(child.HAlign == HAlignment.Right) _SetX(child, X + Width - child.Width);
                if(child.VAlign == VAlignment.Top) _SetY(child, Y);
                if(child.VAlign == VAlignment.Center) _SetY(child, Y + Height/2 - child.Height/2);
                if(child.VAlign == VAlignment.Bottom) _SetY(child, Y + Height - child.Height);
            });
            
            if(HGrow == 0) Width = w;
            if(VGrow == 0) Height = h;
        }
    }
}