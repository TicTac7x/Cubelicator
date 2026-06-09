using Microsoft.UI.Xaml;

namespace Cubelicator;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        TitleBar.Title = Strings.AppName;
        Root.ActualThemeChanged += (_, _) => UpdateTitleBarTheme();
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