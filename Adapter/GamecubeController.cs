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
        public event Action<ControllerButton, bool> OnButtonChanged = delegate { };
        public event Action<ControllerSide, ControllerStickAxis, int> OnStickChanged = delegate { };
        public event Action<ControllerSide, int> OnTriggerChanged = delegate { };

        private readonly IXbox360Controller controller;
        private GamecubeControllerProfile profile;
        private GamecubeControllerCalibration? calibration = null;
        private GamecubeControllerState _state = new GamecubeControllerState();

        public GamecubeController(ViGEmClient vigem, GamecubeControllerProfile profile)
        {
            this.profile = profile;
            controller = CreateController(vigem);
            controller.AutoSubmitReport = false;
        }

        public void SetProfile(GamecubeControllerProfile profile)
        {
            this.profile = profile;
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
            _state.Connected = false;
        }

        public bool IsConnected()
        {
            return _state.Connected;
        }

        public bool IsCalibrated()
        {
            return calibration != null;
        }

        public void SetState(GamecubeControllerState state)
        {
            if (_state.Connected != state.Connected)
            {
                _state.Connected = state.Connected;
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
            HandleButton(ControllerButton.A, _state.ButtonA, state.ButtonA, profile.A);
            HandleButton(ControllerButton.B, _state.ButtonB, state.ButtonB, profile.B);
            HandleButton(ControllerButton.X, _state.ButtonX, state.ButtonX, profile.X);
            HandleButton(ControllerButton.Y, _state.ButtonY, state.ButtonY, profile.Y);
            HandleButton(ControllerButton.Z, _state.ButtonZ, state.ButtonZ, profile.Z);
            HandleButton(ControllerButton.Start, _state.ButtonStart, state.ButtonStart, profile.Start);
            HandleButton(ControllerButton.LeftShoulder, _state.ButtonLeftShoulder, state.ButtonLeftShoulder, profile.LeftShoulder);
            HandleButton(ControllerButton.RightShoulder, _state.ButtonRightShoulder, state.ButtonRightShoulder, profile.RightShoulder);
            HandleButton(ControllerButton.DPadUp, _state.ButtonDPadUp, state.ButtonDPadUp, profile.DPadUp);
            HandleButton(ControllerButton.DPadDown, _state.ButtonDPadDown, state.ButtonDPadDown, profile.DPadDown);
            HandleButton(ControllerButton.DPadLeft, _state.ButtonDPadLeft, state.ButtonDPadLeft, profile.DPadLeft);
            HandleButton(ControllerButton.DPadRight, _state.ButtonDPadRight, state.ButtonDPadRight, profile.DPadRight);

            // Sticks
            // Left Stick X
            HandleStick(
                ControllerSide.Left, 
                ControllerStickAxis.X, 
                _state.StickLeftX, 
                state.StickLeftX, 
                calibration?.LeftStickXMin, 
                calibration?.LeftStickXCenter, 
                calibration?.LeftStickXMax, 
                profile.LeftStickDeadzone,
                profile.LeftStickSensitivity,
                profile.LeftStickX
            );

            // Left Stick Y
            HandleStick(
                ControllerSide.Left,
                ControllerStickAxis.Y,
                _state.StickLeftY,
                state.StickLeftY,
                calibration?.LeftStickYMin,
                calibration?.LeftStickYCenter,
                calibration?.LeftStickYMax,
                profile.LeftStickDeadzone,
                profile.LeftStickSensitivity,
                profile.LeftStickY
            );

            // Right Stick X
            HandleStick(
                ControllerSide.Right,
                ControllerStickAxis.X,
                _state.StickRightX,
                state.StickRightX,
                calibration?.RightStickXMin,
                calibration?.RightStickXCenter,
                calibration?.RightStickXMax,
                profile.RightStickDeadzone,
                profile.RightStickSensitivity,
                profile.RightStickX
            );

            // Right Stick Y
            HandleStick(
                ControllerSide.Right,
                ControllerStickAxis.Y,
                _state.StickRightY,
                state.StickRightY,
                calibration?.RightStickYMin,
                calibration?.RightStickYCenter,
                calibration?.RightStickYMax,
                profile.RightStickDeadzone,
                profile.RightStickSensitivity,
                profile.RightStickY
            );

            // Left Trigger
            HandleTrigger(
                ControllerSide.Left,
                _state.TriggerLeft,
                state.TriggerLeft,
                calibration?.LeftTriggerMin,
                calibration?.LeftTriggerMax,
                profile.LeftTriggerDeadzone,
                profile.LeftTriggerSensitivity,
                profile.LeftTrigger);

            // Right Trigger
            HandleTrigger(
                ControllerSide.Right,
                _state.TriggerRight,
                state.TriggerRight,
                calibration?.RightTriggerMin,
                calibration?.RightTriggerMax,
                profile.RightTriggerDeadzone,
                profile.RightTriggerSensitivity,
                profile.RightTrigger);

            if (state.Connected)
            {
                controller.SubmitReport();
            }

            _state = state;
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

        private int CalibrateStick(int value, int? resting, int? min, int? max, int targetRange)
        {
            if (resting is not int restVal ||
                min is not int minVal ||
                max is not int maxVal)
            {
                return value;
            }

            value = Math.Clamp(value, minVal, maxVal);

            int centered = value - restVal;

            if (centered >= 0)
            {
                int range = maxVal - restVal;
                return range == 0 ? value : (int)(centered / (float)range * targetRange);
            }

            int negativeRange = restVal - minVal;
            return negativeRange == 0 ? value : (int)(centered / (float)negativeRange * targetRange);
        }

        private int CalibrateTrigger(int value, int? min, int? max, int? range)
        {
            if (min is not int minVal ||
                max is not int maxVal ||
                range is not int rangeVal ||
                maxVal == minVal)
            {
                return value;
            }

            value = Math.Clamp(value, minVal, maxVal);

            return (int) ((value - minVal) / (float)(maxVal - minVal) * rangeVal);
        }

        private void HandleButton(
            ControllerButton button, 
            bool oldValue, 
            bool newValue, 
            GamecubeControllerInput mappedButton
        )
        {
            if (newValue != oldValue)
            {
                controller.SetButtonState(ToButton(mappedButton), newValue);
                OnButtonChanged(button, newValue);
            }
        }

        private void HandleStick(
            ControllerSide side, 
            ControllerStickAxis axis, 
            int oldValue, 
            int newValue, 
            int? calibrationMin,
            int? calibrationCenter, 
            int? calibrationMax, 
            float deadzone, 
            float sensitivity, 
            GamecubeControllerInput mappedButton
        )
        {
            if (newValue != oldValue)
            {
                int stickDeadzone = ApplyDeadzone(newValue, deadzone, Constants.GamecubeControllerStickRange);
                int stickCalibrate = CalibrateStick(stickDeadzone, calibrationCenter, calibrationMin, calibrationMax, Constants.GamecubeControllerStickRange);
                float stickSensitivity = stickCalibrate * sensitivity;
                short stickFinal = (short) Math.Clamp(stickSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
                controller.SetAxisValue(ToAxis(mappedButton), stickFinal);
                OnStickChanged(side, axis, newValue);
            }
        }

        private void HandleTrigger(
            ControllerSide side,
            int oldValue,
            int newValue,
            int? calibrationMin,
            int? calibrationMax,
            float deadzone,
            float sensitivity,
            GamecubeControllerInput mappedTrigger
        )
        {
            if (newValue != oldValue)
            {
                int triggerDeadzone = ApplyDeadzone(
                    newValue,
                    deadzone,
                    Constants.GamecubeControllerTriggerRange);

                int triggerCalibrate = CalibrateTrigger(
                    triggerDeadzone,
                    calibrationMin,
                    calibrationMax,
                    Constants.GamecubeControllerTriggerRange);

                float triggerSensitivity = triggerCalibrate * sensitivity;

                byte triggerFinal = (byte)Math.Clamp(
                    triggerSensitivity,
                    0,
                    Constants.GamecubeControllerTriggerRange);

                controller.SetSliderValue(ToSlider(mappedTrigger), triggerFinal);
                OnTriggerChanged(side, newValue);
            }
        }

        private Xbox360Button? ToButton(GamecubeControllerInput input)
        {
            return input switch
            {
                GamecubeControllerInput.A => Xbox360Button.A,
                GamecubeControllerInput.B => Xbox360Button.B,
                GamecubeControllerInput.X => Xbox360Button.X,
                GamecubeControllerInput.Y => Xbox360Button.Y,

                GamecubeControllerInput.Z => Xbox360Button.Back,
                GamecubeControllerInput.Start => Xbox360Button.Start,

                GamecubeControllerInput.DPadUp => Xbox360Button.Up,
                GamecubeControllerInput.DPadDown => Xbox360Button.Down,
                GamecubeControllerInput.DPadLeft => Xbox360Button.Left,
                GamecubeControllerInput.DPadRight => Xbox360Button.Right,

                GamecubeControllerInput.LeftShoulder => Xbox360Button.LeftShoulder,
                GamecubeControllerInput.RightShoulder => Xbox360Button.RightShoulder,
                _ => null
            };
        }

        private Xbox360Slider? ToSlider(GamecubeControllerInput input)
        {
            return input switch
            {
                GamecubeControllerInput.LeftTrigger => Xbox360Slider.LeftTrigger,
                GamecubeControllerInput.RightTrigger => Xbox360Slider.RightTrigger,
                _ => null
            };
        }

        private Xbox360Axis? ToAxis(GamecubeControllerInput input)
        {
            return input switch
            {
                GamecubeControllerInput.LeftStickX => Xbox360Axis.LeftThumbX,
                GamecubeControllerInput.LeftStickY => Xbox360Axis.LeftThumbY,
                GamecubeControllerInput.RightStickX => Xbox360Axis.RightThumbX,
                GamecubeControllerInput.RightStickY => Xbox360Axis.RightThumbY,
                _ => null
            };
        }
    }
}