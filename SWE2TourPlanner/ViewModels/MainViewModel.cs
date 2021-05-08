using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using SWE2TourPlanner.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ITourItemFactory tourItemFactory;
        private TourItem currentItem;
        private LogItem currentLog;
        private string searchText;

        private ICommand popUpAddCommand;
        private ICommand deleteTourCommand;
        private ICommand popUpModifyTourCommand;
        private ICommand popUpAddLogCommand;
        private ICommand deleteLogCommand;
        private ICommand popUpModifyLogCommand;
        private ICommand searchCommand;
        private ICommand clearCommand;


        public ICommand PopUpAddCommand => popUpAddCommand ??= new RelayCommand(PopUpAdd);
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(DeleteTour);
        public ICommand PopUpModifyTourCommand => popUpModifyTourCommand ??= new RelayCommand(PopUpModify);
        public ICommand PopUpAddLogCommand => popUpAddLogCommand ??= new RelayCommand(PopUpAddLog);
        public ICommand DeleteLogCommand => deleteLogCommand ??= new RelayCommand(DeleteLog);
        public ICommand PopUpModifyLogCommand => popUpModifyLogCommand ??= new RelayCommand(PopUpModifyLog);
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => clearCommand ??= new RelayCommand(Clear);

        //public TourAddViewModel tourAddViewModel;
        public LogAddViewModel logAddViewModel;
        public TourModifyViewModel tourModifyViewModel;
        public LogModifyViewModel logModifyViewModel;

        public ObservableCollection<TourItem> TourItems { get; set; }
        public ObservableCollection<LogItem> LogItems { get; set; }

        public LogItem CurrentLog
        {
            get
            {
                return currentLog;
            }
            set
            {
                if ((currentLog != value) && (value != null))
                {
                    currentLog = value;
                    RaisePropertyChangedEvent(nameof(CurrentLog));
                }
            }
        }

        public TourItem CurrentItem
        {
            get 
            {  
                return currentItem;
            }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                    LogItems.Clear();
                    FillDataGrid(CurrentItem.TourId);
                }
            }
        }

        public MainViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
            this.tourItemFactory.DeleteImages();
            InitListBox();
            InitDataGrid();
        }

        private void InitDataGrid()
        {
            LogItems = new ObservableCollection<LogItem>();
        }

        private void FillDataGrid(int tourid)
        {
            foreach (LogItem item in this.tourItemFactory.GetLogs(tourid))
            {
                LogItems.Add(item);
            }
        }

        private void InitListBox()
        {
            TourItems = new ObservableCollection<TourItem>();
            FillListBox();
        }

        private void FillListBox()
        {
            foreach (TourItem item in this.tourItemFactory.GetItems())
            {
                TourItems.Add(item);
            }
        }

        private void PopUpAdd(object commandParameter)
        {
            //this.tourAddViewModel = new TourAddViewModel();
            TourAddWindow view = new TourAddWindow();
            view.DataContext = new TourAddViewModel();
            view.ShowDialog();

            TourItems.Clear();
            FillListBox();
        }

        private void PopUpAddLog(object commandParameter)
        {
            if(CurrentItem != null)
            {
                this.logAddViewModel = new LogAddViewModel();
                logAddViewModel.CurrentTour = CurrentItem;
                LogAddWindow view = new LogAddWindow();
                view.DataContext = this.logAddViewModel;

                view.ShowDialog();

                LogItems.Clear();
                FillDataGrid(CurrentItem.TourId);
            }
        }

        private void DeleteTour(object commandParameter)
        {
            string path = CurrentItem.ImagePath;
            this.tourItemFactory.DeleteItemAndSavePath(CurrentItem.TourId, path);
            TourItems.Clear();
            FillListBox();
            LogItems.Clear();
        }

        private void DeleteLog(object commandParameter)
        {
            this.tourItemFactory.DeleteLog(CurrentLog.LogId);
            LogItems.Clear();
            FillDataGrid(CurrentItem.TourId);
        }

        private void PopUpModify(object commandParameter)
        {
            if(CurrentItem != null)
            {
                this.tourModifyViewModel = new TourModifyViewModel();
                tourModifyViewModel.CurrentTour = CurrentItem;
                TourModifyWindow view = new TourModifyWindow();
                view.DataContext = this.tourModifyViewModel;

                view.ShowDialog();

                TourItems.Clear();
                FillListBox();
            }
        }

        private void PopUpModifyLog(object commandParameter)
        {
            if (CurrentLog != null)
            {
                this.logModifyViewModel = new LogModifyViewModel();
                logModifyViewModel.CurrentLog = CurrentLog;
                LogModifyWindow view = new LogModifyWindow();
                view.DataContext = this.logModifyViewModel;

                view.ShowDialog();

                LogItems.Clear();
                FillDataGrid(CurrentItem.TourId);
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    RaisePropertyChangedEvent(nameof(SearchText));
                }
            }
        }

        private void Search(object commandParameter)
        {
            IEnumerable foundItems = this.tourItemFactory.Search(SearchText);
            TourItems.Clear();
            foreach (TourItem item in foundItems)
            {
                TourItems.Add(item);
            }
        }

        private void Clear(object commandParameter)
        {
            TourItems.Clear();
            SearchText = "";
            FillListBox();
        }
    }
}
