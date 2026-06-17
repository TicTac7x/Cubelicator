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

        public Navigator(ViewsManager viewsManager, GamecubeAdapter gamecubeAdapter)
        {
            this.viewsManager = viewsManager;
            this.gamecubeAdapter = gamecubeAdapter;

            gamecubeAdapter.OnControllerConnectionChanged += (port, connected) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    var button = port switch
                    {
                        AdapterPort.One => Controller1,
                        AdapterPort.Two => Controller2,
                        AdapterPort.Three => Controller3,
                        AdapterPort.Four => Controller4,
                        _ => null
                    };

                    if (button == null)
                        return;

                    button.Visibility = connected ? Visibility.Visible : Visibility.Collapsed;
                });
            };

            InitializeComponent();
            viewsManager.OnViewChanged += OnViewChanged;
        }

        private void OnViewChanged(AppView view)
        {
            Dashboard.Background = IsViewVisible(AppView.Dashboard, view);
            Controller1.Background = IsViewVisible(AppView.Controller1, view);
            Controller2.Background = IsViewVisible(AppView.Controller2, view);
            Controller3.Background = IsViewVisible(AppView.Controller3, view);
            Controller4.Background = IsViewVisible(AppView.Controller4, view);
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

        private void OnNavClick(object sender, RoutedEventArgs args)
        {
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

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border b && b != _active)
            {
                b.Background = App.StringToSolidColorBrush(Colors.ButtonHoverBackground);
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border b && b != _active)
            {
                b.Background = App.StringToSolidColorBrush(Colors.ButtonBackground);
            }
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is not Border b)
                return;

            // reset previous active
            if (_active != null)
            {
                _active.Background = App.StringToSolidColorBrush(Colors.ButtonBackground);
            }

            // set new active
            _active = b;
            _active.Background = App.StringToSolidColorBrush(Colors.ButtonPressedBackground);
        }
    }
}