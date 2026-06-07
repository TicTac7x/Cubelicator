using System.Windows;

namespace CubeGem
{
    public partial class App : System.Windows.Application
    {
        private readonly Adapter _adapter;
        private NotifyIcon _notifyIcon;

        public App()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _adapter = new Adapter();
            _notifyIcon = CreateTrayIcon();
        }

        protected override void OnExit(ExitEventArgs args)
        {
            _adapter?.Stop();
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            base.OnExit(args);
        }

        private NotifyIcon CreateTrayIcon()
        {
            var notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = Strings.AppName,
                Visible = true
            };

            var menu = new ContextMenuStrip();

            menu.Items.Add(Strings.Exit + " " + Strings.AppName, null, (_, _) =>
            {
                Shutdown();
            });

            notifyIcon.ContextMenuStrip = menu;

            return notifyIcon;
        }

        public static void DebugOutput(object a)
        {
            System.Diagnostics.Debug.WriteLine(a);
        }
    }
}