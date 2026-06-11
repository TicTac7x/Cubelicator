using Cubelicator.Services;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using Nefarius.ViGEm.Client;

namespace Cubelicator
{
    public class GamecubeAdapter
    {
        public event Action<int, bool> onControllerConnectionChanged = delegate { };

        private const int DEVICE_VID = 0x057E;
        private const int DEVICE_PID = 0x0337;
        private const int ADAPTER_CONNECTED = 0x21;
        private const int ADAPTER_RUMBLE = 0x11;

        private readonly GamecubeController[] controllers = new GamecubeController[4];
        private ViGEmClient? vigemClient;
        private UsbDevice? usbDevice;
        private UsbEndpointReader? usbReader;
        private UsbEndpointWriter? usbWriter;
        private CancellationTokenSource? cts;
        private Task? pollingLoopTask;

        
        public GamecubeAdapter(CalibrationManager calibrationManager)
        {
            initializeAdapter();
            createControllers();

            calibrationManager.onPortControllerCalibrationLoaded += (port, calibration) =>
            {
                setPortControllerCalibration(port, calibration);
            };
        }

        public void start()
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;

            pollingLoopTask = Task.Run(() =>
            {
                byte[] data = new byte[0x25];

                while (!token.IsCancellationRequested)
                {
                    if (usbDevice == null || usbReader == null)
                        break;

                    var result = usbReader.Read(data, 5000, out int len);

                    if (result != ErrorCode.None || len <= 0)
                        continue;

                    if (data[0] != ADAPTER_CONNECTED)
                        continue;

                    for (int port = 0; port < controllers.Length; port++)
                    {
                        controllers[port].setState(decodeAdapterHexDataToControllerState(data, port));
                    }
                }
            }, token);
        }

        private void initializeAdapter()
        {
            var finder = new UsbDeviceFinder(DEVICE_VID, DEVICE_PID);

            usbDevice = UsbDevice.OpenUsbDevice(finder);
            if (usbDevice == null)
            {
                System.Diagnostics.Debug.WriteLine("Adapter device not found.");
                return;

            }

            usbReader = usbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
            usbWriter = usbDevice.OpenEndpointWriter(WriteEndpointID.Ep02);

            // Initialize adapter into streaming mode
            var usbSetupPacket = new UsbSetupPacket(0x21, 11, 0x0001, 0, 0);
            usbDevice.ControlTransfer(ref usbSetupPacket, IntPtr.Zero, 0, out _);
            usbWriter.Write([0x13], 5000, out int _);
        }

        private void createControllers()
        {
            vigemClient = new ViGEmClient();
            for (int i = 0; i < controllers.Length; i++)
            {
                int port = i + 1;
                var controller = new GamecubeController(vigemClient, new GamecubeControllerProfile());

                controller.onRumbleChanged += (bool rumble) =>
                {
                    setControllerRumble(port, rumble);
                };

                controller.onConnectionChanged += (bool connected) =>
                {
                    onControllerConnectionChanged(port, connected);
                };

                controllers[i] = controller;
            }
        }

        private void setControllerRumble(int port, bool rumble)
        {
            byte[] report = [ADAPTER_RUMBLE, 0, 0, 0, 0];
            report[port] = (byte) (rumble ? 1 : 0);
            usbWriter?.Write(report, 1000, out _);
        }

        public async void stop()
        {
            try
            {
                cts?.Cancel();

                if (pollingLoopTask != null)
                    await pollingLoopTask;
            }
            catch { }

            foreach (var controller in controllers)
            {
                controller?.disconnect();
            }
            usbDevice?.Close();

            UsbDevice.Exit();
        }

        private GamecubeControllerState decodeAdapterHexDataToControllerState(byte[] data, int port)
        {
            int offset = 1 + port * 9;
            byte status = data[offset];

            byte b1 = data[offset + 1];
            byte b2 = data[offset + 2];

            return new GamecubeControllerState
            {
                connected = status == 0x10,

                buttonA = (b1 & 1) != 0,
                buttonB = (b1 & 2) != 0,
                buttonX = (b1 & 4) != 0,
                buttonY = (b1 & 8) != 0,

                buttonStart = (b2 & 1) != 0,
                buttonZ = (b2 & 2) != 0,
                buttonRightTrigger = (b2 & 4) != 0,
                buttonLeftTrigger = (b2 & 8) != 0,

                buttonDPadLeft = (b1 & 0x10) != 0,
                buttonDPadRight = (b1 & 0x20) != 0,
                buttonDPadDown = (b1 & 0x40) != 0,
                buttonDPadUp = (b1 & 0x80) != 0,

                stickLeftX = (short)((data[offset + 3] - Constants.gamecubeControllerStickRange)),
                stickLeftY = (short)((data[offset + 4] - Constants.gamecubeControllerStickRange)),
                stickRightX = (short)((data[offset + 5] - Constants.gamecubeControllerStickRange)),
                stickRightY = (short)((data[offset + 6] - Constants.gamecubeControllerStickRange)),

                triggerLeft = data[offset + 7],
                triggerRight = data[offset + 8]
            };
        }

        public void setPortControllerCalibration(int port, GamecubeControllerCalibration calibration)
        {
            controllers[port - 1].setCalibration(calibration);
        }

        public GamecubeController getPortController(int port)
        {
            return controllers[port - 1];
        }
    }
}