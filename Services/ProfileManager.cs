using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cubelicator.Services
{
    public class ProfileManager
    {
        public event Action<List<GamecubeControllerProfile>> OnProfilesChanged = delegate { };

        private readonly string directoryProfiles = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Cubelicator",
            "Profiles");
        private readonly List<GamecubeControllerProfile> profiles = new List<GamecubeControllerProfile>();

        public ProfileManager() {
            Directory.CreateDirectory(directoryProfiles);
            CreateDefaultProfileIfDoesntExist();
        }

        private void CreateDefaultProfileIfDoesntExist()
        {
            var files = Directory.GetFiles(directoryProfiles, "*.json");

            if (files.Length > 0)
                return;

            var defaultProfile = new GamecubeControllerProfile();

            string filePath = Path.Combine(directoryProfiles, defaultProfile.Name + ".json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

            string json = JsonSerializer.Serialize(defaultProfile, options);

            File.WriteAllText(filePath, json);
        }

        public void Start()
        {
            var files = Directory.GetFiles(directoryProfiles, "*.json");

            foreach (var file in files)
            {
                string json = File.ReadAllText(file);
                var profile = JsonSerializer.Deserialize<GamecubeControllerProfile>(json, new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter()
                    }
                });

                if (profile != null)
                {
                    profiles.Add(profile);
                }
            }

            OnProfilesChanged(profiles);
        }

        public GamecubeControllerProfile? GetProfile(string profileName)
        {
            foreach (var profile in profiles)
            {
                if (profile.Name == profileName)
                {
                    return profile;
                }
            }

            return null;
        }
    }
}
