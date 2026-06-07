namespace CubeGem
{
    internal struct GamecubeControllerState
    {
        public bool Connected;
        public bool A, B, X, Y;
        public bool Start, Z, LeftTriggerButton, RightTriggerButton;
        public bool DpadUp, DpadDown, DpadLeft, DpadRight;

        public short LeftStickX, LeftStickY;
        public short RightStickX, RightStickY;

        public byte LeftTrigger, RightTrigger;
    }
}