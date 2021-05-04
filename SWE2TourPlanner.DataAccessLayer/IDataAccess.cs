﻿using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    interface IDataAccess
    {
        public List<TourItem> GetItems();
        public List<LogItem> GetLogs(int tourid);
        public void AddItem(string name, string from, string to, string imagePath);
        public void DeleteItem(int tourid);
        public string CreateImage(string from, string to, string path = "No path");
        public void DeleteImage(string path = "No path");
        public void SaveImagePath(string path, string deletePath = "No path");
        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime);
    }
}
