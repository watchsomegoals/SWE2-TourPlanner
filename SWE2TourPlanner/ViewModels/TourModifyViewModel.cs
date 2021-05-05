using SWE2TourPlanner.BusinessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SWE2TourPlanner.ViewModels
{
    public class TourModifyViewModel : ViewModelBase
    {
        private string description;
        private RouteType currentRoute;
        private TourItem currentTour;
        private ITourItemFactory tourItemFactory;
        private ICommand modifyTourCommand;

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
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }

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
            this.tourItemFactory.ModifyTour(CurrentTour, Description, CurrentRoute.TypeRoute);
            var window = (Window)commandParameter;
            window.Close();
        }
    }
}
