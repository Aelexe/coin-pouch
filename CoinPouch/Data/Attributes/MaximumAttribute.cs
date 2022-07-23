using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class MaximumAttribute : Attribute {
        public MaximumAttribute(int v) {
            Value = v;
        }

        public int Value { get; }
    }
}