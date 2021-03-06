using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    interface IDataAccess
    {
        public List<TourItem> GetItems();
        public List<LogItem> GetLogs(int tourid);
        public void AddItem(string name, string from, string to, string imagePath,string description, string route);
        public void DeleteItem(int tourid);
        public string CreateImage(string from, string to, string path = "No path");
        public void DeleteImage(string path = "No path");
        public void SaveImagePath(string path, string deletePath = "No path");
        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime, string rating, string avgSpeed, string inclination, string topSpeed, string maxHeight, string minHeight);
        public void DeleteLog(int logid);
        public void ModifyTour(TourItem currentTour, string description, string route);
        public void ModifyLog(LogItem currentLog, string typeLogData, string newEntry);
        public void CreatePdf(TourItem tourItem, List<LogItem> logItems);
        public bool DoesTourExist(int tourid);
        public bool DoesLogExist(int logid);
        public void Export(List<ExportObject> exportObjects);
        public int GetToursCount();
        public float GetDistanceSum();
        public int GetLogsCount();
        public int GetRatingSum();
        public int GetAvgSpeedSum();
        public int GetTopSpeed();
        public void CreatePdfSummary(int tourNr, int logsNr, float distanceSum, float distanceAvg, float ratingAvg, float avgSpeedAvg, int topSpeed);
    }
}
