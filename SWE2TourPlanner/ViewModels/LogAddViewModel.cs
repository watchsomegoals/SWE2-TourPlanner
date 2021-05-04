using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class LogAddViewModel : ViewModelBase
    {
        private string dateTime;
        private string report;
        private string distance;
        private string totalTime;
        private TourItem currentTour;
        

        private ITourItemFactory tourItemFactory;
        private ICommand addLogCommand;

        public ICommand AddLogCommand => addLogCommand ??= new RelayCommand(AddLog);

        public TourItem CurrentTour
        {
            get { return currentTour; }
            set
            {
                if ((currentTour != value) && (value != null))
                {
                    currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }

        public string DateTime
        {
            get { return dateTime; }
            set
            {
                if ((dateTime != value) && (value != null))
                {
                    dateTime = value;
                    RaisePropertyChangedEvent(nameof(DateTime));
                }
            }
        }

        public string Report
        {
            get { return report; }
            set
            {
                if ((report != value) && (value != null))
                {
                    report = value;
                    RaisePropertyChangedEvent(nameof(Report));
                }
            }
        }

        public string Distance
        {
            get { return distance; }
            set
            {
                if ((distance != value) && (value != null))
                {
                    distance = value;
                    RaisePropertyChangedEvent(nameof(Distance));
                }
            }
        }

        public string TotalTime
        {
            get { return totalTime; }
            set
            {
                if ((totalTime != value) && (value != null))
                {
                    totalTime = value;
                    RaisePropertyChangedEvent(nameof(TotalTime));
                }
            }
        }

        public LogAddViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
        }

        private void AddLog(object commandParameter)
        {
            this.tourItemFactory.AddLog(CurrentTour.TourId, DateTime, Report, Distance, TotalTime);
            var window = (Window)commandParameter;
            window.Close();
        }
    }
}
