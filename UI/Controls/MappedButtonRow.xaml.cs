using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Cubelicator
{
    public partial class MappedButtonRow : UserControl
    {
        public event Action<bool> Event_ExpandedChanged = delegate { };
        private readonly GamecubeControllerInput gamecubeControllerInput;
        private XboxControllerInput xboxControllerInput;
        private readonly GamecubeController gamecubeController;
        private bool isExpanded = false;

        public MappedButtonRow(GamecubeController gamecubeController, GamecubeControllerInput gamecubeControllerInput, XboxControllerInput xboxControllerInput)
        {
            this.gamecubeController = gamecubeController;
            this.gamecubeControllerInput = gamecubeControllerInput;
            this.xboxControllerInput = xboxControllerInput;

            InitializeComponent();
            InitializeMenus();
            SetupEvents();

            var typeName = gamecubeControllerInput switch
            {
                GamecubeControllerButtonInput button =>
                    $"Cubelicator.{button.value}",

                GamecubeControllerStickInput stick =>
                    $"Cubelicator.{stick.value}",

                GamecubeControllerTriggerInput trigger =>
                    $"Cubelicator.{trigger.value}"
            };
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new Exception("UI.Controller.Button" + gamecubeControllerInput + " not found");
            }
            var control = Activator.CreateInstance(type) as Control;
            if (control == null)
            {
                throw new Exception("Failed to create gamecube button control");
            }

            Element_GamecubeButton.Children.Add(control);
            Element_GamecubeButtonName.Text = gamecubeControllerInput.ToString();
            SetXboxControllerButton(xboxControllerInput);
        }

        private void SetupEvents()
        {
            gamecubeController.Event_ProfileChanged += profile =>
            {
                XboxControllerInput xboxInput = gamecubeControllerInput switch
                {
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.A } => new XboxControllerButtonInput(profile.A),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.B } => new XboxControllerButtonInput(profile.B),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.X } => new XboxControllerButtonInput(profile.X),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.Y } => new XboxControllerButtonInput(profile.Y),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.Z } => new XboxControllerButtonInput(profile.Z),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.Start } => new XboxControllerButtonInput(profile.Start),

                    GamecubeControllerButtonInput { value: GamecubeControllerButton.DPadUp } => new XboxControllerButtonInput(profile.DPadUp),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.DPadDown } => new XboxControllerButtonInput(profile.DPadDown),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.DPadLeft } => new XboxControllerButtonInput(profile.DPadLeft),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.DPadRight } => new XboxControllerButtonInput(profile.DPadRight),

                    GamecubeControllerButtonInput { value: GamecubeControllerButton.LeftBumper } => new XboxControllerButtonInput(profile.LeftBumper),
                    GamecubeControllerButtonInput { value: GamecubeControllerButton.RightBumper } => new XboxControllerButtonInput(profile.RightBumper),

                    GamecubeControllerStickInput { value: GamecubeControllerStick.LeftStick } => new XboxControllerStickInput(profile.LeftStick),
                    GamecubeControllerStickInput { value: GamecubeControllerStick.RightStick } => new XboxControllerStickInput(profile.RightStick),

                    GamecubeControllerTriggerInput { value: GamecubeControllerTrigger.LeftTrigger } => new XboxControllerTriggerInput(profile.LeftTrigger),
                    GamecubeControllerTriggerInput { value: GamecubeControllerTrigger.RightTrigger } => new XboxControllerTriggerInput(profile.RightTrigger),
                };

                SetXboxControllerButton(xboxInput);
            };
        }

        public void SetExpandablePanelVisibility(bool visible)
        {
            isExpanded = visible;
            Element_ExpandablePanel.Visibility = isExpanded ? Visibility.Visible : Visibility.Collapsed;
            Element_Root.Background = App.StringToSolidColorBrush(isExpanded ? Colors.ButtonHoverBackground : Colors.ButtonPressedBackground);
            Event_ExpandedChanged(isExpanded);
        }

        private void SetXboxControllerButton(XboxControllerInput xboxControllerInput)
        {
            string value = xboxControllerInput switch
            {
                XboxControllerButtonInput button => button.value.ToString(),
                XboxControllerStickInput stick => stick.value.ToString(),
                XboxControllerTriggerInput trigger => trigger.value.ToString(),
                _ => throw new NotSupportedException()
            };

            Element_XboxButtonName.Text = value;
            Element_XboxButton.Source = new SvgImageSource(
                new Uri($"ms-appx:///Assets/XboxButton{value}.svg")
            );
        }

        private void InitializeMenus()
        {
            if (gamecubeControllerInput is GamecubeControllerButtonInput gamecubeControllerButton)
            {
                Element_MappedButton.Content = xboxControllerInput switch
                {
                    XboxControllerButtonInput button => button.value.ToString(),
                    XboxControllerTriggerInput trigger => trigger.value.ToString()
                };

                foreach (XboxControllerButton xboxControllerButton in Enum.GetValues<XboxControllerButton>())
                {
                    var menuItem = new MenuFlyoutItem
                    {
                        Text = xboxControllerButton.ToString(),
                    };
                    menuItem.Click += (_, _) => {
                        Element_MappedButton.Content = xboxControllerButton.ToString();
                        SetXboxControllerButton(new XboxControllerButtonInput(xboxControllerButton));
                        gamecubeController.Profile.SetButton(gamecubeControllerButton.value, xboxControllerButton);
                    };
                    Element_MappableButtons.Items.Add(menuItem);
                }
            }

            if (gamecubeControllerInput is GamecubeControllerTriggerInput gamecubeControllerTrigger)
            {
                Element_MappedButton.Content = xboxControllerInput switch
                {
                    XboxControllerTriggerInput trigger => trigger.value.ToString()
                };

                foreach (XboxControllerTrigger xboxControllerTrigger in Enum.GetValues<XboxControllerTrigger>())
                {
                    var menuItem = new MenuFlyoutItem
                    {
                        Text = xboxControllerTrigger.ToString(),
                    };
                    menuItem.Click += (_, _) => {
                        Element_MappedButton.Content = xboxControllerTrigger.ToString();
                        SetXboxControllerButton(new XboxControllerTriggerInput(xboxControllerTrigger));
                        gamecubeController.Profile.SetTrigger(gamecubeControllerTrigger.value, xboxControllerTrigger);
                    };
                    Element_MappableButtons.Items.Add(menuItem);
                }

                Element_SensitivityPanel.Visibility = Visibility.Visible;
                Element_DeadzonePanel.Visibility = Visibility.Visible;

                float sensitivity = gamecubeControllerTrigger.value == GamecubeControllerTrigger.LeftTrigger ? gamecubeController.Profile.LeftTriggerSensitivity : gamecubeController.Profile.RightTriggerSensitivity;
                float deadzone = gamecubeControllerTrigger.value == GamecubeControllerTrigger.LeftTrigger ? gamecubeController.Profile.LeftTriggerDeadzone : gamecubeController.Profile.RightTriggerDeadzone;

                SetSensitivity(sensitivity);
                SetDeadzone(deadzone);
            }

            if (gamecubeControllerInput is GamecubeControllerStickInput gamecubeControllerStick)
            {
                Element_MappedButton.Content = xboxControllerInput switch
                {
                    XboxControllerStickInput stick => stick.value.ToString()
                };

                foreach (XboxControllerStick xboxControllerStick in Enum.GetValues<XboxControllerStick>())
                {
                    var menuItem = new MenuFlyoutItem
                    {
                        Text = xboxControllerStick.ToString(),
                    };
                    menuItem.Click += (_, _) => {
                        Element_MappedButton.Content = xboxControllerStick.ToString();
                        SetXboxControllerButton(new XboxControllerStickInput(xboxControllerStick));
                        gamecubeController.Profile.SetStick(gamecubeControllerStick.value, xboxControllerStick);
                    };
                    Element_MappableButtons.Items.Add(menuItem);
                }

                Element_SensitivityPanel.Visibility = Visibility.Visible;
                Element_DeadzonePanel.Visibility = Visibility.Visible;

                float sensitivity = gamecubeControllerStick.value == GamecubeControllerStick.LeftStick ? gamecubeController.Profile.LeftStickSensitivity : gamecubeController.Profile.RightStickSensitivity;
                float deadzone = gamecubeControllerStick.value == GamecubeControllerStick.LeftStick ? gamecubeController.Profile.LeftStickDeadzone: gamecubeController.Profile.RightStickDeadzone;

                SetSensitivity(sensitivity);
                SetDeadzone(deadzone);
            }
        }

        private void SetSensitivity(float sensitivity)
        {
            Element_SensitivityValue.Text = sensitivity.ToString("0.00").Replace(",", ".");
            Element_SensitivitySlider.Value = sensitivity;
        }

        private void SetDeadzone(float deadzone)
        {
            Element_DeadzoneValue.Text = deadzone.ToString("0.00").Replace(",", ".");
            Element_DeadzoneSlider.Value = deadzone;
        }

        private void OnSensitivityChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            float sensitivity = (float) e.NewValue;

            if (gamecubeControllerInput is GamecubeControllerStickInput gamecubeControllerStick)
            {
                gamecubeController.Profile.SetSensitivity(gamecubeControllerStick.value, sensitivity);
            } else if (gamecubeControllerInput is GamecubeControllerTriggerInput gamecubeControllerTrigger)
            {
                gamecubeController.Profile.SetSensitivity(gamecubeControllerTrigger.value, sensitivity);
            }

            if (Element_SensitivityValue != null)
            {
                SetSensitivity(sensitivity);
            }
        }

        private void OnDeadzoneChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            float deadzone = (float)e.NewValue;

            if (gamecubeControllerInput is GamecubeControllerStickInput gamecubeControllerStick)
            {
                gamecubeController.Profile.SetDeadzone(gamecubeControllerStick.value, deadzone);
            }
            else if (gamecubeControllerInput is GamecubeControllerTriggerInput gamecubeControllerTrigger)
            {
                gamecubeController.Profile.SetDeadzone(gamecubeControllerTrigger.value, deadzone);
            }

            if (Element_DeadzoneValue != null)
            {
                SetDeadzone(deadzone);
            }
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            Element_Root.Background = App.StringToSolidColorBrush(Colors.ButtonHoverBackground);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            if (!isExpanded)
            {
            Element_Root.Background = App.StringToSolidColorBrush(Colors.ButtonPressedBackground);
            }
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            SetExpandablePanelVisibility(!isExpanded);
        }
    }
}