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
        public event PropertyChangedEventHandler PropertyChanged;
        private float tax;
        private float price;
        private string custOrder;
        private bool checkedSalami;
        private bool checkedProsciutto;
        private bool checkedCorn;
        private bool checkedBacon;
        private bool checkedMushrooms;
        public ICommand ExecuteCommand { get; }
        public MainViewModel()
        {
            this.ExecuteCommand = new ExecuteCommand(this);
        }

        public float Tax
        {
            get
            {
                Debug.Print("read tax percentage");
                return tax;
            }
            set
            {
                if(Tax != value)
                {
                    Debug.Print("set tax value");
                    tax = value;

                    Debug.Print("fire propertyChanged: Tax");
                    OnPropertyChanged(nameof(Tax));
                }
            }
        }

        public float Price
        {
            get
            {
                Debug.Print("read price");
                return price;
            }
            set
            {
                if (Price != value)
                {
                    Debug.Print("set price value");
                    price = value;

                    Debug.Print("fire propertyChanged: Price");
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public string CustOrder
        {
            get
            {
                Debug.Print("read customer order");
                return custOrder;
            }
            set
            {
                if(CustOrder != value)
                {
                    Debug.Print("set customer order value");
                    custOrder = value;

                    Debug.Print("fire propertyChanged: Customer Order");
                    OnPropertyChanged(nameof(CustOrder));
                }
            }
        }

        public bool IsCheckedBoxSalami
        {
            get { return checkedSalami; }
            set
            {
                if(IsCheckedBoxSalami != value)
                {
                    checkedSalami = value;
                    OnPropertyChanged(nameof(IsCheckedBoxSalami));
                }
            }
        }

        public bool IsCheckedBoxProsciutto
        {
            get { return checkedProsciutto; }
            set
            {
                if(IsCheckedBoxProsciutto != value)
                {
                    checkedProsciutto = value;
                    OnPropertyChanged(nameof(IsCheckedBoxProsciutto));
                }
            }
        }

        public bool IsCheckedBoxCorn
        {
            get { return checkedCorn; }
            set
            {
                if (IsCheckedBoxCorn != value)
                {
                    checkedCorn = value;
                    OnPropertyChanged(nameof(IsCheckedBoxCorn));
                }
            }
        }

        public bool IsCheckedBoxBacon
        {
            get { return checkedBacon; }
            set
            {
                if (IsCheckedBoxBacon != value)
                {
                    checkedBacon = value;
                    OnPropertyChanged(nameof(IsCheckedBoxBacon));
                }
            }
        }

        public bool IsCheckedBoxMushrooms
        {
            get { return checkedMushrooms; }
            set
            {
                if (IsCheckedBoxMushrooms != value)
                {
                    checkedMushrooms = value;
                    OnPropertyChanged(nameof(IsCheckedBoxMushrooms));
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
