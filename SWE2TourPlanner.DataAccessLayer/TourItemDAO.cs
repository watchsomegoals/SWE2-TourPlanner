using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    public class TourItemDAO
    {
        private IDataAccess dataAccess;

        public TourItemDAO()
        {
            // check which datasource to use
            // dataAccess = new FileSystem();
            dataAccess = new Database();
        }

        public List<TourItem> GetItems()
        {
            return dataAccess.GetItems();
        }

        public void AddItem(string name)
        {
            dataAccess.AddItem(name);
        }
    }
}
