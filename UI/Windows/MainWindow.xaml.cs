using Cubelicator.Services;
using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml;

namespace Cubelicator.UI.Windows;

public sealed partial class MainWindow : Window
{
    private bool exitApp = false;
    private readonly Settings settings;
    private readonly GamecubeAdapter gamecubeAdapter;
    private readonly ControllerPortTile[] controllerPortTiles = new ControllerPortTile[4];
    private readonly CalibrationManager calibrationManager;
    private readonly ProfileManager profileManager;

    public MainWindow(GamecubeAdapter gamecubeAdapter, Settings settings, CalibrationManager calibrationManager, ProfileManager profileManager)
    {
        this.settings = settings;
        this.gamecubeAdapter = gamecubeAdapter;
        this.calibrationManager = calibrationManager;
        this.profileManager = profileManager;

        InitializeComponent();
        InitializeControllerPortTiles();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        TitleBar.Title = Strings.AppName;
        Root.ActualThemeChanged += (_, _) => UpdateTitleBarTheme();
        this.Closed += OnClose;
        this.profileManager = profileManager;
    }

    private void InitializeControllerPortTiles()
    {
        for (int port = 1; port <= controllerPortTiles.Length; port++)
        {
            var controllerPortTile = new ControllerPortTile(port, gamecubeAdapter.GetPortController(port), settings, calibrationManager, profileManager);
            controllerPortTiles[port - 1] = controllerPortTile;
            ControllerPortTiles.Children.Add(controllerPortTile);
        }
    }

    private void UpdateTitleBarTheme()
    {
        var titleBar = AppWindow.TitleBar;

        bool isDark =
            Root.ActualTheme == ElementTheme.Dark;

        titleBar.ButtonForegroundColor = isDark
            ? Microsoft.UI.Colors.White
            : Microsoft.UI.Colors.Black;
    }

    private void OnClose(object sender, WindowEventArgs e)
    {
            AppWindow.Hide();
            e.Handled = !exitApp;
    }

    public void Exit()
    {
        exitApp = true;
        Close();
    }

    private string ColorAppBackground = Colors.AppBackground;
    private string ColorDeviceBackground = Colors.DeviceBackground;
}