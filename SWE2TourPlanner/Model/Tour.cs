using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SWE2TourPlanner
{
    public class Tour : INotifyPropertyChanged
    {
        private string _name;

        public Tour(string name)
        {
            this.Name = name;
        }

        public string Name
        {
            get => this._name;
            set
            {
                this._name = value;
                this.OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
