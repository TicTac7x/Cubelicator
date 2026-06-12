namespace Cubelicator
{
    internal static class Strings
    {
        public const string AppName = "Cubelicator";
        public const string Exit = "Exit";
        public const string Open = "Open";
        public const string Controller = "Controller";
        public const string Port = "Port";
        public static string TooltipPortIsNotCalibrated(int port)
        {
            return "Port " + port + " is not calibrated";
        }
        public static string TooltipPortIsCalibrated(int port)
        {
            return "Port " + port + " is calibrated";
        }
        public const string Calibrate = "Calibrate";
        public const string ChangeColor = "Change Color";
    }
}
