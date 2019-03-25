using System.Linq;
using System;

namespace Mgx.Layout {
    public class StackPane : LayoutPane {
        public StackPane(params Component[] children) : base(children) {}

        protected override void AlignChildren() {
            /// Default alignment strategy of containers is
            /// equal to the alignment strategy of a StackPane
            base.AlignChildren();
        }
    }
}