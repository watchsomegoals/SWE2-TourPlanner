using SWE2TourPlanner.BusinessLayer;
using System;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class TourAddViewModel : ViewModelBase
    {
        private string newTourText;
        private string from;
        private string to;
        private string description;
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

        public string From
        {
            get { return from; }
            set
            {
                if ((from != value) && (value != null))
                {
                    from = value;
                    RaisePropertyChangedEvent(nameof(From));
                }
            }
        }

        public string To
        {
            get { return to; }
            set
            {
                if ((to != value) && (value != null))
                {
                    to = value;
                    RaisePropertyChangedEvent(nameof(To));
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if ((description != value) && (value != null))
                {
                    description = value;
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }

        public TourAddViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
        }

        private void AddTour(object commandParameter)
        {
            this.tourItemFactory.AddItem(NewTourText, Description, From, To);
        }

    }
}
