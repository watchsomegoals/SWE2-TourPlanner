using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SWE2TourPlanner
{
    public class ExecuteCommand : ICommand
    {
        private readonly MainViewModel mainViewModel;

        public ExecuteCommand(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
        }
    }
}
