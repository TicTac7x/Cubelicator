namespace Cubelicator
{
    public class GamecubeControllerState
    {
        public bool Connected;
        public bool A, B, X, Y;
        public bool Start, Z, LeftBumper, RightBumper;
        public bool DPadUp, DPadDown, DPadLeft, DPadRight;

        public short LeftStickX, LeftStickY;
        public short RightStickX, RightStickY;

        public byte LeftTrigger, RightTrigger;
    }
}