using System.Windows;

namespace ConsoleApp1
{
    public partial class App : Application
    {
        private readonly Adapter _adapter;

        public App()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _adapter = new Adapter();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _adapter?.Stop();
            base.OnExit(e);
        }

        public static void DebugOutput(object a)
        {
            System.Diagnostics.Debug.WriteLine(a);
        }
    }
}