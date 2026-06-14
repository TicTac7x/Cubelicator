using Cubelicator.Services;
using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Views;

public partial class ControllerEditor : UserControl
{
    private readonly ViewsManager viewsManager;
    private readonly Settings settings;

    private GamecubeController gamecubeController;

    public ControllerEditor(ViewsManager viewsManager, Settings settings)
    {
        this.viewsManager = viewsManager;
        this.settings = settings;

        InitializeComponent();
        InitializeEventListeners();
    }

    private void InitializeEventListeners()
    {
        viewsManager.OnShowControllerEditor += (port, gamecubeController) =>
        {
            ControllerRoot.Children.Clear();

            this.gamecubeController = gamecubeController;

            var controllerView = new GamecubeControllerView(gamecubeController);
            controllerView.SetControllerColor(ControllerColors.Map[settings.GetControllerColor(port)]);
            ControllerRoot.Children.Add(controllerView);

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
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.Z, gamecubeController.Profile.ButtonZ));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.Start, gamecubeController.Profile.ButtonStart));

        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.DPadUp, gamecubeController.Profile.ButtonY));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.DPadDown, gamecubeController.Profile.ButtonY));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.DPadLeft, gamecubeController.Profile.ButtonY));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerButton.DPadRight, gamecubeController.Profile.ButtonY));
    }

    private string ColorButtonPressed => Colors.ButtonPressedBackground;
}