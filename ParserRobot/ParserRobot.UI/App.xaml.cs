using Autofac;
using ParserRobot.UI.Container;
using System.Windows;

namespace ParserRobot.UI
{
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container = ContainerConfig.Configure();
        }
    }
}