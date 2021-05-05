using Newtonsoft.Json;
using Npgsql;
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
            connectionString = "Host=" + this.configFile.DbSettings.Host + ";Username=" + this.configFile.DbSettings.Username + ";Password=" + 
                                this.configFile.DbSettings.Password + ";Database=" + this.configFile.DbSettings.Database + ";Port=" + this.configFile.DbSettings.Port;
        }

        public void AddItem(string name, string from, string to, string imagePath, string description, string route)
        {
            int tourId = 0;
            string url = @"http://www.mapquestapi.com/directions/v2/route?key=" + key + "&from=" + from + "&to=" + to + "&routeType=" + route;

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(url);
                RequestData reqObj = JsonConvert.DeserializeObject<RequestData>(json);

                TimeSpan time = TimeSpan.FromSeconds(reqObj.route.Time);
                string str = time.ToString(@"hh\:mm\:ss");
                reqObj.route.Distance = Math.Round(reqObj.route.Distance * 1.60934, 2);

                description += "\n\nDistance: " + reqObj.route.Distance + " km" + "\nTime: " + str + "\nHas Highway: " + reqObj.route.HasHighway +
                              "\nHas TollRoad: " + reqObj.route.HasTollRoad + "\nRoute Type: "+ route +"\nFrom: " + from + "\nTo: " + to;
             
                tourId = GetTourItemIdForInsert();

                InsertTour(tourId, name, description, from, to, imagePath, route);
            }
        }

        public void InsertTour(int tourId, string name, string description, string from, string to, string imagePath, string route)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strcomm = "INSERT INTO tours (tourid, name, description, startpoint, endpoint, path, route) VALUES (@tourid, @name, @description, @startpoint, @endpoint, @path, @route)";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strcomm, conn);

            sqlcomm.Parameters.AddWithValue("tourid", tourId);
            sqlcomm.Parameters.AddWithValue("name", name);
            sqlcomm.Parameters.AddWithValue("description", description);
            sqlcomm.Parameters.AddWithValue("startpoint", from);
            sqlcomm.Parameters.AddWithValue("endpoint", to);
            sqlcomm.Parameters.AddWithValue("path", imagePath);
            sqlcomm.Parameters.AddWithValue("route", route);
            
            sqlcomm.Prepare();
            sqlcomm.ExecuteNonQuery();

            conn.Close();
        }

        public int GetTourItemIdForInsert()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strmax = "Select max(tourId) from tours";
            string strcount = "Select count(*) from tours";

            NpgsqlCommand sqlmaxcmd = new NpgsqlCommand(strmax, conn);
            NpgsqlCommand sqlountcmd = new NpgsqlCommand(strcount, conn);

            Int32 count = Convert.ToInt32(sqlountcmd.ExecuteScalar());

            if(count == 0)
            {
                conn.Close();
                return 1;
            }
            else
            {
                Int32 tourId = Convert.ToInt32(sqlmaxcmd.ExecuteScalar());
                conn.Close();
                tourId++;
                return tourId;
            }
        }

        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime, string rating, string avgSpeed, string inclination, string topSpeed, string maxHeight, string minHeight)
        {
            int logId = GetLogItemIdForInsert();
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strcomm = "INSERT INTO logs (logid, fk_tourid, datetime, report, distance, totaltime, rating, avgspeed, inclination, topspeed, maxheight, minheight) VALUES (@logid, @fk_tourid, @datetime, @report, @distance, @totaltime, @rating, @avgspeed, @inclination, @topspeed, @maxheight, @minheight)";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strcomm, conn);

            sqlcomm.Parameters.AddWithValue("logid", logId);
            sqlcomm.Parameters.AddWithValue("fk_tourid", tourid);
            sqlcomm.Parameters.AddWithValue("datetime", dateTime);
            sqlcomm.Parameters.AddWithValue("report", report);
            sqlcomm.Parameters.AddWithValue("distance", distance);
            sqlcomm.Parameters.AddWithValue("totaltime", totalTime);
            sqlcomm.Parameters.AddWithValue("rating", rating);
            sqlcomm.Parameters.AddWithValue("avgspeed", avgSpeed);
            sqlcomm.Parameters.AddWithValue("inclination", inclination);
            sqlcomm.Parameters.AddWithValue("topspeed", topSpeed);
            sqlcomm.Parameters.AddWithValue("maxheight", maxHeight);
            sqlcomm.Parameters.AddWithValue("minheight", minHeight);

            sqlcomm.Prepare();
            sqlcomm.ExecuteNonQuery();

            conn.Close();
        }

        public int GetLogItemIdForInsert()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strmax = "Select max(logId) from logs";
            string strcount = "Select count(*) from logs";

            NpgsqlCommand sqlmaxcmd = new NpgsqlCommand(strmax, conn);
            NpgsqlCommand sqlountcmd = new NpgsqlCommand(strcount, conn);

            Int32 count = Convert.ToInt32(sqlountcmd.ExecuteScalar());

            if (count == 0)
            {
                conn.Close();
                return 1;
            }
            else
            {
                Int32 logId = Convert.ToInt32(sqlmaxcmd.ExecuteScalar());
                conn.Close();
                logId++;
                return logId;
            }
        }

        public void DeleteItem(int tourid)
        {
            tourItems.RemoveAt(tourItems.FindIndex(item => item.TourId == tourid));

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string logdelete = "Delete from logs where fk_tourid = @tourid";
            NpgsqlCommand sqllogdelete = new NpgsqlCommand(logdelete, conn);
            sqllogdelete.Parameters.AddWithValue("tourid", tourid);
            sqllogdelete.Prepare();
            sqllogdelete.ExecuteNonQuery();

            string strdelete = "Delete from tours where tourid = @tourid";
            NpgsqlCommand sqldelete = new NpgsqlCommand(strdelete, conn);
            sqldelete.Parameters.AddWithValue("tourid", tourid);
            sqldelete.Prepare();
            sqldelete.ExecuteNonQuery();

            conn.Close();
        }

        public void DeleteLog(int logid)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string strdelete = "Delete from logs where logid = @logid";
            NpgsqlCommand sqldelete = new NpgsqlCommand(strdelete, conn);

            sqldelete.Parameters.AddWithValue("logid", logid);
            sqldelete.Prepare();
            sqldelete.ExecuteNonQuery();

            conn.Close();
        }

        public List<LogItem> GetLogs(int tourid)
        {
            logItems.Clear();

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string strlogs = "Select * from logs where fk_tourid = @tourid";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strlogs, conn);
            sqlcomm.Parameters.AddWithValue("tourid", tourid);
            sqlcomm.Prepare();

            NpgsqlDataReader reader = sqlcomm.ExecuteReader();
            while (reader.Read())
            {
                logItems.Add(new LogItem()
                {
                    LogId = reader.GetInt32(0),
                    TourId = reader.GetInt32(1),
                    DateTime = reader.GetString(2),
                    Report = reader.GetString(3),
                    Distance = reader.GetString(4),
                    TotalTime = reader.GetString(5),
                    Rating = reader.GetString(6),
                    AvgSpeed = reader.GetString(7),
                    Inclination = reader.GetString(8),
                    TopSpeed = reader.GetString(9),
                    MaxHeight = reader.GetString(10),
                    MinHeight = reader.GetString(11)
                });
            }
            conn.Close();

            return logItems;
        }

        public List<TourItem> GetItems()
        {
            tourItems.Clear();

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            string strtours = "Select * from tours";
            NpgsqlCommand sqlcomm = new NpgsqlCommand(strtours, conn);
            sqlcomm.Prepare();

            NpgsqlDataReader reader = sqlcomm.ExecuteReader();
            while (reader.Read())
            {
                tourItems.Add(new TourItem()
                {
                    TourId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    From = reader.GetString(3),
                    To = reader.GetString(4),
                    ImagePath = reader.GetString(5),
                    Route = reader.GetString(6)
                });
            }
            conn.Close();
            
            return tourItems;
        }

        public void ModifyTour(TourItem currentTour, string description, string route)
        {
            string url = @"http://www.mapquestapi.com/directions/v2/route?key=" + key + "&from=" + currentTour.From + "&to=" + currentTour.To + "&routeType=" + route;

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(url);
                RequestData reqObj = JsonConvert.DeserializeObject<RequestData>(json);

                TimeSpan time = TimeSpan.FromSeconds(reqObj.route.Time);
                string str = time.ToString(@"hh\:mm\:ss");
                reqObj.route.Distance = Math.Round(reqObj.route.Distance * 1.60934, 2);

                description += "\n\nDistance: " + reqObj.route.Distance + " km" + "\nTime: " + str + "\nHas Highway: " + reqObj.route.HasHighway +
                              "\nHas TollRoad: " + reqObj.route.HasTollRoad + "\nRoute Type: " + route + "\nFrom: " + currentTour.From + "\nTo: " + currentTour.To;
            }

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string updatetours = "Update tours set description = @description, route = @route where tourid = @tourid";
            NpgsqlCommand sqlupdate = new NpgsqlCommand(updatetours, conn);
            sqlupdate.Parameters.AddWithValue("description", description);
            sqlupdate.Parameters.AddWithValue("route", route);
            sqlupdate.Parameters.AddWithValue("tourid", currentTour.TourId);
            sqlupdate.Prepare();
            sqlupdate.ExecuteNonQuery();
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
