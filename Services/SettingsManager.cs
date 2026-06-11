using System.Text.Json;

namespace Cubelicator.Services
{
    public class SettingsManager
    {
        public Settings Settings { get; private set; } = new Settings();

        private readonly string settingsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Cubelicator",
            "Settings.json");

        public SettingsManager() { }

        public void Start()
        {
            LoadSettings();
            Settings.OnSettingsChanged += () =>
            {
                Save();
            };
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(
                Settings,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(settingsFile, json);
        }

        private void LoadSettings()
        {
            try
            {
                // Save default settings to file
                if (!File.Exists(settingsFile))
                {
                    SaveSettings(this.Settings);
                    return;
                }

                string json = File.ReadAllText(settingsFile);
                var settings = JsonSerializer.Deserialize<Settings>(json);

                if (settings != null)
                {
                    Settings.Controller1Color = settings.Controller1Color;
                    Settings.Controller2Color = settings.Controller2Color;
                    Settings.Controller3Color = settings.Controller3Color;
                    Settings.Controller4Color = settings.Controller4Color;
                }
            }
            catch { }
        }

        private void SaveSettings(Settings settings)
        {
            string json = JsonSerializer.Serialize(
                settings,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(settingsFile, json);
        }
    }
}