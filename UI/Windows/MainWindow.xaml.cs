using Cubelicator.Services;
using Cubelicator.UI.Controls;
using Cubelicator.UI.Views;
using Microsoft.UI.Xaml;

namespace Cubelicator.UI.Windows;

public partial class MainWindow : Window
{
    private readonly ViewsManager viewsManager;
    private readonly Dashboard dashboard;
    private readonly ControllerEditor controllerEditor;

    private readonly Navigator navigator;
    private bool exitApp = false;

    public MainWindow(GamecubeAdapter gamecubeAdapter, Settings settings, CalibrationManager calibrationManager, ProfileManager profileManager)
    {
        viewsManager = new ViewsManager();
        dashboard = new Dashboard(profileManager, calibrationManager, gamecubeAdapter, settings, viewsManager);
        controllerEditor = new ControllerEditor(viewsManager, settings);
        navigator = new Navigator(viewsManager, gamecubeAdapter);

        InitializeComponent();
        InitializeWindow();
        NavigatorRoot.Children.Add(navigator);
        DashboardRoot.Children.Add(dashboard);
        ControllerEditorRoot.Children.Add(controllerEditor);

        viewsManager.OnShowDashboard += () =>
        {
            DashboardRoot.Visibility = Visibility.Visible;
            ControllerEditorRoot.Visibility = Visibility.Collapsed;
        };

        viewsManager.OnShowControllerEditor += (_, _) =>
        {
            DashboardRoot.Visibility = Visibility.Collapsed;
            ControllerEditorRoot.Visibility = Visibility.Visible;
        };
    }

    private void InitializeWindow()
    {
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        TitleBar.Title = Strings.AppName;
        Root.ActualThemeChanged += (_, _) => UpdateTitleBarTheme();
        this.Closed += OnClose;
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