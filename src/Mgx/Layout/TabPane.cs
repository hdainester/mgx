using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace Chaotx.Mgx.Layout {
    public class TabPane : Container {
        private List<Container> tabs;
        public ReadOnlyCollection<Container> Tabs => tabs.AsReadOnly();
        public int TabIndex {get; protected set;} = -1;

        public TabPane(params Container[] tabs) {
            this.tabs = new List<Container>(tabs);
            NextTab();
        }

        public bool NextTab() {
            int index = Math.Min(Tabs.Count-1, TabIndex+1);

            if(index != TabIndex) {
                if(TabIndex >= 0) HideTab(Tabs[TabIndex]);
                ShowTab(Tabs[TabIndex = index]);
                return true;
            }

            return false;
        }

        public bool PrevTab() {
            if(TabIndex < 0) return false;

            int index = Math.Max(0, TabIndex-1);
            if(index != TabIndex) {
                HideTab(Tabs[TabIndex]);
                ShowTab(Tabs[TabIndex = index]);
                return true;
            }

            return false;
        }

        protected virtual void ShowTab(Container tab) {
            _Add(tab);
        }

        protected virtual void HideTab(Container tab) {
            _Remove(tab);
        }

        protected override void AlignChildren() {
            base.AlignChildren();
            _DefaultAlign();
        }
    }
}