using SWE2TourPlanner.BusinessLayer;
using System;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class TourAddViewModel : ViewModelBase
    {
        private string newTourText;
        private ITourItemFactory tourItemFactory;
        private ICommand addTourCommand;

        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);
        
        public string NewTourText
        {
            get { return newTourText; }
            set
            {
                if ((newTourText != value) && (value != null))
                {
                    newTourText = value;
                    RaisePropertyChangedEvent(nameof(NewTourText));
                }
            }
        }

        public TourAddViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
        }

        private void AddTour(object commandParameter)
        {
            this.tourItemFactory.AddItem(NewTourText);
            

        }

    }
}
