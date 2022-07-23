using System;

namespace CoinPouch.Data.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    internal class TerritoryMapAttribute : Attribute {
        public TerritoryMapAttribute(int territory, int map) {
            this.territory = territory;
            this.map = map;
        }

        public int territory { get; }
        public int map { get; }
    }
}