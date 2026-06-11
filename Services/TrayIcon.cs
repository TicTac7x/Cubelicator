using System.Drawing;
using System.Windows.Forms;

namespace Cubelicator;

public sealed class TrayIcon
{
    private readonly NotifyIcon notifyIcon;

    public TrayIcon(Action openMainWindow, Action exitApp)
    {
        var menu = new ContextMenuStrip();

        menu.Items.Add(Strings.open, null, (_, _) => openMainWindow());
        menu.Items.Add(Strings.exit, null, (_, _) => exitApp());

        notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Text = Strings.appName,
            Visible = true,
            ContextMenuStrip = menu
        };
    }

    public void stop()
    {
        notifyIcon.Visible = false;
        notifyIcon.Dispose();
    }
}