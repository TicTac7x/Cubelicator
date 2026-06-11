using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Cubelicator.UI.Controls
{
    public sealed partial class GamecubeController : UserControl
    {
        public GamecubeController()
        {
            InitializeComponent();
            setControllerColor(Colors.indigo);
        }

        private void setControllerColor(string color)
        {
            var brush = stringToColor(color);
            BaseCenter.Fill = brush;
            BaseLeftPalm.Fill = brush;
            BaseLeftPlate.Fill = brush;
            BaseRightPalm.Fill = brush;
            BaseRightPlate.Fill = brush;
        }

        private static SolidColorBrush stringToColor(string color)
        {
            color = color.TrimStart('#');

            byte r = Convert.ToByte(color.Substring(0, 2), 16);
            byte g = Convert.ToByte(color.Substring(2, 2), 16);
            byte b = Convert.ToByte(color.Substring(4, 2), 16);

            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }
    }
}