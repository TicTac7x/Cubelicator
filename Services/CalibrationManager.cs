using System.Text.Json;

namespace Cubelicator.Services
{
    public class CalibrationManager
    {
        public event Action<int, GamecubeControllerCalibration> OnCalibrationLoaded = delegate { };

        private readonly string _basePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Cubelicator",
            "Calibrations");

        public CalibrationManager()
        {
            Directory.CreateDirectory(_basePath);
        }

        public void Start()
        {
            for (int port = 1; port <= 4; port++)
            {
                LoadCalibration(port);
            }
        }

        private void LoadCalibration(int port)
        {
            try
            {
                var file = Path.Combine(_basePath, $"Port{port}.json");

                if (!File.Exists(file)) return;

                var json = File.ReadAllText(file);
                var calibration = JsonSerializer.Deserialize<GamecubeControllerCalibration>(json);

                if (calibration == null) return;

                OnCalibrationLoaded.Invoke(port, calibration);
            }
            catch
            {
                return;
            }
        }
    }
}