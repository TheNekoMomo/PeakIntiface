using System;
using System.Threading.Tasks;
using BepInEx.Logging;

using Buttplug.Client;
using Buttplug.Core.Messages;

using PeakIntiface.Buttplug;

namespace PeakIntiface.Toy
{
    public class ToyController
    {
        private readonly ManualLogSource logger;
        private readonly ButtplugManager buttplugManager;

        public ToyController(ButtplugManager buttplugManager, ManualLogSource logger)
        {
            this.logger = logger;
            this.buttplugManager = buttplugManager;
        }

        public async Task StartVibrationAsync(double intensity)
        {
            if (!buttplugManager.IsConnected) return;

            double maximumIntensity = Plugin.MaximumIntensity.Value;
            double clampedIntensity = Math.Max(0.0f, Math.Min(intensity, maximumIntensity));

            foreach (ButtplugClientDevice device in buttplugManager.Client.Devices)
            {
                if (!device.HasOutput(OutputType.Vibrate)) continue;

                try
                {
                    await device.RunOutputAsync(DeviceOutput.Vibrate.Percent(clampedIntensity));
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Could not control {device.Name}: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task StopVibrationAsync()
        {
            if(!buttplugManager.IsConnected) return;

            try
            {
                await buttplugManager.Client.StopAllDevicesAsync();
			}
            catch (Exception ex)
            {
                logger.LogWarning($"Could not stop devices: {ex.Message}");
				throw;
            }
		}
    }
}
