namespace Cubelicator
{
    public class Settings
    {
        public event Action<string> OnController1ColorChanged = delegate { };
        public event Action<string> OnController2ColorChanged = delegate { };
        public event Action<string> OnController3ColorChanged = delegate { };
        public event Action<string> OnController4ColorChanged = delegate { };
        public event Action OnSettingsChanged = delegate { };

        private string controller1Color = Colors.Indigo;
        private string controller2Color = Colors.Indigo;
        private string controller3Color = Colors.Indigo;
        private string controller4Color = Colors.Indigo;


        public string Controller1Color
        {
            get => controller1Color;
            set
            {
                controller1Color = value;
                OnController1ColorChanged(value);
                OnSettingsChanged();
            }
        }

        public string Controller2Color
        {
            get => controller2Color;
            set
            {
                controller2Color = value;
                OnController2ColorChanged(value);
                OnSettingsChanged();
            }
        }

        public string Controller3Color
        {
            get => controller3Color;
            set
            {
                controller3Color = value;
                OnController3ColorChanged(value);
                OnSettingsChanged();
            }
        }

        public string Controller4Color
        {
            get => controller4Color;
            set
            {
                controller4Color = value;
                OnController4ColorChanged(value);
                OnSettingsChanged();
            }
        }
    }
}