using Cubelicator.Services;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using Nefarius.ViGEm.Client;

namespace Cubelicator
{
    public class GamecubeAdapter
    {
        public event Action<int, bool> OnControllerConnectionChanged = delegate { };

        private const int DEVICE_VID = 0x057E;
        private const int DEVICE_PID = 0x0337;
        private const int ADAPTER_CONNECTED = 0x21;
        private const int ADAPTER_RUMBLE = 0x11;

        private readonly CalibrationManager calibrationManager;
        private readonly Settings settings;
        private readonly GamecubeController[] controllers = new GamecubeController[4];
        private ViGEmClient? vigemClient;
        private UsbDevice? usbDevice;
        private UsbEndpointReader? usbReader;
        private UsbEndpointWriter? usbWriter;
        private CancellationTokenSource? cts;
        private Task? pollingLoopTask;

        public GamecubeAdapter(CalibrationManager calibrationManager, Settings settings)
        {
            this.calibrationManager = calibrationManager;
            this.settings = settings;

            InitializeAdapter();
            CreateControllers();
            SetupListeners();

            
        }

        private void SetupListeners()
        {
            calibrationManager.OnPortControllerCalibrationLoaded += (port, calibration) =>
            {
                SetPortControllerCalibration(port, calibration);
            };

            settings.OnControllerProfileChanged += (port, profile) =>
            {
                switch (port)
                {
                    case 1:
                        controllers[0].Profile = profile;
                        break;
                    case 2:
                        controllers[1].Profile = profile;
                        break;
                    case 3:
                        controllers[2].Profile = profile;
                        break;
                    case 4:
                        controllers[3].Profile = profile;
                        break;
                }
            };
        }

        public void Start()
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
                        controllers[port].SetState(DecodeAdapterHexDataToControllerState(data, port));
                    }
                }
            }, token);
        }

        private void InitializeAdapter()
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

        private void CreateControllers()
        {
            vigemClient = new ViGEmClient();
            for (int i = 0; i < controllers.Length; i++)
            {
                int port = i + 1;
                var controller = new GamecubeController(vigemClient, new GamecubeControllerProfile());

                controller.OnRumbleChanged += (bool rumble) =>
                {
                    SetControllerRumble(port, rumble);
                };

                controller.OnConnectionChanged += (bool connected) =>
                {
                    OnControllerConnectionChanged(port, connected);
                };

                controllers[i] = controller;
            }
        }

        private void SetControllerRumble(int port, bool rumble)
        {
            byte[] report = [ADAPTER_RUMBLE, 0, 0, 0, 0];
            report[port] = (byte) (rumble ? 1 : 0);
            usbWriter?.Write(report, 1000, out _);
        }

        public async void Stop()
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
                controller?.Disconnect();
            }
            usbDevice?.Close();

            UsbDevice.Exit();
        }

        private GamecubeControllerState DecodeAdapterHexDataToControllerState(byte[] data, int port)
        {
            int offset = 1 + port * 9;
            byte status = data[offset];

            byte b1 = data[offset + 1];
            byte b2 = data[offset + 2];

            return new GamecubeControllerState
            {
                Connected = status == 0x10,

                ButtonA = (b1 & 1) != 0,
                ButtonB = (b1 & 2) != 0,
                ButtonX = (b1 & 4) != 0,
                ButtonY = (b1 & 8) != 0,

                ButtonStart = (b2 & 1) != 0,
                ButtonZ = (b2 & 2) != 0,
                ButtonRightShoulder = (b2 & 4) != 0,
                ButtonLeftShoulder = (b2 & 8) != 0,

                ButtonDPadLeft = (b1 & 0x10) != 0,
                ButtonDPadRight = (b1 & 0x20) != 0,
                ButtonDPadDown = (b1 & 0x40) != 0,
                ButtonDPadUp = (b1 & 0x80) != 0,

                StickLeftX = (short)((data[offset + 3] - Constants.GamecubeControllerStickRange)),
                StickLeftY = (short)((data[offset + 4] - Constants.GamecubeControllerStickRange)),
                StickRightX = (short)((data[offset + 5] - Constants.GamecubeControllerStickRange)),
                StickRightY = (short)((data[offset + 6] - Constants.GamecubeControllerStickRange)),

                TriggerLeft = data[offset + 7],
                TriggerRight = data[offset + 8]
            };
        }

        public void SetPortControllerCalibration(int port, GamecubeControllerCalibration calibration)
        {
            controllers[port - 1].SetCalibration(calibration);
        }

        public GamecubeController GetPortController(int port)
        {
            return controllers[port - 1];
        }
    }
}