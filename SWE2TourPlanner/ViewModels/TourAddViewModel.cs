﻿using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class TourAddViewModel : ViewModelBase
    {
        private string newTourText;
        private string from;
        private string to;
        private string description;
        private RouteType currentRoute;
        private ITourItemFactory tourItemFactory;
        private ICommand addTourCommand;

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
                    RaisePropertyChangedEvent(nameof(CurrentRoute));
                }
            }
        }

        public string NewTourText
        {
            get { return newTourText; }
            set
            {
                if ((newTourText != value) && (value != null))
                {
                    newTourText = value;
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
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }

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
            this.tourItemFactory.AddItem(NewTourText, From, To, Description, CurrentRoute.TypeRoute);
            var window = (Window)commandParameter;
            NewTourText = string.Empty;
            From = string.Empty;
            To = string.Empty;
            Description = string.Empty;
            window.Close();
        }

    }
}
