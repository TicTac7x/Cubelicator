using Microsoft.UI.Xaml;

namespace Cubelicator;

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
        controllerEditor = new ControllerEditor(AdapterPort.One, gamecubeAdapter.GetPortController(AdapterPort.One), profileManager, viewsManager, settings);
        navigator = new Navigator(viewsManager, gamecubeAdapter);

        InitializeComponent();
        InitializeWindow();
        Element_Navigator.Children.Add(navigator);
        Element_Dashboard.Children.Add(dashboard);
        Element_ControllerEditor.Children.Add(controllerEditor);

        viewsManager.Event_ShowDashboard += () =>
        {
            Element_Dashboard.Visibility = Visibility.Visible;
            Element_ControllerEditor.Visibility = Visibility.Collapsed;
        };

        viewsManager.Event_ShowControllerEditor += (_, _) =>
        {
            Element_Dashboard.Visibility = Visibility.Collapsed;
            Element_ControllerEditor.Visibility = Visibility.Visible;
        };
    }

    private void InitializeWindow()
    {
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        TitleBar.Title = Strings.AppName;
        Element_Root.ActualThemeChanged += (_, _) => UpdateTitleBarTheme();
        this.Closed += OnClose;
    }

    private void UpdateTitleBarTheme()
    {
        var titleBar = AppWindow.TitleBar;

        bool isDark =
            Element_Root.ActualTheme == ElementTheme.Dark;

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
}