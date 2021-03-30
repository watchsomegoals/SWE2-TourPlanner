using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner
{
    public class ExecuteAddTour : ICommand
    {
        private readonly TourAddViewModel _tourAddViewModel;

        public ExecuteAddTour(TourAddViewModel tourAddViewModel)
        {
            _tourAddViewModel = tourAddViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Tour tour = new Tour(_tourAddViewModel.NewTourText);
            _tourAddViewModel.TourList.Add(tour);


            MessageBox.Show(_tourAddViewModel.TourList.Count.ToString());
        }
    }
}
