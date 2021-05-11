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
        private bool isPopUpDeleteTourVisible;
        private bool isPopUpModifyTourVisible;
        private bool isPopUpDeleteLogVisible;
        private bool isPopUpModifyLogVisible;
        private bool isPopUpAddLogVisible;
        private bool isPopUpCreatePdfVisible;

        private ICommand popUpAddCommand;
        private ICommand deleteTourCommand;
        private ICommand popUpModifyTourCommand;
        private ICommand popUpAddLogCommand;
        private ICommand deleteLogCommand;
        private ICommand popUpModifyLogCommand;
        private ICommand searchCommand;
        private ICommand clearCommand;
        private ICommand openDeleteTourPopUpCommand;
        private ICommand closeDeleteTourPopUpCommand;
        private ICommand openModifyTourPopUpCommand;
        private ICommand closeModifyTourPopUpCommand;
        private ICommand openDeleteLogPopUpCommand;
        private ICommand closeDeleteLogPopUpCommand;
        private ICommand openModifyLogPopUpCommand;
        private ICommand closeModifyLogPopUpCommand;
        private ICommand openAddLogPopUpCommand;
        private ICommand closeAddLogPopUpCommand;
        private ICommand createPdfCommand;
        private ICommand openCreatePdfPopUpCommand;
        private ICommand closeCreatePdfPopUpCommand;
        private ICommand exportCommand;
        private ICommand importCommand;

        public ICommand PopUpAddCommand => popUpAddCommand ??= new RelayCommand(PopUpAdd);
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(DeleteTour);
        public ICommand PopUpModifyTourCommand => popUpModifyTourCommand ??= new RelayCommand(PopUpModify);
        public ICommand PopUpAddLogCommand => popUpAddLogCommand ??= new RelayCommand(PopUpAddLog);
        public ICommand DeleteLogCommand => deleteLogCommand ??= new RelayCommand(DeleteLog);
        public ICommand PopUpModifyLogCommand => popUpModifyLogCommand ??= new RelayCommand(PopUpModifyLog);
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => clearCommand ??= new RelayCommand(Clear);
        public ICommand OpenDeleteTourPopUpCommand => openDeleteTourPopUpCommand ??= new RelayCommand(OpenDeleteTourPopUp);
        public ICommand CloseDeleteTourPopUpCommand => closeDeleteTourPopUpCommand ??= new RelayCommand(CloseDeleteTourPopUp);
        public ICommand OpenModifyTourPopUpCommand => openModifyTourPopUpCommand ??= new RelayCommand(OpenModifyTourPopUp);
        public ICommand CloseModifyTourPopUpCommand => closeModifyTourPopUpCommand ??= new RelayCommand(CloseModifyTourPopUp);
        public ICommand OpenDeleteLogPopUpCommand => openDeleteLogPopUpCommand ??= new RelayCommand(OpenDeleteLogPopUp);
        public ICommand CloseDeleteLogPopUpCommand => closeDeleteLogPopUpCommand ??= new RelayCommand(CloseDeleteLogPopUp);
        public ICommand OpenModifyLogPopUpCommand => openModifyLogPopUpCommand ??= new RelayCommand(OpenModifyLogPopUp);
        public ICommand CloseModifyLogPopUpCommand => closeModifyLogPopUpCommand ??= new RelayCommand(CloseModifyLogPopUp);
        public ICommand OpenAddLogPopUpCommand => openAddLogPopUpCommand ??= new RelayCommand(OpenAddLogPopUp);
        public ICommand CloseAddLogPopUpCommand => closeAddLogPopUpCommand ??= new RelayCommand(CloseAddLogPopUp);
        public ICommand CreatePdfCommand => createPdfCommand ??= new RelayCommand(CreatePdf);
        public ICommand OpenCreatePdfPopUpCommand => openCreatePdfPopUpCommand ??= new RelayCommand(OpenCreatePdfPopUp);
        public ICommand CloseCreatePdfPopUpCommand => closeCreatePdfPopUpCommand ??= new RelayCommand(CloseCreatePdfPopUp);
        public ICommand ExportCommand => exportCommand ??= new RelayCommand(Export);
        public ICommand ImportCommand => importCommand ??= new RelayCommand(Import);

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

        public bool IsPopUpCreatePdfVisible
        {
            get { return isPopUpCreatePdfVisible; }
            set
            {
                if (isPopUpCreatePdfVisible != value)
                {
                    isPopUpCreatePdfVisible = value;
                    RaisePropertyChangedEvent(nameof(IsPopUpCreatePdfVisible));
                }
            }
        }

        public bool IsPopUpAddLogVisible
        {
            get { return isPopUpAddLogVisible; }
            set
            {
                if (isPopUpAddLogVisible != value)
                {
                    isPopUpAddLogVisible = value;
                    RaisePropertyChangedEvent(nameof(IsPopUpAddLogVisible));
                }
            }
        }

        public bool IsPopUpDeleteTourVisible
        {
            get { return isPopUpDeleteTourVisible; }
            set
            {
                if(isPopUpDeleteTourVisible != value)
                {
                    isPopUpDeleteTourVisible = value;
                    RaisePropertyChangedEvent(nameof(IsPopUpDeleteTourVisible));
                }
            }
        }

        public bool IsPopUpModifyTourVisible
        {
            get { return isPopUpModifyTourVisible; }
            set
            {
                if(isPopUpModifyTourVisible != value)
                {
                    isPopUpModifyTourVisible = value;
                    RaisePropertyChangedEvent(nameof(IsPopUpModifyTourVisible));
                }
            }
        }

        public bool IsPopUpDeleteLogVisible
        {
            get { return isPopUpDeleteLogVisible; }
            set
            {
                if (isPopUpDeleteLogVisible != value)
                {
                    isPopUpDeleteLogVisible = value;
                    RaisePropertyChangedEvent(nameof(IsPopUpDeleteLogVisible));
                }
            }
        }

        public bool IsPopUpModifyLogVisible
        {
            get { return isPopUpModifyLogVisible; }
            set
            {
                if (isPopUpModifyLogVisible != value)
                {
                    isPopUpModifyLogVisible = value;
                    RaisePropertyChangedEvent(nameof(IsPopUpModifyLogVisible));
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
            if(!IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                //this.tourAddViewModel = new TourAddViewModel();
                TourAddWindow view = new TourAddWindow();
                view.DataContext = new TourAddViewModel();
                view.ShowDialog();

                TourItems.Clear();
                FillListBox();
            }
        }

        private void PopUpAddLog(object commandParameter)
        {
            if(CurrentItem != null && !IsPopUpAddLogVisible && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpCreatePdfVisible)
            {
                this.logAddViewModel = new LogAddViewModel();
                logAddViewModel.CurrentTour = CurrentItem;
                LogAddWindow view = new LogAddWindow();
                view.DataContext = this.logAddViewModel;

                view.ShowDialog();

                LogItems.Clear();
                FillDataGrid(CurrentItem.TourId);
            }
            else if(CurrentItem == null && !IsPopUpAddLogVisible && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpCreatePdfVisible)
            {
                OpenAddLogPopUp(commandParameter);
            }
        }

        private void DeleteTour(object commandParameter)
        {
            if(CurrentItem != null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                string path = CurrentItem.ImagePath;
                this.tourItemFactory.DeleteItemAndSavePath(CurrentItem.TourId, path);
                TourItems.Clear();
                FillListBox();
                LogItems.Clear();
            }
            else if(CurrentItem == null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                OpenDeleteTourPopUp(commandParameter);
            }
        }
        
        private void DeleteLog(object commandParameter)
        {
            if (CurrentLog != null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                this.tourItemFactory.DeleteLog(CurrentLog.LogId);
                LogItems.Clear();
                FillDataGrid(CurrentItem.TourId);
            }
            else if (CurrentLog == null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                OpenDeleteLogPopUp(commandParameter);
            }
        }

        private void PopUpModify(object commandParameter)
        {
            if (CurrentItem != null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                this.tourModifyViewModel = new TourModifyViewModel();
                tourModifyViewModel.CurrentTour = CurrentItem;
                TourModifyWindow view = new TourModifyWindow();
                view.DataContext = this.tourModifyViewModel;

                view.ShowDialog();

                TourItems.Clear();
                FillListBox();
            }
            else if (CurrentItem == null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                OpenModifyTourPopUp(commandParameter);
            }
        }

        private void PopUpModifyLog(object commandParameter)
        {
            if (CurrentLog != null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                this.logModifyViewModel = new LogModifyViewModel();
                logModifyViewModel.CurrentLog = CurrentLog;
                LogModifyWindow view = new LogModifyWindow();
                view.DataContext = this.logModifyViewModel;

                view.ShowDialog();

                LogItems.Clear();
                FillDataGrid(CurrentItem.TourId);
            }
            else if (CurrentLog == null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                OpenModifyLogPopUp(commandParameter);
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
            if(!string.IsNullOrEmpty(SearchText) && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                IEnumerable foundItems = this.tourItemFactory.Search(SearchText);
                TourItems.Clear();
                foreach (TourItem item in foundItems)
                {
                    TourItems.Add(item);
                }
            }
        }

        private void Clear(object commandParameter)
        {
            if(!IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                TourItems.Clear();
                SearchText = "";
                FillListBox();
            }
        }

        private void CreatePdf(object commandParameter)
        {
            if(CurrentItem != null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                tourItemFactory.CreatePdf(CurrentItem);
            }
            else if(CurrentItem == null && !IsPopUpDeleteTourVisible && !IsPopUpModifyTourVisible && !IsPopUpDeleteLogVisible && !IsPopUpModifyLogVisible && !IsPopUpAddLogVisible && !IsPopUpCreatePdfVisible)
            {
                OpenCreatePdfPopUp(commandParameter);
            }
        }

        private void Export(object commandParameter)
        {
            tourItemFactory.Export();
        }

        private void Import(object commandParameter)
        {
            string filePath;
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                filePath = openFileDlg.FileName;
                tourItemFactory.Import(filePath);
            }
            TourItems.Clear();
            FillListBox();
        }

        private void OpenDeleteTourPopUp(object commandParameter)
        {
            IsPopUpDeleteTourVisible = true;
        }

        private void CloseDeleteTourPopUp(object commandParameter)
        {
            IsPopUpDeleteTourVisible = false;
        }

        private void OpenModifyTourPopUp(object commandParameter)
        {
            IsPopUpModifyTourVisible = true;
        }

        private void CloseModifyTourPopUp(object commandParameter)
        {
            IsPopUpModifyTourVisible = false;
        }

        private void OpenDeleteLogPopUp(object commandParameter)
        {
            IsPopUpDeleteLogVisible = true;
        }

        private void CloseDeleteLogPopUp(object commandParameter)
        {
            IsPopUpDeleteLogVisible = false;
        }

        private void OpenModifyLogPopUp(object commandParameter)
        {
            IsPopUpModifyLogVisible = true;
        }

        private void CloseModifyLogPopUp(object commandParameter)
        {
            IsPopUpModifyLogVisible = false;
        }

        private void OpenAddLogPopUp(object commandParameter)
        {
            IsPopUpAddLogVisible = true;
        }

        private void CloseAddLogPopUp(object commandParameter)
        {
            IsPopUpAddLogVisible = false;
        }

        private void OpenCreatePdfPopUp(object commandParameter)
        {
            IsPopUpCreatePdfVisible = true;
        }

        private void CloseCreatePdfPopUp(object commandParameter)
        {
            IsPopUpCreatePdfVisible = false;
        }
    }
}
