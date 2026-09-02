using System;
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

        // Character States
        public static ConfigEntry<bool> DeadEnabled;
        public static ConfigEntry<float> DeadIntensity;
        public static ConfigEntry<float> DeadTime;

        public static ConfigEntry<bool> PassedOutEnabled;
        public static ConfigEntry<float> PassedOutIntensity;
        public static ConfigEntry<float> PassedOutTime;

        public static ConfigEntry<bool> RagdollEnabled;
        public static ConfigEntry<float> RagdollIntensity;
        public static ConfigEntry<float> RagdollTime;

        public ConfigManager(ConfigFile config)
        {
            this.config = config;

            LoadConfig();
        }

        private void LoadConfig()
        {
            Enabled = config.Bind("General", "Enabled", true, "Enable or Disable toy control");
            Enabled.SettingChanged += EnabledSettingChnaged;
            MaximumIntensity = config.Bind("General", "Maximum Intensity", 0.7f,
                new ConfigDescription("Maximumtoy intensity from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ServerIP = config.Bind("General", "Server IP", "127.0.0.1", "IP address of the Intiface Server");
            ServerPort = config.Bind("General", "Server Port", 12345, "Port of the Intiface Server");
            // TODo: Fix this, it currently has a problem inside of ButtplugManager
            //ServerIP.SettingChanged += ServerAdressChnaged;
            //ServerPort.SettingChanged += ServerAdressChnaged;

            UseCarried = config.Bind(new ConfigDefinition("Player State", "Use Carried"), true,
                new ConfigDescription("Use the player who is carring you for triggers"));
            UseSpectate = config.Bind(new ConfigDefinition("Player State", "Use Spectate"), true,
                new ConfigDescription("Use the player who you are Spectating for triggers"));

            // Load movement trigger settings
            // Normal Climbing Trigger
            NormalClimbingTriggerEnabled = config.Bind("Movement", "Normal Climbing Trigger Enabled", true, "Enable or Disable Normal Climbing Trigger");
            NormalClimbingTriggerMaximumIntensity = config.Bind("Movement", "Normal Climbing Trigger Maximum Intensity", 0.7f,
                new ConfigDescription("Maximum intensity for Normal Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            NormalClimbingTriggerMinimumIntensity = config.Bind("Movement", "Normal Climbing Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Normal Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Rope Climbing Trigger
            RopeClimbingTriggerEnabled = config.Bind("Movement", "Rope Climbing Trigger Enabled", true, "Enable or Disable Rope Climbing Trigger");
            RopeClimbingTriggerMaximumIntensity = config.Bind("Movement", "Rope Climbing Trigger Maximum Intensity", 0.7f,
                new ConfigDescription("Maximum intensity for Rope Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            RopeClimbingTriggerMinimumIntensity = config.Bind("Movement", "Rope Climbing Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Rope Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Vine Climbing Trigger
            VineClimbingTriggerEnabled = config.Bind("Movement", "Vine Climbing Trigger Enabled", true, "Enable or Disable Vine Climbing Trigger");
            VineClimbingTriggerMaximumIntensity = config.Bind("Movement", "Vine Climbing Trigger Maximum Intensity", 0.7f,
                new ConfigDescription("Maximum intensity for Vine Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            VineClimbingTriggerMinimumIntensity = config.Bind("Movement", "Vine Climbing Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Vine Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Sprinting Trigger
            SprintingTriggerEnabled = config.Bind("Movement", "Sprinting Trigger Enabled", true, "Enable or Disable Sprinting Trigger");
            SprintingTriggerMaximumIntensity = config.Bind("Movement", "Sprinting Trigger Maximum Intensity", 0.7f,
                new ConfigDescription("Maximum intensity for Sprinting Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SprintingTriggerMinimumIntensity = config.Bind("Movement", "Sprinting Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Sprinting Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));

            // Load status effect trigger settings
            InjuryTriggerEnabled = config.Bind("Triggers", "Injury Trigger Enabled", true, "Enable or Disable Injury Trigger");
            InjuryTriggerMaximumIntensity = config.Bind("Triggers", "Injury Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Injury Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            InjuryTriggerMinimumIntensity = config.Bind("Triggers", "Injury Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Injury Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            InjuryTriggerDuration = config.Bind("Triggers", "Injury Trigger Duration", 5000f,
                new ConfigDescription("Duration for Injury Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ColdTriggerEnabled = config.Bind("Triggers", "Cold Trigger Enabled", true, "Enable or Disable Cold Trigger");
            ColdTriggerMaximumIntensity = config.Bind("Triggers", "Cold Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Cold Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ColdTriggerMinimumIntensity = config.Bind("Triggers", "Cold Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Cold Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ColdTriggerDuration = config.Bind("Triggers", "Cold Trigger Duration", 1000f,
                new ConfigDescription("Duration for Cold Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            PoisonTriggerEnabled = config.Bind("Triggers", "Poison Trigger Enabled", true, "Enable or Disable Poison Trigger");
            PoisonTriggerMaximumIntensity = config.Bind("Triggers", "Poison Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Poison Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PoisonTriggerMinimumIntensity = config.Bind("Triggers", "Poison Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Poison Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PoisonTriggerDuration = config.Bind("Triggers", "Poison Trigger Duration", 3000f,
                new ConfigDescription("Duration for Poison Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            CurseTriggerEnabled = config.Bind("Triggers", "Curse Trigger Enabled", true, "Enable or Disable Curse Trigger");
            CurseTriggerMaximumIntensity = config.Bind("Triggers", "Curse Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Curse Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            CurseTriggerMinimumIntensity = config.Bind("Triggers", "Curse Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Curse Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            CurseTriggerDuration = config.Bind("Triggers", "Curse Trigger Duration", 5000f,
                new ConfigDescription("Duration for Curse Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            DrowsyTriggerEnabled = config.Bind("Triggers", "Drowsy Trigger Enabled", true, "Enable or Disable Drowsy Trigger");
            DrowsyTriggerMaximumIntensity = config.Bind("Triggers", "Drowsy Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Drowsy Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            DrowsyTriggerMinimumIntensity = config.Bind("Triggers", "Drowsy Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Drowsy Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            DrowsyTriggerDuration = config.Bind("Triggers", "Drowsy Trigger Duration", 1000f,
                new ConfigDescription("Duration for Drowsy Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            HotTriggerEnabled = config.Bind("Triggers", "Hot Trigger Enabled", true, "Enable or Disable Hot Trigger");
            HotTriggerMaximumIntensity = config.Bind("Triggers", "Hot Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Hot Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            HotTriggerMinimumIntensity = config.Bind("Triggers", "Hot Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Hot Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            HotTriggerDuration = config.Bind("Triggers", "Hot Trigger Duration", 1000f,
                new ConfigDescription("Duration for Hot Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ThornsTriggerEnabled = config.Bind("Triggers", "Thorns Trigger Enabled", true, "Enable or Disable Thorns Trigger");
            ThornsTriggerMaximumIntensity = config.Bind("Triggers", "Thorns Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Thorns Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ThornsTriggerMinimumIntensity = config.Bind("Triggers", "Thorns Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Thorns Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ThornsTriggerDuration = config.Bind("Triggers", "Thorns Trigger Duration", 3000f,
                new ConfigDescription("Duration for Thorns Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            SporesTriggerEnabled = config.Bind("Triggers", "Spores Trigger Enabled", true, "Enable or Disable Spores Trigger");
            SporesTriggerMaximumIntensity = config.Bind("Triggers", "Spores Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Spores Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SporesTriggerMinimumIntensity = config.Bind("Triggers", "Spores Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Spores Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SporesTriggerDuration = config.Bind("Triggers", "Spores Trigger Duration", 5000f,
                new ConfigDescription("Duration for Spores Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            WebTriggerEnabled = config.Bind("Triggers", "Web Trigger Enabled", true, "Enable or Disable Web Trigger");
            WebTriggerMaximumIntensity = config.Bind("Triggers", "Web Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Web Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            WebTriggerMinimumIntensity = config.Bind("Triggers", "Web Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Web Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            WebTriggerDuration = config.Bind("Triggers", "Web Trigger Duration", 2000f,
                new ConfigDescription("Duration for Web Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ArrowTriggerEnabled = config.Bind("Triggers", "Arrow Trigger Enabled", true, "Enable or Disable Arrow Trigger");
            ArrowTriggerMaximumIntensity = config.Bind("Triggers", "Arrow Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Arrow Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ArrowTriggerMinimumIntensity = config.Bind("Triggers", "Arrow Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Arrow Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ArrowTriggerDuration = config.Bind("Triggers", "Arrow Trigger Duration", 5000f,
                new ConfigDescription("Duration for Arrow Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            PetrifyTriggerEnabled = config.Bind("Triggers", "Petrify Trigger Enabled", true, "Enable or Disable Petrify Trigger");
            PetrifyTriggerMaximumIntensity = config.Bind("Triggers", "Petrify Trigger Maximum Intensity", 0.5f,
                new ConfigDescription("Maximum intensity for Petrify Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PetrifyTriggerMinimumIntensity = config.Bind("Triggers", "Petrify Trigger Minimum Intensity", 0.1f,
                new ConfigDescription("Minimum intensity for Petrify Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PetrifyTriggerDuration = config.Bind("Triggers", "Petrify Trigger Duration", 5000f,
                new ConfigDescription("Duration for Petrify Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            DeadEnabled = config.Bind(new ConfigDefinition("Player State", "Dead Enabled"), true,
                new ConfigDescription("Enabled on death to trigger toy."));
            DeadIntensity = config.Bind(new ConfigDefinition("Player State", "Dead Intensity"), 1f,
                new ConfigDescription("How strong the toy should go when you die.", new AcceptableValueRange<float>(0.01f, 1f)));
            DeadTime = config.Bind(new ConfigDefinition("Player State", "Dead Time"), 5000f,
                new ConfigDescription("How strong the toy should go when you die.", new AcceptableValueRange<float>(100f, 10000f)));

            PassedOutEnabled = config.Bind(new ConfigDefinition("Player State", "Passed Out Enabled"), true,
                new ConfigDescription("Enabled on passing out to trigger toy."));
            PassedOutIntensity = config.Bind(new ConfigDefinition("Player State", "Passed Out Intensity"), 1f,
                new ConfigDescription("How strong the toy should go when you pass out.", new AcceptableValueRange<float>(0.01f, 1f)));
            PassedOutTime = config.Bind(new ConfigDefinition("Player State", "Passed Out Time"), 5000f,
                new ConfigDescription("How long the toy should go when you pass out.", new AcceptableValueRange<float>(100f, 10000f)));

            RagdollEnabled = config.Bind(new ConfigDefinition("Player State", "Ragdoll Enabled"), true,
                new ConfigDescription("Enabled on ragdoll to trigger toy."));
            RagdollIntensity = config.Bind(new ConfigDefinition("Player State", "Ragdoll Intensity"), 1f,
                new ConfigDescription("How strong the toy should go when you ragdoll.", new AcceptableValueRange<float>(0.01f, 1f)));
            RagdollTime = config.Bind(new ConfigDefinition("Player State", "Ragdoll Time"), 5000f,
                new ConfigDescription("How long the toy should go when you ragdoll.", new AcceptableValueRange<float>(100f, 10000f)));
        }

        private void EnabledSettingChnaged(object sender, System.EventArgs e)
        {
            if (Plugin.ButtplugManager == null) return;

            if (Enabled.Value)
            {
                _ = Plugin.ButtplugManager.StartReconnecting();
            }
            else
            {
                _ = Plugin.ButtplugManager.DisconnectAsync();
            }
        }
    }
}