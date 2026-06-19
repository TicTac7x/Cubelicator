using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace Cubelicator
{
    public partial class Navigator: UserControl
    {
        private readonly ViewsManager viewsManager;
        private readonly GamecubeAdapter gamecubeAdapter;

        private bool initialized = false;

        public Navigator(ViewsManager viewsManager, GamecubeAdapter gamecubeAdapter)
        {
            this.viewsManager = viewsManager;
            this.gamecubeAdapter = gamecubeAdapter;

            InitializeComponent();
            SetupEvents();
            initialized = true;
        }

        private void SetupEvents()
        {
            gamecubeAdapter.Event_ControllerConnectionChanged += (port, connected) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    var button = port switch
                    {
                        AdapterPort.One => Element_Controller1,
                        AdapterPort.Two => Element_Controller2,
                        AdapterPort.Three => Element_Controller3,
                        AdapterPort.Four => Element_Controller4,
                        _ => null
                    };

                    if (button == null)
                        return;

                    button.Visibility = connected ? Visibility.Visible : Visibility.Collapsed;
                });
            };

            viewsManager.Event_ViewChanged += OnViewChanged;
        }

        private void OnViewChanged(AppView view)
        {
            Element_Dashboard.Background = IsViewVisible(AppView.Dashboard, view);
            Element_Controller1.Background = IsViewVisible(AppView.Controller1, view);
            Element_Controller2.Background = IsViewVisible(AppView.Controller2, view);
            Element_Controller3.Background = IsViewVisible(AppView.Controller3, view);
            Element_Controller4.Background = IsViewVisible(AppView.Controller4, view);
            Element_Calibration.Background = IsViewVisible(AppView.Calibration, view);

            Element_Calibration.Visibility = view == AppView.Calibration ? Visibility.Visible : Visibility.Collapsed;
        }

        private Brush IsViewVisible(AppView neededView, AppView currentView)
        {
            if (currentView == neededView)
            {
                return App.StringToSolidColorBrush(Colors.AppBackground);
            } else
            {
                return App.StringToSolidColorBrush(Colors.DeviceBackground);
            }
        }

        private void UIEvent_NavigationItem(object sender, RoutedEventArgs args)
        {
            if (!initialized) return;

            if (sender is not Border border)
                return;

            if (border.Tag is not AppView view)
                return;

            switch (view)
            {
                case AppView.Dashboard:
                    viewsManager.ShowDashboard();
                    break;

                case AppView.Controller1:
                    viewsManager.ShowControllerEditor(
                        AdapterPort.One,
                        gamecubeAdapter.GetPortController(AdapterPort.One));
                    break;

                case AppView.Controller2:
                    viewsManager.ShowControllerEditor(
                        AdapterPort.Two,
                        gamecubeAdapter.GetPortController(AdapterPort.Two));
                    break;

                case AppView.Controller3:
                    viewsManager.ShowControllerEditor(
                        AdapterPort.Three,
                        gamecubeAdapter.GetPortController(AdapterPort.Three));
                    break;

                case AppView.Controller4:
                    viewsManager.ShowControllerEditor(
                        AdapterPort.Four,
                        gamecubeAdapter.GetPortController(AdapterPort.Four));
                    break;
            }
        }

        private Border? _active;

        private void UIEvent_OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            if (!initialized) return;

            if (sender is Border border && border != _active)
            {
                border.Background = App.StringToSolidColorBrush(Colors.ButtonHoverBackground);
            }
        }

        private void UIEvent_OnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            if (!initialized) return;

            if (sender is Border border && border != _active)
            {
                border.Background = App.StringToSolidColorBrush(Colors.ButtonBackground);
            }
        }

        private void UIEvent_OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            if (!initialized) return;

            if (sender is not Border border)
                return;

            // reset previous active
            if (_active != null)
            {
                _active.Background = App.StringToSolidColorBrush(Colors.ButtonBackground);
            }

            // set new active
            _active = border;
            _active.Background = App.StringToSolidColorBrush(Colors.ButtonPressedBackground);
        }
    }
}