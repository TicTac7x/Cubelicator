using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace Cubelicator
{
    public class GamecubeController
    {
        public event Action<bool> OnConnectionChanged = delegate { };
        public event Action<bool> OnRumbleChanged = delegate { };
        public event Action<GamecubeControllerCalibration> OnCalibrationChanged = delegate { };

        private readonly IXbox360Controller controller;
        private bool connected = false;
        private readonly GamecubeControllerProfile profile;
        private GamecubeControllerCalibration? calibration = null;

        public GamecubeController(ViGEmClient vigem, GamecubeControllerProfile profile)
        {
            this.profile = profile;
            controller = CreateController(vigem);
            controller.AutoSubmitReport = false;
        }

        private IXbox360Controller CreateController(ViGEmClient vigem)
        {
            var controller = vigem.CreateXbox360Controller();
            controller.FeedbackReceived += (_, args) =>
            {
                OnRumbleChanged.Invoke(args.LargeMotor > 0 || args.SmallMotor > 0);
            };

            return controller;
        }

        public void Disconnect()
        {
            controller.Disconnect();
            connected = false;
        }

        public bool IsConnected()
        {
            return connected;
        }

        public bool IsCalibrated()
        {
            return calibration != null;
        }

        public void SetState(GamecubeControllerState state)
        {
            if (connected != state.Connected)
            {
                connected = state.Connected;
                if (state.Connected)
                {
                    controller.Connect();
                } else
                {
                    controller.Disconnect();
                }
                OnConnectionChanged.Invoke(state.Connected);
            }

            if (!state.Connected)
            {
                return;
            }

            // Buttons
            controller.SetButtonState(profile.A, state.ButtonA);
            controller.SetButtonState(profile.B, state.ButtonB);
            controller.SetButtonState(profile.X, state.ButtonX);
            controller.SetButtonState(profile.Y, state.ButtonY);
            controller.SetButtonState(profile.Z, state.ButtonZ);
            controller.SetButtonState(profile.Start, state.ButtonStart);
            controller.SetButtonState(profile.LeftTriggerButton, state.ButtonLeftShoulder);
            controller.SetButtonState(profile.RightTriggerButton, state.ButtonRightShoulder);

            // Dpad
            controller.SetButtonState(profile.DPadUp, state.ButtonDPadUp);
            controller.SetButtonState(profile.DPadDown, state.ButtonDPadDown);
            controller.SetButtonState(profile.DPadLeft, state.ButtonDPadLeft);
            controller.SetButtonState(profile.DPadRight, state.ButtonDPadRight);

            // Left Stick X
            int leftStickXDeadzone = ApplyDeadzone(state.StickLeftX, profile.LeftStickDeadzone, Constants.GamecubeControllerStickRange);
            int leftStickXCalibrate = (calibration != null) ? CalibrateStick(leftStickXDeadzone, calibration.LeftStickXCenter, calibration.LeftStickXMin, calibration.LeftStickXMax, Constants.GamecubeControllerStickRange) : leftStickXDeadzone;
            float leftStickXSensitivity = leftStickXCalibrate * profile.LeftStickSensitivity;
            short leftStickXFinal = (short)Math.Clamp(leftStickXSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, leftStickXFinal);

            // Left Stick Y
            int leftStickYDeadzone = ApplyDeadzone(state.StickLeftY, profile.LeftStickDeadzone, Constants.GamecubeControllerStickRange);
            int leftStickYCalibrate = (calibration != null) ? CalibrateStick(leftStickYDeadzone, calibration.LeftStickYCenter, calibration.LeftStickYMin, calibration.LeftStickYMax, Constants.GamecubeControllerStickRange) : leftStickYDeadzone;
            float leftStickYSensitivity = leftStickYCalibrate * profile.LeftStickSensitivity;
            short leftStickYFinal = (short)Math.Clamp(leftStickYSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, leftStickYFinal);

            // Right Stick X
            int rightStickXDeadzone = ApplyDeadzone(state.StickRightX, profile.RightStickDeadzone, Constants.GamecubeControllerStickRange);
            int rightStickXCalibrate = (calibration != null) ? CalibrateStick(rightStickXDeadzone, calibration.RightStickXCenter, calibration.RightStickXMin, calibration.RightStickXMax, Constants.GamecubeControllerStickRange) : rightStickXDeadzone;
            float rightStickXSensitivity = rightStickXCalibrate * profile.RightStickSensitivity;
            short rightStickXFinal = (short)Math.Clamp(rightStickXSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            controller.SetAxisValue(Xbox360Axis.RightThumbX, rightStickXFinal);

            // Right Stick Y
            int rightStickYDeadzone = ApplyDeadzone(state.StickRightY, profile.RightStickDeadzone, Constants.GamecubeControllerStickRange);
            int rightStickYCalibrate = (calibration != null) ? CalibrateStick(rightStickYDeadzone, calibration.RightStickYCenter, calibration.RightStickYMin, calibration.RightStickYMax, Constants.GamecubeControllerStickRange) : rightStickYDeadzone;
            float rightStickYSensitivity = rightStickYCalibrate * profile.RightStickSensitivity;
            short rightStickYFinal = (short)Math.Clamp(rightStickYSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
            controller.SetAxisValue(Xbox360Axis.RightThumbY, rightStickYFinal);

            // Left Trigger
            int leftTriggerDeadzone = ApplyDeadzone(state.TriggerLeft, profile.LeftTriggerDeadzone, Constants.GamecubeControllerTriggerRange);
            int leftTriggerCalibrate = (calibration != null) ? CalibrateTrigger(leftTriggerDeadzone, calibration.LeftTriggerMin, calibration.LeftTriggerMax, Constants.GamecubeControllerTriggerRange) : leftTriggerDeadzone;
            float leftTriggerSensitivity = leftTriggerCalibrate * profile.LeftTriggerSensitivity;
            byte leftTriggerFinal = (byte)Math.Clamp(leftTriggerSensitivity, 0, Constants.GamecubeControllerTriggerRange);
            controller.SetSliderValue(profile.LeftTrigger, leftTriggerFinal);

            // Right Trigger
            int rightTriggerDeadzone = ApplyDeadzone(state.TriggerRight, profile.RightTriggerDeadzone, Constants.GamecubeControllerTriggerRange);
            int rightTriggerCalibrate = (calibration != null) ? CalibrateTrigger(rightTriggerDeadzone, calibration.RightTriggerMin, calibration.RightTriggerMax, Constants.GamecubeControllerTriggerRange) : rightTriggerDeadzone;
            float rightTriggerSensitivity = rightTriggerCalibrate * profile.RightTriggerSensitivity;
            byte rightTriggerFinal = (byte)Math.Clamp(rightTriggerSensitivity, 0, Constants.GamecubeControllerTriggerRange);
            controller.SetSliderValue(profile.RightTrigger, rightTriggerFinal);

            if (connected)
            {
                controller.SubmitReport();
            }
        }

        public void SetCalibration(GamecubeControllerCalibration calibration)
        {
            this.calibration = calibration;
            OnCalibrationChanged(calibration);
        }

        private int ApplyDeadzone(int value, float deadzone, int range)
        {
            if (Math.Abs(value) < (deadzone * range))
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