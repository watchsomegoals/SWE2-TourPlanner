using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class LogAddViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string dateTime;
        private string report;
        private string distance;
        private string totalTime;
        private string rating;
        private string avgSpeed;
        private string inclination;
        private string topSpeed;
        private string maxHeight;
        private string minHeight;
        private TourItem currentTour;
        

        private ITourItemFactory tourItemFactory;
        private ICommand addLogCommand;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public ICommand AddLogCommand => addLogCommand ??= new RelayCommand(AddLog);

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

        public string DateTime
        {
            get { return dateTime; }
            set
            {
                if ((dateTime != value) && (value != null))
                {
                    dateTime = value;
                    RaisePropertyChangedEvent(nameof(DateTime));
                }
            }
        }

        public string Report
        {
            get { return report; }
            set
            {
                if ((report != value) && (value != null))
                {
                    report = value;
                    RaisePropertyChangedEvent(nameof(Report));
                }
            }
        }

        public string Distance
        {
            get { return distance; }
            set
            {
                if ((distance != value) && (value != null))
                {
                    distance = value;
                    RaisePropertyChangedEvent(nameof(Distance));
                }
            }
        }

        public string TotalTime
        {
            get { return totalTime; }
            set
            {
                if ((totalTime != value) && (value != null))
                {
                    totalTime = value;
                    ValidateTotalTime();
                    RaisePropertyChangedEvent(nameof(TotalTime));
                }
            }
        }

        public string Rating
        {
            get { return rating; }
            set
            {
                if ((rating != value) && (value != null))
                {
                    rating = value;
                    ValidateRating();
                    RaisePropertyChangedEvent(nameof(Rating));
                }
            }
        }

        public string AvgSpeed
        {
            get { return avgSpeed; }
            set
            {
                if ((avgSpeed != value) && (value != null))
                {
                    avgSpeed = value;
                    ValidateAvgSpeed();
                    RaisePropertyChangedEvent(nameof(AvgSpeed));
                }
            }
        }

        public string Inclination
        {
            get { return inclination; }
            set
            {
                if ((inclination != value) && (value != null))
                {
                    inclination = value;
                    ValidateInclination();
                    RaisePropertyChangedEvent(nameof(Inclination));
                }
            }
        }

        public string TopSpeed
        {
            get { return topSpeed; }
            set
            {
                if ((topSpeed != value) && (value != null))
                {
                    topSpeed = value;
                    ValidateTopSpeed();
                    RaisePropertyChangedEvent(nameof(TopSpeed));
                }
            }
        }

        public string MaxHeight
        {
            get { return maxHeight; }
            set
            {
                if ((maxHeight != value) && (value != null))
                {
                    maxHeight = value;
                    ValidateMaxHeight();
                    RaisePropertyChangedEvent(nameof(MaxHeight));
                }
            }
        }

        public string MinHeight
        {
            get { return minHeight; }
            set
            {
                if ((minHeight != value) && (value != null))
                {
                    minHeight = value;
                    ValidateMinHeight();
                    RaisePropertyChangedEvent(nameof(MinHeight));
                }
            }
        }

        public bool HasErrors => _errorsByPropertyName.Any();

        public LogAddViewModel()
        {
            this.tourItemFactory = TourItemFactory.GetInstance();
        }

        private void AddLog(object commandParameter)
        {
            if(ValidateTotalTime())
            {
                this.tourItemFactory.AddLog(CurrentTour.TourId, DateTime, Report, Distance, TotalTime, Rating, AvgSpeed, Inclination, TopSpeed, MaxHeight, MinHeight);
                var window = (Window)commandParameter;
                window.Close();
            }
            else
            {
                ValidateTotalTime();
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

        private bool ValidateTotalTime()
        {
            Regex regex = new Regex(@"^(?:[01]\d|2[0123]):(?:[012345]\d):(?:[012345]\d)$");
            ClearErrors(nameof(TotalTime));

            if (string.IsNullOrEmpty(TotalTime))
            {
                AddError(nameof(TotalTime), "Total Time cannot be empty.");
                return false;
            }
            if (!regex.IsMatch(TotalTime))
            {
                AddError(nameof(TotalTime), "Total Time Format must be HH:MM:SS.");
                return false;
            }
            return true;
        }

        private bool ValidateRating()
        {
            bool res;
            int rating;
            res = int.TryParse(Rating, out rating);
            ClearErrors(nameof(Rating));

            if (string.IsNullOrWhiteSpace(Rating))
            {
                AddError(nameof(Rating), "Rating cannot be empty.");
                return false;
            }
            if (!res)
            {
                AddError(nameof(Rating), "Rating has to be an integer.");
                return false;
            }
            else
            {
                if ((rating < 1) || (rating > 10))
                {
                    AddError(nameof(Rating), "Rating has to be between 1 and 10.");
                    return false;
                }
            }
            return true;
        }

        private bool ValidateAvgSpeed()
        {
            bool res;
            int avgSpeed;
            res = int.TryParse(AvgSpeed, out avgSpeed);
            ClearErrors(nameof(AvgSpeed));

            if (string.IsNullOrWhiteSpace(AvgSpeed))
            {
                AddError(nameof(AvgSpeed), "Average Speed cannot be empty.");
                return false;
            }
            if (!res)
            {
                AddError(nameof(AvgSpeed), "Average Speed has to be an integer.");
                return false;
            }
            else
            {
                if ((avgSpeed < 1) || (avgSpeed > 200))
                {
                    AddError(nameof(AvgSpeed), "Average Speed has to be between 1 and 200 km/h.");
                    return false;
                }
            }
            return true;
        }

        private bool ValidateInclination()
        {
            bool res;
            int inclination;
            res = int.TryParse(Inclination, out inclination);
            ClearErrors(nameof(Inclination));

            if (string.IsNullOrWhiteSpace(Inclination))
            {
                AddError(nameof(Inclination), "Inclination cannot be empty.");
                return false;
            }
            if (!res)
            {
                AddError(nameof(Inclination), "Inclination has to be an integer.");
                return false;
            }
            else
            {
                if ((inclination < 1) || (inclination > 70))
                {
                    AddError(nameof(Inclination), "Inclination has to be between 1 and 70 degrees.");
                    return false;
                }
            }
            return true;
        }

        private bool ValidateTopSpeed()
        {
            bool res;
            int topSpeed;
            res = int.TryParse(TopSpeed, out topSpeed);
            ClearErrors(nameof(TopSpeed));

            if (string.IsNullOrWhiteSpace(TopSpeed))
            {
                AddError(nameof(TopSpeed), "Top Speed cannot be empty.");
                return false;
            }
            if (!res)
            {
                AddError(nameof(TopSpeed), "Top Speed has to be an integer.");
                return false;
            }
            else
            {
                if ((topSpeed < 1) || (topSpeed > 300))
                {
                    AddError(nameof(TopSpeed), "Top Speed has to be between 1 and 300 km/h.");
                    return false;
                }
            }
            return true;
        }

        private bool ValidateMaxHeight()
        {
            bool res;
            int maxHeight;
            res = int.TryParse(MaxHeight, out maxHeight);
            ClearErrors(nameof(MaxHeight));

            if (string.IsNullOrWhiteSpace(MaxHeight))
            {
                AddError(nameof(MaxHeight), "Max Height cannot be empty.");
                return false;
            }
            if (!res)
            {
                AddError(nameof(MaxHeight), "Max Height has to be an integer.");
                return false;
            }
            else
            {
                if ((maxHeight < 1) || (maxHeight > 10000))
                {
                    AddError(nameof(MaxHeight), "Max Height has to be between 1 and 10000 metres.");
                    return false;
                }
            }
            return true;
        }

        private bool ValidateMinHeight()
        {
            bool res;
            int minHeight;
            res = int.TryParse(MinHeight, out minHeight);
            ClearErrors(nameof(MinHeight));

            if (string.IsNullOrWhiteSpace(MinHeight))
            {
                AddError(nameof(MinHeight), "Min Height cannot be empty.");
                return false;
            }
            if (!res)
            {
                AddError(nameof(MinHeight), "Min Height has to be an integer.");
                return false;
            }
            else
            {
                if ((minHeight < 1) || (minHeight > 10000))
                {
                    AddError(nameof(MinHeight), "Min Height has to be between 1 and 10000 metres.");
                    return false;
                }
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
