using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    public enum DataType
    {
        Database,
        Filesystem
    }

    public class TourItemDAO
    {
        private IDataAccess dataAccess;

        public TourItemDAO(DataType dataType)
        {
            if (dataType == DataType.Database)
            {
                dataAccess = new Database();
            }
            else if (dataType == DataType.Filesystem)
            {
                dataAccess = new FileSystem();
            }
        }

        public string CreateImage(string from, string to)
        {
            return dataAccess.CreateImage(from, to);
        }

        public List<TourItem> GetItems()
        {
            return dataAccess.GetItems();
        }

        public void AddItem(string name, string description, string from, string to, string imagePath)
        {
            dataAccess.AddItem(name, description, from, to, imagePath);
        }

        public void DeleteItem(string name)
        {
            dataAccess.DeleteItem(name);
        }

        public void DeleteImage()
        {
            dataAccess.DeleteImage();
        }

        public void SaveImagePath(string path)
        {
            dataAccess.SaveImagePath(path);
        }
    }
}
