using Cubelicator.Services;
using System.Text.Json.Serialization;



namespace Cubelicator
{
    public class Settings
    {
        public static int a = 1;

        private readonly ProfileManager profileManager;

        private readonly int id = a++;

        public event Action<ControllerColor> OnController1ColorChanged = delegate { };
        public event Action<ControllerColor> OnController2ColorChanged = delegate { };
        public event Action<ControllerColor> OnController3ColorChanged = delegate { };
        public event Action<ControllerColor> OnController4ColorChanged = delegate { };
        public event Action<int, ControllerColor> OnControllerColorChanged = delegate { };

        public event Action<GamecubeControllerProfile> OnController1ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> OnController2ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> OnController3ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> OnController4ProfileChanged = delegate { };
        public event Action<int, GamecubeControllerProfile> OnControllerProfileChanged = delegate { };

        public event Action OnSettingsChanged = delegate { };

        private ControllerColor controller1Color = ControllerColor.Indigo;
        private ControllerColor controller2Color = ControllerColor.Indigo;
        private ControllerColor controller3Color = ControllerColor.Indigo;
        private ControllerColor controller4Color = ControllerColor.Indigo;

        private string controller1Profile = "Default";
        private string controller2Profile = "Default";
        private string controller3Profile = "Default";
        private string controller4Profile = "Default";

        public Settings(ProfileManager profileManager)
        {
            this.profileManager = profileManager;
        }

        public ControllerColor Controller1Color
        {
            get => controller1Color;
            set
            {
                controller1Color = value;
                OnController1ColorChanged(value);
                OnControllerColorChanged(1, value);
                OnSettingsChanged();
            }
        }

        public ControllerColor Controller2Color
        {
            get => controller2Color;
            set
            {
                controller2Color = value;
                OnController2ColorChanged(value);
                OnControllerColorChanged(2, value);
                OnSettingsChanged();
            }
        }

        public ControllerColor Controller3Color
        {
            get => controller3Color;
            set
            {
                controller3Color = value;
                OnController3ColorChanged(value);
                OnControllerColorChanged(3, value);
                OnSettingsChanged();
            }
        }

        public ControllerColor Controller4Color
        {
            get => controller4Color;
            set
            {
                controller4Color = value;
                OnController4ColorChanged(value);
                OnControllerColorChanged(4, value);
                OnSettingsChanged();
            }
        }

        public string Controller1Profile
        {
            get => controller1Profile;
            set
            {
                var profile = profileManager.GetProfile(value);
                controller1Profile = value;
                OnController1ProfileChanged(profile);
                OnControllerProfileChanged(1, profile);
                OnSettingsChanged();
            }
        }

        public string Controller2Profile
        {
            get => controller2Profile;
            set
            {
                var profile = profileManager.GetProfile(value);
                controller2Profile = value;
                OnController2ProfileChanged(profile);
                OnControllerProfileChanged(2, profile);
                OnSettingsChanged();
            }
        }

        public string Controller3Profile
        {
            get => controller3Profile;
            set
            {
                var profile = profileManager.GetProfile(value);
                controller3Profile = value;
                OnController3ProfileChanged(profile);
                OnControllerProfileChanged(3, profile);
                OnSettingsChanged();
            }
        }

        public string Controller4Profile
        {
            get => controller4Profile;
            set
            {
                var profile = profileManager.GetProfile(value);
                controller4Profile = value;
                OnController4ProfileChanged(profile);
                OnControllerProfileChanged(4, profile);
                OnSettingsChanged();
            }
        }
    }
}