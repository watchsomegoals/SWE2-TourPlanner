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
        private string searchText;

        private ICommand popUpAddCommand;
        private ICommand deleteTourCommand;
        private ICommand popUpAddLogCommand;
        private ICommand searchCommand;
        private ICommand clearCommand;

        public ICommand PopUpAddCommand => popUpAddCommand ??= new RelayCommand(PopUpAdd);
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(DeleteTour);
        public ICommand PopUpAddLogCommand => popUpAddLogCommand ??= new RelayCommand(PopUpAddLog);
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => clearCommand ??= new RelayCommand(Clear);

        //public TourAddViewModel tourAddViewModel;
        public LogAddViewModel logAddViewModel;

        public ObservableCollection<TourItem> TourItems { get; set; }

        public ObservableCollection<LogItem> LogItems { get; set; }

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
            //bool?dialogResult = view.ShowDialog();
            //if(!(bool)dialogResult)
            //{
                TourItems.Clear();
                FillListBox();
            //}
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
