using System.Drawing;
using System.Windows.Forms;

namespace Cubelicator;

public sealed class TrayIcon
{
    public event Action OnOpenMainWindow = delegate { };
    public event Action OnAppExit = delegate { };

    private readonly NotifyIcon _notifyIcon;

    public TrayIcon()
    {
        var menu = new ContextMenuStrip();

        menu.Items.Add(Strings.Open, null, (_, _) => OnOpenMainWindow.Invoke());
        menu.Items.Add(Strings.Exit, null, (_, _) => OnAppExit.Invoke());

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