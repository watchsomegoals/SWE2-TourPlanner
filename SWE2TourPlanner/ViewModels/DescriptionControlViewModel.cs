using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.ViewModels
{
    public class DescriptionControlViewModel : ViewModelBase
    {
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }
    }
}
