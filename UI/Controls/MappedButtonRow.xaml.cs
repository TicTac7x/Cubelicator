using Cubelicator.UI.Controller;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Cubelicator.UI.Controls
{
    public partial class MappedButtonRow : UserControl
    {
        public MappedButtonRow(GamecubeControllerInput gamecubeControllerButton, XboxControllerButton xboxControllerButton)
        {
            InitializeComponent();

            var typeName = $"Cubelicator.UI.Controller.{gamecubeControllerButton}";
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new Exception("UI.Controller.Button" + gamecubeControllerButton + " not found");
            }
            var control = Activator.CreateInstance(type) as Control;
            if (control == null)
            {
                throw new Exception("Failed to create gamecube button control");
            }

            GamecubeButton.Children.Add(control);
            GamecubeButtonName.Text = gamecubeControllerButton.ToString();

            XboxButtonName.Text = xboxControllerButton.ToString();
            var a = $"ms-appx:///Assets/XboxButton{xboxControllerButton}.svg";
            XboxButton.Source = new SvgImageSource(new Uri($"ms-appx:///Assets/XboxButton{xboxControllerButton}.svg"));
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            if (sender is Grid element)
            {
                element.Background = App.StringToSolidColorBrush(Colors.ButtonHoverBackground);
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            if (sender is Grid element)
            {
                element.Background = App.StringToSolidColorBrush(ColorRowBackground);
            }
        }

        private string ColorRowBackground = Colors.ButtonPressedBackground;
    }
}