namespace Cubelicator
{
    internal static class Strings
    {
        public const string AppName = "Cubelicator";
        public const string Exit = "Exit";
        public const string Open = "Open";
        public const string Controller = "Controller";
        public const string Port = "Port";
        public static string TooltipPortIsNotCalibrated(AdapterPort port)
        {
            return $"Port {(int) port} is not calibrated";
        }
        public static string TooltipPortIsCalibrated(AdapterPort port)
        {
            return $"Port {(int) port} is calibrated";
        }
        public const string Calibrate = "Calibrate";
        public const string ChangeColor = "Change Color";

        public const string PortTile_Edit = "Edit";

        public const string ProfileSelector_New = "New";
        public const string ProfileSelector_Rename = "Rename";
        public const string ProfileSelector_Delete = "Delete";
        public const string ProfileSelector_EnableRumble = "Enable rumble";
        public const string ProfileSelector_DisableRumble = "Disable rumble";
    }
}
