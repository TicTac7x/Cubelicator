using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace ConsoleApp1
{
    internal class GamecubeController
    {
        private readonly IXbox360Controller _controller;
        private bool _connected = false;
        private readonly GamecubeControllerProfile _profile;

        private const int XBOX_AXIS_MULTIPLIER = 256;
        private const int GAMECUBE_CONTROLLER_TRIGGER_RANGE = 256;

        public GamecubeController(ViGEmClient client, GamecubeControllerProfile profile)
        {
            _profile = profile;
            _controller = client.CreateXbox360Controller();
            _controller.AutoSubmitReport = false;
        }

        public void Disconnect()
        {
            _controller.Disconnect();
            _connected = false;
        }

        public void SetState(GamecubeControllerState state)
        {
            if (_connected != state.Connected)
            {
                _connected = state.Connected;

                if (_connected)
                    _controller.Connect();
                else
                    _controller.Disconnect();
            }

            if (!state.Connected)
            {
                return;
            }

            // Buttons
            _controller.SetButtonState(_profile.A, state.A);
            _controller.SetButtonState(_profile.B, state.B);
            _controller.SetButtonState(_profile.X, state.X);
            _controller.SetButtonState(_profile.Y, state.Y);
            _controller.SetButtonState(_profile.Z, state.Z);
            _controller.SetButtonState(_profile.Start, state.Start);

            // Dpad
            _controller.SetButtonState(_profile.DPadUp, state.DpadUp);
            _controller.SetButtonState(_profile.DPadDown, state.DpadDown);
            _controller.SetButtonState(_profile.DPadLeft, state.DpadLeft);
            _controller.SetButtonState(_profile.DPadRight, state.DpadRight);

            // Sticks
            _controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short) (ApplyDeadzone(state.LeftStickX, _profile.LeftStickDeadzone, Adapter.STICK_RANGE) * XBOX_AXIS_MULTIPLIER));
            _controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short) (ApplyDeadzone(state.LeftStickY, _profile.LeftStickDeadzone, Adapter.STICK_RANGE) * XBOX_AXIS_MULTIPLIER));
            _controller.SetAxisValue(Xbox360Axis.RightThumbX, (short) (ApplyDeadzone(state.RightStickX, _profile.RightStickDeadzone, Adapter.STICK_RANGE) * XBOX_AXIS_MULTIPLIER));
            _controller.SetAxisValue(Xbox360Axis.RightThumbY, (short) (ApplyDeadzone(state.RightStickY, _profile.RightStickDeadzone, Adapter.STICK_RANGE) * XBOX_AXIS_MULTIPLIER));

            App.DebugOutput(state.LeftTrigger);

            // Triggers
            _controller.SetSliderValue(_profile.LeftTrigger, (byte) ApplyDeadzone(state.LeftTrigger, _profile.LeftTriggerDeadzone, GAMECUBE_CONTROLLER_TRIGGER_RANGE));
            _controller.SetSliderValue(_profile.RightTrigger, (byte) ApplyDeadzone(state.RightTrigger, _profile.RightTriggerDeadzone, GAMECUBE_CONTROLLER_TRIGGER_RANGE));
            _controller.SetButtonState(_profile.LeftTriggerButton, state.L);
            _controller.SetButtonState(_profile.RightTriggerButton, state.R);

            if (_connected)
            {
                _controller.SubmitReport();
            }
        }

        private int ApplyDeadzone(int value, double deadzone, int range)
        {
            if (Math.Abs(value) < deadzone * range)
            {
                return 0;
            }

            return value;
        }
    }
}