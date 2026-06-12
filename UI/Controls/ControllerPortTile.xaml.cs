using Cubelicator.Services;
using Cubelicator.UI.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controls
{
    public sealed partial class ControllerPortTile : UserControl
    {
        private readonly int port;
        private readonly Cubelicator.GamecubeController gamecubeController;
        private readonly Settings settings;
        private readonly CalibrationManager calibrationManager;

        public ControllerPortTile(int port, Cubelicator.GamecubeController gamecubeController, Settings settings, CalibrationManager calibrationManager)
        {
            this.port = port;
            this.gamecubeController = gamecubeController;
            this.settings = settings;
            this.calibrationManager = calibrationManager;

            InitializeComponent();
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
                    Controller.Visibility = controllerVisible;
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
            GamecubeController.SetControllerColor(color);
        }

        private void OnMenuItemChangeColor(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuFlyoutItem item)
                return;

            string? colorName = item.Tag?.ToString();

            string? colorValue = colorName switch
            {
                nameof(Colors.Indigo) => Colors.Indigo,
                nameof(Colors.JetBlack) => Colors.JetBlack,
                nameof(Colors.SpiceOrange) => Colors.SpiceOrange,
                nameof(Colors.Platinum) => Colors.Platinum,
                nameof(Colors.EmeraldBlue) => Colors.EmeraldBlue,
                nameof(Colors.White) => Colors.White,
                nameof(Colors.StarlightGold) => Colors.StarlightGold,
                nameof(Colors.SymphonicGreen) => Colors.SymphonicGreen,
                nameof(Colors.LuigiGreen) => Colors.LuigiGreen,
                nameof(Colors.MarioRed) => Colors.MarioRed,
                nameof(Colors.WarioYellow) => Colors.WarioYellow,
                nameof(Colors.GundamChar) => Colors.GundamChar,
                _ => null
            };

            if (colorValue != null)
            {
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
        }

        public string MenuCalibrate => Strings.Calibrate;
        public string MenuChangeColor => Strings.ChangeColor;
    }
}