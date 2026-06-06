using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Exceptions;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace ConsoleApp1
{
    internal class GamecubeController
    {
        private static readonly int XBOX_AXIS_MULTIPLIER = 256;

        private readonly IXbox360Controller _controller;
        private bool _connected = false;

        public GamecubeController(ViGEmClient client)
        {
            _controller = client.CreateXbox360Controller();
            _controller.AutoSubmitReport = false;
        }

        public void SetState(GamecubeControllerState state)
        {
            // Check if virtual controller needs to connect or disconnect.
            if (_connected != state.Connected)
            {
                _connected = state.Connected;
                if (state.Connected)
                {
                    _controller.Connect();
                } else
                {
                    _controller.Disconnect();
                }
            }

            if (!state.Connected) return;
            App.DebugOutput(state.Z);

            _controller.SetButtonState(Xbox360Button.A, state.A);
            _controller.SetButtonState(Xbox360Button.B, state.B);
            _controller.SetButtonState(Xbox360Button.X, state.X);
            _controller.SetButtonState(Xbox360Button.Y, state.Y);

            _controller.SetButtonState(Xbox360Button.Start, state.Start);
            _controller.SetButtonState(Xbox360Button.Back, state.Z);
            _controller.SetButtonState(Xbox360Button.LeftShoulder, state.L);
            _controller.SetButtonState(Xbox360Button.RightShoulder, state.R);

            _controller.SetButtonState(Xbox360Button.Up, state.DpadUp);
            _controller.SetButtonState(Xbox360Button.Down, state.DpadDown);
            _controller.SetButtonState(Xbox360Button.Left, state.DpadLeft);
            _controller.SetButtonState(Xbox360Button.Right, state.DpadRight);

            _controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short) (state.LeftStickX * XBOX_AXIS_MULTIPLIER));
            _controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short) (state.LeftStickY * XBOX_AXIS_MULTIPLIER));
            _controller.SetAxisValue(Xbox360Axis.RightThumbX, (short) (state.RightStickX * XBOX_AXIS_MULTIPLIER));
            _controller.SetAxisValue(Xbox360Axis.RightThumbY, (short) (state.RightStickY * XBOX_AXIS_MULTIPLIER));

            _controller.SetSliderValue(Xbox360Slider.LeftTrigger, state.TriggerLeft);
            _controller.SetSliderValue(Xbox360Slider.RightTrigger, state.TriggerRight);

            if (_connected)
            {
                _controller.SubmitReport();
            }
        }

        public void Disconnect()
        {
            _controller.Disconnect();
        }
    }
}