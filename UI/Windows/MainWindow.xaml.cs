using Microsoft.UI.Xaml;

namespace Cubelicator.UI.Windows;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        TitleBar.Title = Strings.AppName;
        Root.ActualThemeChanged += (_, _) => UpdateTitleBarTheme();
        ControllerPortTile1.SetPort(1);
        ControllerPortTile2.SetPort(2);
        ControllerPortTile3.SetPort(3);
        ControllerPortTile4.SetPort(4);
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
}