using System;
using System.Threading.Tasks;
using BepInEx.Logging;
using Buttplug.Client;

namespace PeakIntiface.Buttplug
{
    public class ButtplugManager
    {
        private readonly ManualLogSource logger;

        private ButtplugClient client;
        private bool reconnecting = false;

        public bool IsConnected { get { return client != null && client.Connected; } }
        public ButtplugClient Client { get { return client; } }

        public ButtplugManager(ManualLogSource logger)
        {
            this.logger = logger;
        }

        public async Task StartReconnecting(string ip, int port)
        {
            if (reconnecting) return;
            reconnecting = true;

            while (reconnecting)
            {
                if (!IsConnected)
                {
                    logger.LogInfo("Retrying reconnecting");
                    await ConnectAsync(ip, port);
                }

                await Task.Delay(5000);
            }
            reconnecting = false;
        }
        private async Task ConnectAsync(string ip, int port)
        {
            if (IsConnected) return;

            string address = $"ws://{ip}:{port}";
            logger.LogInfo($"Connecting to {address}...");

            try
            {
                client = new ButtplugClient("PEAK Intiface");

                client.DeviceAdded += OnDeviceAdded;
                client.DeviceRemoved += OnDeviceRemoved;

                ButtplugWebsocketConnector connector =new ButtplugWebsocketConnector(new Uri(address));
                await client.ConnectAsync(connector);

                logger.LogInfo("Connected to Intiface!");

                await client.StartScanningAsync();
            }
            catch
            {
                logger.LogError($"Could not connect to Intiface");
                client = null;
            }
        }
        public async Task DisconnectAsync()
        {
            reconnecting = false;

            if (!IsConnected) return;

            try
            {
                try
                {
                    await client.StopScanningAsync().ConfigureAwait(false);
                }
                catch { }

                try
                {
                    await client.StopAllDevicesAsync().ConfigureAwait(false);
                }
                catch { }

                await client.DisconnectAsync().ConfigureAwait(false);

                logger.LogInfo("Disconnected from Intiface.");
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Error disconnecting from Intiface: {ex.Message}");
            }
            finally
            {
                client = null;
            }
        }

        private void OnDeviceAdded(object sender, DeviceAddedEventArgs args)
        {
            logger.LogInfo($"Device connected: {args.Device.Name}");
        }
        private void OnDeviceRemoved(object sender, DeviceRemovedEventArgs args)
        {
            logger.LogInfo($"Device disconnected: {args.Device.Name}");
        }
    }
}