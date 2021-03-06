using System.Collections.Generic;
using System.Linq;
using System;

namespace Chaotx.Mgx.Layout {
    public class HPane : LayoutPane {
        public HPane() : this(new Component[0]) {}
        public HPane(params Component[] children) : base(children) {}

        internal override void AlignChildren() {
            base.AlignChildren();
            float w = 0, h = 0;
            float m = Children.Sum(c => c.HGrow);

            Children.ToList().ForEach(child => {
                if(child.HGrow == 0) w += child.Width;
                if(VGrow == 0 && child.VGrow == 0
                && child.Height > h) h = child.Height;
            });

            if(HGrow == 0) Width = w;
            if(VGrow == 0) Height = h;

            Children.ToList().ForEach(child => {
                if(HGrow == 0 && child.HGrow >= 1) child.Width = Width*child.HGrow/m;
                else if(HGrow > 0 && child.HGrow >= 1) child.Width = (Width-w)*child.HGrow/m;
                else if(child.HGrow > 0) child.Width = child.HGrow*(Width-w);
                if(child.VGrow > 0) child.Height = Math.Min(1, child.VGrow)*Height;
            });

            List<Component> left = Children.Where(c => c.HAlign == HAlignment.Left).ToList();
            List<Component> center = Children.Where(c => c.HAlign == HAlignment.Center).ToList();
            List<Component> right = Children.Where(c => c.HAlign == HAlignment.Right).ToList();
            float cw = center.Sum(c => c.Width);
            float rw = right.Sum(c => c.Width);
            _AlignGroup(0, left);
            _AlignGroup(Width/2 - cw/2, center);
            _AlignGroup(Width - rw, right);
        }
        
        private void _AlignGroup(float x, List<Component> group) {
            group.ForEach(child => {
                child.X = X + x;
                child.Y = Y + (child.VAlign == VAlignment.Top ? 0
                    : child.VAlign == VAlignment.Center
                    ? (Height/2 - child.Height/2)
                    : (Height - child.Height));
                x += child.Width;
            });
        }
    }
}