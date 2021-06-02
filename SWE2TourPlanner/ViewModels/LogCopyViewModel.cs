using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class LogCopyViewModel : ViewModelBase
    {
        public ObservableCollection<TourItem> TourItems { get; set; }
        private LogItem currentLog;
        private TourItem currentTour;
        private ITourItemFactory tourItemFactory;
        private ICommand copyLogCommand;

        public LogItem CurrentLog
        {
            get { return currentLog; }
            set
            {
                if ((currentLog != value) && (value != null))
                {
                    currentLog = value;
                    RaisePropertyChangedEvent(nameof(CurrentLog));
                }
            }
        }

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

        public ICommand CopyLogCommand => copyLogCommand ??= new RelayCommand(CopyLog);

        private void CopyLog(object commandParameter)
        {
            if(CurrentTour != null)
            {
                this.tourItemFactory.AddLog(CurrentTour.TourId, CurrentLog.DateTime, CurrentLog.Report, CurrentLog.Distance, CurrentLog.TotalTime, CurrentLog.Rating, CurrentLog.AvgSpeed, CurrentLog.Inclination, CurrentLog.TopSpeed, CurrentLog.MaxHeight, CurrentLog.MinHeight);
                var window = (Window)commandParameter;
                window.Close();
            }
        }

        public LogCopyViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
        }
    }
}
