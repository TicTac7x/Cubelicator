using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Cubelicator
{
    public class SettingsManager
    {
        private readonly ProfileManager profileManager;
        private readonly Settings settings = new Settings();

        private readonly string settingsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Cubelicator",
            "Settings.json");

        public Settings Settings { get => settings; }

        public SettingsManager(ProfileManager profileManager) {
            this.profileManager = profileManager;
        }

        public void Start()
        {
            LoadSettings();
            settings.Event_SettingsChanged += () =>
            {
                Save();
            };
        }

        public void Save()
        {
            var jsonObject = new JsonObject
            {
                ["Controller1Color"] = settings.Controller1Color.ToString(),
                ["Controller2Color"] = settings.Controller2Color.ToString(),
                ["Controller3Color"] = settings.Controller3Color.ToString(),
                ["Controller4Color"] = settings.Controller4Color.ToString(),

                ["Controller1Profile"] = settings.Controller1Profile.Name,
                ["Controller2Profile"] = settings.Controller2Profile.Name,
                ["Controller3Profile"] = settings.Controller3Profile.Name,
                ["Controller4Profile"] = settings.Controller4Profile.Name
            };

            string json = jsonObject.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true
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

            var profile1 = profileManager.GetProfile(root.GetProperty("Controller1Profile").GetString() ?? "Default");
            if (profile1 != null)
            {
                Settings.Controller1Profile = profile1;
            }

            var profile2 = profileManager.GetProfile(root.GetProperty("Controller2Profile").GetString() ?? "Default");
            if (profile2 != null)
            {
                Settings.Controller2Profile = profile2;
            }

            var profile3 = profileManager.GetProfile(root.GetProperty("Controller3Profile").GetString() ?? "Default");
            if (profile3 != null)
            {
                Settings.Controller3Profile = profile3;
            }

            var profile4 = profileManager.GetProfile(root.GetProperty("Controller4Profile").GetString() ?? "Default");
            if (profile4 != null)
            {
                Settings.Controller4Profile = profile4;
            }
        }
    }
}