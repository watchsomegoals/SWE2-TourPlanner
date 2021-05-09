using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class TourAddViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string newTourText;
        private string from;
        private string to;
        private string description;
        private RouteType currentRoute;
        private ITourItemFactory tourItemFactory;
        private ICommand addTourCommand;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);

        public ObservableCollection<RouteType> RouteItems { get; set; }

        public RouteType CurrentRoute
        {
            get
            {
                return currentRoute;
            }
            set
            {
                if ((currentRoute != value) && (value != null))
                {
                    currentRoute = value;
                    CheckInputRoute();
                    RaisePropertyChangedEvent(nameof(CurrentRoute));
                }
            }
        }

        public string NewTourText
        {
            get 
            { return newTourText; }
            set
            {
                if ((newTourText != value) && (value != null))
                {
                    newTourText = value;
                    CheckInputNewTourText();
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
                    CheckInputFrom();
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
                    CheckInputTo();
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
                    CheckInputDescription();
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }

        public bool HasErrors => _errorsByPropertyName.Any();

        public TourAddViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
            InitRouteTypes();
        }

        private void InitRouteTypes()
        {
            RouteItems = new ObservableCollection<RouteType>()
            {
                new RouteType(){TypeRoute="fastest", Display="Fastest"},
                new RouteType(){TypeRoute="shortest", Display="Shortest"},
                new RouteType(){TypeRoute="pedestrian", Display="Pedestrian"},
                new RouteType(){TypeRoute="bicycle", Display="Bicycle"}
            };
        }

        private void AddTour(object commandParameter)
        {
            if (!string.IsNullOrEmpty(NewTourText) && !string.IsNullOrEmpty(From) && !string.IsNullOrEmpty(To) && !string.IsNullOrEmpty(Description) && CurrentRoute != null)
            {
                this.tourItemFactory.AddItem(NewTourText, From, To, Description, CurrentRoute.TypeRoute);
                var window = (Window)commandParameter;
                NewTourText = string.Empty;
                From = string.Empty;
                To = string.Empty;
                Description = string.Empty;
                window.Close();
            }
            else
            {
                CheckInputNewTourText();
                CheckInputFrom();
                CheckInputTo();
                CheckInputDescription();
                CheckInputRoute();
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
            _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool CheckInputRoute()
        {
            ClearErrors(nameof(CurrentRoute));
            if (CurrentRoute == null)
            {
                AddError(nameof(CurrentRoute), "Route Type cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckInputNewTourText()
        {
            ClearErrors(nameof(NewTourText));
            if (string.IsNullOrWhiteSpace(NewTourText))
            {
                AddError(nameof(NewTourText), "Name cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckInputFrom()
        {
            ClearErrors(nameof(From));
            if (string.IsNullOrWhiteSpace(From))
            {
                AddError(nameof(From), "Starting Point cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckInputTo()
        {
            ClearErrors(nameof(To));
            if (string.IsNullOrWhiteSpace(To))
            {
                AddError(nameof(To), "Destination cannot be empty.");
                return false;
            }
            return true;
        }

        public bool CheckInputDescription()
        {
            ClearErrors(nameof(Description));
            if (string.IsNullOrWhiteSpace(Description))
            {
                AddError(nameof(Description), "Description cannot be empty.");
                return false;
            }
            return true;
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
