using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controller
{
    public partial class RightStick : UserControl
    {
        public RightStick()
        {
            InitializeComponent();
        }

        private string ColorButtonA => ControllerInputColors.ButtonA;
    }
}
