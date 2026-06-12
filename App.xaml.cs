using Microsoft.UI.Xaml;
using Cubelicator.UI.Windows;
using Cubelicator.Services;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Cubelicator;

public partial class App : Application
{
    private readonly MainWindow mainWindow;
    private readonly TrayIcon trayIcon;
    private readonly CalibrationManager calibrationManager;
    private readonly SettingsManager settingsManager;
    private readonly GamecubeAdapter gamecubeAdapter;

    public App()
    {
        InitializeComponent();

        calibrationManager = new CalibrationManager();
        settingsManager = new SettingsManager();
        gamecubeAdapter = new GamecubeAdapter(calibrationManager);
        trayIcon = new TrayIcon(OpenMainWindow, Exit);
        mainWindow = new MainWindow(gamecubeAdapter, settingsManager.Settings, calibrationManager);

        calibrationManager.Start();
        settingsManager.Start();
        gamecubeAdapter.Start();
        mainWindow.Activate();
    }

    public new void Exit()
    {
        mainWindow.Close();
        gamecubeAdapter.Stop();
        trayIcon.Stop();
        Current.Exit();
    }

    private void OpenMainWindow()
    {
        mainWindow.Activate();
    }

    public static void DebugOutput(object a)
    {
        System.Diagnostics.Debug.WriteLine(a);
    }

    public static SolidColorBrush StringToSolidColorBrush(string color)
    {
        color = color.TrimStart('#');

        byte r = Convert.ToByte(color.Substring(0, 2), 16);
        byte g = Convert.ToByte(color.Substring(2, 2), 16);
        byte b = Convert.ToByte(color.Substring(4, 2), 16);

        return new SolidColorBrush(Color.FromArgb(255, r, g, b));
    }
}