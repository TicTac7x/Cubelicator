using Cubelicator.Services;

namespace Cubelicator
{
    public class Settings
    {
        public event Action<ControllerColor> OnController1ColorChanged = delegate { };
        public event Action<ControllerColor> OnController2ColorChanged = delegate { };
        public event Action<ControllerColor> OnController3ColorChanged = delegate { };
        public event Action<ControllerColor> OnController4ColorChanged = delegate { };
        public event Action<AdapterPort, ControllerColor> OnControllerColorChanged = delegate { };

        public event Action<GamecubeControllerProfile> OnController1ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> OnController2ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> OnController3ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> OnController4ProfileChanged = delegate { };
        public event Action<AdapterPort, GamecubeControllerProfile> OnControllerProfileChanged = delegate { };

        public event Action OnSettingsChanged = delegate { };

        private ControllerColor controller1Color = ControllerColor.Indigo;
        private ControllerColor controller2Color = ControllerColor.Indigo;
        private ControllerColor controller3Color = ControllerColor.Indigo;
        private ControllerColor controller4Color = ControllerColor.Indigo;

        private GamecubeControllerProfile controller1Profile = new GamecubeControllerProfile();
        private GamecubeControllerProfile controller2Profile = new GamecubeControllerProfile();
        private GamecubeControllerProfile controller3Profile = new GamecubeControllerProfile();
        private GamecubeControllerProfile controller4Profile = new GamecubeControllerProfile();

        public void SetControllerColor(AdapterPort port, ControllerColor color)
        {
            switch(port)
            {
                case AdapterPort.One:
                    Controller1Color = color;
                    break;
                case AdapterPort.Two:
                    Controller2Color = color;
                    break;
                case AdapterPort.Three:
                    Controller3Color = color;
                    break;
                case AdapterPort.Four:
                    Controller4Color = color;
                    break;
            }
        }

        public void SetControllerProfile(AdapterPort port, GamecubeControllerProfile profile)
        {
            switch (port)
            {
                case AdapterPort.One:
                    Controller1Profile = profile;
                    break;
                case AdapterPort.Two:
                    Controller2Profile = profile;
                    break;
                case AdapterPort.Three:
                    Controller3Profile = profile;
                    break;
                case AdapterPort.Four:
                    Controller4Profile = profile;
                    break;
            }
        }

        public ControllerColor Controller1Color
        {
            get => controller1Color;
            set
            {
                controller1Color = value;
                OnController1ColorChanged(value);
                OnControllerColorChanged(AdapterPort.One, value);
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
                OnControllerColorChanged(AdapterPort.Two, value);
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
                OnControllerColorChanged(AdapterPort.Three, value);
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
                OnControllerColorChanged(AdapterPort.Four, value);
                OnSettingsChanged();
            }
        }

        public GamecubeControllerProfile Controller1Profile
        {
            get => controller1Profile;
            set
            {
                controller1Profile = value;
                OnController1ProfileChanged(value);
                OnControllerProfileChanged(AdapterPort.One, value);
                OnSettingsChanged();
            }
        }

        public GamecubeControllerProfile Controller2Profile
        {
            get => controller2Profile;
            set
            {
                controller2Profile = value;
                OnController2ProfileChanged(value);
                OnControllerProfileChanged(AdapterPort.Two, value);
                OnSettingsChanged();
            }
        }

        public GamecubeControllerProfile Controller3Profile
        {
            get => controller3Profile;
            set
            {
                controller3Profile = value;
                OnController3ProfileChanged(value);
                OnControllerProfileChanged(AdapterPort.Three, value);
                OnSettingsChanged();
            }
        }

        public GamecubeControllerProfile Controller4Profile
        {
            get => controller4Profile;
            set
            {
                controller4Profile = value;
                OnController4ProfileChanged(value);
                OnControllerProfileChanged(AdapterPort.Four, value);
                OnSettingsChanged();
            }
        }
    }
}