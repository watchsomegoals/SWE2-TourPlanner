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
            float price = 0;
            mainViewModel.CustOrder = "A pizza with";
            if (mainViewModel.IsCheckedBoxSalami)
            {
                mainViewModel.CustOrder += " salami";
                price += 2;
            }
            if (mainViewModel.IsCheckedBoxProsciutto)
            {
                mainViewModel.CustOrder += " prosciutto";
                price += 3;
            }
            if (mainViewModel.IsCheckedBoxCorn)
            {
                mainViewModel.CustOrder += " corn";
                price += 1;
            }
            if (mainViewModel.IsCheckedBoxBacon)
            {
                mainViewModel.CustOrder += " bacon";
                price += 3;
            }
            if (mainViewModel.IsCheckedBoxMushrooms)
            {
                mainViewModel.CustOrder += " mushrooms";
                price += 2;
            }
            mainViewModel.Price = price + (price * mainViewModel.Tax/100);
        }
    }
}
