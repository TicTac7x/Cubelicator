using System.Windows;

namespace ConsoleApp1
{
    public partial class App : System.Windows.Application
    {
        private readonly Adapter _adapter;
        private NotifyIcon _notifyIcon;

        public App()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _adapter = new Adapter();
            CreateTrayIcon();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _adapter?.Stop();
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            base.OnExit(e);
        }

        private void CreateTrayIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = "My Controller App",
                Visible = true
            };

            var menu = new ContextMenuStrip();

            menu.Items.Add("Exit", null, (_, _) =>
            {
                Shutdown();
            });

            _notifyIcon.ContextMenuStrip = menu;
        }

        public static void DebugOutput(object a)
        {
            System.Diagnostics.Debug.WriteLine(a);
        }
    }
}