using System;
using System.Collections.Generic;
using CoinPouch.Data;
using CoinPouch.Data.Attributes;
using CoinPouch.Util;
using Dalamud.Configuration;

namespace CoinPouch.Config {

    public class AlertConfig {
        public bool enabled;
        public int threshold;
    }

    [Serializable]
    public class Configuration : IPluginConfiguration {
        public event Action? OnSave;

        public int Version { get; set; } = 1;
        public bool loginAlerts = true;
        public bool zoningAlerts = true;
        public int zoningAlertsTimeout = 5;
        public bool liveAlerts = true;
        public bool alertSound = true;
        public byte sound = (byte)Sound.Sound02;
        public bool pinVendor = true;

        public Dictionary<int, AlertConfig> alerts { get; set; } = null!;

        public void Initialize() {
            // Initialise the config.
            if(alerts == null) {
                alerts = new();
            }

            EnumUtil.Each<Currency>((currency) => {
                int id = EnumUtil.GetAttribute<IDAttribute>(currency).Value;
                if(!alerts.ContainsKey(id)) {
                    int threshold = EnumUtil.GetAttribute<DefaultThresholdAttribute>(currency).Value;
                    alerts[id] = new() {
                        enabled = true,
                        threshold = threshold
                    };
                }
            });

            Save();
        }

        public void Save() {
            Plugin.PluginInterface.SavePluginConfig(this);
            OnSave?.Invoke();
        }
    }
}