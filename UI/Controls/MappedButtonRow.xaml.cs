using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Cubelicator
{
    public partial class MappedButtonRow : UserControl
    {
        public event Action<bool> OnExpandedChanged = delegate { };
        private readonly GamecubeControllerInput gamecubeControllerInput;
        private XboxControllerInput xboxControllerInput;
        private readonly GamecubeControllerProfile profile;
        private bool isExpanded = false;

        public MappedButtonRow(GamecubeControllerProfile profile, GamecubeControllerInput gamecubeControllerInput, XboxControllerInput xboxControllerInput)
        {
            this.profile = profile;
            this.gamecubeControllerInput = gamecubeControllerInput;
            this.xboxControllerInput = xboxControllerInput;

            InitializeComponent();
            InitializeMenus();

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

            GamecubeButton.Children.Add(control);
            GamecubeButtonName.Text = gamecubeControllerInput.ToString();
            SetXboxControllerButton(xboxControllerInput);
        }

        public void SetExpandablePanelVisibility(bool visible)
        {
            isExpanded = visible;
            ExpandablePanel.Visibility = isExpanded ? Visibility.Visible : Visibility.Collapsed;
            Root.Background = App.StringToSolidColorBrush(isExpanded ? Colors.ButtonHoverBackground : Colors.ButtonPressedBackground);
            OnExpandedChanged(isExpanded);
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

            XboxButtonName.Text = value;
            XboxButton.Source = new SvgImageSource(
                new Uri($"ms-appx:///Assets/XboxButton{value}.svg")
            );
        }

        private void InitializeMenus()
        {
            if (gamecubeControllerInput is GamecubeControllerButtonInput gamecubeControllerButton)
            {
                MappedButton.Content = xboxControllerInput switch
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
                        MappedButton.Content = xboxControllerButton.ToString();
                        SetXboxControllerButton(new XboxControllerButtonInput(xboxControllerButton));
                        profile.SetButton(gamecubeControllerButton.value, xboxControllerButton);
                    };
                    RootMappableButtons.Items.Add(menuItem);
                }
            }

            if (gamecubeControllerInput is GamecubeControllerTriggerInput gamecubeControllerTrigger)
            {
                MappedButton.Content = xboxControllerInput switch
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
                        MappedButton.Content = xboxControllerTrigger.ToString();
                        SetXboxControllerButton(new XboxControllerTriggerInput(xboxControllerTrigger));
                        profile.SetTrigger(gamecubeControllerTrigger.value, xboxControllerTrigger);
                    };
                    RootMappableButtons.Items.Add(menuItem);
                }

                SensitivityPanel.Visibility = Visibility.Visible;
                DeadzonePanel.Visibility = Visibility.Visible;

                float sensitivity = gamecubeControllerTrigger.value == GamecubeControllerTrigger.LeftTrigger ? profile.LeftTriggerSensitivity : profile.RightTriggerSensitivity;
                float deadzone = gamecubeControllerTrigger.value == GamecubeControllerTrigger.LeftTrigger ? profile.LeftTriggerDeadzone : profile.RightTriggerDeadzone;

                SetSensitivity(sensitivity);
                SetDeadzone(deadzone);
            }

            if (gamecubeControllerInput is GamecubeControllerStickInput gamecubeControllerStick)
            {
                MappedButton.Content = xboxControllerInput switch
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
                        MappedButton.Content = xboxControllerStick.ToString();
                        SetXboxControllerButton(new XboxControllerStickInput(xboxControllerStick));
                        profile.SetStick(gamecubeControllerStick.value, xboxControllerStick);
                    };
                    RootMappableButtons.Items.Add(menuItem);
                }

                SensitivityPanel.Visibility = Visibility.Visible;
                DeadzonePanel.Visibility = Visibility.Visible;

                float sensitivity = gamecubeControllerStick.value == GamecubeControllerStick.LeftStick ? profile.LeftStickSensitivity : profile.RightStickSensitivity;
                float deadzone = gamecubeControllerStick.value == GamecubeControllerStick.LeftStick ? profile.LeftStickDeadzone: profile.RightStickDeadzone;

                SetSensitivity(sensitivity);
                SetDeadzone(deadzone);
            }
        }

        private void SetSensitivity(float sensitivity)
        {
            SensitivityValue.Text = sensitivity.ToString("0.00").Replace(",", ".");
            SensitivitySlider.Value = sensitivity;
        }

        private void SetDeadzone(float deadzone)
        {
            DeadzoneValue.Text = deadzone.ToString("0.00").Replace(",", ".");
            DeadzoneSlider.Value = deadzone;
        }

        private void OnSensitivityChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            float sensitivity = (float) e.NewValue;

            if (gamecubeControllerInput is GamecubeControllerStickInput gamecubeControllerStick)
            {
                profile.SetSensitivity(gamecubeControllerStick.value, sensitivity);
            } else if (gamecubeControllerInput is GamecubeControllerTriggerInput gamecubeControllerTrigger)
            {
                profile.SetSensitivity(gamecubeControllerTrigger.value, sensitivity);
            }

            if (SensitivityValue != null)
            {
                SetSensitivity(sensitivity);
            }
        }

        private void OnDeadzoneChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            float deadzone = (float)e.NewValue;

            if (gamecubeControllerInput is GamecubeControllerStickInput gamecubeControllerStick)
            {
                profile.SetDeadzone(gamecubeControllerStick.value, deadzone);
            }
            else if (gamecubeControllerInput is GamecubeControllerTriggerInput gamecubeControllerTrigger)
            {
                profile.SetDeadzone(gamecubeControllerTrigger.value, deadzone);
            }

            if (DeadzoneValue != null)
            {
                SetDeadzone(deadzone);
            }
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            Root.Background = App.StringToSolidColorBrush(Colors.ButtonHoverBackground);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            if (!isExpanded)
            {
            Root.Background = App.StringToSolidColorBrush(Colors.ButtonPressedBackground);
            }
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            SetExpandablePanelVisibility(!isExpanded);
        }
    }
}