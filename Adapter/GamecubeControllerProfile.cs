using System.Text.Json.Serialization;

namespace Cubelicator
{
    public class GamecubeControllerProfile
    {
        public event Action Event_Deleted = delegate { };
        public event Action<string, string> Event_NameChanged = delegate { };
        public event Action Event_Changed = delegate { };
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
                
                Event_NameChanged(oldName, value);
                Event_Changed();
            }
        }

        // Buttons
        [JsonInclude]
        public XboxControllerButton A { get; private set; } = XboxControllerButton.A;
        [JsonInclude]
        public XboxControllerButton B { get; private set; } = XboxControllerButton.B;
        [JsonInclude]
        public XboxControllerButton X { get; private set; } = XboxControllerButton.X;
        [JsonInclude]
        public XboxControllerButton Y { get; private set; } = XboxControllerButton.Y;
        [JsonInclude]
        public XboxControllerButton Z { get; private set; } = XboxControllerButton.Back;
        [JsonInclude]
        public XboxControllerButton Start { get; private set; } = XboxControllerButton.Start;
        [JsonInclude]
        public XboxControllerButton LeftBumper { get; private set; } = XboxControllerButton.LeftBumper;
        [JsonInclude]
        public XboxControllerButton RightBumper { get; private set; } = XboxControllerButton.RightBumper;

        // DPad
        [JsonInclude]
        public XboxControllerButton DPadUp { get; private set; } = XboxControllerButton.DPadUp;
        [JsonInclude]
        public XboxControllerButton DPadDown { get; private set; } = XboxControllerButton.DPadDown;
        [JsonInclude]
        public XboxControllerButton DPadLeft { get; private set; } = XboxControllerButton.DPadLeft;
        [JsonInclude]
        public XboxControllerButton DPadRight { get; private set; } = XboxControllerButton.DPadRight;

        // Triggers
        [JsonInclude]
        public XboxControllerTrigger LeftTrigger { get; private set; } = XboxControllerTrigger.LeftTrigger;
        [JsonInclude]
        public XboxControllerTrigger RightTrigger { get; private set; } = XboxControllerTrigger.RightTrigger;

        // Sticks
        [JsonInclude]
        public XboxControllerStick LeftStick { get; private set; } = XboxControllerStick.LeftStick;
        [JsonInclude]
        public XboxControllerStick RightStick { get; private set; } = XboxControllerStick.RightStick;

        // Specials
        [JsonInclude]
        public bool DisableLeftTriggerOnClick { get; set; } = false;
        [JsonInclude]
        public bool DisableRightTriggerOnClick { get; set; } = false;

        // Deadzones
        [JsonInclude]
        public float LeftStickDeadzone { get; private set; } = 0f;
        [JsonInclude]
        public float RightStickDeadzone { get; private set; } = 0f;
        [JsonInclude]
        public float LeftTriggerDeadzone { get; private set; } = 0f;
        [JsonInclude]
        public float RightTriggerDeadzone { get; private set; } = 0f;

        // Sensitivity
        [JsonInclude]
        public float LeftStickSensitivity { get; private set; } = 1f;
        [JsonInclude]
        public float RightStickSensitivity { get; private set; } = 1f;
        [JsonInclude]
        public float LeftTriggerSensitivity { get; private set; } = 1f;
        [JsonInclude]
        public float RightTriggerSensitivity { get; private set; } = 1f;

        // Rumble
        public bool Rumble { get; private set; } = true;

        public void SetRumble(bool rumble)
        {
            Rumble = rumble;
            Event_Changed();
        }

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

            Event_Changed();
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

            Event_Changed();
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

            Event_Changed();
        }

        public void SetSensitivity(GamecubeControllerStick stick, float sensitivity)
        {
            if (stick == GamecubeControllerStick.LeftStick)
                LeftStickSensitivity = sensitivity;
            else
                RightStickSensitivity = sensitivity;

            Event_Changed();
        }

        public void SetSensitivity(GamecubeControllerTrigger trigger, float sensitivity)
        {
            if (trigger == GamecubeControllerTrigger.LeftTrigger)
                LeftTriggerSensitivity = sensitivity;
            else
                RightTriggerSensitivity = sensitivity;

            Event_Changed();
        }

        public void SetDeadzone(GamecubeControllerStick stick, float sensitivity)
        {
            if (stick == GamecubeControllerStick.LeftStick)
                LeftStickDeadzone = sensitivity;
            else
                RightStickDeadzone = sensitivity;

            Event_Changed();
        }

        public void SetDeadzone(GamecubeControllerTrigger trigger, float deadzone)
        {
            if (trigger == GamecubeControllerTrigger.LeftTrigger)
                LeftTriggerDeadzone = deadzone;
            else
                RightTriggerDeadzone = deadzone;

            Event_Changed();
        }

        public void Delete()
        {
            Event_Deleted();
        }

        public void SetDisableTriggerOnClick(GamecubeControllerTrigger trigger, bool disable)
        {
            if (trigger == GamecubeControllerTrigger.LeftTrigger)
            {
                DisableLeftTriggerOnClick = disable;
            } else
            {
                DisableRightTriggerOnClick = disable;
            }

            Event_Changed();
        }
    }
}