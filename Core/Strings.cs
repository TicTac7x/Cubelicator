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
            return "Port " + port.ToString() + " is not calibrated";
        }
        public static string TooltipPortIsCalibrated(AdapterPort port)
        {
            return "Port " + port.ToString() + " is calibrated";
        }
        public const string Calibrate = "Calibrate";
        public const string ChangeColor = "Change Color";
        public const string Edit = "Edit";
    }
}
