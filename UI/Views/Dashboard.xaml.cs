using Cubelicator.Services;
using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Views;

public sealed partial class Dashboard : UserControl
{
    private readonly ViewsManager viewsManager;
    private readonly ProfileManager profileManager;
    private readonly CalibrationManager calibrationManager;
    private readonly GamecubeAdapter gamecubeAdapter;
    private readonly Settings settings;

    private readonly ControllerPortTile[] controllerPortTiles = new ControllerPortTile[4];
    public Dashboard(ProfileManager profileManager, CalibrationManager calibrationManager, GamecubeAdapter gamecubeAdapter, Settings settings, ViewsManager viewsManager)
    {
        this.viewsManager = viewsManager;
        this.profileManager = profileManager;
        this.calibrationManager = calibrationManager;
        this.gamecubeAdapter = gamecubeAdapter;
        this.settings = settings;

        InitializeComponent();
        InitializeDashboard();
    }

    private void InitializeDashboard()
    {
        for (int port = 1; port <= controllerPortTiles.Length; port++)
        {
            var controllerPortTile = new ControllerPortTile(App.IntToAdapterPort(port), gamecubeAdapter.GetPortController(port), settings, calibrationManager, profileManager, viewsManager);
            controllerPortTiles[port - 1] = controllerPortTile;
            ControllerPortTiles.Children.Add(controllerPortTile);
        }
    }

    private String ColorAppBackground => Colors.AppBackground;
}