using System.Runtime.CompilerServices;
using System;

namespace Chaotx.Mgx {
    // https://stackoverflow.com/a/17998371
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class OrderedAttribute : Attribute {
        public float Order => value;
        private readonly int value;

        public OrderedAttribute([CallerLineNumber]int order = 0) {
            value = order;
        }
    }
}