using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace CubeGem
{
    internal class GamecubeController
    {
        private readonly IXbox360Controller _controller;
        private bool _connected = false;
        private readonly GamecubeControllerProfile _profile;
        private readonly GamecubeControllerCalibration _calibration;

        public GamecubeController(ViGEmClient client, GamecubeControllerProfile profile, GamecubeControllerCalibration calibration)
        {
            _profile = profile;
            _calibration = calibration;
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
            _controller.SetButtonState(_profile.LeftTriggerButton, state.LeftTriggerButton);
            _controller.SetButtonState(_profile.RightTriggerButton, state.RightTriggerButton);

            // Dpad
            _controller.SetButtonState(_profile.DPadUp, state.DpadUp);
            _controller.SetButtonState(_profile.DPadDown, state.DpadDown);
            _controller.SetButtonState(_profile.DPadLeft, state.DpadLeft);
            _controller.SetButtonState(_profile.DPadRight, state.DpadRight);

            // Sticks
            var leftStickX = ApplyDeadzone(state.LeftStickX, _profile.LeftStickDeadzone, Constants.GAMECUBE_CONTROLLER_STICK_RANGE);
            leftStickX = CalibrateStick(leftStickX, _calibration.LeftStickXCenter, _calibration.LeftStickXMin, _calibration.LeftStickXMax, Constants.GAMECUBE_CONTROLLER_STICK_RANGE);
            leftStickX *= (int) _profile.LeftStickSensitivity;
            _controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short)Math.Clamp((leftStickX * Constants.XBOX_CONTROLLER_AXIS_MULTIPLIER), -Constants.XBOX_CONTROLLER_AXIS_RANGE, Constants.XBOX_CONTROLLER_AXIS_RANGE));

            var leftStickY = ApplyDeadzone(state.LeftStickY, _profile.LeftStickDeadzone, Constants.GAMECUBE_CONTROLLER_STICK_RANGE);
            leftStickY = CalibrateStick(leftStickY, _calibration.LeftStickYCenter, _calibration.LeftStickYMin, _calibration.LeftStickYMax, Constants.GAMECUBE_CONTROLLER_STICK_RANGE);
            leftStickY *= (int)_profile.LeftStickSensitivity;
            _controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short)Math.Clamp((leftStickY * Constants.XBOX_CONTROLLER_AXIS_MULTIPLIER), -Constants.XBOX_CONTROLLER_AXIS_RANGE, Constants.XBOX_CONTROLLER_AXIS_RANGE));

            var rightStickX = ApplyDeadzone(state.RightStickX, _profile.RightStickDeadzone, Constants.GAMECUBE_CONTROLLER_STICK_RANGE);
            rightStickX = CalibrateStick(rightStickX, _calibration.RightStickXCenter, _calibration.RightStickXMin, _calibration.RightStickXMax, Constants.GAMECUBE_CONTROLLER_STICK_RANGE);
            rightStickX *= (int)_profile.RightStickSensitivity;
            _controller.SetAxisValue(Xbox360Axis.RightThumbX, (short)Math.Clamp((rightStickX * Constants.XBOX_CONTROLLER_AXIS_MULTIPLIER), -Constants.XBOX_CONTROLLER_AXIS_RANGE, Constants.XBOX_CONTROLLER_AXIS_RANGE));

            var rightStickY = ApplyDeadzone(state.RightStickY, _profile.RightStickDeadzone, Constants.GAMECUBE_CONTROLLER_STICK_RANGE);
            rightStickY = CalibrateStick(rightStickY, _calibration.RightStickYCenter, _calibration.RightStickYMin, _calibration.RightStickYMax, Constants.GAMECUBE_CONTROLLER_STICK_RANGE);
            rightStickY *= (int)_profile.RightStickSensitivity;
            _controller.SetAxisValue(Xbox360Axis.RightThumbY, (short)Math.Clamp((rightStickY * Constants.XBOX_CONTROLLER_AXIS_MULTIPLIER), -Constants.XBOX_CONTROLLER_AXIS_RANGE, Constants.XBOX_CONTROLLER_AXIS_RANGE));

            // Triggers
            var leftTrigger = ApplyDeadzone(state.LeftTrigger, _profile.LeftTriggerDeadzone, Constants.GAMECUBE_CONTROLLER_TRIGGER_RANGE);
            leftTrigger = CalibrateTrigger(leftTrigger, _calibration.LeftTriggerMin, _calibration.LeftTriggerMax, Constants.GAMECUBE_CONTROLLER_TRIGGER_RANGE);
            leftTrigger *= (int)_profile.LeftTriggerSensitivity;
            _controller.SetSliderValue(_profile.LeftTrigger, (byte) Math.Clamp(leftTrigger, 0, Constants.GAMECUBE_CONTROLLER_TRIGGER_RANGE));

            var rightTrigger = ApplyDeadzone(state.RightTrigger, _profile.RightTriggerDeadzone, Constants.GAMECUBE_CONTROLLER_TRIGGER_RANGE);
            rightTrigger = CalibrateTrigger(rightTrigger, _calibration.RightTriggerMin, _calibration.RightTriggerMax, Constants.GAMECUBE_CONTROLLER_TRIGGER_RANGE);
            rightTrigger *= (int)_profile.RightTriggerSensitivity;
            _controller.SetSliderValue(_profile.RightTrigger, (byte) Math.Clamp(rightTrigger, 0, Constants.GAMECUBE_CONTROLLER_TRIGGER_RANGE));


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

        private int CalibrateStick(int value, int resting, int min, int max, int targetRange)
        {
            int centered = value - resting;

            if (centered >= 0)
            {
                return (int)(centered / (float)(max - resting) * targetRange);
            }
            else
            {
                return (int)(centered / (float)(resting - min) * targetRange);
            }
        }

        private int CalibrateTrigger(int value, int min, int max, int range)
        {
            value = Math.Clamp(value, min, max);
            return (int)((value - min) / (float)(max - min) * range);
        }
    }
}