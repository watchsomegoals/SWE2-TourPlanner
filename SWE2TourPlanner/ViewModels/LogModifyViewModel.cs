using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class LogModifyViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string newEntry;
        private LogItem currentLog;
        private LogDataTypes currentLogDataType;
        private ITourItemFactory tourItemFactory;
        private ICommand modifyLogCommand;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

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
                    ValidateLogDataType();
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
                    ValidateNewEntry();
                    RaisePropertyChangedEvent(nameof(NewEntry));
                }
            }
        }

        public bool HasErrors => _errorsByPropertyName.Any();

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
            if(ValidateLogDataType() && ValidateNewEntry())
            {
                this.tourItemFactory.ModifyLog(CurrentLog, CurrentLogDataType.TypeLogData, NewEntry);
                var window = (Window)commandParameter;
                window.Close();
            }
            else
            {
                ValidateLogDataType();
                ValidateNewEntry();
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

        private bool ValidateLogDataType()
        {
            ClearErrors(nameof(CurrentLogDataType));
            if (CurrentLogDataType == null)
            {
                AddError(nameof(CurrentLogDataType), "Log data cannot be empty.");
                return false;
            }
            return true;
        }

        private bool ValidateNewEntry()
        {
            ClearErrors(nameof(NewEntry));
            if(CurrentLogDataType == null)
            {
                AddError(nameof(NewEntry), "Choose a log column.");
                return false;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(NewEntry))
                {
                    AddError(nameof(NewEntry), "New Entry cannot be empty.");
                    return false;
                }
                else if (CurrentLogDataType.Display == "Date/Time")
                {
                    Regex regex = new Regex(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$");
                    if (!regex.IsMatch(NewEntry))
                    {
                        AddError(nameof(NewEntry), "Date Time Format must be DD/MM/YYYY.");
                        return false;
                    }
                    return true;
                }
                else if (CurrentLogDataType.Display == "Report")
                {
                    if (NewEntry.Length > 50)
                    {
                        AddError(nameof(NewEntry), "Report has to be lower than 50 characters.");
                        return false;
                    }
                    return true;
                }
                else if (CurrentLogDataType.Display == "Distance")
                {
                    bool res;
                    float distance;
                    res = float.TryParse(NewEntry, out distance);
                    if (!res)
                    {
                        AddError(nameof(NewEntry), "Distance has to be a float.");
                        return false;
                    }
                    else
                    {
                        if ((distance < 1) || (distance > 10000))
                        {
                            AddError(nameof(NewEntry), "Distance has to be between 1 and 10000 km.");
                            return false;
                        }
                    }
                    return true;
                }
                else if (CurrentLogDataType.Display == "Total Time")
                {
                    Regex regex = new Regex(@"^(?:[01]\d|2[0123]):(?:[012345]\d):(?:[012345]\d)$");
                    if (!regex.IsMatch(NewEntry))
                    {
                        AddError(nameof(NewEntry), "Total Time Format must be HH:MM:SS.");
                        return false;
                    }
                    return true;
                }
                else if (CurrentLogDataType.Display == "Rating")
                {
                    bool res;
                    int rating;
                    res = int.TryParse(NewEntry, out rating);
                    if (!res)
                    {
                        AddError(nameof(NewEntry), "Rating has to be an integer.");
                        return false;
                    }
                    else
                    {
                        if ((rating < 1) || (rating > 10))
                        {
                            AddError(nameof(NewEntry), "Rating has to be between 1 and 10.");
                            return false;
                        }
                    }
                    return true;
                }
                else if (CurrentLogDataType.Display == "Average Speed")
                {
                    bool res;
                    int avgSpeed;
                    res = int.TryParse(NewEntry, out avgSpeed);
                    if (!res)
                    {
                        AddError(nameof(NewEntry), "Average Speed has to be an integer.");
                        return false;
                    }
                    else
                    {
                        if ((avgSpeed < 1) || (avgSpeed > 200))
                        {
                            AddError(nameof(NewEntry), "Average Speed has to be between 1 and 200 km/h.");
                            return false;
                        }
                    }
                    return true;
                }
                else if (CurrentLogDataType.Display == "Inclination")
                {
                    bool res;
                    int inclination;
                    res = int.TryParse(NewEntry, out inclination);
                    if (!res)
                    {
                        AddError(nameof(NewEntry), "Inclination has to be an integer.");
                        return false;
                    }
                    else
                    {
                        if ((inclination < 1) || (inclination > 70))
                        {
                            AddError(nameof(NewEntry), "Inclination has to be between 1 and 70 degrees.");
                            return false;
                        }
                    }
                    return true;
                }
                else if (CurrentLogDataType.Display == "Top Speed")
                {
                    bool res;
                    int topSpeed;
                    res = int.TryParse(NewEntry, out topSpeed);
                    if (!res)
                    {
                        AddError(nameof(NewEntry), "Top Speed has to be an integer.");
                        return false;
                    }
                    else
                    {
                        if ((topSpeed < 1) || (topSpeed > 300))
                        {
                            AddError(nameof(NewEntry), "Top Speed has to be between 1 and 300 km/h.");
                            return false;
                        }
                    }
                    return true;
                }
                else if (CurrentLogDataType.Display == "Max Height")
                {
                    bool res;
                    int maxHeight;
                    res = int.TryParse(NewEntry, out maxHeight);
                    if (!res)
                    {
                        AddError(nameof(NewEntry), "Max Height has to be an integer.");
                        return false;
                    }
                    else
                    {
                        if ((maxHeight < 1) || (maxHeight > 10000))
                        {
                            AddError(nameof(NewEntry), "Max Height has to be between 1 and 10000 metres.");
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    bool res;
                    int minHeight;
                    res = int.TryParse(NewEntry, out minHeight);
                    if (!res)
                    {
                        AddError(nameof(NewEntry), "Min Height has to be an integer.");
                        return false;
                    }
                    else
                    {
                        if ((minHeight < 1) || (minHeight > 10000))
                        {
                            AddError(nameof(NewEntry), "Min Height has to be between 1 and 10000 metres.");
                            return false;
                        }
                    }
                    return true;
                }
            }
            
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
