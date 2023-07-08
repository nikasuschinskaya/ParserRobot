using Autofac;
using Microsoft.Extensions.Logging;
using ParserRobot.BLL.Workers;
using ParserRobot.UI.Commands;
using ParserRobot.UI.ViewModels.Base;
using System.Globalization;
using System.Windows.Forms;
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

        private async void StartButtonClicked(object parameter)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
            _logger.LogInformation("Робот начал работу");
            await _worker.StartWorkAsync();
        }
    }
}
