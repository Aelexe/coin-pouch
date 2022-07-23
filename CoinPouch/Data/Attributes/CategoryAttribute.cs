using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class CategoryAttribute : Attribute {
        public CategoryAttribute(string v) {
            Value = v;
        }

        public string Value { get; }
    }
}
