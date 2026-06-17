using System.Drawing;
using System.Windows.Forms;

namespace Cubelicator
{
    public class TrayIcon
    {
        private readonly NotifyIcon notifyIcon;

        public TrayIcon(Action OpenMainWindow, Action ExitApp)
        {
            var menu = new ContextMenuStrip();

            menu.Items.Add(Strings.Open, null, (_, _) => OpenMainWindow());
            menu.Items.Add(Strings.Exit, null, (_, _) => ExitApp());

            notifyIcon = new NotifyIcon
            {
                Icon = new Icon("Cubelicator.ico"),
                Text = Strings.AppName,
                Visible = true,
                ContextMenuStrip = menu
            };
        }

        public void Stop()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }
    }
}