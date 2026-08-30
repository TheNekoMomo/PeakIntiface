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
        public const string PluginVersion = "0.0.1";

        public static ConfigEntry<bool> Enabled;
        public static ConfigEntry<string> ServerIP;
        public static ConfigEntry<int> ServerPort;
        public static ConfigEntry<float> MaximumIntensity;

        public static ButtplugManager ButtplugManager;
        public static ToyController ToyController;
        public static StaminaTrigger StaminaTrigger;

        private void Awake()
        {
            Logger.LogInfo($"{PluginName} v{PluginVersion} loading...");

            LoadConfig();

            ButtplugManager = new ButtplugManager(Logger);
            if (Enabled.Value) _ = ButtplugManager.StartReconnecting(ServerIP.Value, ServerPort.Value);
            ToyController = new ToyController(ButtplugManager, Logger);

            StaminaTrigger = new StaminaTrigger(Logger);

            Logger.LogInfo($"Intiface Address: {ServerIP.Value}:{ServerPort.Value}");
            Logger.LogInfo($"{PluginName} Loaded!");
        }

        private void Update()
        {
            StaminaTrigger?.Update();
        }

        private void LoadConfig()
        {
            Enabled = Config.Bind("General", "Enabled", true, "Enable or Disable toy control");
            MaximumIntensity = Config.Bind("General", "MaximumIntensity", 0.5f, 
                new ConfigDescription("Maximumtoy intensity from 0.0 to 1.0", new AcceptableValueRange<float>(0, 1)));
            ServerIP = Config.Bind("Connection", "ServerIP", "127.0.0.1", "IP address of the Intiface Server");
            ServerPort = Config.Bind("Connection", "ServerPort", 12345, "Port of the Intiface Server");
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