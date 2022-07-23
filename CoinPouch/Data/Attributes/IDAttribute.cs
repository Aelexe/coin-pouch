using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class IDAttribute : Attribute {
        public IDAttribute(int v) {
            Value = v;
        }

        public int Value { get; }
    }
}