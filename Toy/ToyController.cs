using System;
using System.Collections.Generic;
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

        private readonly Dictionary<string, double> sourceIntensities = new Dictionary<string, double>();

        public ToyController(ButtplugManager buttplugManager, ManualLogSource logger)
        {
            this.logger = logger;
            this.buttplugManager = buttplugManager;
        }

        public async void SetSourceIntensity(string source, double intensity)
        {
            // Clamp the intensity to the maximum value defined in the plugin configuration
            double maximumIntensity = ConfigManager.MaximumIntensity.Value;
            double clampedIntensity = Math.Max(0.0f, Math.Min(intensity, maximumIntensity));
            // Store the clamped intensity for the given source
            sourceIntensities[source] = clampedIntensity;

            await UpdateVibrationAsync();
        }

        private async Task UpdateVibrationAsync()
        {
            // Check if the ButtplugManager is connected before trying to control devices
            if (!buttplugManager.IsConnected) return;

            double highestIntensity = 0.0;
            string highestSource = "None";

            // Iterate through all source intensities to find the highest one
            foreach (KeyValuePair<string, double> sourceIntensity in sourceIntensities)
            {
                if (sourceIntensity.Value > highestIntensity)
                {
                    highestIntensity = sourceIntensity.Value;
                    highestSource = sourceIntensity.Key;
                }
            }

            if (highestIntensity > 0)
            {
                //logger.LogInfo($"Toy output: {highestIntensity * 100}% from {highestSource}");
            }

            if (highestIntensity <= 0.0)
            {
                await StopVibrationAsync();
                return;
            }

            // Iterate through all connected devices and send the vibration command
            foreach (ButtplugClientDevice device in buttplugManager.Client.Devices)
            {
                // Check if the device supports vibration output before sending the command
                if (!device.HasOutput(OutputType.Vibrate)) continue;
                // Send the vibration command to the device and handle any exceptions that may occur
                try
                {
                    await device.RunOutputAsync(DeviceOutput.Vibrate.Percent(highestIntensity));
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Could not control {device.Name}: {ex.Message}");
                }
            }
        }
        public async Task StopVibrationAsync()
        {
            // Check if the ButtplugManager is connected before trying to stop devices
            if (!buttplugManager.IsConnected) return;
            // Attempt to stop all devices and handle any exceptions that may occur
            try
            {
                await buttplugManager.Client.StopAllDevicesAsync();
			}
            catch (Exception ex)
            {
                logger.LogWarning($"Could not stop devices: {ex.Message}");
            }
		}
    }
}
