using Microsoft.UI.Xaml;
using Cubelicator.UI.Windows;

namespace Cubelicator;

public partial class App : Application
{
    private readonly GamecubeAdapter _adapter;
    private readonly TrayIcon _trayIcon;
    private MainWindow? _mainWindow;

    public App()
    {
        InitializeComponent();
        _adapter = new GamecubeAdapter();
        _trayIcon = new TrayIcon(Exit);
        _mainWindow = new MainWindow();
        _mainWindow.Activate();
    }

    public new void Exit()
    {
        _mainWindow?.Close();
        _adapter.Stop();
        _trayIcon.Stop();
        Current.Exit();
    }

    public static void DebugOutput(object a)
    {
        System.Diagnostics.Debug.WriteLine(a);
    }
}