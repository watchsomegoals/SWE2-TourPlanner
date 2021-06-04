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

        public void AddItem(string name, string from, string to, string description, string route)
        {
            string imagePath = filesystemTourItemDAO.CreateImage(from, to);
            databaseTourItemDAO.AddItem(name, from, to, imagePath, description, route);
        }

        public void DeleteItemAndSavePath(int tourid, string path)
        {
            if(databaseTourItemDAO.DoesTourExist(tourid))
            {
                filesystemTourItemDAO.SaveImagePath(path);
                databaseTourItemDAO.DeleteItem(tourid);
            }
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

        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime, string rating, string avgSpeed, string inclination, string topSpeed, string maxHeight, string minHeight)
        {
            databaseTourItemDAO.AddLog(tourid, dateTime, report, distance, totalTime, rating, avgSpeed, inclination, topSpeed, maxHeight, minHeight);
        }

        public void DeleteLog(int logid)
        {
            if(databaseTourItemDAO.DoesLogExist(logid))
            {
                databaseTourItemDAO.DeleteLog(logid);
            }
        }

        public void ModifyTour(TourItem currentTour, string description, string route)
        {
            databaseTourItemDAO.ModifyTour(currentTour, description, route);
        }

        public void ModifyLog(LogItem currentLog, string typeLogData, string newEntry)
        {
            databaseTourItemDAO.ModifyLog(currentLog, typeLogData, newEntry);
        }
        public void CreatePdf(TourItem tourItem)
        {
            filesystemTourItemDAO.CreatePdf(tourItem, databaseTourItemDAO.GetLogs(tourItem.TourId));
        }

        public void CreatePdfSummary()
        {
            int tourNr = databaseTourItemDAO.GetToursCount();
            int logsNr = databaseTourItemDAO.GetLogsCount();
            int topSpeed;
            int avgSpeedSum;
            float avgSpeedAvg;
            int ratingSum;
            float ratingAvg;
            float distanceSum;
            float distanceAvg;
            if(tourNr > 0 && logsNr > 0)
            {
                distanceSum = databaseTourItemDAO.GetDistanceSum();
                distanceAvg = distanceSum / logsNr;
                ratingSum = databaseTourItemDAO.GetRatingSum();
                ratingAvg = ratingSum / logsNr;
                avgSpeedSum = databaseTourItemDAO.GetAvgSpeedSum();
                avgSpeedAvg = avgSpeedSum / logsNr;
                topSpeed = databaseTourItemDAO.GetTopSpeed();

                filesystemTourItemDAO.CreatePdfSummary(tourNr, logsNr, distanceSum, distanceAvg, ratingAvg, avgSpeedAvg, topSpeed);
            }
        }

        public void Export()
        {
            List<ExportObject> exportObjects = new List<ExportObject>();
            List<LogItem> logItems = new List<LogItem>();
            List<TourItem> tourItems = new List<TourItem>();
            tourItems = databaseTourItemDAO.GetItems();

            foreach (TourItem tour in tourItems)
            {
                ExportObject exportObject = new ExportObject() { Item = tour };
                exportObject.LogItems = new List<LogItem>();
                if(databaseTourItemDAO.GetLogs(exportObject.Item.TourId).Count != 0)
                {
                    foreach (LogItem log in databaseTourItemDAO.GetLogs(exportObject.Item.TourId))
                    {
                        if (log != null)
                        {
                            exportObject.LogItems.Add(log);
                        }
                    }
                }
                
                exportObjects.Add(exportObject);
            }

            filesystemTourItemDAO.Export(exportObjects);
        }

        public void DeleteAllToursAndLogs()
        {
            IEnumerable<TourItem> items = GetItems();
            if(items != null)
            {
                foreach (TourItem tourItem in items.ToList())
                {
                    IEnumerable<LogItem> logItems = GetLogs(tourItem.TourId);
                    foreach(LogItem logItem in logItems.ToList())
                    {
                        DeleteLog(logItem.LogId);
                    }
                    DeleteItemAndSavePath(tourItem.TourId, tourItem.ImagePath);
                }
            }
        }

        public void Import(string filePath)
        {
            DeleteAllToursAndLogs();

            string json = File.ReadAllText(filePath);
            List<ExportObject> exportObjects = JsonConvert.DeserializeObject<List<ExportObject>>(json);

            foreach(ExportObject exportObject in exportObjects)
            {
                AddItem(exportObject.Item.Name, exportObject.Item.From, exportObject.Item.To, exportObject.Item.Description, exportObject.Item.Route);
                if(exportObject.LogItems != null)
                {
                    foreach (LogItem logItem in exportObject.LogItems)
                    {
                        AddLog(exportObject.Item.TourId, logItem.DateTime, logItem.Report, logItem.Distance, logItem.TotalTime, 
                            logItem.Rating, logItem.AvgSpeed, logItem.Inclination, logItem.TopSpeed, logItem.MaxHeight, logItem.MinHeight);
                    }
                }   
            }
        }
    }
}
