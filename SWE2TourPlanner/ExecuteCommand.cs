using SWE2TourPlanner.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner
{
    public class ExecuteCommand : ICommand
    {
        private readonly MainViewModel _mainViewModel;

        public ExecuteCommand(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            TourAddWindow view = new TourAddWindow();
            view.DataContext = _mainViewModel.tourAddViewModel;

            view.ShowDialog();
        }
    }
}
