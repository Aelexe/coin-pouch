using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class CoordinateAttribute : Attribute {
        public CoordinateAttribute(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public float x { get; }
        public float y { get; }
    }
}