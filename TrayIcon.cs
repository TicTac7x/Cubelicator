using System.Drawing;
using System.Windows.Forms;

namespace Cubelicator;

public sealed class TrayIcon
{
    private readonly NotifyIcon _notifyIcon;

    public TrayIcon(Action ExitApp)
    {
        var menu = new ContextMenuStrip();

        menu.Items.Add(Strings.Exit + " " + Strings.AppName, null, (_, _) =>
        {
            ExitApp();
        });

        _notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Text = Strings.AppName,
            Visible = true,
            ContextMenuStrip = menu
        };
    }

    public void Stop()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
    }
}