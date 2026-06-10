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

        _calibrationManager = new CalibrationManager();
        _gamecubeAdapter = new GamecubeAdapter();
        _mainWindow = new MainWindow();
        _trayIcon = new TrayIcon();

        _trayIcon.OnOpenMainWindow += OpenMainWindow;
        _trayIcon.OnAppExit += Exit;

        _calibrationManager.OnCalibrationLoaded += (port, calibration) =>
            _gamecubeAdapter.SetPortCalibration(port, calibration);

        _gamecubeAdapter.OnControllerConnectionChanged += (port, connected) =>
            _mainWindow.OnControllerConnectionChanged(port, connected);

        _calibrationManager.OnCalibrationLoaded += (port, calibration) =>
            _mainWindow.SetPortCalibration(port, calibration);

        _gamecubeAdapter.Start();
        _calibrationManager.Start();
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

    public static void DebugOutput(object a)
    {
        System.Diagnostics.Debug.WriteLine(a);
    }
}