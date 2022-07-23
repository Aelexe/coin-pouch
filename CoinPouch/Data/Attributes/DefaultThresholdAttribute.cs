using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class DefaultThresholdAttribute : Attribute {
        public DefaultThresholdAttribute(int v) {
            Value = v;
        }

        public int Value { get; }
    }
}