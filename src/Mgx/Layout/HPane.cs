using System.Collections.Generic;
using System.Linq;

namespace Mgx.Layout {
    public class HPane : Container {
        protected override void AlignChildren() {
            List<Component> left = Children.Where(c => c.HAlign == HAlignment.Left).ToList();
            List<Component> center = Children.Where(c => c.HAlign == HAlignment.Center).ToList();
            List<Component> right = Children.Where(c => c.HAlign == HAlignment.Right).ToList();

            float m = Children.Sum(c => c.HGrow);
            float cw = center.Sum(c => c.Width);
            float rw = right.Sum(c => c.Width);
            float w = 0, h = 0;

            Children.ToList().ForEach(child => {
                // TODO items wont shrink back if H/VGrow == 0
                if(child.HGrow > 0 && HGrow > 0) _SetWidth(child, Width*child.HGrow/m);
                else w += child.Width;

                if(child.VGrow > 0 && VGrow > 0) _SetHeight(child, Height);
                else if(child.Height > h) h = child.Height;
            });

            if(HGrow == 0) Width = w;
            if(VGrow == 0) Height = h;

            _AlignGroup(0, left);
            _AlignGroup(Width/2 - cw/2, center);
            _AlignGroup(Width - rw/2, right);
        }

        private void _AlignGroup(float x, List<Component> group) {
            group.ForEach(child => {
                _SetX(child, X + x);
                _SetY(child, Y + (child.VAlign == VAlignment.Top ? 0
                    : child.VAlign == VAlignment.Center
                    ? (Height/2 - child.Height/2)
                    : (Height - child.Height)));
                x += child.Width;
            });
        }
    }
}