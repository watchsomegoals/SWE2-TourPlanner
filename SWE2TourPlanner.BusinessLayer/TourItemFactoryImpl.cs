using Newtonsoft.Json;
using SWE2TourPlanner.DataAccessLayer;
using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public void AddItem(string name, string from, string to)
        {
            string imagePath = filesystemTourItemDAO.CreateImage(from, to);
            databaseTourItemDAO.AddItem(name, from, to, imagePath);
        }

        public void DeleteItemAndSavePath(int tourid, string path)
        {
            filesystemTourItemDAO.SaveImagePath(path);
            databaseTourItemDAO.DeleteItem(tourid);
        }

        public void DeleteImages()
        {
            filesystemTourItemDAO.DeleteImage();
        }

        public IEnumerable<TourItem> GetItems()
        {
            return databaseTourItemDAO.GetItems();
        }

        public IEnumerable<LogItem> GetLogs(int tourid)
        {
            return databaseTourItemDAO.GetLogs(tourid);
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

        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime)
        {
            databaseTourItemDAO.AddLog(tourid, dateTime, report, distance, totalTime);
        }
    }
}
