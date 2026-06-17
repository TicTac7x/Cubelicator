using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator
{
    public partial class View_Calibrator : UserControl
    {
        private readonly Settings settings;
        private readonly ViewsManager viewsManager;
        private readonly CalibrationManager calibrationManager;

        private AdapterPort? port;
        private GamecubeController? controller;
        private GamecubeControllerCalibration? calibration;

        public View_Calibrator(ViewsManager viewsManager, CalibrationManager calibrationManager, Settings settings)
        {
            this.settings = settings;
            this.viewsManager = viewsManager;
            this.calibrationManager = calibrationManager;
            InitializeComponent();
        }

        public void StartCalibration(AdapterPort port, GamecubeController gamecubeController)
        {
            this.port = port;
            controller = gamecubeController;
            calibration = new GamecubeControllerCalibration
            {
                LeftStickXMin = Constants.GamecubeControllerStickRange,
                LeftStickXCenter = gamecubeController.GetStickValue(GamecubeControllerStick.LeftStick, Axis.X),
                LeftStickXMax = -Constants.GamecubeControllerStickRange,

                LeftStickYMin = Constants.GamecubeControllerStickRange,
                LeftStickYCenter = gamecubeController.GetStickValue(GamecubeControllerStick.LeftStick, Axis.Y),
                LeftStickYMax = -Constants.GamecubeControllerStickRange,

                RightStickXMin = Constants.GamecubeControllerStickRange,
                RightStickXCenter = gamecubeController.GetStickValue(GamecubeControllerStick.RightStick, Axis.X),
                RightStickXMax = -Constants.GamecubeControllerStickRange,

                RightStickYMin = Constants.GamecubeControllerStickRange,
                RightStickYCenter = gamecubeController.GetStickValue(GamecubeControllerStick.RightStick, Axis.Y),
                RightStickYMax = -Constants.GamecubeControllerStickRange,

                LeftTriggerMin = Constants.GamecubeControllerTriggerRange,
                LeftTriggerMax = 0,

                RightTriggerMin = Constants.GamecubeControllerTriggerRange,
                RightTriggerMax = 0,
            };

            controller.Event_StickChanged += (stick, axis, value) =>
            {
                if (calibration == null)
                    return;

                switch (stick)
                {
                    case GamecubeControllerStick.LeftStick:
                        if (axis == Axis.X)
                        {
                            calibration.LeftStickXMin = Math.Min(calibration.LeftStickXMin, value);
                            calibration.LeftStickXMax = Math.Max(calibration.LeftStickXMax, value);
                        }
                        else
                        {
                            calibration.LeftStickYMin = Math.Min(calibration.LeftStickYMin, value);
                            calibration.LeftStickYMax = Math.Max(calibration.LeftStickYMax, value);
                        }
                        break;

                    case GamecubeControllerStick.RightStick:
                        if (axis == Axis.X)
                        {
                            calibration.RightStickXMin = Math.Min(calibration.RightStickXMin, value);
                            calibration.RightStickXMax = Math.Max(calibration.RightStickXMax, value);
                        }
                        else
                        {
                            calibration.RightStickYMin = Math.Min(calibration.RightStickYMin, value);
                            calibration.RightStickYMax = Math.Max(calibration.RightStickYMax, value);
                        }
                        break;
                }

                UpdateCalibration();
            };

            controller.Event_TriggerChanged += (trigger, value) =>
            {
                if (calibration == null)
                    return;

                switch (trigger)
                {
                    case GamecubeControllerTrigger.LeftTrigger:
                        calibration.LeftTriggerMin = Math.Min(calibration.LeftTriggerMin, value);
                        calibration.LeftTriggerMax = Math.Max(calibration.LeftTriggerMax, value);
                        break;

                    case GamecubeControllerTrigger.RightTrigger:
                        calibration.RightTriggerMin = Math.Min(calibration.RightTriggerMin, value);
                        calibration.RightTriggerMax = Math.Max(calibration.RightTriggerMax, value);
                        break;
                }

                UpdateCalibration();
            };

            Element_Controller.Children.Clear();
            Element_Controller.Children.Add(new GamecubeControllerView(port, gamecubeController, settings));
        }

        private void UpdateCalibration()
        {
            if (calibration == null)
                return;

            DispatcherQueue.TryEnqueue(() =>
            {
                Element_Calibration.Text =
                $"""
                Left Stick X: {calibration.LeftStickXMin} / {calibration.LeftStickXCenter} / {calibration.LeftStickXMax}
                Left Stick Y: {calibration.LeftStickYMin} / {calibration.LeftStickYCenter} / {calibration.LeftStickYMax}

                Right Stick X: {calibration.RightStickXMin} / {calibration.RightStickXCenter} / {calibration.RightStickXMax}
                Right Stick Y: {calibration.RightStickYMin} / {calibration.RightStickYCenter} / {calibration.RightStickYMax}

                Left Trigger: {calibration.LeftTriggerMin} / {calibration.LeftTriggerMax}
                Right Trigger: {calibration.RightTriggerMin} / {calibration.RightTriggerMax}
                """;
            });
        }

        private void Click_FinishCalibration(object sender, RoutedEventArgs args)
        {
            if (controller is { } controllerLocal &&
                calibration is { } calibrationLocal &&
                port is { } portLocal)
            {
                controllerLocal.SetCalibration(calibrationLocal);
                viewsManager.ShowDashboard();
                calibrationManager.SaveCalibration(portLocal, calibrationLocal);
            }
        }
    }
}
