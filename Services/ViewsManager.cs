namespace Cubelicator.Services
{
    public class ViewsManager
    {
        public event Action OnShowDashboard = delegate { };
        public event Action<GamecubeController> OnShowControllerEditor = delegate { };

        public void ShowDashboard()
        {
            OnShowDashboard();
        }

        public void ShowControllerEditor(GamecubeController gamecubeController)
        {
            OnShowControllerEditor(gamecubeController);
        }
    }
}
