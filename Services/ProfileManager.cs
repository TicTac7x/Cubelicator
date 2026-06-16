using System.Text.Json;
using System.Text.Json.Serialization;

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
        public IReadOnlyList<GamecubeControllerProfile> Profiles => profiles;

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
            SaveProfile(defaultProfile);
        }

        private void SaveProfile(GamecubeControllerProfile profile)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            string json = JsonSerializer.Serialize(profile, options);
            File.WriteAllText(Path.Combine(directoryProfiles, profile.Name + ".json"), json);
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
                    profile.Name = Path.GetFileNameWithoutExtension(file);
                    SetupProfileEvents(profile);
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

        public GamecubeControllerProfile NewProfile()
        {
            int index = 1;

            string name;

            do
            {
                name = $"New{index}";
                index++;
            }
            while (profiles.Any(p => p.Name == name));

            var profile = new GamecubeControllerProfile
            {
                Name = name
            };
            SetupProfileEvents(profile);
            profiles.Add(profile);

            SaveProfile(profile);
            OnProfilesChanged(profiles);

            return profile;
        }

        private void SetupProfileEvents(GamecubeControllerProfile profile)
        {
            profile.OnChanged += () =>
            {
                SaveProfile(profile);
            };

            profile.OnNameChanged += (oldName, newName) =>
            {
                string oldPath = Path.Combine(directoryProfiles, oldName + ".json");

                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
            };

            profile.OnDelete += () =>
            {
                if (profiles.Count <= 1) return;
                DeleteProfile(profile);
            };
        }

        private void DeleteProfile(GamecubeControllerProfile profile)
        {
            if (!profiles.Contains(profile))
                return;

            profiles.Remove(profile);

            string path = Path.Combine(directoryProfiles, profile.Name + ".json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            OnProfilesChanged(profiles);
        }
    }
}
