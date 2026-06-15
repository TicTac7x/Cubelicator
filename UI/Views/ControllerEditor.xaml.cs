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
            controllerView.SetControllerColor(
                ControllerColors.Map[settings.GetControllerColor(port)]
            );
            ControllerRoot.Children.Add(controllerView);

            InitializeMappedButtons();
        };
    }

    private void InitializeMappedButtons()
    {
        RootMappedButtons.Children.Clear();
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.A),
                new XboxControllerButtonInput(gamecubeController.Profile.A)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.B),
                new XboxControllerButtonInput(gamecubeController.Profile.B)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.X),
                new XboxControllerButtonInput(gamecubeController.Profile.X)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.Y),
                new XboxControllerButtonInput(gamecubeController.Profile.Y)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.Z),
                new XboxControllerButtonInput(gamecubeController.Profile.Z)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.Start),
                new XboxControllerButtonInput(gamecubeController.Profile.Start)
            )
        );

        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.DPadUp),
                new XboxControllerButtonInput(gamecubeController.Profile.DPadUp)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.DPadDown),
                new XboxControllerButtonInput(gamecubeController.Profile.DPadDown)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.DPadLeft),
                new XboxControllerButtonInput(gamecubeController.Profile.DPadLeft)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.DPadRight),
                new XboxControllerButtonInput(gamecubeController.Profile.DPadRight)
            )
        );

        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerStickInput(GamecubeControllerStick.LeftStick),
                new XboxControllerStickInput(gamecubeController.Profile.LeftStick)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerStickInput(GamecubeControllerStick.RightStick),
                new XboxControllerStickInput(gamecubeController.Profile.RightStick)
            )
        );

        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerTriggerInput(GamecubeControllerTrigger.LeftTrigger),
                new XboxControllerTriggerInput(gamecubeController.Profile.LeftTrigger)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerTriggerInput(GamecubeControllerTrigger.RightTrigger),
                new XboxControllerTriggerInput(gamecubeController.Profile.RightTrigger)
            )
        );

        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.LeftBumper),
                new XboxControllerButtonInput(gamecubeController.Profile.LeftBumper)
            )
        );
        RootMappedButtons.Children.Add(
            new MappedButtonRow(
                gamecubeController.Profile,
                new GamecubeControllerButtonInput(GamecubeControllerButton.RightBumper),
                new XboxControllerButtonInput(gamecubeController.Profile.RightBumper)
            )
        );
    }

    private string ColorButtonPressed => Colors.ButtonPressedBackground;
    private string ColorDeviceBackground => Colors.DeviceBackground;
}
