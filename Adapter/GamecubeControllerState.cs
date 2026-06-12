namespace Cubelicator
{
    public class GamecubeControllerState
    {
        public bool Connected;
        public bool ButtonA, ButtonB, ButtonX, ButtonY;
        public bool ButtonStart, ButtonZ, ButtonLeftShoulder, ButtonRightShoulder;
        public bool ButtonDPadUp, ButtonDPadDown, ButtonDPadLeft, ButtonDPadRight;

        public short StickLeftX, StickLeftY;
        public short StickRightX, StickRightY;

        public byte TriggerLeft, TriggerRight;
    }
}