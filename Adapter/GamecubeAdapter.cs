using LibUsbDotNet;
using LibUsbDotNet.Main;
using Nefarius.ViGEm.Client;

namespace Cubelicator
{
    public class GamecubeAdapter
    {
        public event Action<AdapterPort, bool> Event_ControllerConnectionChanged = delegate { };
        public event Action<AdapterPort, GamecubeControllerProfile> Event_ControllerProfileChanged = delegate { };

        private const int DEVICE_VID = 0x057E;
        private const int DEVICE_PID = 0x0337;
        private const int ADAPTER_CONNECTED = 0x21;
        private const int ADAPTER_RUMBLE = 0x11;

        private readonly CalibrationManager calibrationManager;
        private readonly ProfileManager profileManager;
        private readonly Settings settings;
        private readonly Dictionary<AdapterPort, GamecubeController> controllers = new Dictionary<AdapterPort, GamecubeController>();
        private ViGEmClient? vigemClient;
        private UsbDevice? usbDevice;
        private UsbEndpointReader? usbReader;
        private UsbEndpointWriter? usbWriter;
        private CancellationTokenSource? cts;
        private Task? pollingLoopTask;

        public GamecubeAdapter(CalibrationManager calibrationManager, Settings settings, ProfileManager profileManager)
        {
            this.calibrationManager = calibrationManager;
            this.profileManager = profileManager;
            this.settings = settings;

            InitializeAdapter();
            CreateControllers();
            SetupListeners();
        }

        private void SetupListeners()
        {
            calibrationManager.Event_PortControllerCalibrationLoaded += (port, calibration) =>
            {
                SetPortControllerCalibration(port, calibration);
            };

            settings.Event_ControllerProfileChanged += (port, profile) =>
            {
                controllers[port].Profile = profile;
            };

            foreach (AdapterPort port in Enum.GetValues<AdapterPort>())
            {
                GetPortController(port).Event_ProfileChanged += (profile) =>
                {
                    Event_ControllerProfileChanged(port, profile);
                };
            }
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

                    var result = usbReader.Read(data, 1000, out int len);

                    if (result != ErrorCode.None || len <= 0)
                        continue;

                    if (data[0] != ADAPTER_CONNECTED)
                        continue;

                    foreach (AdapterPort port in Enum.GetValues<AdapterPort>())
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
            usbWriter.Write([0x13], 1000, out int _);
        }

        private void CreateControllers()
        {
            vigemClient = new ViGEmClient();

            foreach (AdapterPort port in Enum.GetValues<AdapterPort>())
            {
                var controller = new GamecubeController(vigemClient, new GamecubeControllerProfile(), profileManager);

                controller.Event_RumbleChanged += (bool rumble) =>
                {
                    SetControllerRumble(port, rumble);
                };

                controller.Event_ConnectionChanged += (bool connected) =>
                {
                    Event_ControllerConnectionChanged(port, connected);
                };

                controllers[port] = controller;
            }
        }

        private void SetControllerRumble(AdapterPort port, bool rumble)
        {
            byte[] report = [ADAPTER_RUMBLE, 0, 0, 0, 0];

            report[(int) port] = (byte) (rumble ? 1 : 0);

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

            foreach (var controller in controllers.Values)
            {
                controller.Disconnect();
            }
            usbDevice?.Close();

            UsbDevice.Exit();
        }

        private GamecubeControllerState DecodeAdapterHexDataToControllerState(byte[] data, AdapterPort port)
        {
            int offset = 1 + ((int) port - 1) * 9;

            byte b1 = data[offset + 1];
            byte b2 = data[offset + 2];
            bool connected =
                data[offset + 1] != 0 ||
                data[offset + 2] != 0 ||
                data[offset + 3] != 0 ||
                data[offset + 4] != 0 ||
                data[offset + 5] != 0 ||
                data[offset + 6] != 0 ||
                data[offset + 7] != 0 ||
                data[offset + 8] != 0;

            return new GamecubeControllerState
            {
                Connected = connected,

                A = (b1 & 1) != 0,
                B = (b1 & 2) != 0,
                X = (b1 & 4) != 0,
                Y = (b1 & 8) != 0,

                Start = (b2 & 1) != 0,
                Z = (b2 & 2) != 0,
                RightBumper = (b2 & 4) != 0,
                LeftBumper = (b2 & 8) != 0,

                DPadLeft = (b1 & 0x10) != 0,
                DPadRight = (b1 & 0x20) != 0,
                DPadDown = (b1 & 0x40) != 0,
                DPadUp = (b1 & 0x80) != 0,

                LeftStickX = (short)((data[offset + 3] - Constants.GamecubeControllerStickRange)),
                LeftStickY = (short)((data[offset + 4] - Constants.GamecubeControllerStickRange)),
                RightStickX = (short)((data[offset + 5] - Constants.GamecubeControllerStickRange)),
                RightStickY = (short)((data[offset + 6] - Constants.GamecubeControllerStickRange)),

                LeftTrigger = data[offset + 7],
                RightTrigger = data[offset + 8]
            };
        }

        public void SetPortControllerCalibration(AdapterPort port, GamecubeControllerCalibration calibration)
        {
            GetPortController(port).SetCalibration(calibration);
        }

        public GamecubeController GetPortController(AdapterPort port)
        {
            return controllers[port];
        }
    }
}