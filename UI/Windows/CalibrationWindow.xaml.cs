using Cubelicator.Services;
using Microsoft.UI.Xaml;

namespace Cubelicator.UI.Windows
{
    public partial class CalibrationWindow : Window
    {
        private readonly int _port;
        private readonly CalibrationManager _calibrationManager;

        public CalibrationWindow(
            int port,
            CalibrationManager calibrationManager)
        {
            InitializeComponent();

            _port = port;
            _calibrationManager = calibrationManager;

            Title = $"Calibrate Port {port}";
        }
    }
}
