using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    interface IDataAccess
    {
        public List<TourItem> GetItems();
        public void AddItem(string name, string description, string from, string to, string imagePath);
        public void DeleteItem(string name);
        public string CreateImage(string from, string to);
    }
}
