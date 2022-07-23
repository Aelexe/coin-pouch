using CoinPouch.Data.Attributes;
using CoinPouch.Util;

namespace CoinPouch.Data {
    public enum Sound : byte {
        [Name("None")]
        None = 0x00,

        [Name("Unknown")]
        Unknown = 0x01,

        [Name("SFX 1")]
        Sound01 = 0x25,

        [Name("SFX 2")]
        Sound02 = 0x26,

        [Name("SFX 3")]
        Sound03 = 0x27,

        [Name("SFX 4")]
        Sound04 = 0x28,

        [Name("SFX 5")]
        Sound05 = 0x29,

        [Name("SFX 6")]
        Sound06 = 0x2A,

        [Name("SFX 7")]
        Sound07 = 0x2B,

        [Name("SFX 8")]
        Sound08 = 0x2C,

        [Name("SFX 9")]
        Sound09 = 0x2D,

        [Name("SFX 10")]
        Sound10 = 0x2E,

        [Name("SFX 11")]
        Sound11 = 0x2F,

        [Name("SFX 12")]
        Sound12 = 0x30,

        [Name("SFX 13")]
        Sound13 = 0x31,

        [Name("SFX 14")]
        Sound14 = 0x32,

        [Name("SFX 15")]
        Sound15 = 0x33,

        [Name("SFX 16")]
        Sound16 = 0x34,
    }

    public static class SoundUtil {
        public static string GetName(Sound sound) {
            return EnumUtil.GetAttribute<NameAttribute>(sound).Value;
        }
    }
}
