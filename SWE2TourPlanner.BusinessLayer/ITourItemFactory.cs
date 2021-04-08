using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.BusinessLayer
{
    public interface ITourItemFactory
    {
        IEnumerable<TourItem> GetItems();
        IEnumerable<TourItem> Search(string itemName, bool caseSensitive = false);
        public void AddItem(string name);
    }
}
