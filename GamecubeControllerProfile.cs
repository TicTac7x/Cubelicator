using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace ConsoleApp1
{
    internal class GamecubeControllerProfile
    {
        public string Name { get; set; } = "Default";

        // Buttons
        public Xbox360Button A { get; set; } = Xbox360Button.A;
        public Xbox360Button B { get; set; } = Xbox360Button.B;
        public Xbox360Button X { get; set; } = Xbox360Button.X;
        public Xbox360Button Y { get; set; } = Xbox360Button.Y;
        public Xbox360Button Z { get; set; } = Xbox360Button.Back;
        public Xbox360Button Start { get; set; } = Xbox360Button.Start;

        // Dpad
        public Xbox360Button DPadUp { get; set; } = Xbox360Button.Up;
        public Xbox360Button DPadDown { get; set; } = Xbox360Button.Down;
        public Xbox360Button DPadLeft { get; set; } = Xbox360Button.Left;
        public Xbox360Button DPadRight { get; set; } = Xbox360Button.Right;

        // Triggers
        public Xbox360Button LeftTriggerButton { get; set; } = Xbox360Button.LeftShoulder;
        public Xbox360Button RightTriggerButton { get; set; } = Xbox360Button.RightShoulder;
        public Xbox360Slider LeftTrigger { get; set; } = Xbox360Slider.LeftTrigger;
        public Xbox360Slider RightTrigger { get; set; } = Xbox360Slider.RightTrigger;

        // Calibration
        public float LeftStickDeadzone { get; set; } = 0f;
        public float RightStickDeadzone { get; set; } = 0f;
        public float LeftTriggerDeadzone { get; set; } = 0f;
        public float RightTriggerDeadzone { get; set; } = 0f;
        //public float LeftStickDeadzone { get; set; } = 0.2f;
        //public float RightStickDeadzone { get; set; } = 0.1f;
        //public float LeftTriggerDeadzone { get; set; } = 0.15f;
        //public float RightTriggerDeadzone { get; set; } = 0.20f;
    }
}