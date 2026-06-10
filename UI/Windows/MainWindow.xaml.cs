using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml;

namespace Cubelicator.UI.Windows;

public sealed partial class MainWindow : Window
{
    private readonly ControllerPortTile[] _controllerPortTiles = new ControllerPortTile[4];

    public MainWindow()
    {
        InitializeComponent();
        InitializeControllerPortTiles();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        TitleBar.Title = Strings.AppName;
        Root.ActualThemeChanged += (_, _) => UpdateTitleBarTheme();
        this.Closed += MainWindow_Closed;
    }

    public void OnControllerConnectionChanged(int port, bool connected)
    {
        _controllerPortTiles[port - 1].SetControllerConnected(connected);
    }

    private void InitializeControllerPortTiles()
    {
        for (int i = 0; i < _controllerPortTiles.Length; i++)
        {
            var controllerPortTile = new ControllerPortTile(i + 1);
            _controllerPortTiles[i] = controllerPortTile;
            ControllerPortTiles.Children.Add(controllerPortTile);

            if (i == 0)
            {
                controllerPortTile.SetControllerConnected(true);
            }
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

    private void MainWindow_Closed(object sender, WindowEventArgs e)
    {
        e.Handled = true;
        AppWindow.Hide();
    }

    public void SetPortCalibration(int port, GamecubeControllerCalibration calibration)
    {
        _controllerPortTiles[port - 1].SetPortCalibration(calibration);
    }
}