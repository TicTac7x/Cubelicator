using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace Cubelicator
{
    public class GamecubeController
    {
        public event Action<GamecubeControllerProfile> Event_ProfileChanged = delegate { };
        public event Action<bool> Event_ConnectionChanged = delegate { };
        public event Action<bool> Event_RumbleChanged = delegate { };
        public event Action<GamecubeControllerCalibration> Event_CalibrationChanged = delegate { };
        public event Action<GamecubeControllerButton, bool> Event_ButtonChanged = delegate { };
        public event Action<GamecubeControllerStick, Axis, int> Event_StickChanged = delegate { };
        public event Action<GamecubeControllerTrigger, int> Event_TriggerChanged = delegate { };

        private GamecubeControllerCalibration? calibration = null;
        public GamecubeControllerCalibration? Calibration => calibration;
        private GamecubeControllerState _state = new GamecubeControllerState();
        private readonly ProfileManager profileManager;
        private readonly IXbox360Controller controller;
        private GamecubeControllerProfile _profile;
        public GamecubeControllerProfile Profile
        {
            get => _profile;
            set
            {
                if (value != _profile)
                {
                    _profile = value;
                    _profile.Event_Deleted += () =>
                    {
                        foreach (var profile in profileManager.Profiles)
                        {
                            if (profile != value)
                            {
                                Profile = profile;
                            }
                        }
                    };
                    Event_ProfileChanged(value);
                }
            }
        }

        public GamecubeController(ViGEmClient vigem, GamecubeControllerProfile profile, ProfileManager profileManager)
        {
            this.profileManager = profileManager;
            _profile = profile;
            controller = CreateController(vigem);
            controller.AutoSubmitReport = false;
        }

        public int GetStickValue(GamecubeControllerStick stick, Axis axis)
        {
            return (stick, axis) switch
            {
                (GamecubeControllerStick.LeftStick, Axis.X) => _state.StickLeftX,
                (GamecubeControllerStick.LeftStick, Axis.Y) => _state.StickLeftY,

                (GamecubeControllerStick.RightStick, Axis.X) => _state.StickRightX,
                (GamecubeControllerStick.RightStick, Axis.Y) => _state.StickRightY,

                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public int GetTriggerValue(GamecubeControllerTrigger trigger)
        {
            return trigger switch
            {
                GamecubeControllerTrigger.LeftTrigger => _state.TriggerLeft,
                GamecubeControllerTrigger.RightTrigger => _state.TriggerRight,

                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private IXbox360Controller CreateController(ViGEmClient vigem)
        {
            var controller = vigem.CreateXbox360Controller();
            controller.FeedbackReceived += (_, args) =>
            {
                if (Profile.Rumble)
                {
                    Event_RumbleChanged.Invoke(args.LargeMotor > 0 || args.SmallMotor > 0);
                }
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
                Event_ConnectionChanged.Invoke(state.Connected);
            }

            if (!state.Connected)
            {
                return;
            }

            // Buttons
            HandleButton(GamecubeControllerButton.A, _state.ButtonA, state.ButtonA, Profile.A);
            HandleButton(GamecubeControllerButton.B, _state.ButtonB, state.ButtonB, Profile.B);
            HandleButton(GamecubeControllerButton.X, _state.ButtonX, state.ButtonX, Profile.X);
            HandleButton(GamecubeControllerButton.Y, _state.ButtonY, state.ButtonY, Profile.Y);
            HandleButton(GamecubeControllerButton.Z, _state.ButtonZ, state.ButtonZ, Profile.Z);
            HandleButton(GamecubeControllerButton.Start, _state.ButtonStart, state.ButtonStart, Profile.Start);
            HandleButton(GamecubeControllerButton.LeftBumper, _state.ButtonLeftShoulder, state.ButtonLeftShoulder, Profile.LeftBumper);
            HandleButton(GamecubeControllerButton.RightBumper, _state.ButtonRightShoulder, state.ButtonRightShoulder, Profile.RightBumper);
            HandleButton(GamecubeControllerButton.DPadUp, _state.ButtonDPadUp, state.ButtonDPadUp, Profile.DPadUp);
            HandleButton(GamecubeControllerButton.DPadDown, _state.ButtonDPadDown, state.ButtonDPadDown, Profile.DPadDown);
            HandleButton(GamecubeControllerButton.DPadLeft, _state.ButtonDPadLeft, state.ButtonDPadLeft, Profile.DPadLeft);
            HandleButton(GamecubeControllerButton.DPadRight, _state.ButtonDPadRight, state.ButtonDPadRight, Profile.DPadRight);

            // Sticks
            // Left Stick X
            HandleStick(
                GamecubeControllerStick.LeftStick,
                Axis.X, 
                _state.StickLeftX, 
                state.StickLeftX, 
                calibration?.LeftStickXMin, 
                calibration?.LeftStickXCenter, 
                calibration?.LeftStickXMax, 
                Profile.LeftStickDeadzone,
                Profile.LeftStickSensitivity,
                Profile.LeftStick
            );

            // Left Stick Y
            HandleStick(
                GamecubeControllerStick.LeftStick,
                Axis.Y,
                _state.StickLeftY,
                state.StickLeftY,
                calibration?.LeftStickYMin,
                calibration?.LeftStickYCenter,
                calibration?.LeftStickYMax,
                Profile.LeftStickDeadzone,
                Profile.LeftStickSensitivity,
                Profile.LeftStick
            );

            // Right Stick X
            HandleStick(
                GamecubeControllerStick.RightStick,
                Axis.X,
                _state.StickRightX,
                state.StickRightX,
                calibration?.RightStickXMin,
                calibration?.RightStickXCenter,
                calibration?.RightStickXMax,
                Profile.RightStickDeadzone,
                Profile.RightStickSensitivity,
                Profile.RightStick
            );

            // Right Stick Y
            HandleStick(
                GamecubeControllerStick.RightStick,
                Axis.Y,
                _state.StickRightY,
                state.StickRightY,
                calibration?.RightStickYMin,
                calibration?.RightStickYCenter,
                calibration?.RightStickYMax,
                Profile.RightStickDeadzone,
                Profile.RightStickSensitivity,
                Profile.RightStick
            );

            // Left Trigger
            HandleTrigger(
                GamecubeControllerTrigger.LeftTrigger,
                _state.TriggerLeft,
                state.TriggerLeft,
                calibration?.LeftTriggerMin,
                calibration?.LeftTriggerMax,
                Profile.LeftTriggerDeadzone,
                Profile.LeftTriggerSensitivity,
                Profile.LeftTrigger);

            // Right Trigger
            HandleTrigger(
                GamecubeControllerTrigger.RightTrigger,
                _state.TriggerRight,
                state.TriggerRight,
                calibration?.RightTriggerMin,
                calibration?.RightTriggerMax,
                Profile.RightTriggerDeadzone,
                Profile.RightTriggerSensitivity,
                Profile.RightTrigger);

            if (state.Connected)
            {
                controller.SubmitReport();
            }

            _state = state;
        }

        public void SetCalibration(GamecubeControllerCalibration calibration)
        {
            this.calibration = calibration;
            Event_CalibrationChanged(calibration);
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
            try
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
            } catch
            {
                return value;
            }
        }

        private int CalibrateTrigger(int value, int? min, int? max, int? range)
        {
            try
            {
                if (min is not int minVal ||
                max is not int maxVal ||
                range is not int rangeVal ||
                maxVal == minVal)
                {
                    return value;
                }

                value = Math.Clamp(value, minVal, maxVal);

                return (int)((value - minVal) / (float)(maxVal - minVal) * rangeVal);
            } catch
            {
                return value;
            }
        }

        private void HandleButton(
            GamecubeControllerButton button, 
            bool oldValue, 
            bool newValue, 
            XboxControllerButton mappedButton
        )
        {
            if (newValue != oldValue && mappedButton != XboxControllerButton.None)
            {
                controller.SetButtonState(ToButton(mappedButton), newValue);
                Event_ButtonChanged(button, newValue);
            }
        }

        private void HandleStick(
            GamecubeControllerStick stick,
            Axis axis,
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
            if (newValue != oldValue && mappedButton != XboxControllerStick.None)
            {
                int stickDeadzone = ApplyDeadzone(newValue, deadzone, Constants.GamecubeControllerStickRange);
                int stickCalibrate = CalibrateStick(stickDeadzone, calibrationCenter, calibrationMin, calibrationMax, Constants.GamecubeControllerStickRange);
                float stickSensitivity = stickCalibrate * sensitivity;
                short stickFinal = (short) Math.Clamp(stickSensitivity * Constants.XboxControllerAxisMultiplier, -Constants.XboxControllerAxisRange, Constants.XboxControllerAxisRange);
                controller.SetAxisValue(ToAxis(mappedButton, axis), stickFinal);
                Event_StickChanged(stick, axis, newValue);
            }
        }

        private void HandleTrigger(
            GamecubeControllerTrigger trigger,
            int oldValue,
            int newValue,
            int? calibrationMin,
            int? calibrationMax,
            float deadzone,
            float sensitivity,
            XboxControllerTrigger mappedTrigger
        )
        {
            if (newValue != oldValue && mappedTrigger != XboxControllerTrigger.None)
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
                Event_TriggerChanged(trigger, newValue);
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

        private Xbox360Axis ToAxis(XboxControllerStick input, Axis axis)
        {
            return input switch
            {
                XboxControllerStick.LeftStick => axis == Axis.X ? Xbox360Axis.LeftThumbX : Xbox360Axis.LeftThumbY,
                XboxControllerStick.RightStick => axis == Axis.X ? Xbox360Axis.RightThumbX : Xbox360Axis.RightThumbY,
            };
        }
    }
}