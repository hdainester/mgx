using System.Linq;
using System;

namespace Chaotx.Mgx.Layout {
    public class StackPane : LayoutPane {
        public StackPane() : this(new Component[0]) {}
        public StackPane(params Component[] children) : base(children) {}

        internal override void AlignChildren() {
            /// Default alignment strategy of containers is
            /// equal to the alignment strategy of a StackPane
            base.AlignChildren();
            _DefaultAlign();
        }
    }
}