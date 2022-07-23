using CoinPouch.Data;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace CoinPouch.Teleport;
internal class Teleporter {
    private static TeleportUI ui = Plugin.teleportUi;

    public static void RequestTeleport(Currency currency) {
        Vendor[] vendors = CurrencyUtil.GetVendors(currency);

        if(vendors.Length == 0) {
            return;
        }

        if(vendors.Length == 1) {
            Vendor vendor = vendors[0];
            Aetheryte aetheryte = VendorUtil.GetAetheryte(vendor);
            TeleportTo(aetheryte);
            if(Plugin.config.pinVendor) {
                AddMapCoordinate(aetheryte, vendor);
            }
            return;
        }

        Aetheryte[] aetherytes = new Aetheryte[vendors.Length];

        for(int i = 0; i < vendors.Length; i++) {
            aetherytes[i] = VendorUtil.GetAetheryte(vendors[i]);
        }

        ui.Open(vendors);
    }

    public static unsafe void TeleportTo(Aetheryte aetheryte) {
        var tp = Telepo.Instance();

        tp->Teleport((uint)AetheryteUtil.GetId(aetheryte), 0);
    }

    public static void AddMapCoordinate(Aetheryte aetheryte, Vendor vendor) {
        TerritoryMap territoryMap = VendorUtil.GetTerritoryMap(vendor) ?? AetheryteUtil.GetTerritoryMap(aetheryte);
        Coordinate coordinate = VendorUtil.GetCoordinate(vendor);
        Plugin.GameGui.OpenMapWithMapLink(new MapLinkPayload((uint)territoryMap.territory, (uint)territoryMap.map, coordinate.x, coordinate.y));
    }
}
