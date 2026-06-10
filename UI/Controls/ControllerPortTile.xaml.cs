using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace Cubelicator.UI.Controls
{
    public sealed partial class ControllerPortTile : UserControl
    {

        public ControllerPortTile(int port)
        {
            InitializeComponent();
            SetPort(port);
        }

        public void SetControllerConnected(bool connected)
        {
            var portVisible = connected ? Visibility.Collapsed : Visibility.Visible;
            var controllerVisible = connected ? Visibility.Visible : Visibility.Collapsed;

            DispatcherQueue.TryEnqueue(() =>
            {
                Port.Visibility = portVisible;
                PortText.Visibility = portVisible;
                Controller.Visibility = controllerVisible;
                ControllerText.Visibility = controllerVisible;
                IconNotCalibrated.Visibility = controllerVisible;
            });
        }

        public void SetControllerCalibration(GamecubeControllerCalibration calibration)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                IconNotCalibrated.Visibility = Visibility.Collapsed;
                IconCalibrated.Visibility = Visibility.Visible;

                ToolTipService.SetToolTip(
                    IconCalibrated,
                    $"""
                    {Strings.TooltipControllerIsCalibrated}

                    Left Stick X: {calibration.LeftStickXMin} / {calibration.LeftStickXCenter} / {calibration.LeftStickXMax}
                    Left Stick Y: {calibration.LeftStickYMin} / {calibration.LeftStickYCenter} / {calibration.LeftStickYMax}
                    Right Stick X: {calibration.RightStickXMin} / {calibration.RightStickXCenter} / {calibration.RightStickXMax}
                    Right Stick Y: {calibration.RightStickYMin} / {calibration.RightStickYCenter} / {calibration.RightStickYMax}
                    Left Trigger: {calibration.LeftTriggerMin} / {calibration.LeftTriggerMax}
                    Right Trigger: {calibration.RightTriggerMin} / {calibration.RightTriggerMax}
                    """
                );
            });
        }

        private void SetPort(int port)
        {
            PortText.Text = PortText.Text + " " + port;
            ControllerText.Text = ControllerText.Text + " " + port;
        }

        public string TooltipControllerIsNotCalibrated => Strings.TooltipControllerIsNotCalibrated;
        public string TooltipControllerIsCalibrated => Strings.TooltipControllerIsCalibrated;

        private void DetailsButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void DetailsButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }
    }
}