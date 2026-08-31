using System.Threading.Tasks;
using BepInEx;
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
        public const string PluginVersion = "0.1.1";

        public static ButtplugManager ButtplugManager;
        public static ToyController ToyController;

        public static MovementTrigger MovementTrigger;
        public static StatustEffectTrigger StatustEffectTrigger;

        private void Awake()
        {
            Logger.LogInfo($"{PluginName} v{PluginVersion} loading...");

            _ = new ConfigManager(Config);

            ButtplugManager = new ButtplugManager(Logger);
            if (ConfigManager.Enabled.Value) _ = ButtplugManager.StartReconnecting(ConfigManager.ServerIP.Value, ConfigManager.ServerPort.Value);
            ToyController = new ToyController(ButtplugManager, Logger);

            MovementTrigger = new MovementTrigger(Logger);
            StatustEffectTrigger = new StatustEffectTrigger(Logger);

            Logger.LogInfo($"Intiface Address: {ConfigManager.ServerIP.Value}:{ConfigManager.ServerPort.Value}");
            Logger.LogInfo($"{PluginName} Loaded!");
        }

        private void Update()
        {
            // Run the Trigger updates only if the plugin is enabled, the ToyController is initialized, and the ButtplugManager is connected
            if (!ConfigManager.Enabled.Value || ToyController == null || !ButtplugManager.IsConnected) return;
            MovementTrigger?.Update();
            StatustEffectTrigger?.Update();
        }

        private void OnApplicationQuit()
        {
            if (ButtplugManager == null) return;

            Logger.LogInfo("PEAK is closing. Shutting down Intiface...");

            try
            {
                Task shutdownTask = ButtplugManager.ShutdownAsync();

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