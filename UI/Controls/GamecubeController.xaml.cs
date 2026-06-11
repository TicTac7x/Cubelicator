using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controls
{
    public sealed partial class GamecubeController : UserControl
    {
        public GamecubeController()
        {
            InitializeComponent();
            SetControllerColor(Colors.Indigo);
        }

        public void SetControllerColor(string color)
        {
            var brush = App.StringToColor(color);
            BaseCenter.Fill = brush;
            BaseLeftPalm.Fill = brush;
            BaseLeftPlate.Fill = brush;
            BaseRightPalm.Fill = brush;
            BaseRightPlate.Fill = brush;
        }
    }
}