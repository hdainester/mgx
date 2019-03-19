using System.Collections.Generic;
using System.Linq;

namespace Mgx.Layout {
    public class VPane : Container {
        public VPane(params Component[] children) : base(children) {}

        protected override void AlignChildren() {
            List<Component> top = Children.Where(c => c.VAlign == VAlignment.Top).ToList();
            List<Component> center = Children.Where(c => c.VAlign == VAlignment.Center).ToList();
            List<Component> bottom = Children.Where(c => c.VAlign == VAlignment.Bottom).ToList();

            float m = Children.Sum(c => c.VGrow);
            float ch = center.Sum(c => c.Height);
            float bh = bottom.Sum(c => c.Height);
            float w = 0, h = 0;

            Children.ToList().ForEach(child => {
                if(HGrow == 0 && child.Width > w) w = child.Width;
                if(VGrow == 0) h += child.Height;
                if(child.HGrow > 0) _SetWidth(child, Width);
                if(child.VGrow > 0) _SetHeight(child, Height*child.VGrow/m);
            });
            
            if(HGrow == 0) Width = w;
            if(VGrow == 0) Height = h;

            _AlignGroup(0, top);
            _AlignGroup(Height/2 - ch/2, center);
            _AlignGroup(Height - bh/2, bottom);
        }

        private void _AlignGroup(float y, List<Component> group) {
            group.ForEach(child => {
                _SetY(child, Y + y);
                _SetX(child, X + (child.HAlign == HAlignment.Left ? 0
                    : child.HAlign == HAlignment.Center
                    ? (Width/2 - child.Width/2)
                    : (Width - child.Width)));
                y += child.Height;
            });
        }
    }
}