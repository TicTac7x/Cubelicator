using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controller
{
    public partial class DPadUp : UserControl
    {
        public DPadUp()
        {
            InitializeComponent();
        }

        private string ColorButton => ControllerInputColors.ButtonDPad;
    }
}
