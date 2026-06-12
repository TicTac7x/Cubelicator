namespace Cubelicator
{
    public static class ControllerColors
    {
        public static readonly Dictionary<ControllerColor, string> Map = new()
        {
            [ControllerColor.Indigo] = "#3C3760",
            [ControllerColor.JetBlack] = "#28282B",
            [ControllerColor.SpiceOrange] = "#CB7017",
            [ControllerColor.Platinum] = "#D3D6D8",
            [ControllerColor.EmeraldBlue] = "#0E8999",
            [ControllerColor.White] = "#FFFFFF",
            [ControllerColor.StarlightGold] = "#988350",
            [ControllerColor.SymphonicGreen] = "#9EB7B4",
            [ControllerColor.LuigiGreen] = "#078E41",
            [ControllerColor.MarioRed] = "#CF2B2A",
            [ControllerColor.WarioYellow] = "#C58A24",
            [ControllerColor.GundamChar] = "#AB435D",
        };
    }

    public static class ControllerInputColors
    {
        public const string ButtonA = "#229D93";
        public const string ButtonB = "#B22222";
        public const string ButtonX = "#DCDCDC";
        public const string ButtonY = ButtonX;
        public const string ButtonZ = "#4400CD";
        public const string ButtonStart = ButtonX;
        public const string ButtonDPadUp = "#A6A6A6";
        public const string ButtonDPadDown = ButtonDPadUp;
        public const string ButtonDPadLeft = ButtonDPadUp;
        public const string ButtonDPadRight = ButtonDPadUp;
        public const string LeftTrigger = ButtonX;
        public const string RightTrigger = ButtonX;
    }
}