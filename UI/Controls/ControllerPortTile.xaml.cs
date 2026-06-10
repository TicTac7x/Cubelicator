using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace Cubelicator.UI.Controls
{
    public sealed partial class ControllerPortTile : UserControl
    {

        public ControllerPortTile(int port)
        {
            InitializeComponent();
            SetPort(port);
        }

        public void SetControllerConnected(bool connected)
        {
            var portVisible = connected ? Visibility.Collapsed : Visibility.Visible;
            var controllerVisible = connected ? Visibility.Visible : Visibility.Collapsed;

            DispatcherQueue.TryEnqueue(() =>
            {
                Port.Visibility = portVisible;
                PortText.Visibility = portVisible;
                Controller.Visibility = controllerVisible;
                ControllerText.Visibility = controllerVisible;
            });
        }

        private void SetPort(int port)
        {
            PortText.Text = PortText.Text + " " + port;
            ControllerText.Text = ControllerText.Text + " " + port;
        }
    }
}