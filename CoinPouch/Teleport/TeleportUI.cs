using System;
using System.Numerics;
using CoinPouch.Data;
using Dalamud.Interface;
using ImGuiNET;

namespace CoinPouch.Teleport {
    public class TeleportUI : IDisposable {

        public Vendor[] vendors;

        public bool isVisible = false;

        private Vector2 position;
        private Vector2 windowSize = new(210, 160);

        public void Open(Vendor[] vendors) {
            isVisible = true;
            position = ImGui.GetMousePos();
            this.vendors = vendors;

            if(position.Y + windowSize.Y > ImGuiHelpers.MainViewport.Size.Y - 100) {
                position.Y = ImGuiHelpers.MainViewport.Size.Y - windowSize.Y - 100;
            }
        }

        public void Dispose() {
        }

        public void Draw() {
            if(!isVisible) {
                return;
            }

            ImGui.SetNextWindowPos(position,ImGuiCond.Appearing);
            ImGui.SetNextWindowSize(windowSize, ImGuiCond.Always);

            if(ImGui.Begin("Coin Pouch Aetheryte Menu", ref isVisible)) {

                foreach(Vendor vendor in vendors) {
                    Aetheryte aetheryte = VendorUtil.GetAetheryte(vendor);
                    ImGui.SetNextItemWidth(100);
                    if(ImGui.Button(AetheryteUtil.GetName(aetheryte), new(180, 20))) {
                        Teleporter.TeleportTo(aetheryte);

                        if(Plugin.config.pinVendor) {
                            Teleporter.AddMapCoordinate(aetheryte, vendor);
                        }

                        isVisible = false;
                    }
                }

                windowSize = ImGui.GetWindowSize();
                ImGui.End();
            }
        }

    }
}