using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace Cubelicator
{
    public partial class CalibrationIcon : UserControl
    {
        private readonly AdapterPort port;
        private readonly GamecubeController gamecubeController;

        public CalibrationIcon(AdapterPort port, GamecubeController gamecubeController)
        {
            this.port = port;
            this.gamecubeController = gamecubeController;

            InitializeComponent();
            InitializeUI();
            SetupEvents();
        }

        private void InitializeUI()
        {
            // Port not calibrated tooltip.
            ToolTipService.SetToolTip(Element_NotCalibrated, Strings.TooltipPortIsNotCalibrated(port));
            SetCalibration(gamecubeController.Calibration);
        }

        private void SetupEvents()
        {
            gamecubeController.Event_CalibrationChanged += (calibration) =>
            {
                SetCalibration(calibration);
            };
        }

        private void SetCalibration(GamecubeControllerCalibration? calibration)
        {
            Element_Calibrated.Visibility = calibration != null ? Visibility.Visible : Visibility.Collapsed;
            Element_NotCalibrated.Visibility = calibration == null ? Visibility.Visible : Visibility.Collapsed;

            if (calibration != null)
            {
                ToolTipService.SetToolTip(
                    Element_Calibrated,
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
            }
        }
    }
}