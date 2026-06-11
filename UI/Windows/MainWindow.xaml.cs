using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml;

namespace Cubelicator.UI.Windows;

public sealed partial class MainWindow : Window
{
    private readonly GamecubeAdapter gamecubeAdapter;
    private readonly ControllerPortTile[] controllerPortTiles = new ControllerPortTile[4];

    public MainWindow(GamecubeAdapter gamecubeAdapter)
    {
        this.gamecubeAdapter = gamecubeAdapter;

        InitializeComponent();
        initializeControllerPortTiles();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        TitleBar.Title = Strings.appName;
        Root.ActualThemeChanged += (_, _) => updateTitleBarTheme();
        this.Closed += mainWindow_Closed;
    }

    private void initializeControllerPortTiles()
    {
        for (int port = 1; port <= controllerPortTiles.Length; port++)
        {
            var controllerPortTile = new ControllerPortTile(port, gamecubeAdapter.getPortController(port));
            controllerPortTiles[port - 1] = controllerPortTile;
            ControllerPortTiles.Children.Add(controllerPortTile);
        }
    }

    private void updateTitleBarTheme()
    {
        var titleBar = AppWindow.TitleBar;

        bool isDark =
            Root.ActualTheme == ElementTheme.Dark;

        titleBar.ButtonForegroundColor = isDark
            ? Microsoft.UI.Colors.White
            : Microsoft.UI.Colors.Black;
    }

    private void mainWindow_Closed(object sender, WindowEventArgs e)
    {
        e.Handled = true;
        AppWindow.Hide();
    }
}