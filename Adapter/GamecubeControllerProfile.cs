using System.Text.Json.Serialization;

namespace Cubelicator
{
    public class GamecubeControllerProfile
    {
        public event Action OnChanged = delegate { };

        [JsonIgnore]
        public string Name { get; set; } = "Default";

        // Buttons
        public XboxControllerButton ButtonA { get; private set; } = XboxControllerButton.A;
        public XboxControllerButton ButtonB { get; private set; } = XboxControllerButton.B;
        public XboxControllerButton ButtonX { get; private set; } = XboxControllerButton.X;
        public XboxControllerButton ButtonY { get; private set; } = XboxControllerButton.Y;
        public XboxControllerButton ButtonZ { get; private set; } = XboxControllerButton.Back;
        public XboxControllerButton ButtonStart { get; private set; } = XboxControllerButton.Start;
        public XboxControllerButton ButtonLeftShoulder { get; private set; } = XboxControllerButton.LeftShoulder;
        public XboxControllerButton ButtonRightShoulder { get; private set; } = XboxControllerButton.RightShoulder;

        // DPad
        public XboxControllerButton ButtonDPadUp { get; private set; } = XboxControllerButton.DPadUp;
        public XboxControllerButton ButtonDPadDown { get; private set; } = XboxControllerButton.DPadDown;
        public XboxControllerButton ButtonDPadLeft { get; private set; } = XboxControllerButton.DPadLeft;
        public XboxControllerButton ButtonDPadRight { get; private set; } = XboxControllerButton.DPadRight;

        // Triggers
        public XboxControllerTrigger TriggerLeft { get; private set; } = XboxControllerTrigger.Left;
        public XboxControllerTrigger TriggerRight { get; private set; } = XboxControllerTrigger.Right;

        // Sticks
        public XboxControllerStick StickLeftX { get; private set; } = XboxControllerStick.LeftX;
        public XboxControllerStick StickLeftY { get; private set; } = XboxControllerStick.LeftY;
        public XboxControllerStick StickRightX { get; private set; } = XboxControllerStick.RightX;
        public XboxControllerStick StickRightY { get; private set; } = XboxControllerStick.RightY;

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

        public void SetButton(GamecubeControllerButton gamecubeControllerButton, XboxControllerButton xboxControllerButton)
        {
            switch (gamecubeControllerButton)
            {
                case GamecubeControllerButton.A:
                    ButtonA = xboxControllerButton;
                    break;

                case GamecubeControllerButton.B:
                    ButtonB = xboxControllerButton;
                    break;

                case GamecubeControllerButton.X:
                    ButtonX = xboxControllerButton;
                    break;

                case GamecubeControllerButton.Y:
                    ButtonY = xboxControllerButton;
                    break;

                case GamecubeControllerButton.Z:
                    ButtonZ = xboxControllerButton;
                    break;

                case GamecubeControllerButton.Start:
                    ButtonStart = xboxControllerButton;
                    break;

                case GamecubeControllerButton.LeftShoulder:
                    ButtonLeftShoulder = xboxControllerButton;
                    break;

                case GamecubeControllerButton.RightShoulder:
                    ButtonRightShoulder = xboxControllerButton;
                    break;

                case GamecubeControllerButton.DPadUp:
                    ButtonDPadUp = xboxControllerButton;
                    break;

                case GamecubeControllerButton.DPadDown:
                    ButtonDPadDown = xboxControllerButton;
                    break;

                case GamecubeControllerButton.DPadLeft:
                    ButtonDPadLeft = xboxControllerButton;
                    break;

                case GamecubeControllerButton.DPadRight:
                    ButtonDPadRight = xboxControllerButton;
                    break;
            }
            OnChanged();
        }
    }
}