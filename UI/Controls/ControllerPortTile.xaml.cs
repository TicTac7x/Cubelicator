using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Text.RegularExpressions;

namespace Cubelicator
{
    public partial class ControllerPortTile : UserControl
    {
        private readonly AdapterPort port;
        private readonly GamecubeController gamecubeController;
        private readonly Settings settings;
        private readonly CalibrationManager calibrationManager;
        private readonly ProfileManager profileManager;
        private readonly GamecubeControllerView gamecubeControllerView;
        private readonly ViewsManager viewsManager;

        public ControllerPortTile(AdapterPort port, GamecubeController gamecubeController, Settings settings, CalibrationManager calibrationManager, ProfileManager profileManager, ViewsManager viewsManager)
        {
            this.port = port;
            this.gamecubeController = gamecubeController;
            this.settings = settings;
            this.calibrationManager = calibrationManager;
            this.profileManager = profileManager;
            this.gamecubeControllerView = new GamecubeControllerView(gamecubeController);
            this.viewsManager = viewsManager;

            InitializeComponent();
            InitializeControllerPortTile();
            SetupEventListeners();
        }

        void InitializeControllerPortTile()
        {
            // Port not calibrated tooltip.
            ToolTipService.SetToolTip(IconNotCalibrated, Strings.TooltipPortIsNotCalibrated(port));

            // Populate list of controller colors.
            foreach (var controllerColor in ControllerColors.Map.Keys)
            {
                var item = new MenuFlyoutItem
                {
                    Text = Regex.Replace(
                    controllerColor.ToString(),
                    "(\\B[A-Z])",
                    " $1"),
                    Tag = controllerColor.ToString()
                };

                item.Click += OnMenuItemChangeColor;

                DynamicControllerColors.Items.Add(item);
            }


            // Gamecube controller view.
            ControllerRoot.Children.Add(gamecubeControllerView);

            // Controller and port texts.
            PortText.Text = PortText.Text + " " + (int) port;
            ControllerText.Text = ControllerText.Text + " " + (int) port;

            RootProfileSelector.Children.Add(new ProfileSelector(profileManager, gamecubeController, false));
        }

        private void SetupEventListeners()
        {
            gamecubeController.OnConnectionChanged += (connected) =>
            {
                var portVisible = connected ? Visibility.Collapsed : Visibility.Visible;
                var controllerVisible = connected ? Visibility.Visible : Visibility.Collapsed;

                DispatcherQueue.TryEnqueue(() =>
                {
                    Root.Opacity = connected ? 1 : 0.4;
                    Port.Visibility = portVisible;
                    PortText.Visibility = portVisible;
                    ControllerRoot.Visibility = controllerVisible;
                    ControllerText.Visibility = controllerVisible;
                    RootProfileSelector.Visibility = controllerVisible;
                    DetailsButton.Visibility = controllerVisible;

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

            settings.OnControllerColorChanged += (port, controllerColor) =>
            {
                if (port == this.port)
                {
                    SetControllerColor(ControllerColors.Map[controllerColor]);
                }
            };
        }

        private void OnMenuItemClickCalibrate(object sender, RoutedEventArgs e)
        {
            var calibrationWindow = new CalibrationWindow(port, calibrationManager);
            calibrationWindow.Activate();
        }

        private void OnMenuItemChangeColor(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuFlyoutItem item)
                return;

            if (item.Tag is not string colorName)
                return;

            if (!Enum.TryParse<ControllerColor>(colorName, out var colorEnum))
                return;

            settings.SetControllerColor(port, colorEnum);
        }

        private void OnMenuItemEdit(object sender, RoutedEventArgs e)
        {
            viewsManager.ShowControllerEditor(port, gamecubeController);
        }

        private void OnMenuItemChangeProfile(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuFlyoutItem item)
                return;

            if (item.Tag is not string profileName)
                return;

            var profile = profileManager.GetProfile(profileName);
            if (profile == null)
                return;

            settings.SetControllerProfile(port, profile);
        }

        public void SetControllerColor(string color)
        {
            gamecubeControllerView.SetControllerColor(color);
        }
    }
}