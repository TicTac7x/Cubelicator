using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cubelicator.Services
{
    public class SettingsManager
    {
        private readonly Settings settings;

        private readonly string settingsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Cubelicator",
            "Settings.json");

        public Settings Settings { get => settings; }

        public SettingsManager(ProfileManager profileManager) {
            settings = new Settings(profileManager);
        }

        public void Start()
        {
            LoadSettings();
            settings.OnSettingsChanged += () =>
            {
                Save();
            };
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(
                settings,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters =
                    {
                new JsonStringEnumConverter()
                    }
                });

            File.WriteAllText(settingsFile, json);
        }

        private void LoadSettings()
        {
            if (!File.Exists(settingsFile))
            {
                Save();
                return;
            }

            string json = File.ReadAllText(settingsFile);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Settings.Controller1Color =
                Enum.Parse<ControllerColor>(root.GetProperty("Controller1Color").GetString()!);

            Settings.Controller2Color =
                Enum.Parse<ControllerColor>(root.GetProperty("Controller2Color").GetString()!);

            Settings.Controller3Color =
                Enum.Parse<ControllerColor>(root.GetProperty("Controller3Color").GetString()!);

            Settings.Controller4Color =
                Enum.Parse<ControllerColor>(root.GetProperty("Controller4Color").GetString()!);

            Settings.Controller1Profile =
                root.GetProperty("Controller1Profile").GetString() ?? "Default";

            Settings.Controller2Profile =
                root.GetProperty("Controller2Profile").GetString() ?? "Default";

            Settings.Controller3Profile =
                root.GetProperty("Controller3Profile").GetString() ?? "Default";

            Settings.Controller4Profile =
                root.GetProperty("Controller4Profile").GetString() ?? "Default";
        }
    }
}