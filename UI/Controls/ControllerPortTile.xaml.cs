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
            this.gamecubeControllerView = new GamecubeControllerView(port, gamecubeController, settings);
            this.viewsManager = viewsManager;

            InitializeComponent();
            InitializeControllerPortTile();
            SetupEventListeners();
        }

        void InitializeControllerPortTile()
        {
            Element_CalibrationIcon.Children.Add(new CalibrationIcon(port, gamecubeController));

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

                Element_ControllerColors.Items.Add(item);
            }

            // Gamecube controller view.
            Element_Controller.Children.Add(gamecubeControllerView);

            // Controller and port texts.
            Element_PortText.Text = Element_PortText.Text + " " + (int) port;
            Element_ControllerText.Text = Element_ControllerText.Text + " " + (int) port;

            Element_ProfileSelector.Children.Add(new ProfileSelector(port, settings, viewsManager, profileManager, gamecubeController, false));
        }

        private void SetupEventListeners()
        {
            gamecubeController.Event_ConnectionChanged += (connected) =>
            {
                var portVisible = connected ? Visibility.Collapsed : Visibility.Visible;
                var controllerVisible = connected ? Visibility.Visible : Visibility.Collapsed;

                DispatcherQueue.TryEnqueue(() =>
                {
                    Element_Root.Opacity = connected ? 1 : 0.4;
                    Element_Port.Visibility = portVisible;
                    Element_PortText.Visibility = portVisible;
                    Element_Controller.Visibility = controllerVisible;
                    Element_ControllerText.Visibility = controllerVisible;
                    Element_ProfileSelector.Visibility = controllerVisible;
                    Element_DetailsButton.Visibility = controllerVisible;
                    Element_CalibrationIcon.Visibility = controllerVisible;
                });
            };
        }

        private void OnMenuItemClickCalibrate(object sender, RoutedEventArgs e)
        {
            viewsManager.ShowCalibration(port, gamecubeController);
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
    }
}