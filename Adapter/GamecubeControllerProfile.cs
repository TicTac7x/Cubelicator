using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace Cubelicator
{
    public class GamecubeControllerProfile
    {
        public string Name { get; set; } = "Default";

        // Buttons
        public GamecubeControllerInput A { get; set; } = GamecubeControllerInput.A;
        public GamecubeControllerInput B { get; set; } = GamecubeControllerInput.B;
        public GamecubeControllerInput X { get; set; } = GamecubeControllerInput.X;
        public GamecubeControllerInput Y { get; set; } = GamecubeControllerInput.Y;
        public GamecubeControllerInput Z { get; set; } = GamecubeControllerInput.Z;
        public GamecubeControllerInput Start { get; set; } = GamecubeControllerInput.Start;
        public GamecubeControllerInput LeftShoulder { get; set; } = GamecubeControllerInput.LeftShoulder;
        public GamecubeControllerInput RightShoulder { get; set; } = GamecubeControllerInput.RightShoulder;

        // DPad
        public GamecubeControllerInput DPadUp { get; set; } = GamecubeControllerInput.DPadUp;
        public GamecubeControllerInput DPadDown { get; set; } = GamecubeControllerInput.DPadDown;
        public GamecubeControllerInput DPadLeft { get; set; } = GamecubeControllerInput.DPadLeft;
        public GamecubeControllerInput DPadRight { get; set; } = GamecubeControllerInput.DPadRight;

        // Triggers
        public GamecubeControllerInput LeftTrigger { get; set; } = GamecubeControllerInput.LeftTrigger;
        public GamecubeControllerInput LeftTriggerButton { get; set; } = GamecubeControllerInput.LeftShoulder;
        public GamecubeControllerInput RightTrigger { get; set; } = GamecubeControllerInput.RightTrigger;
        public GamecubeControllerInput RightTriggerButton { get; set; } = GamecubeControllerInput.RightShoulder;

        // Sticks
        public GamecubeControllerInput LeftStickX { get; set; } = GamecubeControllerInput.LeftStickX;
        public GamecubeControllerInput LeftStickY { get; set; } = GamecubeControllerInput.LeftStickY;
        public GamecubeControllerInput RightStickX { get; set; } = GamecubeControllerInput.RightStickX;
        public GamecubeControllerInput RightStickY { get; set; } = GamecubeControllerInput.RightStickY;

        // Deadzones
        public float LeftStickDeadzone { get; set; } = 0f;
        public float RightStickDeadzone { get; set; } = 0f;
        public float LeftTriggerDeadzone { get; set; } = 0f;
        public float RightTriggerDeadzone { get; set; } = 0f;

        // Sensitivity
        public float LeftStickSensitivity { get; set; } = 1f;
        public float RightStickSensitivity { get; set; } = 1f;
        public float LeftTriggerSensitivity { get; set; } = 1f;
        public float RightTriggerSensitivity { get; set; } = 1f;

    }
}