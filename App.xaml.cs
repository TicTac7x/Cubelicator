using Microsoft.UI.Xaml;
using Cubelicator.UI.Windows;
using Cubelicator.Services;

namespace Cubelicator;

public partial class App : Application
{
    private readonly MainWindow _mainWindow;
    private readonly TrayIcon _trayIcon;
    private readonly CalibrationManager _calibrationManager;
    private readonly GamecubeAdapter _gamecubeAdapter;

    public App()
    {
        InitializeComponent();
        _mainWindow = new MainWindow();
        _trayIcon = CreateTrayIcon();
        _gamecubeAdapter = CreateGamecubeAdapter();
        _calibrationManager = CreateCalibrationManager();
        _calibrationManager.LoadCalibrations();
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

    private CalibrationManager CreateCalibrationManager()
    {
        var calibrationManager = new CalibrationManager();

        calibrationManager.OnCalibrationLoaded += (port, calibration) =>
        {
            _gamecubeAdapter.SetPortCalibration(port, calibration);
            _mainWindow.SetPortCalibration(port, calibration);
        };

        return calibrationManager;
    }

    public static void DebugOutput(object a)
    {
        System.Diagnostics.Debug.WriteLine(a);
    }
}