using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace Cubelicator
{
    public class GamecubeController
    {
        public event Action<bool> onConnectionChanged = delegate { };
        public event Action<bool> onRumbleChanged = delegate { };
        public event Action<GamecubeControllerCalibration> onCalibrationChanged = delegate { };

        private readonly IXbox360Controller controller;
        private bool connected = false;
        private readonly GamecubeControllerProfile profile;
        private GamecubeControllerCalibration? calibration;

        public GamecubeController(ViGEmClient vigem, GamecubeControllerProfile profile)
        {
            this.profile = profile;
            controller = createController(vigem);
            controller.AutoSubmitReport = false;
        }

        private IXbox360Controller createController(ViGEmClient vigem)
        {
            var controller = vigem.CreateXbox360Controller();
            controller.FeedbackReceived += (_, args) =>
            {
                onRumbleChanged.Invoke(args.LargeMotor > 0 || args.SmallMotor > 0);
            };

            return controller;
        }

        public void disconnect()
        {
            controller.Disconnect();
            connected = false;
        }

        public bool isConnected()
        {
            return connected;
        }

        public bool isCalibrated()
        {
            return calibration != null;
        }

        public void setState(GamecubeControllerState state)
        {
            if (connected != state.connected)
            {
                connected = state.connected;
                if (state.connected)
                {
                    controller.Connect();
                } else
                {
                    controller.Disconnect();
                }
                onConnectionChanged.Invoke(state.connected);
            }

            if (!state.connected)
            {
                return;
            }

            // Buttons
            controller.SetButtonState(profile.A, state.buttonA);
            controller.SetButtonState(profile.B, state.buttonB);
            controller.SetButtonState(profile.X, state.buttonX);
            controller.SetButtonState(profile.Y, state.buttonY);
            controller.SetButtonState(profile.Z, state.buttonZ);
            controller.SetButtonState(profile.Start, state.buttonStart);
            controller.SetButtonState(profile.LeftTriggerButton, state.buttonLeftTrigger);
            controller.SetButtonState(profile.RightTriggerButton, state.buttonRightTrigger);

            // Dpad
            controller.SetButtonState(profile.DPadUp, state.buttonDPadUp);
            controller.SetButtonState(profile.DPadDown, state.buttonDPadDown);
            controller.SetButtonState(profile.DPadLeft, state.buttonDPadLeft);
            controller.SetButtonState(profile.DPadRight, state.buttonDPadRight);

            // Left Stick X
            int leftStickX = applyDeadzone(state.stickLeftX, profile.LeftStickDeadzone, Constants.gamecubeControllerStickRange);
            if (calibration != null)
                leftStickX = calibrateStick(leftStickX, calibration.leftStickXCenter, calibration.leftStickXMin, calibration.leftStickXMax, Constants.gamecubeControllerStickRange);
            float leftStickXSensitivity = leftStickX * profile.LeftStickSensitivity;
            short leftStickXFinal = (short) Math.Clamp(leftStickXSensitivity * Constants.xboxControllerAxisMultiplier, -Constants.xboxControllerAxisRange, Constants.xboxControllerAxisRange);
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, leftStickXFinal);

            // Left Stick Y
            int leftStickY = applyDeadzone(state.stickLeftY, profile.LeftStickDeadzone, Constants.gamecubeControllerStickRange);
            if (calibration != null)
                leftStickY = calibrateStick(leftStickY, calibration.leftStickYCenter, calibration.leftStickYMin, calibration.leftStickYMax, Constants.gamecubeControllerStickRange);
            float leftStickYSensitivity = leftStickY * profile.LeftStickSensitivity;
            short leftStickYFinal = (short)Math.Clamp(
                leftStickYSensitivity * Constants.xboxControllerAxisMultiplier,
                -Constants.xboxControllerAxisRange,
                Constants.xboxControllerAxisRange
            );
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, leftStickYFinal);

            // Right Stick X
            int rightStickX = applyDeadzone(state.stickRightX, profile.RightStickDeadzone, Constants.gamecubeControllerStickRange);
            if (calibration != null)
                rightStickX = calibrateStick(rightStickX, calibration.rightStickXCenter, calibration.rightStickXMin, calibration.rightStickXMax, Constants.gamecubeControllerStickRange);
            float rightStickXSensitivity = rightStickX * profile.RightStickSensitivity;
            short rightStickXFinal = (short)Math.Clamp(
                rightStickXSensitivity * Constants.xboxControllerAxisMultiplier,
                -Constants.xboxControllerAxisRange,
                Constants.xboxControllerAxisRange
            );
            controller.SetAxisValue(Xbox360Axis.RightThumbX, rightStickXFinal);

            // Right Stick Y
            int rightStickY = applyDeadzone(state.stickRightY, profile.RightStickDeadzone, Constants.gamecubeControllerStickRange);
            if (calibration != null)
                rightStickY = calibrateStick(rightStickY, calibration.rightStickYCenter, calibration.rightStickYMin, calibration.rightStickYMax, Constants.gamecubeControllerStickRange);
            float rightStickYSensitivity = rightStickY * profile.RightStickSensitivity;
            short rightStickYFinal = (short)Math.Clamp(
                rightStickYSensitivity * Constants.xboxControllerAxisMultiplier,
                -Constants.xboxControllerAxisRange,
                Constants.xboxControllerAxisRange
            );
            controller.SetAxisValue(Xbox360Axis.RightThumbY, rightStickYFinal);

            // Left Trigger
            int leftTrigger = applyDeadzone(state.triggerLeft, profile.LeftTriggerDeadzone, Constants.gamecubeControllerTriggerRange);
            if (calibration != null)
                leftTrigger = calibrateTrigger(leftTrigger, calibration.leftTriggerMin, calibration.leftTriggerMax, Constants.gamecubeControllerTriggerRange);
            float leftTriggerSensitivity = leftTrigger * profile.LeftTriggerSensitivity;
            byte leftTriggerFinal = (byte)Math.Clamp(
                leftTriggerSensitivity,
                0,
                Constants.gamecubeControllerTriggerRange
            );
            controller.SetSliderValue(profile.LeftTrigger, leftTriggerFinal);

            // Right Trigger
            int rightTrigger = applyDeadzone(state.triggerRight, profile.RightTriggerDeadzone, Constants.gamecubeControllerTriggerRange);
            if (calibration != null)
                rightTrigger = calibrateTrigger(rightTrigger, calibration.rightTriggerMin, calibration.rightTriggerMax, Constants.gamecubeControllerTriggerRange);
            float rightTriggerSensitivity = rightTrigger * profile.RightTriggerSensitivity;
            byte rightTriggerFinal = (byte)Math.Clamp(
                rightTriggerSensitivity,
                0,
                Constants.gamecubeControllerTriggerRange
            );
            controller.SetSliderValue(profile.RightTrigger, rightTriggerFinal);

            if (connected)
            {
                controller.SubmitReport();
            }
        }

        public void setCalibration(GamecubeControllerCalibration calibration)
        {
            this.calibration = calibration;
            App.debugOutput("changed" + calibration.leftStickXMin);
            onCalibrationChanged(calibration);
        }

        private int applyDeadzone(int value, double deadzone, int range)
        {
            if (Math.Abs(value) < deadzone * range)
            {
                return 0;
            }

            return value;
        }

        private int calibrateStick(int value, int resting, int min, int max, int targetRange)
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

        private int calibrateTrigger(int value, int min, int max, int range)
        {
            value = Math.Clamp(value, min, max);
            return (int)((value - min) / (float)(max - min) * range);
        }
    }
}