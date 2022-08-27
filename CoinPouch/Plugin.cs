using System.Collections.Generic;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using CoinPouch.Config;
using Dalamud.Data;
using Lumina.Excel.GeneratedSheets;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using CoinPouch.Data;
using System;
using System.Timers;
using Dalamud.Configuration;
using CoinPouch.Util;
using CoinPouch.SeFunctions;
using CoinPouch.Teleport;

namespace CoinPouch {
    public class Plugin : IDalamudPlugin {
        public string Name => "Coin Pouch";
        private readonly string commandName = "/coinpouch";

        public static Configuration config;
        private readonly PluginUI ui;
        public static readonly TeleportUI teleportUi = new();

        private bool loginAlertPending = false;
        private bool zoningAlertPending = false;
        private bool zoningAlertTimeout = false;
        private int lastZoningAlertTimeout = -1;
        private bool liveAlertPending = false;
        private readonly Timer liveAlertTimer;
        private Dictionary<int, int> currencyQuantities = new();
        private Dictionary<int, DalamudLinkPayload> teleportLinkHandlers = new();

        [PluginService] public static DalamudPluginInterface PluginInterface { get; set; } = null!;
        [PluginService] public static KeyState KeyState { get; set; } = null!;
        [PluginService] public static Framework Framework { get; set; } = null!;
        [PluginService] public static ClientState ClientState { get; set; } = null!;
        [PluginService] public static Dalamud.Game.ClientState.Conditions.Condition Condition { get; set; } = null!;
        [PluginService] public static CommandManager CommandManager { get; set; } = null!;
        [PluginService] public static ChatGui ChatGui { get; set; } = null!;
        [PluginService] public static GameGui GameGui { get; set; } = null!;
        [PluginService] public static DataManager DataManager { get; set; } = null!;
        [PluginService] public static SigScanner SigScanner { get; set; } = null!;
        public static PlaySound PlaySound { get; private set; } = null!;

        public Plugin() {
            PlaySound = new PlaySound(SigScanner);

            IPluginConfiguration? loadedConfig = PluginInterface.GetPluginConfig();

            if(loadedConfig == null || loadedConfig.Version != 1) {
                config = new();
            } else {
                config = (Configuration)loadedConfig;
            }

            config.Initialize();

            ui = new PluginUI(config);
            PluginInterface.UiBuilder.Draw += ui.Draw;
            PluginInterface.UiBuilder.Draw += teleportUi.Draw;
            PluginInterface.UiBuilder.OpenConfigUi += () => ui.isVisible = true;

            Framework.Update += OnUpdate;
            ClientState.Login += OnLogin;
            ClientState.Logout += OnLogout;
            ClientState.TerritoryChanged += OnTerritoryChanged;

            liveAlertTimer = new() {
                Interval = 10 * 1000,
                AutoReset = true
            };
            liveAlertTimer.Elapsed += (object sender, ElapsedEventArgs e) => { liveAlertPending = true; };

            EnumUtil.Each((Currency currency) => {
                int id = CurrencyUtil.GetId(currency);
                teleportLinkHandlers[id] = PluginInterface.AddChatLinkHandler((uint)id, (uint id, SeString message) => {
                    Currency currency = (Currency)CurrencyUtil.GetById((int)id);

                    Teleporter.RequestTeleport(currency);
                });
            });

            CommandManager.AddHandler(commandName, new CommandInfo(OnCommand) {
                HelpMessage = "Opens the configuration interface."
            });

            if(ClientState.IsLoggedIn) {
                LoadInitialQuantities();
                liveAlertTimer.Start();
            }
        }

        public void Dispose() {
            Framework.Update -= OnUpdate;
            ClientState.Login -= OnLogin;
            ClientState.Logout -= OnLogout;
            ClientState.TerritoryChanged -= OnTerritoryChanged;

            liveAlertTimer.Stop();
            liveAlertTimer.Dispose();

            CommandManager.RemoveHandler(commandName);
        }

        private void OnUpdate(Framework framework) {
            if(Condition[ConditionFlag.BetweenAreas]) {
                return;
            }

            if(loginAlertPending) {
                loginAlertPending = false;
                zoningAlertPending = false;

                if(config.loginAlerts) {
                    Timeout(() => { SendAllAlerts(false); }, 1000);
                }
            }

            if(zoningAlertPending) {
                zoningAlertPending = false;

                // Only send zoning alerts if they are enabled and zoning alerts aren't in timeout, or if the zoning timeout has changed.
                if(config.zoningAlerts && (!zoningAlertTimeout || config.zoningAlertsTimeout != lastZoningAlertTimeout)) {
                    if(config.zoningAlertsTimeout > 0) {
                    // Set the timeout.
                    zoningAlertTimeout = true;
                    lastZoningAlertTimeout = config.zoningAlertsTimeout;

                    // Set the timer to reset the timeout.
                    Timeout(() => { zoningAlertTimeout = false; }, config.zoningAlertsTimeout * 60 * 1000);
                    }

                    // Send the alerts.
                    Timeout(SendAllAlerts, 1000);
                }
            }

            if(liveAlertPending) {
                liveAlertPending = false;
                if(config.liveAlerts) {
                    SendLiveAlerts();
                }
            }
        }

        private void OnLogin(object? sender, EventArgs e) {
            loginAlertPending = true;

            LoadInitialQuantities();
            liveAlertTimer.Start();
        }

        private void OnLogout(object? sender, EventArgs e) {
            liveAlertTimer.Stop();
        }

        private void OnTerritoryChanged(object? sender, ushort territoryId) {
            zoningAlertPending = true;
        }

        private void OnCommand(string command, string args) {
            if(args == "debug") {
                SendAllAlerts(false, true);
            }
            ui.isVisible = !ui.isVisible;
        }

        private void LoadInitialQuantities() {
            EnumUtil.Each((Currency currency) => {
                int id = CurrencyUtil.GetId(currency);
                int quantity = CurrencyUtil.GetQuantity(currency);

                currencyQuantities[id] = quantity;
            });
        }

        private void SendAllAlerts() {
            SendAllAlerts(true);
        }
        private void SendAllAlerts(bool playSound) {
            SendAllAlerts(playSound, false);
        }
        private void SendAllAlerts(bool playSound, bool debug) {
            List<(Currency, int, int)> alerts = new();

            foreach(KeyValuePair<int, AlertConfig> entry in config.alerts) {
                int currencyId = entry.Key;
                AlertConfig alertConfig = entry.Value;

                if(!alertConfig.enabled && !debug) {
                    continue;
                }

                Currency? nullishCurrency = CurrencyUtil.GetById(currencyId);

                if(!nullishCurrency.HasValue) {
                    continue;
                }

                Currency currency = CurrencyUtil.GetById(currencyId)!.Value;

                int quantity = CurrencyUtil.GetQuantity(currency);

                currencyQuantities[currencyId] = quantity;

                if(quantity < alertConfig.threshold && !debug) {
                    continue;
                }

                alerts.Add((currency, quantity, alertConfig.threshold));
            }

            if(alerts.Count > 0 && playSound && config.alertSound) {
                PlaySound.Play((Sound)config.sound);
            }

            PrintCurrencyAlerts(alerts);
        }

        private void SendLiveAlerts() {
            List<(Currency, int, int)> alerts = new();

            foreach(KeyValuePair<int, AlertConfig> entry in config.alerts) {
                int currencyId = entry.Key;
                AlertConfig alertConfig = entry.Value;

                if(!alertConfig.enabled) {
                    continue;
                }

                Currency? nullishCurrency = CurrencyUtil.GetById(currencyId);

                if(!nullishCurrency.HasValue) {
                    continue;
                }

                Currency currency = CurrencyUtil.GetById(currencyId)!.Value;

                int quantity = CurrencyUtil.GetQuantity(currency);
                int previousQuantity = 0;

                if(currencyQuantities.ContainsKey(currencyId)) {
                    previousQuantity = currencyQuantities[currencyId];
                }

                currencyQuantities[currencyId] = quantity;

                if(quantity < alertConfig.threshold) {
                    continue;
                }

                if(previousQuantity >= alertConfig.threshold) {
                    continue;
                }

                alerts.Add((currency, quantity, alertConfig.threshold));
            }

            if(alerts.Count > 0 && config.alertSound) {
                PlaySound.Play((Sound)config.sound);
            }

            PrintCurrencyAlerts(alerts);
        }

        private void PrintCurrencyAlerts(List<(Currency, int, int)> currencyAlerts) {
            currencyAlerts.ForEach((currencyAlert) => {
                PrintCurrencyAlert(currencyAlert.Item1, currencyAlert.Item2, currencyAlert.Item3);
            });
        }

        public void PrintCurrencyAlert(Currency currency, int quantity, int threshold) {
            Item item = DataManager.Excel.GetSheet<Item>()!.GetRow((uint)CurrencyUtil.GetId(currency))!;

            SeStringBuilder stringBuilder = new();
            stringBuilder.AddUiForeground(62);
            stringBuilder.AddText($"[Coin Pouch] ");
            stringBuilder.AddUiForegroundOff();

            stringBuilder.AddUiForeground((ushort)(0x223 + item.Rarity * 2));
            stringBuilder.AddUiGlow((ushort)(0x224 + item.Rarity * 2));

            stringBuilder.AddUiForeground(0);
            stringBuilder.AddUiGlow(0);
            stringBuilder.AddText(CurrencyUtil.GetName(currency));

            stringBuilder.AddUiForeground(1);
            stringBuilder.AddText($" {quantity}/{CurrencyUtil.GetMaximum(currency)}");

            Vendor[] vendors = CurrencyUtil.GetVendors(currency);

            if(vendors?.Length > 0) {
                stringBuilder.Add(teleportLinkHandlers[CurrencyUtil.GetId(currency)]);
                stringBuilder.AddText(" ");
                stringBuilder.AddIcon(BitmapFontIcon.Aetheryte);
                stringBuilder.AddText(" ");
                stringBuilder.Add(RawPayload.LinkTerminator);
            }

            ChatGui.Print(stringBuilder.BuiltString);
        }

        public void Timeout(System.Action action, int timeout) {
            Timer timer = new() {
                Interval = timeout
            };
            timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                action();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}