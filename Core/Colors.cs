namespace Cubelicator
{
    public static class Colors
    {
        // Base backgrounds
        public const string AppBackground = "#222222";
        public const string DeviceBackground = "#111111";

        // Text / foreground
        public const string DeviceForeground = "#D3D3D3";

        // Controller interaction
        public const string ControllerButtonPressed = "#4897D2";

        // Navigation button states
        public const string ButtonBackground = DeviceBackground;
        public const string ButtonHoverBackground = "#313131";
        public const string ButtonPressedBackground = "#1C1C1C";
    }

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
        public const string ButtonDPad = ButtonX;
        public const string Trigger = ButtonX;
    }
}