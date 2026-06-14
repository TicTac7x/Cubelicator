namespace Cubelicator.Services
{
    public class ViewsManager
    {
        public event Action OnShowDashboard = delegate { };
        public event Action<AdapterPort, GamecubeController> OnShowControllerEditor = delegate { };
        public event Action<AppView> OnViewChanged = delegate { };

        public void ShowDashboard()
        {
            OnShowDashboard();
            OnViewChanged(AppView.Dashboard);
        }

        public void ShowControllerEditor(AdapterPort port, GamecubeController gamecubeController)
        {
            OnShowControllerEditor(port, gamecubeController);

            switch (port)
            {
                case AdapterPort.One:
                    OnViewChanged(AppView.Controller1);
                    break;
                case AdapterPort.Two:
                    OnViewChanged(AppView.Controller2);
                    break;
                case AdapterPort.Three:
                    OnViewChanged(AppView.Controller3);
                    break;
                case AdapterPort.Four:
                    OnViewChanged(AppView.Controller4);
                    break;
            }
        }
    }
}
