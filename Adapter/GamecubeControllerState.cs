namespace Cubelicator
{
    public struct GamecubeControllerState
    {
        public bool connected;
        public bool buttonA, buttonB, buttonX, buttonY;
        public bool buttonStart, buttonZ, buttonLeftTrigger, buttonRightTrigger;
        public bool buttonDPadUp, buttonDPadDown, buttonDPadLeft, buttonDPadRight;

        public short stickLeftX, stickLeftY;
        public short stickRightX, stickRightY;

        public byte triggerLeft, triggerRight;
    }
}