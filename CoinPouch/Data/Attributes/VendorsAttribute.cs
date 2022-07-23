using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class VendorsAttribute : Attribute {
        public VendorsAttribute(params Vendor[] v) {
            Value = v;
        }

        public Vendor[] Value { get; }
    }
}