using Cubelicator.Services;
using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Views;

public partial class ControllerEditor : UserControl
{
    private readonly ViewsManager viewsManager;
    private GamecubeController gamecubeController;

    public ControllerEditor(ViewsManager viewsManager)
    {
        this.viewsManager = viewsManager;

        InitializeComponent();
        InitializeEventListeners();
    }

    private void InitializeEventListeners()
    {
        viewsManager.OnShowControllerEditor += (gamecubeController) =>
        {
            ControllerRoot.Children.Clear();

            this.gamecubeController = gamecubeController;
            ControllerRoot.Children.Add(new GamecubeControllerView(gamecubeController));

            InitializeMappedButtons();
        };
    }

    private void InitializeMappedButtons()
    {
        RootMappedButtons.Children.Clear();
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.A, gamecubeController.Profile.ButtonA));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.B, gamecubeController.Profile.ButtonB));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.X, gamecubeController.Profile.ButtonX));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.Y, gamecubeController.Profile.ButtonY));
    }

    private string ColorButtonPressed => Colors.ButtonPressedBackground;
}