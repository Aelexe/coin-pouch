using CoinPouch.Data.Attributes;
using CoinPouch.Util;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace CoinPouch.Data {
    public enum Currency {
        [Name("Tomestone of Poetics"), ID(28), Maximum(2000), DefaultThreshold(1400), Category("Battle"), Vendors(Vendor.RowenaRepLimsa, Vendor.RowenaRepGridania, Vendor.RowenaRepUldah, Vendor.Auriana, Vendor.RowenaRepIshgard, Vendor.RowenaRepKugane, Vendor.MowensMerchant)]
        TomestoneOfPoetics,
        [Name("Tomestone of Astronomy"), ID(43), Maximum(2000), DefaultThreshold(1700), Category("Battle"), Vendors(Vendor.Cihanti)]
        TomestoneOfAstronomy,
        [Name("Tomestone of Causality"), ID(44), Maximum(2000), DefaultThreshold(1700), Category("Battle"), Vendors(Vendor.Cihanti)]
        TomestoneOfCausality,

        [Name("Storm Seal"), ID(20), Maximum(75000), DefaultThreshold(75000), Category("Common"), Vendors(Vendor.StormQuartermaster)]
        StormSeal,
        [Name("Serpent Seal"), ID(21), Maximum(75000), DefaultThreshold(75000), Category("Common"), Vendors(Vendor.SerpentQuartermaster)]
        SerpentSeal,
        [Name("Flame Seal"), ID(22), Maximum(75000), DefaultThreshold(75000), Category("Common"), Vendors(Vendor.FlameQuartermaster)]
        FlameSeal,

        [Name("Wolf Mark"), ID(25), Maximum(20000), DefaultThreshold(18000), Category("Battle"), Vendors(Vendor.MarkQuartermaster)]
        WolfMark,
        [Name("Trophy Crystal"), ID(36656), Maximum(20000), DefaultThreshold(18000), Category("Battle"), Vendors(Vendor.CrystalQuartermaster)]
        TrophyCrystal,

        [Name("Allied Seal"), ID(27), Maximum(4000), DefaultThreshold(3500), Category("Battle"), Vendors(Vendor.HuntBillmasterLimsaLominsa, Vendor.HuntBillmasterGridania, Vendor.HuntBillmasterUldah)]
        AlliedSeal,
        [Name("Centurio Seal"), ID(10307), Maximum(4000), DefaultThreshold(3500), Category("Battle"), Vendors(Vendor.ArdolaineYolaine, Vendor.LeuekinBillebaut, Vendor.SatsuyaEstrild)]
        CenturioSeal,
        [Name("Sack of Nuts"), ID(26533), Maximum(4000), DefaultThreshold(3500), Category("Battle"), Vendors(Vendor.Xylle)]
        SackOfNut,
        [Name("Bicolor Gemstone"), ID(26807), Maximum(1000), DefaultThreshold(800), Category("Battle"), Vendors(Vendor.Gramsol, Vendor.Pedronille, Vendor.Siulmet, Vendor.Zumutt, Vendor.Halden, Vendor.SulLad, Vendor.Nacille, Vendor.GoushsOoan, Vendor.Gadfrid, Vendor.Sajareen, Vendor.Faezbroes, Vendor.Mahveydah, Vendor.Zawawa, Vendor.Tradingway, Vendor.Aisara, Vendor.N1499)]
        BicolorGemstone,

        [Name("White Crafters' Scrip"), ID(25199), Maximum(4000), DefaultThreshold(3500), Category("Other")]
        WhiteCraftersScrip,
        [Name("Purple Crafters' Scrip"), ID(33913), Maximum(4000), DefaultThreshold(3500), Category("Other")]
        PurpleCraftersScrip,
        [Name("White Gatherers' Scrip"), ID(25200), Maximum(4000), DefaultThreshold(3500), Category("Other")]
        WhiteGatherersScrip,
        [Name("Purple Gatherers' Scrip"), ID(33914), Maximum(4000), DefaultThreshold(3500), Category("Other")]
        PurpleGatherersScrip,
        [Name("Skybuilders' Scrip"), ID(28063), Maximum(10000), DefaultThreshold(7500), Category("Other")]
        SkybuildersScrip
    }

    public static class CurrencyUtil {
        public static Currency? GetById(int id) {
            Currency? matchingCurrency = null;

            EnumUtil.Each<Currency>((currency) => {
                int foundId = EnumUtil.GetAttribute<IDAttribute>(currency).Value;

                if(id == foundId) {
                    matchingCurrency = currency;
                }
            });

            return matchingCurrency;
        }

        public static int GetId(Currency currency) {
            return EnumUtil.GetAttribute<IDAttribute>(currency).Value;
        }

        public static string GetName(Currency currency) {
            return EnumUtil.GetAttribute<NameAttribute>(currency).Value;
        }

        public static int GetQuantity(Currency currency) {
            int quantity = 0;
            unsafe {
                InventoryManager* inventoryManager = InventoryManager.Instance();
                quantity = inventoryManager->GetInventoryItemCount((uint)GetId(currency));
            }

            return quantity;
        }

        public static int GetMaximum(Currency currency) {
            return EnumUtil.GetAttribute<MaximumAttribute>(currency).Value;
        }

        public static Vendor[] GetVendors(Currency currency) {
            VendorsAttribute vendors = EnumUtil.GetAttribute<VendorsAttribute>(currency);

            if(vendors == null) {
                return System.Array.Empty<Vendor>();
            }

            return vendors.Value;
        }
    }
}
