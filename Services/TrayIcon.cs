using System.Drawing;
using System.Windows.Forms;

namespace Cubelicator
{
    public class TrayIcon
    {
        private readonly NotifyIcon notifyIcon;
        private readonly Action OpenMainWindow;
        private readonly Action HideMainWindow;

        private bool isVisible = true;

        public TrayIcon(Action OpenMainWindow, Action HideMainWindow, Action ExitApp)
        {
            this.OpenMainWindow = OpenMainWindow;
            this.HideMainWindow = HideMainWindow;

            var menu = new ContextMenuStrip();

            menu.Items.Add(Strings.Open, null, (_, _) => ShowWindow());
            menu.Items.Add(Strings.Exit, null, (_, _) => ExitApp());

            notifyIcon = new NotifyIcon
            {
                Icon = new Icon("Cubelicator.ico"),
                Text = Strings.AppName,
                Visible = true,
                ContextMenuStrip = menu
            };

            notifyIcon.MouseClick += NotifyIcon_MouseClick;
        }

        private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ToggleWindow();
            }
        }

        private void ToggleWindow()
        {
            if (isVisible)
            {
                HideMainWindow();
                isVisible = false;
            }
            else
            {
                ShowWindow();
                isVisible = true;
            }
        }

        private void ShowWindow()
        {
            OpenMainWindow();
            isVisible = true;
        }

        public void Stop()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }
    }
}