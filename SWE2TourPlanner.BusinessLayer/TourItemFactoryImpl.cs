using SWE2TourPlanner.DataAccessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWE2TourPlanner.BusinessLayer
{
    public class TourItemFactoryImpl : ITourItemFactory
    {
        private TourItemDAO databaseTourItemDAO;
        private TourItemDAO filesystemTourItemDAO;

        public TourItemFactoryImpl()
        {
            databaseTourItemDAO = new TourItemDAO(DataType.Database);
            filesystemTourItemDAO = new TourItemDAO(DataType.Filesystem);
        }

        public void AddItem(string name, string description, string from, string to)
        {
            string imagePath = filesystemTourItemDAO.CreateImage(from, to);
            databaseTourItemDAO.AddItem(name, description, from, to, imagePath);
        }

        public void DeleteItem(string name)
        {
            databaseTourItemDAO.DeleteItem(name);
        }

        public IEnumerable<TourItem> GetItems()
        {
            return databaseTourItemDAO.GetItems();
        }

        public IEnumerable<TourItem> Search(string itemName, bool caseSensitive = false)
        {
            IEnumerable<TourItem> items = GetItems();

            if(caseSensitive)
            {
                return items.Where(x => x.Name.Contains(itemName));
            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }
    }
}
