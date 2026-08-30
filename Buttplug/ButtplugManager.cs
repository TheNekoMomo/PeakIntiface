using System;
using System.Threading.Tasks;
using BepInEx.Logging;
using Buttplug.Client;
using Buttplug.Core.Messages;


namespace PeakIntiface.Buttplug
{
    public class ButtplugManager
    {
        private readonly ManualLogSource logger;

        private ButtplugClient client;
        private bool shuttingDown = false;
        private bool reconnecting = false;

        public bool IsConnected
        {
            get
            {
                return client != null && client.Connected;
            }
        }
        public ButtplugClient Client
        {
            get
            {
                return client;
            }
        }

        public ButtplugManager(ManualLogSource logger)
        {
            this.logger = logger;
        }

        public async Task ConnectAsync(string ip, int port)
        {
            if (IsConnected || shuttingDown) return;

            string address = $"ws://{ip}:{port}";
            logger.LogInfo($"Connecting to {address}...");

            try
            {
                client = new ButtplugClient("PEAK Intiface");

                client.DeviceAdded += OnDeviceAdded;
                client.DeviceRemoved += OnDeviceRemoved;
                client.ScanningFinished += OnScanningFinished;

                ButtplugWebsocketConnector connector =new ButtplugWebsocketConnector(new Uri(address));
                await client.ConnectAsync(connector);

                logger.LogInfo("Connected to Intiface!");

                await StartScanningAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"Could not connect to Intiface: {ex}"
                );

                client = null;
            }
        }
        public async Task DisconnectAsync()
        {
            reconnecting = false;

            if (!IsConnected)
            {
                return;
            }

            try
            {
                try
                {
                    await client
                        .StopScanningAsync()
                        .ConfigureAwait(false);
                }
                catch
                {
                    // Scanner may already be stopped.
                }


                try
                {
                    await client
                        .StopAllDevicesAsync()
                        .ConfigureAwait(false);
                }
                catch
                {
                    // Continue disconnecting even if stopping failed.
                }


                await client
                    .DisconnectAsync()
                    .ConfigureAwait(false);

                logger.LogInfo(
                    "Disconnected from Intiface."
                );
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    $"Error disconnecting from Intiface: {ex.Message}"
                );
            }
            finally
            {
                client = null;
            }
        }
        public async Task StartReconnecting(string ip, int port)
        {
            if (reconnecting) return;

            reconnecting = true;

            while (reconnecting)
            {
                if (shuttingDown) break;
                if (!IsConnected) await ConnectAsync(ip, port);

                await Task.Delay(5000);
            }

            reconnecting = false;
        }

        private void OnDeviceAdded(object sender, DeviceAddedEventArgs args)
        {
            if(shuttingDown) return;
            logger.LogInfo(
                $"Device connected: {args.Device.Name}"
            );
        }
        private void OnDeviceRemoved(object sender, DeviceRemovedEventArgs args)
        {
            if (shuttingDown) return;
            logger.LogInfo(
                $"Device disconnected: {args.Device.Name}"
            );
        }
        private void OnScanningFinished(object sender, EventArgs args)
        {
            logger.LogInfo(
                "Device scanning finished."
            );
        }

        public async Task StartScanningAsync()
        {
            if (shuttingDown) return;
            if (!IsConnected)
            {
                logger.LogWarning("Cannot scan because Intiface is not connected.");
                return;
            }

            try
            {
                logger.LogInfo(
                    "Starting device scan..."
                );

                await client.StartScanningAsync();

                logger.LogInfo(
                    "Device scan started."
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"Could not start device scan: {ex.Message}"
                );
            }
        }
        public async Task StopScanningAsync()
        {
            if (!IsConnected)
            {
                return;
            }


            try
            {
                await client.StopScanningAsync();

                logger.LogInfo(
                    "Device scan stopped."
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"Could not stop device scan: {ex.Message}"
                );
            }
        }
        public async Task ShutdownAsync()
        {
            if (shuttingDown)
            {
                return;
            }

            shuttingDown = true;

            logger.LogInfo("Shutting down Intiface connection...");

            if (client == null)
            {
                return;
            }

            try
            {
                if (client.Connected)
                {
                    try
                    {
                        logger.LogInfo("Stopping device scan...");

                        await client
                            .StopScanningAsync()
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(
                            $"Could not stop scanning: {ex.Message}"
                        );
                    }


                    try
                    {
                        logger.LogInfo("Stopping all devices...");

                        await client
                            .StopAllDevicesAsync()
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(
                            $"Could not stop devices: {ex.Message}"
                        );
                    }


                    try
                    {
                        logger.LogInfo("Disconnecting from Intiface...");

                        await client
                            .DisconnectAsync()
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(
                            $"Could not disconnect cleanly: {ex.Message}"
                        );
                    }
                }
            }
            finally
            {
                client = null;

                logger.LogInfo(
                    "Intiface shutdown complete."
                );
            }
        }
    }
}