namespace Cubelicator
{
    public class Settings
    {
        public event Action<ControllerColor> Event_Controller1ColorChanged = delegate { };
        public event Action<ControllerColor> Event_Controller2ColorChanged = delegate { };
        public event Action<ControllerColor> Event_Controller3ColorChanged = delegate { };
        public event Action<ControllerColor> Event_Controller4ColorChanged = delegate { };
        public event Action<AdapterPort, ControllerColor> Event_ControllerColorChanged = delegate { };

        public event Action<GamecubeControllerProfile> Event_Controller1ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> Event_Controller2ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> Event_Controller3ProfileChanged = delegate { };
        public event Action<GamecubeControllerProfile> Event_Controller4ProfileChanged = delegate { };
        public event Action<AdapterPort, GamecubeControllerProfile> Event_ControllerProfileChanged = delegate { };

        public event Action Event_SettingsChanged = delegate { };

        private ControllerColor controller1Color = ControllerColor.Indigo;
        private ControllerColor controller2Color = ControllerColor.Indigo;
        private ControllerColor controller3Color = ControllerColor.Indigo;
        private ControllerColor controller4Color = ControllerColor.Indigo;

        private GamecubeControllerProfile controller1Profile = new GamecubeControllerProfile();
        private GamecubeControllerProfile controller2Profile = new GamecubeControllerProfile();
        private GamecubeControllerProfile controller3Profile = new GamecubeControllerProfile();
        private GamecubeControllerProfile controller4Profile = new GamecubeControllerProfile();

        public ControllerColor GetControllerColor(AdapterPort port)
        {
            return port switch
            {
                AdapterPort.One => controller1Color,
                AdapterPort.Two => controller2Color,
                AdapterPort.Three => controller3Color,
                AdapterPort.Four => controller4Color,
            };
        }

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
                Event_Controller1ColorChanged(value);
                Event_ControllerColorChanged(AdapterPort.One, value);
                Event_SettingsChanged();
            }
        }

        public ControllerColor Controller2Color
        {
            get => controller2Color;
            set
            {
                controller2Color = value;
                Event_Controller2ColorChanged(value);
                Event_ControllerColorChanged(AdapterPort.Two, value);
                Event_SettingsChanged();
            }
        }

        public ControllerColor Controller3Color
        {
            get => controller3Color;
            set
            {
                controller3Color = value;
                Event_Controller3ColorChanged(value);
                Event_ControllerColorChanged(AdapterPort.Three, value);
                Event_SettingsChanged();
            }
        }

        public ControllerColor Controller4Color
        {
            get => controller4Color;
            set
            {
                controller4Color = value;
                Event_Controller4ColorChanged(value);
                Event_ControllerColorChanged(AdapterPort.Four, value);
                Event_SettingsChanged();
            }
        }

        public GamecubeControllerProfile Controller1Profile
        {
            get => controller1Profile;
            set
            {
                controller1Profile = value;
                Event_Controller1ProfileChanged(value);
                Event_ControllerProfileChanged(AdapterPort.One, value);
                Event_SettingsChanged();
            }
        }

        public GamecubeControllerProfile Controller2Profile
        {
            get => controller2Profile;
            set
            {
                controller2Profile = value;
                Event_Controller2ProfileChanged(value);
                Event_ControllerProfileChanged(AdapterPort.Two, value);
                Event_SettingsChanged();
            }
        }

        public GamecubeControllerProfile Controller3Profile
        {
            get => controller3Profile;
            set
            {
                controller3Profile = value;
                Event_Controller3ProfileChanged(value);
                Event_ControllerProfileChanged(AdapterPort.Three, value);
                Event_SettingsChanged();
            }
        }

        public GamecubeControllerProfile Controller4Profile
        {
            get => controller4Profile;
            set
            {
                controller4Profile = value;
                Event_Controller4ProfileChanged(value);
                Event_ControllerProfileChanged(AdapterPort.Four, value);
                Event_SettingsChanged();
            }
        }
    }
}