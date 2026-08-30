using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using PeakIntiface.Buttplug;
using PeakIntiface.Toy;
using PeakIntiface.Triggers;


namespace PeakIntiface
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "momo.peakintiface";
        public const string PluginName = "PEAK Intiface";
        public const string PluginVersion = "0.1.0";

        #region Config Entries
        // Config entries
        public static ConfigEntry<bool> Enabled;
        public static ConfigEntry<string> ServerIP;
        public static ConfigEntry<int> ServerPort;
        public static ConfigEntry<float> MaximumIntensity;
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

        public static ConfigEntry<bool> HungerTriggerEnabled;
        public static ConfigEntry<float> HungerTriggerMaximumIntensity;
        public static ConfigEntry<float> HungerTriggerMinimumIntensity;
        public static ConfigEntry<float> HungerTriggerDuration;

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
        #endregion

        public static ButtplugManager ButtplugManager;
        public static ToyController ToyController;

        public static MovementTrigger MovementTrigger;
        public static StatustEffectTrigger StatustEffectTrigger;

        private void Awake()
        {
            Logger.LogInfo($"{PluginName} v{PluginVersion} loading...");

            LoadConfig();

            ButtplugManager = new ButtplugManager(Logger);
            if (Enabled.Value) _ = ButtplugManager.StartReconnecting(ServerIP.Value, ServerPort.Value);
            ToyController = new ToyController(ButtplugManager, Logger);

            MovementTrigger = new MovementTrigger(Logger);
            StatustEffectTrigger = new StatustEffectTrigger(Logger);

            Logger.LogInfo($"Intiface Address: {ServerIP.Value}:{ServerPort.Value}");
            Logger.LogInfo($"{PluginName} Loaded!");
        }

        private void Update()
        {
            // Run the Trigger updates only if the plugin is enabled, the ToyController is initialized, and the ButtplugManager is connected
            if (!Enabled.Value || ToyController == null || !ButtplugManager.IsConnected) return;
            MovementTrigger?.Update();
            StatustEffectTrigger?.Update();
        }

        private void LoadConfig()
        {
            Enabled = Config.Bind("General", "Enabled", true, "Enable or Disable toy control");
            Enabled.SettingChanged += EnabledSettingChnaged;
            MaximumIntensity = Config.Bind("General", "MaximumIntensity", 0.5f, 
                new ConfigDescription("Maximumtoy intensity from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ServerIP = Config.Bind("Connection", "ServerIP", "127.0.0.1", "IP address of the Intiface Server");
            ServerPort = Config.Bind("Connection", "ServerPort", 12345, "Port of the Intiface Server");

            // Load movement trigger settings
            // Normal Climbing Trigger
            NormalClimbingTriggerEnabled = Config.Bind("Triggers", "NormalClimbingTriggerEnabled", true, "Enable or Disable Normal Climbing Trigger");
            NormalClimbingTriggerMaximumIntensity = Config.Bind("Triggers", "NormalClimbingTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Normal Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            NormalClimbingTriggerMinimumIntensity = Config.Bind("Triggers", "NormalClimbingTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Normal Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Rope Climbing Trigger
            RopeClimbingTriggerEnabled = Config.Bind("Triggers", "RopeClimbingTriggerEnabled", true, "Enable or Disable Rope Climbing Trigger");
            RopeClimbingTriggerMaximumIntensity = Config.Bind("Triggers", "RopeClimbingTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Rope Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            RopeClimbingTriggerMinimumIntensity = Config.Bind("Triggers", "RopeClimbingTriggerMinimumIntensity", 0.5f,
                new ConfigDescription("Minimum intensity for Rope Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Vine Climbing Trigger
            VineClimbingTriggerEnabled = Config.Bind("Triggers", "VineClimbingTriggerEnabled", true, "Enable or Disable Vine Climbing Trigger");
            VineClimbingTriggerMaximumIntensity = Config.Bind("Triggers", "VineClimbingTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Vine Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            VineClimbingTriggerMinimumIntensity = Config.Bind("Triggers", "VineClimbingTriggerMinimumIntensity", 0.5f,
                new ConfigDescription("Minimum intensity for Vine Climbing Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            // Sprinting Trigger
            SprintingTriggerEnabled = Config.Bind("Triggers", "SprintingTriggerEnabled", true, "Enable or Disable Sprinting Trigger");
            SprintingTriggerMaximumIntensity = Config.Bind("Triggers", "SprintingTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Sprinting Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SprintingTriggerMinimumIntensity = Config.Bind("Triggers", "SprintingTriggerMinimumIntensity", 0.5f,
                new ConfigDescription("Minimum intensity for Sprinting Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));

            // Load status effect trigger settings
            InjuryTriggerEnabled = Config.Bind("Triggers", "InjuryTriggerEnabled", true, "Enable or Disable Injury Trigger");
            InjuryTriggerMaximumIntensity = Config.Bind("Triggers", "InjuryTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Injury Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            InjuryTriggerMinimumIntensity = Config.Bind("Triggers", "InjuryTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Injury Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            InjuryTriggerDuration = Config.Bind("Triggers", "InjuryTriggerDuration", 1000f, 
                new ConfigDescription("Duration for Injury Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            HungerTriggerEnabled = Config.Bind("Triggers", "HungerTriggerEnabled", false, "Enable or Disable Hunger Trigger");
            HungerTriggerMaximumIntensity = Config.Bind("Triggers", "HungerTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Hunger Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            HungerTriggerMinimumIntensity = Config.Bind("Triggers", "HungerTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Hunger Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            HungerTriggerDuration = Config.Bind("Triggers", "HungerTriggerDuration", 1000f,
                new ConfigDescription("Duration for Hunger Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ColdTriggerEnabled = Config.Bind("Triggers", "ColdTriggerEnabled", true, "Enable or Disable Cold Trigger");
            ColdTriggerMaximumIntensity = Config.Bind("Triggers", "ColdTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Cold Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ColdTriggerMinimumIntensity = Config.Bind("Triggers", "ColdTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Cold Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ColdTriggerDuration = Config.Bind("Triggers", "ColdTriggerDuration", 1000f,
                new ConfigDescription("Duration for Cold Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            PoisonTriggerEnabled = Config.Bind("Triggers", "PoisonTriggerEnabled", true, "Enable or Disable Poison Trigger");
            PoisonTriggerMaximumIntensity = Config.Bind("Triggers", "PoisonTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Poison Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PoisonTriggerMinimumIntensity = Config.Bind("Triggers", "PoisonTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Poison Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PoisonTriggerDuration = Config.Bind("Triggers", "PoisonTriggerDuration", 1000f,
                new ConfigDescription("Duration for Poison Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            CurseTriggerEnabled = Config.Bind("Triggers", "CurseTriggerEnabled", true, "Enable or Disable Curse Trigger");
            CurseTriggerMaximumIntensity = Config.Bind("Triggers", "CurseTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Curse Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            CurseTriggerMinimumIntensity = Config.Bind("Triggers", "CurseTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Curse Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            CurseTriggerDuration = Config.Bind("Triggers", "CurseTriggerDuration", 1000f,
                new ConfigDescription("Duration for Curse Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            DrowsyTriggerEnabled = Config.Bind("Triggers", "DrowsyTriggerEnabled", true, "Enable or Disable Drowsy Trigger");
            DrowsyTriggerMaximumIntensity = Config.Bind("Triggers", "DrowsyTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Drowsy Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            DrowsyTriggerMinimumIntensity = Config.Bind("Triggers", "DrowsyTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Drowsy Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            DrowsyTriggerDuration = Config.Bind("Triggers", "DrowsyTriggerDuration", 1000f,
                new ConfigDescription("Duration for Drowsy Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            HotTriggerEnabled = Config.Bind("Triggers", "HotTriggerEnabled", true, "Enable or Disable Hot Trigger");
            HotTriggerMaximumIntensity = Config.Bind("Triggers", "HotTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Hot Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            HotTriggerMinimumIntensity = Config.Bind("Triggers", "HotTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Hot Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            HotTriggerDuration = Config.Bind("Triggers", "HotTriggerDuration", 1000f,
                new ConfigDescription("Duration for Hot Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ThornsTriggerEnabled = Config.Bind("Triggers", "ThornsTriggerEnabled", true, "Enable or Disable Thorns Trigger");
            ThornsTriggerMaximumIntensity = Config.Bind("Triggers", "ThornsTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Thorns Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ThornsTriggerMinimumIntensity = Config.Bind("Triggers", "ThornsTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Thorns Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ThornsTriggerDuration = Config.Bind("Triggers", "ThornsTriggerDuration", 1000f,
                new ConfigDescription("Duration for Thorns Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            SporesTriggerEnabled = Config.Bind("Triggers", "SporesTriggerEnabled", true, "Enable or Disable Spores Trigger");
            SporesTriggerMaximumIntensity = Config.Bind("Triggers", "SporesTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Spores Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SporesTriggerMinimumIntensity = Config.Bind("Triggers", "SporesTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Spores Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            SporesTriggerDuration = Config.Bind("Triggers", "SporesTriggerDuration", 1000f,
                new ConfigDescription("Duration for Spores Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            WebTriggerEnabled = Config.Bind("Triggers", "WebTriggerEnabled", true, "Enable or Disable Web Trigger");
            WebTriggerMaximumIntensity = Config.Bind("Triggers", "WebTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Web Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            WebTriggerMinimumIntensity = Config.Bind("Triggers", "WebTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Web Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            WebTriggerDuration = Config.Bind("Triggers", "WebTriggerDuration", 1000f,
                new ConfigDescription("Duration for Web Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            ArrowTriggerEnabled = Config.Bind("Triggers", "ArrowTriggerEnabled", true, "Enable or Disable Arrow Trigger");
            ArrowTriggerMaximumIntensity = Config.Bind("Triggers", "ArrowTriggerMaximumIntensity", 0.5f, 
                new ConfigDescription("Maximum intensity for Arrow Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ArrowTriggerMinimumIntensity = Config.Bind("Triggers", "ArrowTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Arrow Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            ArrowTriggerDuration = Config.Bind("Triggers", "ArrowTriggerDuration", 1000f,
                new ConfigDescription("Duration for Arrow Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));

            PetrifyTriggerEnabled = Config.Bind("Triggers", "PetrifyTriggerEnabled", true, "Enable or Disable Petrify Trigger");
            PetrifyTriggerMaximumIntensity = Config.Bind("Triggers", "PetrifyTriggerMaximumIntensity", 0.5f,
                new ConfigDescription("Maximum intensity for Petrify Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PetrifyTriggerMinimumIntensity = Config.Bind("Triggers", "PetrifyTriggerMinimumIntensity", 0.1f,
                new ConfigDescription("Minimum intensity for Petrify Trigger from 0.0 to 1.0", new AcceptableValueRange<float>(0.01f, 1)));
            PetrifyTriggerDuration = Config.Bind("Triggers", "PetrifyTriggerDuration", 1000f,
                new ConfigDescription("Duration for Petrify Trigger in milliseconds", new AcceptableValueRange<float>(100f, 10000f)));
        }

        private void EnabledSettingChnaged(object sender, System.EventArgs e)
        {
            if (Enabled.Value)
            {
                _ = ButtplugManager.StartReconnecting(ServerIP.Value, ServerPort.Value);
            }
            else
            {
                _ = ButtplugManager.DisconnectAsync();
            }
        }

        private void OnApplicationQuit()
        {
            if (ButtplugManager == null)
            {
                return;
            }

            Logger.LogInfo(
                "PEAK is closing. Shutting down Intiface..."
            );


            try
            {
                Task shutdownTask =
                    ButtplugManager.ShutdownAsync();

                if (!shutdownTask.Wait(2000))
                {
                    Logger.LogWarning(
                        "Intiface shutdown timed out. Allowing PEAK to close."
                    );
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogWarning(
                    $"Error during Intiface shutdown: {ex.Message}"
                );
            }
        }
    }
}