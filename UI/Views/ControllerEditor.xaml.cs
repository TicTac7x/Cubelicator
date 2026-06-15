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
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.A, gamecubeController.Profile.ButtonA));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.B, gamecubeController.Profile.ButtonB));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.X, gamecubeController.Profile.ButtonX));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.Y, gamecubeController.Profile.ButtonY));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.Z, gamecubeController.Profile.ButtonZ));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.Start, gamecubeController.Profile.ButtonStart));

        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.DPadUp, gamecubeController.Profile.ButtonDPadUp));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.DPadDown, gamecubeController.Profile.ButtonDPadDown));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.DPadLeft, gamecubeController.Profile.ButtonDPadLeft));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.DPadRight, gamecubeController.Profile.ButtonDPadRight));

        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.LeftStick, XboxControllerButton.LeftThumb));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.RightStick, XboxControllerButton.RightThumb));

        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.LeftTrigger, XboxControllerButton.LeftTrigger));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.RightTrigger, XboxControllerButton.RightTrigger));

        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.LeftShoulder, XboxControllerButton.LeftShoulder));
        RootMappedButtons.Children.Add(new MappedButtonRow(GamecubeControllerInput.RightShoulder, XboxControllerButton.RightShoulder));
    }

    private string ColorButtonPressed => Colors.ButtonPressedBackground;
    private string ColorDeviceBackground => Colors.DeviceBackground;
}