using System;
using System.Numerics;
using CoinPouch.Config;
using ImGuiNET;
using CoinPouch.Util;
using CoinPouch.Data;
using CoinPouch.Data.Attributes;

namespace CoinPouch {
    public class PluginUI : IDisposable {

        private Configuration config;

        public bool isVisible = false;

        private Vector2 windowSize = new(400, 500);

        public PluginUI(Configuration config) {
            this.config = config;
        }

        public void Dispose() {
        }

        public void Draw() {
            if(!isVisible) {
                return;
            }

            ImGui.SetNextWindowSize(windowSize, ImGuiCond.Always);

            if(ImGui.Begin("Coin Pouch Configuration", ref isVisible)) {
                if(ImGui.BeginTabBar("")) {
                    DrawAlertsTab();
                    DrawSettingsTab();
                    ImGui.EndTabBar();
                }
                windowSize = ImGui.GetWindowSize();
                ImGui.End();
            }
        }

        private void DrawAlertsTab() {
            if(ImGui.BeginTabItem("Alerts")) {
                if(ImGui.BeginTable("alerts-table", 2)) {
                    ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 200);
                    ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 100);

                    // Headers
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.Text("Currency");
                    ImGui.TableNextColumn();
                    ImGui.Text("Threshold");

                    string currentCategory = "";
                    EnumUtil.Each<Currency>((currency) => {
                        ImGui.TableNextRow();
                        ImGui.TableNextColumn();
                        int id = EnumUtil.GetAttribute<IDAttribute>(currency).Value;
                        string name = EnumUtil.GetAttribute<NameAttribute>(currency).Value;
                        string category = EnumUtil.GetAttribute<CategoryAttribute>(currency).Value;

                        if(category != currentCategory) {
                            currentCategory = category;
                            ImGui.Text(category);
                            ImGui.TableNextRow();
                            ImGui.TableSetColumnIndex(0);
                        }

                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 4);
                        if(ImGui.Checkbox($"##{id}-enabled", ref config.alerts[id].enabled)) {
                            config.Save();
                        }
                        ImGui.SameLine();
                        ImGui.Text(name);

                        ImGui.TableNextColumn();
                        ImGui.SetNextItemWidth(ImGui.GetColumnWidth());
                        if(ImGui.InputInt($"##{id}-threshold", ref config.alerts[id].threshold, 0)) {
                            config.Save();
                        }

                        if(ImGui.IsItemHovered()) {
                            if(currency == Currency.StormSeal || currency == Currency.SerpentSeal || currency == Currency.FlameSeal) {
                                ImGui.SetTooltip($"Maximum: {CurrencyUtil.GetMaximum(currency)}\n(Dependent on ranking)");
                            } else {
                                ImGui.SetTooltip($"Maximum: {CurrencyUtil.GetMaximum(currency)}");
                            }
                        }
                    });

                    ImGui.EndTable();
                }
                ImGui.EndTabItem();
            }
        }

        private void DrawSettingsTab() {
            if(ImGui.BeginTabItem("Settings")) {
                if(ImGui.BeginTable("alerts-table", 2)) {
                    ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 160);
                    ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 160);

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    if(ImGui.Checkbox($"##login-alerts", ref config.loginAlerts)) {
                        config.Save();
                    }
                    ImGui.SameLine();
                    ImGui.Text("Enable login alerts");

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    if(ImGui.Checkbox("##zoning-alerts", ref config.zoningAlerts)) {
                        config.Save();
                    }
                    ImGui.SameLine();
                    ImGui.Text("Enable zoning alerts");

                    if(config.zoningAlerts) {
                        ImGui.TableNextColumn();
                        ImGui.SetNextItemWidth(50);
                        if(ImGui.InputInt("##zoning-alerts-timeout", ref config.zoningAlertsTimeout,0)) {
                            if(config.zoningAlertsTimeout < 0) {
                                config.zoningAlertsTimeout = 0;
                            }
                            config.Save();
                        }
                        ImGui.SameLine();
                        ImGui.Text("Timeout (minutes)");
                    }

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    if(ImGui.Checkbox("##live-alerts", ref config.liveAlerts)) {
                        config.Save();
                    }
                    ImGui.SameLine();
                    ImGui.Text("Enable live alerts");

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    if(ImGui.Checkbox("##sound-alerts", ref config.alertSound)) {
                        config.Save();
                    }
                    ImGui.SameLine();
                    ImGui.Text("Enable alert sound effect");

                    if(config.alertSound) {
                        ImGui.TableNextColumn();
                        ImGui.SetNextItemWidth(70);
                        if(ImGui.BeginCombo("##alert-sound", SoundUtil.GetName((Sound)config.sound))) {
                            EnumUtil.Each((Sound sound) => {
                                if(sound == Sound.None || sound == Sound.Unknown) {
                                    return;
                                }

                                if(ImGui.Selectable(SoundUtil.GetName(sound))) {
                                    config.sound = (byte)sound;
                                    config.Save();

                                    Plugin.PlaySound.Play(sound);
                                }
                            });

                            ImGui.EndCombo();
                        }
                    }

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    if(ImGui.Checkbox("##pin-vendor", ref config.pinVendor)) {
                        config.Save();
                    }
                    ImGui.SameLine();
                    ImGui.Text("Pin vendor location on teleport");

                    ImGui.EndTable();
                }
                ImGui.EndTabItem();
            }
        }
    }
}