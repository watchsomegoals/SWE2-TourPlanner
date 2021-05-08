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
    public class LogModifyViewModel : ViewModelBase
    {
        private string newEntry;
        private LogItem currentLog;
        private LogDataTypes currentLogDataType;
        private ITourItemFactory tourItemFactory;
        private ICommand modifyLogCommand;

        public ICommand ModifyLogCommand => modifyLogCommand ??= new RelayCommand(ModifyLog);

        public ObservableCollection<LogDataTypes> LogDataTypesItems { get; set; }

        public LogDataTypes CurrentLogDataType
        {
            get
            {
                return currentLogDataType;
            }
            set
            {
                if ((currentLogDataType != value) && (value != null))
                {
                    currentLogDataType = value;
                    RaisePropertyChangedEvent(nameof(CurrentLogDataType));
                }
            }
        }

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

        public string NewEntry
        {
            get { return newEntry; }
            set
            {
                if ((newEntry != value) && (value != null))
                {
                    newEntry = value;
                    RaisePropertyChangedEvent(nameof(NewEntry));
                }
            }
        }

        public LogModifyViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
            InitLogDataTypes();
        }

        private void InitLogDataTypes()
        {
            LogDataTypesItems = new ObservableCollection<LogDataTypes>()
            {
                new LogDataTypes(){TypeLogData="datetime", Display="Date/Time"},
                new LogDataTypes(){TypeLogData="report", Display="Report"},
                new LogDataTypes(){TypeLogData="distance", Display="Distance"},
                new LogDataTypes(){TypeLogData="totaltime", Display="Total Time"},
                new LogDataTypes(){TypeLogData="rating", Display="Rating"},
                new LogDataTypes(){TypeLogData="avgspeed", Display="Average Speed"},
                new LogDataTypes(){TypeLogData="inclination", Display="Inclination"},
                new LogDataTypes(){TypeLogData="topspeed", Display="Top Speed"},
                new LogDataTypes(){TypeLogData="maxheight", Display="Max Height"},
                new LogDataTypes(){TypeLogData="minheight", Display="Min Height"}
            };
        }

        private void ModifyLog(object commandParameter)
        {
            this.tourItemFactory.ModifyLog(CurrentLog, CurrentLogDataType.TypeLogData, NewEntry);
            var window = (Window)commandParameter;
            window.Close();
        }
    }
}
