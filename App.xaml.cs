using Microsoft.UI.Xaml;
using Cubelicator.UI.Windows;

namespace Cubelicator;

public partial class App : Application
{
    private readonly GamecubeAdapter _gamecubeAdapter;
    private readonly TrayIcon _trayIcon;
    private readonly MainWindow _mainWindow;

    public App()
    {
        InitializeComponent();
        _mainWindow = new MainWindow();
        _gamecubeAdapter = CreateGamecubeAdapter();
        _trayIcon = CreateTrayIcon();
        _mainWindow.Activate();
    }

    public new void Exit()
    {
        _mainWindow.Close();
        _gamecubeAdapter.Stop();
        _trayIcon.Stop();
        Current.Exit();
    }

    private void OpenMainWindow()
    {
        _mainWindow.Activate();
    }

    private GamecubeAdapter CreateGamecubeAdapter()
    {
        var gamecubeAdapter = new GamecubeAdapter();

        gamecubeAdapter.OnControllerConnectionChanged += (port, connected) =>
            _mainWindow.OnControllerConnectionChanged(port, connected);

        return gamecubeAdapter;
    }

    private TrayIcon CreateTrayIcon()
    {
        var trayIcon = new TrayIcon();

        trayIcon.OnOpenMainWindow += OpenMainWindow;
        trayIcon.OnAppExit += Exit;

        return trayIcon;
    }

    public static void DebugOutput(object a)
    {
        System.Diagnostics.Debug.WriteLine(a);
    }
}