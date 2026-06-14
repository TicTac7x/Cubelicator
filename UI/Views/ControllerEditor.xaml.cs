using Cubelicator.Services;
using Cubelicator.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Views;

public partial class ControllerEditor : UserControl
{
    private GamecubeControllerView? gamecubeControllerView;

    public ControllerEditor(ViewsManager viewsManager)
    {
        InitializeComponent();

        viewsManager.OnShowControllerEditor += (gamecubeController) =>
        {
            ControllerRoot.Children.Clear();
            ControllerRoot.Children.Add(new GamecubeControllerView(gamecubeController));
        };
    }
}