using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace SWE2TourPlanner
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public TourAddViewModel tourAddViewModel;

        public List<Tour> _data = new List<Tour>();

        public event PropertyChangedEventHandler PropertyChanged;
        private string _searchText;
        public ICommand ExecuteCommand { get; }
        
        public MainViewModel()
        {
            this.ExecuteCommand = new ExecuteCommand(this);
            this.tourAddViewModel = new TourAddViewModel();
            Data.Add(new Tour("tour1"));
            Data.Add(new Tour("tour2"));
        }

        public List<Tour> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = new List<Tour>(tourAddViewModel.TourList);
                OnPropertyChanged(nameof(Data));
            }
        }

        public string SearchText
        {
            get
            {
                Debug.Print("read search text");
                return _searchText;
            }
            set
            {
                if(SearchText != value)
                {
                    Debug.Print("set customer order value");
                    _searchText = value;

                    Debug.Print("fire propertyChanged: Customer Order");
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.Print($"propertyChanged \"{propertyName}\"");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
