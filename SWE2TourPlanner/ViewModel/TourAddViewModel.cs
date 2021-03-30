using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace SWE2TourPlanner
{
    public class TourAddViewModel : INotifyPropertyChanged
    {
        private List<Tour> _tourList;
        public ICommand ExecuteAddTour { get; }
        public TourAddViewModel()
        {
            this.ExecuteAddTour = new ExecuteAddTour(this);
            this._tourList = new List<Tour>();
        }

        public List<Tour> TourList
        {
            get
            {
                return _tourList;
            }
            set
            {
                _tourList = value;
                OnPropertyChanged(nameof(TourList));
            }
        }

        private string _newTourText;
        public string NewTourText
        {
            get
            {
                Debug.Print("read NewTourText");
                return _newTourText;
            }
            set
            {
                if (NewTourText != value)
                {
                    Debug.Print("set NewTourText value");
                    _newTourText = value;

                    Debug.Print("fire propertyChanged: NewTourText");
                    OnPropertyChanged(nameof(NewTourText));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.Print($"propertyChanged \"{propertyName}\"");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
