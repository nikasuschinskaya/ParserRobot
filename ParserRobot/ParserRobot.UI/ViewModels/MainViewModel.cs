using ParserRobot.BLL.Workers;
using ParserRobot.UI.Commands;
using ParserRobot.UI.ViewModels.Base;
using System.Windows.Input;

namespace ParserRobot.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand OnClickStartButton { get; }

        public MainViewModel() => OnClickStartButton = new RelayCommand(StartButtonClicked);

        private void StartButtonClicked(object parameter)
        {
            Worker worker = new Worker();
            worker.StartWorkAsync();
        }
    }
}
