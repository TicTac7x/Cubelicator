using Microsoft.UI.Xaml;

namespace Cubelicator;

public partial class App : Application
{
    private readonly GamecubeAdapter _adapter;
    private readonly TrayIcon _trayIcon;
    private Window1? _window1;

    public App()
    {
        InitializeComponent();
        _adapter = new GamecubeAdapter();
        _trayIcon = new TrayIcon(Exit);
        _window1 = new Window1();
        _window1.Activate();
    }

    public new void Exit()
    {
        _window1?.Close();
        _adapter.Stop();
        _trayIcon.Stop();
        Current.Exit();
    }

    public static void DebugOutput(object a)
    {
        System.Diagnostics.Debug.WriteLine(a);
    }
}