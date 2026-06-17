using Microsoft.UI.Xaml;

namespace Cubelicator
{
    public partial class CalibrationWindow : Window
    {
        private readonly AdapterPort port;
        private readonly CalibrationManager calibrationManager;

        public CalibrationWindow(
            AdapterPort port,
            CalibrationManager calibrationManager)
        {
            InitializeComponent();

            this.port = port;
            this.calibrationManager = calibrationManager;

            Title = $"Calibrate Port {port}";
        }
    }
}
