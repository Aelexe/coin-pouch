using CoinPouch.Data.Attributes;
using CoinPouch.Util;

namespace CoinPouch.Data {
    public enum Aetheryte {
        // Capitals
        [Name("Limsa Lominsa"), ID(8), TerritoryMap(129, 12)]
        LimsaLominsa,
        [Name("Gridania"), ID(2), TerritoryMap(132, 2)]
        Gridania,
        [Name("Ul'dah"), ID(9), TerritoryMap(130, 13)]
        Uldah,
        [Name("Mor Dhona"), ID(24), TerritoryMap(156, 25)]
        MorDhona,
        [Name("Ishgard"), ID(70), TerritoryMap(418, 218)]
        Ishgard,
        [Name("Kugane"), ID(111), TerritoryMap(628, 370)]
        Kugane,
        [Name("Rhalgr's Reach"), ID(104), TerritoryMap(635, 366)]
        RhalgrsReach,
        [Name("Crystarium"), ID(133), TerritoryMap(819, 497) ]
        Crystarium,
        [Name("Eulmore"), ID(134), TerritoryMap(820, 498)]
        Eulmore,
        [Name("Old Sharlayan"), ID(182), TerritoryMap(962, 693)]
        OldSharlayan,
        [Name("Radz-at-Han"), ID(183), TerritoryMap(963, 694)]
        RazAtHan,

        // PVP Hub
        [Name("Wolves' Den Pier"), ID(55), TerritoryMap(250, 51)]
        WolvesDenPier,

        // Shadowbringers Zones
        [Name("Lakeland"), ID(132), TerritoryMap(813, 491)]
        Lakeland,
        [Name("Kholusia"), ID(139), TerritoryMap(814, 492)]
        Kholusia,
        [Name("Ahm Araeng"), ID(141), TerritoryMap(815, 493)]
        AhmAraeng,
        [Name("Il Mheg"), ID(144), TerritoryMap(816, 494)]
        IlMheg,
        [Name("The Rak'tika Greatwood"), ID(143), TerritoryMap(817, 495)]
        TheRaktikaGreatwood,
        [Name("The Tempest"), ID(147), TerritoryMap(818, 496)]
        TheTempest,

        // Endwalker Zones
        [Name("Labyrinthos"), ID(166), TerritoryMap(956, 695)]
        Labyrinthos,
        [Name("Thavnair"), ID(169), TerritoryMap(957, 696)]
        Thavnair,
        [Name("Garlemald"), ID(172), TerritoryMap(958, 697)]
        Garlemald,
        [Name("Mare Lamentorum"), ID(175), TerritoryMap(959, 698)]
        MareLamentorum,
        [Name("Ultima Thule"), ID(181), TerritoryMap(960, 699)]
        UltimaThule,
        [Name("Elpis"), ID(176), TerritoryMap(961, 700)]
        Elpis,
    }

    public static class AetheryteUtil {
        public static int GetId(Aetheryte aetheryte) {
            return EnumUtil.GetAttribute<IDAttribute>(aetheryte).Value;
        }

        public static string GetName(Aetheryte aetheryte) {
            return EnumUtil.GetAttribute<NameAttribute>(aetheryte).Value;
        }

        public static TerritoryMap? GetTerritoryMap(Aetheryte aetheryte) {
            TerritoryMapAttribute territoryMap = EnumUtil.GetAttribute<TerritoryMapAttribute>(aetheryte);

            if(territoryMap == null) {
                return null;
            }

            return new(territoryMap.territory, territoryMap.map);
        }
    }
}
