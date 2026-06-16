using Cubelicator.Services;
using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Views;

public partial class ControllerEditor : UserControl
{
    private readonly AdapterPort port;
    private readonly GamecubeController gamecubeController;
    private readonly ProfileManager profileManager;
    private readonly ViewsManager viewsManager;
    private readonly Settings settings;

    private readonly List<MappedButtonRow> mappedRows = new List<MappedButtonRow>();

    public ControllerEditor(AdapterPort port, GamecubeController controller, ProfileManager profileManager, ViewsManager viewsManager, Settings settings)
    {
        this.port = port;
        this.gamecubeController = gamecubeController;
        this.profileManager = profileManager;
        this.viewsManager = viewsManager;
        this.settings = settings;

        InitializeComponent();
        InitializeEvents();
    }

    private void InitializeUI(AdapterPort port, GamecubeController gamecubeController)
    {
        RootProfileSelector.Children.Clear();
        RootProfileSelector.Children.Add(new ProfileSelector(profileManager, gamecubeController, true));

        ControllerRoot.Children.Clear();
        var controllerView = new GamecubeControllerView(gamecubeController);
        controllerView.SetControllerColor(
            ControllerColors.Map[settings.GetControllerColor(port)]
        );
        ControllerRoot.Children.Add(controllerView);

        InitializeMappedButtons(gamecubeController);
    }

    private void InitializeEvents()
    {
        viewsManager.OnShowControllerEditor += (port, gamecubeController) =>
        {
            InitializeUI(port, gamecubeController);
        };
    }

    private void InitializeMappedButtons(GamecubeController gamecubeController)
    {
        RootMappedButtons.Children.Clear();
        mappedRows.Clear();

        void AddRow(MappedButtonRow row)
        {
            mappedRows.Add(row);

            row.OnExpandedChanged += isExpanded =>
            {
                if (!isExpanded) return;

                foreach (var other in mappedRows)
                {
                    if (!ReferenceEquals(other, row))
                    {
                        other.SetExpandablePanelVisibility(false);
                    }
                }
            };

            RootMappedButtons.Children.Add(row);
        }

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.A),
            new XboxControllerButtonInput(gamecubeController.Profile.A)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.B),
            new XboxControllerButtonInput(gamecubeController.Profile.B)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.X),
            new XboxControllerButtonInput(gamecubeController.Profile.X)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.Y),
            new XboxControllerButtonInput(gamecubeController.Profile.Y)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.Z),
            new XboxControllerButtonInput(gamecubeController.Profile.Z)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.Start),
            new XboxControllerButtonInput(gamecubeController.Profile.Start)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.DPadUp),
            new XboxControllerButtonInput(gamecubeController.Profile.DPadUp)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.DPadDown),
            new XboxControllerButtonInput(gamecubeController.Profile.DPadDown)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.DPadLeft),
            new XboxControllerButtonInput(gamecubeController.Profile.DPadLeft)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.DPadRight),
            new XboxControllerButtonInput(gamecubeController.Profile.DPadRight)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerStickInput(GamecubeControllerStick.LeftStick),
            new XboxControllerStickInput(gamecubeController.Profile.LeftStick)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerStickInput(GamecubeControllerStick.RightStick),
            new XboxControllerStickInput(gamecubeController.Profile.RightStick)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerTriggerInput(GamecubeControllerTrigger.LeftTrigger),
            new XboxControllerTriggerInput(gamecubeController.Profile.LeftTrigger)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerTriggerInput(GamecubeControllerTrigger.RightTrigger),
            new XboxControllerTriggerInput(gamecubeController.Profile.RightTrigger)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.LeftBumper),
            new XboxControllerButtonInput(gamecubeController.Profile.LeftBumper)
        ));

        AddRow(new MappedButtonRow(
            gamecubeController.Profile,
            new GamecubeControllerButtonInput(GamecubeControllerButton.RightBumper),
            new XboxControllerButtonInput(gamecubeController.Profile.RightBumper)
        ));
    }

    private string ColorButtonPressed => Colors.ButtonPressedBackground;
    private string ColorDeviceBackground => Colors.DeviceBackground;
}
