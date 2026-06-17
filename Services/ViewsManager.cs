namespace Cubelicator
{
    public class ViewsManager
    {
        public event Action Event_ShowDashboard = delegate { };
        public event Action<AdapterPort, GamecubeController> Event_ShowCalibration = delegate { };
        public event Action<AdapterPort, GamecubeController> Event_ShowControllerEditor = delegate { };
        public event Action<AppView> Event_ViewChanged = delegate { };

        public void ShowDashboard()
        {
            Event_ShowDashboard();
            Event_ViewChanged(AppView.Dashboard);
        }

        public void ShowCalibration(AdapterPort port, GamecubeController gamecubeController)
        {
            Event_ShowCalibration(port, gamecubeController);
            Event_ViewChanged(AppView.Calibration);
        }

        public void ShowControllerEditor(AdapterPort port, GamecubeController gamecubeController)
        {
            Event_ShowControllerEditor(port, gamecubeController);

            switch (port)
            {
                case AdapterPort.One:
                    Event_ViewChanged(AppView.Controller1);
                    break;
                case AdapterPort.Two:
                    Event_ViewChanged(AppView.Controller2);
                    break;
                case AdapterPort.Three:
                    Event_ViewChanged(AppView.Controller3);
                    break;
                case AdapterPort.Four:
                    Event_ViewChanged(AppView.Controller4);
                    break;
            }
        }
    }
}
