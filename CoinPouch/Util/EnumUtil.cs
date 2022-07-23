using System;

namespace CoinPouch.Util {
    public static class EnumUtil {
        public static T GetAttribute<T>(this Enum enumVal) where T : Attribute {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }

        public static void Each<T>(Action<T> action) {
            foreach(var item in Enum.GetValues(typeof(T))) {
                action((T)item);
            }
        }
    }
}
