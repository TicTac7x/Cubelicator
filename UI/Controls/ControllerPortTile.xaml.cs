using Cubelicator.Services;
using Cubelicator.UI.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controls
{
    public sealed partial class ControllerPortTile : UserControl
    {
        private readonly int port;
        private readonly GamecubeController gamecubeController;
        private readonly Settings settings;
        private readonly CalibrationManager calibrationManager;
        private readonly GamecubeControllerView gamecubeControllerView;

        public ControllerPortTile(int port, GamecubeController gamecubeController, Settings settings, CalibrationManager calibrationManager)
        {
            this.port = port;
            this.gamecubeController = gamecubeController;
            this.settings = settings;
            this.calibrationManager = calibrationManager;
            this.gamecubeControllerView = new GamecubeControllerView(gamecubeController);

            InitializeComponent();
            ControllerRoot.Children.Add(gamecubeControllerView);
            PortText.Text = PortText.Text + " " + port;
            ControllerText.Text = ControllerText.Text + " " + port;
            ToolTipService.SetToolTip(IconNotCalibrated, Strings.TooltipPortIsNotCalibrated(port));

            SetupEventListeners();
        }

        private void SetupEventListeners()
        {
            gamecubeController.OnConnectionChanged += (connected) =>
            {
                var portVisible = connected ? Visibility.Collapsed : Visibility.Visible;
                var controllerVisible = connected ? Visibility.Visible : Visibility.Collapsed;

                DispatcherQueue.TryEnqueue(() =>
                {
                    Port.Visibility = portVisible;
                    PortText.Visibility = portVisible;
                    ControllerRoot.Visibility = controllerVisible;
                    ControllerText.Visibility = controllerVisible;

                    if (connected)
                    {
                        var calibrated = gamecubeController.IsCalibrated();
                        IconCalibrated.Visibility = calibrated ? Visibility.Visible : Visibility.Collapsed;
                        IconNotCalibrated.Visibility = calibrated ? Visibility.Collapsed : Visibility.Visible;
                    }
                    else
                    {
                        IconCalibrated.Visibility = Visibility.Collapsed;
                        IconNotCalibrated.Visibility = Visibility.Collapsed;
                    }
                });
            };

            gamecubeController.OnCalibrationChanged += (calibration) =>
            {
                if (!gamecubeController.IsConnected()) return;

                DispatcherQueue.TryEnqueue(() =>
                {
                    IconNotCalibrated.Visibility = Visibility.Collapsed;
                    IconCalibrated.Visibility = Visibility.Visible;

                    ToolTipService.SetToolTip(
                        IconCalibrated,
                        $"""
                        {Strings.TooltipPortIsCalibrated(port)}

                        Left Stick X: {calibration.LeftStickXMin} / {calibration.LeftStickXCenter} / {calibration.LeftStickXMax}
                        Left Stick Y: {calibration.LeftStickYMin} / {calibration.LeftStickYCenter} / {calibration.LeftStickYMax}
                        Right Stick X: {calibration.RightStickXMin} / {calibration.RightStickXCenter} / {calibration.RightStickXMax}
                        Right Stick Y: {calibration.RightStickYMin} / {calibration.RightStickYCenter} / {calibration.RightStickYMax}
                        Left Trigger: {calibration.LeftTriggerMin} / {calibration.LeftTriggerMax}
                        Right Trigger: {calibration.RightTriggerMin} / {calibration.RightTriggerMax}
                        """
                    );
                });
            };

            settings.OnControllerColorChanged += (port, color) =>
            {
                if (port == this.port)
                {
                    SetControllerColor(color);
                }
            };
        }

        private void OnMenuItemClickCalibrate(object sender, RoutedEventArgs e)
        {
            var calibrationWindow = new CalibrationWindow(port, calibrationManager);
            calibrationWindow.Activate();
        }

        public void SetControllerColor(string color)
        {
            gamecubeControllerView.SetControllerColor(color);
        }

        private void OnMenuItemChangeColor(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuFlyoutItem item)
                return;

            if (item.Tag is not string colorName)
                return;

            if (!Enum.TryParse<ControllerColor>(colorName, out var colorEnum))
                return;

            if (!ControllerColors.Map.TryGetValue(colorEnum, out var colorValue))
                return;

            switch (port)
            {
                case 1:
                    settings.Controller1Color = colorValue;
                    break;
                case 2:
                    settings.Controller2Color = colorValue;
                    break;
                case 3:
                    settings.Controller3Color = colorValue;
                    break;
                case 4:
                    settings.Controller4Color = colorValue;
                    break;
            }
        }

        private string StringCalibrate => Strings.Calibrate;
        private string StringChangeColor => Strings.ChangeColor;
    }
}