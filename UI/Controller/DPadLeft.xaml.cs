using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controller
{
    public partial class DPadLeft : UserControl
    {
        public DPadLeft()
        {
            InitializeComponent();
        }

        private string ColorButton => ControllerInputColors.ButtonDPad;
    }
}
