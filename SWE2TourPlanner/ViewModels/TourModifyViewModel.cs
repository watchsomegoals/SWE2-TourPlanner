using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class TourModifyViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string description;
        private RouteType currentRoute;
        private TourItem currentTour;
        private ITourItemFactory tourItemFactory;
        private ICommand modifyTourCommand;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public ICommand ModifyTourCommand => modifyTourCommand ??= new RelayCommand(ModifyTour);
        
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

        public TourModifyViewModel()
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

        private void ModifyTour(object commandParameter)
        {
            if (!string.IsNullOrEmpty(Description) && CurrentRoute != null)
            {
                string logs = "Tour with id: " + CurrentTour.TourId + " modified.";
                log.Info(logs);

                this.tourItemFactory.ModifyTour(CurrentTour, Description, CurrentRoute.TypeRoute);
                var window = (Window)commandParameter;
                window.Close();
            }
            else
            {
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
