using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controls
{
    public sealed partial class ControllerPortTile : UserControl
    {

        public ControllerPortTile()
        {
            InitializeComponent();
        }

        public void SetPort(int port)
        {
            PortText.Text = $"Controller {port}";
        }
    }
}