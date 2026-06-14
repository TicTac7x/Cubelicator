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
                profile = value;
                OnProfileChanged(value);
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
            HandleButton(GamecubeControllerButton.A, _state.ButtonA, state.ButtonA, profile.ButtonA);
            HandleButton(GamecubeControllerButton.B, _state.ButtonB, state.ButtonB, profile.ButtonB);
            HandleButton(GamecubeControllerButton.X, _state.ButtonX, state.ButtonX, profile.ButtonX);
            HandleButton(GamecubeControllerButton.Y, _state.ButtonY, state.ButtonY, profile.ButtonY);
            HandleButton(GamecubeControllerButton.Z, _state.ButtonZ, state.ButtonZ, profile.ButtonZ);
            HandleButton(GamecubeControllerButton.Start, _state.ButtonStart, state.ButtonStart, profile.ButtonStart);
            HandleButton(GamecubeControllerButton.LeftShoulder, _state.ButtonLeftShoulder, state.ButtonLeftShoulder, profile.ButtonLeftShoulder);
            HandleButton(GamecubeControllerButton.RightShoulder, _state.ButtonRightShoulder, state.ButtonRightShoulder, profile.ButtonRightShoulder);
            HandleButton(GamecubeControllerButton.DPadUp, _state.ButtonDPadUp, state.ButtonDPadUp, profile.ButtonDPadUp);
            HandleButton(GamecubeControllerButton.DPadDown, _state.ButtonDPadDown, state.ButtonDPadDown, profile.ButtonDPadDown);
            HandleButton(GamecubeControllerButton.DPadLeft, _state.ButtonDPadLeft, state.ButtonDPadLeft, profile.ButtonDPadLeft);
            HandleButton(GamecubeControllerButton.DPadRight, _state.ButtonDPadRight, state.ButtonDPadRight, profile.ButtonDPadRight);

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
                profile.StickLeftX
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
                profile.StickLeftY
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
                profile.StickRightX
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
                profile.StickRightY
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
                profile.TriggerLeft);

            // Right Trigger
            HandleTrigger(
                ControllerSide.Right,
                _state.TriggerRight,
                state.TriggerRight,
                calibration?.RightTriggerMin,
                calibration?.RightTriggerMax,
                profile.RightTriggerDeadzone,
                profile.RightTriggerSensitivity,
                profile.TriggerRight);

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

                XboxControllerButton.LeftThumb=> Xbox360Button.LeftThumb,
                XboxControllerButton.RightThumb => Xbox360Button.RightThumb,

                XboxControllerButton.DPadUp => Xbox360Button.Up,
                XboxControllerButton.DPadDown => Xbox360Button.Down,
                XboxControllerButton.DPadLeft => Xbox360Button.Left,
                XboxControllerButton.DPadRight => Xbox360Button.Right,

                XboxControllerButton.LeftShoulder => Xbox360Button.LeftShoulder,
                XboxControllerButton.RightShoulder => Xbox360Button.RightShoulder,
            };
        }

        private Xbox360Slider ToSlider(XboxControllerTrigger input)
        {
            return input switch
            {
                XboxControllerTrigger.Left=> Xbox360Slider.LeftTrigger,
                XboxControllerTrigger.Right=> Xbox360Slider.RightTrigger,
            };
        }

        private Xbox360Axis ToAxis(XboxControllerStick input)
        {
            return input switch
            {
                XboxControllerStick.LeftX => Xbox360Axis.LeftThumbX,
                XboxControllerStick.LeftY => Xbox360Axis.LeftThumbY,
                XboxControllerStick.RightX => Xbox360Axis.RightThumbX,
                XboxControllerStick.RightY => Xbox360Axis.RightThumbY,
            };
        }
    }
}