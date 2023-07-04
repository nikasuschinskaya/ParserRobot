using Autofac;
using Microsoft.Extensions.Logging;
using ParserRobot.BLL.Workers;
using ParserRobot.UI.Commands;
using ParserRobot.UI.ViewModels.Base;
using System.Windows.Input;

namespace ParserRobot.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Worker _worker;
        private readonly ILogger<MainViewModel> _logger;

        public ICommand OnClickStartButton { get; }

        public MainViewModel()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                _logger = scope.Resolve<ILogger<MainViewModel>>();
                _worker = scope.Resolve<Worker>();
            }
            OnClickStartButton = new RelayCommand(StartButtonClicked);
        }

        private void StartButtonClicked(object parameter)
        {
            _logger.LogInformation("Робот начал работу");
            _worker.StartWorkAsync();
        }
    }
}
