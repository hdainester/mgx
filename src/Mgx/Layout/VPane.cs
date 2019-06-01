using System.Collections.Generic;
using System.Linq;
using System;

namespace Chaotx.Mgx.Layout {
    public class VPane : LayoutPane {
        public VPane() : this(new Component[0]) {}
        public VPane(params Component[] children) : base(children) {}

        internal override void AlignChildren() {
            base.AlignChildren();
            float w = 0, h = 0;
            float m = Children.Sum(c => c.VGrow);

            Children.ToList().ForEach(child => {
                if(child.VGrow == 0) h += child.Height;
                if(HGrow == 0 && child.HGrow == 0
                && child.Width > w) w = child.Width;
            });

            if(HGrow == 0) Width = w;
            if(VGrow == 0) Height = h;

            Children.ToList().ForEach(child => {
                if(VGrow == 0 && child.VGrow >= 1) child.Height = Height*child.VGrow/m;
                else if(VGrow > 0 && child.VGrow >= 1) child.Height = (Height-h)*child.VGrow/m;
                else if(child.VGrow > 0) child.Height = child.VGrow*(Height-h);
                if(child.HGrow > 0) child.Width = Math.Min(1, child.HGrow)*Width;
            });

            List<Component> top = Children.Where(c => c.VAlign == VAlignment.Top).ToList();
            List<Component> center = Children.Where(c => c.VAlign == VAlignment.Center).ToList();
            List<Component> bot = Children.Where(c => c.VAlign == VAlignment.Bottom).ToList();
            float ch = center.Sum(c => c.Height);
            float bh = bot.Sum(c => c.Height);
            _AlignGroup(0, top);
            _AlignGroup(Height/2 - ch/2, center);
            _AlignGroup(Height - bh, bot);
        }
        
        private void _AlignGroup(float y, List<Component> group) {
            group.ForEach(child => {
                child.Y = Y + y;
                child.X = X + (child.HAlign == HAlignment.Left ? 0
                    : child.HAlign == HAlignment.Center
                    ? (Width/2 - child.Width/2)
                    : (Width - child.Width));
                y += child.Height;
            });
        }
    }
}