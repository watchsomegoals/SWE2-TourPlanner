using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using SWE2TourPlanner.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ITourItemFactory tourItemFactory;
        private TourItem currentItem;

        private ICommand popUpAddCommand;

        public ICommand PopUpAddCommand => popUpAddCommand ??= new RelayCommand(PopUpAdd); 

        public TourAddViewModel tourAddViewModel;

        public ObservableCollection<TourItem> TourItems { get; set; }

        public TourItem CurrentItem
        {
            get { return currentItem; }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                }
            }
        }

        public MainViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
            InitListBox();
            this.tourAddViewModel = new TourAddViewModel();
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
            TourAddWindow view = new TourAddWindow();
            view.DataContext = this.tourAddViewModel;
            bool?dialogResult = view.ShowDialog();
            if(!(bool)dialogResult)
            {
                TourItems.Clear();
                FillListBox();
            }

        }

        

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    Debug.Print($"propertyChanged \"{propertyName}\"");
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
