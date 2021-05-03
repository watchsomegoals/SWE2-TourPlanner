using Newtonsoft.Json;
using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SWE2TourPlanner.DataAccessLayer
{
    class Database : IDataAccess
    {
        private string connectionString;
        private List<TourItem> tourItems = new List<TourItem>();
        private List<LogItem> logItems = new List<LogItem>();
        private ConfigFile configFile;
        private string key;

        public Database()
        {
            string path = "configFile.json";
            string json = File.ReadAllText(path);
            this.configFile = JsonConvert.DeserializeObject<ConfigFile>(json);
            this.key = this.configFile.FsSettings.Key;
            // get connection data e.g. from config file
            connectionString = "...";
            // establish connection with db
        }

        public void AddItem(string name, string from, string to, string imagePath)
        {
            string description = null;
            string routeType = "bicycle";
            string url = @"http://www.mapquestapi.com/directions/v2/route?key=" + key + "&from=" + from + "&to=" + to + "&routeType=" + routeType;

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(url);
                // Now parse with JSON.Net
                RequestData reqObj = JsonConvert.DeserializeObject<RequestData>(json);

                TimeSpan time = TimeSpan.FromSeconds(reqObj.route.Time);
                //here backslash is must to tell that colon is
                //not the part of format, it just a character that we want in output
                string str = time.ToString(@"hh\:mm\:ss");
                reqObj.route.Distance = Math.Round(reqObj.route.Distance * 1.60934, 2);

                description = "Distance: " + reqObj.route.Distance + " km" + "\nTime: " + str + "\nHas Highway: " + reqObj.route.HasHighway;
                tourItems.Add(new TourItem()
                {
                    Name = name,
                    Description = description,
                    From = from,
                    To = to,
                    ImagePath = imagePath
                });
            }
        }

        public void AddLog(TourItem currentTour, string dateTime, string report, string distance, string totalTime)
        {
            logItems.Add(new LogItem()
            {
                DateTime = dateTime,
                Report = report,
                Distance = distance,
                TotalTime = totalTime,
                Touritem = currentTour
            });
        }

        public void DeleteItem(string name)
        {
            tourItems.RemoveAt(tourItems.FindIndex(item => item.Name == name));
        }

        public List<LogItem> GetLogs()
        {
            return logItems;
        }

        public List<TourItem> GetItems()
        {
            // select SQL query
            return tourItems;
        }

        public string CreateImage(string from, string to, string path = "No path")
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(string path = "No path")
        {
            throw new NotImplementedException();
        }

        public void SaveImagePath(string path, string deletePath = "No path")
        {
            throw new NotImplementedException();
        }
    }
}
