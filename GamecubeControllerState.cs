namespace ConsoleApp1
{
    internal struct GamecubeControllerState
    {
        public bool Connected;
        public bool A, B, X, Y;
        public bool Start, Z, L, R;
        public bool DpadUp, DpadDown, DpadLeft, DpadRight;

        public short LeftStickX, LeftStickY;
        public short RightStickX, RightStickY;

        public byte TriggerLeft, TriggerRight;
    }
}