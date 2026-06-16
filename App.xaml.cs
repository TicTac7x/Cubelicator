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
    private readonly ProfileManager profileManager;

    public App()
    {
        InitializeComponent();

        calibrationManager = new CalibrationManager();
        profileManager = new ProfileManager();
        settingsManager = new SettingsManager(profileManager);
        gamecubeAdapter = new GamecubeAdapter(calibrationManager, settingsManager.Settings, profileManager);
        trayIcon = new TrayIcon(OpenMainWindow, ExitApp);
        mainWindow = new MainWindow(gamecubeAdapter, settingsManager.Settings, calibrationManager, profileManager);

        SetupEvents();

        calibrationManager.Start();
        profileManager.Start();
        settingsManager.Start();
        gamecubeAdapter.Start();
        mainWindow.Activate();
    }

    private void SetupEvents()
    {
        gamecubeAdapter.OnControllerProfileChanged += (port, profile) =>
        {
            settingsManager.Settings.SetControllerProfile(port, profile);
        };
    }

    private void ExitApp()
    {
        mainWindow.Exit();
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

    public static AdapterPort IntToAdapterPort(int port)
    {
        switch (port)
        {
            case 1:
                return AdapterPort.One;
            case 2:
                return AdapterPort.Two;
            case 3:
                return AdapterPort.Three;
            case 4:
                return AdapterPort.Four;
            default:
                throw new Exception("Invalid adapter port " + port);
        }
    }
}