using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controller
{
    public partial class DPadRight : UserControl
    {
        public DPadRight()
        {
            InitializeComponent();
        }

        private string ColorButton => ControllerInputColors.ButtonDPad;
    }
}
