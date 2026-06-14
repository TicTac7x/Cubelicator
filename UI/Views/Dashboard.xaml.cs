using Cubelicator.Services;
using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Views;

public partial class Dashboard : UserControl
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
        foreach (AdapterPort port in Enum.GetValues<AdapterPort>())
        {
            int index = (int) port - 1;
            var controllerPortTile = new ControllerPortTile(
                port,
                gamecubeAdapter.GetPortController(port),
                settings,
                calibrationManager,
                profileManager,
                viewsManager);

            controllerPortTiles[index] = controllerPortTile;
            ControllerPortTiles.Children.Add(controllerPortTile);
        }
    }

    private String ColorAppBackground => Colors.AppBackground;
}