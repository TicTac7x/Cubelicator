using LibUsbDotNet;
using LibUsbDotNet.Main;
using Nefarius.ViGEm.Client;

namespace ConsoleApp1
{
    internal class Adapter
    {
        private const int DEVICE_VID = 0x057E;
        private const int DEVICE_PID = 0x0337;
        public static readonly int STICK_RANGE = 128;

        private GamecubeController[] _controllers = new GamecubeController[4];

        private ViGEmClient? _vigemClient;
        private UsbDevice? _device;
        private UsbEndpointReader? _reader;
        private UsbEndpointWriter? _writer;
        private CancellationTokenSource? _cts;
        private Task? _loopTask;

        public Adapter()
        {
            InitializeAdapter();
            CreateControllers();
            StartPolling();
        }

        private void InitializeAdapter()
        {
            var finder = new UsbDeviceFinder(DEVICE_VID, DEVICE_PID);

            _device = UsbDevice.OpenUsbDevice(finder);
            if (_device == null)
            {
                System.Diagnostics.Debug.WriteLine("Adapter device not found.");
                return;

            }

            _reader = _device.OpenEndpointReader(ReadEndpointID.Ep01);
            _writer = _device.OpenEndpointWriter(WriteEndpointID.Ep02);

            // Initialize adapter into streaming mode
            var usbSetupPacket = new UsbSetupPacket(0x21, 11, 0x0001, 0, 0);
            _device.ControlTransfer(ref usbSetupPacket, IntPtr.Zero, 0, out _);
            _writer.Write([0x13], 5000, out int _);
        }

        private void CreateControllers()
        {
            _vigemClient = new ViGEmClient();
            for (int i = 0; i < _controllers.Length; i++)
            {
                _controllers[i] = new GamecubeController(_vigemClient, new GamecubeControllerProfile());
            }
        }

        private void StartPolling()
        {
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _loopTask = Task.Run(() =>
            {
                byte[] data = new byte[0x25];

                while (!token.IsCancellationRequested)
                {
                    if (_device == null || _reader == null)
                        break;

                    var result = _reader.Read(data, 5000000, out int len);

                    if (result != ErrorCode.None || len <= 0)
                        continue;

                    if (data[0] != 0x21)
                        continue;

                    for (int port = 0; port < _controllers.Length; port++)
                    {
                        _controllers[port].SetState(DecodeAdapterHexDataToControllerState(data, port));
                    }
                }
            }, token);
        }

        public async void Stop()
        {
            try
            {
                _cts?.Cancel();

                if (_loopTask != null)
                    await _loopTask;
            }
            catch { }

            foreach (var controller in _controllers)
            {
                controller?.Disconnect();
            }
            _device?.Close();

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

                A = (b1 & 1) != 0,
                B = (b1 & 2) != 0,
                X = (b1 & 4) != 0,
                Y = (b1 & 8) != 0,

                Start = (b2 & 1) != 0,
                Z = (b2 & 2) != 0,
                R = (b2 & 4) != 0,
                L = (b2 & 8) != 0,

                DpadLeft = (b1 & 0x10) != 0,
                DpadRight = (b1 & 0x20) != 0,
                DpadDown = (b1 & 0x40) != 0,
                DpadUp = (b1 & 0x80) != 0,

                LeftStickX = (short)((data[offset + 3] - STICK_RANGE)),
                LeftStickY = (short)((data[offset + 4] - STICK_RANGE)),
                RightStickX = (short)((data[offset + 5] - STICK_RANGE)),
                RightStickY = (short)((data[offset + 6] - STICK_RANGE)),

                LeftTrigger = data[offset + 7],
                RightTrigger = data[offset + 8]
            };
        }
    }
}