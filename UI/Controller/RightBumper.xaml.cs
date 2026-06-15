using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controller
{
    public partial class RightBumper : UserControl
    {
        public RightBumper()
        {
            InitializeComponent();
        }

        private string ColorButton => ControllerInputColors.Trigger;
    }
}
