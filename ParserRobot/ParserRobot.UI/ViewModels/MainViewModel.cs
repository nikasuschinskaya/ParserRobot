using ParserRobot.BLL.Workers;
using ParserRobot.UI.Commands;
using ParserRobot.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ParserRobot.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand OnClickStartButton { get; }

        public MainViewModel()
        {
            OnClickStartButton = new RelayCommand(StartButtonClicked);
        }

        private void StartButtonClicked(object parameter)
        {
            Worker worker = new Worker();
            worker.StartWorkAsync();
            //string s = string.Empty;
            //foreach (var item in worker.GetFiles())
            //{
            //    s += item + "\n";
            //}
            //MessageBox.Show(s);
        }
    }
}
