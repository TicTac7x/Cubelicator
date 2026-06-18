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

        public const string MappedButtonRow_DisableTriggerOnClick = "Disable trigger on click";

        public const string PortTile_Edit = "Edit";

        public const string ProfileSelector_New = "New";
        public const string ProfileSelector_Rename = "Rename";
        public const string ProfileSelector_Delete = "Delete";
        public const string ProfileSelector_EnableRumble = "Enable rumble";
        public const string ProfileSelector_DisableRumble = "Disable rumble";

        public const string Navigator_Dashboard = "DASHBOARD";
        public const string Navigator_Controller1 = "CONTROLLER 1";
        public const string Navigator_Controller2 = "CONTROLLER 2";
        public const string Navigator_Controller3 = "CONTROLLER 3";
        public const string Navigator_Controller4 = "CONTROLLER 4";
        public const string Navigator_Calibration = "CALIBRATION";

        public const string Calibrator_Description =
        """
        Rotate both sticks in full circles and squeeze each trigger several times,
        until the calibration numbers no longer change.
        """;
        public const string Calibrator_FinishCalibration = "Finish Calibration";
    }
}
