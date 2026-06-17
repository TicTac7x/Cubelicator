using System.Text.Json;

namespace Cubelicator
{
    public class CalibrationManager
    {
        public event Action<AdapterPort, GamecubeControllerCalibration> Event_PortControllerCalibrationLoaded = delegate { };

        private readonly string directoryCalibrations = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Cubelicator",
            "Calibrations");

        public CalibrationManager()
        {
            Directory.CreateDirectory(directoryCalibrations);
        }

        public void Start()
        {
            foreach (AdapterPort port in Enum.GetValues<AdapterPort>())
            {
                LoadCalibration(port);
            }
        }

        private void LoadCalibration(AdapterPort port)
        {
            try
            {
                var file = Path.Combine(directoryCalibrations, $"Port{(int)port}.json");

                if (!File.Exists(file)) return;

                var json = File.ReadAllText(file);
                var calibration = JsonSerializer.Deserialize<GamecubeControllerCalibration>(json);

                if (calibration == null) return;

                Event_PortControllerCalibrationLoaded(port, calibration);
            }
            catch
            {
                return;
            }
        }
    }
}