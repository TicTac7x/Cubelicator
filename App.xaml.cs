using Microsoft.UI.Xaml;
using Cubelicator.UI.Windows;
using Cubelicator.Services;

namespace Cubelicator;

public partial class App : Application
{
    private readonly MainWindow mainWindow;
    private readonly TrayIcon trayIcon;
    private readonly CalibrationManager calibrationManager;
    private readonly GamecubeAdapter gamecubeAdapter;

    public App()
    {
        InitializeComponent();

        calibrationManager = new CalibrationManager();
        gamecubeAdapter = new GamecubeAdapter(calibrationManager);
        trayIcon = new TrayIcon(openMainWindow, Exit);
        mainWindow = new MainWindow(gamecubeAdapter);

        gamecubeAdapter.start();
        calibrationManager.start();
        mainWindow.Activate();
    }

    public new void Exit()
    {
        mainWindow.Close();
        gamecubeAdapter.stop();
        trayIcon.stop();
        Current.Exit();
    }

    private void openMainWindow()
    {
        mainWindow.Activate();
    }

    public static void debugOutput(object a)
    {
        System.Diagnostics.Debug.WriteLine(a);
    }
}