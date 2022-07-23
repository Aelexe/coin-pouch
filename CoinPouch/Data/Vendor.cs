using CoinPouch.Data.Attributes;
using CoinPouch.Util;

namespace CoinPouch.Data {
    public enum Vendor {
        // Capitals
        [Name("Rowena's Representative"), Aetheryte(Aetheryte.LimsaLominsa), Coordinate(9.0f, 11.1f)]
        RowenaRepLimsa,
        [Name("Storm Quartermaster"), Aetheryte(Aetheryte.LimsaLominsa), TerritoryMap(128, 11), Coordinate(13.1f, 12.7f)]
        StormQuartermaster,
        [Name("Hunt Billmaster"), Aetheryte(Aetheryte.LimsaLominsa), TerritoryMap(128, 11), Coordinate(13.1f, 12.4f)]
        HuntBillmasterLimsaLominsa,
        [Name("Rowena's Representative"), Aetheryte(Aetheryte.Gridania), Coordinate(11.9f, 12.3f)]
        RowenaRepGridania,
        [Name("Serpent Quartermaster"), Aetheryte(Aetheryte.Gridania), Coordinate(9.8f, 11.0f)]
        SerpentQuartermaster,
        [Name("Hunt Billmaster"), Aetheryte(Aetheryte.Gridania), Coordinate(9.7f, 11.2f)]
        HuntBillmasterGridania,
        [Name("Rowena's Representative"), Aetheryte(Aetheryte.Uldah), Coordinate(9.1f, 8.3f)]
        RowenaRepUldah,
        [Name("Flame Quartermaster"), Aetheryte(Aetheryte.Uldah), Coordinate(8.3f, 9.0f)]
        FlameQuartermaster,
        [Name("Hunt Billmaster"), Aetheryte(Aetheryte.Uldah), Coordinate(8.1f, 9.3f)]
        HuntBillmasterUldah,
        [Name("Auriana"), Aetheryte(Aetheryte.MorDhona), Coordinate(22.7f, 6.7f)]
        Auriana,
        [Name("Rowena's Representative"), Aetheryte(Aetheryte.Ishgard), Coordinate(10.5f, 11.7f)]
        RowenaRepIshgard,
        [Name("Ardolain and Yolaine"), Aetheryte(Aetheryte.Ishgard), Coordinate(13.0f, 11.8f)]
        ArdolaineYolaine,
        [Name("Leuekin and Billebaut"), Aetheryte(Aetheryte.RhalgrsReach), Coordinate(13.1f, 11.7f)]
        LeuekinBillebaut,
        [Name("Rowena's Representative"), Aetheryte(Aetheryte.Kugane), Coordinate(12.2f, 10.8f)]
        RowenaRepKugane,
        [Name("Satsuya and Estrild"), Aetheryte(Aetheryte.Kugane), Coordinate(10.4f, 10.2f)]
        SatsuyaEstrild,
        [Name("Gramsol"), Aetheryte(Aetheryte.Crystarium), Coordinate(11.1f, 13.6f)] // Gemstones
        Gramsol,
        [Name("Xylle"), Aetheryte(Aetheryte.Crystarium), Coordinate(9.4f, 9.5f)] // Sack of Nuts
        Xylle,
        [Name("Mowen's Merchant"), Aetheryte(Aetheryte.Crystarium), Coordinate(10.2f, 11.8f)]
        MowensMerchant,
        [Name("Pedronille"), Aetheryte(Aetheryte.Eulmore), Coordinate(10.5f, 12.1f)] // Bicolor Gemstone
        Pedronille,
        [Name("Cihanti"), Aetheryte(Aetheryte.RazAtHan), Coordinate(10.8f, 10.3f)] // Tomestones
        Cihanti,
        [Name("Gadfrid"), Aetheryte(Aetheryte.OldSharlayan), Coordinate(12.7f, 10.4f)] // Bicolor Gemstone
        Gadfrid,
        [Name("Sajareen"), Aetheryte(Aetheryte.RazAtHan), Coordinate(11.1f, 10.2f)] // Bicolor Gemstone
        Sajareen,

        // PVP Hub
        [Name("Mark Quartermaster"), Aetheryte(Aetheryte.WolvesDenPier), Coordinate(6.05f, 5.95f)]
        MarkQuartermaster,
        [Name("Crystal Quartermaster"), Aetheryte(Aetheryte.WolvesDenPier), Coordinate(6.05f, 6.05f)]
        CrystalQuartermaster,

        // Shadowbringers Zones
        [Name("Siulmet"), Aetheryte(Aetheryte.Lakeland), Coordinate(35.5f, 20.7f)]
        Siulmet,
        [Name("Zumutt"), Aetheryte(Aetheryte.Kholusia), Coordinate(11.8f, 8.9f)]
        Zumutt,
        [Name("Halden"), Aetheryte(Aetheryte.AhmAraeng), Coordinate(10.6f, 17.1f)]
        Halden,
        [Name("Sul Lad"), Aetheryte(Aetheryte.IlMheg), Coordinate(16.0f, 31.0f)]
        SulLad,
        [Name("Nacille"), Aetheryte(Aetheryte.TheRaktikaGreatwood), Coordinate(28.0f, 18.0f)]
        Nacille,
        [Name("Goushs Ooan"), Aetheryte(Aetheryte.TheTempest), Coordinate(33.0f, 18.0f)]
        GoushsOoan,

        // Endwalker Zones
        [Name("Faezbroes"), Aetheryte(Aetheryte.Labyrinthos), Coordinate(29.9f, 12.9f)]
        Faezbroes,
        [Name("Mahveydah"), Aetheryte(Aetheryte.Thavnair), Coordinate(25.8f, 34.5f)]
        Mahveydah,
        [Name("Zawawa"), Aetheryte(Aetheryte.Garlemald), Coordinate(12.9f, 30.0f)]
        Zawawa,
        [Name("Tradingway"), Aetheryte(Aetheryte.MareLamentorum), Coordinate(21.8f, 12.2f)]
        Tradingway,
        [Name("Aisara"), Aetheryte(Aetheryte.Elpis), Coordinate(24.4f, 23.4f)]
        Aisara,
        [Name("N-1499"), Aetheryte(Aetheryte.UltimaThule), Coordinate(20.8f, 28.0f)]
        N1499,
    }

    public static class VendorUtil {
        public static Aetheryte GetAetheryte(Vendor vendor) {
            return EnumUtil.GetAttribute<AetheryteAttribute>(vendor).Value;
        }

        public static Coordinate? GetCoordinate(Vendor vendor) {
            CoordinateAttribute coordinate = EnumUtil.GetAttribute<CoordinateAttribute>(vendor);

            if(coordinate == null) {
                return null;
            }

            return new(coordinate.x, coordinate.y);
        }

        public static TerritoryMap? GetTerritoryMap(Vendor vendor) {
            TerritoryMapAttribute territoryMap = EnumUtil.GetAttribute<TerritoryMapAttribute>(vendor);

            if(territoryMap == null) {
                return null;
            }

            return new(territoryMap.territory, territoryMap.map);
        }
    }
}
