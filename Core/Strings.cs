namespace Cubelicator
{
    internal static class Strings
    {
        public const string appName = "Cubelicator";
        public const string exit = "Exit";
        public const string open = "Open";
        public const string controller = "Controller";
        public const string port = "Port";
        public static string tooltipPortIsNotCalibrated(int port)
        {
            return "Port " + port + " is not calibrated";
        }
        public static string tooltipPortIsCalibrated(int port)
        {
            return "Port " + port + " is calibrated";
        }
    }
}
