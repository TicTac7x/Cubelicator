using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace Cubelicator.UI.Controls
{
    public sealed partial class ControllerPortTile : UserControl
    {
        private readonly int port;
        private readonly Cubelicator.GamecubeController gamecubeController;
        public ControllerPortTile(int port, Cubelicator.GamecubeController gamecubeController)
        {
            this.port = port;
            this.gamecubeController = gamecubeController;

            InitializeComponent();
            PortText.Text = PortText.Text + " " + port;
            ControllerText.Text = ControllerText.Text + " " + port;
            ToolTipService.SetToolTip(IconNotCalibrated, Strings.tooltipPortIsNotCalibrated(port));

            setupEventListeners();
        }

        private void setupEventListeners()
        {
            gamecubeController.onConnectionChanged += (connected) =>
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
                        var calibrated = gamecubeController.isCalibrated();
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

            gamecubeController.onCalibrationChanged += (calibration) =>
            {
                if (!gamecubeController.isConnected()) return;

                DispatcherQueue.TryEnqueue(() =>
                {
                    IconNotCalibrated.Visibility = Visibility.Collapsed;
                    IconCalibrated.Visibility = Visibility.Visible;

                    ToolTipService.SetToolTip(
                        IconCalibrated,
                        $"""
                        {Strings.tooltipPortIsCalibrated(port)}

                        Left Stick X: {calibration.leftStickXMin} / {calibration.leftStickXCenter} / {calibration.leftStickXMax}
                        Left Stick Y: {calibration.leftStickYMin} / {calibration.leftStickYCenter} / {calibration.leftStickYMax}
                        Right Stick X: {calibration.rightStickXMin} / {calibration.rightStickXCenter} / {calibration.rightStickXMax}
                        Right Stick Y: {calibration.rightStickYMin} / {calibration.rightStickYCenter} / {calibration.rightStickYMax}
                        Left Trigger: {calibration.leftTriggerMin} / {calibration.leftTriggerMax}
                        Right Trigger: {calibration.rightTriggerMin} / {calibration.rightTriggerMax}
                        """
                    );
                });
            };
        }

        private void detailsButtonPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void detailsButtonPointerExited(object sender, PointerRoutedEventArgs e)
        {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }
    }
}