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
        private GamecubeControllerCalibration? _calibration;

        public GamecubeController(ViGEmClient vigem, GamecubeControllerProfile profile)
        {
            _profile = profile;
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
            int leftStickX = ApplyDeadzone(state.LeftStickX, _profile.LeftStickDeadzone, Constants.GamecubeControllerStickRange);
            if (_calibration != null)
                leftStickX = CalibrateStick(leftStickX, _calibration.LeftStickXCenter, _calibration.LeftStickXMin, _calibration.LeftStickXMax, Constants.GamecubeControllerStickRange);
            float leftStickXSensitivity = leftStickX * _profile.LeftStickSensitivity;
            short leftStickXFinal = (short) Math.Clamp(leftStickXSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            _controller.SetAxisValue(Xbox360Axis.LeftThumbX, leftStickXFinal);

            // Left Stick Y
            int leftStickY = ApplyDeadzone(state.LeftStickY, _profile.LeftStickDeadzone, Constants.GamecubeControllerStickRange);
            if (_calibration != null)
                leftStickY = CalibrateStick(leftStickY, _calibration.LeftStickYCenter, _calibration.LeftStickYMin, _calibration.LeftStickYMax, Constants.GamecubeControllerStickRange);
            float leftStickYSensitivity = leftStickY * _profile.LeftStickSensitivity;
            short leftStickYFinal = (short)Math.Clamp(
                leftStickYSensitivity * Constants.XboxControllerAxisMultiplier,
                -Constants.XboxControllerAxisRange,
                Constants.XboxControllerAxisRange
            );
            _controller.SetAxisValue(Xbox360Axis.LeftThumbY, leftStickYFinal);

            // Right Stick X
            int rightStickX = ApplyDeadzone(state.RightStickX, _profile.RightStickDeadzone, Constants.GamecubeControllerStickRange);
            if (_calibration != null)
                rightStickX = CalibrateStick(rightStickX, _calibration.RightStickXCenter, _calibration.RightStickXMin, _calibration.RightStickXMax, Constants.GamecubeControllerStickRange);
            float rightStickXSensitivity = rightStickX * _profile.RightStickSensitivity;
            short rightStickXFinal = (short)Math.Clamp(
                rightStickXSensitivity * Constants.XboxControllerAxisMultiplier,
                -Constants.XboxControllerAxisRange,
                Constants.XboxControllerAxisRange
            );
            _controller.SetAxisValue(Xbox360Axis.RightThumbX, rightStickXFinal);

            // Right Stick Y
            int rightStickY = ApplyDeadzone(state.RightStickY, _profile.RightStickDeadzone, Constants.GamecubeControllerStickRange);
            if (_calibration != null)
                rightStickY = CalibrateStick(rightStickY, _calibration.RightStickYCenter, _calibration.RightStickYMin, _calibration.RightStickYMax, Constants.GamecubeControllerStickRange);
            float rightStickYSensitivity = rightStickY * _profile.RightStickSensitivity;
            short rightStickYFinal = (short)Math.Clamp(
                rightStickYSensitivity * Constants.XboxControllerAxisMultiplier,
                -Constants.XboxControllerAxisRange,
                Constants.XboxControllerAxisRange
            );
            _controller.SetAxisValue(Xbox360Axis.RightThumbY, rightStickYFinal);

            // Left Trigger
            int leftTrigger = ApplyDeadzone(state.LeftTrigger, _profile.LeftTriggerDeadzone, Constants.GamecubeControllerTriggerRange);
            if (_calibration != null)
                leftTrigger = CalibrateTrigger(leftTrigger, _calibration.LeftTriggerMin, _calibration.LeftTriggerMax, Constants.GamecubeControllerTriggerRange);
            float leftTriggerSensitivity = leftTrigger * _profile.LeftTriggerSensitivity;
            byte leftTriggerFinal = (byte)Math.Clamp(
                leftTriggerSensitivity,
                0,
                Constants.GamecubeControllerTriggerRange
            );
            _controller.SetSliderValue(_profile.LeftTrigger, leftTriggerFinal);

            // Right Trigger
            int rightTrigger = ApplyDeadzone(state.RightTrigger, _profile.RightTriggerDeadzone, Constants.GamecubeControllerTriggerRange);
            if (_calibration != null)
                rightTrigger = CalibrateTrigger(rightTrigger, _calibration.RightTriggerMin, _calibration.RightTriggerMax, Constants.GamecubeControllerTriggerRange);
            float rightTriggerSensitivity = rightTrigger * _profile.RightTriggerSensitivity;
            byte rightTriggerFinal = (byte)Math.Clamp(
                rightTriggerSensitivity,
                0,
                Constants.GamecubeControllerTriggerRange
            );
            _controller.SetSliderValue(_profile.RightTrigger, rightTriggerFinal);

            if (_connected)
            {
                _controller.SubmitReport();
            }
        }

        public void SetCalibration(GamecubeControllerCalibration calibration)
        {
            _calibration = calibration;
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