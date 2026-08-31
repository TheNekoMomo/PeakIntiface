using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using PeakIntiface.Buttplug;
using UnityEngine;

namespace PeakIntiface
{
    internal class ConfigManager
    {
        private readonly ConfigFile config;

        public static ConfigEntry<bool> Enabled;
        public static ConfigEntry<string> ServerIP;
        public static ConfigEntry<int> ServerPort;
        public static ConfigEntry<float> MaximumIntensity;
        // Config for when caried or watching a player
        public static ConfigEntry<bool> UseCarried;
        public static ConfigEntry<bool> UseSpectate;
        // Config entries for movement triggers
        public static ConfigEntry<bool> NormalClimbingTriggerEnabled;
        public static ConfigEntry<float> NormalClimbingTriggerMaximumIntensity;
        public static ConfigEntry<float> NormalClimbingTriggerMinimumIntensity;
        public static ConfigEntry<bool> RopeClimbingTriggerEnabled;
        public static ConfigEntry<float> RopeClimbingTriggerMaximumIntensity;
        public static ConfigEntry<float> RopeClimbingTriggerMinimumIntensity;
        public static ConfigEntry<bool> VineClimbingTriggerEnabled;
        public static ConfigEntry<float> VineClimbingTriggerMaximumIntensity;
        public static ConfigEntry<float> VineClimbingTriggerMinimumIntensity;
        public static ConfigEntry<bool> SprintingTriggerEnabled;
        public static ConfigEntry<float> SprintingTriggerMaximumIntensity;
        public static ConfigEntry<float> SprintingTriggerMinimumIntensity;
        // Config entries for status effect triggers
        public static ConfigEntry<bool> InjuryTriggerEnabled;
        public static ConfigEntry<float> InjuryTriggerMaximumIntensity;
        public static ConfigEntry<float> InjuryTriggerMinimumIntensity;
        public static ConfigEntry<float> InjuryTriggerDuration;

        public static ConfigEntry<bool> ColdTriggerEnabled;
        public static ConfigEntry<float> ColdTriggerMaximumIntensity;
        public static ConfigEntry<float> ColdTriggerMinimumIntensity;
        public static ConfigEntry<float> ColdTriggerDuration;

        public static ConfigEntry<bool> PoisonTriggerEnabled;
        public static ConfigEntry<float> PoisonTriggerMaximumIntensity;
        public static ConfigEntry<float> PoisonTriggerMinimumIntensity;
        public static ConfigEntry<float> PoisonTriggerDuration;

        public static ConfigEntry<bool> CurseTriggerEnabled;
        public static ConfigEntry<float> CurseTriggerMaximumIntensity;
        public static ConfigEntry<float> CurseTriggerMinimumIntensity;
        public static ConfigEntry<float> CurseTriggerDuration;

        public static ConfigEntry<bool> DrowsyTriggerEnabled;
        public static ConfigEntry<float> DrowsyTriggerMaximumIntensity;
        public static ConfigEntry<float> DrowsyTriggerMinimumIntensity;
        public static ConfigEntry<float> DrowsyTriggerDuration;

        public static ConfigEntry<bool> HotTriggerEnabled;
        public static ConfigEntry<float> HotTriggerMaximumIntensity;
        public static ConfigEntry<float> HotTriggerMinimumIntensity;
        public static ConfigEntry<float> HotTriggerDuration;

        public static ConfigEntry<bool> ThornsTriggerEnabled;
        public static ConfigEntry<float> ThornsTriggerMaximumIntensity;
        public static ConfigEntry<float> ThornsTriggerMinimumIntensity;
        public static ConfigEntry<float> ThornsTriggerDuration;

        public static ConfigEntry<bool> SporesTriggerEnabled;
        public static ConfigEntry<float> SporesTriggerMaximumIntensity;
        public static ConfigEntry<float> SporesTriggerMinimumIntensity;
        public static ConfigEntry<float> SporesTriggerDuration;

        public static ConfigEntry<bool> WebTriggerEnabled;
        public static ConfigEntry<float> WebTriggerMaximumIntensity;
        public static ConfigEntry<float> WebTriggerMinimumIntensity;
        public static ConfigEntry<float> WebTriggerDuration;

        public static ConfigEntry<bool> ArrowTriggerEnabled;
        public static ConfigEntry<float> ArrowTriggerMaximumIntensity;
        public static ConfigEntry<float> ArrowTriggerMinimumIntensity;
        public static ConfigEntry<float> ArrowTriggerDuration;

        public static ConfigEntry<bool> PetrifyTriggerEnabled;
        public static ConfigEntry<float> PetrifyTriggerMaximumIntensity;
        public static ConfigEntry<float> PetrifyTriggerMinimumIntensity;
        public static ConfigEntry<float> PetrifyTriggerDuration;

        public ConfigManager(ConfigFile config)
        {
            this.config = config;

            LoadConfig();
        }

        private void LoadConfig()
        {
            Enabled = config.Bind("General", "Enabled", true, "Enable or Disable toy control");
            Enabled.SettingChanged += EnabledSettingChnaged;
            MaximumIntensity = config.Bind("General", "MaximumIntensity", 0.7f,
                new ConfigDescription("Maximumtoy intensity from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ServerIP = config.Bind("Connection", "ServerIP", "127.0.0.1", "IP address of the Intiface Server");
            ServerPort = config.Bind("Connection", "ServerPort", 12345, "Port of the Intiface Server");

            UseCarried = config.Bind(new ConfigDefinition("Use Carried Stats", "UseCarried"), true, 
                new ConfigDescription("Use the player who is carring you for triggers"));
            UseSpectate = config.Bind(new ConfigDefinition("Use Spectate Stats", "UseSpectate"), true,
                new ConfigDescription("Use the player who you are Spectating for triggers"));

            // Load movement trigger settings
            // Normal Climbing Trigger
            NormalClimbingTriggerEnabled = config.Bind("Triggers", "NormalClimbingTriggerEnabled", true, "Enable or Disable Normal Climbing Trigger");
            NormalClimbingTriggerMaximumIntensity = config.Bind("Triggers", "NormalClimbingTriggerMaximumIntensity", 0.7f,
                new ConfigDescription("Maximum intensity for Normal Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            NormalClimbingTriggerMinimumIntensity = config.Bind("Triggers", "NormalClimbingTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Normal Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Rope Climbing Trigger
            RopeClimbingTriggerEnabled = config.Bind("Triggers", "RopeClimbingTriggerEnabled", true, "Enable or Disable Rope Climbing Trigger");
            RopeClimbingTriggerMaximumIntensity = config.Bind("Triggers", "RopeClimbingTriggerMaximumIntensity", 0.7f,
                new ConfigDescription("Maximum intensity for Rope Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            RopeClimbingTriggerMinimumIntensity = config.Bind("Triggers", "RopeClimbingTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Rope Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Vine Climbing Trigger
            VineClimbingTriggerEnabled = config.Bind("Triggers", "VineClimbingTriggerEnabled", true, "Enable or Disable Vine Climbing Trigger");
            VineClimbingTriggerMaximumIntensity = config.Bind("Triggers", "VineClimbingTriggerMaximumIntensity", 0.7f,
                new ConfigDescription("Maximum intensity for Vine Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            VineClimbingTriggerMinimumIntensity = config.Bind("Triggers", "VineClimbingTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Vine Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Sprinting Trigger
            SprintingTriggerEnabled = config.Bind("Triggers", "SprintingTriggerEnabled", true, "Enable or Disable Sprinting Trigger");
            SprintingTriggerMaximumIntensity = config.Bind("Triggers", "SprintingTriggerMaximumIntensity", 0.7f,
                new ConfigDescription("Maximum intensity for Sprinting Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SprintingTriggerMinimumIntensity = config.Bind("Triggers", "SprintingTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Sprinting Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));

            // Load status effect trigger settings
            InjuryTriggerEnabled = config.Bind("Triggers", "InjuryTriggerEnabled", true, "Enable or Disable Injury Trigger");
            InjuryTriggerMaximumIntensity = config.Bind("Triggers", "InjuryTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Injury Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            InjuryTriggerMinimumIntensity = config.Bind("Triggers", "InjuryTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Injury Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            InjuryTriggerDuration = config.Bind("Triggers", "InjuryTriggerDuration", 5000f,
                new ConfigDescription("Duration for Injury Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ColdTriggerEnabled = config.Bind("Triggers", "ColdTriggerEnabled", true, "Enable or Disable Cold Trigger");
            ColdTriggerMaximumIntensity = config.Bind("Triggers", "ColdTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Cold Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ColdTriggerMinimumIntensity = config.Bind("Triggers", "ColdTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Cold Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ColdTriggerDuration = config.Bind("Triggers", "ColdTriggerDuration", 1000f,
                new ConfigDescription("Duration for Cold Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            PoisonTriggerEnabled = config.Bind("Triggers", "PoisonTriggerEnabled", true, "Enable or Disable Poison Trigger");
            PoisonTriggerMaximumIntensity = config.Bind("Triggers", "PoisonTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Poison Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PoisonTriggerMinimumIntensity = config.Bind("Triggers", "PoisonTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Poison Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PoisonTriggerDuration = config.Bind("Triggers", "PoisonTriggerDuration", 3000f,
                new ConfigDescription("Duration for Poison Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            CurseTriggerEnabled = config.Bind("Triggers", "CurseTriggerEnabled", true, "Enable or Disable Curse Trigger");
            CurseTriggerMaximumIntensity = config.Bind("Triggers", "CurseTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Curse Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            CurseTriggerMinimumIntensity = config.Bind("Triggers", "CurseTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Curse Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            CurseTriggerDuration = config.Bind("Triggers", "CurseTriggerDuration", 5000f,
                new ConfigDescription("Duration for Curse Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            DrowsyTriggerEnabled = config.Bind("Triggers", "DrowsyTriggerEnabled", true, "Enable or Disable Drowsy Trigger");
            DrowsyTriggerMaximumIntensity = config.Bind("Triggers", "DrowsyTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Drowsy Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            DrowsyTriggerMinimumIntensity = config.Bind("Triggers", "DrowsyTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Drowsy Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            DrowsyTriggerDuration = config.Bind("Triggers", "DrowsyTriggerDuration", 1000f,
                new ConfigDescription("Duration for Drowsy Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            HotTriggerEnabled = config.Bind("Triggers", "HotTriggerEnabled", true, "Enable or Disable Hot Trigger");
            HotTriggerMaximumIntensity = config.Bind("Triggers", "HotTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Hot Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            HotTriggerMinimumIntensity = config.Bind("Triggers", "HotTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Hot Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            HotTriggerDuration = config.Bind("Triggers", "HotTriggerDuration", 1000f,
                new ConfigDescription("Duration for Hot Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ThornsTriggerEnabled = config.Bind("Triggers", "ThornsTriggerEnabled", true, "Enable or Disable Thorns Trigger");
            ThornsTriggerMaximumIntensity = config.Bind("Triggers", "ThornsTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Thorns Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ThornsTriggerMinimumIntensity = config.Bind("Triggers", "ThornsTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Thorns Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ThornsTriggerDuration = config.Bind("Triggers", "ThornsTriggerDuration", 3000f,
                new ConfigDescription("Duration for Thorns Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            SporesTriggerEnabled = config.Bind("Triggers", "SporesTriggerEnabled", true, "Enable or Disable Spores Trigger");
            SporesTriggerMaximumIntensity = config.Bind("Triggers", "SporesTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Spores Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SporesTriggerMinimumIntensity = config.Bind("Triggers", "SporesTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Spores Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SporesTriggerDuration = config.Bind("Triggers", "SporesTriggerDuration", 5000f,
                new ConfigDescription("Duration for Spores Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            WebTriggerEnabled = config.Bind("Triggers", "WebTriggerEnabled", true, "Enable or Disable Web Trigger");
            WebTriggerMaximumIntensity = config.Bind("Triggers", "WebTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Web Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            WebTriggerMinimumIntensity = config.Bind("Triggers", "WebTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Web Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            WebTriggerDuration = config.Bind("Triggers", "WebTriggerDuration", 2000f,
                new ConfigDescription("Duration for Web Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ArrowTriggerEnabled = config.Bind("Triggers", "ArrowTriggerEnabled", true, "Enable or Disable Arrow Trigger");
            ArrowTriggerMaximumIntensity = config.Bind("Triggers", "ArrowTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Arrow Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ArrowTriggerMinimumIntensity = config.Bind("Triggers", "ArrowTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Arrow Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ArrowTriggerDuration = config.Bind("Triggers", "ArrowTriggerDuration", 5000f,
                new ConfigDescription("Duration for Arrow Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            PetrifyTriggerEnabled = config.Bind("Triggers", "PetrifyTriggerEnabled", true, "Enable or Disable Petrify Trigger");
            PetrifyTriggerMaximumIntensity = config.Bind("Triggers", "PetrifyTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Petrify Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PetrifyTriggerMinimumIntensity = config.Bind("Triggers", "PetrifyTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Petrify Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PetrifyTriggerDuration = config.Bind("Triggers", "PetrifyTriggerDuration", 5000f,
                new ConfigDescription("Duration for Petrify Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));
        }

        public void EnabledSettingChnaged(object sender, System.EventArgs e)
        {
            if (Plugin.ButtplugManager == null) return;

            if (Enabled.Value)
            {
                _ = Plugin.ButtplugManager.StartReconnecting(ServerIP.Value, ServerPort.Value);
            }
            else
            {
                _ = Plugin.ButtplugManager.DisconnectAsync();
            }
        }
    }
}
