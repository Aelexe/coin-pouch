using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class NameAttribute : Attribute {
        public NameAttribute(string v) {
            Value = v;
        }

        public string Value { get; }
    }
}
