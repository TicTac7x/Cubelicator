using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controller
{
    public partial class LeftStick : UserControl
    {
        public LeftStick()
        {
            InitializeComponent();
        }

        private string ColorButtonA => ControllerInputColors.ButtonA;
    }
}
