using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controller
{
    public partial class DPadDown : UserControl
    {
        public DPadDown()
        {
            InitializeComponent();
        }

        private string ColorButton => ControllerInputColors.ButtonDPad;
    }
}
