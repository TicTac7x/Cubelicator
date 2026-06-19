using Microsoft.UI.Xaml;

namespace Cubelicator;

public partial class MainWindow : Window
{
    private readonly ViewsManager viewsManager;
    private readonly View_Dashboard dashboard;
    private readonly View_ControllerEditor controllerEditor;
    private readonly View_Calibrator viewCalibrator;

    private readonly Navigator navigator;
    private bool exitApp = false;

    public MainWindow(GamecubeAdapter gamecubeAdapter, Settings settings, CalibrationManager calibrationManager, ProfileManager profileManager)
    {
        viewsManager = new ViewsManager();
        dashboard = new View_Dashboard(profileManager, calibrationManager, gamecubeAdapter, settings, viewsManager);
        controllerEditor = new View_ControllerEditor(AdapterPort.One, gamecubeAdapter.GetPortController(AdapterPort.One), profileManager, viewsManager, settings);
        navigator = new Navigator(viewsManager, gamecubeAdapter);
        viewCalibrator = new View_Calibrator(viewsManager, calibrationManager, settings);

        InitializeComponent();
        InitializeWindow();
        SetupEvents();
    }

    private void InitializeWindow()
    {
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        TitleBar.Title = Strings.AppName;
        Element_Root.ActualThemeChanged += (_, _) => UpdateTitleBarTheme();
        this.Closed += OnClose;

        Element_Navigator.Children.Add(navigator);
        Element_Dashboard.Children.Add(dashboard);
        Element_ControllerEditor.Children.Add(controllerEditor);
        Element_Calibration.Children.Add(viewCalibrator);
    }

    private void SetupEvents()
    {
        viewsManager.Event_ShowDashboard += () =>
        {
            Element_Dashboard.Visibility = Visibility.Visible;
            Element_ControllerEditor.Visibility = Visibility.Collapsed;
            Element_Calibration.Visibility = Visibility.Collapsed;
        };

        viewsManager.Event_ShowControllerEditor += (_, _) =>
        {
            Element_Dashboard.Visibility = Visibility.Collapsed;
            Element_ControllerEditor.Visibility = Visibility.Visible;
            Element_Calibration.Visibility = Visibility.Collapsed;
        };

        viewsManager.Event_ShowCalibration += (port, gamecubeController) => {
            Element_Dashboard.Visibility = Visibility.Collapsed;
            Element_ControllerEditor.Visibility = Visibility.Collapsed;
            Element_Calibration.Visibility = Visibility.Visible;
            viewCalibrator.StartCalibration(port, gamecubeController);
         };
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

    private void OnClose(object sender, WindowEventArgs args)
    {
        Hide();
        args.Handled = !exitApp;
    }

    public void Exit()
    {
        exitApp = true;
        Close();
    }

    public void Hide()
    {
        AppWindow.Hide();
    }
}