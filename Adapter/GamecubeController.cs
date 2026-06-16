using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace Cubelicator
{
    public class GamecubeController
    {
        public event Action<GamecubeControllerProfile> OnProfileChanged = delegate { };
        public event Action<bool> OnConnectionChanged = delegate { };
        public event Action<bool> OnRumbleChanged = delegate { };
        public event Action<GamecubeControllerCalibration> OnCalibrationChanged = delegate { };
        public event Action<GamecubeControllerButton, bool> OnButtonChanged = delegate { };
        public event Action<ControllerSide, ControllerStickAxis, int> OnStickChanged = delegate { };
        public event Action<ControllerSide, int> OnTriggerChanged = delegate { };

        private readonly IXbox360Controller controller;
        private GamecubeControllerProfile profile;
        public GamecubeControllerProfile Profile
        {
            get => profile;
            set
            {
                if (value != profile)
                {
                    profile = value;
                    OnProfileChanged(value);
                }
            }
        }
        private GamecubeControllerCalibration? calibration = null;
        private GamecubeControllerState _state = new GamecubeControllerState();

        

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
            HandleButton(GamecubeControllerButton.A, _state.ButtonA, state.ButtonA, profile.A);
            HandleButton(GamecubeControllerButton.B, _state.ButtonB, state.ButtonB, profile.B);
            HandleButton(GamecubeControllerButton.X, _state.ButtonX, state.ButtonX, profile.X);
            HandleButton(GamecubeControllerButton.Y, _state.ButtonY, state.ButtonY, profile.Y);
            HandleButton(GamecubeControllerButton.Z, _state.ButtonZ, state.ButtonZ, profile.Z);
            HandleButton(GamecubeControllerButton.Start, _state.ButtonStart, state.ButtonStart, profile.Start);
            HandleButton(GamecubeControllerButton.LeftBumper, _state.ButtonLeftShoulder, state.ButtonLeftShoulder, profile.LeftBumper);
            HandleButton(GamecubeControllerButton.RightBumper, _state.ButtonRightShoulder, state.ButtonRightShoulder, profile.RightBumper);
            HandleButton(GamecubeControllerButton.DPadUp, _state.ButtonDPadUp, state.ButtonDPadUp, profile.DPadUp);
            HandleButton(GamecubeControllerButton.DPadDown, _state.ButtonDPadDown, state.ButtonDPadDown, profile.DPadDown);
            HandleButton(GamecubeControllerButton.DPadLeft, _state.ButtonDPadLeft, state.ButtonDPadLeft, profile.DPadLeft);
            HandleButton(GamecubeControllerButton.DPadRight, _state.ButtonDPadRight, state.ButtonDPadRight, profile.DPadRight);

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
                profile.LeftStick
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
                profile.LeftStick
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
                profile.RightStick
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
                profile.RightStick
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
            GamecubeControllerButton button, 
            bool oldValue, 
            bool newValue, 
            XboxControllerButton mappedButton
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
            XboxControllerStick mappedButton
        )
        {
            if (newValue != oldValue)
            {
                int stickDeadzone = ApplyDeadzone(newValue, deadzone, Constants.GamecubeControllerStickRange);
                int stickCalibrate = CalibrateStick(stickDeadzone, calibrationCenter, calibrationMin, calibrationMax, Constants.GamecubeControllerStickRange);
                float stickSensitivity = stickCalibrate * sensitivity;
                short stickFinal = (short) Math.Clamp(stickSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
                controller.SetAxisValue(ToAxis(mappedButton, axis), stickFinal);
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
            XboxControllerTrigger mappedTrigger
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

        private Xbox360Button ToButton(XboxControllerButton input)
        {
            return input switch
            {
                XboxControllerButton.A => Xbox360Button.A,
                XboxControllerButton.B => Xbox360Button.B,
                XboxControllerButton.X => Xbox360Button.X,
                XboxControllerButton.Y => Xbox360Button.Y,

                XboxControllerButton.Back => Xbox360Button.Back,
                XboxControllerButton.Start => Xbox360Button.Start,
                XboxControllerButton.Guide => Xbox360Button.Guide,

                XboxControllerButton.LeftStickDown=> Xbox360Button.LeftThumb,
                XboxControllerButton.RightStickDown => Xbox360Button.RightThumb,

                XboxControllerButton.DPadUp => Xbox360Button.Up,
                XboxControllerButton.DPadDown => Xbox360Button.Down,
                XboxControllerButton.DPadLeft => Xbox360Button.Left,
                XboxControllerButton.DPadRight => Xbox360Button.Right,

                XboxControllerButton.LeftBumper => Xbox360Button.LeftShoulder,
                XboxControllerButton.RightBumper => Xbox360Button.RightShoulder,
            };
        }

        private Xbox360Slider ToSlider(XboxControllerTrigger input)
        {
            return input switch
            {
                XboxControllerTrigger.LeftTrigger=> Xbox360Slider.LeftTrigger,
                XboxControllerTrigger.RightTrigger=> Xbox360Slider.RightTrigger,
            };
        }

        private Xbox360Axis ToAxis(XboxControllerStick input, ControllerStickAxis axis)
        {
            return input switch
            {
                XboxControllerStick.LeftStick => axis == ControllerStickAxis.X ? Xbox360Axis.LeftThumbX : Xbox360Axis.LeftThumbY,
                XboxControllerStick.RightStick => axis == ControllerStickAxis.X ? Xbox360Axis.RightThumbX : Xbox360Axis.RightThumbY,
            };
        }
    }
}