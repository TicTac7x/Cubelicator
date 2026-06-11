using System.Text.Json;

namespace Cubelicator.Services
{
    public class CalibrationManager
    {
        public event Action<int, GamecubeControllerCalibration> onPortControllerCalibrationLoaded = delegate { };

        private readonly string directoryCalibrations = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Cubelicator",
            "Calibrations");

        public CalibrationManager()
        {
            Directory.CreateDirectory(directoryCalibrations);
        }

        public void start()
        {
            for (int port = 1; port <= 4; port++)
            {
                loadCalibration(port);
            }
        }

        private void loadCalibration(int port)
        {
            try
            {
                var file = Path.Combine(directoryCalibrations, $"Port{port}.json");

                if (!File.Exists(file)) return;

                var json = File.ReadAllText(file);
                var calibration = JsonSerializer.Deserialize<GamecubeControllerCalibration>(json);

                if (calibration == null) return;

                onPortControllerCalibrationLoaded.Invoke(port, calibration);
            }
            catch
            {
                return;
            }
        }
    }
}