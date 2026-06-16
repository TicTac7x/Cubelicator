using System.Text.Json.Serialization;

namespace Cubelicator
{
    public class GamecubeControllerProfile
    {
        public event Action OnDelete = delegate { };
        public event Action<string, string> OnNameChanged = delegate { };
        public event Action OnChanged = delegate { };
        private string name = "Default";

        [JsonIgnore]
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name) return;

                var oldName = name;
                name = value;
                
                OnNameChanged(name, value);
                OnChanged();
            }
        }

        // Buttons
        public XboxControllerButton A { get; private set; } = XboxControllerButton.A;
        public XboxControllerButton B { get; private set; } = XboxControllerButton.B;
        public XboxControllerButton X { get; private set; } = XboxControllerButton.X;
        public XboxControllerButton Y { get; private set; } = XboxControllerButton.Y;
        public XboxControllerButton Z { get; private set; } = XboxControllerButton.Back;
        public XboxControllerButton Start { get; private set; } = XboxControllerButton.Start;
        public XboxControllerButton LeftBumper { get; private set; } = XboxControllerButton.LeftBumper;
        public XboxControllerButton RightBumper { get; private set; } = XboxControllerButton.RightBumper;

        // DPad
        public XboxControllerButton DPadUp { get; private set; } = XboxControllerButton.DPadUp;
        public XboxControllerButton DPadDown { get; private set; } = XboxControllerButton.DPadDown;
        public XboxControllerButton DPadLeft { get; private set; } = XboxControllerButton.DPadLeft;
        public XboxControllerButton DPadRight { get; private set; } = XboxControllerButton.DPadRight;

        // Triggers
        public XboxControllerTrigger LeftTrigger { get; private set; } = XboxControllerTrigger.LeftTrigger;
        public XboxControllerTrigger RightTrigger { get; private set; } = XboxControllerTrigger.RightTrigger;

        // Sticks
        public XboxControllerStick LeftStick { get; private set; } = XboxControllerStick.LeftStick;
        public XboxControllerStick RightStick { get; private set; } = XboxControllerStick.RightStick;

        // Deadzones
        public float LeftStickDeadzone { get; private set; } = 0f;
        public float RightStickDeadzone { get; private set; } = 0f;
        public float LeftTriggerDeadzone { get; private set; } = 0f;
        public float RightTriggerDeadzone { get; private set; } = 0f;

        // Sensitivity
        public float LeftStickSensitivity { get; private set; } = 1f;
        public float RightStickSensitivity { get; private set; } = 1f;
        public float LeftTriggerSensitivity { get; private set; } = 1f;
        public float RightTriggerSensitivity { get; private set; } = 1f;

        public void SetButton(GamecubeControllerButton button, XboxControllerButton xboxControllerButton)
        {
            switch (button)
            {
                case GamecubeControllerButton.A:
                    A = xboxControllerButton;
                    break;

                case GamecubeControllerButton.B:
                    B = xboxControllerButton;
                    break;

                case GamecubeControllerButton.X:
                    X = xboxControllerButton;
                    break;

                case GamecubeControllerButton.Y:
                    Y = xboxControllerButton;
                    break;

                case GamecubeControllerButton.Z:
                    Z = xboxControllerButton;
                    break;

                case GamecubeControllerButton.Start:
                    Start = xboxControllerButton;
                    break;

                case GamecubeControllerButton.LeftBumper:
                    LeftBumper = xboxControllerButton;
                    break;

                case GamecubeControllerButton.RightBumper:
                    RightBumper = xboxControllerButton;
                    break;

                case GamecubeControllerButton.DPadUp:
                    DPadUp = xboxControllerButton;
                    break;

                case GamecubeControllerButton.DPadDown:
                    DPadDown = xboxControllerButton;
                    break;

                case GamecubeControllerButton.DPadLeft:
                    DPadLeft = xboxControllerButton;
                    break;

                case GamecubeControllerButton.DPadRight:
                    DPadRight = xboxControllerButton;
                    break;
            }

            OnChanged();
        }

        public void SetStick(GamecubeControllerStick stick, XboxControllerStick xboxControllerStick)
        {
            switch (stick)
            {
                case GamecubeControllerStick.LeftStick:
                    LeftStick = xboxControllerStick;
                    break;

                case GamecubeControllerStick.RightStick:
                    RightStick = xboxControllerStick;
                    break;
            }

            OnChanged();
        }

        public void SetTrigger(GamecubeControllerTrigger trigger, XboxControllerTrigger xboxControllerTrigger)
        {
            switch (trigger)
            {
                case GamecubeControllerTrigger.LeftTrigger:
                    LeftTrigger = xboxControllerTrigger;
                    break;

                case GamecubeControllerTrigger.RightTrigger:
                    RightTrigger = xboxControllerTrigger;
                    break;
            }

            OnChanged();
        }

        public void SetSensitivity(GamecubeControllerStick stick, float sensitivity)
        {
            if (stick == GamecubeControllerStick.LeftStick)
                LeftStickSensitivity = sensitivity;
            else
                RightStickSensitivity = sensitivity;

            OnChanged();
        }

        public void SetSensitivity(GamecubeControllerTrigger trigger, float sensitivity)
        {
            if (trigger == GamecubeControllerTrigger.LeftTrigger)
                LeftTriggerSensitivity = sensitivity;
            else
                RightTriggerSensitivity = sensitivity;

            OnChanged();
        }

        public void SetDeadzone(GamecubeControllerStick stick, float sensitivity)
        {
            if (stick == GamecubeControllerStick.LeftStick)
                LeftStickDeadzone = sensitivity;
            else
                RightStickDeadzone = sensitivity;

            OnChanged();
        }

        public void SetDeadzone(GamecubeControllerTrigger trigger, float deadzone)
        {
            if (trigger == GamecubeControllerTrigger.LeftTrigger)
                LeftTriggerDeadzone = deadzone;
            else
                RightTriggerDeadzone = deadzone;

            OnChanged();
        }

        public void Delete()
        {
            OnDelete();
        }
    }
}