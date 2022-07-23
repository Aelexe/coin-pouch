using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class AetheryteAttribute : Attribute {
        public AetheryteAttribute(Aetheryte v) {
            Value = v;
        }

        public Aetheryte Value { get; }
    }
}