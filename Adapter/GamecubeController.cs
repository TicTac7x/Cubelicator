using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace Cubelicator
{
    internal class GamecubeController
    {
        public event Action<bool> OnConnectionChanged = delegate { };
        public event Action<bool> OnRumbleChanged = delegate { };

        private readonly IXbox360Controller _controller;
        private bool _connected = false;
        private readonly GamecubeControllerProfile _profile;
        private readonly GamecubeControllerCalibration _calibration;

        public GamecubeController(ViGEmClient vigem, GamecubeControllerProfile profile, GamecubeControllerCalibration calibration)
        {
            _profile = profile;
            _calibration = calibration;
            _controller = CreateController(vigem);
            _controller.AutoSubmitReport = false;
        }

        private IXbox360Controller CreateController(ViGEmClient vigem)
        {
            var controller = vigem.CreateXbox360Controller();
            controller.FeedbackReceived += (sender, args) =>
            {
                OnRumbleChanged.Invoke(args.LargeMotor > 0 || args.SmallMotor > 0);
            };

            return controller;
        }

        public void Disconnect()
        {
            _controller.Disconnect();
            _connected = false;
        }

        public bool IsConnected()
        {
            return _connected;
        }

        public void SetState(GamecubeControllerState state)
        {
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
                OnConnectionChanged.Invoke(state.Connected);
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

            // Left Stick X
            int leftStickXRaw = state.LeftStickX;
            int leftStickXDeadzoned = ApplyDeadzone(leftStickXRaw, _profile.LeftStickDeadzone, Constants.GamecubeControllerStickRange);
            int leftStickXCalibrated = CalibrateStick(leftStickXDeadzoned, _calibration.LeftStickXCenter, _calibration.LeftStickXMin, _calibration.LeftStickXMax, Constants.GamecubeControllerStickRange);
            float leftStickXSensitivity = leftStickXCalibrated * _profile.LeftStickSensitivity;
            short leftStickXFinal = (short) Math.Clamp(leftStickXSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            _controller.SetAxisValue(Xbox360Axis.LeftThumbX, leftStickXFinal);

            // Left Stick Y
            int leftStickYRaw = state.LeftStickY;
            int leftStickYDeadzoned = ApplyDeadzone(leftStickYRaw, _profile.LeftStickDeadzone, Constants.GamecubeControllerStickRange);
            int leftStickYCalibrated = CalibrateStick(leftStickYDeadzoned, _calibration.LeftStickYCenter, _calibration.LeftStickYMin, _calibration.LeftStickYMax, Constants.GamecubeControllerStickRange);
            float leftStickYSensitivity = leftStickYCalibrated * _profile.LeftStickSensitivity;
            short leftStickYFinal = (short) Math.Clamp(leftStickYSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            _controller.SetAxisValue(Xbox360Axis.LeftThumbY, leftStickYFinal);

            // Right Stick X
            int rightStickXRaw = state.RightStickX;
            int rightStickXDeadzoned = ApplyDeadzone(rightStickXRaw, _profile.RightStickDeadzone, Constants.GamecubeControllerStickRange);
            int rightStickXCalibrated = CalibrateStick(rightStickXDeadzoned, _calibration.RightStickXCenter, _calibration.RightStickXMin, _calibration.RightStickXMax, Constants.GamecubeControllerStickRange);
            float rightStickXSensitivity = rightStickXCalibrated * _profile.RightStickSensitivity;
            short rightStickXFinal = (short) Math.Clamp(rightStickXSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            _controller.SetAxisValue(Xbox360Axis.RightThumbX, rightStickXFinal);

            // Right Stick Y
            int rightStickYRaw = state.RightStickY;
            int rightStickYDeadzoned = ApplyDeadzone(rightStickYRaw, _profile.RightStickDeadzone, Constants.GamecubeControllerStickRange);
            int rightStickYCalibrated = CalibrateStick(rightStickYDeadzoned, _calibration.RightStickYCenter, _calibration.RightStickYMin, _calibration.RightStickYMax, Constants.GamecubeControllerStickRange);
            float rightStickYSensitivity = rightStickYCalibrated * _profile.RightStickSensitivity;
            short rightStickYFinal = (short) Math.Clamp(rightStickYSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            _controller.SetAxisValue(Xbox360Axis.RightThumbY, rightStickYFinal);

            // Left Trigger
            int leftTriggerRaw = state.LeftTrigger;
            int leftTriggerDeadzoned = ApplyDeadzone(leftTriggerRaw, _profile.LeftTriggerDeadzone, Constants.GamecubeControllerTriggerRange);
            int leftTriggerCalibrated = CalibrateTrigger(leftTriggerDeadzoned, _calibration.LeftTriggerMin, _calibration.LeftTriggerMax, Constants.GamecubeControllerTriggerRange);
            float leftTriggerSensitivity = leftTriggerCalibrated * _profile.LeftTriggerSensitivity;
            byte leftTriggerFinal = (byte) Math.Clamp(leftTriggerSensitivity, 0, Constants.GamecubeControllerTriggerRange);
            _controller.SetSliderValue(_profile.LeftTrigger, leftTriggerFinal);

            // Right Trigger
            int rightTriggerRaw = state.RightTrigger;
            int rightTriggerDeadzoned = ApplyDeadzone(rightTriggerRaw, _profile.RightTriggerDeadzone, Constants.GamecubeControllerTriggerRange);
            int rightTriggerCalibrated = CalibrateTrigger(rightTriggerDeadzoned, _calibration.RightTriggerMin, _calibration.RightTriggerMax, Constants.GamecubeControllerTriggerRange);
            float rightTriggerSensitivity = rightTriggerCalibrated * _profile.RightTriggerSensitivity;
            byte rightTriggerFinal = (byte) Math.Clamp(rightTriggerSensitivity, 0, Constants.GamecubeControllerTriggerRange);
            _controller.SetSliderValue(_profile.RightTrigger, rightTriggerFinal);


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